using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreItem : MonoBehaviourPunCallbacks
{
	public TMP_Text ScoreText;

	Player player;

    void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Initialize(player);
        }
    }

	public void Initialize(Player player)
	{
		this.player = player;
		UpdateStats();
	}

	void UpdateStats()
	{
		if(player.CustomProperties.TryGetValue("Score", out object hoopsScore))
        {
            ScoreText.text = hoopsScore.ToString();
        }
	}

    void Update()
    {
        UpdateStats();
    }


}