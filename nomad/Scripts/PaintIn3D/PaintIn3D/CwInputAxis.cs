using System;
using UnityEngine;
using UnityEngine.Events;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwInputAxis")]
	[AddComponentMenu("CW/Paint in 3D/CW Input Axis")]
	public class CwInputAxis : MonoBehaviour
	{
		[Serializable]
		public class BoolEvent : UnityEvent<bool>
		{
		}

		[SerializeField]
		private string axisName;

		[SerializeField]
		private int axisIndex;

		[SerializeField]
		private BoolEvent onValue;

		public string AxisName
		{
			get
			{
				return axisName;
			}
			set
			{
				axisName = value;
			}
		}

		public int AxisIndex
		{
			get
			{
				return axisIndex;
			}
			set
			{
				axisIndex = value;
			}
		}

		public BoolEvent OnValue
		{
			get
			{
				if (onValue == null)
				{
					onValue = new BoolEvent();
				}
				return onValue;
			}
		}

		protected virtual void Update()
		{
			if (onValue != null)
			{
				onValue.Invoke(Input.GetAxisRaw(axisName) > 0.1f);
			}
		}
	}
}
