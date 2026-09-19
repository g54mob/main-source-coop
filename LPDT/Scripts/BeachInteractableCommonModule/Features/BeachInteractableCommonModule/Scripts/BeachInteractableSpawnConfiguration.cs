using Fusion;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.BeachInteractableCommonModule.Scripts
{
	[CreateAssetMenu(fileName = "BeachInteractableSpawnConfiguration_Default", menuName = "Configurations/BeachInteractablesCommon/BeachInteractableSpawnConfiguration")]
	public class BeachInteractableSpawnConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public Fusion.SerializableDictionary<BeachInteractableType, Fusion.SerializableDictionary<BeachInteractableLocationType, NetworkPrefabRef>> NetworkBeachInteractablePool { get; private set; }

		[field: SerializeField]
		public Global.SerializableDictionary.SerializableDictionary<BeachInteractableType, Global.SerializableDictionary.SerializableDictionary<BeachInteractableLocationType, GameObject>> LocalBeachInteractablePool { get; private set; }

		public bool TryGetNetworkBeachInteractablePrefab(BeachInteractableType interactableType, BeachInteractableLocationType locationType, out NetworkPrefabRef interactablePrefab)
		{
			interactablePrefab = NetworkPrefabRef.Empty;
			if (NetworkBeachInteractablePool.TryGetValue(interactableType, out var value))
			{
				return value.TryGetValue(locationType, out interactablePrefab);
			}
			return false;
		}

		public bool TryGetLocalBeachInteractablePrefab(BeachInteractableType interactableType, BeachInteractableLocationType locationType, out GameObject interactablePrefab)
		{
			interactablePrefab = null;
			if (LocalBeachInteractablePool.TryGetValue(interactableType, out var value))
			{
				return value.TryGetValue(locationType, out interactablePrefab);
			}
			return false;
		}
	}
}
