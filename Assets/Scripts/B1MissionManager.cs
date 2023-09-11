using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class B1MissionManager : MonoBehaviour
{
    [Header("GameObjects")]
    public TextMeshProUGUI score;
    public TextMeshProUGUI countdownText;
    public GameObject drone;

    [Header("Variables")]
    private float countdownTime = 5.0f;
    [SerializeField] public bool countdownActive = false;
    private BeginnerMission beginnerMission;
    public GameObject[] rings;
    private Transform grandChildTransform;
    public GameObject landingPad;
    public GameObject MissionCompleted;
    public GameObject MissionFailed;
    public void InitializeReferences()
    {
        score.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(true);
        countdownActive = true;
        beginnerMission = drone.GetComponent<BeginnerMission>();
        drone.transform.position = new Vector3(95.9609985f,7.88800001f,28.5725574f);
        drone.transform.rotation = new Quaternion(0, 0, 0, 0);
        drone.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
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
        if (score.gameObject.activeSelf)
        {
            UpdateScore();
        }
        if(countdownActive){
            drone.GetComponent<Rigidbody>().velocity = Vector3.zero;
            drone.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        MissionEnded();
    }

    private void UpdateScore()
    {
        score.text = beginnerMission.hoopsScore.ToString();
    }

    public IEnumerator StartCountdown()
    {
        float currentTime = countdownTime;
        while (currentTime > 0)
        {
            countdownText.text = Mathf.Ceil(currentTime).ToString();
            yield return new WaitForSeconds(1.0f);
            currentTime -= 1.0f;
        }
        countdownText.text = "GO";
        countdownActive = false;
    }
    
    public void MissionEnded()
    {
        Debug.Log("Distance: " + Vector3.Distance(drone.transform.position, landingPad.transform.position));
        if(Vector3.Distance(drone.transform.position, landingPad.transform.position) < 0.5f && beginnerMission.hoopsScore >= 10)
        {
            Debug.Log("Mission Completed");
            Time.timeScale = 0;
            MissionCompleted.SetActive(true);
        } else if(Vector3.Distance(drone.transform.position, landingPad.transform.position) < 0.5f && beginnerMission.hoopsScore < 10){
            MissionFailed.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("Mission Completed");
        }
    }
}
