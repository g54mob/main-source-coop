using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PaintCore
{
	[RequireComponent(typeof(Image))]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwColorCounterFill")]
	[AddComponentMenu("CW/Paint Core/CW Color Counter Fill")]
	public class CwColorCounterFill : MonoBehaviour
	{
		[SerializeField]
		private List<CwColorCounter> counters;

		[SerializeField]
		private CwColor color;

		[SerializeField]
		private bool inverse;

		[NonSerialized]
		private Image cachedImage;

		public List<CwColorCounter> Counters
		{
			get
			{
				if (counters == null)
				{
					counters = new List<CwColorCounter>();
				}
				return counters;
			}
		}

		public CwColor Color
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

		public bool Inverse
		{
			get
			{
				return inverse;
			}
			set
			{
				inverse = value;
			}
		}

		protected virtual void OnEnable()
		{
			cachedImage = GetComponent<Image>();
		}

		protected virtual void Update()
		{
			List<CwColorCounter> list = ((counters.Count > 0) ? counters : null);
			float num = CwColorCounter.GetRatio(color, list);
			if (inverse)
			{
				num = 1f - num;
			}
			cachedImage.fillAmount = Mathf.Clamp01(num);
		}
	}
}
