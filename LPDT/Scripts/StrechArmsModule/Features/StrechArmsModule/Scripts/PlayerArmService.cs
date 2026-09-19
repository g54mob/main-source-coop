using System.Linq;
using Fusion;

namespace Features.StrechArmsModule.Scripts
{
	public class PlayerArmService : IPlayerArmService
	{
		private readonly PlayersArmsModel _playersArmsModel;

		private readonly ArmControllersModel _armControllersModel;

		public PlayerArmService(PlayersArmsModel playersArmsModel, ArmControllersModel armControllersModel)
		{
			_playersArmsModel = playersArmsModel;
			_armControllersModel = armControllersModel;
		}

		public void TearOffPlayerArm(PlayerRef player, Arm arm)
		{
			if (IsAbleToTearOffArm(player))
			{
				ArmControllerData armControllerData = _armControllersModel.PlayerStretchArmControllers[player].FirstOrDefault((ArmControllerData x) => x.Arm == arm);
				if (armControllerData != null)
				{
					armControllerData.StretchArmController.TearOffArm();
					_playersArmsModel.RemovePlayerArm(player.PlayerId, arm);
				}
			}
		}

		private bool IsAbleToTearOffArm(PlayerRef player)
		{
			if (_playersArmsModel.AvailableArms.TryGetValue(player.PlayerId, out var _))
			{
				return _armControllersModel.PlayerStretchArmControllers.ContainsKey(player);
			}
			return false;
		}

		public bool IsCanTearOfArm(PlayerRef player, Arm arm)
		{
			if (_playersArmsModel.AvailableArms.TryGetValue(player.PlayerId, out var value) && _armControllersModel.PlayerStretchArmControllers.ContainsKey(player))
			{
				return value.Contains(arm);
			}
			return false;
		}
	}
}
