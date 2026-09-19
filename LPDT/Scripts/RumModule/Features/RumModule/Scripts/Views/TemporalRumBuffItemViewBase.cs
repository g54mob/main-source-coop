using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.RumModule.Scripts.Views
{
	public abstract class TemporalRumBuffItemViewBase : ViewBehaviour
	{
		public abstract void SetRumIcon(Sprite icon);

		public abstract void SetStackCount(int count);

		public abstract void SetRemainingSeconds(int seconds);
	}
}
