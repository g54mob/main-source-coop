using System;
using System.Collections.Generic;

namespace EvilCore.Recording
{
	[Serializable]
	public class DirectorPresetData
	{
		public string presetName = "New Preset";

		public List<DirectorCameraData> cameras = new List<DirectorCameraData>();
	}
}
