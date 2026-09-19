using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Data;
using Features.PlayerStatesModule.Scripts;
using Fusion;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Sensors
{
	public class HeadcrabResnapSensor
	{
		private readonly HeadcrabEnemyContext _context;

		private readonly IPlayerStateService _playerStateService;

		private readonly BusyByHeadCrabPlayers _busyPlayers;

		public HeadcrabResnapSensor(HeadcrabEnemyContext context, IPlayerStateService playerStateService, BusyByHeadCrabPlayers busyPlayers)
		{
			_context = context;
			_playerStateService = playerStateService;
			_busyPlayers = busyPlayers;
		}

		public bool TryGetResnapCandidate(PlayerRef currentTarget, out PlayerRef candidate)
		{
			candidate = PlayerRef.None;
			if (_context.ResnapTargetDetector == null)
			{
				return false;
			}
			List<PlayerRef> detectTargets = _context.ResnapTargetDetector.DetectTargets;
			if (detectTargets == null || detectTargets.Count == 0)
			{
				return false;
			}
			foreach (PlayerRef item in detectTargets)
			{
				if (!(item == currentTarget) && _playerStateService.IsPlayerAlive(item.PlayerId) && (_busyPlayers == null || !_busyPlayers.BusyPlayers.Contains(item)))
				{
					candidate = item;
					return true;
				}
			}
			return false;
		}
	}
}
