using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public float maxHealth =100.0f;
    public float currentHealth;

    public HealthBar healthBar;
    public B1MissionManager missionManager;
    public IntermediateMissionManager intermediateMissionManager;

    private void Start()
    {
        currentHealth=maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public  void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            Debug.Log("Collided");
            float collisionImpact = collision.relativeVelocity.magnitude;
            float healthDepletion  = collisionImpact  *1.5f;
            currentHealth -= healthDepletion;
            currentHealth = Mathf.Max(currentHealth, 0 );
            healthBar.SetHealth(currentHealth);

            if (currentHealth<=0)
            {
                DestroyDrone();
            }
        }
    }

    private void DestroyDrone()
    {
        Destroy(gameObject);
        if(missionManager!=null){
            missionManager.OnHealthEnded();
        } else{
            intermediateMissionManager.OnHealthEnded();
        }
        Debug.Log("Drone is destroyed");
    }
}
