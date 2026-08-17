using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PaintCore
{
	[RequireComponent(typeof(Image))]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwChannelCounterFill")]
	[AddComponentMenu("CW/Paint Core/CW Channel Counter Fill")]
	public class CwChannelCounterFill : MonoBehaviour
	{
		public enum ChannelType
		{
			Red = 0,
			Green = 1,
			Blue = 2,
			Alpha = 3
		}

		[SerializeField]
		private List<CwChannelCounter> counters;

		[SerializeField]
		private ChannelType channel;

		[SerializeField]
		private bool inverse;

		[NonSerialized]
		private Image cachedImage;

		public List<CwChannelCounter> Counters
		{
			get
			{
				if (counters == null)
				{
					counters = new List<CwChannelCounter>();
				}
				return counters;
			}
		}

		public ChannelType Channel
		{
			get
			{
				return channel;
			}
			set
			{
				channel = value;
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
			List<CwChannelCounter> list = ((counters.Count > 0) ? counters : null);
			float num = 0f;
			switch (channel)
			{
			case ChannelType.Red:
				num = CwChannelCounter.GetRatioR(list);
				break;
			case ChannelType.Green:
				num = CwChannelCounter.GetRatioG(list);
				break;
			case ChannelType.Blue:
				num = CwChannelCounter.GetRatioB(list);
				break;
			case ChannelType.Alpha:
				num = CwChannelCounter.GetRatioA(list);
				break;
			}
			if (inverse)
			{
				num = 1f - num;
			}
			cachedImage.fillAmount = Mathf.Clamp01(num);
		}
	}
}
