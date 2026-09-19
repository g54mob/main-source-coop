using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Serialization;

namespace RSG.ThirdPerson.JoystickModule
{
	public abstract class JoystickBase : OnScreenControl, IPointerDownHandler, IEventSystemHandler, IDragHandler, IPointerUpHandler
	{
		[Serializable]
		public class Settings
		{
			public float HandleRange = 1f;

			public float DeadZone;

			public AxisOptions AxisOptions;

			public bool SnapX;

			public bool SnapY;
		}

		[SerializeField]
		protected Settings _settings;

		[SerializeField]
		[InputControl(layout = "Vector2")]
		private string _nameOfAction;

		[FormerlySerializedAs("background")]
		[SerializeField]
		protected RectTransform _background;

		[FormerlySerializedAs("handle")]
		[SerializeField]
		protected RectTransform _handle;

		private RectTransform _baseRect;

		private Canvas _canvas;

		private Camera _camera;

		private Vector2 _internalInput = Vector2.zero;

		private Vector2 Input
		{
			get
			{
				return _internalInput;
			}
			set
			{
				_internalInput = value;
				SendValueToControl(_internalInput);
			}
		}

		public float Horizontal
		{
			get
			{
				if (!_settings.SnapX)
				{
					return Input.x;
				}
				return SnapFloat(Input.x, AxisOptions.Horizontal);
			}
		}

		public float Vertical
		{
			get
			{
				if (!_settings.SnapY)
				{
					return Input.y;
				}
				return SnapFloat(Input.y, AxisOptions.Vertical);
			}
		}

		public Vector2 Direction => new Vector2(Horizontal, Vertical);

		public float HandleRange
		{
			get
			{
				return _settings.HandleRange;
			}
			set
			{
				_settings.HandleRange = Mathf.Abs(value);
			}
		}

		public float DeadZone
		{
			get
			{
				return _settings.DeadZone;
			}
			set
			{
				_settings.DeadZone = Mathf.Abs(value);
			}
		}

		public AxisOptions AxisOptions
		{
			get
			{
				return _settings.AxisOptions;
			}
			set
			{
				_settings.AxisOptions = value;
			}
		}

		public bool SnapX
		{
			get
			{
				return _settings.SnapX;
			}
			set
			{
				_settings.SnapX = value;
			}
		}

		public bool SnapY
		{
			get
			{
				return _settings.SnapY;
			}
			set
			{
				_settings.SnapY = value;
			}
		}

		private void Awake()
		{
			SetControlPathInternal();
		}

		protected virtual void Start()
		{
			WarmupSettings();
			InitDependencies();
			AdjustCenterOfHandle();
		}

		protected override void OnDisable()
		{
			_handle.anchoredPosition = Vector2.zero;
			base.OnDisable();
		}

		public void OnDrag(PointerEventData eventData)
		{
			PrepareCamera();
			Vector2 radius = GetRadius();
			SetInput(eventData, radius);
			FormatInput();
			HandleInput(Input.magnitude, Input.normalized, radius, _camera);
			SetHandleAnchoredPosition(radius);
		}

		public virtual void OnPointerDown(PointerEventData eventData)
		{
			OnDrag(eventData);
		}

		public virtual void OnPointerUp(PointerEventData eventData)
		{
			Input = Vector2.zero;
			_handle.anchoredPosition = Vector2.zero;
		}

		public void SetDependencies(Settings settings, Canvas canvas, Camera uiCamera)
		{
			_settings = settings;
			_canvas = canvas;
			_camera = uiCamera;
		}

		protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
		{
			if (magnitude > _settings.DeadZone)
			{
				if (magnitude > 1f)
				{
					Input = normalised;
				}
			}
			else
			{
				Input = Vector2.zero;
			}
		}

		protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
		{
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_baseRect, screenPosition, _camera, out var localPoint))
			{
				Vector2 vector = _baseRect.pivot * _baseRect.sizeDelta;
				return localPoint - _background.anchorMax * _baseRect.sizeDelta + vector;
			}
			return Vector2.zero;
		}

		private void SetControlPathInternal()
		{
			controlPathInternal = _nameOfAction;
		}

		private Vector2 GetRadius()
		{
			return _background.sizeDelta / 2f;
		}

		private void SetHandleAnchoredPosition(Vector2 radius)
		{
			_handle.anchoredPosition = Input * radius * _settings.HandleRange;
		}

		private void SetInput(PointerEventData eventData, Vector2 radius)
		{
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(_camera, _background.position);
			Input = (eventData.position - vector) / (radius * _canvas.scaleFactor);
		}

		private void PrepareCamera()
		{
			_camera = null;
			if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				_camera = _canvas.worldCamera;
			}
		}

		private void AdjustCenterOfHandle()
		{
			Vector2 vector = new Vector2(0.5f, 0.5f);
			_background.pivot = vector;
			_handle.anchorMin = vector;
			_handle.anchorMax = vector;
			_handle.pivot = vector;
			_handle.anchoredPosition = Vector2.zero;
		}

		private void InitDependencies()
		{
			_baseRect = GetComponent<RectTransform>();
			_canvas = GetComponentInParent<Canvas>();
			if (_canvas == null)
			{
				Debug.LogError("The Joystick is not placed inside a canvas");
			}
		}

		private void WarmupSettings()
		{
			HandleRange = _settings.HandleRange;
			DeadZone = _settings.DeadZone;
		}

		private void FormatInput()
		{
			if (_settings.AxisOptions == AxisOptions.Horizontal)
			{
				Input = new Vector2(Input.x, 0f);
			}
			else if (_settings.AxisOptions == AxisOptions.Vertical)
			{
				Input = new Vector2(0f, Input.y);
			}
		}

		private float SnapFloat(float value, AxisOptions snapAxis)
		{
			if (value == 0f)
			{
				return value;
			}
			if (_settings.AxisOptions == AxisOptions.Both)
			{
				return CalculateBothAxisOptions(value, snapAxis);
			}
			if (value > 0f)
			{
				return 1f;
			}
			if (value < 0f)
			{
				return -1f;
			}
			return 0f;
		}

		private float CalculateBothAxisOptions(float value, AxisOptions snapAxis)
		{
			float num = Vector2.Angle(Input, Vector2.up);
			switch (snapAxis)
			{
			case AxisOptions.Horizontal:
				if (num < 22.5f || num > 157.5f)
				{
					return 0f;
				}
				return (value > 0f) ? 1 : (-1);
			default:
				return value;
			case AxisOptions.Vertical:
				if (num > 67.5f && num < 112.5f)
				{
					return 0f;
				}
				return (value > 0f) ? 1 : (-1);
			}
		}
	}
}
