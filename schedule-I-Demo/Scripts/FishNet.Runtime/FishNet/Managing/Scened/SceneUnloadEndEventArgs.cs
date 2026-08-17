using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace FishNet.Managing.Scened
{
	public struct SceneUnloadEndEventArgs
	{
		public readonly UnloadQueueData QueueData;

		[Obsolete("Use UnloadedScenesV2")]
		public int[] UnloadedSceneHandles;

		[Obsolete("Use UnloadedScenesV2")]
		public string[] UnloadedSceneNames;

		public List<Scene> UnloadedScenes;

		public List<UnloadedScene> UnloadedScenesV2;

		internal SceneUnloadEndEventArgs(UnloadQueueData sqd, List<Scene> unloadedScenes, List<UnloadedScene> newUnloadedScenes)
		{
			QueueData = sqd;
			UnloadedScenes = unloadedScenes;
			UnloadedScenesV2 = newUnloadedScenes;
			UnloadedSceneNames = new string[newUnloadedScenes.Count];
			UnloadedSceneHandles = new int[newUnloadedScenes.Count];
			for (int i = 0; i < newUnloadedScenes.Count; i++)
			{
				UnloadedSceneNames[i] = newUnloadedScenes[i].Name;
				UnloadedSceneHandles[i] = newUnloadedScenes[i].Handle;
			}
		}
	}
}
