using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwChangeCounterEvent")]
	[AddComponentMenu("CW/Paint Core/CW Change Counter Event")]
	public class CwChangeCounterEvent : MonoBehaviour
	{
		[SerializeField]
		private List<CwChangeCounter> counters;

		[SerializeField]
		private Vector2 range = new Vector2(0f, 1f);

		[SerializeField]
		private bool inside;

		[SerializeField]
		private UnityEvent onInside;

		[SerializeField]
		private UnityEvent onOutside;

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

		public float Ratio => CwChangeCounter.GetRatio((counters != null && counters.Count > 0) ? counters : null);

		public bool AllCountersReady => CwChangeCounter.GetReady((counters != null && counters.Count > 0) ? counters : null);

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
