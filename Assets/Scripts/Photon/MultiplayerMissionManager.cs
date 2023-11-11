using System.Collections;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class MultiplayerMissionManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("GameObjects")]
    public TextMeshProUGUI score;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI takeoffText;
    public TextMeshProUGUI timerText;
    public GameObject MissionCompleted;
    public GameObject MissionFailed;
    public GameObject PausePanel;
    public InputManager input;

    [Header("Variables")]
    private float countdownTime = 5.0f;
    [SerializeField] public bool countdownActive = false;
    public BeginnerMission beginnerMission;
    public GameObject[] rings;
    public GameObject dronePrefab; // Prefab of the drone
    public GameObject landingPadPrefab; // Prefab of the landing pad

    private GameObject localDrone; // Reference to the local player's drone
    private GameObject[] drones; // Array of all drones in the game
    private Transform grandChildTransform;
    private bool paused = false;

    // Timer variables
    private bool missionStarted = false;
    private float missionStartTime;
    private float missionDuration;

    void Start()
    {
        if (photonView.IsMine)
        {
            drones = GameObject.FindGameObjectsWithTag("Player");
            //getting photon local player instance
            localDrone = PhotonView.Find((int)photonView.InstantiationData[0]).gameObject;
            StartCoroutine(StartCountdown());
        }
    }

    public void InitializeReferences()
    {
        if (photonView.IsMine)
        {
            countdownActive = true;
            Transform childTransform = localDrone.transform.GetChild(1);
            grandChildTransform = childTransform.GetChild(2);
            foreach (GameObject ring in rings)
            {
                ring.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            UpdateScore();
            UpdateTimer(); // Continuously update the timer display
            if (countdownActive)
            {
                localDrone.GetComponent<Rigidbody>().velocity = Vector3.zero;
                localDrone.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
            if (input.Pause == 1.0f)
            {
                Pause();
            }

            MissionEnded();
        }
    }

    [PunRPC]
    void UpdateScore(int newScore)
    {
        beginnerMission.hoopsScore = newScore;
        score.text = beginnerMission.hoopsScore.ToString();
    }

    [PunRPC]
    void SyncMissionTime(float startTime)
    {
        missionStarted = true;
        missionStartTime = startTime;
    }

    [PunRPC]
    void FinishMission(bool success)
    {
        if (success)
        {
            Debug.Log("Mission Completed");
            Time.timeScale = 0;
            MissionCompleted.SetActive(true);

            // Calculate mission duration if the mission has started.
            if (missionStarted)
            {
                missionDuration = Time.time - missionStartTime;
                Debug.Log("Mission Duration: " + missionDuration + " seconds");
            }
        }
        else
        {
            MissionFailed.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("Mission Failed");
        }
    }

    private void UpdateScore()
    {
        photonView.RPC("UpdateScore", RpcTarget.All, beginnerMission.hoopsScore);
    }

    private void UpdateTimer()
    {
        if (missionStarted)
        {
            float currentTime = Time.time - missionStartTime;
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

                // Start recording the mission time when the countdown is over.
                missionStarted = true;
                missionStartTime = Time.time;

                // Synchronize mission time across the network
                photonView.RPC("SyncMissionTime", RpcTarget.All, missionStartTime);
            }
        }
        countdownActive = false;
    }

    public void MissionEnded()
    {
        foreach (GameObject drone in drones)
        {
            if (Vector3.Distance(drone.transform.position, landingPadPrefab.transform.position) < 1.0f &&
                beginnerMission.hoopsScore >= 2)
            {
                Debug.Log("Mission Completed");

                // Finish the mission on the master client
                if (photonView.IsMine)
                {
                    photonView.RPC("FinishMission", RpcTarget.All, true);
                }
            }
            else if (Vector3.Distance(drone.transform.position, landingPadPrefab.transform.position) < 1.0f &&
                     beginnerMission.hoopsScore < 2)
            {
                // Finish the mission on the master client
                if (photonView.IsMine)
                {
                    photonView.RPC("FinishMission", RpcTarget.All, false);
                }
            }
        }
    }

    public void Pause()
    {
        paused = true;
        PausePanel.gameObject.SetActive(true);
    }

}
