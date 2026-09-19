using UnityEngine.InputSystem;

namespace RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts
{
	public interface IInputDeviceActions
	{
		InputActionAsset asset { get; }

		InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false);
	}
}
