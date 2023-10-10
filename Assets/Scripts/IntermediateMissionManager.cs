// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class IntermediateMissionManager : MonoBehaviour
// {
//     [Header("GameObjects")]
//     public GameObject InstructionPanel;
//     public Button StartButton;
//     public TextMeshProUGUI score;
//     public TextMeshProUGUI countdownText;
//     public TextMeshProUGUI takeoffText;
//     public TextMeshProUGUI timerText; // Added for the timer display
//     public GameObject drone;
//     public GameObject landingPad;
//     public GameObject MissionCompleted;
//     public GameObject MissionFailed;
//     public GameObject PausePanel;
//     public InputManager input;

//     [Header("Variables")]
//     private float countdownTime = 5.0f;
//     [SerializeField] public bool countdownActive = false;
//     public IntermediateMission interMission;
//     public GameObject[] rings;
//     private Transform grandChildTransform;
//     public bool paused = false;
//     public bool start ;

//     // Timer variables
//     private bool missionStarted = false;
//     private float missionStartTime;
//     private float missionDuration;


//     void Start()
//     {
//         InstructionPanel.SetActive(true);
//         input = GameObject.Find("CargoDrone").GetComponent<InputManager>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (StartButton())
//         {
//             InstructionPanel.SetActive(false);
//         }
        
//     }

//     public bool StartButton()
//     {
//         missionStarted = true;
        
//     }

    


// }
