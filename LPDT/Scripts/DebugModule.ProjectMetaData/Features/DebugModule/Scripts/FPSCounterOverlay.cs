using TMPro;
using UnityEngine;

namespace Features.DebugModule.Scripts
{
	public class FPSCounterOverlay : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _fpsCounterText;

		[SerializeField]
		private float _checkInterval = 0.5f;

		private float _accumulatedTime;

		private int _frameCount;

		private void Update()
		{
			_accumulatedTime += Time.unscaledDeltaTime;
			_frameCount++;
			if (_accumulatedTime >= _checkInterval)
			{
				float f = (float)_frameCount / _accumulatedTime;
				if ((Object)(object)_fpsCounterText != null)
				{
					_fpsCounterText.text = $"FPS: {Mathf.CeilToInt(f)}";
				}
				_accumulatedTime = 0f;
				_frameCount = 0;
			}
		}
	}
}
