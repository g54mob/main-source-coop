using System;
using System.Collections.Generic;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.SceneManagement
{
	[Serializable]
	public class DespawnNetworkObjectsNetworkEvent : NetworkEventBase<DespawnNetworkObjectsNetworkEvent>
	{
		[field: SerializeField]
		public List<string> ObjectsToDespawnScenes { get; private set; }

		[field: SerializeField]
		public bool CleanUpObjectPool { get; private set; }

		public void SendEvent(List<string> objectsToDespawnScenes, bool cleanUpObjectPool)
		{
			ObjectsToDespawnScenes = objectsToDespawnScenes;
			CleanUpObjectPool = cleanUpObjectPool;
			Send();
		}
	}
}
