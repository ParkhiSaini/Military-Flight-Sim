using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MultiMissionManager : MonoBehaviourPunCallbacks
{
    [Header("GameObjects")]
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI takeoffText;
    public TextMeshProUGUI timerText;
    public GameObject drone;
    public GameObject landingPad1;
    public GameObject landingPad2;
    public GameObject MissionCompleted;
    public GameObject PausePanel;
    public InputManager input;

    [Header("Variables")]
    private float countdownTime = 5.0f;
    [SerializeField] public bool countdownActive = false;
    public bool paused =false;

    // Timer variables
    private bool missionStarted = false;
    private float missionStartTime;
    private float missionDuration;

    [Header("UI")]
    public TMP_Text hoops;
    public TMP_Text timeTaken;
    public TMP_Text position;

    void Start()
    {
        countdownActive = true;
        StartCoroutine(StartCountdown());
    }

    
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

    private void Update()
    {
        if(drone == null)
        {
            StartMission();
        }
        UpdateTimer();
        if (countdownActive && drone!=null)
        {
            drone.GetComponent<MultiplayerDroneController>().enabled = false;
        }else if (!countdownActive && drone!=null){
            drone.GetComponent<MultiplayerDroneController>().enabled = true;
        }
        if(input.Pause == 1.0f && input != null)
        {
            Pause();
        }
        MissionEnded();
    }

    private void UpdateTimer()
    {
        if (missionStarted)
        {
            float currentTime = Time.timeSinceLevelLoad - missionStartTime;
            string minutes = Mathf.Floor(currentTime / 60).ToString("00");
            string seconds = (currentTime % 60).ToString("00");
            timerText.text = "Time: " + minutes + ":" + seconds;
        }
    }

    public IEnumerator StartCountdown()
    {
        float currentTime = countdownTime;
        while (currentTime > 0)
        {
            countdownText.text = Mathf.Ceil(currentTime).ToString();
            yield return new WaitForSeconds(1.0f);
            currentTime -= 1.0f;
            if (currentTime == 0)
            {
                takeoffText.gameObject.SetActive(true);
                countdownText.gameObject.SetActive(false);
                yield return new WaitForSeconds(1.0f);
                takeoffText.gameObject.SetActive(false);
                missionStarted = true;
                missionStartTime = Time.time;
            }
        }
        countdownActive = false;
    }

    public void MissionEnded()
    {
        if(PhotonNetwork.IsMasterClient){
            if (Vector3.Distance(drone.transform.position, landingPad1.transform.position) < 1.0f)
            {
                Time.timeScale = 0;
                MissionCompleted.SetActive(true);

                if (missionStarted)
                {
                    missionDuration = Time.timeSinceLevelLoad - missionStartTime;
                }
                missionDuration = (float)System.Math.Round(missionDuration,2);
                hoops.text =drone.GetComponent<ScoreSetter>().hoopsScore.ToString();
                timeTaken.text = missionDuration.ToString() + "seconds";
            } 
        } else{
            if (Vector3.Distance(drone.transform.position, landingPad2.transform.position) < 1.0f)
            {
                Time.timeScale = 0;
                MissionCompleted.SetActive(true);

                if (missionStarted)
                {
                    missionDuration = Time.timeSinceLevelLoad - missionStartTime;
                }
                missionDuration = (float)System.Math.Round(missionDuration,2);
                hoops.text =drone.GetComponent<ScoreSetter>().hoopsScore.ToString();
                timeTaken.text = missionDuration.ToString() + "seconds";
                
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
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("RoomMenu");
    }

    public void MainMenu()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }

}
