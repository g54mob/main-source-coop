using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Rewired.ComponentControls.Data;
using Rewired.Internal;
using Rewired.Utils;
using Rewired.Utils.Attributes;
using Rewired.Utils.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Rewired.ComponentControls
{
	[Serializable]
	[AddComponentMenu("Rewired/Touch Joystick")]
	[DisallowMultipleComponent]
	public sealed class TouchJoystick : TouchInteractable
	{
		public enum AxisDirection
		{
			Both = 0,
			Horizontal = 1,
			Vertical = 2
		}

		public enum JoystickMode
		{
			Analog = 0,
			Digital = 1
		}

		public enum SnapDirections
		{
			None = 0,
			Four = 4,
			Eight = 8,
			Sixteen = 0x10,
			ThirtyTwo = 0x20,
			SixtyFour = 0x40
		}

		private enum HAucqZcqPbqeaRkOAvBFuZtmlrbX
		{
			None = 0,
			TowardTouch = 1,
			TowardHome = 2
		}

		private enum AQQeIWhyIPZKFavzzcGnNuzAgdWEb
		{
			Local = 0,
			TouchRegion = 1
		}

		public enum StickBounds
		{
			Circle = 0,
			Square = 1
		}

		[Serializable]
		public class ValueChangedEventHandler : UnityEvent<Vector2>
		{
		}

		[Serializable]
		public class StickPositionChangedEventHandler : UnityEvent<Vector2>
		{
		}

		[Serializable]
		public class TapEventHandler : UnityEvent
		{
		}

		[Serializable]
		public class TouchStartedEventHandler : UnityEvent
		{
		}

		[Serializable]
		public class TouchEndedEventHandler : UnityEvent
		{
		}

		public interface IValueChangedHandler
		{
			void OnValueChanged(Vector2 value);
		}

		public interface IStickPositionChangedHandler
		{
			void OnStickPositionChanged(Vector2 value);
		}

		[Serializable]
		private sealed class ngnRiuTkxrVoOanVtjdtgQrNtVte
		{
			public static readonly ngnRiuTkxrVoOanVtjdtgQrNtVte _003C_003E9 = new ngnRiuTkxrVoOanVtjdtgQrNtVte();

			public static tASbVocwrkZfBjDhttUGkjNilJQyA.EventFunction<IValueChangedHandler, Vector2> _003C_003E9__277_0;

			public static tASbVocwrkZfBjDhttUGkjNilJQyA.EventFunction<IStickPositionChangedHandler, Vector2> _003C_003E9__280_0;

			internal void soYOmwhqrGvbRItoviGirdtfERTp(IValueChangedHandler P_0, Vector2 P_1)
			{
				P_0.OnValueChanged(P_1);
			}

			internal void DHmjQlbjFdELmcAZMiZLomjFsAU(IStickPositionChangedHandler P_0, Vector2 P_1)
			{
				P_0.OnStickPositionChanged(P_1);
			}
		}

		private sealed class HTHcLOaXfkEsqLumLPjjbRjLqNLx : IDisposable, IEnumerator, IEnumerator<object>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private object VqEePGSMyrGKIqkWibsjeHcWPSIx;

			public float QzkoAujiXELSEATTFbUXkPALueUS;

			public TouchJoystick TtytLoUfsgUyhsklaKccrnoMiiek;

			public PositionType NrdyBFMMWwnWFfEyWKBnxrSnZvnA;

			public Vector2 JTKktbfSAqGDQjeacpqTtOeybKpt;

			public HAucqZcqPbqeaRkOAvBFuZtmlrbX sGqbUlnFwRgsWfMjUESVfFgHGuQnb;

			private RectTransform zhIaopVbzvThffCKABmaaMXeNOEU;

			private Vector2 MAKcOIKScodBQvETwJwUTnXGybGz;

			private float EqWwPxCLglgHGkbPhQwGHBtsGbeTA;

			private float ifUZPTedFJKUBUSvTHjRpfgDkWRg;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public HTHcLOaXfkEsqLumLPjjbRjLqNLx(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				TouchJoystick ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_010c;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				if (!(QzkoAujiXELSEATTFbUXkPALueUS <= 0f))
				{
					zhIaopVbzvThffCKABmaaMXeNOEU = ttytLoUfsgUyhsklaKccrnoMiiek.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA;
					MAKcOIKScodBQvETwJwUTnXGybGz = sFgWwfUJkMIxinvyEhNsisGjhMkS.aMkDURFFHrdJnCwawIgfGyvPbVKb(zhIaopVbzvThffCKABmaaMXeNOEU, NrdyBFMMWwnWFfEyWKBnxrSnZvnA);
					float magnitude = (JTKktbfSAqGDQjeacpqTtOeybKpt - MAKcOIKScodBQvETwJwUTnXGybGz).magnitude;
					if (!(magnitude < 0.01f))
					{
						ttytLoUfsgUyhsklaKccrnoMiiek._isMoving = true;
						EqWwPxCLglgHGkbPhQwGHBtsGbeTA = magnitude / QzkoAujiXELSEATTFbUXkPALueUS;
						ifUZPTedFJKUBUSvTHjRpfgDkWRg = 0f;
						goto IL_010c;
					}
				}
				goto IL_0119;
				IL_0119:
				ttytLoUfsgUyhsklaKccrnoMiiek.ROeJSkZhnkTDcLHfcKCeHhXUonug(sGqbUlnFwRgsWfMjUESVfFgHGuQnb, JTKktbfSAqGDQjeacpqTtOeybKpt, NrdyBFMMWwnWFfEyWKBnxrSnZvnA);
				return false;
				IL_010c:
				if (ifUZPTedFJKUBUSvTHjRpfgDkWRg <= 1f)
				{
					ifUZPTedFJKUBUSvTHjRpfgDkWRg += Time.unscaledDeltaTime / EqWwPxCLglgHGkbPhQwGHBtsGbeTA;
					sFgWwfUJkMIxinvyEhNsisGjhMkS.jdVjbFKPJGjGyhWuwNTOVPPfnNwT(zhIaopVbzvThffCKABmaaMXeNOEU, Vector2.Lerp(MAKcOIKScodBQvETwJwUTnXGybGz, JTKktbfSAqGDQjeacpqTtOeybKpt, Mathf.SmoothStep(0f, 1f, ifUZPTedFJKUBUSvTHjRpfgDkWRg)), NrdyBFMMWwnWFfEyWKBnxrSnZvnA);
					VqEePGSMyrGKIqkWibsjeHcWPSIx = null;
					RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
					return true;
				}
				goto IL_0119;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		private const float MAX_MOVE_SPEED = 20f;

		[SerializeField]
		[Tooltip("The Custom Controller element(s) that will receive input values from the joystick's X axis.")]
		[CustomObfuscation(rename = false)]
		private CustomControllerElementTargetSetForFloat _horizontalAxisCustomControllerElement = new CustomControllerElementTargetSetForFloat();

		[Tooltip("The Custom Controller element(s) that will receive input values from the joystick's Y axis.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private CustomControllerElementTargetSetForFloat _verticalAxisCustomControllerElement = new CustomControllerElementTargetSetForFloat();

		[Tooltip("The Custom Controller element that will receive input values from taps.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private CustomControllerElementTargetSetForBoolean _tapCustomControllerElement = new CustomControllerElementTargetSetForBoolean();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("The Rect Transform of the stick disc. This is moved around by the user when manipulating the joystick.")]
		private RectTransform _stickTransform;

		[Tooltip("The joystick's mode of operation. Set this to Digital to simulate a D-Pad which has only On/Off states. If you want mimic a real D-Pad, you should also set Snap Directions to 8.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private JoystickMode _joystickMode;

		[CustomObfuscation(rename = false)]
		[Range(0f, 1f)]
		[Tooltip("A dead zone which is applied when Stick Mode is set to Digital. This is used to filter out tiny stick movements near 0, 0.")]
		[SerializeField]
		private float _digitalModeDeadZone = 0.3f;

		[Range(0.01f, 1000f)]
		[SerializeField]
		[Tooltip("The range of movement of the stick in Canvas pixels. The larger the number, the further the stick must be moved from center to register movement.")]
		[CustomObfuscation(rename = false)]
		private float _stickRange = 60f;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("If enabled, the stick range will scale with parent controls. Otherwise, the stick range will remain constant.")]
		private bool _scaleStickRange = true;

		[SerializeField]
		[Tooltip("The shape of the range of movement of the joystick.")]
		[CustomObfuscation(rename = false)]
		private StickBounds _stickBounds;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("The axis directions in which movement is allowed. You can restrict movement to one or both axes.")]
		private AxisDirection _axesToUse;

		[SerializeField]
		[Tooltip("Snaps joystick movement to a fixed number of directions. This can be used to create a D-Pad, for example, setting it to 4 or 8 directions. If you want a true D-Pad, Stick Mode should be set to digital.")]
		[CustomObfuscation(rename = false)]
		private SnapDirections _snapDirections;

		[Tooltip("If true, the stick disc will snap immediately to the touch position when initially touched. This results in the stick disc being centered to the touch position. This will cause the stick to generate input immediately when touched if not touched perfectly centered.If false, the stick disc will remain in its current position on touch, and when dragged will retain the same offset. The stick's center point will be set to the position of the touch. The initial touch will not cause the stick to pop in any direction.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private bool _snapStickToTouch;

		[Tooltip("If true, the stick will return to the center after it is released. Otherwise, the stick will remain in the last position and continue to return input.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _centerStickOnRelease = true;

		[Tooltip("The underlying Axis 2D.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private StandaloneAxis2D _axis2D = new StandaloneAxis2D();

		[CustomObfuscation(rename = false)]
		[Tooltip("If true, the joystick can be activated by a touch swipe that began in an area outside the joystick region. If false, the joystick can only be activated by a direct touch.")]
		[SerializeField]
		private bool _activateOnSwipeIn;

		[Tooltip("If true, the joystick will stay engaged even if the touch that activated it moves outside the joystick region. If false, the joystick will be released once the touch that activated it moves outside the joystick region.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _stayActiveOnSwipeOut = true;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("Should taps on the touch pad be processed?")]
		private bool _allowTap;

		[FieldRange(0f, float.MaxValue)]
		[CustomObfuscation(rename = false)]
		[Tooltip("The maximum touch duration allowed for the touch to be considered a tap. A touch that lasts longer than this value will not trigger a tap when released.")]
		[SerializeField]
		private float _tapTimeout = 0.25f;

		[CustomObfuscation(rename = false)]
		[FieldRange(-1, int.MaxValue)]
		[Tooltip("The maximum movement distance allowed in pixels since the touch began for the touch to be considered a tap. [-1 = no limit]")]
		[SerializeField]
		private int _tapDistanceLimit = 10;

		[Tooltip("Optional external region to use for hover/click/touch detection. If set, this region will be used for touch detection instead of or in addition to the joystick's RectTransform. This can be useful if you want a larger area of the screen to act as a joystick.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private TouchRegion _touchRegion;

		[Tooltip("If True, hovers/clicks/touches on the local joystick will be ignored and only Touch Region touches will be used. Otherwise, both touches on the joystick and on the Touch Region will be used. This also applies to mouse hover. This setting has no effect if no Touch Region is set.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private bool _useTouchRegionOnly = true;

		[CustomObfuscation(rename = false)]
		[Tooltip("If True, the joystick will move to the location of the current touch in the Touch Region. This can be used to designate an area of the screen as a hot-spot for a joystick and have the joystick graphics follow the users touches. This only has an effect if a Touch Region is set.")]
		[SerializeField]
		private bool _moveToTouchPosition;

		[CustomObfuscation(rename = false)]
		[Tooltip("If Move To Touch Position is enabled, this will make the joystick return to its original position after the press is released. This only has an effect if a Touch Region is set.")]
		[SerializeField]
		private bool _returnOnRelease = true;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("If True, the joystick will follow the touch around until released. This setting overrides Move To Touch Position.")]
		private bool _followTouchPosition;

		[Tooltip("Should the joystick animate when moving to the touch point? This only has an effect if Move To Touch Position is True and a Touch Region is set. This setting is ignored if Follow Touch Position is True.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private bool _animateOnMoveToTouch = true;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Range(0f, 20f)]
		[Tooltip("The speed at which the joystick will move toward the touch position measured in screens per second (based on the larger of width and height). [1.0 = Move 1 screen/sec]. This only has an effect if Move To Touch Position is True, Animate On Move To Touch is true, and a Touch Region is set. This setting is ignored if Follow Touch Position is True.")]
		private float _moveToTouchSpeed = 2f;

		[Tooltip("Should the joystick animate when moving back to its original position? This only has an effect if Follow Touch Position is True, or if Move To Touch Position is True and a Touch Region is set, and Return on Release is True.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private bool _animateOnReturn = true;

		[CustomObfuscation(rename = false)]
		[Range(0f, 20f)]
		[SerializeField]
		[Tooltip("The speed at which the joystick will move back toward its original position measured in screens per second (based on the larger of width and height). [1.0 = Move 1 screen/sec]. This only has an effect if Follow Touch Position is True, or if Move To Touch Position is True and a Touch Region is set, and Return on Release and Animate on Return are both True.")]
		private float _returnSpeed = 2f;

		[Tooltip("If True, it will attempt to automatically manage Graphic component raycasting for best results based on your current settings.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private bool _manageRaycasting = true;

		private bool _useXAxis;

		private bool _useYAxis;

		private tASbVocwrkZfBjDhttUGkjNilJQyA.HierarchyEventHelper<IValueChangedHandler, Vector2> _hierarchyValueChangedHandlers;

		private tASbVocwrkZfBjDhttUGkjNilJQyA.HierarchyEventHelper<IStickPositionChangedHandler, Vector2> _hierarchyStickPositionChangedHandlers;

		private TouchRegion _workingTouchRegion;

		private Vector2 _origAnchoredPosition;

		private Vector2 _origStickAnchoredPosition;

		private Vector2 _lastPressAnchoredPosition;

		private bool _isMoving;

		private bool _isMovedFromDefaultPosition;

		private HAucqZcqPbqeaRkOAvBFuZtmlrbX _moveDirection;

		private int _pointerId = int.MinValue;

		private int _realMousePointerId = int.MinValue;

		[NonSerialized]
		private bool ALBJgfsRYJdMVJwePTiUwIfQxDNM;

		[NonSerialized]
		private bool zmNsczlnaJKPNGfDEeyyFdsXhpjgb;

		private bool _pointerDownIsFake;

		private Vector2 _lastPressStartingValue;

		private AQQeIWhyIPZKFavzzcGnNuzAgdWEb _lastClaimSource;

		private float _touchStartTime;

		private Vector2 _touchStartPosition;

		private IEnumerator _coroutineMove;

		private orKfWSGFqzCFUysymbkGocsATicLA _imageRaycastHelper = new orKfWSGFqzCFUysymbkGocsATicLA();

		private int _calculatedStickRange_lastUpdatedFrame = -1;

		private int _lastTapFrame = -1;

		private bool _isEligibleForTap;

		private float __calculatedStickRange_cachedValue;

		private Action<HAucqZcqPbqeaRkOAvBFuZtmlrbX> __moveStartedDelegate;

		private Action<HAucqZcqPbqeaRkOAvBFuZtmlrbX> __moveEndedDelegate;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("Event sent when the joystick value changes.")]
		private ValueChangedEventHandler _onValueChanged = new ValueChangedEventHandler();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("Event sent when the joystick's stick position changes.")]
		private ValueChangedEventHandler _onStickPositionChanged = new ValueChangedEventHandler();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("Event sent when the joystick is touched.")]
		private TouchStartedEventHandler _onTouchStarted = new TouchStartedEventHandler();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private TouchEndedEventHandler _onTouchEnded = new TouchEndedEventHandler();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("Event sent when the touch pad is tapped. This event will only be sent if allowTap is True.")]
		private TapEventHandler _onTap = new TapEventHandler();

		private Dictionary<int, PointerEventData> __fakePointerEventData;

		private static tASbVocwrkZfBjDhttUGkjNilJQyA.EventFunction<IValueChangedHandler, Vector2> __valueChangedHandlerDelegate;

		private static tASbVocwrkZfBjDhttUGkjNilJQyA.EventFunction<IStickPositionChangedHandler, Vector2> __stickPositionChangedHandlerDelegate;

		public CustomControllerElementTargetSetForFloat horizontalAxisCustomControllerElement => _horizontalAxisCustomControllerElement;

		public CustomControllerElementTargetSetForFloat verticalAxisCustomControllerElement => _verticalAxisCustomControllerElement;

		public CustomControllerElementTargetSetForBoolean tapCustomControllerElement => _tapCustomControllerElement;

		public RectTransform stickTransform
		{
			get
			{
				return _stickTransform;
			}
			set
			{
				if (!(_stickTransform == value))
				{
					_stickTransform = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public JoystickMode joystickMode
		{
			get
			{
				return _joystickMode;
			}
			set
			{
				if (_joystickMode != value)
				{
					_joystickMode = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public float digitalModeDeadZone
		{
			get
			{
				return _digitalModeDeadZone;
			}
			set
			{
				value = MathTools.Clamp01(value);
				if (_digitalModeDeadZone != value)
				{
					_digitalModeDeadZone = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public float stickRange
		{
			get
			{
				return _stickRange;
			}
			set
			{
				value = MathTools.Clamp(value, 1f, 1000f);
				if (_stickRange != value)
				{
					_stickRange = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool scaleStickRange
		{
			get
			{
				return _scaleStickRange;
			}
			set
			{
				if (_scaleStickRange != value)
				{
					_scaleStickRange = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		private StickBounds mcPkZahauhqShOrqPIJhykUvoUKW
		{
			get
			{
				return _stickBounds;
			}
			set
			{
				if (_stickBounds != stickBounds)
				{
					_stickBounds = stickBounds;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

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

		public SnapDirections snapDirections
		{
			get
			{
				return _snapDirections;
			}
			set
			{
				if (_snapDirections != value)
				{
					_snapDirections = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool snapStickToTouch
		{
			get
			{
				return _snapStickToTouch;
			}
			set
			{
				if (_snapStickToTouch != value)
				{
					_snapStickToTouch = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool centerStickOnRelease
		{
			get
			{
				return _centerStickOnRelease;
			}
			set
			{
				if (_centerStickOnRelease != value)
				{
					_centerStickOnRelease = value;
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
				if (NDhTMIQItThJsXWyCtLUVEorVCTb())
				{
					return true;
				}
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

		public TouchRegion touchRegion
		{
			get
			{
				return _touchRegion;
			}
			set
			{
				if (!(_touchRegion == value))
				{
					_touchRegion = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool useTouchRegionOnly
		{
			get
			{
				return _useTouchRegionOnly;
			}
			set
			{
				if (_useTouchRegionOnly != value)
				{
					_useTouchRegionOnly = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool moveToTouchPosition
		{
			get
			{
				return _moveToTouchPosition;
			}
			set
			{
				if (_moveToTouchPosition != value)
				{
					_moveToTouchPosition = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool returnOnRelease
		{
			get
			{
				return _returnOnRelease;
			}
			set
			{
				if (_returnOnRelease != value)
				{
					_returnOnRelease = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool followTouchPosition
		{
			get
			{
				return _followTouchPosition;
			}
			set
			{
				if (_followTouchPosition != value)
				{
					_followTouchPosition = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool animateOnMoveToTouch
		{
			get
			{
				return _animateOnMoveToTouch;
			}
			set
			{
				if (_animateOnMoveToTouch != value)
				{
					_animateOnMoveToTouch = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public float moveToTouchSpeed
		{
			get
			{
				return _moveToTouchSpeed;
			}
			set
			{
				value = MathTools.Clamp(value, 0f, 20f);
				if (_moveToTouchSpeed != value)
				{
					_moveToTouchSpeed = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool animateOnReturn
		{
			get
			{
				return _animateOnReturn;
			}
			set
			{
				if (_animateOnReturn != value)
				{
					_animateOnReturn = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public float returnSpeed
		{
			get
			{
				return _returnSpeed;
			}
			set
			{
				value = MathTools.Clamp(value, 0f, 20f);
				if (_returnSpeed != value)
				{
					_returnSpeed = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool manageRaycasting
		{
			get
			{
				return _manageRaycasting;
			}
			set
			{
				if (_manageRaycasting != value)
				{
					_manageRaycasting = value;
					if (value)
					{
						aaiAJUIcGSdONuPwfwJBPYnPJCarA();
					}
					else
					{
						_imageRaycastHelper.SPGTRPyvIslcMdbPTItsewSLRPxx();
					}
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public AxisCalibration horizontalAxisCalibration => _axis2D.xAxis.calibration;

		public AxisCalibration verticalAxisCalibration => _axis2D.yAxis.calibration;

		[Obsolete("Use axis2DCalibration instead.", false)]
		public Axis2DCalibration deadZoneType => _axis2D.calibration;

		public Axis2DCalibration axis2DCalibration => _axis2D.calibration;

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

		private bool yjikdyNyZFDrkcdNNgEgZYvwGHKQ => _lastTapFrame == Time.frameCount;

		internal StandaloneAxis2D ACDlWRYqRUOGCISCfVnFWZBaWOxV => _axis2D;

		private Action<HAucqZcqPbqeaRkOAvBFuZtmlrbX> sYYYyCpyZfOyIwxaAvXQFATMFmho
		{
			get
			{
				if (__moveStartedDelegate == null)
				{
					return __moveStartedDelegate = XNKImEVsUNPavzxgPJMfIdoSEAWgA;
				}
				return __moveStartedDelegate;
			}
		}

		private Action<HAucqZcqPbqeaRkOAvBFuZtmlrbX> pCTEPHMlMUJAWuyayBZlAAjMxtTy
		{
			get
			{
				if (__moveEndedDelegate == null)
				{
					return __moveEndedDelegate = UkrWacxKoOMuRkiKLRmEmILrFhRJ;
				}
				return __moveEndedDelegate;
			}
		}

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

		private RectTransform CemuTvKBxSlWusQeIceOjYYrEMlr
		{
			get
			{
				if (_lastClaimSource != AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion)
				{
					return base.transform as RectTransform;
				}
				return base.transform.parent as RectTransform;
			}
		}

		private float QNkuiWUZthEWSYPljzmqFaGIdebL
		{
			get
			{
				if (Time.frameCount == _calculatedStickRange_lastUpdatedFrame)
				{
					return __calculatedStickRange_cachedValue;
				}
				RectTransform rectTransform = base.whnJFhhvlVIfvAzDjDUVAxyZymsH;
				RectTransform rectTransform2 = CemuTvKBxSlWusQeIceOjYYrEMlr;
				Vector3 position = new Vector3(0f, _stickRange, 0f);
				Vector3 vector = rectTransform.TransformPoint(position) - rectTransform.position;
				Vector3 a = rectTransform2.InverseTransformPoint(vector + rectTransform2.position);
				float magnitude;
				if (_scaleStickRange)
				{
					Vector3 lossyScale = rectTransform.lossyScale;
					Vector3 lossyScale2 = rectTransform2.lossyScale;
					if (lossyScale.x != 0f)
					{
						lossyScale2.x /= lossyScale.x;
					}
					if (lossyScale.y != 0f)
					{
						lossyScale2.y /= lossyScale.y;
					}
					if (lossyScale.z != 0f)
					{
						lossyScale2.z /= lossyScale.z;
					}
					if (_lastClaimSource == AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion)
					{
						lossyScale2.Scale(base.transform.localScale);
					}
					magnitude = Vector3.Scale(a, lossyScale2).magnitude;
				}
				else
				{
					magnitude = a.magnitude;
				}
				__calculatedStickRange_cachedValue = magnitude;
				_calculatedStickRange_lastUpdatedFrame = Time.frameCount;
				return magnitude;
			}
		}

		internal static tASbVocwrkZfBjDhttUGkjNilJQyA.EventFunction<IValueChangedHandler, Vector2> iucgcTjctUgiESJftOWYTkXrkXNHA
		{
			get
			{
				if (__valueChangedHandlerDelegate == null)
				{
					__valueChangedHandlerDelegate = ngnRiuTkxrVoOanVtjdtgQrNtVte._003C_003E9.soYOmwhqrGvbRItoviGirdtfERTp;
				}
				return __valueChangedHandlerDelegate;
			}
		}

		internal static tASbVocwrkZfBjDhttUGkjNilJQyA.EventFunction<IStickPositionChangedHandler, Vector2> WlcekNxMkJpWXiTrWPtArhIsjaTZ
		{
			get
			{
				if (__stickPositionChangedHandlerDelegate == null)
				{
					__stickPositionChangedHandlerDelegate = ngnRiuTkxrVoOanVtjdtgQrNtVte._003C_003E9.DHmjQlbjFdELmcAZMiZLomjFsAU;
				}
				return __stickPositionChangedHandlerDelegate;
			}
		}

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

		public event UnityAction<Vector2> StickPositionChangedEvent
		{
			add
			{
				_onStickPositionChanged.AddListener(value);
			}
			remove
			{
				_onStickPositionChanged.RemoveListener(value);
			}
		}

		public event UnityAction TouchDownEvent
		{
			add
			{
				_onTouchStarted.AddListener(value);
			}
			remove
			{
				_onTouchStarted.RemoveListener(value);
			}
		}

		public event UnityAction TouchUpEvent
		{
			add
			{
				_onTouchEnded.AddListener(value);
			}
			remove
			{
				_onTouchEnded.RemoveListener(value);
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

		[CustomObfuscation(rename = false)]
		private TouchJoystick()
		{
		}

		public Vector2 GetValue()
		{
			if (!base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				return _axis2D.rawZero;
			}
			return _axis2D.value;
		}

		public Vector2 GetRawValue()
		{
			if (!base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				return _axis2D.rawZero;
			}
			return _axis2D.rawValue;
		}

		public void SetRawValue(Vector2 value)
		{
			if (!base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				return;
			}
			if (_joystickMode == JoystickMode.Digital)
			{
				if (value.sqrMagnitude <= _digitalModeDeadZone * _digitalModeDeadZone)
				{
					value.x = 0f;
					value.y = 0f;
				}
				else
				{
					value.Normalize();
				}
			}
			if (_snapDirections != SnapDirections.None)
			{
				value = MathTools.SnapVectorToNearestAngle(value, 360f / (float)_snapDirections);
				if (value.x != 0f)
				{
					if (MathTools.IsNearZero(value.x, 0.0001f))
					{
						value.x = 0f;
					}
					else if (MathTools.IsNear(value.x, 1f, 0.0001f))
					{
						value.x = 1f;
					}
					else if (MathTools.IsNear(value.x, -1f, 0.0001f))
					{
						value.x = -1f;
					}
				}
				if (value.y != 0f)
				{
					if (MathTools.IsNearZero(value.y, 0.0001f))
					{
						value.y = 0f;
					}
					else if (MathTools.IsNear(value.y, 1f, 0.0001f))
					{
						value.y = 1f;
					}
					else if (MathTools.IsNear(value.y, -1f, 0.0001f))
					{
						value.y = -1f;
					}
				}
			}
			if (_useXAxis || _useYAxis)
			{
				_axis2D.SetRawValue(_useXAxis ? value.x : 0f, _useYAxis ? value.y : 0f);
			}
		}

		public void SetDefaultPosition()
		{
			ILkWicEjdmMiShclEHEAqLqIsFgs(base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA.anchoredPosition);
		}

		private void ILkWicEjdmMiShclEHEAqLqIsFgs(Vector2 P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				_origAnchoredPosition = P_0;
			}
		}

		public void ReturnToDefaultPosition(bool instant)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				xHovnLtQkBnCuETzVQnpnWVuJUuI(_origAnchoredPosition, PositionType.Anchored, !instant && _animateOnReturn, _returnSpeed, HAucqZcqPbqeaRkOAvBFuZtmlrbX.TowardHome);
			}
		}

		public void ReturnToDefaultPosition()
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				ReturnToDefaultPosition(instant: false);
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void Awake()
		{
			base.Awake();
			if (Application.isPlaying)
			{
				_origAnchoredPosition = base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA.anchoredPosition;
				if (_stickTransform != null)
				{
					_origStickAnchoredPosition = _stickTransform.anchoredPosition;
				}
				SetRawValue(ACDlWRYqRUOGCISCfVnFWZBaWOxV.rawZero);
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnEnable()
		{
			base.OnEnable();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				khkdMwnTAvlDqPvCkDAZhRJFGYZR();
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnDisable()
		{
			base.OnDisable();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				_axis2D.Deinitialize();
				vXIboQFSGKdCLZnWmauizJnaOLuX();
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

		internal override void ZCGETbjMQZUkyflRtYAqwQNUBPQIb()
		{
			base.ZCGETbjMQZUkyflRtYAqwQNUBPQIb();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				uiycTgIPBbGghVXEDqOPpxkdfEhq();
				nxTbtLCLjbyuyRGfsvxPBPMmlZRq();
				SUSGFUJwfFASbZtQzNdxeGuaWaYnA();
			}
		}

		internal override bool lpOPYPkfRAdylCMSLphTlIgUWIWy()
		{
			if (!base.lpOPYPkfRAdylCMSLphTlIgUWIWy())
			{
				return false;
			}
			GxzFEFeJIqsFFABrybfdpQNfDFNi();
			_axis2D.Initialize();
			return true;
		}

		internal override void xhHhPOcavYUrBJGXQmMDzQMMGISaA()
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && osKcqUcyYlVGlGygpMaOnUYNqJDBA)
			{
				Vector2 value = _axis2D.value;
				if (_useXAxis)
				{
					SPXsRbvylxmzyfkxsgnXmtaaKOyb(_horizontalAxisCustomControllerElement, value.x, _axis2D.xAxis.buttonActivationThreshold);
				}
				if (_useYAxis)
				{
					SPXsRbvylxmzyfkxsgnXmtaaKOyb(_verticalAxisCustomControllerElement, value.y, _axis2D.yAxis.buttonActivationThreshold);
				}
				if (_allowTap)
				{
					SPXsRbvylxmzyfkxsgnXmtaaKOyb(_tapCustomControllerElement, yjikdyNyZFDrkcdNNgEgZYvwGHKQ);
				}
			}
		}

		internal override void sGOXUkdDJMvaXZNxRxZLevjalIqm()
		{
			base.sGOXUkdDJMvaXZNxRxZLevjalIqm();
			_axis2D.ValueChangedEvent += ghoFULOHEeeqyJlbKyZkQSyDIDpV;
		}

		internal override void LYluBUMWPKbwashzGpNPLTeasipd()
		{
			base.LYluBUMWPKbwashzGpNPLTeasipd();
			_axis2D.ValueChangedEvent -= ghoFULOHEeeqyJlbKyZkQSyDIDpV;
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
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				_pointerId = int.MinValue;
				_realMousePointerId = int.MinValue;
				ALBJgfsRYJdMVJwePTiUwIfQxDNM = false;
				zmNsczlnaJKPNGfDEeyyFdsXhpjgb = false;
				_pointerDownIsFake = false;
				_lastPressAnchoredPosition = Vector2.zero;
				_lastPressStartingValue = Vector2.zero;
				_calculatedStickRange_lastUpdatedFrame = -1;
				_lastTapFrame = -1;
				_isEligibleForTap = false;
				if (_returnOnRelease && _isMovedFromDefaultPosition && (_moveToTouchPosition || _followTouchPosition))
				{
					ReturnToDefaultPosition(instant: true);
				}
				_isMovedFromDefaultPosition = false;
				_isMoving = false;
				_moveDirection = HAucqZcqPbqeaRkOAvBFuZtmlrbX.None;
				zPMUivaPRCCBydGFdfPeBETKRYnmc();
				_axis2D.Clear();
				khkdMwnTAvlDqPvCkDAZhRJFGYZR();
			}
		}

		internal override void AOKLCSwSOHWHrUKFiqLEizXMqAyJ()
		{
			base.AOKLCSwSOHWHrUKFiqLEizXMqAyJ();
			if (_hierarchyValueChangedHandlers == null)
			{
				_hierarchyValueChangedHandlers = new tASbVocwrkZfBjDhttUGkjNilJQyA.HierarchyEventHelper<IValueChangedHandler, Vector2>(iucgcTjctUgiESJftOWYTkXrkXNHA);
			}
			_hierarchyValueChangedHandlers.GetHandlers(base.transform);
			if (_hierarchyStickPositionChangedHandlers == null)
			{
				_hierarchyStickPositionChangedHandlers = new tASbVocwrkZfBjDhttUGkjNilJQyA.HierarchyEventHelper<IStickPositionChangedHandler, Vector2>(WlcekNxMkJpWXiTrWPtArhIsjaTZ);
			}
			_hierarchyStickPositionChangedHandlers.GetHandlers(base.transform);
		}

		public override void ClearValue()
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				_axis2D.Clear();
				_lastTapFrame = -1;
				if (osKcqUcyYlVGlGygpMaOnUYNqJDBA)
				{
					base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ClearElementValue(_horizontalAxisCustomControllerElement);
					base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ClearElementValue(_verticalAxisCustomControllerElement);
					base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ClearElementValue(_tapCustomControllerElement);
				}
			}
		}

		internal override bool jlMJYbxJlzjsBwCBJxvDZPhnPAZF()
		{
			if (!base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				return false;
			}
			if (!EDufGzVNigBlMAOWvMGsHZmtQaph())
			{
				return false;
			}
			return ALBJgfsRYJdMVJwePTiUwIfQxDNM;
		}

		internal override bool mxaQUajJeAklKpqxemiTJFTBFCaq(GameObject P_0)
		{
			if (P_0 == null)
			{
				return false;
			}
			if (base.mxaQUajJeAklKpqxemiTJFTBFCaq(P_0))
			{
				return true;
			}
			if (_workingTouchRegion != null)
			{
				return _workingTouchRegion.gameObject == P_0;
			}
			return false;
		}

		private void khkdMwnTAvlDqPvCkDAZhRJFGYZR()
		{
			_horizontalAxisCustomControllerElement.ClearElementCaches();
			_verticalAxisCustomControllerElement.ClearElementCaches();
			_tapCustomControllerElement.ClearElementCaches();
			SUSGFUJwfFASbZtQzNdxeGuaWaYnA();
			aaiAJUIcGSdONuPwfwJBPYnPJCarA();
		}

		private void aaiAJUIcGSdONuPwfwJBPYnPJCarA()
		{
			if (_manageRaycasting)
			{
				_imageRaycastHelper.mKNIhyiGRhrEXKxQDuaJFRiTMRVl(base.transform, UZuPaidSRpiDUdxlqCyJWkepnHav());
			}
		}

		private bool UZuPaidSRpiDUdxlqCyJWkepnHav()
		{
			if (_workingTouchRegion != null && _useTouchRegionOnly)
			{
				return false;
			}
			return true;
		}

		private void duaIiAtDTEcsqfybpdyfGRlfYedB(TouchRegion P_0)
		{
			if (!(P_0 == null))
			{
				dlnLVQxHbOeHUrCToOHILuLpTVKI(P_0);
				P_0.PointerDownEvent += lEDjUpnxqBucRFkEDyJKbVEmsmkA;
				P_0.PointerUpEvent += EuQQZFwdcthAzHoBOBDBcyHGjRwyB;
				P_0.PointerEnterEvent += OmQwIffSZPImRUCIYEOwFrZRxIQW;
				P_0.PointerExitEvent += PnDoCbelFWRcICvBVIgnpCPKYTkI;
				P_0.BeginDragEvent += ElBBXVTrkqqOSaAcSRJhTyyORDhq;
				P_0.DragEvent += smhAMMKHqpAxqkfKwVnFAxcYqokSA;
				P_0.EndDragEvent += xApIcLqpCsqKyGaPkToTqMKtCkTcA;
			}
		}

		private void dlnLVQxHbOeHUrCToOHILuLpTVKI(TouchRegion P_0)
		{
			if (!(P_0 == null))
			{
				P_0.PointerDownEvent -= lEDjUpnxqBucRFkEDyJKbVEmsmkA;
				P_0.PointerUpEvent -= EuQQZFwdcthAzHoBOBDBcyHGjRwyB;
				P_0.PointerEnterEvent -= OmQwIffSZPImRUCIYEOwFrZRxIQW;
				P_0.PointerExitEvent -= PnDoCbelFWRcICvBVIgnpCPKYTkI;
				P_0.BeginDragEvent -= ElBBXVTrkqqOSaAcSRJhTyyORDhq;
				P_0.DragEvent -= smhAMMKHqpAxqkfKwVnFAxcYqokSA;
				P_0.EndDragEvent -= xApIcLqpCsqKyGaPkToTqMKtCkTcA;
			}
		}

		private void SUSGFUJwfFASbZtQzNdxeGuaWaYnA()
		{
			if (!(_workingTouchRegion == _touchRegion))
			{
				dlnLVQxHbOeHUrCToOHILuLpTVKI(_workingTouchRegion);
				_workingTouchRegion = _touchRegion;
				duaIiAtDTEcsqfybpdyfGRlfYedB(_workingTouchRegion);
			}
		}

		private void jbQgKcxKQGzETsTgYjuvYckRsvZo(Vector2 P_0, bool P_1, float P_2, HAucqZcqPbqeaRkOAvBFuZtmlrbX P_3)
		{
			RectTransform rectTransform = base.transform.parent as RectTransform;
			Vector2 vector = sFgWwfUJkMIxinvyEhNsisGjhMkS.PCIXWfbuGOHlbBawYtVicRpFfIoX(base.vQpmvzIqZzQRgZjNdTCOIzNihLiH, rectTransform, P_0);
			Vector2 pivot = base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA.pivot;
			Vector2 sizeDelta = base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA.sizeDelta;
			Vector3 localScale = base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA.localScale;
			vector += new Vector2((pivot.x - 0.5f) * sizeDelta.x * localScale.x, (pivot.y - 0.5f) * sizeDelta.y * localScale.y);
			xHovnLtQkBnCuETzVQnpnWVuJUuI(vector, PositionType.Local, P_1, P_2, P_3);
		}

		private void xHovnLtQkBnCuETzVQnpnWVuJUuI(Vector2 P_0, PositionType P_1, bool P_2, float P_3, HAucqZcqPbqeaRkOAvBFuZtmlrbX P_4)
		{
			if (_isMoving && P_2 && _moveDirection == P_4)
			{
				return;
			}
			if (_isMoving && _coroutineMove != null)
			{
				zPMUivaPRCCBydGFdfPeBETKRYnmc();
				_isMoving = false;
				_moveDirection = HAucqZcqPbqeaRkOAvBFuZtmlrbX.None;
			}
			if (base.vQpmvzIqZzQRgZjNdTCOIzNihLiH == null)
			{
				Logger.LogWarning("Animation cannot be used without a Canvas.");
				P_2 = false;
			}
			else if (base.vQpmvzIqZzQRgZjNdTCOIzNihLiH.renderMode == RenderMode.WorldSpace)
			{
				Logger.LogWarning("Animation can only be used with a screen space Canvas.");
				P_2 = false;
			}
			if (P_2)
			{
				Transform parent = base.transform;
				RectTransform rectTransform = base.whnJFhhvlVIfvAzDjDUVAxyZymsH;
				Vector2 one = Vector2.one;
				while ((parent = parent.parent) != rectTransform && !(parent == null))
				{
					one.x *= parent.localScale.x;
					one.y *= parent.localScale.y;
				}
				Vector2 sizeDelta = rectTransform.sizeDelta;
				bool num = sizeDelta.x < sizeDelta.y;
				float num2 = MathTools.Max(sizeDelta.x, sizeDelta.y);
				float num3 = (num ? one.y : one.x);
				if (num3 == 0f)
				{
					num3 = 0.0001f;
				}
				P_3 = P_3 / num3 * num2;
				_coroutineMove = tIfcRAEhuytogERPthhbDYOlhzfR(P_0, P_1, P_3, P_4);
				StartCoroutine(_coroutineMove);
				_moveDirection = P_4;
				_isMovedFromDefaultPosition = true;
				sYYYyCpyZfOyIwxaAvXQFATMFmho(P_4);
			}
			else
			{
				sYYYyCpyZfOyIwxaAvXQFATMFmho(P_4);
				ROeJSkZhnkTDcLHfcKCeHhXUonug(P_4, P_0, P_1);
			}
		}

		private IEnumerator tIfcRAEhuytogERPthhbDYOlhzfR(Vector2 P_0, PositionType P_1, float P_2, HAucqZcqPbqeaRkOAvBFuZtmlrbX P_3)
		{
			return new HTHcLOaXfkEsqLumLPjjbRjLqNLx(0)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				JTKktbfSAqGDQjeacpqTtOeybKpt = P_0,
				NrdyBFMMWwnWFfEyWKBnxrSnZvnA = P_1,
				QzkoAujiXELSEATTFbUXkPALueUS = P_2,
				sGqbUlnFwRgsWfMjUESVfFgHGuQnb = P_3
			};
		}

		private void ROeJSkZhnkTDcLHfcKCeHhXUonug(HAucqZcqPbqeaRkOAvBFuZtmlrbX P_0, Vector2 P_1, PositionType P_2)
		{
			sFgWwfUJkMIxinvyEhNsisGjhMkS.jdVjbFKPJGjGyhWuwNTOVPPfnNwT(base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA, P_1, P_2);
			_isMoving = false;
			_moveDirection = HAucqZcqPbqeaRkOAvBFuZtmlrbX.None;
			switch (P_0)
			{
			case HAucqZcqPbqeaRkOAvBFuZtmlrbX.TowardHome:
				_isMovedFromDefaultPosition = false;
				break;
			case HAucqZcqPbqeaRkOAvBFuZtmlrbX.TowardTouch:
				_isMovedFromDefaultPosition = true;
				break;
			}
			zPMUivaPRCCBydGFdfPeBETKRYnmc();
			pCTEPHMlMUJAWuyayBZlAAjMxtTy(P_0);
		}

		private void XNKImEVsUNPavzxgPJMfIdoSEAWgA(HAucqZcqPbqeaRkOAvBFuZtmlrbX P_0)
		{
			if (_manageRaycasting)
			{
				bool flag = false;
				bool flag2 = false;
				if (((_followTouchPosition && stayActiveOnSwipeOut) || (!_followTouchPosition && _workingTouchRegion != null && !_useTouchRegionOnly && _moveToTouchPosition)) && _returnOnRelease && P_0 == HAucqZcqPbqeaRkOAvBFuZtmlrbX.TowardTouch)
				{
					flag = true;
					flag2 = false;
				}
				if (flag)
				{
					_imageRaycastHelper.mKNIhyiGRhrEXKxQDuaJFRiTMRVl(base.transform, flag2);
				}
			}
		}

		private void UkrWacxKoOMuRkiKLRmEmILrFhRJ(HAucqZcqPbqeaRkOAvBFuZtmlrbX P_0)
		{
			if (_manageRaycasting)
			{
				bool flag = false;
				bool flag2 = false;
				if (((_followTouchPosition && stayActiveOnSwipeOut) || (!_followTouchPosition && _workingTouchRegion != null && !_useTouchRegionOnly && _moveToTouchPosition)) && _returnOnRelease && P_0 == HAucqZcqPbqeaRkOAvBFuZtmlrbX.TowardHome)
				{
					flag = true;
					flag2 = UZuPaidSRpiDUdxlqCyJWkepnHav();
				}
				if (flag)
				{
					_imageRaycastHelper.mKNIhyiGRhrEXKxQDuaJFRiTMRVl(base.transform, flag2);
				}
			}
		}

		private void zPMUivaPRCCBydGFdfPeBETKRYnmc()
		{
			if (_coroutineMove != null)
			{
				try
				{
					StopCoroutine(_coroutineMove);
				}
				catch
				{
				}
				_coroutineMove = null;
			}
		}

		private void GmcjKJeaVVcqXFRzEcparhxCpdPsA(int P_0, Vector2 P_1, PositionType P_2)
		{
			if (TouchInteractable.uwuMHSNgPoeyIZKBbzpvCZyfdwOl(P_0))
			{
				xHovnLtQkBnCuETzVQnpnWVuJUuI((Vector2)sFgWwfUJkMIxinvyEhNsisGjhMkS.aMkDURFFHrdJnCwawIgfGyvPbVKb(base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA, P_2) + P_1, P_2, false, 0f, HAucqZcqPbqeaRkOAvBFuZtmlrbX.TowardTouch);
				if (_lastClaimSource == AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion)
				{
					_lastPressAnchoredPosition += P_1;
				}
			}
		}

		private void nxTbtLCLjbyuyRGfsvxPBPMmlZRq()
		{
			if (!hasPointer)
			{
				return;
			}
			if (!TouchInteractable.uwuMHSNgPoeyIZKBbzpvCZyfdwOl(RdKVxosdmPbtxnVbOZiHUHSPKFAe))
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
			else if (_pointerDownIsFake)
			{
				PointerEventData pointerEventData2 = lhPOsOAoYCOtTCHWjaXddmYdLcxRA(RdKVxosdmPbtxnVbOZiHUHSPKFAe, (_workingTouchRegion != null && _useTouchRegionOnly) ? _workingTouchRegion.gameObject : ((_stickTransform != null) ? _stickTransform.gameObject : base.gameObject));
				if (pointerEventData2 != null)
				{
					WxHbRRhLgInuzWPzlqWmcIMfWEbmA(pointerEventData2, _lastClaimSource);
				}
			}
		}

		private void uiycTgIPBbGghVXEDqOPpxkdfEhq()
		{
			if (hasPointer)
			{
				Vector2 vector = TouchInteractable.TgEgBYQvFnhTTkcyuYMooLsCFzzE(RdKVxosdmPbtxnVbOZiHUHSPKFAe);
				QKLGRQbsDyoftuuWluaENjTJJkboA(ref vector);
			}
		}

		private void QKLGRQbsDyoftuuWluaENjTJJkboA(ref Vector2 P_0)
		{
			if (_allowTap && _isEligibleForTap && ((_tapTimeout > 0f && Time.realtimeSinceStartup - _touchStartTime > _tapTimeout) || (_tapDistanceLimit >= 0 && Vector2.Distance(_touchStartPosition, P_0) > (float)_tapDistanceLimit)))
			{
				_isEligibleForTap = false;
			}
		}

		private bool NDhTMIQItThJsXWyCtLUVEorVCTb()
		{
			if (!_followTouchPosition)
			{
				return false;
			}
			if (_touchRegion != null && _useTouchRegionOnly)
			{
				return false;
			}
			return true;
		}

		private void rEkWtHXjAKTOKMkJBxWaofkAFFvH()
		{
			_pointerId = int.MinValue;
			_realMousePointerId = int.MinValue;
			_lastClaimSource = AQQeIWhyIPZKFavzzcGnNuzAgdWEb.Local;
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

		private void WxHbRRhLgInuzWPzlqWmcIMfWEbmA(PointerEventData P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb P_1)
		{
			if (P_0 != null)
			{
				switch (P_1)
				{
				case AQQeIWhyIPZKFavzzcGnNuzAgdWEb.Local:
					OnDrag(P_0);
					break;
				case AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion:
					smhAMMKHqpAxqkfKwVnFAxcYqokSA(P_0);
					break;
				default:
					throw new NotImplementedException();
				}
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
					int targetCount = _horizontalAxisCustomControllerElement.targetCount;
					for (int i = 0; i < targetCount; i++)
					{
						base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ClearElementValue(_horizontalAxisCustomControllerElement[i]);
					}
				}
			}
			bool flag2 = P_0 == AxisDirection.Both || P_0 == AxisDirection.Vertical;
			if (_useYAxis != flag2)
			{
				_useYAxis = flag2;
				if (!flag2 && osKcqUcyYlVGlGygpMaOnUYNqJDBA)
				{
					int targetCount2 = _verticalAxisCustomControllerElement.targetCount;
					for (int j = 0; j < targetCount2; j++)
					{
						base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ClearElementValue(_verticalAxisCustomControllerElement[j]);
					}
				}
			}
			_axesToUse = P_0;
		}

		private void kenMZNZStIQnTAavioStPdqUwyIF(PointerEventData P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb P_1)
		{
			if (!hasPointer || SBhvinEbIDGWUMwfXythhrIpigKu(P_0.pointerId))
			{
				if (EDufGzVNigBlMAOWvMGsHZmtQaph() && IsInteractable())
				{
					PARBcfOJshIGNJtBDsuAyiETdYwy(P_0.pointerId, P_0.pressPosition, P_1);
				}
				base.OnPointerDown(P_0);
			}
		}

		private void ZEbUZdUWFIEfiSqeUnrGJHjcPybD(PointerEventData P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb P_1)
		{
			if ((!hasPointer || SBhvinEbIDGWUMwfXythhrIpigKu(P_0.pointerId)) && !TouchInteractable.uwuMHSNgPoeyIZKBbzpvCZyfdwOl(RdKVxosdmPbtxnVbOZiHUHSPKFAe))
			{
				MHcBbLUbNvVfJsmQBGwoxkdhumdt();
				base.OnPointerUp(P_0);
			}
		}

		private void ZWAzvWwAMRawEGwPixAMPjfyqNgpA(PointerEventData P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb P_1)
		{
			if (hasPointer && !SBhvinEbIDGWUMwfXythhrIpigKu(P_0.pointerId))
			{
				return;
			}
			bool flag = TouchInteractable.sWBkWrgBUhjXAreNdBTEqWiNGgHjA(P_0.pointerId);
			bool flag2 = false;
			MouseButtonFlags mouseButtonFlags = P_1 switch
			{
				AQQeIWhyIPZKFavzzcGnNuzAgdWEb.Local => base.allowedMouseButtons, 
				AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion => _touchRegion.allowedMouseButtons, 
				_ => throw new NotImplementedException(), 
			};
			if (_activateOnSwipeIn && EDufGzVNigBlMAOWvMGsHZmtQaph() && IsInteractable() && (!flag || TouchInteractable.LTWUvUGhphDXhnFmUjsOImULiiXRA(mouseButtonFlags)) && !ALBJgfsRYJdMVJwePTiUwIfQxDNM)
			{
				if (flag)
				{
					if (TouchInteractable.ZbAsyupCrOtBnZlFjdZxrpdrFStI(mouseButtonFlags, out var realMousePointerId))
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
				GameObject gameObject = P_1 switch
				{
					AQQeIWhyIPZKFavzzcGnNuzAgdWEb.Local => base.gameObject, 
					AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion => _workingTouchRegion.gameObject, 
					_ => throw new NotImplementedException(), 
				};
				PointerEventData pointerEventData = utfJPhgnwTqKykeFFJYQHUTouefi((_realMousePointerId != int.MinValue) ? _realMousePointerId : P_0.pointerId, gameObject);
				if (pointerEventData != null)
				{
					kenMZNZStIQnTAavioStPdqUwyIF(pointerEventData, P_1);
					if (ALBJgfsRYJdMVJwePTiUwIfQxDNM)
					{
						_pointerDownIsFake = true;
					}
				}
			}
			zmNsczlnaJKPNGfDEeyyFdsXhpjgb = true;
		}

		private void zHqZkjRbrFTkqMfkkoUpBAuhCzKfA(PointerEventData P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb P_1)
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

		private void ojGbWuDiNTcQBJhQBRTYbvGTjlqpA(PointerEventData P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb P_1)
		{
			if (hasPointer && SBhvinEbIDGWUMwfXythhrIpigKu(P_0.pointerId))
			{
				base.OnBeginDrag(P_0);
			}
		}

		private void NjjRPLcHoUjpwCFvtzZQbZnNETGqA(PointerEventData P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb P_1)
		{
			if (!hasPointer || !SBhvinEbIDGWUMwfXythhrIpigKu(P_0.pointerId))
			{
				return;
			}
			RectTransform rectTransform = CemuTvKBxSlWusQeIceOjYYrEMlr;
			Vector2 vector = ((!_snapStickToTouch) ? _lastPressAnchoredPosition : sFgWwfUJkMIxinvyEhNsisGjhMkS.AiXkiJjjQxAhaFhTkYgcoVKAkfwS(base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA, rectTransform, base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA.rect.center));
			if (!_centerStickOnRelease && !_snapStickToTouch)
			{
				vector -= _lastPressStartingValue * QNkuiWUZthEWSYPljzmqFaGIdebL;
			}
			Vector2 vector2 = sFgWwfUJkMIxinvyEhNsisGjhMkS.jbBxkZzbJVPbDkWGyaxqxDcSTVxt(base.vQpmvzIqZzQRgZjNdTCOIzNihLiH, rectTransform, P_0.position);
			Vector2 vector3 = new Vector2(_useXAxis ? (vector2.x - vector.x) : 0f, _useYAxis ? (vector2.y - vector.y) : 0f);
			Vector2 vector4;
			if (_stickBounds == StickBounds.Circle)
			{
				vector4 = Vector2.ClampMagnitude(vector3, QNkuiWUZthEWSYPljzmqFaGIdebL);
			}
			else
			{
				if (_stickBounds != StickBounds.Square)
				{
					throw new NotImplementedException();
				}
				vector4 = MathTools.Clamp(vector3, 0f - QNkuiWUZthEWSYPljzmqFaGIdebL, QNkuiWUZthEWSYPljzmqFaGIdebL);
			}
			Vector2 rawValue = vector4 / QNkuiWUZthEWSYPljzmqFaGIdebL;
			SetRawValue(rawValue);
			if (_followTouchPosition)
			{
				if (_stickBounds == StickBounds.Circle)
				{
					if (vector3.sqrMagnitude > QNkuiWUZthEWSYPljzmqFaGIdebL)
					{
						Vector2 vector5 = new Vector2(_useXAxis ? (vector3.x - vector4.x) : 0f, _useXAxis ? (vector3.y - vector4.y) : 0f);
						GmcjKJeaVVcqXFRzEcparhxCpdPsA(RdKVxosdmPbtxnVbOZiHUHSPKFAe, vector5, PositionType.Anchored);
					}
				}
				else
				{
					if (_stickBounds != StickBounds.Square)
					{
						throw new NotImplementedException();
					}
					bool flag = Mathf.Abs(vector3.x) > QNkuiWUZthEWSYPljzmqFaGIdebL;
					bool flag2 = Mathf.Abs(vector3.y) > QNkuiWUZthEWSYPljzmqFaGIdebL;
					if (flag || flag2)
					{
						Vector2 vector6 = new Vector2((_useXAxis && flag) ? (vector3.x - vector4.x) : 0f, (_useXAxis && flag2) ? (vector3.y - vector4.y) : 0f);
						GmcjKJeaVVcqXFRzEcparhxCpdPsA(RdKVxosdmPbtxnVbOZiHUHSPKFAe, vector6, PositionType.Anchored);
					}
				}
			}
			base.OnDrag(P_0);
		}

		private void BLlpBBqEIQYHGAZMZaujMMEjFrsdA(PointerEventData P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb P_1)
		{
			if (hasPointer && SBhvinEbIDGWUMwfXythhrIpigKu(P_0.pointerId))
			{
				base.OnEndDrag(P_0);
			}
		}

		private void PARBcfOJshIGNJtBDsuAyiETdYwy(int P_0, Vector2 P_1, AQQeIWhyIPZKFavzzcGnNuzAgdWEb P_2)
		{
			_pointerId = P_0;
			_lastClaimSource = P_2;
			_isEligibleForTap = true;
			_lastPressAnchoredPosition = sFgWwfUJkMIxinvyEhNsisGjhMkS.jbBxkZzbJVPbDkWGyaxqxDcSTVxt(base.vQpmvzIqZzQRgZjNdTCOIzNihLiH, CemuTvKBxSlWusQeIceOjYYrEMlr, P_1);
			ALBJgfsRYJdMVJwePTiUwIfQxDNM = true;
			_lastPressStartingValue.x = MathTools.Clamp(_axis2D.value.x, -1f, 1f);
			_lastPressStartingValue.y = MathTools.Clamp(_axis2D.value.y, -1f, 1f);
			_touchStartTime = Time.realtimeSinceStartup;
			_touchStartPosition = P_1;
			if (P_2 == AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion && (_moveToTouchPosition || _followTouchPosition))
			{
				if (_followTouchPosition)
				{
					jbQgKcxKQGzETsTgYjuvYckRsvZo(P_1, false, 0f, HAucqZcqPbqeaRkOAvBFuZtmlrbX.TowardTouch);
				}
				else
				{
					jbQgKcxKQGzETsTgYjuvYckRsvZo(P_1, _animateOnMoveToTouch, _moveToTouchSpeed, HAucqZcqPbqeaRkOAvBFuZtmlrbX.TowardTouch);
				}
			}
			if (_onTouchStarted != null)
			{
				_onTouchStarted.Invoke();
			}
			PointerEventData pointerEventData = lhPOsOAoYCOtTCHWjaXddmYdLcxRA(_pointerId, (P_2 == AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion) ? _workingTouchRegion.gameObject : ((_stickTransform != null) ? _stickTransform.gameObject : base.gameObject));
			if (pointerEventData != null)
			{
				WxHbRRhLgInuzWPzlqWmcIMfWEbmA(pointerEventData, P_2);
			}
		}

		private void MHcBbLUbNvVfJsmQBGwoxkdhumdt()
		{
			rEkWtHXjAKTOKMkJBxWaofkAFFvH();
			bool num = _allowTap && _isEligibleForTap;
			ALBJgfsRYJdMVJwePTiUwIfQxDNM = false;
			_pointerDownIsFake = false;
			_lastPressAnchoredPosition = Vector2.zero;
			_lastPressStartingValue = Vector2.zero;
			if ((_followTouchPosition || _moveToTouchPosition) && _returnOnRelease && _isMovedFromDefaultPosition)
			{
				ReturnToDefaultPosition();
			}
			if (_centerStickOnRelease)
			{
				SetRawValue(_axis2D.rawZero);
			}
			if (_onTouchEnded != null)
			{
				_onTouchEnded.Invoke();
			}
			_isEligibleForTap = false;
			if (num)
			{
				_lastTapFrame = Time.frameCount + 1;
				_onTap.Invoke();
			}
		}

		internal void MbXbjtguZaBfbfqmjlBSCyVMORejB(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerUp) && (!(_workingTouchRegion != null) || !_useTouchRegionOnly))
			{
				ZEbUZdUWFIEfiSqeUnrGJHjcPybD(P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb.Local);
			}
		}

		internal void jdTaWudIAtigvcgWdtrrcBEKJLcPB(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerDown) && (!(_workingTouchRegion != null) || !_useTouchRegionOnly))
			{
				kenMZNZStIQnTAavioStPdqUwyIF(P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb.Local);
			}
		}

		internal void vpQLxEpNsNjHEQUMTNafaJqaWgfe(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerEnter) && (!(_workingTouchRegion != null) || !_useTouchRegionOnly))
			{
				ZWAzvWwAMRawEGwPixAMPjfyqNgpA(P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb.Local);
			}
		}

		internal void OCiTmGENfjrRiUQnHUcKDaHRzTDj(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerExit) && (!(_workingTouchRegion != null) || !_useTouchRegionOnly))
			{
				zHqZkjRbrFTkqMfkkoUpBAuhCzKfA(P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb.Local);
			}
		}

		internal void CeCjfxfrPRDIAfCVrQPnmMvELNpzA(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.BeginDrag) && (!(_workingTouchRegion != null) || !_useTouchRegionOnly))
			{
				ojGbWuDiNTcQBJhQBRTYbvGTjlqpA(P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb.Local);
			}
		}

		internal void JuzlkzmoAOuJMYLEkFxWgiAjnolW(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.Drag) && (!(_workingTouchRegion != null) || !_useTouchRegionOnly))
			{
				NjjRPLcHoUjpwCFvtzZQbZnNETGqA(P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb.Local);
			}
		}

		internal void LgBQosPAyBTzQOLHtYNFjeRHthOd(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.EndDrag) && (!(_workingTouchRegion != null) || !_useTouchRegionOnly))
			{
				BLlpBBqEIQYHGAZMZaujMMEjFrsdA(P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb.Local);
			}
		}

		private void lEDjUpnxqBucRFkEDyJKbVEmsmkA(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerDown))
			{
				kenMZNZStIQnTAavioStPdqUwyIF(P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion);
			}
		}

		private void EuQQZFwdcthAzHoBOBDBcyHGjRwyB(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerUp))
			{
				ZEbUZdUWFIEfiSqeUnrGJHjcPybD(P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion);
			}
		}

		private void OmQwIffSZPImRUCIYEOwFrZRxIQW(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerEnter))
			{
				ZWAzvWwAMRawEGwPixAMPjfyqNgpA(P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion);
			}
		}

		private void PnDoCbelFWRcICvBVIgnpCPKYTkI(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerExit))
			{
				zHqZkjRbrFTkqMfkkoUpBAuhCzKfA(P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion);
			}
		}

		private void ElBBXVTrkqqOSaAcSRJhTyyORDhq(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.BeginDrag))
			{
				ojGbWuDiNTcQBJhQBRTYbvGTjlqpA(P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion);
			}
		}

		private void smhAMMKHqpAxqkfKwVnFAxcYqokSA(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.Drag))
			{
				NjjRPLcHoUjpwCFvtzZQbZnNETGqA(P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion);
			}
		}

		private void xApIcLqpCsqKyGaPkToTqMKtCkTcA(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.EndDrag))
			{
				BLlpBBqEIQYHGAZMZaujMMEjFrsdA(P_0, AQQeIWhyIPZKFavzzcGnNuzAgdWEb.TouchRegion);
			}
		}

		private void ghoFULOHEeeqyJlbKyZkQSyDIDpV(Vector2 P_0)
		{
			MyVxLtVZLEiRaloNqnuyxYfDkCJB(null);
			Vector2 value = P_0;
			if (_axis2D.xAxis.calibration.invert)
			{
				value.x *= -1f;
			}
			if (_axis2D.yAxis.calibration.invert)
			{
				value.y *= -1f;
			}
			value = MathTools.Clamp(value, -1f, 1f);
			if (_stickTransform != null)
			{
				RectTransform rectTransform = CemuTvKBxSlWusQeIceOjYYrEMlr;
				Vector3 position = value * QNkuiWUZthEWSYPljzmqFaGIdebL;
				position += rectTransform.InverseTransformPoint(base.transform.position);
				Vector3 position2 = rectTransform.TransformPoint(position);
				Vector3 vector = _stickTransform.parent.InverseTransformPoint(position2);
				Vector2 anchoredPosition = sFgWwfUJkMIxinvyEhNsisGjhMkS.ZytRLSzbasMIqArulEmHcbPhWPbU(_stickTransform.parent as RectTransform, vector);
				anchoredPosition += _origStickAnchoredPosition;
				_stickTransform.anchoredPosition = anchoredPosition;
			}
			_hierarchyValueChangedHandlers.ExecuteOnAll(P_0);
			_hierarchyStickPositionChangedHandlers.ExecuteOnAll(value);
			_onValueChanged.Invoke(P_0);
			_onStickPositionChanged.Invoke(value);
		}
	}
}
