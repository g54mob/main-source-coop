using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.PlayersStatisticsModule.Scripts.Views.SpawnedNewItem
{
	public abstract class SpawnedNewItemViewBase : ViewBehaviour
	{
		public abstract void SetImage(Sprite sprite);

		public abstract void SetVisible(bool visible);
	}
}
