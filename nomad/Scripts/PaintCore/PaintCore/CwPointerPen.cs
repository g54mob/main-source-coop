using System;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	[RequireComponent(typeof(CwHitPointers))]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwPointerPen")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Pointer Pen")]
	public class CwPointerPen : CwPointer
	{
		[SerializeField]
		private bool preview;

		[SerializeField]
		private float offset;

		private readonly int PREVIEW_FINGER_INDEX = -1;

		private readonly int PAINT_FINGER_INDEX = 1;

		[NonSerialized]
		private bool oldHeld;

		public bool Preview
		{
			get
			{
				return preview;
			}
			set
			{
				preview = value;
			}
		}

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

		protected virtual void Update()
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (GetPenExists())
			{
				flag = GetPenHeld();
				flag3 = flag || oldHeld;
				flag2 = preview && !flag3;
				CwInputManager.Finger finger;
				if (flag2)
				{
					GetFinger(PREVIEW_FINGER_INDEX, GetPenPosition(), GetPenPressure(), set: true, out finger);
					cachedHitPointers.HandleFingerUpdate(finger, down: false, up: false);
				}
				if (flag3)
				{
					bool finger2 = GetFinger(PAINT_FINGER_INDEX, GetPenPosition(), GetPenPressure(), set: true, out finger);
					cachedHitPointers.HandleFingerUpdate(finger, finger2, !flag);
				}
			}
			if (!flag2)
			{
				TryNullFinger(PREVIEW_FINGER_INDEX);
			}
			if (!flag3)
			{
				TryNullFinger(PAINT_FINGER_INDEX);
			}
			oldHeld = flag;
		}

		public static bool GetPenExists()
		{
			return false;
		}

		public static Vector2 GetPenPosition()
		{
			return Vector2.zero;
		}

		public static float GetPenPressure()
		{
			return 0f;
		}

		public static bool GetPenHeld()
		{
			return false;
		}
	}
}
