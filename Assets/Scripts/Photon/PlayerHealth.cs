using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;

    public HealthBar healthBar;
    PhotonView PV;
    bool healthSet = false;
    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        if(healthBar == null){
            SetHealthBar();
        }
    }

    private void SetHealthBar(){
        if(PV.IsMine){
            enabled = true;
            healthBar = transform.GetChild(3).GetComponentInChildren<HealthBar>();
            currentHealth=maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        } else {
            enabled = false;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain")){
            float collisionImpact = collision.relativeVelocity.magnitude;

            float healthDepletion  = collisionImpact  *0.1f;

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
        if(PV.IsMine){
            PhotonNetwork.Destroy(gameObject);
        }
    }
}

