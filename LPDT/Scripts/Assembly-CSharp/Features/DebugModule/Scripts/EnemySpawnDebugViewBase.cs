using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.DebugModule.Scripts
{
	public abstract class EnemySpawnDebugViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public TMP_Dropdown EnemyDropdown { get; private set; }

		[field: SerializeField]
		public Button SpawnEnemyButton { get; private set; }

		[field: SerializeField]
		public Button SpawnEnemyNearPlayerButton { get; private set; }
	}
}
