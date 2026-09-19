using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.DebugModule.Scripts
{
	public abstract class DamageableDebugViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public Button DamageButton { get; private set; }

		[field: SerializeField]
		public Button SetHealthTo10000Button { get; private set; }

		[field: SerializeField]
		public Button SetStaminaTo10000Button { get; private set; }

		[field: SerializeField]
		public TMP_InputField HealthAmountInputField { get; private set; }
	}
}
