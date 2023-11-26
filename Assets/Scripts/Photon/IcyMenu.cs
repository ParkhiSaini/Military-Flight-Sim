using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class IcyMenu : MonoBehaviourPunCallbacks
{

    public GameObject PausePanel;

    public bool paused = false;

    public GameObject drone;
    public InputManager input;

    public void StartMission()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                drone = player;
            }
        }
        if (drone != null)
        {
            input = drone.GetComponent<InputManager>();
        }
    }
    void Update()
    {
        if (drone == null){
            StartMission();
        }
        if(drone != null){
            if(input.Pause == 1.0f){
                Pause();
            }
        }
    }
    public void Pause()
    {
        paused = true;
        PausePanel.SetActive(true);
    }

    public void Resume()
    {
        paused = false;
        PausePanel.SetActive(false);
    }

    public void MultiplayerMenu()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.Disconnect();
    }

    //onleftroom
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("RoomMenu");
    }

    public void MainMenu()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        if(!PhotonNetwork.InRoom){
            SceneManager.LoadScene("MainMenu");
        }
    }
}
