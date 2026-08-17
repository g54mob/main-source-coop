using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;
using UnityEngine.Events;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwChannelCounterText")]
	[AddComponentMenu("CW/Paint Core/CW Channel Counter Text")]
	public class CwChannelCounterText : MonoBehaviour
	{
		[Serializable]
		public class StringEvent : UnityEvent<string>
		{
		}

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

		[SerializeField]
		private int decimalPlaces;

		[Multiline]
		[SerializeField]
		private string format = "{PERCENT}";

		[SerializeField]
		private StringEvent onString;

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

		public int DecimalPlaces
		{
			get
			{
				return decimalPlaces;
			}
			set
			{
				decimalPlaces = value;
			}
		}

		public string Format
		{
			get
			{
				return format;
			}
			set
			{
				format = value;
			}
		}

		public StringEvent OnString
		{
			get
			{
				if (onString == null)
				{
					onString = new StringEvent();
				}
				return onString;
			}
		}

		protected virtual void Update()
		{
			List<CwChannelCounter> list = ((counters.Count > 0) ? counters : null);
			long total = CwChannelCounter.GetTotal(list);
			long num = 0L;
			switch (channel)
			{
			case ChannelType.Red:
				num = CwChannelCounter.GetCountR(list);
				break;
			case ChannelType.Green:
				num = CwChannelCounter.GetCountG(list);
				break;
			case ChannelType.Blue:
				num = CwChannelCounter.GetCountB(list);
				break;
			case ChannelType.Alpha:
				num = CwChannelCounter.GetCountA(list);
				break;
			}
			if (inverse)
			{
				num = total - num;
			}
			string text = format;
			float num2 = CwCommon.RatioToPercentage(CwHelper.Divide(num, total), decimalPlaces);
			text = text.Replace("{TOTAL}", total.ToString());
			text = text.Replace("{COUNT}", num.ToString());
			text = text.Replace("{PERCENT}", num2.ToString());
			if (onString != null)
			{
				onString.Invoke(text);
			}
		}
	}
}
