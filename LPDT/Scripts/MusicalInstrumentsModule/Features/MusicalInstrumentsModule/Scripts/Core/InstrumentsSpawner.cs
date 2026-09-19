using System;
using Fusion;
using UnityEngine;

namespace Features.MusicalInstrumentsModule.Scripts.Core
{
	[NetworkBehaviourWeaved(0)]
	public class InstrumentsSpawner : NetworkBehaviour
	{
		[Serializable]
		private struct InstrumentSpawnPoint
		{
			public MusicalInstrumentType InstrumentType;

			public Transform SpawnPoint;
		}

		[SerializeField]
		private NetworkObject _flutePrefab;

		[SerializeField]
		private NetworkObject _guitarPrefab;

		[SerializeField]
		private InstrumentSpawnPoint[] _spawnPoints;

		public override void Spawned()
		{
			if (base.HasStateAuthority)
			{
				SpawnInstruments();
			}
		}

		private void SpawnInstruments()
		{
			if (_spawnPoints == null)
			{
				return;
			}
			for (int i = 0; i < _spawnPoints.Length; i++)
			{
				InstrumentSpawnPoint instrumentSpawnPoint = _spawnPoints[i];
				if (!(instrumentSpawnPoint.SpawnPoint == null) && instrumentSpawnPoint.SpawnPoint.gameObject.activeInHierarchy && TryResolvePrefab(instrumentSpawnPoint.InstrumentType, out var prefab))
				{
					base.Runner.Spawn(prefab, instrumentSpawnPoint.SpawnPoint.position, instrumentSpawnPoint.SpawnPoint.rotation, base.Runner.LocalPlayer);
				}
			}
		}

		private bool TryResolvePrefab(MusicalInstrumentType instrumentType, out NetworkObject prefab)
		{
			prefab = instrumentType switch
			{
				MusicalInstrumentType.Flute => _flutePrefab, 
				MusicalInstrumentType.Guitar => _guitarPrefab, 
				_ => null, 
			};
			return prefab != null;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
