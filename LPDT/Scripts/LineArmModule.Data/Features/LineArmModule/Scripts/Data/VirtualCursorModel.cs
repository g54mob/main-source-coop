using System;
using UnityEngine;

namespace Features.LineArmModule.Scripts.Data
{
	public class VirtualCursorModel
	{
		public bool IsVirtualCursorActive { get; private set; }

		public Vector2 ScreenPosition { get; private set; }

		public event Action<bool, Vector2> OnStateChanged;

		public void SetState(bool isVirtualCursorActive, Vector2 screenPosition)
		{
			if (IsVirtualCursorActive != isVirtualCursorActive || !((ScreenPosition - screenPosition).sqrMagnitude < 0.0001f))
			{
				IsVirtualCursorActive = isVirtualCursorActive;
				ScreenPosition = screenPosition;
				this.OnStateChanged?.Invoke(IsVirtualCursorActive, ScreenPosition);
			}
		}
	}
}
