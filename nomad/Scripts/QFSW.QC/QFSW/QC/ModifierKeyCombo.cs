using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace QFSW.QC
{
	[Serializable]
	public struct ModifierKeyCombo
	{
		[FormerlySerializedAs("key")]
		public KeyCode Key;

		[FormerlySerializedAs("ctrl")]
		public bool Ctrl;

		[FormerlySerializedAs("alt")]
		public bool Alt;

		[FormerlySerializedAs("shift")]
		public bool Shift;

		public bool ModifiersActive
		{
			get
			{
				bool num = !Ctrl ^ (InputHelper.GetKey(KeyCode.LeftControl) || InputHelper.GetKey(KeyCode.RightControl) || InputHelper.GetKey(KeyCode.LeftMeta) || InputHelper.GetKey(KeyCode.RightMeta));
				bool flag = !Alt ^ (InputHelper.GetKey(KeyCode.LeftAlt) || InputHelper.GetKey(KeyCode.RightAlt));
				bool flag2 = !Shift ^ (InputHelper.GetKey(KeyCode.LeftShift) || InputHelper.GetKey(KeyCode.RightShift));
				return num && flag && flag2;
			}
		}

		public bool IsHeld()
		{
			if (ModifiersActive)
			{
				return InputHelper.GetKey(Key);
			}
			return false;
		}

		public bool IsPressed()
		{
			if (ModifiersActive)
			{
				return InputHelper.GetKeyDown(Key);
			}
			return false;
		}

		public static implicit operator ModifierKeyCombo(KeyCode key)
		{
			return new ModifierKeyCombo
			{
				Key = key
			};
		}
	}
}
