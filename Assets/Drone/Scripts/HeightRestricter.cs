using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightRestricter : MonoBehaviour
{
    public Transform dronePos;
    public GameObject fade;
    public GameObject finalWarning;

    private void HeightLimit(){
        if(dronePos != null){
            if(dronePos.position.y < 65){
                fade.SetActive(false);
            }
            if(dronePos.position.y > 65 && dronePos.position.y < 67){
            fade.SetActive(true);
            }
            else if(dronePos.position.y > 67 && dronePos.position.y < 69){
                fade.SetActive(false);
            }
            else if (dronePos.position.y > 71 && dronePos.position.y < 74){
                finalWarning.SetActive(false);
            }
            else if (dronePos.position.y > 69 && dronePos.position.y < 71){
                fade.SetActive(true);
            }
            if (dronePos.position.y > 74){
                finalWarning.SetActive(true);
            } else{
                finalWarning.SetActive(false);
            }
        }
    }

    public void Update()
    {
        HeightLimit();
    }
}
