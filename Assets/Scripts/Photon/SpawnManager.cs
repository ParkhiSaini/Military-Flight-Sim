using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance;

	public Spawnpoint[] spawnpoints;
	int index = 0;

	void Awake()
	{
		Instance = this;
	}

	public Transform GetSpawnpoint()
	{
		if(PhotonNetwork.IsMasterClient){
			index = 0;
		} else {
			index = 1;
		}
		return spawnpoints[index].transform;

	}

	public int GetIndex()
	{
		return index;
	}

}