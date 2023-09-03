using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    [SerializeField] private float pickupRange = 10.0f;
    [SerializeField] private float pickupForce = 150.0f;

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
    }

    protected virtual void HandleControls()
    {
        float pitch = input.Cyclic.y * minMaxPitch;
        float roll = -input.Cyclic.x * minMaxRoll;
        yaw += input.Pedals * yawPower;

        _finalPitch = Mathf.Lerp(_finalPitch, pitch, lerpSpeed * Time.deltaTime);
        _finalRoll = Mathf.Lerp(_finalRoll, roll, lerpSpeed * Time.deltaTime);
        _finalYaw = Mathf.Lerp(_finalYaw, yaw, lerpSpeed * Time.deltaTime);
        if(!tutorial.CanMoveForward()){
            _finalPitch = 0;
        }
        if(!tutorial.CanMoveLeftRight()){
            _finalRoll = 0;
        }
        if(!tutorial.CanMoveInCyclic()){
            _finalYaw = 0;
        }
    
        //pitch = move Forward
        //finalRoll = move left/right
        //yaw = rotate right/left
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
        if(pickedObj.GetComponent<Rigidbody>()){
            Debug.Log("Picked Object");
            heldObjRB =  pickedObj.GetComponent<Rigidbody>();
            heldObjRB.drag = 05;
            heldObjRB.constraints= RigidbodyConstraints.FreezeRotation;
            heldObjRB.transform.parent = holdArea;
            heldObj = pickedObj;
        }
    }

    void MoveObject(){
        if(Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f){
            Vector3 moveDir = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDir * pickupForce);
        }
    }

    void DropObject(){
        heldObjRB.drag = 1;
        heldObjRB.constraints= RigidbodyConstraints.None;
        heldObjRB.transform.parent = null;
        heldObj = null;
    }

    #endregion

}
