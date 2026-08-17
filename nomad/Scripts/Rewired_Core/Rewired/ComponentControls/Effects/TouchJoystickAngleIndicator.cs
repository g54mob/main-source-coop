using Rewired.UI;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.ComponentControls.Effects
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Image))]
	[DisallowMultipleComponent]
	[AddComponentMenu("Rewired/Touch Controls/Effects/Touch Joystick Angle Indicator")]
	public sealed class TouchJoystickAngleIndicator : MonoBehaviour, IVisibilityChangedHandler, TouchJoystick.IStickPositionChangedHandler
	{
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Tooltip("Toggles visibility.")]
		private bool _visible = true;

		[Tooltip("If enabled, the target angle will be determined by the transform's Local Rotation Z. Otherwise, the activation angle must be manually set.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _targetAngleFromRotation = true;

		[Tooltip("The joystick angle at which this object should be considered fully active.\n0 = up with negative values increase rotating clockwise. Example: -45 degrees = up-right.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Range(0f, -360f)]
		private float _targetAngle;

		[Tooltip("If enabled, the color will fade in and out based on the current joystick value.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _fadeWithValue = true;

		[Tooltip("If enabled, the color will fade in and out based on the current joystick angle. As the angle approaches the Target Angle, the color will become more intense.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private bool _fadeWithAngle = true;

		[Tooltip("The angle of rotation away from the Target Angle where the color fully fades out. If Fade with Angle is enabled, this is used to determine when the color will fully fade out when the joystick angle rotates away from the the Target Angle. This should be set to 1/2 of the complete rotation arc. Example: A value of 45 degrees would make the color fully fade out when the joystick angle is 45 degrees away from the Target Angle on either side, giving a complete arc of 90 degrees.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		[Range(0f, 360f)]
		private float _fadeRange = 45f;

		[Tooltip("The color when fully active.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private Color _activeColor = new Color(1f, 1f, 1f, 1f);

		[Tooltip("The color when not active.")]
		[SerializeField]
		[CustomObfuscation(rename = false)]
		private Color _normalColor = new Color(1f, 1f, 1f, 0.3f);

		private Image qojjRCUoovmwNZBpvvZblWmfavFw;

		private RectTransform bgNWdRmJRymGwLgBTjRLAnDcgTSRA;

		private Vector2 xNqgBtrDdYxthkEwGWWuUXTfTfgG;

		private bool MyCQTTZDYazfwtrGdFUxWJOyGDXQ;

		private IRegistrar<TouchJoystickAngleIndicator> hoBgLZKtaVLSFqJywetZHpTphVsfA;

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
					XsLZMWTGKNuFpRSpjZCrvcZZPSOk(value, false);
					BLAMmDueEQrclqqWmAzySZwrFNYc();
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
					BLAMmDueEQrclqqWmAzySZwrFNYc();
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
					BLAMmDueEQrclqqWmAzySZwrFNYc();
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
					BLAMmDueEQrclqqWmAzySZwrFNYc();
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
					BLAMmDueEQrclqqWmAzySZwrFNYc();
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
					BLAMmDueEQrclqqWmAzySZwrFNYc();
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
				BLAMmDueEQrclqqWmAzySZwrFNYc();
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
				BLAMmDueEQrclqqWmAzySZwrFNYc();
			}
		}

		internal Image QScKEvMphHAWUkntnsFfKBUwfsMj => qojjRCUoovmwNZBpvvZblWmfavFw ?? (qojjRCUoovmwNZBpvvZblWmfavFw = GetComponent<Image>());

		internal Sprite YwAAJLdwsFINnFzbVTPgPHGtLURJA
		{
			get
			{
				if (QScKEvMphHAWUkntnsFfKBUwfsMj == null)
				{
					return null;
				}
				if (qojjRCUoovmwNZBpvvZblWmfavFw.overrideSprite != null)
				{
					return qojjRCUoovmwNZBpvvZblWmfavFw.overrideSprite;
				}
				return qojjRCUoovmwNZBpvvZblWmfavFw.sprite;
			}
		}

		internal RectTransform wayHUgyyGWEYbKcGrguEwQoeoscE => bgNWdRmJRymGwLgBTjRLAnDcgTSRA ?? (bgNWdRmJRymGwLgBTjRLAnDcgTSRA = GetComponent<RectTransform>());

		[CustomObfuscation(rename = false)]
		private TouchJoystickAngleIndicator()
		{
		}

		internal bool RGdWgCoUeFKrkjmMIFgNnHpXPkSf(out Vector2 P_0)
		{
			P_0 = Vector2.zero;
			if (QScKEvMphHAWUkntnsFfKBUwfsMj == null)
			{
				return false;
			}
			Sprite sprite = qojjRCUoovmwNZBpvvZblWmfavFw.overrideSprite ?? qojjRCUoovmwNZBpvvZblWmfavFw.sprite;
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
			aCThfgRufZgMZqUueolDYhXWiMaF();
		}

		[CustomObfuscation(rename = false)]
		private void OnEnable()
		{
			if (!Application.isPlaying)
			{
				aCThfgRufZgMZqUueolDYhXWiMaF();
				OMAMLKoFkyXcPjVKDdSvllSQDyEv();
			}
			OXyWrORavIiqhMwBuolDRBDeENGH(xNqgBtrDdYxthkEwGWWuUXTfTfgG);
		}

		[CustomObfuscation(rename = false)]
		private void OnDisable()
		{
			zPofNAvdTkdzZopWoKvEFqjCXeEy();
		}

		[CustomObfuscation(rename = false)]
		private void OnValidate()
		{
			zyhBphvfYQBdwXAuSRHoBIrjymYL();
			OXyWrORavIiqhMwBuolDRBDeENGH(xNqgBtrDdYxthkEwGWWuUXTfTfgG);
		}

		[CustomObfuscation(rename = false)]
		private void OnTransformParentChanged()
		{
			OMAMLKoFkyXcPjVKDdSvllSQDyEv();
		}

		private void XsLZMWTGKNuFpRSpjZCrvcZZPSOk(bool P_0, bool P_1)
		{
			if (_visible != P_0 || P_1)
			{
				_visible = P_0;
				if (!P_0)
				{
					Color targetColor = _normalColor;
					targetColor.a = 0f;
					QScKEvMphHAWUkntnsFfKBUwfsMj.CrossFadeColor(targetColor, 0f, ignoreTimeScale: true, useAlpha: true);
				}
				else
				{
					OXyWrORavIiqhMwBuolDRBDeENGH(xNqgBtrDdYxthkEwGWWuUXTfTfgG);
				}
			}
		}

		private void OXyWrORavIiqhMwBuolDRBDeENGH(Vector2 P_0)
		{
			if (!_visible)
			{
				Color targetColor = _normalColor;
				targetColor.a = 0f;
				QScKEvMphHAWUkntnsFfKBUwfsMj.CrossFadeColor(targetColor, 0f, ignoreTimeScale: true, useAlpha: true);
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
				QScKEvMphHAWUkntnsFfKBUwfsMj.CrossFadeColor(targetColor2, 0f, ignoreTimeScale: true, useAlpha: true);
			}
			else
			{
				QScKEvMphHAWUkntnsFfKBUwfsMj.CrossFadeColor(_normalColor, 0f, ignoreTimeScale: true, useAlpha: true);
			}
		}

		private void aCThfgRufZgMZqUueolDYhXWiMaF()
		{
			MyCQTTZDYazfwtrGdFUxWJOyGDXQ = _visible;
		}

		private void zyhBphvfYQBdwXAuSRHoBIrjymYL()
		{
			if (MyCQTTZDYazfwtrGdFUxWJOyGDXQ != _visible)
			{
				MyCQTTZDYazfwtrGdFUxWJOyGDXQ = _visible;
				XsLZMWTGKNuFpRSpjZCrvcZZPSOk(_visible, true);
			}
		}

		private void BLAMmDueEQrclqqWmAzySZwrFNYc()
		{
		}

		private void OMAMLKoFkyXcPjVKDdSvllSQDyEv()
		{
			zPofNAvdTkdzZopWoKvEFqjCXeEy();
			IRegistrar<TouchJoystickAngleIndicator> componentInSelfOrParents = UnityTools.GetComponentInSelfOrParents<IRegistrar<TouchJoystickAngleIndicator>>(base.transform);
			if (!componentInSelfOrParents.IsNullOrDestroyed())
			{
				componentInSelfOrParents.Register(this);
				hoBgLZKtaVLSFqJywetZHpTphVsfA = componentInSelfOrParents;
			}
		}

		private void zPofNAvdTkdzZopWoKvEFqjCXeEy()
		{
			if (hoBgLZKtaVLSFqJywetZHpTphVsfA.IsNullOrDestroyed())
			{
				if (hoBgLZKtaVLSFqJywetZHpTphVsfA != null)
				{
					hoBgLZKtaVLSFqJywetZHpTphVsfA = null;
				}
			}
			else
			{
				hoBgLZKtaVLSFqJywetZHpTphVsfA.Deregister(this);
				hoBgLZKtaVLSFqJywetZHpTphVsfA = null;
			}
		}

		public void OnVisibilityChanged(bool state)
		{
			XsLZMWTGKNuFpRSpjZCrvcZZPSOk(state, false);
		}

		void IVisibilityChangedHandler.OnVisibilityChanged(bool state)
		{
			//ILSpy generated this explicit interface implementation from .override directive in OnVisibilityChanged
			this.OnVisibilityChanged(state);
		}

		public void OnTouchJoystickStickPositionChanged(Vector2 value)
		{
			if (!(this == null))
			{
				xNqgBtrDdYxthkEwGWWuUXTfTfgG = value;
				if (UnityTools.IsActiveAndEnabled(this) && _visible)
				{
					OXyWrORavIiqhMwBuolDRBDeENGH(value);
				}
			}
		}

		void TouchJoystick.IStickPositionChangedHandler.OnStickPositionChanged(Vector2 value)
		{
			OnTouchJoystickStickPositionChanged(value);
		}
	}
}
