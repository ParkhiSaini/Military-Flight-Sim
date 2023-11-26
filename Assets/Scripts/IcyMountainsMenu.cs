using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IcyMountainsMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public bool paused = false;

    public GameObject drone;
    public InputManager input;

    void Start()
    {
        drone = GameObject.FindGameObjectWithTag("Player");
        if (drone != null)
        {
            input = drone.GetComponent<InputManager>();
        }
    }
    void Update()
    {
        if(drone != null){
            if(input.Pause == 1.0f){
                Pause();
            }
        }
    }
    public void Pause()
    {
        paused = true;
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void RestartMission()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        paused = false;
        pauseMenu.gameObject.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1.0f;
    }
}
