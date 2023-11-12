using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    GameObject controller;

    void Awake(){
        PV = GetComponent<PhotonView>();
        
    }

    void Start(){
        if(PV.IsMine){
            CreateController();
        }
    }

    void CreateController(){
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerDrone"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
    }

    void Die(){
        PhotonNetwork.Destroy(controller);
        CreateController();
    }
}
