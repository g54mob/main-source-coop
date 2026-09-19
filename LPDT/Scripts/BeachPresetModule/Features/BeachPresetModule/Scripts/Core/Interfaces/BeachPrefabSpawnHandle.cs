using System.Collections.Generic;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public class BeachPrefabSpawnHandle
	{
		internal List<GameObject> SpawnedObjects { get; } = new List<GameObject>();

		internal Transform RuntimeRoot { get; set; }
	}
}
