using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwChannelCounterEvent")]
	[AddComponentMenu("CW/Paint Core/CW Channel Counter Event")]
	public class CwChannelCounterEvent : MonoBehaviour
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
		private ChannelType channel = ChannelType.Alpha;

		[SerializeField]
		private Vector2 range = new Vector2(0f, 1f);

		[SerializeField]
		private bool inside;

		[SerializeField]
		private UnityEvent onInside;

		[SerializeField]
		private UnityEvent onOutside;

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
				List<CwChannelCounter> list = ((counters != null && counters.Count > 0) ? counters : null);
				return channel switch
				{
					ChannelType.Red => CwChannelCounter.GetRatioR(list), 
					ChannelType.Green => CwChannelCounter.GetRatioG(list), 
					ChannelType.Blue => CwChannelCounter.GetRatioB(list), 
					ChannelType.Alpha => CwChannelCounter.GetRatioA(list), 
					_ => 0f, 
				};
			}
		}

		public bool AllCountersReady => CwChannelCounter.GetReady((counters != null && counters.Count > 0) ? counters : null);

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
