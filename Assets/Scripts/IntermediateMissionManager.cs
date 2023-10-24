using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermediateMissionManager : MonoBehaviour
{
    // [Header("GameObjects")]
    // public GameObject InstructionPanel;*
    // public Button StartButton;
    // public TextMeshProUGUI score;
    // public TextMeshProUGUI countdownText;
    // public TextMeshProUGUI takeoffText;
    // public TextMeshProUGUI timerText;
    // public GameObject drone;
    // public GameObject landingPad;
    // public InputManager input;

    // [Header("Variables")]
    // private float countdownTime = 5.0f;
    // [SerializeField] public bool countdownActive = false;
    // public IntermediateMission interMission;
    // public GameObject[] rings;
    // private Transform grandChildTransform;
    // public bool paused = false;
    // public bool start ;

    // private bool missionStarted = false;
    // private float missionStartTime;
    // private float missionDuration;


    // void Start()
    // {
    //     InstructionPanel.SetActive(true);
    //     input = GameObject.Find("CargoDrone").GetComponent<InputManager>();
    // }

    // public void InitializeReferences()
    // {
    //     countdownActive = true;
    //     Transform childTransform = drone.transform.GetChild(1);
    //     grandChildTransform = childTransform.GetChild(2);
    //     foreach (GameObject ring in rings)
    //     {
    //         ring.SetActive(true);
    //         landingPad.SetActive(true);
    //     }
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (StartButton())
    //     {
    //         InstructionPanel.SetActive(false);
    //     }
        
    // }

    // public bool StartButton()
    // {
    //     missionStarted = true;
    // }

    // public IEnumerator StartCountdown()
    // {
    //     float currentTime = countdownTime;
    //     while (currentTime > 0)
    //     {
    //         countdownText.text = Mathf.Ceil(currentTime).ToString();
    //         yield return new WaitForSeconds(1.0f);
    //         currentTime -= 1.0f;
    //         if (currentTime == 0)
    //         {
    //             takeoffText.gameObject.SetActive(true);
    //             countdownText.gameObject.SetActive(false);
    //             yield return new WaitForSeconds(1.0f);
    //             takeoffText.gameObject.SetActive(false);
    //             missionStarted = true;
    //             missionStartTime = Time.time;
    //         }
    //     }
    //     countdownActive = false;
    // }
 


}
