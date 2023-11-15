using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InputManager))]
public class DroneController : RigidBodyManager
{
    #region Variables

    [Header("Control Property")]
    [SerializeField] private float minMaxPitch = 30;
    [SerializeField] private float minMaxRoll = 30;
    [SerializeField] private float yawPower = 4;
    [SerializeField] private float lerpSpeed = 1;
    private InputManager input;
    private TutorialManager tutorial;
    private List<IEngine> _engines = new List<IEngine>();

    private float _finalPitch;
    private float _finalRoll;
    private float _finalYaw;
    private float yaw;
    [SerializeField] Transform holdArea;
    private GameObject heldObj;
    public Rigidbody heldObjRB;
    [SerializeField] private float pickupRange = 3.0f;
    public AudioSource droneSound;
    // [SerializeField] private float minPitch = 1.0f;
    // [SerializeField] private float maxPitch = 2.0f;
    // [SerializeField] private float minVolume = 0.5f;
    // [SerializeField] private float maxVolume = 1.0f;

    #endregion

    #region Main Methods

    // Start is called before the first frame update
    void Start()
    {
        tutorial = FindObjectOfType<TutorialManager>();
        input = GetComponent<InputManager>();
        _engines = GetComponentsInChildren<IEngine>().ToList();
        droneSound = GetComponent<AudioSource>();
    }

    #endregion

    #region CustomMethods

    protected override void HandlePhysics()
    {
        HandleEngines();
        HandleControls();
        HandleLoad();
        // UpdateDroneSound();
    }

    protected virtual void HandleControls()
    {
        float pitch = input.Cyclic.y * minMaxPitch;
        float roll = -input.Cyclic.x * minMaxRoll;
        yaw += input.Pedals * yawPower;

        _finalPitch = Mathf.Lerp(_finalPitch, pitch, lerpSpeed * Time.deltaTime);
        _finalRoll = Mathf.Lerp(_finalRoll, roll, lerpSpeed * Time.deltaTime);
        _finalYaw = Mathf.Lerp(_finalYaw, yaw, lerpSpeed * Time.deltaTime);
        if (SceneManager.GetActiveScene().name == "TrainingGround")
        {
            if (!tutorial.CanMoveForward())
            {
                _finalPitch = 0;
            }
            if (!tutorial.CanMoveLeftRight())
            {
                _finalRoll = 0;
            }
            if (!tutorial.CanMoveInCyclic())
            {
                _finalYaw = 0;
            }
        }
        Quaternion rot = Quaternion.Euler(_finalPitch, _finalYaw, _finalRoll);
        _rb.MoveRotation(rot);
    }

    protected virtual void HandleEngines()
    {
        foreach (IEngine engine in _engines)
        {
            engine.UpdateEngine(_rb, input);
        }
    }

    protected virtual void HandleLoad()
    {
        if (input.Loaded > 0 && heldObj == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, pickupRange))
            {
                PickUpObject(hit.transform.gameObject);
            }
        }
        else if (input.Loaded < 0 && heldObj != null)
        {
            DropObject();
        }

        if (heldObj != null)
        {
            MoveObject();
        }
    }

    void PickUpObject(GameObject pickedObj)
    {
        if (pickedObj.GetComponent<Rigidbody>() && pickedObj.tag == "Cube")
        {
            heldObjRB = pickedObj.GetComponent<Rigidbody>();
            heldObjRB.drag = 10;
            heldObjRB.useGravity = false;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
            heldObjRB.transform.parent = holdArea;
            heldObj = pickedObj;
        }
    }

    void MoveObject()
    {
        Vector3 desiredPosition = holdArea.position;
        heldObjRB.MovePosition(desiredPosition);
    }

    void DropObject()
    {
        heldObjRB.drag = 1;
        heldObjRB.useGravity = true;
        heldObjRB.constraints = RigidbodyConstraints.None;
        heldObjRB.transform.parent = null;
        heldObj = null;
    }

    // private void UpdateDroneSound()
    // {
    //     float speed = _rb.velocity.magnitude;

    //     float pitch = Mathf.Lerp(minPitch, maxPitch, speed / 10f); // You can adjust the divisor for the desired sensitivity
    //     float volume = Mathf.Lerp(minVolume, maxVolume, speed / 10f);

    //     droneSound.pitch = pitch;
        
    //     droneSound.volume = volume;
    //     Debug.Log(pitch);
    //     Debug.Log(volume);

    //     if (speed > 0.1f && !droneSound.isPlaying)
    //     {
    //         droneSound.Play();
    //     }
    //     else if (speed <= 0.1f && droneSound.isPlaying)
    //     {
    //         droneSound.Stop();
    //     }
    // }

    #endregion
}
