using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.DeadPartsModule.Scripts.Views
{
	public abstract class PlayerResurrectionDebugViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public Button ResurrectionButton { get; private set; }
	}
}
