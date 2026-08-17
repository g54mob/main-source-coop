using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadedTechnology.WindshieldRainAsset.Demo
{
	public class HideHUD : MonoBehaviour
	{
		public GameObject canvas;

		private bool canvasEnabled = true;

		public void OnHideHUDKey(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				canvasEnabled = !canvasEnabled;
				canvas.SetActive(canvasEnabled);
			}
		}
	}
}
