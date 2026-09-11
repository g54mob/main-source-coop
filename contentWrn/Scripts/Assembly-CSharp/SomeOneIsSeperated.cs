using System.Collections.Generic;
using UnityEngine;
using pworld.Scripts.Extensions;

public class SomeOneIsSeperated : MonoBehaviour
{
	public float minDistanceForPlayerToBeConsideredToBeAlone = 50f;

	public List<Transform> players = new List<Transform>();

	public Transform targetPlayer;

	public float aggroRangeIfSeperated = 150f;

	public float GetMinDistanceToOtherPlayers(Transform player, out Transform closestPlayer)
	{
		float num = float.MaxValue;
		closestPlayer = null;
		foreach (Transform player2 in players)
		{
			if (!(player2 == player))
			{
				float num2 = Vector3.Distance(player.transform.position, player2.transform.position);
				if (num2 < num)
				{
					num = num2;
					closestPlayer = player2;
				}
			}
		}
		return num;
	}

	public bool SomeoneIsAlone()
	{
		float num = float.MinValue;
		Transform transform = null;
		Transform transform2 = null;
		foreach (Transform player in players)
		{
			Transform closestPlayer;
			float minDistanceToOtherPlayers = GetMinDistanceToOtherPlayers(player, out closestPlayer);
			if (minDistanceToOtherPlayers > num)
			{
				num = minDistanceToOtherPlayers;
				transform = player;
				transform2 = closestPlayer;
			}
		}
		bool flag = false;
		flag = ((!(targetPlayer != null)) ? (num > minDistanceForPlayerToBeConsideredToBeAlone) : (num + 5f > minDistanceForPlayerToBeConsideredToBeAlone));
		Debug.DrawLine(transform.position, transform2.position, flag ? Color.green : Color.red);
		return flag;
	}

	public Transform GetClosestPlayer()
	{
		return players.FindClosest(base.transform.position);
	}

	private void Go()
	{
		if (SomeoneIsAlone())
		{
			targetPlayer = GetClosestPlayer();
		}
		else
		{
			targetPlayer = null;
		}
		if (targetPlayer != null)
		{
			Debug.DrawLine(base.transform.position, targetPlayer.position, Color.yellow);
		}
	}

	private void OnDrawGizmos()
	{
		if (players.Count != 0)
		{
			Go();
		}
	}

	private void Update()
	{
	}
}
