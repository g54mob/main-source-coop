using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	[RequireComponent(typeof(CwHitPointers))]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwPointerMouse")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Pointer Mouse")]
	public class CwPointerMouse : CwPointer
	{
		[SerializeField]
		private bool preview;

		[SerializeField]
		private List<KeyCode> keys;

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

		public List<KeyCode> Keys
		{
			get
			{
				if (keys == null)
				{
					keys = new List<KeyCode>();
				}
				return keys;
			}
		}

		public bool TryAddKey(KeyCode key)
		{
			if (!Keys.Contains(key))
			{
				keys.Add(key);
				return true;
			}
			return false;
		}

		public bool GetKeyHeld()
		{
			if (keys != null)
			{
				foreach (KeyCode key in keys)
				{
					if (CwInput.GetKeyIsHeld(key))
					{
						return true;
					}
				}
			}
			return false;
		}

		protected virtual void Update()
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (CwInput.GetMouseExists())
			{
				flag = GetKeyHeld();
				flag3 = flag || oldHeld;
				flag2 = preview && !flag3;
				CwInputManager.Finger finger;
				if (flag2)
				{
					GetFinger(PREVIEW_FINGER_INDEX, CwInput.GetMousePosition(), 1f, set: true, out finger);
					cachedHitPointers.HandleFingerUpdate(finger, down: false, up: false);
				}
				if (flag3)
				{
					bool finger2 = GetFinger(PAINT_FINGER_INDEX, CwInput.GetMousePosition(), 1f, set: true, out finger);
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
	}
}
