using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.QuotaModule.Scripts.Views
{
	public abstract class QuotaDebugViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public Button AddQuotaButton { get; private set; }

		[field: SerializeField]
		public Button RemoveQuotaButton { get; private set; }
	}
}
