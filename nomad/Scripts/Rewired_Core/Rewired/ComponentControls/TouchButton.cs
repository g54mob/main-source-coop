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
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[AddComponentMenu("Rewired/Touch Controls/Touch Button")]
	public sealed class TouchButton : TouchInteractable
	{
		public enum ButtonType
		{
			Standard = 0,
			ToggleSwitch = 1
		}

		private enum aVYRyhLvDnDQRtsgCbXARDiQyrlL
		{
			None = 0,
			TowardTouch = 1,
			TowardHome = 2
		}

		private enum oZSPUtSMLykXFyaOBGLpRDfTGQxD
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

		private sealed class eMKEIxFumelteMSgBIbPxXPjmvTgA : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int iRtjKLgbcRMcMUxhTOgobKUMqzYK;

			private object MwEDocKARRgKhUxTGDnKPwooHDqM;

			public float DAwBTBGSdPlAWFcbwgPRogqUmnkN;

			public TouchButton WsLhEveRVYFLoBBCBUwJehRhOFMx;

			public PositionType YFAcNYpXzKFwLtRlxYrnbcRuHvrm;

			public Vector2 KMdAaGHbcHVmtfpitwHrdWNqXXkE;

			public aVYRyhLvDnDQRtsgCbXARDiQyrlL rjTaJmdJwXvSGkCVHcSGSsUcYXCkA;

			private RectTransform OhAGReSZANCgQAhRmYFPbvAZSkqg;

			private Vector2 unAAhIyWlCAwTwzYMZcTubdYnxWK;

			private float WybFaVKDQzPxzFLCCKFFUbRiiwry;

			private float DosWZKRjCWtSCVNvJADEAdUEWlBP;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return MwEDocKARRgKhUxTGDnKPwooHDqM;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return MwEDocKARRgKhUxTGDnKPwooHDqM;
				}
			}

			[DebuggerHidden]
			public eMKEIxFumelteMSgBIbPxXPjmvTgA(int P_0)
			{
				iRtjKLgbcRMcMUxhTOgobKUMqzYK = P_0;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				OhAGReSZANCgQAhRmYFPbvAZSkqg = null;
				iRtjKLgbcRMcMUxhTOgobKUMqzYK = -2;
			}

			private bool MoveNext()
			{
				int num = iRtjKLgbcRMcMUxhTOgobKUMqzYK;
				TouchButton wsLhEveRVYFLoBBCBUwJehRhOFMx = WsLhEveRVYFLoBBCBUwJehRhOFMx;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					iRtjKLgbcRMcMUxhTOgobKUMqzYK = -1;
					goto IL_010c;
				}
				iRtjKLgbcRMcMUxhTOgobKUMqzYK = -1;
				if (!(DAwBTBGSdPlAWFcbwgPRogqUmnkN <= 0f))
				{
					OhAGReSZANCgQAhRmYFPbvAZSkqg = wsLhEveRVYFLoBBCBUwJehRhOFMx.XgRjXqPEGoTEAcTuxzCYeFqkaaly;
					unAAhIyWlCAwTwzYMZcTubdYnxWK = ItUXShgHgqMTvNPoaDBaDFBZYvtd.BrgGlubhkUYkSMDAYAtTVxcWogbn(OhAGReSZANCgQAhRmYFPbvAZSkqg, YFAcNYpXzKFwLtRlxYrnbcRuHvrm);
					float magnitude = (KMdAaGHbcHVmtfpitwHrdWNqXXkE - unAAhIyWlCAwTwzYMZcTubdYnxWK).magnitude;
					if (!(magnitude < 0.01f))
					{
						wsLhEveRVYFLoBBCBUwJehRhOFMx.LavfOtccbuiAxIFfAselJsTDBFVTA = true;
						WybFaVKDQzPxzFLCCKFFUbRiiwry = magnitude / DAwBTBGSdPlAWFcbwgPRogqUmnkN;
						DosWZKRjCWtSCVNvJADEAdUEWlBP = 0f;
						goto IL_010c;
					}
				}
				goto IL_0119;
				IL_0119:
				wsLhEveRVYFLoBBCBUwJehRhOFMx.yIlvcDQWdPMQJVKNWtiEfPJgdqsm(rjTaJmdJwXvSGkCVHcSGSsUcYXCkA, KMdAaGHbcHVmtfpitwHrdWNqXXkE, YFAcNYpXzKFwLtRlxYrnbcRuHvrm);
				return false;
				IL_010c:
				if (DosWZKRjCWtSCVNvJADEAdUEWlBP <= 1f)
				{
					DosWZKRjCWtSCVNvJADEAdUEWlBP += Time.unscaledDeltaTime / WybFaVKDQzPxzFLCCKFFUbRiiwry;
					ItUXShgHgqMTvNPoaDBaDFBZYvtd.AVBaoIMoopyfpbSwkSCPNfEaGuXL(OhAGReSZANCgQAhRmYFPbvAZSkqg, Vector2.Lerp(unAAhIyWlCAwTwzYMZcTubdYnxWK, KMdAaGHbcHVmtfpitwHrdWNqXXkE, Mathf.SmoothStep(0f, 1f, DosWZKRjCWtSCVNvJADEAdUEWlBP)), YFAcNYpXzKFwLtRlxYrnbcRuHvrm);
					MwEDocKARRgKhUxTGDnKPwooHDqM = null;
					iRtjKLgbcRMcMUxhTOgobKUMqzYK = 1;
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

		private const float brvjoFQMzFYQZTjbUBlEvfGKQkYk = 20f;

		[Tooltip("The Custom Controller element that will receive input values from this control.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private CustomControllerElementTargetSetForFloat _targetCustomControllerElement = new CustomControllerElementTargetSetForFloat(new CustomControllerElementTarget(new CustomControllerElementSelector
		{
			elementType = CustomControllerElementSelector.ElementType.Button
		}));

		[Tooltip("The type of button.\nStandard: A momentary switch. Returns True while the button is pressed down.\nToggle Switch: Alternately turns on and off with each press.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ButtonType _buttonType;

		[Tooltip("If true, the button can be turned on by a touch swipe that began in an area outside the button region. If false, the button can only be turned on by a direct press.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _activateOnSwipeIn;

		[Tooltip("If true, the button will stay on even if the touch that activated it moves outside the button region. If false, the button will turn off once the touch that activated it moves outside the button region.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _stayActiveOnSwipeOut = true;

		[Tooltip("Makes the axis value gradually change over time based on gravity and sensitivity as the button is pressed.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _useDigitalAxisSimulation;

		[Tooltip("Speed (units/sec) that the axis value falls toward 0 when not pressed. A value of 1.0 means an axis value of 1 will drain to 0 over 1 second. A value of 3 equates to 1/3 of a second, and so on.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[FieldRange(0f, 1f / 0f)]
		private float _digitalAxisGravity = 3f;

		[Tooltip("Speed to move toward an axis value of 1.0 in units/sec when pressed. A value of 1.0 means an axis value of 0 will reach 1 over 1 second. A value of 3 equates to 1/3 of a second, and so on.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[FieldRange(0f, 1f / 0f)]
		private float _digitalAxisSensitivity = 3f;

		[Tooltip("The internal axis of the button. The axis is used for all value calculations.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private StandaloneAxis _axis = new StandaloneAxis();

		[Tooltip("Optional external region to use for hover/click/touch detection. If set, this region will be used for touch detection instead of or in addition to the button's RectTransform. This can be useful if you want a larger area of the screen to act as a button.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private TouchRegion _touchRegion;

		[Tooltip("If True, hovers/clicks/touches on the local button will be ignored and only Touch Region touches will be used. Otherwise, both touches on the button and on the Touch Region will be used. This also applies to mouse hover. This setting has no effect if no Touch Region is set.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _useTouchRegionOnly = true;

		[Tooltip("If True, the button will move to the location of the current touch in the Touch Region. This can be used to designate an area of the screen as a hot-spot for a button and have the button graphics follow the users touches. This only has an effect if a Touch Region is set.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _moveToTouchPosition;

		[Tooltip("If Move To Touch Position is enabled, this will make the button return to its original position after the press is released. This only has an effect if a Touch Region is set.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _returnOnRelease = true;

		[Tooltip("If True, the button will follow the touch around until released. This setting overrides Move To Touch Position.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _followTouchPosition;

		[Tooltip("Should the button animate when moving to the touch point? This only has an effect if Move To Touch Position is True and a Touch Region is set. This setting is ignored if Follow Touch Position is True.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _animateOnMoveToTouch = true;

		[Tooltip("The speed at which the button will move toward the touch position measured in screens per second (based on the larger of width and height). [1.0 = Move 1 screen/sec]. This only has an effect if Move To Touch Position is True, Animate On Move To Touch is true, and a Touch Region is set. This setting is ignored if Follow Touch Position is True.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Range(0f, 20f)]
		private float _moveToTouchSpeed = 2f;

		[Tooltip("Should the button animate when moving back to its original position? This only has an effect if Follow Touch Position is True, or if Move To Touch Position is True and a Touch Region is set, and Return on Release is True.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _animateOnReturn = true;

		[Tooltip("The speed at which the button will move back toward its original position measured in screens per second (based on the larger of width and height). [1.0 = Move 1 screen/sec]. This only has an effect if Follow Touch Position is True, or if Move To Touch Position is True and a Touch Region is set, and Return on Release and Animate on Return are both True.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Range(0f, 20f)]
		private float _returnSpeed = 2f;

		[Tooltip("If True, it will attempt to automatically manage Graphic component raycasting for best results based on your current settings.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _manageRaycasting = true;

		private float veqIdftfxVwqrRMjNLnghBsjRQxj;

		private float morhpiFCxEcCMCmJZCAAfatUCuXoA;

		private TouchRegion KmMjlOsEwHJinoEgDQYqQKuWedzN;

		private Vector2 MqlCxycIVsoUTBdcDeBpGwrdBvZt;

		private bool LavfOtccbuiAxIFfAselJsTDBFVTA;

		private bool zJmUhIMSOXJCOwCWarexWLLAaJCE;

		private aVYRyhLvDnDQRtsgCbXARDiQyrlL rnvFtgSEnnHFCAKsHMzPgjtsvPcG;

		private int aPtZESJuhKiNXuvEGsNqLcgZebmGA = -2147483648;

		private int FjrauICVvjmGCWKEOsHOrIIljKfGA = -2147483648;

		[NonSerialized]
		private bool JlZaTEhesShUrbCppitCuzfsHjNTA;

		[NonSerialized]
		private bool tgYlkgwcMrhcNjjuPouHldoNtXbf;

		private IEnumerator AicESUopQddcJjSwuhiQPKcFksgk;

		private KbsjaYbXcZsfBGHFCqdYNahakHjI iuhdHlvlQrGTlTZEdGaHAkNJaFiW = new KbsjaYbXcZsfBGHFCqdYNahakHjI();

		private Action<aVYRyhLvDnDQRtsgCbXARDiQyrlL> RGecsFShySetacPaCoPHIvlaWMul;

		private Action<aVYRyhLvDnDQRtsgCbXARDiQyrlL> HGXDlHcUPMiwKRiLbCQkwXdfLJWN;

		[Tooltip("Event sent when the axis value changes.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private AxisValueChangedEventHandler _onAxisValueChanged = new AxisValueChangedEventHandler();

		[Tooltip("Event sent when the button value changes.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ButtonValueChangedEventHandler _onButtonValueChanged = new ButtonValueChangedEventHandler();

		[Tooltip("Event sent when the button is pressed.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ButtonDownEventHandler _onButtonDown = new ButtonDownEventHandler();

		[Tooltip("Event sent when the button is released.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private ButtonUpEventHandler _onButtonUp = new ButtonUpEventHandler();

		private Dictionary<int, PointerEventData> aoAVxmCuBwuoJwIYhcofcRNrOSzdA;

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
				if (MYyypwTdepcxSPYAsjzvenIHuNYP())
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
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
						GFVSOWRrjQhhJVXfnWxbhsmeoKmK();
					}
					else
					{
						iuhdHlvlQrGTlTZEdGaHAkNJaFiW.WnkVoLeLcDIxHhUkiCBDfHgIYoxub();
					}
					TmwAiSAtmeUBheGXtgadcPKUnBaqA();
				}
			}
		}

		public int pointerId
		{
			get
			{
				return aPtZESJuhKiNXuvEGsNqLcgZebmGA;
			}
			set
			{
				aPtZESJuhKiNXuvEGsNqLcgZebmGA = value;
			}
		}

		public bool hasPointer => aPtZESJuhKiNXuvEGsNqLcgZebmGA != -2147483648;

		internal StandaloneAxis axis => _axis;

		private Action<aVYRyhLvDnDQRtsgCbXARDiQyrlL> moveStartedDelegate
		{
			get
			{
				if (RGecsFShySetacPaCoPHIvlaWMul == null)
				{
					return RGecsFShySetacPaCoPHIvlaWMul = DvVfVhptWdFJwGhwEgojUQvqZfVEA;
				}
				return RGecsFShySetacPaCoPHIvlaWMul;
			}
		}

		private Action<aVYRyhLvDnDQRtsgCbXARDiQyrlL> moveEndedDelegate
		{
			get
			{
				if (HGXDlHcUPMiwKRiLbCQkwXdfLJWN == null)
				{
					return HGXDlHcUPMiwKRiLbCQkwXdfLJWN = LseoHtzRyqSPWMMDQHEDANiMdGpfb;
				}
				return HGXDlHcUPMiwKRiLbCQkwXdfLJWN;
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
				return veqIdftfxVwqrRMjNLnghBsjRQxj;
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
				return morhpiFCxEcCMCmJZCAAfatUCuXoA;
			}
		}

		private bool buttonValue => _axis.buttonValue;

		private bool buttonValuePrev => _axis.buttonValuePrev;

		private int effectivePointerId
		{
			get
			{
				if (aPtZESJuhKiNXuvEGsNqLcgZebmGA == -2147483648)
				{
					return -2147483648;
				}
				if (FjrauICVvjmGCWKEOsHOrIIljKfGA != -2147483648)
				{
					return FjrauICVvjmGCWKEOsHOrIIljKfGA;
				}
				return aPtZESJuhKiNXuvEGsNqLcgZebmGA;
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
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				_axis.SetRawValue(value);
			}
		}

		public void SetDefaultPosition()
		{
			eqceivjgpaQYbEkJpZwFEhCbojOd(base.XgRjXqPEGoTEAcTuxzCYeFqkaaly.anchoredPosition);
		}

		private void eqceivjgpaQYbEkJpZwFEhCbojOd(Vector2 P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				MqlCxycIVsoUTBdcDeBpGwrdBvZt = P_0;
			}
		}

		public void ReturnToDefaultPosition(bool instant)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				POIDaPjFhCPKiTdaZKCZHBUIOZoIB(MqlCxycIVsoUTBdcDeBpGwrdBvZt, PositionType.Anchored, !instant && _animateOnReturn, _returnSpeed, aVYRyhLvDnDQRtsgCbXARDiQyrlL.TowardHome);
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
				MqlCxycIVsoUTBdcDeBpGwrdBvZt = base.XgRjXqPEGoTEAcTuxzCYeFqkaaly.anchoredPosition;
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnEnable()
		{
			base.OnEnable();
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				mjzgvUOBUzKfqEPMBMKZfGXzperx();
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnDisable()
		{
			base.OnDisable();
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				oupESpAKdTodzbcjadIjdJbxShhrb();
			}
		}

		[CustomObfuscation(rename = false)]
		internal override void OnValidate()
		{
			base.OnValidate();
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				mjzgvUOBUzKfqEPMBMKZfGXzperx();
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
			base.MJCnBosQYbUlJanFrVIDjtRnnxaI();
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				QXTrmSxQzUbwnSHcszrMeWzNYMie();
				wnyAQmYBHgcEKhsVMsmmIScDnfFF();
				vqxSTcDCgjSzUXgmfwxFiZtMLXad();
				if (_followTouchPosition)
				{
					xTpBoZCnIkZxobWXzPguQAmkLETaA(effectivePointerId);
				}
			}
		}

		internal bool OnInitialize()
		{
			if (!ngChnvFyiPBLCncDhdTerijdqmaY())
			{
				return false;
			}
			return true;
		}

		internal void OnCustomControllerUpdate()
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && ghAsJwRhDmClYOgMqzKuSmomibZfA)
			{
				GLmFZCmUYCmgFyInFMQJxMkuRoJw(_targetCustomControllerElement, axisValue, _axis.buttonActivationThreshold);
			}
		}

		internal void OnSubscribeEvents()
		{
			KXZAZBKCBLiVYWwSNbnniwYSIrBkA();
			_axis.AxisValueChangedEvent += ghfquyOxSLqEuLUZibQmrISpwxLk;
			_axis.ButtonValueChangedEvent += mroKnxkYNFzSDxsukvJsGBzsnCMF;
			_axis.ButtonDownEvent += BAfbKTCGSVqeKFaTvcQBeIJbyDZxA;
			_axis.ButtonUpEvent += xtadVNVmkrkhOAnIyjQdWUMsRmxQ;
		}

		internal void OnUnsubscribeEvents()
		{
			rYnUOltSwBtwnpEBEVDesiwZDlZGA();
			_axis.AxisValueChangedEvent -= ghfquyOxSLqEuLUZibQmrISpwxLk;
			_axis.ButtonValueChangedEvent -= mroKnxkYNFzSDxsukvJsGBzsnCMF;
			_axis.ButtonDownEvent -= BAfbKTCGSVqeKFaTvcQBeIJbyDZxA;
			_axis.ButtonUpEvent -= xtadVNVmkrkhOAnIyjQdWUMsRmxQ;
		}

		internal void OnSetProperty()
		{
			ZFNYLzeTNLUpKCdifbpdezFLAlNRA();
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				mjzgvUOBUzKfqEPMBMKZfGXzperx();
			}
		}

		internal void OnClear()
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				aPtZESJuhKiNXuvEGsNqLcgZebmGA = -2147483648;
				FjrauICVvjmGCWKEOsHOrIIljKfGA = -2147483648;
				JlZaTEhesShUrbCppitCuzfsHjNTA = false;
				tgYlkgwcMrhcNjjuPouHldoNtXbf = false;
				if (_returnOnRelease && zJmUhIMSOXJCOwCWarexWLLAaJCE && (_moveToTouchPosition || _followTouchPosition))
				{
					ReturnToDefaultPosition(instant: true);
				}
				zJmUhIMSOXJCOwCWarexWLLAaJCE = false;
				LavfOtccbuiAxIFfAselJsTDBFVTA = false;
				rnvFtgSEnnHFCAKsHMzPgjtsvPcG = aVYRyhLvDnDQRtsgCbXARDiQyrlL.None;
				lvjegEHHcnrRhXQKhAEgtTHvhHoW();
				_axis.Clear();
				veqIdftfxVwqrRMjNLnghBsjRQxj = 0f;
				morhpiFCxEcCMCmJZCAAfatUCuXoA = 0f;
				mjzgvUOBUzKfqEPMBMKZfGXzperx();
			}
		}

		public override void ClearValue()
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				_axis.Clear();
				veqIdftfxVwqrRMjNLnghBsjRQxj = 0f;
				if (ghAsJwRhDmClYOgMqzKuSmomibZfA)
				{
					base.KIgQwfyVIoOiIXKpXkLoXgtMAONn.ClearElementValue(_targetCustomControllerElement);
				}
			}
		}

		internal bool IsPressed()
		{
			if (!base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				return false;
			}
			if (!IcqbeYEmGpfkqqAVukZKtDJbdtuLA())
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
			if (base.stScqZNiRHjjKdoCuFFXfYVzkfCN(gameObject))
			{
				return true;
			}
			if (KmMjlOsEwHJinoEgDQYqQKuWedzN != null)
			{
				return KmMjlOsEwHJinoEgDQYqQKuWedzN.gameObject == gameObject;
			}
			return false;
		}

		private void vqxSTcDCgjSzUXgmfwxFiZtMLXad()
		{
			if (_useDigitalAxisSimulation)
			{
				if (_axis.buttonValue)
				{
					aQmeLjKXodFGCavevbREvoPLylyMA();
				}
				else
				{
					dudPhrxVeKRovoaZEOSahdEQQPhw();
				}
			}
		}

		private void aQmeLjKXodFGCavevbREvoPLylyMA()
		{
			float num = ((_axis.value >= 0f) ? 1f : (-1f));
			float num2 = MathTools.Abs(_digitalAxisSensitivity);
			num *= num2 * Time.unscaledDeltaTime;
			num += veqIdftfxVwqrRMjNLnghBsjRQxj;
			num = MathTools.Clamp(num, -1f, 1f);
			UpSfgxhQcZwwRoQgynnYxVZMNTJsA(num, true);
		}

		private void dudPhrxVeKRovoaZEOSahdEQQPhw()
		{
			float num = _digitalAxisGravity;
			if (num == 0f)
			{
				return;
			}
			float num2 = veqIdftfxVwqrRMjNLnghBsjRQxj;
			if (num2 != 0f)
			{
				float num3 = num * Time.unscaledDeltaTime;
				float num4;
				if (MathTools.Abs(num3) >= MathTools.Abs(num2))
				{
					num4 = 0f;
				}
				else
				{
					float num5 = ((num2 > 0f) ? (-1f) : 1f);
					num4 = num2 + num5 * num3;
				}
				UpSfgxhQcZwwRoQgynnYxVZMNTJsA(num4, true);
			}
		}

		private void UpSfgxhQcZwwRoQgynnYxVZMNTJsA(float P_0, bool P_1)
		{
			morhpiFCxEcCMCmJZCAAfatUCuXoA = veqIdftfxVwqrRMjNLnghBsjRQxj;
			veqIdftfxVwqrRMjNLnghBsjRQxj = P_0;
			if (P_0 != morhpiFCxEcCMCmJZCAAfatUCuXoA)
			{
				hURaZktijlmPxmBSjDIeeNMcvCPLA(null);
			}
			if (P_1 && P_0 != morhpiFCxEcCMCmJZCAAfatUCuXoA)
			{
				_onAxisValueChanged.Invoke(P_0);
			}
		}

		private void mTxEkEjrFNdWkbsXIVNNcXAgRnbTd()
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

		private void CMbXxoUfbDCDsojCSDZgDkATsfrqA()
		{
			if (_buttonType == ButtonType.Standard)
			{
				_axis.SetRawValue(_axis.rawZero);
			}
		}

		private void mjzgvUOBUzKfqEPMBMKZfGXzperx()
		{
			_targetCustomControllerElement.ClearElementCaches();
			wnyAQmYBHgcEKhsVMsmmIScDnfFF();
			GFVSOWRrjQhhJVXfnWxbhsmeoKmK();
		}

		private void GFVSOWRrjQhhJVXfnWxbhsmeoKmK()
		{
			if (_manageRaycasting)
			{
				iuhdHlvlQrGTlTZEdGaHAkNJaFiW.xdyFfgAnFtvUqduRTeXdrPcIqmrJA(base.transform, CBtGhvdIhGqTGycRUEkBfApitlYh());
			}
		}

		private bool CBtGhvdIhGqTGycRUEkBfApitlYh()
		{
			if (KmMjlOsEwHJinoEgDQYqQKuWedzN != null && _useTouchRegionOnly)
			{
				return false;
			}
			return true;
		}

		private void rzvHAuDkXKFpRfoTndiyiPFWwOqlA(TouchRegion P_0)
		{
			if (!(P_0 == null))
			{
				iWUBFdJFBmBoOOxkjTnumbkFKkpZ(P_0);
				P_0.PointerDownEvent += AsXGvRHnhONYtrJzgciiyFycmnTib;
				P_0.PointerUpEvent += idyaEkqeKhxIkhSODJvLsaQzalMt;
				P_0.PointerEnterEvent += ZKocuczICVjwjcdkrZFnnalEFciCb;
				P_0.PointerExitEvent += pPgWgoKWgyhGRlTkkUvUhjZlyIrv;
			}
		}

		private void iWUBFdJFBmBoOOxkjTnumbkFKkpZ(TouchRegion P_0)
		{
			if (!(P_0 == null))
			{
				P_0.PointerDownEvent -= AsXGvRHnhONYtrJzgciiyFycmnTib;
				P_0.PointerUpEvent -= idyaEkqeKhxIkhSODJvLsaQzalMt;
				P_0.PointerEnterEvent -= ZKocuczICVjwjcdkrZFnnalEFciCb;
				P_0.PointerExitEvent -= pPgWgoKWgyhGRlTkkUvUhjZlyIrv;
			}
		}

		private void wnyAQmYBHgcEKhsVMsmmIScDnfFF()
		{
			if (!(KmMjlOsEwHJinoEgDQYqQKuWedzN == _touchRegion))
			{
				iWUBFdJFBmBoOOxkjTnumbkFKkpZ(KmMjlOsEwHJinoEgDQYqQKuWedzN);
				KmMjlOsEwHJinoEgDQYqQKuWedzN = _touchRegion;
				rzvHAuDkXKFpRfoTndiyiPFWwOqlA(KmMjlOsEwHJinoEgDQYqQKuWedzN);
			}
		}

		private void kMXbaumBuBzbBtAEMZusjhbrxdSs(Vector2 P_0, bool P_1, float P_2, aVYRyhLvDnDQRtsgCbXARDiQyrlL P_3)
		{
			RectTransform rectTransform = base.transform.parent as RectTransform;
			Vector2 vector = ItUXShgHgqMTvNPoaDBaDFBZYvtd.GQvayxmeWFjZrrOOTmjgUzFrfKKK(base.pEKfbfSDEyiQffkZVSLqoSXLfhMc, rectTransform, P_0);
			Vector2 pivot = base.XgRjXqPEGoTEAcTuxzCYeFqkaaly.pivot;
			Vector2 sizeDelta = base.XgRjXqPEGoTEAcTuxzCYeFqkaaly.sizeDelta;
			Vector3 localScale = base.XgRjXqPEGoTEAcTuxzCYeFqkaaly.localScale;
			vector += new Vector2((pivot.x - 0.5f) * sizeDelta.x * localScale.x, (pivot.y - 0.5f) * sizeDelta.y * localScale.y);
			POIDaPjFhCPKiTdaZKCZHBUIOZoIB(vector, PositionType.Local, P_1, P_2, P_3);
		}

		private void POIDaPjFhCPKiTdaZKCZHBUIOZoIB(Vector2 P_0, PositionType P_1, bool P_2, float P_3, aVYRyhLvDnDQRtsgCbXARDiQyrlL P_4)
		{
			if (LavfOtccbuiAxIFfAselJsTDBFVTA && P_2 && rnvFtgSEnnHFCAKsHMzPgjtsvPcG == P_4)
			{
				return;
			}
			if (LavfOtccbuiAxIFfAselJsTDBFVTA && AicESUopQddcJjSwuhiQPKcFksgk != null)
			{
				lvjegEHHcnrRhXQKhAEgtTHvhHoW();
				LavfOtccbuiAxIFfAselJsTDBFVTA = false;
				rnvFtgSEnnHFCAKsHMzPgjtsvPcG = aVYRyhLvDnDQRtsgCbXARDiQyrlL.None;
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
				AicESUopQddcJjSwuhiQPKcFksgk = gOHtoWUbdxIRmBSkanyjWVlzxezV(P_0, P_1, P_3, P_4);
				StartCoroutine(AicESUopQddcJjSwuhiQPKcFksgk);
				rnvFtgSEnnHFCAKsHMzPgjtsvPcG = P_4;
				zJmUhIMSOXJCOwCWarexWLLAaJCE = true;
				moveStartedDelegate(P_4);
			}
			else
			{
				moveStartedDelegate(P_4);
				yIlvcDQWdPMQJVKNWtiEfPJgdqsm(P_4, P_0, P_1);
			}
		}

		[IteratorStateMachine(typeof(eMKEIxFumelteMSgBIbPxXPjmvTgA))]
		private IEnumerator gOHtoWUbdxIRmBSkanyjWVlzxezV(Vector2 P_0, PositionType P_1, float P_2, aVYRyhLvDnDQRtsgCbXARDiQyrlL P_3)
		{
			return new eMKEIxFumelteMSgBIbPxXPjmvTgA(0)
			{
				WsLhEveRVYFLoBBCBUwJehRhOFMx = this,
				KMdAaGHbcHVmtfpitwHrdWNqXXkE = P_0,
				YFAcNYpXzKFwLtRlxYrnbcRuHvrm = P_1,
				DAwBTBGSdPlAWFcbwgPRogqUmnkN = P_2,
				rjTaJmdJwXvSGkCVHcSGSsUcYXCkA = P_3
			};
		}

		private void yIlvcDQWdPMQJVKNWtiEfPJgdqsm(aVYRyhLvDnDQRtsgCbXARDiQyrlL P_0, Vector2 P_1, PositionType P_2)
		{
			ItUXShgHgqMTvNPoaDBaDFBZYvtd.AVBaoIMoopyfpbSwkSCPNfEaGuXL(base.XgRjXqPEGoTEAcTuxzCYeFqkaaly, P_1, P_2);
			LavfOtccbuiAxIFfAselJsTDBFVTA = false;
			rnvFtgSEnnHFCAKsHMzPgjtsvPcG = aVYRyhLvDnDQRtsgCbXARDiQyrlL.None;
			switch (P_0)
			{
			case aVYRyhLvDnDQRtsgCbXARDiQyrlL.TowardHome:
				zJmUhIMSOXJCOwCWarexWLLAaJCE = false;
				break;
			case aVYRyhLvDnDQRtsgCbXARDiQyrlL.TowardTouch:
				zJmUhIMSOXJCOwCWarexWLLAaJCE = true;
				break;
			}
			lvjegEHHcnrRhXQKhAEgtTHvhHoW();
			moveEndedDelegate(P_0);
		}

		private void DvVfVhptWdFJwGhwEgojUQvqZfVEA(aVYRyhLvDnDQRtsgCbXARDiQyrlL P_0)
		{
			if (_manageRaycasting)
			{
				bool flag = false;
				bool flag2 = false;
				if (((_followTouchPosition && stayActiveOnSwipeOut) || (!_followTouchPosition && KmMjlOsEwHJinoEgDQYqQKuWedzN != null && !_useTouchRegionOnly && _moveToTouchPosition)) && _returnOnRelease && P_0 == aVYRyhLvDnDQRtsgCbXARDiQyrlL.TowardTouch)
				{
					flag = true;
					flag2 = false;
				}
				if (flag)
				{
					iuhdHlvlQrGTlTZEdGaHAkNJaFiW.xdyFfgAnFtvUqduRTeXdrPcIqmrJA(base.transform, flag2);
				}
			}
		}

		private void LseoHtzRyqSPWMMDQHEDANiMdGpfb(aVYRyhLvDnDQRtsgCbXARDiQyrlL P_0)
		{
			if (_manageRaycasting)
			{
				bool flag = false;
				bool flag2 = false;
				if (((_followTouchPosition && stayActiveOnSwipeOut) || (!_followTouchPosition && KmMjlOsEwHJinoEgDQYqQKuWedzN != null && !_useTouchRegionOnly && _moveToTouchPosition)) && _returnOnRelease && P_0 == aVYRyhLvDnDQRtsgCbXARDiQyrlL.TowardHome)
				{
					flag = true;
					flag2 = CBtGhvdIhGqTGycRUEkBfApitlYh();
				}
				if (flag)
				{
					iuhdHlvlQrGTlTZEdGaHAkNJaFiW.xdyFfgAnFtvUqduRTeXdrPcIqmrJA(base.transform, flag2);
				}
			}
		}

		private void xTpBoZCnIkZxobWXzPguQAmkLETaA(int P_0)
		{
			if (TouchInteractable.MiEJmWCYYHJMBAtKMSSlbJKQgXxfA(P_0))
			{
				kMXbaumBuBzbBtAEMZusjhbrxdSs(TouchInteractable.DTRRiAsrHICJIvfdMohYjrgotXjG(P_0), false, 0f, aVYRyhLvDnDQRtsgCbXARDiQyrlL.TowardTouch);
			}
		}

		private void lvjegEHHcnrRhXQKhAEgtTHvhHoW()
		{
			if (AicESUopQddcJjSwuhiQPKcFksgk != null)
			{
				try
				{
					StopCoroutine(AicESUopQddcJjSwuhiQPKcFksgk);
				}
				catch
				{
				}
				AicESUopQddcJjSwuhiQPKcFksgk = null;
			}
		}

		private void QXTrmSxQzUbwnSHcszrMeWzNYMie()
		{
			if (hasPointer && !TouchInteractable.MiEJmWCYYHJMBAtKMSSlbJKQgXxfA(effectivePointerId))
			{
				PointerEventData pointerEventData = gqFcbTFpGHcVbfeJYRtPDNeAFcdUA(effectivePointerId);
				if (pointerEventData != null && pointerEventData.pointerPress != null)
				{
					PeVMOscMLzmpklBzhbXNkJVPeZXV(pointerEventData);
				}
				else
				{
					DZDIuQSFrHzcdXNiFgHHpBKIkiaH();
				}
			}
		}

		private bool MYyypwTdepcxSPYAsjzvenIHuNYP()
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

		private void MzWNJMyGZcAMqGrfmcyfWFokoDQn()
		{
			aPtZESJuhKiNXuvEGsNqLcgZebmGA = -2147483648;
			FjrauICVvjmGCWKEOsHOrIIljKfGA = -2147483648;
		}

		private bool PBYgeMWJZDepoSmFVmiSrjtwcAF(int P_0)
		{
			if (P_0 == -2147483648)
			{
				return false;
			}
			if (aPtZESJuhKiNXuvEGsNqLcgZebmGA == -2147483648)
			{
				return false;
			}
			if (aPtZESJuhKiNXuvEGsNqLcgZebmGA == P_0)
			{
				return true;
			}
			if (TouchInteractable.mBmltzbovhkqiRskAgVuCNaKKMGn(P_0) && FjrauICVvjmGCWKEOsHOrIIljKfGA != -2147483648 && P_0 == FjrauICVvjmGCWKEOsHOrIIljKfGA)
			{
				return true;
			}
			return false;
		}

		private PointerEventData ApUBypMhLMiosFuaneZlBFbfGjYaB(int P_0, GameObject P_1)
		{
			PointerEventData pointerEventData = gqFcbTFpGHcVbfeJYRtPDNeAFcdUA(P_0);
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

		private PointerEventData DIilqkkvKEnTJFliiLwRUrykjBbS(int P_0)
		{
			PointerEventData pointerEventData = gqFcbTFpGHcVbfeJYRtPDNeAFcdUA(P_0);
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

		private void PeVMOscMLzmpklBzhbXNkJVPeZXV(PointerEventData P_0)
		{
			if (P_0 != null)
			{
				OnPointerUp(P_0);
				DIilqkkvKEnTJFliiLwRUrykjBbS(effectivePointerId);
			}
		}

		private PointerEventData gqFcbTFpGHcVbfeJYRtPDNeAFcdUA(int P_0)
		{
			if (P_0 == -2147483648)
			{
				return null;
			}
			if (aoAVxmCuBwuoJwIYhcofcRNrOSzdA == null)
			{
				aoAVxmCuBwuoJwIYhcofcRNrOSzdA = new Dictionary<int, PointerEventData>();
			}
			if (!aoAVxmCuBwuoJwIYhcofcRNrOSzdA.TryGetValue(P_0, out var value))
			{
				value = new PointerEventData(EventSystem.current);
				value.pointerId = P_0;
				aoAVxmCuBwuoJwIYhcofcRNrOSzdA.Add(P_0, value);
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

		private void EfohawdVDFGVJdhBHpZSfIfqMyGpA(PointerEventData P_0, oZSPUtSMLykXFyaOBGLpRDfTGQxD P_1)
		{
			if (!hasPointer || PBYgeMWJZDepoSmFVmiSrjtwcAF(P_0.pointerId))
			{
				if (IcqbeYEmGpfkqqAVukZKtDJbdtuLA() && IsInteractable())
				{
					aIwnreRVAjSGKyebXlsorJMoxGQp(P_0.pointerId, P_0.pressPosition, P_1);
				}
				base.OnPointerDown(P_0);
			}
		}

		private void NMstCYpOldRyUZnYUKDGUyQBUTWA(PointerEventData P_0, oZSPUtSMLykXFyaOBGLpRDfTGQxD P_1)
		{
			if ((!hasPointer || PBYgeMWJZDepoSmFVmiSrjtwcAF(P_0.pointerId)) && !TouchInteractable.MiEJmWCYYHJMBAtKMSSlbJKQgXxfA(effectivePointerId))
			{
				DZDIuQSFrHzcdXNiFgHHpBKIkiaH();
				base.OnPointerUp(P_0);
			}
		}

		private void tWUMSTWXxGfPKFHaUwyHDrfSIMHE(PointerEventData P_0, oZSPUtSMLykXFyaOBGLpRDfTGQxD P_1)
		{
			if (hasPointer && !PBYgeMWJZDepoSmFVmiSrjtwcAF(P_0.pointerId))
			{
				return;
			}
			bool flag = TouchInteractable.mBmltzbovhkqiRskAgVuCNaKKMGn(P_0.pointerId);
			bool flag2 = false;
			MouseButtonFlags mouseButtonFlags = P_1 switch
			{
				oZSPUtSMLykXFyaOBGLpRDfTGQxD.Local => base.allowedMouseButtons, 
				oZSPUtSMLykXFyaOBGLpRDfTGQxD.TouchRegion => _touchRegion.allowedMouseButtons, 
				_ => throw new NotImplementedException(), 
			};
			if (_activateOnSwipeIn && IcqbeYEmGpfkqqAVukZKtDJbdtuLA() && IsInteractable() && (!flag || TouchInteractable.aEOEYhFHczvhfJeopcXMsyPJOZaFA(mouseButtonFlags)) && !JlZaTEhesShUrbCppitCuzfsHjNTA)
			{
				if (flag)
				{
					if (TouchInteractable.hAWfiRgEUZhSRENpfwbhcWZRcXkUA(mouseButtonFlags, out var fjrauICVvjmGCWKEOsHOrIIljKfGA))
					{
						FjrauICVvjmGCWKEOsHOrIIljKfGA = fjrauICVvjmGCWKEOsHOrIIljKfGA;
					}
					else
					{
						FjrauICVvjmGCWKEOsHOrIIljKfGA = P_0.pointerId;
					}
				}
				flag2 = true;
			}
			base.OnPointerEnter(P_0);
			if (flag2)
			{
				GameObject gameObject = P_1 switch
				{
					oZSPUtSMLykXFyaOBGLpRDfTGQxD.Local => base.gameObject, 
					oZSPUtSMLykXFyaOBGLpRDfTGQxD.TouchRegion => KmMjlOsEwHJinoEgDQYqQKuWedzN.gameObject, 
					_ => throw new NotImplementedException(), 
				};
				PointerEventData pointerEventData = ApUBypMhLMiosFuaneZlBFbfGjYaB((FjrauICVvjmGCWKEOsHOrIIljKfGA != -2147483648) ? FjrauICVvjmGCWKEOsHOrIIljKfGA : P_0.pointerId, gameObject);
				if (pointerEventData != null)
				{
					EfohawdVDFGVJdhBHpZSfIfqMyGpA(pointerEventData, P_1);
				}
			}
			tgYlkgwcMrhcNjjuPouHldoNtXbf = true;
		}

		private void qdNqcHEByzLPgDJtnuCbgnBbRNHy(PointerEventData P_0, oZSPUtSMLykXFyaOBGLpRDfTGQxD P_1)
		{
			if (hasPointer && !PBYgeMWJZDepoSmFVmiSrjtwcAF(P_0.pointerId))
			{
				base.OnPointerExit(P_0);
				return;
			}
			if (!stayActiveOnSwipeOut && JlZaTEhesShUrbCppitCuzfsHjNTA)
			{
				DZDIuQSFrHzcdXNiFgHHpBKIkiaH();
			}
			base.OnPointerExit(P_0);
			tgYlkgwcMrhcNjjuPouHldoNtXbf = false;
		}

		private void aIwnreRVAjSGKyebXlsorJMoxGQp(int P_0, Vector2 P_1, oZSPUtSMLykXFyaOBGLpRDfTGQxD P_2)
		{
			aPtZESJuhKiNXuvEGsNqLcgZebmGA = P_0;
			JlZaTEhesShUrbCppitCuzfsHjNTA = true;
			if (_followTouchPosition)
			{
				xTpBoZCnIkZxobWXzPguQAmkLETaA(P_0);
			}
			else if (P_2 == oZSPUtSMLykXFyaOBGLpRDfTGQxD.TouchRegion && _moveToTouchPosition)
			{
				kMXbaumBuBzbBtAEMZusjhbrxdSs(P_1, _animateOnMoveToTouch, _moveToTouchSpeed, aVYRyhLvDnDQRtsgCbXARDiQyrlL.TowardTouch);
			}
			mTxEkEjrFNdWkbsXIVNNcXAgRnbTd();
		}

		private void DZDIuQSFrHzcdXNiFgHHpBKIkiaH()
		{
			MzWNJMyGZcAMqGrfmcyfWFokoDQn();
			JlZaTEhesShUrbCppitCuzfsHjNTA = false;
			if ((_followTouchPosition || _moveToTouchPosition) && _returnOnRelease && zJmUhIMSOXJCOwCWarexWLLAaJCE)
			{
				ReturnToDefaultPosition();
			}
			CMbXxoUfbDCDsojCSDZgDkATsfrqA();
		}

		internal override void OnPointerDown(PointerEventData eventData)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(eventData.pointerId, base.allowedMouseButtons, EventTriggerType.PointerDown) && (!(KmMjlOsEwHJinoEgDQYqQKuWedzN != null) || !_useTouchRegionOnly))
			{
				EfohawdVDFGVJdhBHpZSfIfqMyGpA(eventData, oZSPUtSMLykXFyaOBGLpRDfTGQxD.Local);
			}
		}

		internal override void OnPointerUp(PointerEventData eventData)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(eventData.pointerId, base.allowedMouseButtons, EventTriggerType.PointerUp) && (!(KmMjlOsEwHJinoEgDQYqQKuWedzN != null) || !_useTouchRegionOnly))
			{
				NMstCYpOldRyUZnYUKDGUyQBUTWA(eventData, oZSPUtSMLykXFyaOBGLpRDfTGQxD.Local);
			}
		}

		internal override void OnPointerEnter(PointerEventData eventData)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(eventData.pointerId, base.allowedMouseButtons, EventTriggerType.PointerEnter) && (!(KmMjlOsEwHJinoEgDQYqQKuWedzN != null) || !_useTouchRegionOnly))
			{
				tWUMSTWXxGfPKFHaUwyHDrfSIMHE(eventData, oZSPUtSMLykXFyaOBGLpRDfTGQxD.Local);
			}
		}

		internal override void OnPointerExit(PointerEventData eventData)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(eventData.pointerId, base.allowedMouseButtons, EventTriggerType.PointerExit) && (!(KmMjlOsEwHJinoEgDQYqQKuWedzN != null) || !_useTouchRegionOnly))
			{
				qdNqcHEByzLPgDJtnuCbgnBbRNHy(eventData, oZSPUtSMLykXFyaOBGLpRDfTGQxD.Local);
			}
		}

		private void AsXGvRHnhONYtrJzgciiyFycmnTib(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerDown))
			{
				EfohawdVDFGVJdhBHpZSfIfqMyGpA(P_0, oZSPUtSMLykXFyaOBGLpRDfTGQxD.TouchRegion);
			}
		}

		private void idyaEkqeKhxIkhSODJvLsaQzalMt(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerUp))
			{
				NMstCYpOldRyUZnYUKDGUyQBUTWA(P_0, oZSPUtSMLykXFyaOBGLpRDfTGQxD.TouchRegion);
			}
		}

		private void ZKocuczICVjwjcdkrZFnnalEFciCb(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerEnter))
			{
				tWUMSTWXxGfPKFHaUwyHDrfSIMHE(P_0, oZSPUtSMLykXFyaOBGLpRDfTGQxD.TouchRegion);
			}
		}

		private void pPgWgoKWgyhGRlTkkUvUhjZlyIrv(PointerEventData P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && TouchInteractable.MsaNPvMHfyDplAeqNkzTmIDcJhWQ(P_0.pointerId, _touchRegion.allowedMouseButtons, EventTriggerType.PointerExit))
			{
				qdNqcHEByzLPgDJtnuCbgnBbRNHy(P_0, oZSPUtSMLykXFyaOBGLpRDfTGQxD.TouchRegion);
			}
		}

		private void ghfquyOxSLqEuLUZibQmrISpwxLk(float P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA && !_useDigitalAxisSimulation)
			{
				hURaZktijlmPxmBSjDIeeNMcvCPLA(null);
				_onAxisValueChanged.Invoke(P_0);
			}
		}

		private void mroKnxkYNFzSDxsukvJsGBzsnCMF(bool P_0)
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				hURaZktijlmPxmBSjDIeeNMcvCPLA(null);
				_onButtonValueChanged.Invoke(P_0);
			}
		}

		private void BAfbKTCGSVqeKFaTvcQBeIJbyDZxA()
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				hURaZktijlmPxmBSjDIeeNMcvCPLA(null);
				_onButtonDown.Invoke();
			}
		}

		private void xtadVNVmkrkhOAnIyjQdWUMsRmxQ()
		{
			if (base.jZvmQixnOgbmYeDtNVyCjgZHOfdeA)
			{
				hURaZktijlmPxmBSjDIeeNMcvCPLA(null);
				_onButtonUp.Invoke();
			}
		}
	}
}
