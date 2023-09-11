using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rings : MonoBehaviour
{
    public B1MissionManager b1MissionManager;
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(b1MissionManager.countdownActive){
            gameObject.SetActive(true);
        }
    }
}
