using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.NetworkInputModule.Scripts
{
	public abstract class InputIconHoldViewBase : ViewBehaviour
	{
		[SerializeField]
		protected InputActionReference _inputAction;
	}
}
