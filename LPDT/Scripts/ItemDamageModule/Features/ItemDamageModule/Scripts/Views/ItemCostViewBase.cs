using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.ItemDamageModule.Scripts.Views
{
	public abstract class ItemCostViewBase : ViewBehaviour
	{
		public float FollowSpeed = 20f;

		public abstract void UpdateCostDisplay(float cost, bool isWithAnimation);

		public abstract void SwitchCostDisplay(bool show);

		public abstract void SetLocalPosition(Vector3 localPoint);

		public abstract void ClearCurrency();
	}
}
