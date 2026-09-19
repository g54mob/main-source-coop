using System.Collections.Generic;
using Features.MultiplayerSessionServices.Scripts;
using Features.SessionManagementModule.Models;
using Features.SessionManagementModule.Networked;
using Fusion;
using UnityEngine;

namespace Features.SessionManagementModule.Gate
{
	public sealed class ShopLeaveVoteObservation : IShopLeaveVoteObservation
	{
		private readonly MultiplayerModel _multiplayerModel;

		public ShopLeaveVoteObservation(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public bool HasEveryPresentPlayerVotedToLeave()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (!networkRunner.IsRunning)
			{
				return false;
			}
			HashSet<PlayerRef> hashSet = new HashSet<PlayerRef>();
			ShopVoteNetworkObject[] array = Object.FindObjectsByType<ShopVoteNetworkObject>(FindObjectsSortMode.None);
			foreach (ShopVoteNetworkObject shopVoteNetworkObject in array)
			{
				if (!(shopVoteNetworkObject.Runner != networkRunner) && shopVoteNetworkObject.HasVotedToLeave)
				{
					hashSet.Add(shopVoteNetworkObject.Object.StateAuthority);
				}
			}
			bool result = false;
			foreach (PlayerRef activePlayer in networkRunner.ActivePlayers)
			{
				result = true;
				if (!hashSet.Contains(activePlayer))
				{
					return false;
				}
			}
			return result;
		}
	}
}
