using System;
using System.Collections.Generic;
using Rewired.ComponentControls.Data;
using Rewired.Internal;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rewired.ComponentControls
{
	[Serializable]
	[RequireComponent(typeof(Image))]
	[DisallowMultipleComponent]
	[AddComponentMenu("Rewired/Touch Pad")]
	public sealed class TouchPad : TouchInteractable, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		public enum AxisDirection
		{
			Both = 0,
			Horizontal = 1,
			Vertical = 2
		}

		public enum TouchPadMode
		{
			Delta = 0,
			ScreenPosition = 1,
			VectorFromCenter = 2,
			VectorFromInitialTouch = 3
		}

		public enum ValueFormat
		{
			Pixels = 0,
			Screen = 1,
			Physical = 2,
			Direction = 3
		}

		private class GcbCkzFgdspYvXjGAvrdBxvawKnQ
		{
			private class nKkGWGseMmfMqOWCbUnSeXqiccpH
			{
				public float MoPTGRsFgXdMwTraXsxZonteyAEm;

				public float eBUQwXOmrrriDgdoiVUdsZqKsHQo;

				public uint XhKxIzDXGwEJghbHIgWHJwLzJsmD;
			}

			private int BhpPNlwkatLKIqHayHczxNXLLkSE;

			private nKkGWGseMmfMqOWCbUnSeXqiccpH[] cnRAHhGwbBYXolTSTEoaPYuDorOJA;

			private int aVOlNuRVvCOJdZFGeiVVgfpKNgOsA = -1;

			public GcbCkzFgdspYvXjGAvrdBxvawKnQ(int P_0)
			{
				if (P_0 < 2)
				{
					throw new ArgumentOutOfRangeException("maxSmoothFrames must be >= 2");
				}
				BhpPNlwkatLKIqHayHczxNXLLkSE = P_0;
				cnRAHhGwbBYXolTSTEoaPYuDorOJA = new nKkGWGseMmfMqOWCbUnSeXqiccpH[P_0];
				ArrayTools.Populate(cnRAHhGwbBYXolTSTEoaPYuDorOJA);
			}

			public void tzfYrQaBdsWHACSbQzIxoLZqqtry(float P_0, float P_1)
			{
				uint currentFrame = ReInput.currentFrame;
				if (aVOlNuRVvCOJdZFGeiVVgfpKNgOsA < 0 || cnRAHhGwbBYXolTSTEoaPYuDorOJA[aVOlNuRVvCOJdZFGeiVVgfpKNgOsA].XhKxIzDXGwEJghbHIgWHJwLzJsmD != currentFrame)
				{
					vvrKfKclDpgNvELvDxraZuIuCBnL();
					nKkGWGseMmfMqOWCbUnSeXqiccpH obj = cnRAHhGwbBYXolTSTEoaPYuDorOJA[aVOlNuRVvCOJdZFGeiVVgfpKNgOsA];
					obj.MoPTGRsFgXdMwTraXsxZonteyAEm = P_0;
					obj.eBUQwXOmrrriDgdoiVUdsZqKsHQo = P_1;
					obj.XhKxIzDXGwEJghbHIgWHJwLzJsmD = currentFrame;
				}
			}

			public Vector2 tsMpNUyOXnUQVPjMscdXFFdEHcWC()
			{
				if (aVOlNuRVvCOJdZFGeiVVgfpKNgOsA < 0)
				{
					return default(Vector2);
				}
				int num = aVOlNuRVvCOJdZFGeiVVgfpKNgOsA;
				nKkGWGseMmfMqOWCbUnSeXqiccpH nKkGWGseMmfMqOWCbUnSeXqiccpH2 = cnRAHhGwbBYXolTSTEoaPYuDorOJA[num];
				Vector2 result = new Vector2(nKkGWGseMmfMqOWCbUnSeXqiccpH2.MoPTGRsFgXdMwTraXsxZonteyAEm, nKkGWGseMmfMqOWCbUnSeXqiccpH2.eBUQwXOmrrriDgdoiVUdsZqKsHQo);
				uint xhKxIzDXGwEJghbHIgWHJwLzJsmD = nKkGWGseMmfMqOWCbUnSeXqiccpH2.XhKxIzDXGwEJghbHIgWHJwLzJsmD;
				int num2 = num;
				int num3 = 1;
				while ((num2 = gmaTZqwvDVelmvFlFCVPFwLnNJLZ(num2, BhpPNlwkatLKIqHayHczxNXLLkSE)) != num)
				{
					nKkGWGseMmfMqOWCbUnSeXqiccpH nKkGWGseMmfMqOWCbUnSeXqiccpH3 = cnRAHhGwbBYXolTSTEoaPYuDorOJA[num2];
					if (!qsQzTOVMTjZtCEykQHzvcpAEPnCq(nKkGWGseMmfMqOWCbUnSeXqiccpH3.XhKxIzDXGwEJghbHIgWHJwLzJsmD, xhKxIzDXGwEJghbHIgWHJwLzJsmD))
					{
						break;
					}
					result.x += nKkGWGseMmfMqOWCbUnSeXqiccpH3.MoPTGRsFgXdMwTraXsxZonteyAEm;
					result.y += nKkGWGseMmfMqOWCbUnSeXqiccpH3.eBUQwXOmrrriDgdoiVUdsZqKsHQo;
					xhKxIzDXGwEJghbHIgWHJwLzJsmD = nKkGWGseMmfMqOWCbUnSeXqiccpH3.XhKxIzDXGwEJghbHIgWHJwLzJsmD;
					num3++;
				}
				if (num3 > 0)
				{
					result.x /= num3;
					result.y /= num3;
				}
				return result;
			}

			private void vvrKfKclDpgNvELvDxraZuIuCBnL()
			{
				aVOlNuRVvCOJdZFGeiVVgfpKNgOsA = HFpRVJKckAeAyUfLgKvrQLbobKpd(aVOlNuRVvCOJdZFGeiVVgfpKNgOsA, BhpPNlwkatLKIqHayHczxNXLLkSE);
			}

			private static int HFpRVJKckAeAyUfLgKvrQLbobKpd(int P_0, int P_1)
			{
				if (P_0 >= P_1 - 1)
				{
					return 0;
				}
				return ++P_0;
			}

			private int gmaTZqwvDVelmvFlFCVPFwLnNJLZ(int P_0, int P_1)
			{
				if (P_0 > 0)
				{
					return --P_0;
				}
				return P_1 - 1;
			}

			private static bool qsQzTOVMTjZtCEykQHzvcpAEPnCq(uint P_0, uint P_1)
			{
				if (P_1 == 0)
				{
					return P_0 == uint.MaxValue;
				}
				return P_0 == P_1 - 1;
			}
		}

		[Serializable]
		public class ValueChangedEventHandler : UnityEvent<Vector2>
		{
		}

		[Serializable]
		public class TapEventHandler : UnityEvent
		{
		}

		[Serializable]
		public class PressDownEventHandler : UnityEvent
		{
		}

		[Serializable]
		public class PressUpEventHandler : UnityEvent
		{
		}

		private const int SMOOTH_DELTA_FRAME_COUNT = 3;

		[Tooltip("The Custom Controller element that will receive input values from the touch pad's X axis.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CustomControllerElementTargetSetForFloat _horizontalAxisCustomControllerElement = new CustomControllerElementTargetSetForFloat();

		[Tooltip("The Custom Controller element that will receive input values from the touch pad's Y axis.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CustomControllerElementTargetSetForFloat _verticalAxisCustomControllerElement = new CustomControllerElementTargetSetForFloat();

		[Tooltip("The Custom Controller element that will receive input values from touch pad taps.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CustomControllerElementTargetSetForBoolean _tapCustomControllerElement = new CustomControllerElementTargetSetForBoolean();

		[Tooltip("The Custom Controller element that will receive input values from touch pad presses.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CustomControllerElementTargetSetForBoolean _pressCustomControllerElement = new CustomControllerElementTargetSetForBoolean();

		[CustomObfuscation(rename = false)]
		[Tooltip("The axis directions in which movement is allowed. You can restrict movement to one or both axes.")]
		[SerializeField]
		private AxisDirection _axesToUse;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("The mode of the touch pad.\n\nDelta - Returns the change in position of the touch from the previous to the current frame.\n\nScreen Position - Returns the absolute position of the touch  on the screen.\n\nVector From Center - Returns a vector from the center of the Touch Pad to the current touch position.\n\nVector From Initial Touch - Returns a vector from the intial touch position to the current touch position.")]
		private TouchPadMode _touchPadMode;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("The format of the resulting data generated by the touch pad.\n\nPixels - Screen pixels.\n\nScreen - The proportion of the value to screen size in the corresponding dimension. 1 unit = 1 screen length (width for X, height for Y).\n\nPhysical - 1 unit = 1/100th of an inch. The resulting value will be consistent across different screen resolutions and sizes. IMPORTANT: This relies on the value returned by UnityEngine.Screen.dpi. If the device does not return a value, a reference resolution of 96 dpi will be used.\n\nDirection - A normalized direction vector.")]
		private ValueFormat _valueFormat;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("If enabled, when swiped and released, the value will slowly fall toward zero based on the Friction value. This only has an effect if Touch Pad Mode is set to Position Delta.")]
		private bool _useInertia;

		[CustomObfuscation(rename = false)]
		[FieldRange(0f, float.MaxValue)]
		[SerializeField]
		[Tooltip("Determines how quickly a swipe value will fall toward zero when Use Inertia is enabled.")]
		private float _inertiaFriction = 3f;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("If true, the touch pad can be activated by a touch swipe that began in an area outside the touch pad region. If false, the touch pad can only be activated by a direct touch.")]
		private bool _activateOnSwipeIn;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("If true, the touch pad will stay engaged even if the touch that activated it moves outside the touch pad region. If false, the touch pad will be released once the touch that activated it moves outside the touch pad region.")]
		private bool _stayActiveOnSwipeOut = true;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Should taps on the touch pad be processed?")]
		private bool _allowTap;

		[CustomObfuscation(rename = false)]
		[FieldRange(0f, float.MaxValue)]
		[SerializeField]
		[Tooltip("The maximum touch duration allowed for the touch to be considered a tap. A touch that lasts longer than this value will not trigger a tap when released.")]
		private float _tapTimeout = 0.25f;

		[CustomObfuscation(rename = false)]
		[FieldRange(-1, int.MaxValue)]
		[SerializeField]
		[Tooltip("The maximum movement distance allowed in pixels since the touch began for the touch to be considered a tap. [-1 = no limit]")]
		private int _tapDistanceLimit = 10;

		[CustomObfuscation(rename = false)]
		[Tooltip("Should presses (continual press like a button) on the touch pad be processed?")]
		[SerializeField]
		private bool _allowPress;

		[SerializeField]
		[Tooltip("Time the touch pad must be touched before it will be considered a press.")]
		[CustomObfuscation(rename = false)]
		private float _pressStartDelay = 0.1f;

		[CustomObfuscation(rename = false)]
		[FieldRange(-1, int.MaxValue)]
		[SerializeField]
		[Tooltip("The maximum movement distance allowed in pixels since the touch began for the touch to be considered a press. Any movement beyond this value will cancel the press. [-1 = no limit]")]
		private int _pressDistanceLimit = 10;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("If enabled, the control will be hidden when gameplay starts.")]
		private bool _hideAtRuntime;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("The underlying Axis 2D.")]
		private StandaloneAxis2D _axis2D = StandaloneAxis2D.CreateRelative();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Event sent when the value changes.")]
		private ValueChangedEventHandler _onValueChanged = new ValueChangedEventHandler();

		[SerializeField]
		[Tooltip("Event sent when the touch pad is tapped. This event will only be sent if allowTap is True.")]
		[CustomObfuscation(rename = false)]
		private TapEventHandler _onTap = new TapEventHandler();

		[SerializeField]
		[Tooltip("Event sent when the touch pad is initally pressed. This event is for the Press button simulation which must be enabled by setting Press Allowed to True. This event will only be sent if allowPress is True.")]
		[CustomObfuscation(rename = false)]
		private PressDownEventHandler _onPressDown = new PressDownEventHandler();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("Event sent when the touch pad is released after a press. This event is for the Press button simulation which must be enabled by setting Press Allowed to True. This event will only be sent if allowPress is True.")]
		private PressUpEventHandler _onPressUp = new PressUpEventHandler();

		private bool _useXAxis;

		private bool _useYAxis;

		private int _pointerId = int.MinValue;

		private int _realMousePointerId = int.MinValue;

		[NonSerialized]
		private bool ALBJgfsRYJdMVJwePTiUwIfQxDNM;

		[NonSerialized]
		private bool zmNsczlnaJKPNGfDEeyyFdsXhpjgb;

		private bool _pointerDownIsFake;

		private Vector2 _touchStartPosition;

		private float _touchStartTime;

		private Vector3 _currentCenter;

		private Vector2 _previousTouchPosition;

		private int _lastTapFrame = -1;

		private bool _isEligibleForTap;

		private bool _isEligibleForPress;

		private bool _pressValue;

		private GcbCkzFgdspYvXjGAvrdBxvawKnQ _smoothDelta = new GcbCkzFgdspYvXjGAvrdBxvawKnQ(3);

		private Dictionary<int, PointerEventData> __fakePointerEventData;

		public CustomControllerElementTargetSetForFloat horizontalAxisCustomControllerElement => _horizontalAxisCustomControllerElement;

		public CustomControllerElementTargetSetForFloat verticalAxisCustomControllerElement => _verticalAxisCustomControllerElement;

		public CustomControllerElementTargetSetForBoolean tapCustomControllerElement => _tapCustomControllerElement;

		public CustomControllerElementTargetSetForBoolean pressCustomControllerElement => _pressCustomControllerElement;

		public AxisDirection axesToUse
		{
			get
			{
				return _axesToUse;
			}
			set
			{
				if (_axesToUse != value)
				{
					GIBVHpzXHQWngXozSfKaKeeIRfeQ(value);
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public TouchPadMode touchPadMode
		{
			get
			{
				return _touchPadMode;
			}
			set
			{
				if (_touchPadMode != value)
				{
					_touchPadMode = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public ValueFormat valueFormat
		{
			get
			{
				return _valueFormat;
			}
			set
			{
				if (_valueFormat != value)
				{
					_valueFormat = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool useInertia
		{
			get
			{
				return _useInertia;
			}
			set
			{
				if (_useInertia != value)
				{
					_useInertia = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public float inertiaFriction
		{
			get
			{
				return _inertiaFriction;
			}
			set
			{
				value = MathTools.Max(0f, value);
				if (_inertiaFriction != value)
				{
					_inertiaFriction = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool activateOnSwipeIn
		{
			get
			{
				return _activateOnSwipeIn;
			}
			set
			{
				if (_activateOnSwipeIn != value)
				{
					_activateOnSwipeIn = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool stayActiveOnSwipeOut
		{
			get
			{
				return _stayActiveOnSwipeOut;
			}
			set
			{
				if (_stayActiveOnSwipeOut != value)
				{
					_stayActiveOnSwipeOut = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool allowTap
		{
			get
			{
				return _allowTap;
			}
			set
			{
				if (_allowTap != value)
				{
					_allowTap = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public float tapTimeout
		{
			get
			{
				return _tapTimeout;
			}
			set
			{
				value = MathTools.Max(0f, value);
				if (_tapTimeout != value)
				{
					_tapTimeout = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public int tapDistanceLimit
		{
			get
			{
				return _tapDistanceLimit;
			}
			set
			{
				value = MathTools.Max(-1, value);
				if (_tapDistanceLimit != value)
				{
					_tapDistanceLimit = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool allowPress
		{
			get
			{
				return _allowPress;
			}
			set
			{
				if (_allowPress != value)
				{
					_allowPress = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public float pressStartDelay
		{
			get
			{
				return _pressStartDelay;
			}
			set
			{
				value = Mathf.Max(0f, value);
				if (_pressStartDelay != value)
				{
					_pressStartDelay = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public int pressDistanceLimit
		{
			get
			{
				return _pressDistanceLimit;
			}
			set
			{
				value = MathTools.Max(-1, value);
				if (_pressDistanceLimit != value)
				{
					_pressDistanceLimit = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool hideAtRuntime
		{
			get
			{
				return _hideAtRuntime;
			}
			set
			{
				if (!(_hideAtRuntime = value))
				{
					_hideAtRuntime = true;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public int pointerId
		{
			get
			{
				return _pointerId;
			}
			set
			{
				_pointerId = value;
			}
		}

		public bool hasPointer => _pointerId != int.MinValue;

		public Vector2 touchStartPosition
		{
			get
			{
				if (!hasPointer)
				{
					return Vector2.zero;
				}
				return _touchStartPosition;
			}
		}

		public Vector2 touchPosition
		{
			get
			{
				if (!TouchInteractable.uwuMHSNgPoeyIZKBbzpvCZyfdwOl(RdKVxosdmPbtxnVbOZiHUHSPKFAe))
				{
					return Vector2.zero;
				}
				return TouchInteractable.TgEgBYQvFnhTTkcyuYMooLsCFzzE(RdKVxosdmPbtxnVbOZiHUHSPKFAe);
			}
		}

		public AxisCalibration horizontalAxisCalibration => _axis2D.xAxis.calibration;

		public AxisCalibration verticalAxisCalibration => _axis2D.yAxis.calibration;

		public Axis2DCalibration axis2DCalibration => _axis2D.calibration;

		internal StandaloneAxis2D ACDlWRYqRUOGCISCfVnFWZBaWOxV => _axis2D;

		private int RdKVxosdmPbtxnVbOZiHUHSPKFAe
		{
			get
			{
				if (_pointerId == int.MinValue)
				{
					return int.MinValue;
				}
				if (_realMousePointerId != int.MinValue)
				{
					return _realMousePointerId;
				}
				return _pointerId;
			}
		}

		private bool yjikdyNyZFDrkcdNNgEgZYvwGHKQ => _lastTapFrame == Time.frameCount;

		public event UnityAction<Vector2> ValueChangedEvent
		{
			add
			{
				_onValueChanged.AddListener(value);
			}
			remove
			{
				_onValueChanged.RemoveListener(value);
			}
		}

		public event UnityAction TapEvent
		{
			add
			{
				_onTap.AddListener(value);
			}
			remove
			{
				_onTap.RemoveListener(value);
			}
		}

		public event UnityAction PressDownEvent
		{
			add
			{
				_onPressDown.AddListener(value);
			}
			remove
			{
				_onPressDown.RemoveListener(value);
			}
		}

		public event UnityAction PressUpEvent
		{
			add
			{
				_onPressUp.AddListener(value);
			}
			remove
			{
				_onPressUp.RemoveListener(value);
			}
		}

		[CustomObfuscation(rename = false)]
		private TouchPad()
		{
		}

		[CustomObfuscation(rename = false)]
		internal override void Awake()
		{
			base.Awake();
			if (Application.isPlaying && _hideAtRuntime)
			{
				base.visible = false;
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnValidate()
		{
			base.OnValidate();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				GxzFEFeJIqsFFABrybfdpQNfDFNi();
				khkdMwnTAvlDqPvCkDAZhRJFGYZR();
			}
		}

		internal override bool lpOPYPkfRAdylCMSLphTlIgUWIWy()
		{
			if (!base.lpOPYPkfRAdylCMSLphTlIgUWIWy())
			{
				return false;
			}
			GxzFEFeJIqsFFABrybfdpQNfDFNi();
			return true;
		}

		internal override void ZCGETbjMQZUkyflRtYAqwQNUBPQIb()
		{
			base.ZCGETbjMQZUkyflRtYAqwQNUBPQIb();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				uiycTgIPBbGghVXEDqOPpxkdfEhq();
				nxTbtLCLjbyuyRGfsvxPBPMmlZRq();
				iriXtBpUKxiZfHBywtkhLeFWPDxr();
				togwggFTtwmzGOzMQNndhLBrYoJk();
				aWUWpIcCIgATqZRkbFWrLxrwjoEo();
			}
		}

		internal override void xhHhPOcavYUrBJGXQmMDzQMMGISaA()
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && osKcqUcyYlVGlGygpMaOnUYNqJDBA)
			{
				Vector2 vector = ((_touchPadMode == TouchPadMode.ScreenPosition) ? _axis2D.rawValue : _axis2D.value);
				if (_useXAxis)
				{
					SPXsRbvylxmzyfkxsgnXmtaaKOyb(_horizontalAxisCustomControllerElement, vector.x, _axis2D.xAxis.buttonActivationThreshold);
				}
				if (_useYAxis)
				{
					SPXsRbvylxmzyfkxsgnXmtaaKOyb(_verticalAxisCustomControllerElement, vector.y, _axis2D.xAxis.buttonActivationThreshold);
				}
				if (_allowTap)
				{
					SPXsRbvylxmzyfkxsgnXmtaaKOyb(_tapCustomControllerElement, yjikdyNyZFDrkcdNNgEgZYvwGHKQ);
				}
				if (_allowPress)
				{
					SPXsRbvylxmzyfkxsgnXmtaaKOyb(_pressCustomControllerElement, _pressValue);
				}
			}
		}

		internal override void FyKKzlnIjsJaSimYwuqKSwNJHIHx()
		{
			base.FyKKzlnIjsJaSimYwuqKSwNJHIHx();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				GxzFEFeJIqsFFABrybfdpQNfDFNi();
				khkdMwnTAvlDqPvCkDAZhRJFGYZR();
			}
		}

		internal override void vXIboQFSGKdCLZnWmauizJnaOLuX()
		{
			base.vXIboQFSGKdCLZnWmauizJnaOLuX();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				_pointerId = int.MinValue;
				_realMousePointerId = int.MinValue;
				ALBJgfsRYJdMVJwePTiUwIfQxDNM = false;
				zmNsczlnaJKPNGfDEeyyFdsXhpjgb = false;
				_pointerDownIsFake = false;
				_currentCenter = Vector2.zero;
				_previousTouchPosition = Vector2.zero;
				_axis2D.Clear();
				_lastTapFrame = -1;
				_pressValue = false;
				_isEligibleForTap = false;
				_isEligibleForPress = false;
			}
		}

		public override void ClearValue()
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				_axis2D.Clear();
				_lastTapFrame = -1;
				_pressValue = false;
				if (osKcqUcyYlVGlGygpMaOnUYNqJDBA)
				{
					base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ClearElementValue(_horizontalAxisCustomControllerElement);
					base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ClearElementValue(_verticalAxisCustomControllerElement);
					base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ClearElementValue(_tapCustomControllerElement);
				}
			}
		}

		private void khkdMwnTAvlDqPvCkDAZhRJFGYZR()
		{
			_horizontalAxisCustomControllerElement.ClearElementCaches();
			_verticalAxisCustomControllerElement.ClearElementCaches();
			_tapCustomControllerElement.ClearElementCaches();
			_pressCustomControllerElement.ClearElementCaches();
		}

		private void GxzFEFeJIqsFFABrybfdpQNfDFNi()
		{
			GIBVHpzXHQWngXozSfKaKeeIRfeQ(_axesToUse);
			if (osKcqUcyYlVGlGygpMaOnUYNqJDBA && base.gGeqnLRNKcjgzKaWVxWuqSNJvSze.useCustomController)
			{
				if (_useXAxis)
				{
					base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ValidateElements(_horizontalAxisCustomControllerElement);
				}
				if (_useYAxis)
				{
					base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ValidateElements(_verticalAxisCustomControllerElement);
				}
				if (_allowTap)
				{
					base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ValidateElements(_tapCustomControllerElement);
				}
				if (_allowPress)
				{
					base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ValidateElements(_pressCustomControllerElement);
				}
			}
		}

		private void GIBVHpzXHQWngXozSfKaKeeIRfeQ(AxisDirection P_0)
		{
			bool flag = P_0 == AxisDirection.Both || P_0 == AxisDirection.Horizontal;
			if (_useXAxis != flag)
			{
				_useXAxis = flag;
				if (!flag && osKcqUcyYlVGlGygpMaOnUYNqJDBA)
				{
					base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ClearElementValue(_horizontalAxisCustomControllerElement);
				}
			}
			bool flag2 = P_0 == AxisDirection.Both || P_0 == AxisDirection.Vertical;
			if (_useYAxis != flag2)
			{
				_useYAxis = flag2;
				if (!flag2 && osKcqUcyYlVGlGygpMaOnUYNqJDBA)
				{
					base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ClearElementValue(_verticalAxisCustomControllerElement);
				}
			}
			_axesToUse = P_0;
		}

		private void nxTbtLCLjbyuyRGfsvxPBPMmlZRq()
		{
			if (hasPointer && !TouchInteractable.uwuMHSNgPoeyIZKBbzpvCZyfdwOl(RdKVxosdmPbtxnVbOZiHUHSPKFAe))
			{
				PointerEventData pointerEventData = HyACTUHUMITHJlwRMGbSiKvZntNC(RdKVxosdmPbtxnVbOZiHUHSPKFAe);
				if (pointerEventData != null && pointerEventData.pointerPress != null)
				{
					LQXnlCGWVQCMAatUhwMXImJPGHhKA(pointerEventData);
				}
				else
				{
					MHcBbLUbNvVfJsmQBGwoxkdhumdt();
				}
			}
		}

		private void iriXtBpUKxiZfHBywtkhLeFWPDxr()
		{
			if (_touchPadMode == TouchPadMode.VectorFromCenter)
			{
				Graphic graphic = base.targetGraphic;
				RectTransform rectTransform = ((graphic != null) ? (graphic.transform as RectTransform) : base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA);
				_currentCenter = rectTransform.TransformPoint(rectTransform.rect.center);
				_currentCenter = RectTransformUtility.WorldToScreenPoint(base.vQpmvzIqZzQRgZjNdTCOIzNihLiH.worldCamera, _currentCenter);
			}
			if (!hasPointer || !TouchInteractable.uwuMHSNgPoeyIZKBbzpvCZyfdwOl(RdKVxosdmPbtxnVbOZiHUHSPKFAe))
			{
				return;
			}
			Vector3 vector = TouchInteractable.TgEgBYQvFnhTTkcyuYMooLsCFzzE(RdKVxosdmPbtxnVbOZiHUHSPKFAe);
			Vector2 vector2;
			if (_touchPadMode == TouchPadMode.ScreenPosition)
			{
				vector2 = vector;
			}
			else
			{
				if (_touchPadMode == TouchPadMode.Delta)
				{
					_currentCenter = _previousTouchPosition;
				}
				vector2 = new Vector2(vector.x - _currentCenter.x, vector.y - _currentCenter.y);
			}
			vector2 = gfuFxNXGHkBsWSHRMGHdjmvDlUbRA(vector2);
			_axis2D.SetRawValue(vector2.x, vector2.y);
			if (_touchPadMode == TouchPadMode.Delta)
			{
				_smoothDelta.tzfYrQaBdsWHACSbQzIxoLZqqtry(vector2.x, vector2.y);
			}
			_previousTouchPosition = vector;
		}

		private void togwggFTtwmzGOzMQNndhLBrYoJk()
		{
			if (_touchPadMode == TouchPadMode.Delta && _useInertia && !hasPointer)
			{
				Vector2 rawValue = _axis2D.rawValue;
				float smoothDeltaTime = Time.smoothDeltaTime;
				float num = Mathf.Lerp(rawValue.x, 0f, _inertiaFriction * smoothDeltaTime);
				float num2 = Mathf.Lerp(rawValue.y, 0f, _inertiaFriction * smoothDeltaTime);
				if (MathTools.IsNearZero(num, 0.0001f))
				{
					num = 0f;
				}
				if (MathTools.IsNearZero(num2, 0.0001f))
				{
					num2 = 0f;
				}
				_axis2D.SetRawValue(num, num2);
			}
		}

		private void uiycTgIPBbGghVXEDqOPpxkdfEhq()
		{
			if (hasPointer)
			{
				Vector2 vector = TouchInteractable.TgEgBYQvFnhTTkcyuYMooLsCFzzE(RdKVxosdmPbtxnVbOZiHUHSPKFAe);
				QKLGRQbsDyoftuuWluaENjTJJkboA(ref vector);
				xsFfBubpiFWpSGqQpCPifdUCjMgj(ref vector);
			}
		}

		private void QKLGRQbsDyoftuuWluaENjTJJkboA(ref Vector2 P_0)
		{
			if (_allowTap && _isEligibleForTap && ((_tapTimeout > 0f && Time.realtimeSinceStartup - _touchStartTime > _tapTimeout) || (_tapDistanceLimit >= 0 && Vector2.Distance(_touchStartPosition, P_0) > (float)_tapDistanceLimit)))
			{
				_isEligibleForTap = false;
			}
		}

		private void xsFfBubpiFWpSGqQpCPifdUCjMgj(ref Vector2 P_0)
		{
			if (_allowPress && _isEligibleForPress)
			{
				if (_pressDistanceLimit >= 0 && Vector2.Distance(_touchStartPosition, P_0) > (float)_pressDistanceLimit)
				{
					_isEligibleForPress = false;
					EiLFJmVuVmsQcybkTdMUeCmZTgWB(false);
				}
				else if (!(_pressStartDelay > 0f) || !(Time.realtimeSinceStartup - _touchStartTime < _pressStartDelay))
				{
					EiLFJmVuVmsQcybkTdMUeCmZTgWB(true);
				}
			}
		}

		private void aWUWpIcCIgATqZRkbFWrLxrwjoEo()
		{
			if (_touchPadMode == TouchPadMode.Delta)
			{
				Vector2 value = _axis2D.value;
				Vector2 valuePrev = _axis2D.valuePrev;
				if (value.x != 0f || value.y != 0f || valuePrev.x != 0f || valuePrev.y != 0f)
				{
					_onValueChanged.Invoke(_axis2D.value);
				}
			}
			else
			{
				Vector2 valueDelta = _axis2D.valueDelta;
				if (valueDelta.x != 0f || valueDelta.y != 0f)
				{
					_onValueChanged.Invoke(_axis2D.value);
				}
			}
		}

		private Vector2 gfuFxNXGHkBsWSHRMGHdjmvDlUbRA(Vector2 P_0)
		{
			switch (_valueFormat)
			{
			case ValueFormat.Screen:
				P_0.x /= Screen.width;
				P_0.y /= Screen.height;
				break;
			case ValueFormat.Physical:
			{
				float num = Screen.dpi;
				if (num < 10f)
				{
					num = 96f;
				}
				P_0 = P_0 / num * 100f;
				break;
			}
			case ValueFormat.Direction:
				P_0.Normalize();
				break;
			default:
				throw new NotImplementedException();
			case ValueFormat.Pixels:
				break;
			}
			return P_0;
		}

		private void EiLFJmVuVmsQcybkTdMUeCmZTgWB(bool P_0)
		{
			if (P_0 != _pressValue)
			{
				_pressValue = P_0;
				if (P_0)
				{
					_onPressDown.Invoke();
				}
				else
				{
					_onPressUp.Invoke();
				}
			}
		}

		private void kenMZNZStIQnTAavioStPdqUwyIF(PointerEventData P_0)
		{
			if (!hasPointer || SBhvinEbIDGWUMwfXythhrIpigKu(P_0.pointerId))
			{
				if (EDufGzVNigBlMAOWvMGsHZmtQaph() && IsInteractable())
				{
					PARBcfOJshIGNJtBDsuAyiETdYwy(P_0.pointerId, P_0.pressPosition);
				}
				base.OnPointerDown(P_0);
			}
		}

		private void ZEbUZdUWFIEfiSqeUnrGJHjcPybD(PointerEventData P_0)
		{
			if ((!hasPointer || SBhvinEbIDGWUMwfXythhrIpigKu(P_0.pointerId)) && !TouchInteractable.uwuMHSNgPoeyIZKBbzpvCZyfdwOl(RdKVxosdmPbtxnVbOZiHUHSPKFAe))
			{
				MHcBbLUbNvVfJsmQBGwoxkdhumdt();
				base.OnPointerUp(P_0);
			}
		}

		private void ZWAzvWwAMRawEGwPixAMPjfyqNgpA(PointerEventData P_0)
		{
			if (hasPointer && !SBhvinEbIDGWUMwfXythhrIpigKu(P_0.pointerId))
			{
				return;
			}
			bool flag = TouchInteractable.sWBkWrgBUhjXAreNdBTEqWiNGgHjA(P_0.pointerId);
			bool flag2 = false;
			if (_activateOnSwipeIn && EDufGzVNigBlMAOWvMGsHZmtQaph() && IsInteractable() && (!flag || TouchInteractable.LTWUvUGhphDXhnFmUjsOImULiiXRA(base.allowedMouseButtons)) && !ALBJgfsRYJdMVJwePTiUwIfQxDNM)
			{
				if (flag)
				{
					if (TouchInteractable.ZbAsyupCrOtBnZlFjdZxrpdrFStI(base.allowedMouseButtons, out var realMousePointerId))
					{
						_realMousePointerId = realMousePointerId;
					}
					else
					{
						_realMousePointerId = P_0.pointerId;
					}
				}
				flag2 = true;
			}
			base.OnPointerEnter(P_0);
			if (flag2)
			{
				GameObject gameObject = base.gameObject;
				PointerEventData pointerEventData = utfJPhgnwTqKykeFFJYQHUTouefi((_realMousePointerId != int.MinValue) ? _realMousePointerId : P_0.pointerId, gameObject);
				if (pointerEventData != null)
				{
					kenMZNZStIQnTAavioStPdqUwyIF(pointerEventData);
					if (ALBJgfsRYJdMVJwePTiUwIfQxDNM)
					{
						_pointerDownIsFake = true;
					}
				}
			}
			zmNsczlnaJKPNGfDEeyyFdsXhpjgb = true;
		}

		private void zHqZkjRbrFTkqMfkkoUpBAuhCzKfA(PointerEventData P_0)
		{
			if (hasPointer && !SBhvinEbIDGWUMwfXythhrIpigKu(P_0.pointerId))
			{
				base.OnPointerExit(P_0);
				return;
			}
			if (!stayActiveOnSwipeOut && ALBJgfsRYJdMVJwePTiUwIfQxDNM)
			{
				MHcBbLUbNvVfJsmQBGwoxkdhumdt();
			}
			base.OnPointerExit(P_0);
			zmNsczlnaJKPNGfDEeyyFdsXhpjgb = false;
		}

		private void PARBcfOJshIGNJtBDsuAyiETdYwy(int P_0, Vector2 P_1)
		{
			_pointerId = P_0;
			ALBJgfsRYJdMVJwePTiUwIfQxDNM = true;
			_isEligibleForTap = true;
			_isEligibleForPress = true;
			if (_touchPadMode != TouchPadMode.VectorFromCenter)
			{
				_currentCenter = P_1;
			}
			if (_touchPadMode == TouchPadMode.Delta)
			{
				_previousTouchPosition = P_1;
			}
			_touchStartTime = Time.realtimeSinceStartup;
			_touchStartPosition = P_1;
		}

		private void MHcBbLUbNvVfJsmQBGwoxkdhumdt()
		{
			bool num = _allowTap && _isEligibleForTap;
			rEkWtHXjAKTOKMkJBxWaofkAFFvH();
			ALBJgfsRYJdMVJwePTiUwIfQxDNM = false;
			if (_useInertia && _touchPadMode == TouchPadMode.Delta)
			{
				_axis2D.SetRawValue(_smoothDelta.tsMpNUyOXnUQVPjMscdXFFdEHcWC());
			}
			else
			{
				_axis2D.SetRawValue(0f, 0f);
			}
			EiLFJmVuVmsQcybkTdMUeCmZTgWB(false);
			_isEligibleForTap = false;
			_isEligibleForPress = false;
			if (num)
			{
				_lastTapFrame = Time.frameCount + 1;
				_onTap.Invoke();
			}
		}

		internal override void OnPointerUp(PointerEventData eventData)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, base.allowedMouseButtons, EventTriggerType.PointerUp))
			{
				ZEbUZdUWFIEfiSqeUnrGJHjcPybD(eventData);
			}
		}

		internal override void OnPointerDown(PointerEventData eventData)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, base.allowedMouseButtons, EventTriggerType.PointerDown))
			{
				kenMZNZStIQnTAavioStPdqUwyIF(eventData);
			}
		}

		internal override void OnPointerEnter(PointerEventData eventData)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, base.allowedMouseButtons, EventTriggerType.PointerEnter))
			{
				ZWAzvWwAMRawEGwPixAMPjfyqNgpA(eventData);
			}
		}

		internal override void OnPointerExit(PointerEventData eventData)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, base.allowedMouseButtons, EventTriggerType.PointerExit))
			{
				zHqZkjRbrFTkqMfkkoUpBAuhCzKfA(eventData);
			}
		}

		private void rEkWtHXjAKTOKMkJBxWaofkAFFvH()
		{
			_pointerId = int.MinValue;
			_realMousePointerId = int.MinValue;
		}

		private bool SBhvinEbIDGWUMwfXythhrIpigKu(int P_0)
		{
			if (P_0 == int.MinValue)
			{
				return false;
			}
			if (_pointerId == int.MinValue)
			{
				return false;
			}
			if (_pointerId == P_0)
			{
				return true;
			}
			if (TouchInteractable.sWBkWrgBUhjXAreNdBTEqWiNGgHjA(P_0) && _realMousePointerId != int.MinValue && P_0 == _realMousePointerId)
			{
				return true;
			}
			return false;
		}

		private PointerEventData utfJPhgnwTqKykeFFJYQHUTouefi(int P_0, GameObject P_1)
		{
			PointerEventData pointerEventData = HyACTUHUMITHJlwRMGbSiKvZntNC(P_0);
			if (pointerEventData == null)
			{
				return null;
			}
			pointerEventData.position = TouchInteractable.TgEgBYQvFnhTTkcyuYMooLsCFzzE(P_0);
			if (TouchInteractable.pmexXrLSDjTQXxgZQgGydOXadJnJ(P_0))
			{
				pointerEventData.eligibleForClick = true;
				pointerEventData.delta = Vector2.zero;
				pointerEventData.dragging = false;
				pointerEventData.useDragThreshold = true;
				pointerEventData.pressPosition = pointerEventData.position;
				pointerEventData.pointerPressRaycast = pointerEventData.pointerCurrentRaycast;
				if (pointerEventData.pointerEnter != P_1)
				{
					pointerEventData.pointerEnter = P_1;
				}
				float unscaledTime = Time.unscaledTime;
				if (P_1 == pointerEventData.lastPress)
				{
					if (unscaledTime - pointerEventData.clickTime < 0.3f)
					{
						int clickCount = pointerEventData.clickCount + 1;
						pointerEventData.clickCount = clickCount;
					}
					else
					{
						pointerEventData.clickCount = 1;
					}
					pointerEventData.clickTime = unscaledTime;
				}
				else
				{
					pointerEventData.clickCount = 1;
				}
				pointerEventData.pointerPress = P_1;
				pointerEventData.rawPointerPress = P_1;
				pointerEventData.clickTime = unscaledTime;
				pointerEventData.pointerDrag = P_1;
			}
			else
			{
				if (!TouchInteractable.sWBkWrgBUhjXAreNdBTEqWiNGgHjA(P_0))
				{
					Logger.LogWarning("Unsupported pointerId: " + P_0);
					return null;
				}
				pointerEventData.eligibleForClick = true;
				pointerEventData.delta = Vector2.zero;
				pointerEventData.dragging = false;
				pointerEventData.useDragThreshold = true;
				pointerEventData.pressPosition = pointerEventData.position;
				pointerEventData.pointerPressRaycast = pointerEventData.pointerCurrentRaycast;
				float unscaledTime2 = Time.unscaledTime;
				if (P_1 == pointerEventData.lastPress)
				{
					if (unscaledTime2 - pointerEventData.clickTime < 0.3f)
					{
						int clickCount = pointerEventData.clickCount + 1;
						pointerEventData.clickCount = clickCount;
					}
					else
					{
						pointerEventData.clickCount = 1;
					}
					pointerEventData.clickTime = unscaledTime2;
				}
				else
				{
					pointerEventData.clickCount = 1;
				}
				pointerEventData.pointerPress = P_1;
				pointerEventData.rawPointerPress = P_1;
				pointerEventData.clickTime = unscaledTime2;
				pointerEventData.pointerDrag = P_1;
			}
			return pointerEventData;
		}

		private PointerEventData lhPOsOAoYCOtTCHWjaXddmYdLcxRA(int P_0, GameObject P_1)
		{
			PointerEventData pointerEventData = HyACTUHUMITHJlwRMGbSiKvZntNC(P_0);
			if (pointerEventData == null)
			{
				return null;
			}
			Vector2 vector = TouchInteractable.TgEgBYQvFnhTTkcyuYMooLsCFzzE(P_0);
			pointerEventData.delta = vector - pointerEventData.position;
			pointerEventData.position = vector;
			pointerEventData.dragging = true;
			pointerEventData.pointerDrag = P_1;
			pointerEventData.useDragThreshold = true;
			pointerEventData.pointerPress = null;
			pointerEventData.rawPointerPress = null;
			return pointerEventData;
		}

		private PointerEventData pVfmwALlLutDLwJmMFqcBjCFoHIS(int P_0)
		{
			PointerEventData pointerEventData = HyACTUHUMITHJlwRMGbSiKvZntNC(P_0);
			if (pointerEventData == null)
			{
				return null;
			}
			if (TouchInteractable.pmexXrLSDjTQXxgZQgGydOXadJnJ(P_0))
			{
				pointerEventData.eligibleForClick = false;
				pointerEventData.pointerPress = null;
				pointerEventData.rawPointerPress = null;
				pointerEventData.dragging = false;
				pointerEventData.pointerDrag = null;
				pointerEventData.pointerEnter = null;
			}
			else
			{
				if (!TouchInteractable.sWBkWrgBUhjXAreNdBTEqWiNGgHjA(P_0))
				{
					Logger.LogWarning("Unsupported pointerId: " + P_0);
					return null;
				}
				pointerEventData.eligibleForClick = false;
				pointerEventData.pointerPress = null;
				pointerEventData.rawPointerPress = null;
				pointerEventData.dragging = false;
				pointerEventData.pointerDrag = null;
			}
			return pointerEventData;
		}

		private void LQXnlCGWVQCMAatUhwMXImJPGHhKA(PointerEventData P_0)
		{
			if (P_0 != null)
			{
				OnPointerUp(P_0);
				pVfmwALlLutDLwJmMFqcBjCFoHIS(RdKVxosdmPbtxnVbOZiHUHSPKFAe);
			}
		}

		private PointerEventData HyACTUHUMITHJlwRMGbSiKvZntNC(int P_0)
		{
			if (P_0 == int.MinValue)
			{
				return null;
			}
			if (__fakePointerEventData == null)
			{
				__fakePointerEventData = new Dictionary<int, PointerEventData>();
			}
			if (!__fakePointerEventData.TryGetValue(P_0, out var value))
			{
				value = new PointerEventData(EventSystem.current);
				value.pointerId = P_0;
				__fakePointerEventData.Add(P_0, value);
				if (TouchInteractable.sWBkWrgBUhjXAreNdBTEqWiNGgHjA(P_0))
				{
					PointerEventData.InputButton button = P_0 switch
					{
						-1 => PointerEventData.InputButton.Left, 
						-2 => PointerEventData.InputButton.Right, 
						-3 => PointerEventData.InputButton.Middle, 
						_ => throw new NotImplementedException(), 
					};
					value.button = button;
				}
			}
			return value;
		}
	}
}
