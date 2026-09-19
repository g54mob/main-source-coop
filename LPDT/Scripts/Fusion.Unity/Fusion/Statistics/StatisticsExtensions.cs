using UnityEngine;
using UnityEngine.UI;

namespace Fusion.Statistics
{
	public static class StatisticsExtensions
	{
		public static FusionStatistics SetupStatistics(this NetworkRunner runner, FusionStatisticsConfig customConfig = null)
		{
			if (!runner || !runner.IsRunning)
			{
				Debug.LogWarning($"NetworkRunner should be running to setup {typeof(FusionStatistics)}");
			}
			FusionStatistics fusionStatistics = runner.gameObject.AddComponent<FusionStatistics>();
			if ((bool)fusionStatistics)
			{
				runner.AddGlobal(fusionStatistics);
			}
			return fusionStatistics;
		}

		public static void RemoveStatistics(this NetworkRunner runner)
		{
			FusionStatistics component = runner.GetComponent<FusionStatistics>();
			if ((bool)runner && runner.IsRunning)
			{
				runner.RemoveGlobal(component);
			}
			Object.Destroy(component);
			Canvas globalStatisticsCanvas = FusionStatistics.GlobalStatisticsCanvas;
			if (!globalStatisticsCanvas)
			{
				return;
			}
			FusionStatisticsRoot fusionStatisticsRoot = ((FusionStatisticsRoot.ActiveRoot == component.Root) ? null : FusionStatisticsRoot.ActiveRoot);
			if (!fusionStatisticsRoot)
			{
				int childCount = globalStatisticsCanvas.transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					if (globalStatisticsCanvas.transform.GetChild(i).TryGetComponent<FusionStatisticsRoot>(out var component2) && component2.Statistics != component)
					{
						fusionStatisticsRoot = component2;
					}
				}
			}
			if (!fusionStatisticsRoot)
			{
				Object.Destroy(globalStatisticsCanvas.gameObject);
			}
			else
			{
				FusionStatisticsRoot.SetActiveRoot(fusionStatisticsRoot);
			}
		}

		public static void SetStatisticsWorldAnchor(this NetworkRunner runner, Transform anchor)
		{
			Canvas globalStatisticsCanvas = FusionStatistics.GlobalStatisticsCanvas;
			if ((bool)globalStatisticsCanvas)
			{
				globalStatisticsCanvas.transform.SetParent(anchor, worldPositionStays: false);
				CanvasScaler component = globalStatisticsCanvas.GetComponent<CanvasScaler>();
				if (!anchor)
				{
					CanvasCreator.SetCanvasToScreenSpace(globalStatisticsCanvas, component);
				}
				else
				{
					CanvasCreator.SetCanvasToWorldSpace(globalStatisticsCanvas, component);
				}
			}
		}
	}
}
