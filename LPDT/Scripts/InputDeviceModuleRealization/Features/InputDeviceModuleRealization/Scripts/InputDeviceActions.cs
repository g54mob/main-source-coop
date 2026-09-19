using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;
using UnityEngine.InputSystem;

namespace Features.InputDeviceModuleRealization.Scripts
{
	public class InputDeviceActions : IInputDeviceActions
	{
		private readonly LocalInputActions _localInputActions;

		public InputActionAsset asset => _localInputActions.asset;

		public InputDeviceActions(LocalInputActions localInputActions)
		{
			_localInputActions = localInputActions;
		}

		public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
		{
			return _localInputActions.FindAction(actionNameOrId, throwIfNotFound);
		}
	}
}
