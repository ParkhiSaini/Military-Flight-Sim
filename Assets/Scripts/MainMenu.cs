using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MainMenu : MonoBehaviour
{
    
    public AudioSource buttonClick;
    public Button droneBtn;
    public Button flightBtn;
    public Button droneExitBtn;
    public Button flightExitBtn;
    public GameObject droneMenu;
    public GameObject flightMenu;

    void Start()
    {
        
    }


    void Update()
    {
    }

    public void PlayButton(){
        buttonClick.Play();
    }

    public void selectedMenu(){
        if(EventSystem.current.currentSelectedGameObject == droneBtn.gameObject){
            droneMenu.SetActive(true);
            flightMenu.SetActive(false);
        }
        else if(EventSystem.current.currentSelectedGameObject == flightBtn.gameObject){
            flightMenu.SetActive(true);
            droneMenu.SetActive(false);
        }
    }

    public void exitMenu(){
        Application.Quit();
    }

    public void StartDrone(){
       UnityEngine.SceneManagement.SceneManager.LoadScene("TrainingGround");
    }

    public void MultiplayerMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("RoomMenu");
    }

}
