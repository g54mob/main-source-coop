using System.Collections.Generic;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired.ComponentControls.Effects
{
	[AddComponentMenu("Rewired/Touch Joystick Radial Indicator")]
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public sealed class TouchJoystickRadialIndicator : MonoBehaviour, IRegistrar<TouchJoystickAngleIndicator>
	{
		[Tooltip("If enabled, the indicators will be scaled based on the size of the RectTransform.")]
		public bool _scale = true;

		[Tooltip("If enabled, the aspect ratio will be determined from the Sprite's texture.")]
		public bool _preserveSpriteAspectRatio;

		[Range(0.01f, 1f)]
		[Tooltip("The scale ratio of the indicators to the current RectTransform's height. A ratio of 0.1 means the indicator will be 0.1 times the size of the RectTransform's height. This is useful if you need to be able to scale the transform and have the indicators also scale with it.")]
		public float _scaleRatio = 0.1f;

		[Range(0.01f, 10f)]
		[Tooltip("The horizontal component of the desired aspect ratio of the indicator.")]
		public float _aspectRatioX = 1f;

		[Range(0.01f, 10f)]
		[Tooltip("The vertical component of the desired aspect ratio of the indicator.")]
		public float _aspectRatioY = 1f;

		[Tooltip("Offsets the indicator position up by this proportion of its height. 1.0 = 1 unit high offset.")]
		public float _offset;

		private static readonly Vector2 OnJDFwaKnemQlOFCZcAnSvELCgXdA = new Vector2(0.5f, 0.5f);

		private RectTransform QDYYpoeyByWqXkmYyeqJcfBaSimc;

		private List<TouchJoystickAngleIndicator> QWZEhyhoBmPNRfpHipsHYaaZolwo = new List<TouchJoystickAngleIndicator>(8);

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
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
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
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
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
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
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
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
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
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
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
					FyKKzlnIjsJaSimYwuqKSwNJHIHx();
				}
			}
		}

		private RectTransform dHBtGVwmKSUQYlNEBqhwMLJxhsUgA => QDYYpoeyByWqXkmYyeqJcfBaSimc ?? (QDYYpoeyByWqXkmYyeqJcfBaSimc = GetComponent<RectTransform>());

		void IRegistrar<TouchJoystickAngleIndicator>.Register(TouchJoystickAngleIndicator registrant)
		{
			if (!(registrant == null) && ListTools.AddIfUnique(QWZEhyhoBmPNRfpHipsHYaaZolwo, registrant) && base.enabled)
			{
				pZzjNtjoEFhrCKbZMkXpacmYGzEhA(registrant);
			}
		}

		void IRegistrar<TouchJoystickAngleIndicator>.Deregister(TouchJoystickAngleIndicator registrant)
		{
			if (!(registrant == null))
			{
				QWZEhyhoBmPNRfpHipsHYaaZolwo.Remove(registrant);
			}
		}

		[CustomObfuscation(rename = false)]
		private void Update()
		{
			bGfIPFdtEnYcjjOWbUoyqbgJbTnS();
		}

		[CustomObfuscation(rename = false)]
		private void OnValidate()
		{
			if (base.enabled)
			{
				CmxxlkWPkryMeJQQeGDoblonfvJab();
				bGfIPFdtEnYcjjOWbUoyqbgJbTnS();
			}
		}

		[CustomObfuscation(rename = false)]
		private void OnEnable()
		{
			bGfIPFdtEnYcjjOWbUoyqbgJbTnS();
		}

		[CustomObfuscation(rename = false)]
		private void OnDestroy()
		{
			QWZEhyhoBmPNRfpHipsHYaaZolwo.Clear();
		}

		private void bGfIPFdtEnYcjjOWbUoyqbgJbTnS()
		{
			for (int num = QWZEhyhoBmPNRfpHipsHYaaZolwo.Count - 1; num >= 0; num--)
			{
				TouchJoystickAngleIndicator touchJoystickAngleIndicator = QWZEhyhoBmPNRfpHipsHYaaZolwo[num];
				if (touchJoystickAngleIndicator.AnFrappGeuvMAKfZPbOgepDZqzOu.IsNullOrDestroyed())
				{
					QWZEhyhoBmPNRfpHipsHYaaZolwo.RemoveAt(num);
				}
				else
				{
					pZzjNtjoEFhrCKbZMkXpacmYGzEhA(touchJoystickAngleIndicator);
				}
			}
		}

		private void pZzjNtjoEFhrCKbZMkXpacmYGzEhA(TouchJoystickAngleIndicator P_0)
		{
			if (!UnityTools.IsActiveAndEnabled(P_0.AnFrappGeuvMAKfZPbOgepDZqzOu))
			{
				return;
			}
			RectTransform rectTransform = P_0.dHBtGVwmKSUQYlNEBqhwMLJxhsUgA;
			if (rectTransform == dHBtGVwmKSUQYlNEBqhwMLJxhsUgA || rectTransform == null)
			{
				return;
			}
			Rect rect = dHBtGVwmKSUQYlNEBqhwMLJxhsUgA.rect;
			if (_scale)
			{
				float num = (num = _aspectRatioX / _aspectRatioY);
				if (_preserveSpriteAspectRatio && P_0.qGAjaUBiEqcaBXNmBhvHvwJEdCCg(out var vector))
				{
					num = vector.x / vector.y;
				}
				Vector2 sizeDelta = new Vector2(rect.height * _scaleRatio * num, rect.height * _scaleRatio);
				rectTransform.sizeDelta = sizeDelta;
			}
			float num2 = (rect.height / 2f / rectTransform.rect.height - 1f) * -1f;
			if (rectTransform.anchorMin != OnJDFwaKnemQlOFCZcAnSvELCgXdA)
			{
				rectTransform.anchorMin = OnJDFwaKnemQlOFCZcAnSvELCgXdA;
			}
			if (rectTransform.anchorMax != OnJDFwaKnemQlOFCZcAnSvELCgXdA)
			{
				rectTransform.anchorMax = OnJDFwaKnemQlOFCZcAnSvELCgXdA;
			}
			Vector2 pivot = rectTransform.pivot;
			pivot.x = 0.5f;
			pivot.y = num2 + _offset * -1f;
			rectTransform.pivot = pivot;
		}

		private void FyKKzlnIjsJaSimYwuqKSwNJHIHx()
		{
			bGfIPFdtEnYcjjOWbUoyqbgJbTnS();
		}

		private void CmxxlkWPkryMeJQQeGDoblonfvJab()
		{
			Transform transform = base.transform;
			QWZEhyhoBmPNRfpHipsHYaaZolwo.Clear();
			int childCount = transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				TouchJoystickAngleIndicator component = transform.GetChild(i).GetComponent<TouchJoystickAngleIndicator>();
				if (component != null)
				{
					QWZEhyhoBmPNRfpHipsHYaaZolwo.Add(component);
				}
			}
		}
	}
}
