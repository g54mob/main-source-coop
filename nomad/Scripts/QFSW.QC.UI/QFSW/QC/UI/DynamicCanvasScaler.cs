using UnityEngine;
using UnityEngine.UI;

namespace QFSW.QC.UI
{
	[ExecuteInEditMode]
	public class DynamicCanvasScaler : MonoBehaviour
	{
		[Range(0.5f, 2f)]
		[SerializeField]
		private float _rectMagnification = 1f;

		[Range(0.5f, 2f)]
		[SerializeField]
		private float _zoomMagnification = 1f;

		[SerializeField]
		private CanvasScaler _scaler;

		[SerializeField]
		private RectTransform _uiRoot;

		[SerializeField]
		private Vector2 _referenceResolution = new Vector2(1920f, 1080f);

		private float _lastScaler;

		public float RectMagnification
		{
			get
			{
				return _rectMagnification;
			}
			set
			{
				if (value > 0f)
				{
					_rectMagnification = value;
				}
			}
		}

		public float ZoomMagnification
		{
			get
			{
				return _zoomMagnification;
			}
			set
			{
				if (value > 0f)
				{
					_zoomMagnification = value;
				}
			}
		}

		private float RootScaler => _rectMagnification / _zoomMagnification;

		private void OnEnable()
		{
			_lastScaler = RootScaler;
		}

		private void Update()
		{
			if ((bool)_scaler && (bool)_uiRoot && RootScaler != _lastScaler)
			{
				Rect rect = new Rect(_uiRoot.offsetMin.x / _lastScaler, _uiRoot.offsetMin.y / _lastScaler, _uiRoot.offsetMax.x / _lastScaler, _uiRoot.offsetMax.y / _lastScaler);
				_lastScaler = RootScaler;
				_scaler.referenceResolution = _referenceResolution / _zoomMagnification;
				_uiRoot.offsetMin = new Vector2(rect.x, rect.y) * RootScaler;
				_uiRoot.offsetMax = new Vector2(rect.width, rect.height) * RootScaler;
			}
		}
	}
}
