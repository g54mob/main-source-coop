using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.DebugModule.Scripts
{
	public abstract class EnemyDespawnDebugViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public Button DespawnEnemiesButton { get; private set; }
	}
}
