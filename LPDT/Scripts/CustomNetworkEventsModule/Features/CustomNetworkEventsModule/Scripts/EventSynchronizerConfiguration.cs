using Fusion;
using UnityEngine;

namespace Features.CustomNetworkEventsModule.Scripts
{
	[CreateAssetMenu(fileName = "EventSynchronizerConfiguration_Default", menuName = "Configurations/EventSynchronizer/EventSynchronizerConfiguration")]
	public class EventSynchronizerConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public NetworkObject NetworkEventSynchronizer { get; private set; }
	}
}
