using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class B1MissionManager : MonoBehaviour
{
    [Header("GameObjects")]
    public TextMeshProUGUI score;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI takeoffText;
    public TextMeshProUGUI timerText;
    public GameObject drone;
    public GameObject landingPad;
    public GameObject MissionCompleted;
    public GameObject MissionFailed;
    public GameObject PausePanel;
    public InputManager input;

    [Header("Variables")]
    private float countdownTime = 5.0f;
    [SerializeField] public bool countdownActive = false;
    public BeginnerMission beginnerMission;
    public GameObject[] rings;
    private Transform grandChildTransform;
    public bool paused =false;

    // Timer variables
    private bool missionStarted = false;
    private float missionStartTime;
    private float missionDuration;

    void Start()
    {
        input = GameObject.Find("CargoDrone").GetComponent<InputManager>();
    }

    public void InitializeReferences()
    {
        countdownActive = true;
        // countdownAnimation = score.GetComponent<Animator>();
        Transform childTransform = drone.transform.GetChild(1);
        grandChildTransform = childTransform.GetChild(2);
        foreach (GameObject ring in rings)
        {
            ring.SetActive(true);
            landingPad.SetActive(true);
        }
    }

    private void Update()
    {
        UpdateScore();
        UpdateTimer(); // Continuously update the timer display
        if (countdownActive)
        {
            drone.GetComponent<Rigidbody>().velocity = Vector3.zero;
            drone.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        if (input.Pause == 1.0f)
        {
            Pause();

        }

        MissionEnded();
    }

    private void UpdateScore()
    {
        score.text = beginnerMission.hoopsScore.ToString();
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
            }
        }
        countdownActive = false;
    }

    public void MissionEnded()
    {
        if (Vector3.Distance(drone.transform.position, landingPad.transform.position) < 1.0f && beginnerMission.hoopsScore >= 2)
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
        else if (Vector3.Distance(drone.transform.position, landingPad.transform.position) < 1.0f && beginnerMission.hoopsScore < 2)
        {
            MissionFailed.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("Mission Failed");
        }
    }

    public void Pause()
    {
        paused = true;
        PausePanel.gameObject.SetActive(true);
    }
}
