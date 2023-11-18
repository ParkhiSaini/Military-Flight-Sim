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
    public AudioSource buttonClick;
    public GameObject FPSCam;


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


    public void Update()
    {
        if (input.CamSwitch== 1.0f)
        {
            FPSCam.gameObject.SetActive(true);
        
        }
    }

    private void Start()
    {
        LoadLevel(currentLevel);
    }

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

   









}
