using CW.Common;
using UnityEngine;

namespace PaintCore
{
	public abstract class CwHitPointers : MonoBehaviour
	{
		[SerializeField]
		private LayerMask guiLayers = 32;

		public LayerMask GuiLayers
		{
			get
			{
				return guiLayers;
			}
			set
			{
				guiLayers = value;
			}
		}

		public virtual void BreakFinger(CwInputManager.Finger finger)
		{
		}

		public virtual void HandleFingerUpdate(CwInputManager.Finger finger, bool down, bool up)
		{
			if (up)
			{
				HandleFingerUp(finger);
			}
			CwPaintableManager.MarkActivelyPainting();
		}

		protected virtual void HandleFingerUp(CwInputManager.Finger finger)
		{
		}
	}
}
