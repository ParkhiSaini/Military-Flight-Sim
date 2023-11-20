using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntermediateMissionManager : MonoBehaviour
{
    
    [Header("UI")]
    public TextMeshProUGUI score;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI takeoffText;
    public TextMeshProUGUI timerText;
    public TMP_Text winhoopScore;
    public TMP_Text winTime;
    public TMP_Text titleEarnedWin;
    public TMP_Text losehoopScore;
    public TMP_Text loseTime;


    [Header("GameObjects")]
    public GameObject[] objectToBePicked;
    public GameObject drone;
    public GameObject[] landingPads;
    public GameObject selectedLandingPad;
    public GameObject MissionCompleted;
    public GameObject MissionFailed;
    public GameObject PausePanel;
    public GameObject[] routes;
    public GameObject selectedRoute;
    public InputManager input;
    public GameObject[] pickupObjects;
    public GameObject selectedPickupObject;
    public GameObject instructions;
    public GameObject HealthBar;

    [Header("Variables")]
    private float countdownTime = 5.0f;
    [SerializeField] public bool countdownActive = false;
    public BeginnerMission beginnerMission;
    
    
    public bool paused =false;
    
    // Timer variables
    private bool missionStarted = false;
    int routeIndex;
    private float missionStartTime;
    private float missionDuration;

    [Header("Transforms")]
    public Transform spawnpoint;
    private Transform grandChildTransform;

    void Start()
    {
        input = GameObject.Find("CargoDrone").GetComponent<InputManager>();
    }

    public void InitializeReferences()
    {
        countdownActive = true;
        Transform childTransform = drone.transform.GetChild(1);
        grandChildTransform = childTransform.GetChild(2);
        routeIndex = 0;
        selectedRoute = routes[routeIndex];
        selectedRoute.SetActive(true);
        selectedLandingPad = landingPads[routeIndex];
        selectedLandingPad.SetActive(true);
        selectedPickupObject = pickupObjects[routeIndex];
        selectedPickupObject.SetActive(true);
        objectToBePicked[routeIndex].SetActive(true);
    }

    private void Update()
    {
        UpdateScore();
        UpdateTimer();
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
                instructions.gameObject.SetActive(false);
                takeoffText.gameObject.SetActive(true);
                countdownText.gameObject.SetActive(false);
                yield return new WaitForSeconds(1.0f);
                takeoffText.gameObject.SetActive(false);

                HealthBar.gameObject.SetActive(true);
                missionStarted = true;
                missionStartTime = Time.time;
            }
        }
        countdownActive = false;
    }

    public void CompletedRoute()
    {
        routeIndex += 1;
        if (routeIndex < routes.Length)
        {
            selectedRoute.SetActive(false);
            selectedLandingPad.SetActive(false);
            selectedPickupObject.SetActive(false);
            objectToBePicked[routeIndex - 1].SetActive(false);
            selectedRoute = routes[routeIndex];
            selectedRoute.SetActive(true);
            selectedLandingPad = landingPads[routeIndex];
            selectedLandingPad.SetActive(true);
            selectedPickupObject = pickupObjects[routeIndex];
            selectedPickupObject.SetActive(true);
            objectToBePicked[routeIndex].SetActive(true);
        }
        drone.transform.position = spawnpoint.position;
        drone.transform.rotation = spawnpoint.rotation;
    }


    public void MissionEnded()
    {
        if(selectedPickupObject != null && selectedLandingPad != null){
            if (Vector3.Distance(selectedPickupObject.transform.position, selectedLandingPad.transform.position) < 1.0f && beginnerMission.hoopsScore >= 30)
            {
                if(routeIndex == routes.Length - 1){
                    Time.timeScale = 0;
                    MissionCompleted.SetActive(true);
                    if (missionStarted)
                    {
                        missionDuration = Time.time - missionStartTime;
                    }
                    winhoopScore.text = beginnerMission.hoopsScore.ToString();
                    winTime.text = missionDuration.ToString("F2");
                    if (beginnerMission.hoopsScore >= 30 && beginnerMission.hoopsScore <= 38 && missionDuration <= 180 && missionDuration > 150)
                    {
                        titleEarnedWin.text = "Novice";
                    }
                    else if (beginnerMission.hoopsScore >= 30 && beginnerMission.hoopsScore <= 60 && missionDuration <= 150 && missionDuration > 135)
                    {
                        titleEarnedWin.text = "Amateur";
                    }
                    else if (beginnerMission.hoopsScore >= 38 && missionDuration <= 135)
                    {
                        titleEarnedWin.text = "Veteran";
                    }

                } else{
                    CompletedRoute();
                }
            }
            else if (Vector3.Distance(selectedPickupObject.transform.position, selectedLandingPad.transform.position) < 1.0f && beginnerMission.hoopsScore < 2)
            {
                
                if(routeIndex == routes.Length){
                    MissionFailed.SetActive(true);
                    Time.timeScale = 0;
                    MissionFailed.SetActive(true);
                    Debug.Log("Mission Failed");
                    losehoopScore.text = beginnerMission.hoopsScore.ToString();
                    if (missionStarted)
                    {
                        missionDuration = Time.time - missionStartTime;
                    }
                    loseTime.text = missionDuration.ToString("F2");
                } else{
                    CompletedRoute();
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
