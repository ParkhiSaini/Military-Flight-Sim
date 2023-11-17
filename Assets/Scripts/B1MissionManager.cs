using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class B1MissionManager : MonoBehaviour
{
    [Header("On Screen UI")]
    public TextMeshProUGUI score;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI takeoffText;
    public TextMeshProUGUI timerText;

    [Header("GameObjects")]
    public GameObject drone;
    public GameObject landingPad;
    public GameObject MissionCompleted;
    public GameObject MissionFailed;
    public GameObject pauseMenu;
    public GameObject instructions;
    public GameObject HealthBar;
    public GameObject FPSCam;

    public TMP_Text hoopScore;

    [Header("References")]
    public InputManager input;

    [Header("Winning Screen UI")]
    public TMP_Text winhoopScore;

    [Header("Failed Screen UI")]
    public TMP_Text losehoopScore;


    [Header("Variables")]
    private float countdownTime = 5.0f;
    [SerializeField] public bool countdownActive = false;
    public BeginnerMission beginnerMission;
    public GameObject[] rings;
    private Transform grandChildTransform;
    public bool paused =false;
    public bool fps=false;

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
        UpdateTimer(); 
        if (countdownActive)
        {
            drone.GetComponent<Rigidbody>().velocity = Vector3.zero;
            drone.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        if (input.Pause == 1.0f)
        {

            pauseMenu.gameObject.SetActive(true);
        }
        if (input.CamSwitch == 1.0f)
        {
            fps=true;
            FPSCam.gameObject.SetActive(true);

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

    public void MissionEnded()
    {
        if (Vector3.Distance(drone.transform.position, landingPad.transform.position) < 1.0f && beginnerMission.hoopsScore >= 2)
        {
            
            Time.timeScale = 0;
            MissionCompleted.SetActive(true);
            hoopScore.text = beginnerMission.hoopsScore.ToString();

           
            if (missionStarted)
            {
                missionDuration = Time.time - missionStartTime;
                
            }
        }
        else if (Vector3.Distance(drone.transform.position, landingPad.transform.position) < 1.0f && beginnerMission.hoopsScore < 2)
        {
            MissionFailed.SetActive(true);
            losehoopScore.text = beginnerMission.hoopsScore.ToString();
            Debug.Log("losehoopScore: " + beginnerMission.hoopsScore.ToString());
            Time.timeScale = 0;
        
        }
    }

    public void Pause()
    {
        paused = true;
        pauseMenu.gameObject.SetActive(true);
    }

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

}
