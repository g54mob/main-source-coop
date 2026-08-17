using System.Collections.Generic;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired.ComponentControls.Effects
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[DisallowMultipleComponent]
	[AddComponentMenu("Rewired/Touch Controls/Effects/Touch Joystick Radial Indicator")]
	public sealed class TouchJoystickRadialIndicator : MonoBehaviour, IRegistrar<TouchJoystickAngleIndicator>
	{
		[Tooltip("If enabled, the indicators will be scaled based on the size of the RectTransform.")]
		public bool _scale = true;

		[Tooltip("If enabled, the aspect ratio will be determined from the Sprite's texture.")]
		public bool _preserveSpriteAspectRatio;

		[Tooltip("The scale ratio of the indicators to the current RectTransform's height. A ratio of 0.1 means the indicator will be 0.1 times the size of the RectTransform's height. This is useful if you need to be able to scale the transform and have the indicators also scale with it.")]
		[Range(0.01f, 1f)]
		public float _scaleRatio = 0.1f;

		[Tooltip("The horizontal component of the desired aspect ratio of the indicator.")]
		[Range(0.01f, 10f)]
		public float _aspectRatioX = 1f;

		[Tooltip("The vertical component of the desired aspect ratio of the indicator.")]
		[Range(0.01f, 10f)]
		public float _aspectRatioY = 1f;

		[Tooltip("Offsets the indicator position up by this proportion of its height. 1.0 = 1 unit high offset.")]
		public float _offset;

		private static readonly Vector2 PlgEYWMvFbQNWVQDbbdfuenTeSDO = new Vector2(0.5f, 0.5f);

		private RectTransform jXFdrNmfvkekgdsqNdBQzRPWUAfMA;

		private List<TouchJoystickAngleIndicator> yIcfSksJRyNaFjkAUATAdbRDcwkD = new List<TouchJoystickAngleIndicator>(8);

		public bool scale
		{
			get
			{
				return _scale;
			}
			set
			{
				if (_scale != value)
				{
					_scale = value;
					SdujKQyDTYqiaEhjaPNSEMPUlknP();
				}
			}
		}

		public bool preserveSpriteAspectRatio
		{
			get
			{
				return _preserveSpriteAspectRatio;
			}
			set
			{
				if (_preserveSpriteAspectRatio != value)
				{
					_preserveSpriteAspectRatio = value;
					SdujKQyDTYqiaEhjaPNSEMPUlknP();
				}
			}
		}

		public float scaleRatio
		{
			get
			{
				return _scaleRatio;
			}
			set
			{
				value = MathTools.Clamp(value, 0.01f, 1f);
				if (_scaleRatio != value)
				{
					_scaleRatio = value;
					SdujKQyDTYqiaEhjaPNSEMPUlknP();
				}
			}
		}

		public float aspectRatioX
		{
			get
			{
				return _aspectRatioX;
			}
			set
			{
				value = MathTools.Clamp(value, 0.01f, 10f);
				if (_aspectRatioX != value)
				{
					_aspectRatioX = value;
					SdujKQyDTYqiaEhjaPNSEMPUlknP();
				}
			}
		}

		public float aspectRatioY
		{
			get
			{
				return _aspectRatioY;
			}
			set
			{
				value = MathTools.Clamp(value, 0.01f, 10f);
				if (_aspectRatioY != value)
				{
					_aspectRatioY = value;
					SdujKQyDTYqiaEhjaPNSEMPUlknP();
				}
			}
		}

		public float offset
		{
			get
			{
				return _offset;
			}
			set
			{
				if (_offset != value)
				{
					_offset = value;
					SdujKQyDTYqiaEhjaPNSEMPUlknP();
				}
			}
		}

		private RectTransform xbavWCsUQwaRHThpFBTBLnAXvjDQ => jXFdrNmfvkekgdsqNdBQzRPWUAfMA ?? (jXFdrNmfvkekgdsqNdBQzRPWUAfMA = GetComponent<RectTransform>());

		void IRegistrar<TouchJoystickAngleIndicator>.Register(TouchJoystickAngleIndicator registrant)
		{
			if (!(registrant == null) && ListTools.AddIfUnique(yIcfSksJRyNaFjkAUATAdbRDcwkD, registrant) && base.enabled)
			{
				uqoRoYBHzpCFyGYnAriXzOfFAKwH(registrant);
			}
		}

		void IRegistrar<TouchJoystickAngleIndicator>.Deregister(TouchJoystickAngleIndicator registrant)
		{
			if (!(registrant == null))
			{
				yIcfSksJRyNaFjkAUATAdbRDcwkD.Remove(registrant);
			}
		}

		[CustomObfuscation(rename = false)]
		private void Update()
		{
			UmcgNljuPUghPJvoPPGBbCRiMkBBB();
		}

		[CustomObfuscation(rename = false)]
		private void OnValidate()
		{
			if (base.enabled)
			{
				SGXQoVEjJLFbGqooahWVhIfZwBfvA();
				UmcgNljuPUghPJvoPPGBbCRiMkBBB();
			}
		}

		[CustomObfuscation(rename = false)]
		private void OnEnable()
		{
			UmcgNljuPUghPJvoPPGBbCRiMkBBB();
		}

		[CustomObfuscation(rename = false)]
		private void OnDestroy()
		{
			yIcfSksJRyNaFjkAUATAdbRDcwkD.Clear();
		}

		private void UmcgNljuPUghPJvoPPGBbCRiMkBBB()
		{
			for (int num = yIcfSksJRyNaFjkAUATAdbRDcwkD.Count - 1; num >= 0; num--)
			{
				TouchJoystickAngleIndicator touchJoystickAngleIndicator = yIcfSksJRyNaFjkAUATAdbRDcwkD[num];
				if (touchJoystickAngleIndicator.QScKEvMphHAWUkntnsFfKBUwfsMj.IsNullOrDestroyed())
				{
					yIcfSksJRyNaFjkAUATAdbRDcwkD.RemoveAt(num);
				}
				else
				{
					uqoRoYBHzpCFyGYnAriXzOfFAKwH(touchJoystickAngleIndicator);
				}
			}
		}

		private void uqoRoYBHzpCFyGYnAriXzOfFAKwH(TouchJoystickAngleIndicator P_0)
		{
			if (!UnityTools.IsActiveAndEnabled(P_0.QScKEvMphHAWUkntnsFfKBUwfsMj))
			{
				return;
			}
			RectTransform rectTransform = P_0.wayHUgyyGWEYbKcGrguEwQoeoscE;
			if (rectTransform == xbavWCsUQwaRHThpFBTBLnAXvjDQ || rectTransform == null)
			{
				return;
			}
			Rect rect = xbavWCsUQwaRHThpFBTBLnAXvjDQ.rect;
			if (_scale)
			{
				float num = (num = _aspectRatioX / _aspectRatioY);
				if (_preserveSpriteAspectRatio && P_0.RGdWgCoUeFKrkjmMIFgNnHpXPkSf(out var vector))
				{
					num = vector.x / vector.y;
				}
				Vector2 sizeDelta = new Vector2(rect.height * _scaleRatio * num, rect.height * _scaleRatio);
				rectTransform.sizeDelta = sizeDelta;
			}
			float num2 = (rect.height / 2f / rectTransform.rect.height - 1f) * -1f;
			if (rectTransform.anchorMin != PlgEYWMvFbQNWVQDbbdfuenTeSDO)
			{
				rectTransform.anchorMin = PlgEYWMvFbQNWVQDbbdfuenTeSDO;
			}
			if (rectTransform.anchorMax != PlgEYWMvFbQNWVQDbbdfuenTeSDO)
			{
				rectTransform.anchorMax = PlgEYWMvFbQNWVQDbbdfuenTeSDO;
			}
			Vector2 pivot = rectTransform.pivot;
			pivot.x = 0.5f;
			pivot.y = num2 + _offset * -1f;
			rectTransform.pivot = pivot;
		}

		private void SdujKQyDTYqiaEhjaPNSEMPUlknP()
		{
			UmcgNljuPUghPJvoPPGBbCRiMkBBB();
		}

		private void SGXQoVEjJLFbGqooahWVhIfZwBfvA()
		{
			Transform transform = base.transform;
			yIcfSksJRyNaFjkAUATAdbRDcwkD.Clear();
			int childCount = transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				TouchJoystickAngleIndicator component = transform.GetChild(i).GetComponent<TouchJoystickAngleIndicator>();
				if (component != null)
				{
					yIcfSksJRyNaFjkAUATAdbRDcwkD.Add(component);
				}
			}
		}
	}
}
