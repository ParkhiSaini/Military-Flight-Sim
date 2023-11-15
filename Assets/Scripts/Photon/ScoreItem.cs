using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreItem : MonoBehaviourPunCallbacks
{
	public TMP_Text ScoreText1;
    public TMP_Text ScoreText2;

	Player player;

    void Start()
    {
        Initialize(PhotonNetwork.PlayerList[0]);
        Initialize(PhotonNetwork.PlayerList[1]);
    }

    void FixedUpdate()
    {
        UpdateStats();
    }

	public void Initialize(Player player)
	{
		this.player = player;
		UpdateStats();
	}

	void UpdateStats()
	{
        if(PhotonNetwork.PlayerList[0].CustomProperties.TryGetValue("Score", out object hoopsScore1))
        {
            ScoreText1.text = hoopsScore1.ToString();
        } else {
            ScoreText1.text = "0";
        }

        if(PhotonNetwork.PlayerList[1].CustomProperties.TryGetValue("Score", out object hoopsScore2))
        {
            ScoreText2.text = hoopsScore2.ToString();
        } else {
            ScoreText2.text = "0";
        }
	}

}