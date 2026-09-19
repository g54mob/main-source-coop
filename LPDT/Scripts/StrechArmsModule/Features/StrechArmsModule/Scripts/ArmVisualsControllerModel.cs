using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;

namespace Features.StrechArmsModule.Scripts
{
	public class ArmVisualsControllerModel : ISessionCleanup
	{
		private static readonly IReadOnlyDictionary<Arm, ArmVisualsController> _emptyArms = new Dictionary<Arm, ArmVisualsController>();

		private readonly Dictionary<int, Dictionary<Arm, ArmVisualsController>> _byPlayerId = new Dictionary<int, Dictionary<Arm, ArmVisualsController>>();

		public event Action<int, Arm> OnPlayerArmVisualsControllerRegistered;

		public event Action<int, Arm> OnPlayerArmVisualsControllerUnregistered;

		public void RegisterArmVisualsController(int playerId, Arm armOrientation, ArmVisualsController armVisualsController)
		{
			if (!_byPlayerId.TryGetValue(playerId, out var value))
			{
				value = new Dictionary<Arm, ArmVisualsController>();
				_byPlayerId[playerId] = value;
			}
			value[armOrientation] = armVisualsController;
			this.OnPlayerArmVisualsControllerRegistered?.Invoke(playerId, armOrientation);
		}

		public void UnregisterArmVisualsController(int playerId, Arm armOrientation, ArmVisualsController armVisualsController)
		{
			if (_byPlayerId.TryGetValue(playerId, out var value) && value.TryGetValue(armOrientation, out var value2) && value2 == armVisualsController)
			{
				value.Remove(armOrientation);
				if (value.Count == 0)
				{
					_byPlayerId.Remove(playerId);
				}
			}
			this.OnPlayerArmVisualsControllerUnregistered?.Invoke(playerId, armOrientation);
		}

		public ArmVisualsController GetArmVisualsController(int playerId, Arm armOrientation)
		{
			if (!_byPlayerId.TryGetValue(playerId, out var value))
			{
				return null;
			}
			return value.GetValueOrDefault(armOrientation);
		}

		public IReadOnlyDictionary<Arm, ArmVisualsController> GetArmVisualsControllers(int playerId)
		{
			if (!_byPlayerId.TryGetValue(playerId, out var value))
			{
				return _emptyArms;
			}
			return value;
		}

		public void Cleanup()
		{
			_byPlayerId.Clear();
		}
	}
}
