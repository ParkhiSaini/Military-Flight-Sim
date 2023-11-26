using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    
    public ParticleSystem groundexplosion;
    public AudioSource explosionSound;

    void Start()
    {
        groundexplosion = GetComponentInChildren<ParticleSystem>();
        explosionSound = GetComponent<AudioSource>();
        groundexplosion.Stop();
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Terrain")
        {
            
            groundexplosion.Play();
            explosionSound.Play();
            Destroy(gameObject,2);
            
        }
    
    }
}
