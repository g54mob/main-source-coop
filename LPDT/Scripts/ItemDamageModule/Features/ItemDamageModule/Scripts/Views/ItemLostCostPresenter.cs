using Features.CameraModelModule;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.ItemDamageModule.Scripts.Views
{
	public class ItemLostCostPresenter : PresenterBehaviour<ItemLostCostViewBase>
	{
		private readonly CameraModel _cameraModel;

		private readonly ItemCostLossNetworkEvent _itemCostLossNetworkEvent;

		public ItemLostCostPresenter(CameraModel cameraModel, ItemCostLossNetworkEvent itemCostLossNetworkEvent)
		{
			_cameraModel = cameraModel;
			_itemCostLossNetworkEvent = itemCostLossNetworkEvent;
		}

		protected override void OnViewSet()
		{
			_itemCostLossNetworkEvent.OnNetworkEventSend += OnItemCostLoss;
			foreach (LostCostAnimation lostCostAnimation in base.View.LostCostAnimations)
			{
				lostCostAnimation.Deactivate();
			}
		}

		protected override void OnDisposed()
		{
			_itemCostLossNetworkEvent.OnNetworkEventSend -= OnItemCostLoss;
		}

		private void OnItemCostLoss(ItemCostLossNetworkEvent eventData)
		{
			Camera cameraObject = _cameraModel.CameraObject;
			if (cameraObject != null)
			{
				base.View.PlayLostCostAnimation(eventData.CostLoss, eventData.CostPosition, cameraObject);
			}
		}
	}
}
