using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.RumModule.Scripts.Views
{
	public abstract class PermanentRumBuffItemViewBase : ViewBehaviour
	{
		public abstract void SetRumIcon(Sprite icon);

		public abstract void SetStackCount(int count);
	}
}
