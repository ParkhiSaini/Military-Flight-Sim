using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    public Transform cameraAnchorPoint;
    CinemachineVirtualCamera cinemachineVirtualCamera;
    bool alreadySet = false;

    void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraAnchorPoint != null && alreadySet == false){
            SetUpCamera();
        }
    }

    public void SetCameraTarget(){
        cameraAnchorPoint = GameObject.Find("Target").transform;
    }

    public void SetUpCamera(){
        cinemachineVirtualCamera.LookAt = cameraAnchorPoint;
        cinemachineVirtualCamera.Follow = cameraAnchorPoint;
        alreadySet = true;
    }
}
