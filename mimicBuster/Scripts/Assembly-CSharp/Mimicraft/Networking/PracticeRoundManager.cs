using System;
using System.Collections.Generic;
using Mimicraft.Gameplay;
using UnityEngine;

namespace Mimicraft.Networking
{
	public class PracticeRoundManager : GameModeController
	{
		[Tooltip("Duvara tirmanma. Modelci'nin online yetenegi - practice'te de acik, boylece denenebilir.")]
		[SerializeField]
		private bool allowWallClimb = true;

		private Transform[] spawnPoints = Array.Empty<Transform>();

		private int spawnIndex;

		private bool placed;

		private const float SpawnSearchGraceSeconds = 8f;

		private float searchStartedAt;

		private bool warnedNoSpawns;

		public override MapMarkerModes MarkerMode => MapMarkerModes.Practice;

		public override bool LocalPlayerShouldHaveModel => true;

		public override bool CanLocalPlayerEditModel => true;

		public override bool IsModelSetupWindow => true;

		public override bool LocalPlayerUsesHunterMovement => false;

		public override bool LocalPlayerMayWallClimb => allowWallClimb;

		public override bool IsLocalPlayerArmed => false;

		public override bool IsLocalPlayerFrozen => false;

		public override bool LocalPlayerMayLeaveEditMode => true;

		public override bool IsLocalPlayerParticipating => true;

		public override bool UsesRolePreference => false;

		public override bool ServerAllowsBodyEdit(ulong clientId)
		{
			return true;
		}

		public override bool ShouldShowNameTag(ulong clientId)
		{
			return true;
		}

		public override void FillLobbyInfo(LobbySettingsData settings, List<LobbyInfoRow> rows)
		{
			base.FillLobbyInfo(settings, rows);
			rows.Add(new LobbyInfoRow("Sure", "yok"));
			rows.Add(new LobbyInfoRow("Avci", "yok"));
		}

		public override void SetMap(Transform[] hunterSpawnPoints, Transform[] hiderSpawnPoints, Transform[] lobbySpawnPoints, GameObject hunterDoor)
		{
			if (CollectSpawnPoints())
			{
				placed = true;
				PlaceConnectedPlayersAtLobbySpawns();
			}
			if (hunterDoor != null)
			{
				hunterDoor.SetActive(value: false);
			}
		}

		private void Update()
		{
			if (base.IsServer && !placed)
			{
				if (searchStartedAt <= 0f)
				{
					searchStartedAt = Time.time;
				}
				if (CollectSpawnPoints())
				{
					placed = true;
					PlaceConnectedPlayersAtLobbySpawns();
				}
				else if (!warnedNoSpawns && Time.time - searchStartedAt > 8f)
				{
					warnedNoSpawns = true;
					Debug.LogWarning("[PracticeRoundManager] Haritada bu moda ait spawn marker'i bulunamadi - oyuncular bulunduklari yerde kaliyor. Marker'larin Modes maskesinde Practice isaretli mi, ve mod sahnesinde MapLoader var mi?", this);
				}
			}
		}

		private bool CollectSpawnPoints()
		{
			List<Transform> list = new List<Transform>();
			List<Transform> list2 = new List<Transform>();
			List<Transform> list3 = new List<Transform>();
			MapMarker[] array = UnityEngine.Object.FindObjectsByType<MapMarker>(FindObjectsSortMode.None);
			foreach (MapMarker mapMarker in array)
			{
				if (mapMarker.AppliesTo(MarkerMode))
				{
					if (mapMarker.Is(MapMarkerKind.ModelerSpawn))
					{
						list.Add(mapMarker.transform);
					}
					if (mapMarker.Is(MapMarkerKind.LobbySpawn))
					{
						list2.Add(mapMarker.transform);
					}
					if (mapMarker.Is(MapMarkerKind.HiderSpawn))
					{
						list3.Add(mapMarker.transform);
					}
				}
			}
			spawnPoints = ((list.Count > 0) ? list : ((list2.Count > 0) ? list2 : list3)).ToArray();
			spawnIndex = 0;
			return spawnPoints.Length != 0;
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
			return "PracticeRoundManager";
		}
	}
}
