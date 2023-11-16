using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSound : MonoBehaviour
{
    public AudioSource droneSound;
    public AudioSource landingSound;

    void Start(){
        droneSound = GetComponentInChildren<AudioSource>();
        landingSound = GetComponentsInChildren<AudioSource>()[1];
    }


    private void OnTriggerExit(){
        droneSound.Play();
    }

    private void OnTriggerEnter(Collider collision)
    {
        droneSound.Stop();
        landingSound.Play();
    }
}
