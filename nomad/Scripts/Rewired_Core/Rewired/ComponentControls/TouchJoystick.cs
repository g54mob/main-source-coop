using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
	[DisallowMultipleComponent]
	[AddComponentMenu("Rewired/Touch Controls/Touch Joystick")]
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

		private enum vkWJVTUNgFBXvHfhceNDsBmGzzqz
		{
			None = 0,
			TowardTouch = 1,
			TowardHome = 2
		}

		private enum eistIGDtvClGGRARBOzaNeJsvHzB
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
		private sealed class LVPgiwEfRFDqBhECdVjfVTPildcPB
		{
			public static readonly LVPgiwEfRFDqBhECdVjfVTPildcPB _003C_003E9 = new LVPgiwEfRFDqBhECdVjfVTPildcPB();

			public static LTiqEoSaGSGvCpWQDOcWSGOWaNVX.EventFunction<IValueChangedHandler, Vector2> _003C_003E9__277_0;

			public static LTiqEoSaGSGvCpWQDOcWSGOWaNVX.EventFunction<IStickPositionChangedHandler, Vector2> _003C_003E9__280_0;

			internal void FiuwadzBqTPOPosmhOFVXZPQqbAY(IValueChangedHandler P_0, Vector2 P_1)
			{
				P_0.OnValueChanged(P_1);
			}

			internal void fuvrNKGUPLoFFtwnCMbRGulrmPTf(IStickPositionChangedHandler P_0, Vector2 P_1)
			{
				P_0.OnStickPositionChanged(P_1);
			}
		}

		private sealed class frlhcQAZKIHVrvDRlpnfEfuftPWN : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int UzgcucEcxQNUScETaItOMJFsZVMYA;

			private object MzcghGRDYZvfcKCIiOcKlkdNhGGr;

			public float dvnGFQsywnDTuJEIcJkUZvwmnNpd;

			public TouchJoystick RvAbxSMwlbfAqDdZKpbdyDMzaPzvA;

			public PositionType QWKfVwcPtOYIZqHvSDlhqYhhsVFM;

			public Vector2 gMivppHjaXgvuCfxJjpYqGmhTrbDA;

			public vkWJVTUNgFBXvHfhceNDsBmGzzqz eSITFdRvjPHqWFHfoRwkIGjXGwpm;

			private RectTransform UAVrnNplqZbNNcdkzjBYDYuqFFTeA;

			private Vector2 PpozjpsNzBAjzsvCBeyNNiXPGaeD;

			private float dyGvIpQnvASaQZHAVJucfjANRQeJ;

			private float pyXmVQKilTabjrvcsddAiofFLOND;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return MzcghGRDYZvfcKCIiOcKlkdNhGGr;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return MzcghGRDYZvfcKCIiOcKlkdNhGGr;
				}
			}

			[DebuggerHidden]
			public frlhcQAZKIHVrvDRlpnfEfuftPWN(int P_0)
			{
				UzgcucEcxQNUScETaItOMJFsZVMYA = P_0;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				UAVrnNplqZbNNcdkzjBYDYuqFFTeA = null;
				UzgcucEcxQNUScETaItOMJFsZVMYA = -2;
			}

			private bool MoveNext()
			{
				int uzgcucEcxQNUScETaItOMJFsZVMYA = UzgcucEcxQNUScETaItOMJFsZVMYA;
				TouchJoystick rvAbxSMwlbfAqDdZKpbdyDMzaPzvA = RvAbxSMwlbfAqDdZKpbdyDMzaPzvA;
				if (uzgcucEcxQNUScETaItOMJFsZVMYA != 0)
				{
					if (uzgcucEcxQNUScETaItOMJFsZVMYA != 1)
					{
						return false;
					}
					UzgcucEcxQNUScETaItOMJFsZVMYA = -1;
					goto IL_010c;
				}
				UzgcucEcxQNUScETaItOMJFsZVMYA = -1;
				if (!(dvnGFQsywnDTuJEIcJkUZvwmnNpd <= 0f))
				{
					UAVrnNplqZbNNcdkzjBYDYuqFFTeA = rvAbxSMwlbfAqDdZKpbdyDMzaPzvA.XgRjXqPEGoTEAcTuxzCYeFqkaaly;
					PpozjpsNzBAjzsvCBeyNNiXPGaeD = ItUXShgHgqMTvNPoaDBaDFBZYvtd.BrgGlubhkUYkSMDAYAtTVxcWogbn(UAVrnNplqZbNNcdkzjBYDYuqFFTeA, QWKfVwcPtOYIZqHvSDlhqYhhsVFM);
					float magnitude = (gMivppHjaXgvuCfxJjpYqGmhTrbDA - PpozjpsNzBAjzsvCBeyNNiXPGaeD).magnitude;
					if (!(magnitude < 0.01f))
					{
						rvAbxSMwlbfAqDdZKpbdyDMzaPzvA._isMoving = true;
						dyGvIpQnvASaQZHAVJucfjANRQeJ = magnitude / dvnGFQsywnDTuJEIcJkUZvwmnNpd;
						pyXmVQKilTabjrvcsddAiofFLOND = 0f;
						goto IL_010c;
					}
				}
				goto IL_0119;
				IL_0119:
				rvAbxSMwlbfAqDdZKpbdyDMzaPzvA.UpcPQfceWmNfTawSGtGhyGTseKKR(eSITFdRvjPHqWFHfoRwkIGjXGwpm, gMivppHjaXgvuCfxJjpYqGmhTrbDA, QWKfVwcPtOYIZqHvSDlhqYhhsVFM);
				return false;
				IL_010c:
				if (pyXmVQKilTabjrvcsddAiofFLOND <= 1f)
				{
					pyXmVQKilTabjrvcsddAiofFLOND += Time.unscaledDeltaTime / dyGvIpQnvASaQZHAVJucfjANRQeJ;
					ItUXShgHgqMTvNPoaDBaDFBZYvtd.AVBaoIMoopyfpbSwkSCPNfEaGuXL(UAVrnNplqZbNNcdkzjBYDYuqFFTeA, Vector2.Lerp(PpozjpsNzBAjzsvCBeyNNiXPGaeD, gMivppHjaXgvuCfxJjpYqGmhTrbDA, Mathf.SmoothStep(0f, 1f, pyXmVQKilTabjrvcsddAiofFLOND)), QWKfVwcPtOYIZqHvSDlhqYhhsVFM);
					MzcghGRDYZvfcKCIiOcKlkdNhGGr = null;
					UzgcucEcxQNUScETaItOMJFsZVMYA = 1;
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

		[Tooltip("The Custom Controller element(s) that will receive input values from the joystick's X axis.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CustomControllerElementTargetSetForFloat _horizontalAxisCustomControllerElement = new CustomControllerElementTargetSetForFloat();

		[Tooltip("The Custom Controller element(s) that will receive input values from the joystick's Y axis.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CustomControllerElementTargetSetForFloat _verticalAxisCustomControllerElement = new CustomControllerElementTargetSetForFloat();

		[Tooltip("The Custom Controller element that will receive input values from taps.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CustomControllerElementTargetSetForBoolean _tapCustomControllerElement = new CustomControllerElementTargetSetForBoolean();

		[Tooltip("The Rect Transform of the stick disc. This is moved around by the user when manipulating the joystick.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private RectTransform _stickTransform;

		[Tooltip("The joystick's mode of operation. Set this to Digital to simulate a D-Pad which has only On/Off states. If you want mimic a real D-Pad, you should also set Snap Directions to 8.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private JoystickMode _joystickMode;

		[Tooltip("A dead zone which is applied when Stick Mode is set to Digital. This is used to filter out tiny stick movements near 0, 0.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Range(0f, 1f)]
		private float _digitalModeDeadZone = 0.3f;

		[Tooltip("The range of movement of the stick in Canvas pixels. The larger the number, the further the stick must be moved from center to register movement.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Range(0.01f, 1000f)]
		private float _stickRange = 60f;

		[Tooltip("If enabled, the stick range will scale with parent controls. Otherwise, the stick range will remain constant.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _scaleStickRange = true;

		[Tooltip("The shape of the range of movement of the joystick.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private StickBounds _stickBounds;

		[Tooltip("The axis directions in which movement is allowed. You can restrict movement to one or both axes.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private AxisDirection _axesToUse;

		[Tooltip("Snaps joystick movement to a fixed number of directions. This can be used to create a D-Pad, for example, setting it to 4 or 8 directions. If you want a true D-Pad, Stick Mode should be set to digital.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private SnapDirections _snapDirections;

		[Tooltip("If true, the stick disc will snap immediately to the touch position when initially touched. This results in the stick disc being centered to the touch position. This will cause the stick to generate input immediately when touched if not touched perfectly centered.If false, the stick disc will remain in its current position on touch, and when dragged will retain the same offset. The stick's center point will be set to the position of the touch. The initial touch will not cause the stick to pop in any direction.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _snapStickToTouch;

		[Tooltip("If true, the stick will return to the center after it is released. Otherwise, the stick will remain in the last position and continue to return input.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _centerStickOnRelease = true;

		[Tooltip("The underlying Axis 2D.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private StandaloneAxis2D _axis2D = new StandaloneAxis2D();

		[Tooltip("If true, the joystick can be activated by a touch swipe that began in an area outside the joystick region. If false, the joystick can only be activated by a direct touch.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _activateOnSwipeIn;

		[Tooltip("If true, the joystick will stay engaged even if the touch that activated it moves outside the joystick region. If false, the joystick will be released once the touch that activated it moves outside the joystick region.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _stayActiveOnSwipeOut = true;

		[Tooltip("Should taps on the touch pad be processed?")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _allowTap;

		[Tooltip("The maximum touch duration allowed for the touch to be considered a tap. A touch that lasts longer than this value will not trigger a tap when released.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[FieldRange(0f, 3.4028235E+38f)]
		private float _tapTimeout = 0.25f;

		[Tooltip("The maximum movement distance allowed in pixels since the touch began for the touch to be considered a tap. [-1 = no limit]")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[FieldRange(-1, 2147483647)]
		private int _tapDistanceLimit = 10;

		[Tooltip("Optional external region to use for hover/click/touch detection. If set, this region will be used for touch detection instead of or in addition to the joystick's RectTransform. This can be useful if you want a larger area of the screen to act as a joystick.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private TouchRegion _touchRegion;

		[Tooltip("If True, hovers/clicks/touches on the local joystick will be ignored and only Touch Region touches will be used. Otherwise, both touches on the joystick and on the Touch Region will be used. This also applies to mouse hover. This setting has no effect if no Touch Region is set.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _useTouchRegionOnly = true;

		[Tooltip("If True, the joystick will move to the location of the current touch in the Touch Region. This can be used to designate an area of the screen as a hot-spot for a joystick and have the joystick graphics follow the users touches. This only has an effect if a Touch Region is set.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _moveToTouchPosition;

		[Tooltip("If Move To Touch Position is enabled, this will make the joystick return to its original position after the press is released. This only has an effect if a Touch Region is set.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _returnOnRelease = true;

		[Tooltip("If True, the joystick will follow the touch around until released. This setting overrides Move To Touch Position.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _followTouchPosition;

		[Tooltip("Should the joystick animate when moving to the touch point? This only has an effect if Move To Touch Position is True and a Touch Region is set. This setting is ignored if Follow Touch Position is True.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _animateOnMoveToTouch = true;

		[Tooltip("The speed at which the joystick will move toward the touch position measured in screens per second (based on the larger of width and height). [1.0 = Move 1 screen/sec]. This only has an effect if Move To Touch Position is True, Animate On Move To Touch is true, and a Touch Region is set. This setting is ignored if Follow Touch Position is True.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Range(0f, 20f)]
		private float _moveToTouchSpeed = 2f;

		[Tooltip("Should the joystick animate when moving back to its original position? This only has an effect if Follow Touch Position is True, or if Move To Touch Position is True and a Touch Region is set, and Return on Release is True.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _animateOnReturn = true;

		[Tooltip("The speed at which the joystick will move back toward its original position measured in screens per second (based on the larger of width and height). [1.0 = Move 1 screen/sec]. This only has an effect if Follow Touch Position is True, or if Move To Touch Position is True and a Touch Region is set, and Return on Release and Animate on Return are both True.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Range(0f, 20f)]
		private float _returnSpeed = 2f;

		[Tooltip("If True, it will attempt to automatically manage Graphic component raycasting for best results based on your current settings.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _manageRaycasting = true;

		private bool _useXAxis;

		private bool _useYAxis;

		private LTiqEoSaGSGvCpWQDOcWSGOWaNVX.HierarchyEventHelper<IValueChangedHandler, Vector2> _hierarchyValueChangedHandlers;

		private LTiqEoSaGSGvCpWQDOcWSGOWaNVX.HierarchyEventHelper<IStickPositionChangedHandler, Vector2> _hierarchyStickPositionChangedHandlers;

		private TouchRegion _workingTouchRegion;

		private Vector2 _origAnchoredPosition;

		private Vector2 _origStickAnchoredPosition;

		private Vector2 _lastPressAnchoredPosition;

		private bool _isMoving;

		private bool _isMovedFromDefaultPosition;

		private vkWJVTUNgFBXvHfhceNDsBmGzzqz _moveDirection;

		private int _pointerId = -2147483648;

		private int _realMousePointerId = -2147483648;

		[NonSerialized]
		private bool HYwbjXFdakovosWZxzzDtDnrmTHRA;

		[NonSerialized]
		private bool zsERuCTpLVOqaWcMsqUUAexgtUFm;

		private bool _pointerDownIsFake;

		private Vector2 _lastPressStartingValue;

		private eistIGDtvClGGRARBOzaNeJsvHzB _lastClaimSource;

		private float _touchStartTime;

		private Vector2 _touchStartPosition;

		private IEnumerator _coroutineMove;

		private KbsjaYbXcZsfBGHFCqdYNahakHjI _imageRaycastHelper = new KbsjaYbXcZsfBGHFCqdYNahakHjI();

		private int _calculatedStickRange_lastUpdatedFrame = -1;

		private int _lastTapFrame = -1;

		private bool _isEligibleForTap;

		private float __calculatedStickRange_cachedValue;

		private Action<vkWJVTUNgFBXvHfhceNDsBmGzzqz> __moveStartedDelegate;

		private Action<vkWJVTUNgFBXvHfhceNDsBmGzzqz> __moveEndedDelegate;

		[Tooltip("Event sent when the joystick value changes.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ValueChangedEventHandler _onValueChanged = new ValueChangedEventHandler();

		[Tooltip("Event sent when the joystick's stick position changes.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ValueChangedEventHandler _onStickPositionChanged = new ValueChangedEventHandler();

		[Tooltip("Event sent when the joystick is touched.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private TouchStartedEventHandler _onTouchStarted = new TouchStartedEventHandler();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		private TouchEndedEventHandler _onTouchEnded = new TouchEndedEventHandler();

		[Tooltip("Event sent when the touch pad is tapped. This event will only be sent if allowTap is True.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private TapEventHandler _onTap = new TapEventHandler();

		private Dictionary<int, PointerEventData> __fakePointerEventData;

		private static LTiqEoSaGSGvCpWQDOcWSGOWaNVX.EventFunction<IValueChangedHandler, Vector2> __valueChangedHandlerDelegate;

		private static LTiqEoSaGSGvCpWQDOcWSGOWaNVX.EventFunction<IStickPositionChangedHandler, Vector2> __stickPositionChangedHandlerDelegate;

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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
				}
			}
		}

		private StickBounds uyoVGdYrDWTwIrwFYsNorupeVWan
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					vMFIiAaaGNmLxVqtWqeZEfxTeFhp(value);
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
				}
			}
		}

		public bool stayActiveOnSwipeOut
		{
			get
			{
				if (OErggtDFItJysteeKDSNMsHGCqKWA())
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
						jTdzsZFdcqgLHcTcbjxpUHscGgejA();
					}
					else
					{
						_imageRaycastHelper.WnkVoLeLcDIxHhUkiCBDfHgIYoxub();
					}
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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

		public bool hasPointer => _pointerId != -2147483648;

		private bool fHMsZUBYHnvNcZsyYHFTKdtsVzfT => _lastTapFrame == Time.frameCount;

		internal StandaloneAxis2D CxXGtmChAsmgkHOOnmLLHOjUbabVA => _axis2D;

		private Action<vkWJVTUNgFBXvHfhceNDsBmGzzqz> wIFWxlDYChhaQxNtZiZxoQjtcHMB
		{
			get
			{
				if (__moveStartedDelegate == null)
				{
					return __moveStartedDelegate = gDSToxnBFAVdUKALkeFYsOIUhkkb;
				}
				return __moveStartedDelegate;
			}
		}

		private Action<vkWJVTUNgFBXvHfhceNDsBmGzzqz> rDeAduOaLRCywcjmJoYrBJTBUrJUB
		{
			get
			{
				if (__moveEndedDelegate == null)
				{
					return __moveEndedDelegate = ixWJtCnVLOccJDbxbHnCwVpSmgxx;
				}
				return __moveEndedDelegate;
			}
		}

		private int zIAcsLBAnaznJRdMrArlAGtkGoBBb
		{
			get
			{
				if (_pointerId == -2147483648)
				{
					return -2147483648;
				}
				if (_realMousePointerId != -2147483648)
				{
					return _realMousePointerId;
				}
				return _pointerId;
			}
		}

		private RectTransform iPXbsIzAbZdRxRwfZPEDWniusguD
		{
			get
			{
				if (_lastClaimSource != eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion)
				{
					return base.transform as RectTransform;
				}
				return base.transform.parent as RectTransform;
			}
		}

		private float QJMwKlJQhKFibmpZSjwWYsWHVrmB
		{
			get
			{
				if (Time.frameCount == _calculatedStickRange_lastUpdatedFrame)
				{
					return __calculatedStickRange_cachedValue;
				}
				RectTransform rectTransform = base.cceFGuGrUOPVoQFhXoaprVXRKreu;
				RectTransform rectTransform2 = iPXbsIzAbZdRxRwfZPEDWniusguD;
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
					if (_lastClaimSource == eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion)
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

		internal static LTiqEoSaGSGvCpWQDOcWSGOWaNVX.EventFunction<IValueChangedHandler, Vector2> thZZiurtynYOddxFVMzVZoReLCrh
		{
			get
			{
				if (__valueChangedHandlerDelegate == null)
				{
					__valueChangedHandlerDelegate = LVPgiwEfRFDqBhECdVjfVTPildcPB._003C_003E9.FiuwadzBqTPOPosmhOFVXZPQqbAY;
				}
				return __valueChangedHandlerDelegate;
			}
		}

		internal static LTiqEoSaGSGvCpWQDOcWSGOWaNVX.EventFunction<IStickPositionChangedHandler, Vector2> iZqJVoKdFbdKuaqMRbxIAhIVUJGS
		{
			get
			{
				if (__stickPositionChangedHandlerDelegate == null)
				{
					__stickPositionChangedHandlerDelegate = LVPgiwEfRFDqBhECdVjfVTPildcPB._003C_003E9.fuvrNKGUPLoFFtwnCMbRGulrmPTf;
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
			if (!base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				return _axis2D.rawZero;
			}
			return _axis2D.value;
		}

		public Vector2 GetRawValue()
		{
			if (!base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				return _axis2D.rawZero;
			}
			return _axis2D.rawValue;
		}

		public void SetRawValue(Vector2 value)
		{
			if (!base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
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
			KiZKWDbqyQtswXjLnhNEtksFvnAx(base.XgRjXqPEGoTEAcTuxzCYeFqkaaly.anchoredPosition);
		}

		private void KiZKWDbqyQtswXjLnhNEtksFvnAx(Vector2 P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				_origAnchoredPosition = P_0;
			}
		}

		public void ReturnToDefaultPosition(bool instant)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				OURLDMVklodxslUMskgHvHflsMwQ(_origAnchoredPosition, PositionType.Anchored, !instant && _animateOnReturn, _returnSpeed, vkWJVTUNgFBXvHfhceNDsBmGzzqz.TowardHome);
			}
		}

		public void ReturnToDefaultPosition()
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
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
				_origAnchoredPosition = base.XgRjXqPEGoTEAcTuxzCYeFqkaaly.anchoredPosition;
				if (_stickTransform != null)
				{
					_origStickAnchoredPosition = _stickTransform.anchoredPosition;
				}
				SetRawValue(CxXGtmChAsmgkHOOnmLLHOjUbabVA.rawZero);
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnEnable()
		{
			base.OnEnable();
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				XsKjeeFYVlWvjgMttYapqawCBLtL();
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnDisable()
		{
			base.OnDisable();
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				_axis2D.Deinitialize();
				oupESpAKdTodzbcjadIjdJbxShhrb();
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnValidate()
		{
			base.OnValidate();
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				BXMwTYFUsamJZToDFfUcgQDBPnmP();
				XsKjeeFYVlWvjgMttYapqawCBLtL();
			}
		}

		internal void FPgbBmdYpIsCVRRpQAiQjDLAeCFgc()
		{
			base.MJCnBosQYbUlJanFrVIDjtRnnxaI();
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				YzCcFkQygeyMWbEeFlzuOltDPqpJ();
				FbFoaRKPMoRkGpOjmsqjOiaFbcSJ();
				sKyWRUCiXzBPjPbrSaadxxAtTfHV();
			}
		}

		internal bool IuqtnNREatdVQhZaexBBfhTaGvfR()
		{
			if (!ngChnvFyiPBLCncDhdTerijdqmaY())
			{
				return false;
			}
			BXMwTYFUsamJZToDFfUcgQDBPnmP();
			_axis2D.Initialize();
			return true;
		}

		internal void syRpPRvcgmGwrbDUmvgJWmHYtQUeA()
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && ghAsJwRhDmClYOgMqzKuSmomibZfA)
			{
				Vector2 value = _axis2D.value;
				if (_useXAxis)
				{
					GLmFZCmUYCmgFyInFMQJxMkuRoJw(_horizontalAxisCustomControllerElement, value.x, _axis2D.xAxis.buttonActivationThreshold);
				}
				if (_useYAxis)
				{
					GLmFZCmUYCmgFyInFMQJxMkuRoJw(_verticalAxisCustomControllerElement, value.y, _axis2D.yAxis.buttonActivationThreshold);
				}
				if (_allowTap)
				{
					mtSnwsaofzmMqmFsUdCcwVcPzKvK(_tapCustomControllerElement, fHMsZUBYHnvNcZsyYHFTKdtsVzfT);
				}
			}
		}

		internal void mWLjTAqPTeiNOTrEuenjlUgKIcWO()
		{
			KXZAZBKCBLiVYWwSNbnniwYSIrBkA();
			_axis2D.ValueChangedEvent += mbyZkjVLnQGryBgZWdlwsnjFqvlJ;
		}

		internal void wZZryhoWyUARYfqLDICHanTSglcT()
		{
			rYnUOltSwBtwnpEBEVDesiwZDlZGA();
			_axis2D.ValueChangedEvent -= mbyZkjVLnQGryBgZWdlwsnjFqvlJ;
		}

		internal void INieZPaToCoAJzVfqeskyOxrcdhD()
		{
			ZFNYLzeTNLUpKCdifbpdezFLAlNRA();
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				BXMwTYFUsamJZToDFfUcgQDBPnmP();
				XsKjeeFYVlWvjgMttYapqawCBLtL();
			}
		}

		internal void WWMCKhhRmQwpGzUOkHVKYRgDaLAy()
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				_pointerId = -2147483648;
				_realMousePointerId = -2147483648;
				HYwbjXFdakovosWZxzzDtDnrmTHRA = false;
				zsERuCTpLVOqaWcMsqUUAexgtUFm = false;
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
				_moveDirection = vkWJVTUNgFBXvHfhceNDsBmGzzqz.None;
				inMrpiaomncsPkPccIUrRHtknuzeA();
				_axis2D.Clear();
				XsKjeeFYVlWvjgMttYapqawCBLtL();
			}
		}

		internal void yeTrnhjJNnbBVkDiLhDfettbocEjb()
		{
			CQHOxfpTNKlIzHfPiNZDAomAlJaL();
			if (_hierarchyValueChangedHandlers == null)
			{
				_hierarchyValueChangedHandlers = new LTiqEoSaGSGvCpWQDOcWSGOWaNVX.HierarchyEventHelper<IValueChangedHandler, Vector2>(thZZiurtynYOddxFVMzVZoReLCrh);
			}
			_hierarchyValueChangedHandlers.GetHandlers(base.transform);
			if (_hierarchyStickPositionChangedHandlers == null)
			{
				_hierarchyStickPositionChangedHandlers = new LTiqEoSaGSGvCpWQDOcWSGOWaNVX.HierarchyEventHelper<IStickPositionChangedHandler, Vector2>(iZqJVoKdFbdKuaqMRbxIAhIVUJGS);
			}
			_hierarchyStickPositionChangedHandlers.GetHandlers(base.transform);
		}

		public override void ClearValue()
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				_axis2D.Clear();
				_lastTapFrame = -1;
				if (ghAsJwRhDmClYOgMqzKuSmomibZfA)
				{
					base.KIgQwfyVIoOiIXKpXkLoXgtMAONn.ClearElementValue(_horizontalAxisCustomControllerElement);
					base.KIgQwfyVIoOiIXKpXkLoXgtMAONn.ClearElementValue(_verticalAxisCustomControllerElement);
					base.KIgQwfyVIoOiIXKpXkLoXgtMAONn.ClearElementValue(_tapCustomControllerElement);
				}
			}
		}

		internal bool lrnCJibegGmYQikUzPgsAwQqKhbQ()
		{
			if (!base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				return false;
			}
			if (!IcqbeYEmGpfkqqAVukZKtDJbdtuLA())
			{
				return false;
			}
			return HYwbjXFdakovosWZxzzDtDnrmTHRA;
		}

		internal bool MNDfMPHYdStWbdOToJVnWpDHQNSY(GameObject P_0)
		{
			if (P_0 == null)
			{
				return false;
			}
			if (base.stScqZNiRHjjKdoCuFFXfYVzkfCN(P_0))
			{
				return true;
			}
			if (_workingTouchRegion != null)
			{
				return _workingTouchRegion.gameObject == P_0;
			}
			return false;
		}

		private void XsKjeeFYVlWvjgMttYapqawCBLtL()
		{
			_horizontalAxisCustomControllerElement.ClearElementCaches();
			_verticalAxisCustomControllerElement.ClearElementCaches();
			_tapCustomControllerElement.ClearElementCaches();
			sKyWRUCiXzBPjPbrSaadxxAtTfHV();
			jTdzsZFdcqgLHcTcbjxpUHscGgejA();
		}

		private void jTdzsZFdcqgLHcTcbjxpUHscGgejA()
		{
			if (_manageRaycasting)
			{
				_imageRaycastHelper.xdyFfgAnFtvUqduRTeXdrPcIqmrJA(base.transform, TPSJGnoxDDEsZuIsDehGedkEpMIBb());
			}
		}

		private bool TPSJGnoxDDEsZuIsDehGedkEpMIBb()
		{
			if (_workingTouchRegion != null && _useTouchRegionOnly)
			{
				return false;
			}
			return true;
		}

		private void xDSrhkoOmMpnxthsfjVvfnWxDeW(TouchRegion P_0)
		{
			if (!(P_0 == null))
			{
				YERKRlAicgcxlhlfQwTwAfITqJTjb(P_0);
				P_0.PointerDownEvent += zCmxggtaNZzcLqJMcaVkgsBMoNWj;
				P_0.PointerUpEvent += FCASIklwHUIlWNMTwqiNfkrncmsC;
				P_0.PointerEnterEvent += TEbNzVIGBMrRPrvlSljpgSDHQzlB;
				P_0.PointerExitEvent += SzCpDcCxcXQpBJnzkCxcWGeGhUJL;
				P_0.BeginDragEvent += ZkLedGGyljWZyucfsbrwRNxJBKzX;
				P_0.DragEvent += EHPxTtwEJBEXpNnjPgcvfuLQsENF;
				P_0.EndDragEvent += KefFhulFeWRNwXieviYQiTNyEioM;
			}
		}

		private void YERKRlAicgcxlhlfQwTwAfITqJTjb(TouchRegion P_0)
		{
			if (!(P_0 == null))
			{
				P_0.PointerDownEvent -= zCmxggtaNZzcLqJMcaVkgsBMoNWj;
				P_0.PointerUpEvent -= FCASIklwHUIlWNMTwqiNfkrncmsC;
				P_0.PointerEnterEvent -= TEbNzVIGBMrRPrvlSljpgSDHQzlB;
				P_0.PointerExitEvent -= SzCpDcCxcXQpBJnzkCxcWGeGhUJL;
				P_0.BeginDragEvent -= ZkLedGGyljWZyucfsbrwRNxJBKzX;
				P_0.DragEvent -= EHPxTtwEJBEXpNnjPgcvfuLQsENF;
				P_0.EndDragEvent -= KefFhulFeWRNwXieviYQiTNyEioM;
			}
		}

		private void sKyWRUCiXzBPjPbrSaadxxAtTfHV()
		{
			if (!(_workingTouchRegion == _touchRegion))
			{
				YERKRlAicgcxlhlfQwTwAfITqJTjb(_workingTouchRegion);
				_workingTouchRegion = _touchRegion;
				xDSrhkoOmMpnxthsfjVvfnWxDeW(_workingTouchRegion);
			}
		}

		private void bcEKpMNVfCrAZGSonicpfVWgNQvm(Vector2 P_0, bool P_1, float P_2, vkWJVTUNgFBXvHfhceNDsBmGzzqz P_3)
		{
			RectTransform rectTransform = base.transform.parent as RectTransform;
			Vector2 vector = ItUXShgHgqMTvNPoaDBaDFBZYvtd.GQvayxmeWFjZrrOOTmjgUzFrfKKK(base.pEKfbfSDEyiQffkZVSLqoSXLfhMc, rectTransform, P_0);
			Vector2 pivot = base.XgRjXqPEGoTEAcTuxzCYeFqkaaly.pivot;
			Vector2 sizeDelta = base.XgRjXqPEGoTEAcTuxzCYeFqkaaly.sizeDelta;
			Vector3 localScale = base.XgRjXqPEGoTEAcTuxzCYeFqkaaly.localScale;
			vector += new Vector2((pivot.x - 0.5f) * sizeDelta.x * localScale.x, (pivot.y - 0.5f) * sizeDelta.y * localScale.y);
			OURLDMVklodxslUMskgHvHflsMwQ(vector, PositionType.Local, P_1, P_2, P_3);
		}

		private void OURLDMVklodxslUMskgHvHflsMwQ(Vector2 P_0, PositionType P_1, bool P_2, float P_3, vkWJVTUNgFBXvHfhceNDsBmGzzqz P_4)
		{
			if (_isMoving && P_2 && _moveDirection == P_4)
			{
				return;
			}
			if (_isMoving && _coroutineMove != null)
			{
				inMrpiaomncsPkPccIUrRHtknuzeA();
				_isMoving = false;
				_moveDirection = vkWJVTUNgFBXvHfhceNDsBmGzzqz.None;
			}
			if (base.pEKfbfSDEyiQffkZVSLqoSXLfhMc == null)
			{
				Logger.LogWarning("Animation cannot be used without a Canvas.");
				P_2 = false;
			}
			else if (base.pEKfbfSDEyiQffkZVSLqoSXLfhMc.renderMode == RenderMode.WorldSpace)
			{
				Logger.LogWarning("Animation can only be used with a screen space Canvas.");
				P_2 = false;
			}
			if (P_2)
			{
				Transform parent = base.transform;
				RectTransform rectTransform = base.cceFGuGrUOPVoQFhXoaprVXRKreu;
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
				_coroutineMove = YipoyWaLdqoDIhaoULmyxBzPWwBk(P_0, P_1, P_3, P_4);
				StartCoroutine(_coroutineMove);
				_moveDirection = P_4;
				_isMovedFromDefaultPosition = true;
				wIFWxlDYChhaQxNtZiZxoQjtcHMB(P_4);
			}
			else
			{
				wIFWxlDYChhaQxNtZiZxoQjtcHMB(P_4);
				UpcPQfceWmNfTawSGtGhyGTseKKR(P_4, P_0, P_1);
			}
		}

		[IteratorStateMachine(typeof(frlhcQAZKIHVrvDRlpnfEfuftPWN))]
		private IEnumerator YipoyWaLdqoDIhaoULmyxBzPWwBk(Vector2 P_0, PositionType P_1, float P_2, vkWJVTUNgFBXvHfhceNDsBmGzzqz P_3)
		{
			return new frlhcQAZKIHVrvDRlpnfEfuftPWN(0)
			{
				RvAbxSMwlbfAqDdZKpbdyDMzaPzvA = this,
				gMivppHjaXgvuCfxJjpYqGmhTrbDA = P_0,
				QWKfVwcPtOYIZqHvSDlhqYhhsVFM = P_1,
				dvnGFQsywnDTuJEIcJkUZvwmnNpd = P_2,
				eSITFdRvjPHqWFHfoRwkIGjXGwpm = P_3
			};
		}

		private void UpcPQfceWmNfTawSGtGhyGTseKKR(vkWJVTUNgFBXvHfhceNDsBmGzzqz P_0, Vector2 P_1, PositionType P_2)
		{
			ItUXShgHgqMTvNPoaDBaDFBZYvtd.AVBaoIMoopyfpbSwkSCPNfEaGuXL(base.XgRjXqPEGoTEAcTuxzCYeFqkaaly, P_1, P_2);
			_isMoving = false;
			_moveDirection = vkWJVTUNgFBXvHfhceNDsBmGzzqz.None;
			switch (P_0)
			{
			case vkWJVTUNgFBXvHfhceNDsBmGzzqz.TowardHome:
				_isMovedFromDefaultPosition = false;
				break;
			case vkWJVTUNgFBXvHfhceNDsBmGzzqz.TowardTouch:
				_isMovedFromDefaultPosition = true;
				break;
			}
			inMrpiaomncsPkPccIUrRHtknuzeA();
			rDeAduOaLRCywcjmJoYrBJTBUrJUB(P_0);
		}

		private void gDSToxnBFAVdUKALkeFYsOIUhkkb(vkWJVTUNgFBXvHfhceNDsBmGzzqz P_0)
		{
			if (_manageRaycasting)
			{
				bool flag = false;
				bool flag2 = false;
				if (((_followTouchPosition && stayActiveOnSwipeOut) || (!_followTouchPosition && _workingTouchRegion != null && !_useTouchRegionOnly && _moveToTouchPosition)) && _returnOnRelease && P_0 == vkWJVTUNgFBXvHfhceNDsBmGzzqz.TowardTouch)
				{
					flag = true;
					flag2 = false;
				}
				if (flag)
				{
					_imageRaycastHelper.xdyFfgAnFtvUqduRTeXdrPcIqmrJA(base.transform, flag2);
				}
			}
		}

		private void ixWJtCnVLOccJDbxbHnCwVpSmgxx(vkWJVTUNgFBXvHfhceNDsBmGzzqz P_0)
		{
			if (_manageRaycasting)
			{
				bool flag = false;
				bool flag2 = false;
				if (((_followTouchPosition && stayActiveOnSwipeOut) || (!_followTouchPosition && _workingTouchRegion != null && !_useTouchRegionOnly && _moveToTouchPosition)) && _returnOnRelease && P_0 == vkWJVTUNgFBXvHfhceNDsBmGzzqz.TowardHome)
				{
					flag = true;
					flag2 = TPSJGnoxDDEsZuIsDehGedkEpMIBb();
				}
				if (flag)
				{
					_imageRaycastHelper.xdyFfgAnFtvUqduRTeXdrPcIqmrJA(base.transform, flag2);
				}
			}
		}

		private void inMrpiaomncsPkPccIUrRHtknuzeA()
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

		private void gMWvHVQKqaoNegIwSWfIiTMboSzo(int P_0, Vector2 P_1, PositionType P_2)
		{
			if (TouchInteractable.MiEJmWCYYHJMBAtKMSSlbJKQgXxfA(P_0))
			{
				OURLDMVklodxslUMskgHvHflsMwQ((Vector2)ItUXShgHgqMTvNPoaDBaDFBZYvtd.BrgGlubhkUYkSMDAYAtTVxcWogbn(base.XgRjXqPEGoTEAcTuxzCYeFqkaaly, P_2) + P_1, P_2, false, 0f, vkWJVTUNgFBXvHfhceNDsBmGzzqz.TowardTouch);
				if (_lastClaimSource == eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion)
				{
					_lastPressAnchoredPosition += P_1;
				}
			}
		}

		private void FbFoaRKPMoRkGpOjmsqjOiaFbcSJ()
		{
			if (!hasPointer)
			{
				return;
			}
			if (!TouchInteractable.MiEJmWCYYHJMBAtKMSSlbJKQgXxfA(zIAcsLBAnaznJRdMrArlAGtkGoBBb))
			{
				PointerEventData pointerEventData = ZulZrnMDjkBvuuPokdYgecqYGNmF(zIAcsLBAnaznJRdMrArlAGtkGoBBb);
				if (pointerEventData != null && pointerEventData.pointerPress != null)
				{
					lcsxgZfMISZoLFlpKFwxHNOvKNotA(pointerEventData);
				}
				else
				{
					cghSeqdsrbwAODCfAHRFWdvnxULj();
				}
			}
			else if (_pointerDownIsFake)
			{
				PointerEventData pointerEventData2 = KkcAGUlmBiNPYFkBRSheCOuKywvJ(zIAcsLBAnaznJRdMrArlAGtkGoBBb, (_workingTouchRegion != null && _useTouchRegionOnly) ? _workingTouchRegion.gameObject : ((_stickTransform != null) ? _stickTransform.gameObject : base.gameObject));
				if (pointerEventData2 != null)
				{
					hzsCUMbARxjDwgXyYMlKndhMRDssA(pointerEventData2, _lastClaimSource);
				}
			}
		}

		private void YzCcFkQygeyMWbEeFlzuOltDPqpJ()
		{
			if (hasPointer)
			{
				Vector2 vector = TouchInteractable.DTRRiAsrHICJIvfdMohYjrgotXjG(zIAcsLBAnaznJRdMrArlAGtkGoBBb);
				qbLqrbOuDPJsEjWFDnSVgeXaoEeB(ref vector);
			}
		}

		private void qbLqrbOuDPJsEjWFDnSVgeXaoEeB(ref Vector2 P_0)
		{
			if (_allowTap && _isEligibleForTap && ((_tapTimeout > 0f && Time.realtimeSinceStartup - _touchStartTime > _tapTimeout) || (_tapDistanceLimit >= 0 && Vector2.Distance(_touchStartPosition, P_0) > (float)_tapDistanceLimit)))
			{
				_isEligibleForTap = false;
			}
		}

		private bool OErggtDFItJysteeKDSNMsHGCqKWA()
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

		private void akHuDEZHdnqvcaDBDjBtCzUJjMgFA()
		{
			_pointerId = -2147483648;
			_realMousePointerId = -2147483648;
			_lastClaimSource = eistIGDtvClGGRARBOzaNeJsvHzB.Local;
		}

		private bool jfSEELFQQYDiLauaJNwrpWAFrlhm(int P_0)
		{
			if (P_0 == -2147483648)
			{
				return false;
			}
			if (_pointerId == -2147483648)
			{
				return false;
			}
			if (_pointerId == P_0)
			{
				return true;
			}
			if (TouchInteractable.mBmltzbovhkqiRskAgVuCNaKKMGn(P_0) && _realMousePointerId != -2147483648 && P_0 == _realMousePointerId)
			{
				return true;
			}
			return false;
		}

		private PointerEventData TiVTvtTamlKQNJnWDWSUTBmirkbc(int P_0, GameObject P_1)
		{
			PointerEventData pointerEventData = ZulZrnMDjkBvuuPokdYgecqYGNmF(P_0);
			if (pointerEventData == null)
			{
				return null;
			}
			pointerEventData.position = TouchInteractable.DTRRiAsrHICJIvfdMohYjrgotXjG(P_0);
			if (TouchInteractable.FFlgOOKqXIvFbSHHgALJmjRufgIc(P_0))
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
				if (!TouchInteractable.mBmltzbovhkqiRskAgVuCNaKKMGn(P_0))
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

		private PointerEventData KkcAGUlmBiNPYFkBRSheCOuKywvJ(int P_0, GameObject P_1)
		{
			PointerEventData pointerEventData = ZulZrnMDjkBvuuPokdYgecqYGNmF(P_0);
			if (pointerEventData == null)
			{
				return null;
			}
			Vector2 vector = TouchInteractable.DTRRiAsrHICJIvfdMohYjrgotXjG(P_0);
			pointerEventData.delta = vector - pointerEventData.position;
			pointerEventData.position = vector;
			pointerEventData.dragging = true;
			pointerEventData.pointerDrag = P_1;
			pointerEventData.useDragThreshold = true;
			pointerEventData.pointerPress = null;
			pointerEventData.rawPointerPress = null;
			return pointerEventData;
		}

		private PointerEventData LFYTcHMQLYIzkRAODEzSVQMSgTxbA(int P_0)
		{
			PointerEventData pointerEventData = ZulZrnMDjkBvuuPokdYgecqYGNmF(P_0);
			if (pointerEventData == null)
			{
				return null;
			}
			if (TouchInteractable.FFlgOOKqXIvFbSHHgALJmjRufgIc(P_0))
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
				if (!TouchInteractable.mBmltzbovhkqiRskAgVuCNaKKMGn(P_0))
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

		private void lcsxgZfMISZoLFlpKFwxHNOvKNotA(PointerEventData P_0)
		{
			if (P_0 != null)
			{
				OnPointerUp(P_0);
				LFYTcHMQLYIzkRAODEzSVQMSgTxbA(zIAcsLBAnaznJRdMrArlAGtkGoBBb);
			}
		}

		private void hzsCUMbARxjDwgXyYMlKndhMRDssA(PointerEventData P_0, eistIGDtvClGGRARBOzaNeJsvHzB P_1)
		{
			if (P_0 != null)
			{
				switch (P_1)
				{
				case eistIGDtvClGGRARBOzaNeJsvHzB.Local:
					OnDrag(P_0);
					break;
				case eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion:
					EHPxTtwEJBEXpNnjPgcvfuLQsENF(P_0);
					break;
				default:
					throw new NotImplementedException();
				}
				LFYTcHMQLYIzkRAODEzSVQMSgTxbA(zIAcsLBAnaznJRdMrArlAGtkGoBBb);
			}
		}

		private PointerEventData ZulZrnMDjkBvuuPokdYgecqYGNmF(int P_0)
		{
			if (P_0 == -2147483648)
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
				if (TouchInteractable.mBmltzbovhkqiRskAgVuCNaKKMGn(P_0))
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

		private void BXMwTYFUsamJZToDFfUcgQDBPnmP()
		{
			vMFIiAaaGNmLxVqtWqeZEfxTeFhp(_axesToUse);
			if (ghAsJwRhDmClYOgMqzKuSmomibZfA && base.xFyfQoQtVcUhUlSjIKbQOAXWbfkp.useCustomController)
			{
				if (_useXAxis)
				{
					base.KIgQwfyVIoOiIXKpXkLoXgtMAONn.ValidateElements(_horizontalAxisCustomControllerElement);
				}
				if (_useYAxis)
				{
					base.KIgQwfyVIoOiIXKpXkLoXgtMAONn.ValidateElements(_verticalAxisCustomControllerElement);
				}
				if (_allowTap)
				{
					base.KIgQwfyVIoOiIXKpXkLoXgtMAONn.ValidateElements(_tapCustomControllerElement);
				}
			}
		}

		private void vMFIiAaaGNmLxVqtWqeZEfxTeFhp(AxisDirection P_0)
		{
			bool flag = P_0 == AxisDirection.Both || P_0 == AxisDirection.Horizontal;
			if (_useXAxis != flag)
			{
				_useXAxis = flag;
				if (!flag && ghAsJwRhDmClYOgMqzKuSmomibZfA)
				{
					int targetCount = _horizontalAxisCustomControllerElement.targetCount;
					for (int i = 0; i < targetCount; i++)
					{
						base.KIgQwfyVIoOiIXKpXkLoXgtMAONn.ClearElementValue(_horizontalAxisCustomControllerElement[i]);
					}
				}
			}
			bool flag2 = P_0 == AxisDirection.Both || P_0 == AxisDirection.Vertical;
			if (_useYAxis != flag2)
			{
				_useYAxis = flag2;
				if (!flag2 && ghAsJwRhDmClYOgMqzKuSmomibZfA)
				{
					int targetCount2 = _verticalAxisCustomControllerElement.targetCount;
					for (int j = 0; j < targetCount2; j++)
					{
						base.KIgQwfyVIoOiIXKpXkLoXgtMAONn.ClearElementValue(_verticalAxisCustomControllerElement[j]);
					}
				}
			}
			_axesToUse = P_0;
		}

		private void cPLkIFYAyNmopRVPIrrNGXtSLIBQ(PointerEventData P_0, eistIGDtvClGGRARBOzaNeJsvHzB P_1)
		{
			if (!hasPointer || jfSEELFQQYDiLauaJNwrpWAFrlhm(P_0.pointerId))
			{
				if (IcqbeYEmGpfkqqAVukZKtDJbdtuLA() && IsInteractable())
				{
					WdjbygtatbCnojvucinTEPpHHFKQ(P_0.pointerId, P_0.pressPosition, P_1);
				}
				base.OnPointerDown(P_0);
			}
		}

		private void VjmpwudlVhHpnHqgTDLIgXziUcYB(PointerEventData P_0, eistIGDtvClGGRARBOzaNeJsvHzB P_1)
		{
			if ((!hasPointer || jfSEELFQQYDiLauaJNwrpWAFrlhm(P_0.pointerId)) && !TouchInteractable.MiEJmWCYYHJMBAtKMSSlbJKQgXxfA(zIAcsLBAnaznJRdMrArlAGtkGoBBb))
			{
				cghSeqdsrbwAODCfAHRFWdvnxULj();
				base.OnPointerUp(P_0);
			}
		}

		private void ybvuvzHenFQElMrNUOWNiHuctpVW(PointerEventData P_0, eistIGDtvClGGRARBOzaNeJsvHzB P_1)
		{
			if (hasPointer && !jfSEELFQQYDiLauaJNwrpWAFrlhm(P_0.pointerId))
			{
				return;
			}
			bool flag = TouchInteractable.mBmltzbovhkqiRskAgVuCNaKKMGn(P_0.pointerId);
			bool flag2 = false;
			MouseButtonFlags mouseButtonFlags = P_1 switch
			{
				eistIGDtvClGGRARBOzaNeJsvHzB.Local => base.allowedMouseButtons, 
				eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion => _touchRegion.allowedMouseButtons, 
				_ => throw new NotImplementedException(), 
			};
			if (_activateOnSwipeIn && IcqbeYEmGpfkqqAVukZKtDJbdtuLA() && IsInteractable() && (!flag || TouchInteractable.aEOEYhFHczvhfJeopcXMsyPJOZaFA(mouseButtonFlags)) && !HYwbjXFdakovosWZxzzDtDnrmTHRA)
			{
				if (flag)
				{
					if (TouchInteractable.hAWfiRgEUZhSRENpfwbhcWZRcXkUA(mouseButtonFlags, out var realMousePointerId))
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
					eistIGDtvClGGRARBOzaNeJsvHzB.Local => base.gameObject, 
					eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion => _workingTouchRegion.gameObject, 
					_ => throw new NotImplementedException(), 
				};
				PointerEventData pointerEventData = TiVTvtTamlKQNJnWDWSUTBmirkbc((_realMousePointerId != -2147483648) ? _realMousePointerId : P_0.pointerId, gameObject);
				if (pointerEventData != null)
				{
					cPLkIFYAyNmopRVPIrrNGXtSLIBQ(pointerEventData, P_1);
					if (HYwbjXFdakovosWZxzzDtDnrmTHRA)
					{
						_pointerDownIsFake = true;
					}
				}
			}
			zsERuCTpLVOqaWcMsqUUAexgtUFm = true;
		}

		private void NWiUOZuaZKlWjTCAivNOUuDQmskT(PointerEventData P_0, eistIGDtvClGGRARBOzaNeJsvHzB P_1)
		{
			if (hasPointer && !jfSEELFQQYDiLauaJNwrpWAFrlhm(P_0.pointerId))
			{
				base.OnPointerExit(P_0);
				return;
			}
			if (!stayActiveOnSwipeOut && HYwbjXFdakovosWZxzzDtDnrmTHRA)
			{
				cghSeqdsrbwAODCfAHRFWdvnxULj();
			}
			base.OnPointerExit(P_0);
			zsERuCTpLVOqaWcMsqUUAexgtUFm = false;
		}

		private void KCYJLBFZsVQrJDvWIChYCCZFqgnn(PointerEventData P_0, eistIGDtvClGGRARBOzaNeJsvHzB P_1)
		{
			if (hasPointer && jfSEELFQQYDiLauaJNwrpWAFrlhm(P_0.pointerId))
			{
				base.OnBeginDrag(P_0);
			}
		}

		private void cWBBbvOMqdPYhveDkaxwWEFAdeLU(PointerEventData P_0, eistIGDtvClGGRARBOzaNeJsvHzB P_1)
		{
			if (!hasPointer || !jfSEELFQQYDiLauaJNwrpWAFrlhm(P_0.pointerId))
			{
				return;
			}
			RectTransform rectTransform = iPXbsIzAbZdRxRwfZPEDWniusguD;
			Vector2 vector = ((!_snapStickToTouch) ? _lastPressAnchoredPosition : ItUXShgHgqMTvNPoaDBaDFBZYvtd.JMbhUddOaTRcHZYrWEmAjLWhhzks(base.XgRjXqPEGoTEAcTuxzCYeFqkaaly, rectTransform, base.XgRjXqPEGoTEAcTuxzCYeFqkaaly.rect.center));
			if (!_centerStickOnRelease && !_snapStickToTouch)
			{
				vector -= _lastPressStartingValue * QJMwKlJQhKFibmpZSjwWYsWHVrmB;
			}
			Vector2 vector2 = ItUXShgHgqMTvNPoaDBaDFBZYvtd.vMdruAypbaTqFSjHCymKLazDYJzQ(base.pEKfbfSDEyiQffkZVSLqoSXLfhMc, rectTransform, P_0.position);
			Vector2 vector3 = new Vector2(_useXAxis ? (vector2.x - vector.x) : 0f, _useYAxis ? (vector2.y - vector.y) : 0f);
			Vector2 vector4;
			if (_stickBounds == StickBounds.Circle)
			{
				vector4 = Vector2.ClampMagnitude(vector3, QJMwKlJQhKFibmpZSjwWYsWHVrmB);
			}
			else
			{
				if (_stickBounds != StickBounds.Square)
				{
					throw new NotImplementedException();
				}
				vector4 = MathTools.Clamp(vector3, 0f - QJMwKlJQhKFibmpZSjwWYsWHVrmB, QJMwKlJQhKFibmpZSjwWYsWHVrmB);
			}
			Vector2 rawValue = vector4 / QJMwKlJQhKFibmpZSjwWYsWHVrmB;
			SetRawValue(rawValue);
			if (_followTouchPosition)
			{
				if (_stickBounds == StickBounds.Circle)
				{
					if (vector3.sqrMagnitude > QJMwKlJQhKFibmpZSjwWYsWHVrmB)
					{
						Vector2 vector5 = new Vector2(_useXAxis ? (vector3.x - vector4.x) : 0f, _useXAxis ? (vector3.y - vector4.y) : 0f);
						gMWvHVQKqaoNegIwSWfIiTMboSzo(zIAcsLBAnaznJRdMrArlAGtkGoBBb, vector5, PositionType.Anchored);
					}
				}
				else
				{
					if (_stickBounds != StickBounds.Square)
					{
						throw new NotImplementedException();
					}
					bool flag = Mathf.Abs(vector3.x) > QJMwKlJQhKFibmpZSjwWYsWHVrmB;
					bool flag2 = Mathf.Abs(vector3.y) > QJMwKlJQhKFibmpZSjwWYsWHVrmB;
					if (flag || flag2)
					{
						Vector2 vector6 = new Vector2((_useXAxis && flag) ? (vector3.x - vector4.x) : 0f, (_useXAxis && flag2) ? (vector3.y - vector4.y) : 0f);
						gMWvHVQKqaoNegIwSWfIiTMboSzo(zIAcsLBAnaznJRdMrArlAGtkGoBBb, vector6, PositionType.Anchored);
					}
				}
			}
			base.OnDrag(P_0);
		}

		private void eezNeuQPqAMQaCuGsRkuXlSyuYdS(PointerEventData P_0, eistIGDtvClGGRARBOzaNeJsvHzB P_1)
		{
			if (hasPointer && jfSEELFQQYDiLauaJNwrpWAFrlhm(P_0.pointerId))
			{
				base.OnEndDrag(P_0);
			}
		}

		private void WdjbygtatbCnojvucinTEPpHHFKQ(int P_0, Vector2 P_1, eistIGDtvClGGRARBOzaNeJsvHzB P_2)
		{
			_pointerId = P_0;
			_lastClaimSource = P_2;
			_isEligibleForTap = true;
			_lastPressAnchoredPosition = ItUXShgHgqMTvNPoaDBaDFBZYvtd.vMdruAypbaTqFSjHCymKLazDYJzQ(base.pEKfbfSDEyiQffkZVSLqoSXLfhMc, iPXbsIzAbZdRxRwfZPEDWniusguD, P_1);
			HYwbjXFdakovosWZxzzDtDnrmTHRA = true;
			_lastPressStartingValue.x = MathTools.Clamp(_axis2D.value.x, -1f, 1f);
			_lastPressStartingValue.y = MathTools.Clamp(_axis2D.value.y, -1f, 1f);
			_touchStartTime = Time.realtimeSinceStartup;
			_touchStartPosition = P_1;
			if (P_2 == eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion && (_moveToTouchPosition || _followTouchPosition))
			{
				if (_followTouchPosition)
				{
					bcEKpMNVfCrAZGSonicpfVWgNQvm(P_1, false, 0f, vkWJVTUNgFBXvHfhceNDsBmGzzqz.TowardTouch);
				}
				else
				{
					bcEKpMNVfCrAZGSonicpfVWgNQvm(P_1, _animateOnMoveToTouch, _moveToTouchSpeed, vkWJVTUNgFBXvHfhceNDsBmGzzqz.TowardTouch);
				}
			}
			if (_onTouchStarted != null)
			{
				_onTouchStarted.Invoke();
			}
			PointerEventData pointerEventData = KkcAGUlmBiNPYFkBRSheCOuKywvJ(_pointerId, (P_2 == eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion) ? _workingTouchRegion.gameObject : ((_stickTransform != null) ? _stickTransform.gameObject : base.gameObject));
			if (pointerEventData != null)
			{
				hzsCUMbARxjDwgXyYMlKndhMRDssA(pointerEventData, P_2);
			}
		}

		private void cghSeqdsrbwAODCfAHRFWdvnxULj()
		{
			akHuDEZHdnqvcaDBDjBtCzUJjMgFA();
			bool num = _allowTap && _isEligibleForTap;
			HYwbjXFdakovosWZxzzDtDnrmTHRA = false;
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

		internal void npDsGrHMcICVdMwmUOGhXhYAKgam(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerUp) && (!(_workingTouchRegion != null) || !_useTouchRegionOnly))
			{
				VjmpwudlVhHpnHqgTDLIgXziUcYB(P_0, eistIGDtvClGGRARBOzaNeJsvHzB.Local);
			}
		}

		internal void WkdsEMflKPGRCQzmbHEDGNWnNoYe(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerDown) && (!(_workingTouchRegion != null) || !_useTouchRegionOnly))
			{
				cPLkIFYAyNmopRVPIrrNGXtSLIBQ(P_0, eistIGDtvClGGRARBOzaNeJsvHzB.Local);
			}
		}

		internal void mXPeXASTdHfEqaAyoZERuYItPzpl(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerEnter) && (!(_workingTouchRegion != null) || !_useTouchRegionOnly))
			{
				ybvuvzHenFQElMrNUOWNiHuctpVW(P_0, eistIGDtvClGGRARBOzaNeJsvHzB.Local);
			}
		}

		internal void CUdDWsoljGXArUIYAqqFrIQFaKGdA(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.PointerExit) && (!(_workingTouchRegion != null) || !_useTouchRegionOnly))
			{
				NWiUOZuaZKlWjTCAivNOUuDQmskT(P_0, eistIGDtvClGGRARBOzaNeJsvHzB.Local);
			}
		}

		internal void OUimofMTkmQLNKIrsOQPszqRFcRP(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.BeginDrag) && (!(_workingTouchRegion != null) || !_useTouchRegionOnly))
			{
				KCYJLBFZsVQrJDvWIChYCCZFqgnn(P_0, eistIGDtvClGGRARBOzaNeJsvHzB.Local);
			}
		}

		internal void SFwaXqZAxyIbSDKJViZnhLlJeYuHA(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.Drag) && (!(_workingTouchRegion != null) || !_useTouchRegionOnly))
			{
				cWBBbvOMqdPYhveDkaxwWEFAdeLU(P_0, eistIGDtvClGGRARBOzaNeJsvHzB.Local);
			}
		}

		internal void EEchsuItEEYWkHkIQLfigxdhophsb(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, base.allowedMouseButtons, EventTriggerType.EndDrag) && (!(_workingTouchRegion != null) || !_useTouchRegionOnly))
			{
				eezNeuQPqAMQaCuGsRkuXlSyuYdS(P_0, eistIGDtvClGGRARBOzaNeJsvHzB.Local);
			}
		}

		private void zCmxggtaNZzcLqJMcaVkgsBMoNWj(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerDown))
			{
				cPLkIFYAyNmopRVPIrrNGXtSLIBQ(P_0, eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion);
			}
		}

		private void FCASIklwHUIlWNMTwqiNfkrncmsC(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerUp))
			{
				VjmpwudlVhHpnHqgTDLIgXziUcYB(P_0, eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion);
			}
		}

		private void TEbNzVIGBMrRPrvlSljpgSDHQzlB(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerEnter))
			{
				ybvuvzHenFQElMrNUOWNiHuctpVW(P_0, eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion);
			}
		}

		private void SzCpDcCxcXQpBJnzkCxcWGeGhUJL(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerExit))
			{
				NWiUOZuaZKlWjTCAivNOUuDQmskT(P_0, eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion);
			}
		}

		private void ZkLedGGyljWZyucfsbrwRNxJBKzX(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.BeginDrag))
			{
				KCYJLBFZsVQrJDvWIChYCCZFqgnn(P_0, eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion);
			}
		}

		private void EHPxTtwEJBEXpNnjPgcvfuLQsENF(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.Drag))
			{
				cWBBbvOMqdPYhveDkaxwWEFAdeLU(P_0, eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion);
			}
		}

		private void KefFhulFeWRNwXieviYQiTNyEioM(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.EndDrag))
			{
				eezNeuQPqAMQaCuGsRkuXlSyuYdS(P_0, eistIGDtvClGGRARBOzaNeJsvHzB.TouchRegion);
			}
		}

		private void mbyZkjVLnQGryBgZWdlwsnjFqvlJ(Vector2 P_0)
		{
			hURaZktijlmPxmBSjDIeeNMcvCPLA(null);
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
				RectTransform rectTransform = iPXbsIzAbZdRxRwfZPEDWniusguD;
				Vector3 position = value * QJMwKlJQhKFibmpZSjwWYsWHVrmB;
				position += rectTransform.InverseTransformPoint(base.transform.position);
				Vector3 position2 = rectTransform.TransformPoint(position);
				Vector3 vector = _stickTransform.parent.InverseTransformPoint(position2);
				Vector2 anchoredPosition = ItUXShgHgqMTvNPoaDBaDFBZYvtd.KkBcQvEoGarwuGfaiiqZqJhESwiI(_stickTransform.parent as RectTransform, vector);
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
