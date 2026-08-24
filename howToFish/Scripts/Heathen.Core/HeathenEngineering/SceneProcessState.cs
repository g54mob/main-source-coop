using System;
using System.Collections.Generic;
using System.Text;

namespace HeathenEngineering
{
	[Serializable]
	public class SceneProcessState
	{
		public int setActiveScene = -1;

		public List<int> unloadTargets;

		public List<int> loadTargets;

		public float unloadProgress;

		public float loadProgress;

		public float transitionProgress;

		public bool complete;

		public bool hasError;

		public string errorMessage;

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Loading [" + loadTargets.Count + "] scenes: " + loadProgress.ToString("P0") + " complete.\n");
			stringBuilder.Append("Unloading [" + unloadTargets.Count + "] scenes " + unloadProgress.ToString("P0") + " complete.\n");
			stringBuilder.Append("Total completion: " + transitionProgress.ToString("P0") + " complete.");
			return stringBuilder.ToString();
		}
	}
}
