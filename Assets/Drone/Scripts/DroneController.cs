using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Cinemachine;

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
    public CinemachineVirtualCamera FPSCam;

    public AudioSource droneSound;
    #endregion

    #region Main Methods

    // Start is called before the first frame update
    void Start()
    {
        tutorial = FindObjectOfType<TutorialManager>();
        input = GetComponent<InputManager>();
        _engines = GetComponentsInChildren<IEngine>().ToList();
    }

    #endregion

    #region CustomMethods

    protected override void HandlePhysics()
    {
        HandleEngines();
        HandleControls();
        HandleLoad();
        UpdateDroneSound();
    }

    void Update(){
        switchCamera();
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

    void switchCamera(){
        if (input.CamSwitch > 0){
            if (FPSCam != null){
                FPSCam.gameObject.SetActive(!FPSCam.gameObject.activeSelf);
            }   
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

    private void UpdateDroneSound()
    {
        droneSound.pitch = 1 + (_rb.velocity.magnitude / 100f);
    }

    #endregion
}
