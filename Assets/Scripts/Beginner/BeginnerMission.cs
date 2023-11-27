using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginnerMission : MonoBehaviour
{
    public int hoopsScore;
    public Color newColor = Color.green;
    private Renderer objectRenderer;
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
            hoopsScore += 1;
            other.gameObject.GetComponent<Renderer>().material.color = newColor;

            scoredRings.Add(other.gameObject);
        }
    }
}
