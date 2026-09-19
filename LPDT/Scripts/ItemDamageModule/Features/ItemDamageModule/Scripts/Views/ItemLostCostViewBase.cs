using System.Collections.Generic;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.ItemDamageModule.Scripts.Views
{
	public abstract class ItemLostCostViewBase : ViewBehaviour
	{
		public abstract List<LostCostAnimation> LostCostAnimations { get; }

		public abstract void PlayLostCostAnimation(float lostCost, Vector3 hitPosition, Camera currentCamera);
	}
}
