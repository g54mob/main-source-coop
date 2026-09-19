using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.PlayerStatesModule.Scripts.Views
{
	public abstract class PlayerStatesDebugViewBase : ViewBehaviour
	{
		[field: SerializeField]
		public Toggle FreeFlyStateToggle { get; private set; }
	}
}
