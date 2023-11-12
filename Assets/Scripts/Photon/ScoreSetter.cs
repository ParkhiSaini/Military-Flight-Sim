using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class ScoreSetter : MonoBehaviourPunCallbacks
{
    public int hoopsScore;
    public Color newColor = Color.green;
    private Renderer objectRenderer;
    private HashSet<GameObject> scoredRings = new HashSet<GameObject>();
    // Start is called before the first frame update

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ring") && !scoredRings.Contains(other.gameObject))
        {
            hoopsScore += 1;
            other.gameObject.GetComponent<Renderer>().material.color = newColor;

            scoredRings.Add(other.gameObject);
            Hashtable hash = new Hashtable();
            hash.Add("Score", hoopsScore);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }
}
