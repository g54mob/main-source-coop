using CW.Common;
using UnityEngine;

namespace PaintCore
{
	[RequireComponent(typeof(CwHitPointers))]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwPointerTouch")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Pointer Touch")]
	public class CwPointerTouch : CwPointer
	{
		[SerializeField]
		private float offset;

		[SerializeField]
		private int maxFingers = -1;

		public float Offset
		{
			get
			{
				return offset;
			}
			set
			{
				offset = value;
			}
		}

		public int MaxFingers
		{
			get
			{
				return maxFingers;
			}
			set
			{
				maxFingers = value;
			}
		}

		protected virtual void Update()
		{
			for (int i = 0; i < CwInput.GetTouchCount(); i++)
			{
				CwInput.GetTouch(i, out var id, out var position, out var pressure, out var set);
				position.y += offset * CwInputManager.ScaleFactor;
				CwInputManager.Finger finger2;
				bool finger = GetFinger(id, position, pressure, set, out finger2);
				if (finger && !finger2.StartedOverGui && maxFingers >= 0 && GetFingerCount(ignoreStartedOverGui: true) > maxFingers)
				{
					TryNullFinger(id);
					continue;
				}
				cachedHitPointers.HandleFingerUpdate(finger2, finger, !set);
				if (!set)
				{
					TryNullFinger(id);
				}
			}
		}
	}
}
