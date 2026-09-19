using Fusion.Statistics;
using UnityEngine;

namespace Features.DebugBuildModule.Scripts
{
	public class FusionStatisticsDisabler : MonoBehaviour
	{
		[SerializeField]
		private FusionStatistics _fusionStatistics;

		private void Start()
		{
			if (!Debug.isDebugBuild)
			{
				Object.Destroy((Object)(object)_fusionStatistics);
			}
		}
	}
}
