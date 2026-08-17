using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;
using UnityEngine.Events;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwChangeCounterText")]
	[AddComponentMenu("CW/Paint Core/CW Change Counter Text")]
	public class CwChangeCounterText : MonoBehaviour
	{
		[Serializable]
		public class StringEvent : UnityEvent<string>
		{
		}

		[SerializeField]
		private List<CwChangeCounter> counters;

		[SerializeField]
		private bool inverse;

		[SerializeField]
		private int decimalPlaces;

		[Multiline]
		[SerializeField]
		private string format = "{PERCENT}";

		[SerializeField]
		private StringEvent onString;

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
			List<CwChangeCounter> obj = ((counters.Count > 0) ? counters : null);
			long total = CwChangeCounter.GetTotal(obj);
			long num = CwChangeCounter.GetCount(obj);
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
