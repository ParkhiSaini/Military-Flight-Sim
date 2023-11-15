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
    PhotonView photonView;
    // Start is called before the first frame update

    void Start(){
        photonView = GetComponent<PhotonView>();
        if(photonView.IsMine){
            enabled = true;
        } else {
            enabled = false;
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if(PhotonNetwork.IsMasterClient){
            if (other.gameObject.CompareTag("RingP2") && !scoredRings.Contains(other.gameObject))
            {
                hoopsScore += 1;
                other.gameObject.GetComponent<Renderer>().material.color = newColor;

                scoredRings.Add(other.gameObject);
                Hashtable hash = new Hashtable();
                hash.Add("Score", hoopsScore);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                photonView.RPC("UpdateScore", RpcTarget.Others, hoopsScore);
            }
        } else{
            if (other.gameObject.CompareTag("RingP1") && !scoredRings.Contains(other.gameObject))
            {
                hoopsScore += 1;
                other.gameObject.GetComponent<Renderer>().material.color = newColor;

                scoredRings.Add(other.gameObject);
                Hashtable hash = new Hashtable();
                hash.Add("Score", hoopsScore);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                photonView.RPC("UpdateScore", RpcTarget.Others, hoopsScore);
            }
        }
    }

    [PunRPC]
    public void UpdateScore(int score)
    {
        hoopsScore = score;
    }

}
