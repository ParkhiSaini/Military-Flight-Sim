using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentLevel = 1;
    public B1MissionManager missionManager;

    public IntermediateMissionManager intermediateMissionManager;
    public AudioSource bgMusic;

    public GameObject pauseMenu;
    public InputManager input;
    public bool paused= false;
    public AudioSource buttonClick;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
        LoadLevel(currentLevel);
      

    }

    // private void Update()
    // {
    //     // if (input.Pause == 1.0f)
    //     // {
    //     //     PauseMenu();

    //     // }
    // }

    public void PlayButton()
    {
        buttonClick.Play();
    }

    public void LoadLevel(int levelIndex)
    {
        var loadLevel = SceneManager.LoadSceneAsync(levelIndex);
        loadLevel.completed += (x) => {
        InitializeMissionsForLevel(levelIndex);
        };
    }

    private void InitializeMissionsForLevel(int levelIndex)
    {
        switch (levelIndex)
        {
            
            case 4:
                B1MissionManager b1MissionManager;
                b1MissionManager = GameObject.Find("MissionManager").GetComponent<B1MissionManager>();
                b1MissionManager.InitializeReferences();
                StartCoroutine(b1MissionManager.StartCountdown());
                break;
            case 5:
                IntermediateMissionManager intermediateMissionManager;
                intermediateMissionManager = GameObject.Find("MissionManager").GetComponent<IntermediateMissionManager>();
                intermediateMissionManager.InitializeReferences();
                StartCoroutine(intermediateMissionManager.StartCountdown());
                break;
        }
    }

    private void PauseMusic()
    {
        if (missionManager.paused)
        {
            bgMusic.Play();
        }
    }

    // private void PauseMenu()
    // {
    //     paused=true;
    //     pauseMenu.gameObject.SetActive(true);
    // }

    public void RestartMission()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;

    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1.0f;

    }

    public void LoadNextLevel(int level)
    {
        SceneManager.LoadScene("BeginnerLevel");
    }









}
