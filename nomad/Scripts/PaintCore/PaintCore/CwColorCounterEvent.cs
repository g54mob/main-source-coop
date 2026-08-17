using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwColorCounterEvent")]
	[AddComponentMenu("CW/Paint Core/CW Color Counter Event")]
	public class CwColorCounterEvent : MonoBehaviour
	{
		[SerializeField]
		private List<CwColorCounter> counters;

		[SerializeField]
		private CwColor color;

		[SerializeField]
		private Vector2 range = new Vector2(0f, 1f);

		[SerializeField]
		private bool inside;

		[SerializeField]
		private UnityEvent onInside;

		[SerializeField]
		private UnityEvent onOutside;

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

		public Vector2 Range
		{
			get
			{
				return range;
			}
			set
			{
				range = value;
			}
		}

		public bool Inside
		{
			get
			{
				return inside;
			}
			set
			{
				inside = value;
			}
		}

		public UnityEvent OnInside
		{
			get
			{
				if (onInside == null)
				{
					onInside = new UnityEvent();
				}
				return onInside;
			}
		}

		public UnityEvent OnOutside
		{
			get
			{
				if (onOutside == null)
				{
					onOutside = new UnityEvent();
				}
				return onOutside;
			}
		}

		public float Ratio
		{
			get
			{
				List<CwColorCounter> list = ((counters != null && counters.Count > 0) ? counters : null);
				return CwColorCounter.GetRatio(color, list);
			}
		}

		public bool AllCountersReady => CwColorCounter.GetReady((counters != null && counters.Count > 0) ? counters : null);

		protected virtual void Update()
		{
			if (AllCountersReady)
			{
				UpdateInside(Ratio);
			}
		}

		private void UpdateInside(float ratio)
		{
			bool flag = false;
			flag = ((range.y != 1f) ? (ratio >= range.x && ratio < range.y) : (ratio >= range.x && ratio <= range.y));
			if (inside && !flag)
			{
				inside = false;
				if (onOutside != null)
				{
					onOutside.Invoke();
				}
			}
			else if (!inside && flag)
			{
				inside = true;
				if (onInside != null)
				{
					onInside.Invoke();
				}
			}
		}
	}
}
