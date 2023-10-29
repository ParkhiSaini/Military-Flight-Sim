using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightRestricter : MonoBehaviour
{
    public Transform dronePos;
    public GameObject fade;
    public GameObject finalWarning;
    public double positiveXLimit = 0.0;
    public double negativeXLimit = 0.0;
    public double positiveZLimit = 0.0;
    public double negativeZLimit = 0.0;
    private void HeightLimit(){
        if(dronePos != null){
            if(dronePos.position.y < 70){
                fade.SetActive(false);
            }
            if(dronePos.position.y > 70 && dronePos.position.y < 73){
            fade.SetActive(true);
            }
            else if(dronePos.position.y > 73 && dronePos.position.y < 75){
                fade.SetActive(false);
            }
            else if (dronePos.position.y > 78 && dronePos.position.y < 80){
                finalWarning.SetActive(false);
            }
            else if (dronePos.position.y > 75 && dronePos.position.y < 77){
                fade.SetActive(true);
            }
            if (dronePos.position.y > 82){
                finalWarning.SetActive(true);
            } else{
                finalWarning.SetActive(false);
            }
        }
    }

    public void mapRestrictor(){
        if(dronePos != null){
            if(dronePos.position.x > positiveXLimit){
                dronePos.position = new Vector3((float)positiveXLimit, dronePos.position.y, dronePos.position.z);
            }
            if(dronePos.position.x < negativeXLimit){
                dronePos.position = new Vector3((float)negativeXLimit, dronePos.position.y, dronePos.position.z);
            }
            if(dronePos.position.z > positiveZLimit){
                dronePos.position = new Vector3(dronePos.position.x, dronePos.position.y, (float)positiveZLimit);
            }
            if(dronePos.position.z < negativeZLimit){
                dronePos.position = new Vector3(dronePos.position.x, dronePos.position.y, (float)negativeZLimit);
            }
        }
    }

    public void Update()
    {
        HeightLimit();
        mapRestrictor();
    }
}
