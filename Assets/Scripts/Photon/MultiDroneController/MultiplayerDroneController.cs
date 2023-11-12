using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Photon.Pun;

[RequireComponent(typeof(InputManager))]
public class MultiplayerDroneController : MultiRigidBodyManager
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
    // [SerializeField] private float pickupForce = 150.0f;
    private AudioSource droneSound;
    CameraHandler cameraHandler;
    PhotonView photonView;


    #endregion

    #region Main Methods

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        cameraHandler = FindObjectOfType<CameraHandler>();
        tutorial = FindObjectOfType<TutorialManager>();
        input = GetComponent<InputManager>();
        _engines = GetComponentsInChildren<IEngine>().ToList();
        cameraHandler.SetCameraTarget();
        // droneSound = gameObject.transform.Find("DroneSound").GetComponent<AudioSource>();    
    }

    #endregion

    #region CustomMethods

    protected override void HandlePhysics()
    {
        if(photonView.IsMine){
            HandleEngines();
            HandleControls();
            HandleLoad();
        } else{
            return;
        }
        // DroneSound();
    }

    protected virtual void HandleControls()
    {
        float pitch = input.Cyclic.y * minMaxPitch;
        float roll = -input.Cyclic.x * minMaxRoll;
        yaw += input.Pedals * yawPower;

        _finalPitch = Mathf.Lerp(_finalPitch, pitch, lerpSpeed * Time.deltaTime);
        _finalRoll = Mathf.Lerp(_finalRoll, roll, lerpSpeed * Time.deltaTime);
        _finalYaw = Mathf.Lerp(_finalYaw, yaw, lerpSpeed * Time.deltaTime);
        if (SceneManager.GetActiveScene().name == "TrainingGround"){
            if(!tutorial.CanMoveForward()){
                _finalPitch = 0;
            }
            if(!tutorial.CanMoveLeftRight()){
                _finalRoll = 0;
            }
            if(!tutorial.CanMoveInCyclic()){
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

    protected virtual void HandleLoad(){
        if(input.Loaded > 0 && heldObj == null){
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, pickupRange)){
                PickUpObject(hit.transform.gameObject);
            }
        }
        else if (input.Loaded < 0 && heldObj != null){
            DropObject();
        }

        if(heldObj!=null){
            MoveObject();
        }
    }

    void PickUpObject(GameObject pickedObj){
        Debug.Log("Pid Object");
        if(pickedObj.GetComponent<Rigidbody>() && pickedObj.tag == "Cube"){
            Debug.Log("Picked Object");
            heldObjRB =  pickedObj.GetComponent<Rigidbody>();
            heldObjRB.drag = 10;
            heldObjRB.useGravity = false;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
            heldObjRB.transform.parent = holdArea;
            heldObj = pickedObj;
        }
    }

    void MoveObject(){
        // if(Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f){
        //     Vector3 moveDir = (holdArea.position - heldObj.transform.position);
        //     heldObjRB.AddForce(moveDir * pickupForce);
        // }
        Vector3 desiredPosition = holdArea.position;
        heldObjRB.MovePosition(desiredPosition);
    }

    void DropObject(){
        Debug.Log("Drop Object");
        heldObjRB.drag = 1;
        heldObjRB.useGravity = true;
        heldObjRB.constraints= RigidbodyConstraints.None;
        heldObjRB.transform.parent = null;
        heldObj = null;
    }

    // void DroneSound(){
    //     droneSound.pitch = 1 + (_rb.velocity.magnitude/100);
    //     if(_finalPitch != 0 || _finalRoll != 0){
    //         droneSound.volume = 0.5f;
    //     }
    //     else{
    //         droneSound.volume = 0.1f;
    //     }
    // }

    #endregion

}
