using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class MultiRigidBodyManager : MonoBehaviour
{
    #region Variables

    [Header("RigidBody Properties")]
    [SerializeField] private float weight = 1f;

    protected
        Rigidbody _rb;
    protected float startDrag;
    protected float startAngularDrag;

    #endregion

    #region MainMethod

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Awake rigid");
        _rb = GetComponent<Rigidbody>();
        if (_rb)
        {
            _rb.mass = weight;
            startDrag = _rb.drag;
            startAngularDrag = _rb.angularDrag;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_rb)
        {
            return;
        }
        
        HandlePhysics();
    }
    #endregion

    #region Custom Methods

    protected virtual void HandlePhysics() { }
    #endregion
}
