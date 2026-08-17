using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;
using UnityEngine.Events;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwColorCounterText")]
	[AddComponentMenu("CW/Paint Core/CW Color Counter Text")]
	public class CwColorCounterText : MonoBehaviour
	{
		[Serializable]
		public class StringEvent : UnityEvent<string>
		{
		}

		[SerializeField]
		private List<CwColorCounter> counters;

		[SerializeField]
		private CwColor color;

		[SerializeField]
		private bool inverse;

		[SerializeField]
		private int decimalPlaces;

		[Multiline]
		[SerializeField]
		private string format = "{PERCENT}";

		[SerializeField]
		private StringEvent onString;

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
			List<CwColorCounter> list = ((counters.Count > 0) ? counters : null);
			long total = CwColorCounter.GetTotal(list);
			long num = CwColorCounter.GetCount(color, list);
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
