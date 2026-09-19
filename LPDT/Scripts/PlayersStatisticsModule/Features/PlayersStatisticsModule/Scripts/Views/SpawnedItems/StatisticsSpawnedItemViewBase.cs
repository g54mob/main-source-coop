using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.PlayersStatisticsModule.Scripts.Views.SpawnedItems
{
	public abstract class StatisticsSpawnedItemViewBase : ViewBehaviour
	{
		public abstract void SetIcon(Sprite icon);

		public abstract void SetCount(int count);
	}
}
