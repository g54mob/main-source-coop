using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Fusion;

namespace Features.StrechArmsModule.Scripts
{
	public class ArmControllersModel : ISessionCleanup
	{
		private readonly Dictionary<PlayerRef, List<ArmControllerData>> _playerStretchArmControllers = new Dictionary<PlayerRef, List<ArmControllerData>>();

		public IReadOnlyDictionary<PlayerRef, List<ArmControllerData>> PlayerStretchArmControllers => _playerStretchArmControllers;

		public event Action<PlayerRef, ArmControllerData> OnPlayerStretchArmControllerAdded;

		public event Action<PlayerRef, ArmControllerData> OnPlayerStretchArmControllerRemoved;

		public void RegisterStretchArmController(PlayerRef player, ArmControllerData armControllerData)
		{
			if (!_playerStretchArmControllers.TryAdd(player, new List<ArmControllerData> { armControllerData }))
			{
				_playerStretchArmControllers[player].Add(armControllerData);
			}
			this.OnPlayerStretchArmControllerAdded?.Invoke(player, armControllerData);
		}

		public void UnregisterStretchArmController(PlayerRef player, ArmControllerData armControllerData)
		{
			_playerStretchArmControllers[player].Remove(armControllerData);
			this.OnPlayerStretchArmControllerRemoved?.Invoke(player, armControllerData);
		}

		public void Cleanup()
		{
			_playerStretchArmControllers.Clear();
		}
	}
}
