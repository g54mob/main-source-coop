using System;
using System.Collections.Generic;
using Mimicraft.Gameplay;
using UnityEngine;

namespace Mimicraft.Networking
{
	public class RangeRoundManager : GameModeController
	{
		private Transform[] spawnPoints = Array.Empty<Transform>();

		private int spawnIndex;

		private float nextSweepAt;

		private const float SweepIntervalSeconds = 0.25f;

		public override MapMarkerModes MarkerMode => MapMarkerModes.Standard;

		public override bool IsLocalPlayerArmed => true;

		public override bool LocalPlayerUsesHunterMovement => true;

		public override bool LocalPlayerMayShoot => true;

		public override bool PenalisesMissedShots => false;

		public override bool LocalPlayerShouldHaveModel => false;

		public override bool CanLocalPlayerEditModel => false;

		public override bool IsModelSetupWindow => false;

		public override bool LocalPlayerMayWallClimb => false;

		public override bool IsLocalPlayerFrozen => false;

		public override bool IsLocalPlayerParticipating => true;

		public override bool UsesRolePreference => false;

		public override bool ServerAllowsShooting(ulong clientId)
		{
			return true;
		}

		public override bool ServerAllowsBodyEdit(ulong clientId)
		{
			return false;
		}

		public override bool ShouldShowNameTag(ulong clientId)
		{
			return true;
		}

		public override void FillLobbyInfo(LobbySettingsData settings, List<LobbyInfoRow> rows)
		{
			base.FillLobbyInfo(settings, rows);
			rows.Add(new LobbyInfoRow("Sure", "yok"));
			rows.Add(new LobbyInfoRow("Modelci", "yok"));
		}

		private void Update()
		{
			if (spawnPoints.Length == 0 && !(Time.time < nextSweepAt))
			{
				nextSweepAt = Time.time + 0.25f;
				SweepForRoomSpawns();
			}
		}

		private void SweepForRoomSpawns()
		{
			List<Transform> list = new List<Transform>();
			MapMarker[] array = UnityEngine.Object.FindObjectsByType<MapMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (MapMarker mapMarker in array)
			{
				if (mapMarker.Is(MapMarkerKind.HunterRoomSpawn))
				{
					list.Add(mapMarker.transform);
				}
			}
			if (list.Count != 0)
			{
				SetHunterRoom(list.ToArray());
			}
		}

		public override void SetHunterRoom(Transform[] hunterRoomSpawnPoints)
		{
			spawnPoints = hunterRoomSpawnPoints ?? Array.Empty<Transform>();
			spawnIndex = 0;
			if (spawnPoints.Length != 0)
			{
				PlaceConnectedPlayersAtLobbySpawns();
			}
		}

		public override bool TryGetLobbySpawn(out Vector3 position, out Quaternion rotation)
		{
			for (int i = 0; i < spawnPoints.Length; i++)
			{
				Transform transform = spawnPoints[spawnIndex++ % spawnPoints.Length];
				if (!(transform == null))
				{
					position = transform.position;
					rotation = transform.rotation;
					return true;
				}
			}
			return base.TryGetLobbySpawn(out position, out rotation);
		}

		public override void OnNetworkSpawn()
		{
			base.OnNetworkSpawn();
			if (base.IsServer)
			{
				CurrentPhase.Value = RoundPhase.Hunt;
			}
		}

		protected override void __initializeVariables()
		{
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			base.__initializeRpcs();
		}

		protected internal override string __getTypeName()
		{
			return "RangeRoundManager";
		}
	}
}
