using System;
using UnityEngine;
using UnityEngine.Events;

namespace PaintCore
{
	[RequireComponent(typeof(CwReadColor))]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwReadColorEvent")]
	[AddComponentMenu("CW/Paint Core/CW Read Color Event")]
	public class CwReadColorEvent : MonoBehaviour
	{
		[Serializable]
		public class ColorEvent : UnityEvent<Color>
		{
		}

		[SerializeField]
		private Color color = Color.white;

		[Range(0f, 1f)]
		[SerializeField]
		private float threshold = 0.1f;

		[SerializeField]
		private ColorEvent onColor;

		[NonSerialized]
		private CwReadColor cachedReadColor;

		public Color Color
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
			}
		}

		public float Threshold
		{
			get
			{
				return threshold;
			}
			set
			{
				threshold = value;
			}
		}

		public ColorEvent OnColor
		{
			get
			{
				if (onColor == null)
				{
					onColor = new ColorEvent();
				}
				return onColor;
			}
		}

		protected virtual void OnEnable()
		{
			cachedReadColor = GetComponent<CwReadColor>();
			cachedReadColor.OnColor.AddListener(HandleColor);
		}

		protected virtual void OnDisable()
		{
			cachedReadColor.OnColor.RemoveListener(HandleColor);
		}

		private void HandleColor(Color read)
		{
			Color32 color = this.color;
			Color32 color2 = read;
			int num = (int)(threshold * 255f);
			if (0 + Math.Abs(color.r - color2.r) + Math.Abs(color.g - color2.g) + Math.Abs(color.b - color2.b) + Math.Abs(color.a - color2.a) <= num && onColor != null)
			{
				onColor.Invoke(this.color);
			}
		}
	}
}
