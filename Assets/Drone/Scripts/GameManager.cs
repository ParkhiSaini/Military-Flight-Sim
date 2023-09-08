using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Mission> missions = new List<Mission>();
    public int currentLevel = 0;
    public delegate void MissionEventHandler(Mission mission);
    public event MissionEventHandler OnMissionStarted;
    public event MissionEventHandler OnMissionCompleted;

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
        SceneManager.LoadScene(levelIndex);
        if(levelIndex != 0){
            InitializeMissionsForLevel(levelIndex);
        }
    }

    private void InitializeMissionsForLevel(int levelIndex)
    {
        missions.Clear();
        switch (levelIndex)
        {
            case 0:
                missions.Add(new Mission("Collect 10 coins", 10));
                Debug.Log("Added mission");
                break;
            case 1:
                missions.Add(new Mission("Defeat 5 enemies", 5));
                missions.Add(new Mission("Reach the exit", 1));
                break;
        }
        foreach (Mission mission in missions)
        {
            OnMissionStarted?.Invoke(mission);
        }
    }

    public void CompleteMission(Mission mission)
    {
        mission.Complete();
        OnMissionCompleted?.Invoke(mission);
        if (MissionsForLevelCompleted())
        {
            Debug.Log("All missions for this level completed!");
        }
    }

    private bool MissionsForLevelCompleted()
    {
        foreach (Mission mission in missions)
        {
            if (!mission.IsCompleted)
                return false;
        }
        return true;
    }

    public void OnTutEnd(){
        Debug.Log("Tutorial ended");
        InitializeMissionsForLevel(0);
    }
}
