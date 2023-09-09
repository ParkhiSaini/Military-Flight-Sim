using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginnerMission : MonoBehaviour
{
    public int hoopsScore;
    private HashSet<GameObject> scoredRings = new HashSet<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ring") && !scoredRings.Contains(other.gameObject))
        {
            Debug.Log("Passed");
            hoopsScore += 1;
            scoredRings.Add(other.gameObject);
        }
    }
}
