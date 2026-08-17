using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PaintCore
{
	[RequireComponent(typeof(Image))]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwChangeCounterFill")]
	[AddComponentMenu("CW/Paint Core/CW Change Counter Fill")]
	public class CwChangeCounterFill : MonoBehaviour
	{
		[SerializeField]
		private List<CwChangeCounter> counters;

		[SerializeField]
		private bool inverse;

		[NonSerialized]
		private Image cachedImage;

		public List<CwChangeCounter> Counters
		{
			get
			{
				if (counters == null)
				{
					counters = new List<CwChangeCounter>();
				}
				return counters;
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
			float num = CwChangeCounter.GetRatio((counters.Count > 0) ? counters : null);
			if (inverse)
			{
				num = 1f - num;
			}
			cachedImage.fillAmount = Mathf.Clamp01(num);
		}
	}
}
