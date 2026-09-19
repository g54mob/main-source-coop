using System.Collections.Generic;
using UnityEngine;

namespace Features.StreamersSupportModule.Scripts
{
	[CreateAssetMenu(fileName = "StreamersStandsConfiguration_Default", menuName = "Configurations/Streamers Support/StreamersStandsConfiguration")]
	public class StreamersStandsConfiguration : ScriptableObject
	{
		public List<StreamersStandData> StreamersStandData = new List<StreamersStandData>();
	}
}
