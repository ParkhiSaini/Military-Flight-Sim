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
    private bool countdownActive = true;
    private BeginnerMission beginnerMission;
    private Transform grandChildTransform;

    private void Start()
    {
        beginnerMission = drone.GetComponent<BeginnerMission>();
        Transform childTransform = drone.transform.GetChild(1);
        grandChildTransform = childTransform.GetChild(2);
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        
        // Drone.transform.position = transform.position;

        UpdateScore();
    }

    private void UpdateScore()
    {
        score.text = beginnerMission.hoopsScore.ToString();
    }

    private IEnumerator StartCountdown()
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
}
