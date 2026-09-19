using Features.MultiplayerSessionServices.Scripts;
using Features.SessionManagementModule.Models;
using Features.StoreModule.Scripts;
using UnityEngine;

namespace Features.SessionManagementModule.Installers
{
	public sealed class SessionShopSeatHealth : IShopSeatHealthObservation
	{
		private readonly MultiplayerModel _multiplayerModel;

		private StoreTableBehaviour _storeTableBehaviour;

		public bool IsStoreTablePresent
		{
			get
			{
				if (_storeTableBehaviour == null)
				{
					_storeTableBehaviour = Object.FindAnyObjectByType<StoreTableBehaviour>();
				}
				return _storeTableBehaviour != null;
			}
		}

		public SessionShopSeatHealth(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public bool TryVerifyLocalSeatSpendable(out string problem)
		{
			if (_storeTableBehaviour == null)
			{
				_storeTableBehaviour = Object.FindAnyObjectByType<StoreTableBehaviour>();
			}
			if (_storeTableBehaviour == null)
			{
				problem = "no store table present yet";
				return false;
			}
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			return _storeTableBehaviour.TryVerifyLocalSeatSpendable(playerId, out problem);
		}

		public bool TryVerifyAllSeatedAvatarsInShop(out string problem)
		{
			if (_storeTableBehaviour == null)
			{
				_storeTableBehaviour = Object.FindAnyObjectByType<StoreTableBehaviour>();
			}
			if (_storeTableBehaviour == null)
			{
				problem = "no store table present";
				return false;
			}
			return _storeTableBehaviour.TryVerifyAllSeatedAvatarsPresent(out problem);
		}

		public bool TryVerifyLocalSeatedAvatarPresent(out string problem)
		{
			if (_storeTableBehaviour == null)
			{
				_storeTableBehaviour = Object.FindAnyObjectByType<StoreTableBehaviour>();
			}
			if (_storeTableBehaviour == null)
			{
				problem = "no store table present";
				return false;
			}
			return _storeTableBehaviour.TryVerifyLocalSeatedAvatarPresent(out problem);
		}
	}
}
