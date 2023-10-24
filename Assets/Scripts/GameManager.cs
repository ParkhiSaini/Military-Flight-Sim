using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentLevel = 1;
    public B1MissionManager missionManager;
    public AudioSource bgMusic;
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
            case 2:
                B1MissionManager b1MissionManager;
                b1MissionManager = GameObject.Find("MissionManager").GetComponent<B1MissionManager>();
                b1MissionManager.InitializeReferences();
                StartCoroutine(b1MissionManager.StartCountdown());
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

    // public void OnButtonSound()
    // {
    //     buttonSound.PlayOneShot(buttonSound.clip);
    // }

    

    
}
