using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QFSW.QC.UI
{
	[ExecuteInEditMode]
	public class ZoomUIController : MonoBehaviour
	{
		[SerializeField]
		private float _zoomIncrement = 0.1f;

		[SerializeField]
		private float _minZoom = 0.1f;

		[SerializeField]
		private float _maxZoom = 2f;

		[SerializeField]
		private Button _zoomDownBtn;

		[SerializeField]
		private Button _zoomUpBtn;

		[SerializeField]
		private DynamicCanvasScaler _scaler;

		[SerializeField]
		private QuantumConsole _quantumConsole;

		[SerializeField]
		private TextMeshProUGUI _text;

		private float _lastZoom = -1f;

		private float ClampAndSnapZoom(float zoom)
		{
			return Mathf.Round(Mathf.Min(_maxZoom, Mathf.Max(_minZoom, zoom)) / _zoomIncrement) * _zoomIncrement;
		}

		public void ZoomUp()
		{
			_scaler.ZoomMagnification = ClampAndSnapZoom(_scaler.ZoomMagnification + _zoomIncrement);
		}

		public void ZoomDown()
		{
			_scaler.ZoomMagnification = ClampAndSnapZoom(_scaler.ZoomMagnification - _zoomIncrement);
		}

		private void Update()
		{
			if ((bool)_quantumConsole && (bool)_quantumConsole.KeyConfig)
			{
				if (_quantumConsole.KeyConfig.ZoomInKey.IsPressed())
				{
					ZoomUp();
				}
				if (_quantumConsole.KeyConfig.ZoomOutKey.IsPressed())
				{
					ZoomDown();
				}
			}
		}

		private void LateUpdate()
		{
			if ((bool)_scaler && (bool)_text)
			{
				float zoomMagnification = _scaler.ZoomMagnification;
				if (zoomMagnification != _lastZoom)
				{
					_lastZoom = zoomMagnification;
					int num = Mathf.RoundToInt(100f * zoomMagnification);
					_text.text = $"{num}%";
				}
			}
			if ((bool)_zoomDownBtn)
			{
				_zoomDownBtn.interactable = _lastZoom > _minZoom;
			}
			if ((bool)_zoomUpBtn)
			{
				_zoomUpBtn.interactable = _lastZoom < _maxZoom;
			}
		}
	}
}
