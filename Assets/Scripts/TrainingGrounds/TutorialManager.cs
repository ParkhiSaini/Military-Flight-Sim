using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    #region Variables

    [Header("Tutorial PopUps")]
    public GameObject[] popUps;
    private int popUpIndex;
    public InputManager input;
    [SerializeField] private bool canMoveForward;
    [SerializeField] private bool canMoveInCyclic;
    [SerializeField] private bool canMoveLeftRight;
    private GameManager gameManager;
    public GameObject pauseMenu;
    public GameObject WinningPanel;
 


    #endregion

    #region Main Methods

    private void Start(){
        gameManager = GameManager.instance;
        input = GameObject.Find("CargoDrone").GetComponent<InputManager>();
    }
    
    private void Update()
    {
        ManagePopUps();
        if (input.Pause == 1.0f)
        {
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
   
       
    }

    public void ManagePopUps(){
        for (int i = 0; i < popUps.Length; i++)
        {
            popUps[i].SetActive(i == popUpIndex);
        }

        switch (popUpIndex)
        {
            case 0:
                if (Mathf.Abs(input.Throttle) > 0.001f)
                {
                    popUpIndex++;
                    canMoveForward = true;
                    

                }
                break;

            case 1:
                if (canMoveForward && Mathf.Abs(input.Cyclic.y) > 0.001f)
                {
                    popUpIndex++;
                    canMoveInCyclic = true;
                }
                break;

            case 2:
                if (canMoveInCyclic && Mathf.Abs(input.Pedals) > 0.001f)
                {
                    popUpIndex++;
                    canMoveLeftRight = true;
                }
                break;

            case 3:
                if (canMoveLeftRight && Mathf.Abs(input.Cyclic.x) > 0.001f)
                {
                    popUpIndex++;
                }
                break;
            case 4:
                if (popUpIndex == 4 && input.SkipTutorial == 1.0f){
                    popUpIndex++;
                    WinningPanel.gameObject.SetActive(true);
                    Time.timeScale=0f;
                    
                }
                break;
        }
    }

    #endregion

    #region getters
    public bool CanMoveForward()
    {
        return canMoveForward;
    }

    public bool CanMoveInCyclic()
    {
        return canMoveInCyclic;
    }

    public bool CanMoveLeftRight()
    {
        return canMoveLeftRight;
    }


    public void RestartMission()
    {
        ResumeMission();
        SceneManager.LoadScene("TrainingGround");

    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale=1.0f;
    }

    public void ResumeMission()
    {
        Debug.Log("Resuming mission");
        Time.timeScale = 1.0f;
        pauseMenu.gameObject.SetActive(false);
        Debug.Log("Resumed mission");
    }


    public void LoadNextLevel()
    {
        gameManager.LoadLevel(4);
        Time.timeScale = 1.0f;
        WinningPanel.gameObject.SetActive(false);
    }

    #endregion
}
