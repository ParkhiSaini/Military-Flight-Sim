using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    
    public ExposionTrigger trigger;

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Terrain")
        {
            
            trigger.Exploded();
            Destroy(gameObject,3);
            
        }
    
    }
}
