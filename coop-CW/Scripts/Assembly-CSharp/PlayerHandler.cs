using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
	public static PlayerHandler instance;

	public List<Player> players;

	public List<Player> playersAlive;

	public Action<Player> OnPlayerJoined;

	public Action<Player> OnPlayerLeft;

	public void AddPlayer(Player player)
	{
		players.Add(player);
		playersAlive.Add(player);
		OnPlayerJoined?.Invoke(player);
	}

	public void RemovePlayer(Player player)
	{
		if (instance.players.Contains(player))
		{
			instance.players.Remove(player);
		}
		if (instance.playersAlive.Contains(player))
		{
			instance.playersAlive.Remove(player);
		}
		OnPlayerLeft?.Invoke(player);
	}

	private void Awake()
	{
		instance = this;
	}

	public bool CanAnAlivePlayerSeePoint(Vector3 point, out Player firstPlayerThatCanSeeIt, Player ignoredPlayer = null)
	{
		foreach (Player item in playersAlive)
		{
			if ((!(ignoredPlayer != null) || !(ignoredPlayer == item)) && item.CanSee(point))
			{
				firstPlayerThatCanSeeIt = item;
				return true;
			}
		}
		firstPlayerThatCanSeeIt = null;
		return false;
	}

	public float GetPlayersClosestDistanceToAnotherPlayer(Player player)
	{
		float num = float.MaxValue;
		foreach (Player item in instance.playersAlive)
		{
			if (!(item == player))
			{
				float num2 = Vector3.Distance(player.Center(), item.Center());
				if (num2 < num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	public bool GetLargestClosestDistanceBetweenPlayers(out float maxMinDistanceBetweenPlayers, out Player mostAlonePlayer)
	{
		maxMinDistanceBetweenPlayers = float.MinValue;
		mostAlonePlayer = null;
		if (instance.playersAlive.Count < 2)
		{
			return false;
		}
		foreach (Player item in instance.playersAlive)
		{
			float playersClosestDistanceToAnotherPlayer = GetPlayersClosestDistanceToAnotherPlayer(item);
			if (!(playersClosestDistanceToAnotherPlayer < maxMinDistanceBetweenPlayers))
			{
				maxMinDistanceBetweenPlayers = playersClosestDistanceToAnotherPlayer;
				mostAlonePlayer = item;
			}
		}
		return mostAlonePlayer != null;
	}

	public Player GetClosestAlivePlayerToPoint(Vector3 point, bool requireVision = false, float maxRange = float.MaxValue)
	{
		float num = float.MaxValue;
		Player result = null;
		foreach (Player item in playersAlive)
		{
			if (!requireVision || !HelperFunctions.LineCheck(point, item.Center(), HelperFunctions.LayerType.TerrainProp).transform)
			{
				float num2 = Vector3.Distance(item.Center(), point);
				if ((!(maxRange < 100000f) || !(num2 > maxRange)) && num2 < num)
				{
					num = num2;
					result = item;
				}
			}
		}
		return result;
	}

	public Player FindClosestPlayerToPlayer(Player player)
	{
		float closestDistance;
		return FindClosestPlayerToPlayer(player, out closestDistance);
	}

	public Player FindClosestPlayerToPlayer(Player player, out float closestDistance)
	{
		closestDistance = float.MaxValue;
		Player result = null;
		foreach (Player item in playersAlive)
		{
			if (!(item == player))
			{
				float num = Vector3.Distance(item.Center(), player.Center());
				if (num < closestDistance)
				{
					closestDistance = num;
					result = item;
				}
			}
		}
		return result;
	}

	public Player GetFurthestPlayerFromPlayer(Player player)
	{
		float num = float.MinValue;
		Player result = null;
		foreach (Player item in playersAlive)
		{
			if (!(item == player))
			{
				float num2 = Vector3.Distance(item.Center(), player.Center());
				if (num2 > num)
				{
					num = num2;
					result = item;
				}
			}
		}
		return result;
	}

	public static Player FindClosest(Vector3 point, List<Player> players)
	{
		float num = float.MaxValue;
		Player result = null;
		foreach (Player player in players)
		{
			float num2 = Vector3.Distance(player.Center(), point);
			if (num2 < num)
			{
				num = num2;
				result = player;
			}
		}
		return result;
	}

	internal Player GetNextPlayer(ref int currentPlayerCheckID)
	{
		if (players.Count == 0)
		{
			return null;
		}
		currentPlayerCheckID++;
		if (currentPlayerCheckID >= players.Count)
		{
			currentPlayerCheckID = 0;
		}
		return players[currentPlayerCheckID];
	}

	internal Player GetNextPlayerAlive(ref int currentPlayerCheckID, int offset = 1)
	{
		if (playersAlive.Count == 0)
		{
			return null;
		}
		currentPlayerCheckID += offset;
		if (currentPlayerCheckID < 0)
		{
			currentPlayerCheckID = playersAlive.Count - 1;
		}
		if (currentPlayerCheckID >= playersAlive.Count)
		{
			currentPlayerCheckID = 0;
		}
		return playersAlive[currentPlayerCheckID];
	}

	internal Player GetNextPlayerAlive(Player currenPlayer = null, int offset = 1)
	{
		if (playersAlive.Count == 0)
		{
			return null;
		}
		int currentPlayerCheckID = 0;
		if (currenPlayer != null)
		{
			currentPlayerCheckID = GetPlayerID(currenPlayer);
		}
		return GetNextPlayerAlive(ref currentPlayerCheckID, offset);
	}

	private int GetPlayerID(Player currenPlayer)
	{
		if (playersAlive.Count == 0)
		{
			return 0;
		}
		for (int i = 0; i < playersAlive.Count; i++)
		{
			if (playersAlive[i] == currenPlayer)
			{
				return i;
			}
		}
		return 0;
	}

	public bool TryGetPlayerFromOwnerID(int photonViewOwnerActorNr, out Player o)
	{
		foreach (Player player in players)
		{
			if (player.refs.view.OwnerActorNr == photonViewOwnerActorNr)
			{
				o = player;
				return true;
			}
		}
		o = null;
		return false;
	}

	internal Player TryGetPlayerFromViewID(int viewID)
	{
		PhotonView photonView = PhotonNetwork.GetPhotonView(viewID);
		if ((bool)photonView)
		{
			return photonView.GetComponent<Player>();
		}
		return null;
	}

	public List<Player> GetAlivePlayersWithinFlatDistanceFromPoint(Vector3 point, float distance)
	{
		List<Player> list = new List<Player>();
		foreach (Player item in playersAlive)
		{
			if (HelperFunctions.FlatDistance(item.refs.ragdoll.GetBodypart(BodypartType.Torso).transform.position, point) < distance)
			{
				list.Add(item);
			}
		}
		return list;
	}

	internal float PlayerVoiceVolumeAtPosition(Vector3 position, float minRange, float maxRange)
	{
		float num = 0f;
		for (int i = 0; i < playersAlive.Count; i++)
		{
			if (!(playersAlive[i].data.microphoneValue < 0.05f))
			{
				float num2 = Mathf.InverseLerp(maxRange, minRange, Vector3.Distance(playersAlive[i].Center(), position));
				float num3 = playersAlive[i].data.microphoneValue * num2;
				if (num3 > num)
				{
					num = num3;
				}
			}
		}
		return num;
	}

	public bool AllPlayersAsleep()
	{
		bool result = true;
		if (playersAlive.Count <= 0)
		{
			return false;
		}
		for (int i = 0; i < playersAlive.Count; i++)
		{
			if (playersAlive[i].data.sleepAmount < 0.9f)
			{
				result = false;
			}
		}
		return result;
	}

	internal bool AllPlayersInBed()
	{
		bool result = true;
		for (int i = 0; i < playersAlive.Count; i++)
		{
			if (playersAlive[i].data.currentBed == null)
			{
				result = false;
			}
		}
		return result;
	}

	internal Player GetRandomPlayerAlive()
	{
		return playersAlive[UnityEngine.Random.Range(0, playersAlive.Count)];
	}
}
