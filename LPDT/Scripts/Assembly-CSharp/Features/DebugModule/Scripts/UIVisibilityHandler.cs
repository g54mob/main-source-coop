using UnityEngine;

namespace Features.DebugModule.Scripts
{
	public class UIVisibilityHandler : MonoBehaviour
	{
		private bool _isUiVisible = true;

		private Canvas[] _cachedCanvases;

		private void Update()
		{
			if (!Debug.isDebugBuild || !Input.GetKeyDown(KeyCode.F11))
			{
				return;
			}
			_isUiVisible = !_isUiVisible;
			if (!_isUiVisible)
			{
				_cachedCanvases = Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None);
			}
			Canvas[] cachedCanvases = _cachedCanvases;
			foreach (Canvas canvas in cachedCanvases)
			{
				if (!(canvas == null))
				{
					canvas.enabled = _isUiVisible;
				}
			}
		}
	}
}
