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
    public static GameObject LocalPlayerInstance;

    void Awake(){
        PV = GetComponent<PhotonView>();
        if(PV.IsMine){
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
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
}
