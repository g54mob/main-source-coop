using System;
using System.Collections.Generic;
using Features.CameraModelModule;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab.CartGrabber;
using Features.MultiplayerSessionServices.Scripts;
using Features.WorldTokenModule.Scripts;
using Features.WorldTokenModule.Scripts.Views;
using UnityEngine;
using Zenject;

namespace Features.ItemsModule.Scripts
{
	public class CartPopupCostSystem : IInitializable, IDisposable
	{
		private readonly CartItemAddedEventClass _cartItemAddedEventClass;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IWorldTokenService _worldTokenService;

		private readonly CameraModel _cameraModel;

		private readonly List<WorldTokenPresenter> _worldTokenPool = new List<WorldTokenPresenter>();

		public CartPopupCostSystem(CartItemAddedEventClass cartItemAddedEventClass, MultiplayerModel multiplayerModel, IWorldTokenService worldTokenService, CameraModel cameraModel)
		{
			_cartItemAddedEventClass = cartItemAddedEventClass;
			_multiplayerModel = multiplayerModel;
			_worldTokenService = worldTokenService;
			_cameraModel = cameraModel;
		}

		public void Initialize()
		{
			_cartItemAddedEventClass.OnGrabableAddedInCart += ProcessAddedItem;
		}

		public void Dispose()
		{
			_cartItemAddedEventClass.OnGrabableAddedInCart -= ProcessAddedItem;
			_worldTokenPool.Clear();
		}

		private void ProcessAddedItem(ICartItemsContainer cartItemsContainer, IPointGrabable pointGrabable, int adderPlayerId)
		{
			if (adderPlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId && pointGrabable.GameObject.TryGetComponent<MonoItem>(out var component))
			{
				WorldTokenPresenter worldTokenFromPool = GetWorldTokenFromPool(WorldTokenType.TextTokenDisappearedWithUpMovement, cartItemsContainer.transform.position + Vector3.up);
				worldTokenFromPool.SetText(component.CurrencyValue + "¢");
				worldTokenFromPool.StartMovement();
			}
		}

		private WorldTokenPresenter GetWorldTokenFromPool(WorldTokenType tokenType, Vector3 position)
		{
			for (int num = _worldTokenPool.Count - 1; num >= 0; num--)
			{
				if (!_worldTokenPool[num].IsActive())
				{
					WorldTokenPresenter worldTokenPresenter = _worldTokenPool[num];
					worldTokenPresenter.SetPosition(position);
					return worldTokenPresenter;
				}
			}
			WorldTokenPresenter worldTokenPresenter2 = _worldTokenService.CreateWorldToken(tokenType, position);
			worldTokenPresenter2.SetCameraToLookAt(_cameraModel.CameraObject);
			_worldTokenPool.Add(worldTokenPresenter2);
			return worldTokenPresenter2;
		}
	}
}
