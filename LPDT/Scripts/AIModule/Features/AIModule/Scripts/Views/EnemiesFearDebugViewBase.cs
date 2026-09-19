using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.AIModule.Scripts.Views
{
	public class EnemiesFearDebugViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public Button FearAllEnemiesButton { get; private set; }

		[field: SerializeField]
		public Button FearEnemiesButton { get; private set; }

		[field: SerializeField]
		public Button FearEnemyButton { get; private set; }

		[field: SerializeField]
		public TMP_Dropdown FearEnemyDropdown { get; private set; }

		[field: SerializeField]
		public TMP_Dropdown FearEnemiesDropdown { get; private set; }
	}
}
