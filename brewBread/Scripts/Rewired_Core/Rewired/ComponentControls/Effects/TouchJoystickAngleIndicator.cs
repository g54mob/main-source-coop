using Rewired.UI;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.ComponentControls.Effects
{
	[AddComponentMenu("Rewired/Touch Joystick Angle Indicator")]
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Image))]
	public sealed class TouchJoystickAngleIndicator : MonoBehaviour, TouchJoystick.IStickPositionChangedHandler, IVisibilityChangedHandler
	{
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Toggles visibility.")]
		private bool _visible = true;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("If enabled, the target angle will be determined by the transform's Local Rotation Z. Otherwise, the activation angle must be manually set.")]
		private bool _targetAngleFromRotation = true;

		[Range(0f, -360f)]
		[Tooltip("The joystick angle at which this object should be considered fully active.\n0 = up with negative values increase rotating clockwise. Example: -45 degrees = up-right.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private float _targetAngle;

		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("If enabled, the color will fade in and out based on the current joystick value.")]
		private bool _fadeWithValue = true;

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("If enabled, the color will fade in and out based on the current joystick angle. As the angle approaches the Target Angle, the color will become more intense.")]
		private bool _fadeWithAngle = true;

		[Tooltip("The angle of rotation away from the Target Angle where the color fully fades out. If Fade with Angle is enabled, this is used to determine when the color will fully fade out when the joystick angle rotates away from the the Target Angle. This should be set to 1/2 of the complete rotation arc. Example: A value of 45 degrees would make the color fully fade out when the joystick angle is 45 degrees away from the Target Angle on either side, giving a complete arc of 90 degrees.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Range(0f, 360f)]
		private float _fadeRange = 45f;

		[Tooltip("The color when fully active.")]
		[CustomObfuscation(rename = false)]
		[SerializeField]
		private Color _activeColor = new Color(1f, 1f, 1f, 1f);

		[CustomObfuscation(rename = false)]
		[SerializeField]
		[Tooltip("The color when not active.")]
		private Color _normalColor = new Color(1f, 1f, 1f, 0.3f);

		private Image xSSmRGYJjzzTnLfidiIkYrQAcdKEA;

		private RectTransform QDYYpoeyByWqXkmYyeqJcfBaSimc;

		private Vector2 kvOHmnbCowVtSYAttNsuKXfrBeJl;

		private bool WKLCOIhVKGKJSYhEOFPFfPADIEsLA;

		private IRegistrar<TouchJoystickAngleIndicator> riiiBNuTETGdwiEWHZGOTWgEFAMW;

		public bool visible
		{
			get
			{
				return _visible;
			}
			set
			{
				if (visible != value)
				{
					nnXqcpdOcMbRXjEWYqRtWFgsBQGCb(value, false);
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool targetAngleFromRotation
		{
			get
			{
				return _targetAngleFromRotation;
			}
			set
			{
				if (_targetAngleFromRotation != value)
				{
					_targetAngleFromRotation = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public float targetAngle
		{
			get
			{
				if (!_targetAngleFromRotation)
				{
					return _targetAngle;
				}
				return base.transform.localEulerAngles.z;
			}
			set
			{
				if (_targetAngle != value)
				{
					_targetAngle = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool fadeWithValue
		{
			get
			{
				return _fadeWithValue;
			}
			set
			{
				if (_fadeWithValue != value)
				{
					_fadeWithValue = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public bool fadeWithAngle
		{
			get
			{
				return _fadeWithAngle;
			}
			set
			{
				if (_fadeWithAngle != value)
				{
					_fadeWithAngle = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public float fadeRange
		{
			get
			{
				return _fadeRange;
			}
			set
			{
				if (_fadeRange != value)
				{
					_fadeRange = value;
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		public Color activeColor
		{
			get
			{
				return _activeColor;
			}
			set
			{
				_activeColor = value;
				FyKKzlnIjsJaSimYwuqKSwNJHIHx();
			}
		}

		public Color normalColor
		{
			get
			{
				return _normalColor;
			}
			set
			{
				_normalColor = value;
				FyKKzlnIjsJaSimYwuqKSwNJHIHx();
			}
		}

		internal Image AnFrappGeuvMAKfZPbOgepDZqzOu => xSSmRGYJjzzTnLfidiIkYrQAcdKEA ?? (xSSmRGYJjzzTnLfidiIkYrQAcdKEA = GetComponent<Image>());

		internal Sprite NQaCBBkZNqrlljgZNmxaaJmozkHxA
		{
			get
			{
				if (AnFrappGeuvMAKfZPbOgepDZqzOu == null)
				{
					return null;
				}
				if (xSSmRGYJjzzTnLfidiIkYrQAcdKEA.overrideSprite != null)
				{
					return xSSmRGYJjzzTnLfidiIkYrQAcdKEA.overrideSprite;
				}
				return xSSmRGYJjzzTnLfidiIkYrQAcdKEA.sprite;
			}
		}

		internal RectTransform dHBtGVwmKSUQYlNEBqhwMLJxhsUgA => QDYYpoeyByWqXkmYyeqJcfBaSimc ?? (QDYYpoeyByWqXkmYyeqJcfBaSimc = GetComponent<RectTransform>());

		[CustomObfuscation(rename = false)]
		private TouchJoystickAngleIndicator()
		{
		}

		internal bool qGAjaUBiEqcaBXNmBhvHvwJEdCCg(out Vector2 P_0)
		{
			P_0 = Vector2.zero;
			if (AnFrappGeuvMAKfZPbOgepDZqzOu == null)
			{
				return false;
			}
			Sprite sprite = xSSmRGYJjzzTnLfidiIkYrQAcdKEA.overrideSprite ?? xSSmRGYJjzzTnLfidiIkYrQAcdKEA.sprite;
			if (sprite == null)
			{
				return false;
			}
			Rect textureRect = sprite.textureRect;
			P_0.x = textureRect.width;
			P_0.y = textureRect.height;
			return true;
		}

		[CustomObfuscation(rename = false)]
		private void Awake()
		{
			OnTouchJoystickStickPositionChanged(Vector2.zero);
			ZOHvREfiEGuSoZkpvCoiaVVliwbJ();
		}

		[CustomObfuscation(rename = false)]
		private void OnEnable()
		{
			if (!Application.isPlaying)
			{
				ZOHvREfiEGuSoZkpvCoiaVVliwbJ();
				TNsFHmCGSRxKxAgWYaxXYcDGUIxvA();
			}
			pZzjNtjoEFhrCKbZMkXpacmYGzEhA(kvOHmnbCowVtSYAttNsuKXfrBeJl);
		}

		[CustomObfuscation(rename = false)]
		private void OnDisable()
		{
			gGrGtoyINBNDSswRwWaVZCwMHTpC();
		}

		[CustomObfuscation(rename = false)]
		private void OnValidate()
		{
			ZESmGDtKgyfxEWszxkXiDLpKTdRJ();
			pZzjNtjoEFhrCKbZMkXpacmYGzEhA(kvOHmnbCowVtSYAttNsuKXfrBeJl);
		}

		[CustomObfuscation(rename = false)]
		private void OnTransformParentChanged()
		{
			TNsFHmCGSRxKxAgWYaxXYcDGUIxvA();
		}

		private void nnXqcpdOcMbRXjEWYqRtWFgsBQGCb(bool P_0, bool P_1)
		{
			if (_visible != P_0 || P_1)
			{
				_visible = P_0;
				if (!P_0)
				{
					Color targetColor = _normalColor;
					targetColor.a = 0f;
					AnFrappGeuvMAKfZPbOgepDZqzOu.CrossFadeColor(targetColor, 0f, ignoreTimeScale: true, useAlpha: true);
				}
				else
				{
					pZzjNtjoEFhrCKbZMkXpacmYGzEhA(kvOHmnbCowVtSYAttNsuKXfrBeJl);
				}
			}
		}

		private void pZzjNtjoEFhrCKbZMkXpacmYGzEhA(Vector2 P_0)
		{
			if (!_visible)
			{
				Color targetColor = _normalColor;
				targetColor.a = 0f;
				AnFrappGeuvMAKfZPbOgepDZqzOu.CrossFadeColor(targetColor, 0f, ignoreTimeScale: true, useAlpha: true);
			}
			else if (!MathTools.ApproximatelyZero(P_0.sqrMagnitude))
			{
				float magnitude = P_0.magnitude;
				float num = Vector2.Angle(Vector2.up, P_0);
				float target = (_targetAngleFromRotation ? base.transform.localEulerAngles.z : _targetAngle) * -1f;
				float num2 = ((P_0.x < 0f) ? (360f - num) : num);
				Color targetColor2;
				if (_fadeWithAngle || _fadeWithValue)
				{
					float num3 = 1f;
					if (_fadeWithValue)
					{
						num3 *= magnitude;
					}
					if (_fadeWithAngle)
					{
						float num4 = Mathf.Abs(MathTools.DeltaAngle(num2, target));
						float num5 = ((_fadeRange != 0f) ? MathTools.Clamp01(1f - num4 / _fadeRange) : 1f);
						num3 *= num5;
					}
					targetColor2 = Color.Lerp(_normalColor, _activeColor, num3);
				}
				else
				{
					targetColor2 = (MathTools.AngleIsNear(num2, target, _fadeRange) ? _activeColor : _normalColor);
				}
				AnFrappGeuvMAKfZPbOgepDZqzOu.CrossFadeColor(targetColor2, 0f, ignoreTimeScale: true, useAlpha: true);
			}
			else
			{
				AnFrappGeuvMAKfZPbOgepDZqzOu.CrossFadeColor(_normalColor, 0f, ignoreTimeScale: true, useAlpha: true);
			}
		}

		private void ZOHvREfiEGuSoZkpvCoiaVVliwbJ()
		{
			WKLCOIhVKGKJSYhEOFPFfPADIEsLA = _visible;
		}

		private void ZESmGDtKgyfxEWszxkXiDLpKTdRJ()
		{
			if (WKLCOIhVKGKJSYhEOFPFfPADIEsLA != _visible)
			{
				WKLCOIhVKGKJSYhEOFPFfPADIEsLA = _visible;
				nnXqcpdOcMbRXjEWYqRtWFgsBQGCb(_visible, true);
			}
		}

		private void FyKKzlnIjsJaSimYwuqKSwNJHIHx()
		{
		}

		private void TNsFHmCGSRxKxAgWYaxXYcDGUIxvA()
		{
			gGrGtoyINBNDSswRwWaVZCwMHTpC();
			IRegistrar<TouchJoystickAngleIndicator> componentInSelfOrParents = UnityTools.GetComponentInSelfOrParents<IRegistrar<TouchJoystickAngleIndicator>>(base.transform);
			if (!componentInSelfOrParents.IsNullOrDestroyed())
			{
				componentInSelfOrParents.Register(this);
				riiiBNuTETGdwiEWHZGOTWgEFAMW = componentInSelfOrParents;
			}
		}

		private void gGrGtoyINBNDSswRwWaVZCwMHTpC()
		{
			if (riiiBNuTETGdwiEWHZGOTWgEFAMW.IsNullOrDestroyed())
			{
				if (riiiBNuTETGdwiEWHZGOTWgEFAMW != null)
				{
					riiiBNuTETGdwiEWHZGOTWgEFAMW = null;
				}
			}
			else
			{
				riiiBNuTETGdwiEWHZGOTWgEFAMW.Deregister(this);
				riiiBNuTETGdwiEWHZGOTWgEFAMW = null;
			}
		}

		public void OnVisibilityChanged(bool state)
		{
			nnXqcpdOcMbRXjEWYqRtWFgsBQGCb(state, false);
		}

		public void OnTouchJoystickStickPositionChanged(Vector2 value)
		{
			if (!(this == null))
			{
				kvOHmnbCowVtSYAttNsuKXfrBeJl = value;
				if (UnityTools.IsActiveAndEnabled(this) && _visible)
				{
					pZzjNtjoEFhrCKbZMkXpacmYGzEhA(value);
				}
			}
		}

		void TouchJoystick.IStickPositionChangedHandler.OnStickPositionChanged(Vector2 value)
		{
			OnTouchJoystickStickPositionChanged(value);
		}
	}
}
