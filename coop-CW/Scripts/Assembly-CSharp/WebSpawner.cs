using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WebSpawner : MonoBehaviour
{
	private void Update()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			base.gameObject.SetActive(value: false);
		}
		else if (PhotonNetwork.InRoom)
		{
			base.gameObject.SetActive(value: false);
			Spawn();
		}
	}

	private void Spawn()
	{
		int num = Random.Range(4, 8);
		if (Random.value < 0.1f)
		{
			num = Random.Range(10, 20);
		}
		for (int i = 0; i < num; i++)
		{
			List<PatrolPoint> list = Level.currentLevel.patrolGroups[PatrolPoint.PatrolGroup.Dog];
			Transform transform = list[Random.Range(0, list.Count)].transform;
			PhotonNetwork.Instantiate("Web", transform.position, Quaternion.identity, 0);
		}
	}
}
