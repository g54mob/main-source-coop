using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.RumModule.Scripts.Views
{
	public abstract class RumBuffsViewBase : ViewBehaviour
	{
		public abstract Transform ItemsContainer { get; }

		public abstract TemporalRumBuffItemViewBase TemporalItemPrefab { get; }

		public abstract PermanentRumBuffItemViewBase PermanentItemPrefab { get; }

		public abstract void SetVisible(bool visible);
	}
}
