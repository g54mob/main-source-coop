using System;
using System.Collections.Generic;
using Features.MultiplayerSessionServices.Scripts;
using Features.NetworkRandomModule.Scripts;

namespace Features.BeachInteractableCommonModule.Scripts
{
	[Serializable]
	public class BeachInteractableSpawnModel : INetworkRandomConsumer, IBeachInteractableRunReset
	{
		private static readonly DeterministicHash NameHash = new DeterministicHash(typeof(BeachInteractableSpawnModel).FullName);

		private readonly List<BeachInteractableSpawnData> _beachInteractablesSpawnData = new List<BeachInteractableSpawnData>();

		public List<RecordedBeachInteractableData> RecordedBeachInteractables { get; set; } = new List<RecordedBeachInteractableData>();

		public Random Random { get; private set; }

		public IReadOnlyList<BeachInteractableSpawnData> BeachInteractablesSpawnData => _beachInteractablesSpawnData;

		public void AddBeachInteractableSpawnData(BeachInteractableSpawnData beachInteractableSpawnData)
		{
			_beachInteractablesSpawnData.Add(beachInteractableSpawnData);
		}

		public void RemoveBeachInteractableSpawnData(BeachInteractableSpawnData beachInteractableSpawnData)
		{
			_beachInteractablesSpawnData.Remove(beachInteractableSpawnData);
		}

		public int GetConsumerIdentifier()
		{
			return NameHash.GetRaw();
		}

		public void InjectNetworkRandom(Random random)
		{
			Random = random;
		}

		public void ResetRecordedInteractables()
		{
			RecordedBeachInteractables.Clear();
		}
	}
}
