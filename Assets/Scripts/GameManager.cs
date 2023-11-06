using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentLevel = 1;
    public B1MissionManager missionManager;
    public InputManager input;
    public IntermediateMissionManager intermediateMissionManager;
    public AudioSource bgMusic;
    public bool fpscam=false;
    // public AudioSource buttonSound;

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
        // input = GameObject.Find("CargoDrone").GetComponent<InputManager>();
    }

    private void Update()
    {
        if (input.CamSwitch==1.0f && fpscam==false )
        {
            GameObject.Find("FPSCamera").SetActive(true);
        }
        else if(input.CamSwitch==1.0f && fpscam==true)
        {
            GameObject.Find("FPSCamera").SetActive(false);
        }
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
