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
	[DisallowMultipleComponent]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[AddComponentMenu("Rewired/Touch Button")]
	public sealed class TouchButton : TouchInteractable
	{
		public enum ButtonType
		{
			Standard = 0,
			ToggleSwitch = 1
		}

		private enum OpkLqbttkDfdWPvVmlLCialkmZyE
		{
			None = 0,
			TowardTouch = 1,
			TowardHome = 2
		}

		private enum KNuxcpaopMTVUOnDxSWfwXInKYgs
		{
			Local = 0,
			TouchRegion = 1
		}

		[Serializable]
		public class AxisValueChangedEventHandler : UnityEvent<float>
		{
		}

		[Serializable]
		public class ButtonValueChangedEventHandler : UnityEvent<bool>
		{
		}

		[Serializable]
		public class ButtonDownEventHandler : UnityEvent
		{
		}

		[Serializable]
		public class ButtonUpEventHandler : UnityEvent
		{
		}

		private sealed class KykhxUELCIErMkPZtnTHwKXXpCVb : IDisposable, IEnumerator, IEnumerator<object>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private object VqEePGSMyrGKIqkWibsjeHcWPSIx;

			public float QzkoAujiXELSEATTFbUXkPALueUS;

			public TouchButton TtytLoUfsgUyhsklaKccrnoMiiek;

			public PositionType NrdyBFMMWwnWFfEyWKBnxrSnZvnA;

			public Vector2 JTKktbfSAqGDQjeacpqTtOeybKpt;

			public OpkLqbttkDfdWPvVmlLCialkmZyE sGqbUlnFwRgsWfMjUESVfFgHGuQnb;

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
			public KykhxUELCIErMkPZtnTHwKXXpCVb(int P_0)
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
				TouchButton ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
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
						ttytLoUfsgUyhsklaKccrnoMiiek.zAGNVhMcIKxGEzzgSRljWYiyquPd = true;
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

		private const float SCWAmlAYUdOqKHXMQDaAhhwDAjLJb = 20f;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("The Custom Controller element that will receive input values from this control.")]
		private CustomControllerElementTargetSetForFloat _targetCustomControllerElement = new CustomControllerElementTargetSetForFloat(new CustomControllerElementTarget(new CustomControllerElementSelector
		{
			elementType = CustomControllerElementSelector.ElementType.Button
		}));

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("The type of button.\nStandard: A momentary switch. Returns True while the button is pressed down.\nToggle Switch: Alternately turns on and off with each press.")]
		private ButtonType _buttonType;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("If true, the button can be turned on by a touch swipe that began in an area outside the button region. If false, the button can only be turned on by a direct press.")]
		private bool _activateOnSwipeIn;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("If true, the button will stay on even if the touch that activated it moves outside the button region. If false, the button will turn off once the touch that activated it moves outside the button region.")]
		private bool _stayActiveOnSwipeOut = true;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("Makes the axis value gradually change over time based on gravity and sensitivity as the button is pressed.")]
		private bool _useDigitalAxisSimulation;

		[CustomObfuscation(rename = false)]
		[FieldRange(0f, float.PositiveInfinity)]
		[SerializeField]
		[Tooltip("Speed (units/sec) that the axis value falls toward 0 when not pressed. A value of 1.0 means an axis value of 1 will drain to 0 over 1 second. A value of 3 equates to 1/3 of a second, and so on.")]
		private float _digitalAxisGravity = 3f;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("Speed to move toward an axis value of 1.0 in units/sec when pressed. A value of 1.0 means an axis value of 0 will reach 1 over 1 second. A value of 3 equates to 1/3 of a second, and so on.")]
		[FieldRange(0f, float.PositiveInfinity)]
		private float _digitalAxisSensitivity = 3f;

		[Tooltip("The internal axis of the button. The axis is used for all value calculations.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private StandaloneAxis _axis = new StandaloneAxis();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("Optional external region to use for hover/click/touch detection. If set, this region will be used for touch detection instead of or in addition to the button's RectTransform. This can be useful if you want a larger area of the screen to act as a button.")]
		private TouchRegion _touchRegion;

		[CustomObfuscation(rename = false)]
		[Tooltip("If True, hovers/clicks/touches on the local button will be ignored and only Touch Region touches will be used. Otherwise, both touches on the button and on the Touch Region will be used. This also applies to mouse hover. This setting has no effect if no Touch Region is set.")]
		[SerializeField]
		private bool _useTouchRegionOnly = true;

		[CustomObfuscation(rename = false)]
		[Tooltip("If True, the button will move to the location of the current touch in the Touch Region. This can be used to designate an area of the screen as a hot-spot for a button and have the button graphics follow the users touches. This only has an effect if a Touch Region is set.")]
		[SerializeField]
		private bool _moveToTouchPosition;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("If Move To Touch Position is enabled, this will make the button return to its original position after the press is released. This only has an effect if a Touch Region is set.")]
		private bool _returnOnRelease = true;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("If True, the button will follow the touch around until released. This setting overrides Move To Touch Position.")]
		private bool _followTouchPosition;

		[SerializeField]
		[Tooltip("Should the button animate when moving to the touch point? This only has an effect if Move To Touch Position is True and a Touch Region is set. This setting is ignored if Follow Touch Position is True.")]
		[CustomObfuscation(rename = false)]
		private bool _animateOnMoveToTouch = true;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Range(0f, 20f)]
		[Tooltip("The speed at which the button will move toward the touch position measured in screens per second (based on the larger of width and height). [1.0 = Move 1 screen/sec]. This only has an effect if Move To Touch Position is True, Animate On Move To Touch is true, and a Touch Region is set. This setting is ignored if Follow Touch Position is True.")]
		private float _moveToTouchSpeed = 2f;

		[SerializeField]
		[Tooltip("Should the button animate when moving back to its original position? This only has an effect if Follow Touch Position is True, or if Move To Touch Position is True and a Touch Region is set, and Return on Release is True.")]
		[CustomObfuscation(rename = false)]
		private bool _animateOnReturn = true;

		[Tooltip("The speed at which the button will move back toward its original position measured in screens per second (based on the larger of width and height). [1.0 = Move 1 screen/sec]. This only has an effect if Follow Touch Position is True, or if Move To Touch Position is True and a Touch Region is set, and Return on Release and Animate on Return are both True.")]
		[Range(0f, 20f)]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private float _returnSpeed = 2f;

		[Tooltip("If True, it will attempt to automatically manage Graphic component raycasting for best results based on your current settings.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private bool _manageRaycasting = true;

		private float GayFivmQsrHyXTnOsAoGyZrYONky;

		private float ZxGKbhZcbUmwqArGhtrVMLaWEajZ;

		private TouchRegion DdEciANgMGwGEPvgxnWPHBeJPbOd;

		private Vector2 lhmAxKFWhlSHZJoCEAJNVcCScTPH;

		private bool zAGNVhMcIKxGEzzgSRljWYiyquPd;

		private bool sTmRLabKJLmxGbwWDHHbRrMNNrqn;

		private OpkLqbttkDfdWPvVmlLCialkmZyE yJgkQZuMZrJUEaNCzYYZYngtVmoe;

		private int aXEzyiFgJCCEqWfKXxtFBJTVbEHIA = int.MinValue;

		private int RxCFBSfZGSaxvGjcDdHvHngyjaKGB = int.MinValue;

		[NonSerialized]
		private bool ALBJgfsRYJdMVJwePTiUwIfQxDNM;

		[NonSerialized]
		private bool zmNsczlnaJKPNGfDEeyyFdsXhpjgb;

		private IEnumerator FcwfREdgzibGQMOLZwevNMKYBahxA;

		private orKfWSGFqzCFUysymbkGocsATicLA RnmnTCNSMxWpWOEFwiXbynsDrRsy = new orKfWSGFqzCFUysymbkGocsATicLA();

		private Action<OpkLqbttkDfdWPvVmlLCialkmZyE> hQWAhuEEpQgIIlrOEloKQoXoiIpCb;

		private Action<OpkLqbttkDfdWPvVmlLCialkmZyE> MnHeXlNYnTFAvDUkeuUfMXqRumEH;

		[CustomObfuscation(rename = false)]
		[Tooltip("Event sent when the axis value changes.")]
		[SerializeField]
		private AxisValueChangedEventHandler _onAxisValueChanged = new AxisValueChangedEventHandler();

		[Tooltip("Event sent when the button value changes.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ButtonValueChangedEventHandler _onButtonValueChanged = new ButtonValueChangedEventHandler();

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("Event sent when the button is pressed.")]
		private ButtonDownEventHandler _onButtonDown = new ButtonDownEventHandler();

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Event sent when the button is released.")]
		private ButtonUpEventHandler _onButtonUp = new ButtonUpEventHandler();

		private Dictionary<int, PointerEventData> EeRYMczwTzlgDjdFdgjMeYiqqewh;

		public CustomControllerElementTargetSetForFloat targetCustomControllerElement => _targetCustomControllerElement;

		public ButtonType buttonType
		{
			get
			{
				return _buttonType;
			}
			set
			{
				if (_buttonType != value)
				{
					_buttonType = value;
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

		public bool useDigitalAxisSimulation
		{
			get
			{
				return _useDigitalAxisSimulation;
			}
			set
			{
				if (_useDigitalAxisSimulation != value)
				{
					_useDigitalAxisSimulation = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public float digitalAxisGravity
		{
			get
			{
				return _digitalAxisGravity;
			}
			set
			{
				if (_digitalAxisGravity != value)
				{
					_digitalAxisGravity = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public float digitalAxisSensitivity
		{
			get
			{
				return _digitalAxisSensitivity;
			}
			set
			{
				if (_digitalAxisSensitivity != value)
				{
					_digitalAxisSensitivity = value;
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
						RnmnTCNSMxWpWOEFwiXbynsDrRsy.SPGTRPyvIslcMdbPTItsewSLRPxx();
					}
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public int pointerId
		{
			get
			{
				return aXEzyiFgJCCEqWfKXxtFBJTVbEHIA;
			}
			set
			{
				aXEzyiFgJCCEqWfKXxtFBJTVbEHIA = value;
			}
		}

		public bool hasPointer => aXEzyiFgJCCEqWfKXxtFBJTVbEHIA != int.MinValue;

		internal StandaloneAxis axis => _axis;

		private Action<OpkLqbttkDfdWPvVmlLCialkmZyE> moveStartedDelegate
		{
			get
			{
				if (hQWAhuEEpQgIIlrOEloKQoXoiIpCb == null)
				{
					return hQWAhuEEpQgIIlrOEloKQoXoiIpCb = XNKImEVsUNPavzxgPJMfIdoSEAWgA;
				}
				return hQWAhuEEpQgIIlrOEloKQoXoiIpCb;
			}
		}

		private Action<OpkLqbttkDfdWPvVmlLCialkmZyE> moveEndedDelegate
		{
			get
			{
				if (MnHeXlNYnTFAvDUkeuUfMXqRumEH == null)
				{
					return MnHeXlNYnTFAvDUkeuUfMXqRumEH = UkrWacxKoOMuRkiKLRmEmILrFhRJ;
				}
				return MnHeXlNYnTFAvDUkeuUfMXqRumEH;
			}
		}

		private float axisValue
		{
			get
			{
				if (!_useDigitalAxisSimulation)
				{
					return _axis.value;
				}
				return GayFivmQsrHyXTnOsAoGyZrYONky;
			}
		}

		private float axisValuePrev
		{
			get
			{
				if (!_useDigitalAxisSimulation)
				{
					return _axis.valuePrev;
				}
				return ZxGKbhZcbUmwqArGhtrVMLaWEajZ;
			}
		}

		private bool buttonValue => _axis.buttonValue;

		private bool buttonValuePrev => _axis.buttonValuePrev;

		private int effectivePointerId
		{
			get
			{
				if (aXEzyiFgJCCEqWfKXxtFBJTVbEHIA == int.MinValue)
				{
					return int.MinValue;
				}
				if (RxCFBSfZGSaxvGjcDdHvHngyjaKGB != int.MinValue)
				{
					return RxCFBSfZGSaxvGjcDdHvHngyjaKGB;
				}
				return aXEzyiFgJCCEqWfKXxtFBJTVbEHIA;
			}
		}

		public event UnityAction<float> AxisValueChangedEvent
		{
			add
			{
				_onAxisValueChanged.AddListener(value);
			}
			remove
			{
				_onAxisValueChanged.RemoveListener(value);
			}
		}

		public event UnityAction<bool> ButtonValueChangedEvent
		{
			add
			{
				_onButtonValueChanged.AddListener(value);
			}
			remove
			{
				_onButtonValueChanged.RemoveListener(value);
			}
		}

		public event UnityAction ButtonDownEvent
		{
			add
			{
				_onButtonDown.AddListener(value);
			}
			remove
			{
				_onButtonDown.RemoveListener(value);
			}
		}

		public event UnityAction ButtonUpEvent
		{
			add
			{
				_onButtonUp.AddListener(value);
			}
			remove
			{
				_onButtonUp.RemoveListener(value);
			}
		}

		[CustomObfuscation(rename = false)]
		private TouchButton()
		{
		}

		public void SetRawValue(float value)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				_axis.SetRawValue(value);
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
				lhmAxKFWhlSHZJoCEAJNVcCScTPH = P_0;
			}
		}

		public void ReturnToDefaultPosition(bool instant)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				xHovnLtQkBnCuETzVQnpnWVuJUuI(lhmAxKFWhlSHZJoCEAJNVcCScTPH, PositionType.Anchored, !instant && _animateOnReturn, _returnSpeed, OpkLqbttkDfdWPvVmlLCialkmZyE.TowardHome);
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
				lhmAxKFWhlSHZJoCEAJNVcCScTPH = base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA.anchoredPosition;
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
				vXIboQFSGKdCLZnWmauizJnaOLuX();
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnValidate()
		{
			base.OnValidate();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				khkdMwnTAvlDqPvCkDAZhRJFGYZR();
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void Reset()
		{
			base.Reset();
			base.transitionType = TransitionTypeFlags.ColorTint;
		}

		internal void OnUpdate()
		{
			base.ZCGETbjMQZUkyflRtYAqwQNUBPQIb();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				nxTbtLCLjbyuyRGfsvxPBPMmlZRq();
				SUSGFUJwfFASbZtQzNdxeGuaWaYnA();
				gvfEiYjewRKHGGOAcrURxtSizbWi();
				if (_followTouchPosition)
				{
					GmcjKJeaVVcqXFRzEcparhxCpdPsA(effectivePointerId);
				}
			}
		}

		internal bool OnInitialize()
		{
			if (!lpOPYPkfRAdylCMSLphTlIgUWIWy())
			{
				return false;
			}
			return true;
		}

		internal void OnCustomControllerUpdate()
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && osKcqUcyYlVGlGygpMaOnUYNqJDBA)
			{
				SPXsRbvylxmzyfkxsgnXmtaaKOyb(_targetCustomControllerElement, axisValue, _axis.buttonActivationThreshold);
			}
		}

		internal void OnSubscribeEvents()
		{
			sGOXUkdDJMvaXZNxRxZLevjalIqm();
			_axis.AxisValueChangedEvent += ghoFULOHEeeqyJlbKyZkQSyDIDpV;
			_axis.ButtonValueChangedEvent += qfUERXIuwfqiizHPBEEFkyvphQUUA;
			_axis.ButtonDownEvent += nmsMSxYgtKmGYShDHFAdlkxbERSEA;
			_axis.ButtonUpEvent += KJnsAwDdnlgDqIbLNFXBLxAueot;
		}

		internal void OnUnsubscribeEvents()
		{
			LYluBUMWPKbwashzGpNPLTeasipd();
			_axis.AxisValueChangedEvent -= ghoFULOHEeeqyJlbKyZkQSyDIDpV;
			_axis.ButtonValueChangedEvent -= qfUERXIuwfqiizHPBEEFkyvphQUUA;
			_axis.ButtonDownEvent -= nmsMSxYgtKmGYShDHFAdlkxbERSEA;
			_axis.ButtonUpEvent -= KJnsAwDdnlgDqIbLNFXBLxAueot;
		}

		internal void OnSetProperty()
		{
			FyKKzlnIjsJaSimYwuqKSwNJHIHx();
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				khkdMwnTAvlDqPvCkDAZhRJFGYZR();
			}
		}

		internal void OnClear()
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				aXEzyiFgJCCEqWfKXxtFBJTVbEHIA = int.MinValue;
				RxCFBSfZGSaxvGjcDdHvHngyjaKGB = int.MinValue;
				ALBJgfsRYJdMVJwePTiUwIfQxDNM = false;
				zmNsczlnaJKPNGfDEeyyFdsXhpjgb = false;
				if (_returnOnRelease && sTmRLabKJLmxGbwWDHHbRrMNNrqn && (_moveToTouchPosition || _followTouchPosition))
				{
					ReturnToDefaultPosition(instant: true);
				}
				sTmRLabKJLmxGbwWDHHbRrMNNrqn = false;
				zAGNVhMcIKxGEzzgSRljWYiyquPd = false;
				yJgkQZuMZrJUEaNCzYYZYngtVmoe = OpkLqbttkDfdWPvVmlLCialkmZyE.None;
				zPMUivaPRCCBydGFdfPeBETKRYnmc();
				_axis.Clear();
				GayFivmQsrHyXTnOsAoGyZrYONky = 0f;
				ZxGKbhZcbUmwqArGhtrVMLaWEajZ = 0f;
				khkdMwnTAvlDqPvCkDAZhRJFGYZR();
			}
		}

		public override void ClearValue()
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				_axis.Clear();
				GayFivmQsrHyXTnOsAoGyZrYONky = 0f;
				if (osKcqUcyYlVGlGygpMaOnUYNqJDBA)
				{
					base.MHuXHKLCPsUIeLOovImpnHVJaYufA.ClearElementValue(_targetCustomControllerElement);
				}
			}
		}

		internal bool IsPressed()
		{
			if (!base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				return false;
			}
			if (!EDufGzVNigBlMAOWvMGsHZmtQaph())
			{
				return false;
			}
			if (!_axis.buttonValue)
			{
				return _axis.value != 0f;
			}
			return true;
		}

		internal bool IsThisOrTouchRegionGameObject(GameObject gameObject)
		{
			if (gameObject == null)
			{
				return false;
			}
			if (base.mxaQUajJeAklKpqxemiTJFTBFCaq(gameObject))
			{
				return true;
			}
			if (DdEciANgMGwGEPvgxnWPHBeJPbOd != null)
			{
				return DdEciANgMGwGEPvgxnWPHBeJPbOd.gameObject == gameObject;
			}
			return false;
		}

		private void gvfEiYjewRKHGGOAcrURxtSizbWi()
		{
			if (_useDigitalAxisSimulation)
			{
				if (_axis.buttonValue)
				{
					YusCwCBXNYifDsXyufhFJipnEDYfA();
				}
				else
				{
					iPyjXvavVUKajQoZZSeerbagLuMc();
				}
			}
		}

		private void YusCwCBXNYifDsXyufhFJipnEDYfA()
		{
			float num = ((_axis.value >= 0f) ? 1f : (-1f));
			float num2 = MathTools.Abs(_digitalAxisSensitivity);
			num *= num2 * Time.unscaledDeltaTime;
			num += GayFivmQsrHyXTnOsAoGyZrYONky;
			num = MathTools.Clamp(num, -1f, 1f);
			SEATEbfdDvHdSchwPlVRmUEOleEvA(num, true);
		}

		private void iPyjXvavVUKajQoZZSeerbagLuMc()
		{
			float num = _digitalAxisGravity;
			if (num == 0f)
			{
				return;
			}
			float gayFivmQsrHyXTnOsAoGyZrYONky = GayFivmQsrHyXTnOsAoGyZrYONky;
			if (gayFivmQsrHyXTnOsAoGyZrYONky != 0f)
			{
				float num2 = num * Time.unscaledDeltaTime;
				float num3;
				if (MathTools.Abs(num2) >= MathTools.Abs(gayFivmQsrHyXTnOsAoGyZrYONky))
				{
					num3 = 0f;
				}
				else
				{
					float num4 = ((gayFivmQsrHyXTnOsAoGyZrYONky > 0f) ? (-1f) : 1f);
					num3 = gayFivmQsrHyXTnOsAoGyZrYONky + num4 * num2;
				}
				SEATEbfdDvHdSchwPlVRmUEOleEvA(num3, true);
			}
		}

		private void SEATEbfdDvHdSchwPlVRmUEOleEvA(float P_0, bool P_1)
		{
			ZxGKbhZcbUmwqArGhtrVMLaWEajZ = GayFivmQsrHyXTnOsAoGyZrYONky;
			GayFivmQsrHyXTnOsAoGyZrYONky = P_0;
			if (P_0 != ZxGKbhZcbUmwqArGhtrVMLaWEajZ)
			{
				MyVxLtVZLEiRaloNqnuyxYfDkCJB(null);
			}
			if (P_1 && P_0 != ZxGKbhZcbUmwqArGhtrVMLaWEajZ)
			{
				_onAxisValueChanged.Invoke(P_0);
			}
		}

		private void nUjZujKpQkHytBBpLGGvYeLbADacA()
		{
			if (_buttonType == ButtonType.ToggleSwitch)
			{
				if (buttonValue)
				{
					_axis.SetRawValue(_axis.rawZero);
				}
				else
				{
					_axis.SetRawValue(_axis.rawMax);
				}
			}
			else if (_buttonType == ButtonType.Standard)
			{
				_axis.SetRawValue(_axis.rawMax);
			}
		}

		private void xDmhrzXrrUeSovFVkqiudNvSOwvT()
		{
			if (_buttonType == ButtonType.Standard)
			{
				_axis.SetRawValue(_axis.rawZero);
			}
		}

		private void khkdMwnTAvlDqPvCkDAZhRJFGYZR()
		{
			_targetCustomControllerElement.ClearElementCaches();
			SUSGFUJwfFASbZtQzNdxeGuaWaYnA();
			aaiAJUIcGSdONuPwfwJBPYnPJCarA();
		}

		private void aaiAJUIcGSdONuPwfwJBPYnPJCarA()
		{
			if (_manageRaycasting)
			{
				RnmnTCNSMxWpWOEFwiXbynsDrRsy.mKNIhyiGRhrEXKxQDuaJFRiTMRVl(base.transform, UZuPaidSRpiDUdxlqCyJWkepnHav());
			}
		}

		private bool UZuPaidSRpiDUdxlqCyJWkepnHav()
		{
			if (DdEciANgMGwGEPvgxnWPHBeJPbOd != null && _useTouchRegionOnly)
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
			}
		}

		private void SUSGFUJwfFASbZtQzNdxeGuaWaYnA()
		{
			if (!(DdEciANgMGwGEPvgxnWPHBeJPbOd == _touchRegion))
			{
				dlnLVQxHbOeHUrCToOHILuLpTVKI(DdEciANgMGwGEPvgxnWPHBeJPbOd);
				DdEciANgMGwGEPvgxnWPHBeJPbOd = _touchRegion;
				duaIiAtDTEcsqfybpdyfGRlfYedB(DdEciANgMGwGEPvgxnWPHBeJPbOd);
			}
		}

		private void jbQgKcxKQGzETsTgYjuvYckRsvZo(Vector2 P_0, bool P_1, float P_2, OpkLqbttkDfdWPvVmlLCialkmZyE P_3)
		{
			RectTransform rectTransform = base.transform.parent as RectTransform;
			Vector2 vector = sFgWwfUJkMIxinvyEhNsisGjhMkS.PCIXWfbuGOHlbBawYtVicRpFfIoX(base.vQpmvzIqZzQRgZjNdTCOIzNihLiH, rectTransform, P_0);
			Vector2 pivot = base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA.pivot;
			Vector2 sizeDelta = base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA.sizeDelta;
			Vector3 localScale = base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA.localScale;
			vector += new Vector2((pivot.x - 0.5f) * sizeDelta.x * localScale.x, (pivot.y - 0.5f) * sizeDelta.y * localScale.y);
			xHovnLtQkBnCuETzVQnpnWVuJUuI(vector, PositionType.Local, P_1, P_2, P_3);
		}

		private void xHovnLtQkBnCuETzVQnpnWVuJUuI(Vector2 P_0, PositionType P_1, bool P_2, float P_3, OpkLqbttkDfdWPvVmlLCialkmZyE P_4)
		{
			if (zAGNVhMcIKxGEzzgSRljWYiyquPd && P_2 && yJgkQZuMZrJUEaNCzYYZYngtVmoe == P_4)
			{
				return;
			}
			if (zAGNVhMcIKxGEzzgSRljWYiyquPd && FcwfREdgzibGQMOLZwevNMKYBahxA != null)
			{
				zPMUivaPRCCBydGFdfPeBETKRYnmc();
				zAGNVhMcIKxGEzzgSRljWYiyquPd = false;
				yJgkQZuMZrJUEaNCzYYZYngtVmoe = OpkLqbttkDfdWPvVmlLCialkmZyE.None;
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
				FcwfREdgzibGQMOLZwevNMKYBahxA = tIfcRAEhuytogERPthhbDYOlhzfR(P_0, P_1, P_3, P_4);
				StartCoroutine(FcwfREdgzibGQMOLZwevNMKYBahxA);
				yJgkQZuMZrJUEaNCzYYZYngtVmoe = P_4;
				sTmRLabKJLmxGbwWDHHbRrMNNrqn = true;
				moveStartedDelegate(P_4);
			}
			else
			{
				moveStartedDelegate(P_4);
				ROeJSkZhnkTDcLHfcKCeHhXUonug(P_4, P_0, P_1);
			}
		}

		private IEnumerator tIfcRAEhuytogERPthhbDYOlhzfR(Vector2 P_0, PositionType P_1, float P_2, OpkLqbttkDfdWPvVmlLCialkmZyE P_3)
		{
			return new KykhxUELCIErMkPZtnTHwKXXpCVb(0)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				JTKktbfSAqGDQjeacpqTtOeybKpt = P_0,
				NrdyBFMMWwnWFfEyWKBnxrSnZvnA = P_1,
				QzkoAujiXELSEATTFbUXkPALueUS = P_2,
				sGqbUlnFwRgsWfMjUESVfFgHGuQnb = P_3
			};
		}

		private void ROeJSkZhnkTDcLHfcKCeHhXUonug(OpkLqbttkDfdWPvVmlLCialkmZyE P_0, Vector2 P_1, PositionType P_2)
		{
			sFgWwfUJkMIxinvyEhNsisGjhMkS.jdVjbFKPJGjGyhWuwNTOVPPfnNwT(base.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA, P_1, P_2);
			zAGNVhMcIKxGEzzgSRljWYiyquPd = false;
			yJgkQZuMZrJUEaNCzYYZYngtVmoe = OpkLqbttkDfdWPvVmlLCialkmZyE.None;
			switch (P_0)
			{
			case OpkLqbttkDfdWPvVmlLCialkmZyE.TowardHome:
				sTmRLabKJLmxGbwWDHHbRrMNNrqn = false;
				break;
			case OpkLqbttkDfdWPvVmlLCialkmZyE.TowardTouch:
				sTmRLabKJLmxGbwWDHHbRrMNNrqn = true;
				break;
			}
			zPMUivaPRCCBydGFdfPeBETKRYnmc();
			moveEndedDelegate(P_0);
		}

		private void XNKImEVsUNPavzxgPJMfIdoSEAWgA(OpkLqbttkDfdWPvVmlLCialkmZyE P_0)
		{
			if (_manageRaycasting)
			{
				bool flag = false;
				bool flag2 = false;
				if (((_followTouchPosition && stayActiveOnSwipeOut) || (!_followTouchPosition && DdEciANgMGwGEPvgxnWPHBeJPbOd != null && !_useTouchRegionOnly && _moveToTouchPosition)) && _returnOnRelease && P_0 == OpkLqbttkDfdWPvVmlLCialkmZyE.TowardTouch)
				{
					flag = true;
					flag2 = false;
				}
				if (flag)
				{
					RnmnTCNSMxWpWOEFwiXbynsDrRsy.mKNIhyiGRhrEXKxQDuaJFRiTMRVl(base.transform, flag2);
				}
			}
		}

		private void UkrWacxKoOMuRkiKLRmEmILrFhRJ(OpkLqbttkDfdWPvVmlLCialkmZyE P_0)
		{
			if (_manageRaycasting)
			{
				bool flag = false;
				bool flag2 = false;
				if (((_followTouchPosition && stayActiveOnSwipeOut) || (!_followTouchPosition && DdEciANgMGwGEPvgxnWPHBeJPbOd != null && !_useTouchRegionOnly && _moveToTouchPosition)) && _returnOnRelease && P_0 == OpkLqbttkDfdWPvVmlLCialkmZyE.TowardHome)
				{
					flag = true;
					flag2 = UZuPaidSRpiDUdxlqCyJWkepnHav();
				}
				if (flag)
				{
					RnmnTCNSMxWpWOEFwiXbynsDrRsy.mKNIhyiGRhrEXKxQDuaJFRiTMRVl(base.transform, flag2);
				}
			}
		}

		private void GmcjKJeaVVcqXFRzEcparhxCpdPsA(int P_0)
		{
			if (TouchInteractable.uwuMHSNgPoeyIZKBbzpvCZyfdwOl(P_0))
			{
				jbQgKcxKQGzETsTgYjuvYckRsvZo(TouchInteractable.TgEgBYQvFnhTTkcyuYMooLsCFzzE(P_0), false, 0f, OpkLqbttkDfdWPvVmlLCialkmZyE.TowardTouch);
			}
		}

		private void zPMUivaPRCCBydGFdfPeBETKRYnmc()
		{
			if (FcwfREdgzibGQMOLZwevNMKYBahxA != null)
			{
				try
				{
					StopCoroutine(FcwfREdgzibGQMOLZwevNMKYBahxA);
				}
				catch
				{
				}
				FcwfREdgzibGQMOLZwevNMKYBahxA = null;
			}
		}

		private void nxTbtLCLjbyuyRGfsvxPBPMmlZRq()
		{
			if (hasPointer && !TouchInteractable.uwuMHSNgPoeyIZKBbzpvCZyfdwOl(effectivePointerId))
			{
				PointerEventData pointerEventData = HyACTUHUMITHJlwRMGbSiKvZntNC(effectivePointerId);
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
			aXEzyiFgJCCEqWfKXxtFBJTVbEHIA = int.MinValue;
			RxCFBSfZGSaxvGjcDdHvHngyjaKGB = int.MinValue;
		}

		private bool SBhvinEbIDGWUMwfXythhrIpigKu(int P_0)
		{
			if (P_0 == int.MinValue)
			{
				return false;
			}
			if (aXEzyiFgJCCEqWfKXxtFBJTVbEHIA == int.MinValue)
			{
				return false;
			}
			if (aXEzyiFgJCCEqWfKXxtFBJTVbEHIA == P_0)
			{
				return true;
			}
			if (TouchInteractable.sWBkWrgBUhjXAreNdBTEqWiNGgHjA(P_0) && RxCFBSfZGSaxvGjcDdHvHngyjaKGB != int.MinValue && P_0 == RxCFBSfZGSaxvGjcDdHvHngyjaKGB)
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
				pVfmwALlLutDLwJmMFqcBjCFoHIS(effectivePointerId);
			}
		}

		private PointerEventData HyACTUHUMITHJlwRMGbSiKvZntNC(int P_0)
		{
			if (P_0 == int.MinValue)
			{
				return null;
			}
			if (EeRYMczwTzlgDjdFdgjMeYiqqewh == null)
			{
				EeRYMczwTzlgDjdFdgjMeYiqqewh = new Dictionary<int, PointerEventData>();
			}
			if (!EeRYMczwTzlgDjdFdgjMeYiqqewh.TryGetValue(P_0, out var value))
			{
				value = new PointerEventData(EventSystem.current);
				value.pointerId = P_0;
				EeRYMczwTzlgDjdFdgjMeYiqqewh.Add(P_0, value);
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

		private void kenMZNZStIQnTAavioStPdqUwyIF(PointerEventData P_0, KNuxcpaopMTVUOnDxSWfwXInKYgs P_1)
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

		private void ZEbUZdUWFIEfiSqeUnrGJHjcPybD(PointerEventData P_0, KNuxcpaopMTVUOnDxSWfwXInKYgs P_1)
		{
			if ((!hasPointer || SBhvinEbIDGWUMwfXythhrIpigKu(P_0.pointerId)) && !TouchInteractable.uwuMHSNgPoeyIZKBbzpvCZyfdwOl(effectivePointerId))
			{
				MHcBbLUbNvVfJsmQBGwoxkdhumdt();
				base.OnPointerUp(P_0);
			}
		}

		private void ZWAzvWwAMRawEGwPixAMPjfyqNgpA(PointerEventData P_0, KNuxcpaopMTVUOnDxSWfwXInKYgs P_1)
		{
			if (hasPointer && !SBhvinEbIDGWUMwfXythhrIpigKu(P_0.pointerId))
			{
				return;
			}
			bool flag = TouchInteractable.sWBkWrgBUhjXAreNdBTEqWiNGgHjA(P_0.pointerId);
			bool flag2 = false;
			MouseButtonFlags mouseButtonFlags = P_1 switch
			{
				KNuxcpaopMTVUOnDxSWfwXInKYgs.Local => base.allowedMouseButtons, 
				KNuxcpaopMTVUOnDxSWfwXInKYgs.TouchRegion => _touchRegion.allowedMouseButtons, 
				_ => throw new NotImplementedException(), 
			};
			if (_activateOnSwipeIn && EDufGzVNigBlMAOWvMGsHZmtQaph() && IsInteractable() && (!flag || TouchInteractable.LTWUvUGhphDXhnFmUjsOImULiiXRA(mouseButtonFlags)) && !ALBJgfsRYJdMVJwePTiUwIfQxDNM)
			{
				if (flag)
				{
					if (TouchInteractable.ZbAsyupCrOtBnZlFjdZxrpdrFStI(mouseButtonFlags, out var rxCFBSfZGSaxvGjcDdHvHngyjaKGB))
					{
						RxCFBSfZGSaxvGjcDdHvHngyjaKGB = rxCFBSfZGSaxvGjcDdHvHngyjaKGB;
					}
					else
					{
						RxCFBSfZGSaxvGjcDdHvHngyjaKGB = P_0.pointerId;
					}
				}
				flag2 = true;
			}
			base.OnPointerEnter(P_0);
			if (flag2)
			{
				GameObject gameObject = P_1 switch
				{
					KNuxcpaopMTVUOnDxSWfwXInKYgs.Local => base.gameObject, 
					KNuxcpaopMTVUOnDxSWfwXInKYgs.TouchRegion => DdEciANgMGwGEPvgxnWPHBeJPbOd.gameObject, 
					_ => throw new NotImplementedException(), 
				};
				PointerEventData pointerEventData = utfJPhgnwTqKykeFFJYQHUTouefi((RxCFBSfZGSaxvGjcDdHvHngyjaKGB != int.MinValue) ? RxCFBSfZGSaxvGjcDdHvHngyjaKGB : P_0.pointerId, gameObject);
				if (pointerEventData != null)
				{
					kenMZNZStIQnTAavioStPdqUwyIF(pointerEventData, P_1);
				}
			}
			zmNsczlnaJKPNGfDEeyyFdsXhpjgb = true;
		}

		private void zHqZkjRbrFTkqMfkkoUpBAuhCzKfA(PointerEventData P_0, KNuxcpaopMTVUOnDxSWfwXInKYgs P_1)
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

		private void PARBcfOJshIGNJtBDsuAyiETdYwy(int P_0, Vector2 P_1, KNuxcpaopMTVUOnDxSWfwXInKYgs P_2)
		{
			aXEzyiFgJCCEqWfKXxtFBJTVbEHIA = P_0;
			ALBJgfsRYJdMVJwePTiUwIfQxDNM = true;
			if (_followTouchPosition)
			{
				GmcjKJeaVVcqXFRzEcparhxCpdPsA(P_0);
			}
			else if (P_2 == KNuxcpaopMTVUOnDxSWfwXInKYgs.TouchRegion && _moveToTouchPosition)
			{
				jbQgKcxKQGzETsTgYjuvYckRsvZo(P_1, _animateOnMoveToTouch, _moveToTouchSpeed, OpkLqbttkDfdWPvVmlLCialkmZyE.TowardTouch);
			}
			nUjZujKpQkHytBBpLGGvYeLbADacA();
		}

		private void MHcBbLUbNvVfJsmQBGwoxkdhumdt()
		{
			rEkWtHXjAKTOKMkJBxWaofkAFFvH();
			ALBJgfsRYJdMVJwePTiUwIfQxDNM = false;
			if ((_followTouchPosition || _moveToTouchPosition) && _returnOnRelease && sTmRLabKJLmxGbwWDHHbRrMNNrqn)
			{
				ReturnToDefaultPosition();
			}
			xDmhrzXrrUeSovFVkqiudNvSOwvT();
		}

		internal override void OnPointerDown(PointerEventData eventData)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, base.allowedMouseButtons, EventTriggerType.PointerDown) && (!(DdEciANgMGwGEPvgxnWPHBeJPbOd != null) || !_useTouchRegionOnly))
			{
				kenMZNZStIQnTAavioStPdqUwyIF(eventData, KNuxcpaopMTVUOnDxSWfwXInKYgs.Local);
			}
		}

		internal override void OnPointerUp(PointerEventData eventData)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, base.allowedMouseButtons, EventTriggerType.PointerUp) && (!(DdEciANgMGwGEPvgxnWPHBeJPbOd != null) || !_useTouchRegionOnly))
			{
				ZEbUZdUWFIEfiSqeUnrGJHjcPybD(eventData, KNuxcpaopMTVUOnDxSWfwXInKYgs.Local);
			}
		}

		internal override void OnPointerEnter(PointerEventData eventData)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, base.allowedMouseButtons, EventTriggerType.PointerEnter) && (!(DdEciANgMGwGEPvgxnWPHBeJPbOd != null) || !_useTouchRegionOnly))
			{
				ZWAzvWwAMRawEGwPixAMPjfyqNgpA(eventData, KNuxcpaopMTVUOnDxSWfwXInKYgs.Local);
			}
		}

		internal override void OnPointerExit(PointerEventData eventData)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(eventData.pointerId, base.allowedMouseButtons, EventTriggerType.PointerExit) && (!(DdEciANgMGwGEPvgxnWPHBeJPbOd != null) || !_useTouchRegionOnly))
			{
				zHqZkjRbrFTkqMfkkoUpBAuhCzKfA(eventData, KNuxcpaopMTVUOnDxSWfwXInKYgs.Local);
			}
		}

		private void lEDjUpnxqBucRFkEDyJKbVEmsmkA(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerDown))
			{
				kenMZNZStIQnTAavioStPdqUwyIF(P_0, KNuxcpaopMTVUOnDxSWfwXInKYgs.TouchRegion);
			}
		}

		private void EuQQZFwdcthAzHoBOBDBcyHGjRwyB(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerUp))
			{
				ZEbUZdUWFIEfiSqeUnrGJHjcPybD(P_0, KNuxcpaopMTVUOnDxSWfwXInKYgs.TouchRegion);
			}
		}

		private void OmQwIffSZPImRUCIYEOwFrZRxIQW(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerEnter))
			{
				ZWAzvWwAMRawEGwPixAMPjfyqNgpA(P_0, KNuxcpaopMTVUOnDxSWfwXInKYgs.TouchRegion);
			}
		}

		private void PnDoCbelFWRcICvBVIgnpCPKYTkI(PointerEventData P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && TouchInteractable.AgilIEWjYyfxLMkJfmkwQMfVpnhH(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerExit))
			{
				zHqZkjRbrFTkqMfkkoUpBAuhCzKfA(P_0, KNuxcpaopMTVUOnDxSWfwXInKYgs.TouchRegion);
			}
		}

		private void ghoFULOHEeeqyJlbKyZkQSyDIDpV(float P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao && !_useDigitalAxisSimulation)
			{
				MyVxLtVZLEiRaloNqnuyxYfDkCJB(null);
				_onAxisValueChanged.Invoke(P_0);
			}
		}

		private void qfUERXIuwfqiizHPBEEFkyvphQUUA(bool P_0)
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				MyVxLtVZLEiRaloNqnuyxYfDkCJB(null);
				_onButtonValueChanged.Invoke(P_0);
			}
		}

		private void nmsMSxYgtKmGYShDHFAdlkxbERSEA()
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				MyVxLtVZLEiRaloNqnuyxYfDkCJB(null);
				_onButtonDown.Invoke();
			}
		}

		private void KJnsAwDdnlgDqIbLNFXBLxAueot()
		{
			if (base.jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				MyVxLtVZLEiRaloNqnuyxYfDkCJB(null);
				_onButtonUp.Invoke();
			}
		}
	}
}
