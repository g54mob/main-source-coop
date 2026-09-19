using System.Linq;
using Features.CameraModelModule;
using Features.GameUpdaterModule;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.ItemDamageModule.Scripts.Views
{
	[PublicAPI]
	public class ItemCostPresenter : PresenterBehaviour<ItemCostViewBase>
	{
		private readonly ItemDamageDisplayModel _itemDamageDisplayModel;

		private readonly IGameUpdater _gameUpdater;

		private readonly CameraModel _cameraModel;

		private Vector3 _pricePosition = Vector3.zero;

		public ItemCostPresenter(ItemDamageDisplayModel itemDamageDisplayModel, IGameUpdater gameUpdater, CameraModel cameraModel)
		{
			_itemDamageDisplayModel = itemDamageDisplayModel;
			_gameUpdater = gameUpdater;
			_cameraModel = cameraModel;
		}

		protected override void OnViewSet()
		{
			_gameUpdater.OnLateUpdate += UpdateViewPosition;
			_itemDamageDisplayModel.OnDisplayedItemChanged += ClearCurrency;
			_itemDamageDisplayModel.OnCurrentCurrencyChanged += OnDisplayedItemChanged;
			OnDisplayedItemChanged(_itemDamageDisplayModel.CurrentCurrency);
		}

		protected override void OnDisposed()
		{
			_gameUpdater.OnLateUpdate -= UpdateViewPosition;
			_itemDamageDisplayModel.OnDisplayedItemChanged -= ClearCurrency;
			_itemDamageDisplayModel.OnCurrentCurrencyChanged -= OnDisplayedItemChanged;
		}

		private void OnDisplayedItemChanged(int currency)
		{
			_pricePosition = Vector3.zero;
			if (currency == 0)
			{
				base.View.SwitchCostDisplay(show: false);
				return;
			}
			base.View.UpdateCostDisplay(currency, _itemDamageDisplayModel.IsWithChildCurrency);
			UpdateViewPosition();
			base.View.SwitchCostDisplay(show: true);
		}

		private void UpdateViewPosition()
		{
			if (_cameraModel.CameraObject == null || _itemDamageDisplayModel.CurrentDisplayedItem.Count == 0)
			{
				return;
			}
			RectTransform rectTransform = base.View.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			RectTransform rectTransform2 = rectTransform.parent as RectTransform;
			if (!(rectTransform2 == null))
			{
				_pricePosition = ((_pricePosition == Vector3.zero) ? _itemDamageDisplayModel.CurrentDisplayedItem.First().GetPricePosition() : Vector3.Lerp(_pricePosition, _itemDamageDisplayModel.CurrentDisplayedItem.First().GetPricePosition(), base.View.FollowSpeed * Time.deltaTime));
				Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(_cameraModel.CameraObject, _pricePosition);
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform2, screenPoint, null, out var localPoint))
				{
					base.View.SetLocalPosition(localPoint);
				}
			}
		}

		private void ClearCurrency()
		{
			base.View.ClearCurrency();
		}
	}
}
