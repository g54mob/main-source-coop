using Features.AIModuleStateMachine.Scripts.Data;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.PlayerStatesModule.Scripts;
using Fusion;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Sensors
{
	public class HeadcrabTargetSensor
	{
		private readonly HeadcrabEnemyContext _context;

		private readonly IPlayerStateService _playerStateService;

		private readonly BusyByHeadCrabPlayers _busyPlayers;

		private readonly IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		public HeadcrabTargetSensor(HeadcrabEnemyContext context, IPlayerStateService playerStateService, BusyByHeadCrabPlayers busyPlayers, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService)
		{
			_context = context;
			_playerStateService = playerStateService;
			_busyPlayers = busyPlayers;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
		}

		public bool TryAcquireTarget(out PlayerRef target)
		{
			target = PlayerRef.None;
			if (_context.TargetDetector == null)
			{
				return false;
			}
			foreach (PlayerRef detectTarget in _context.TargetDetector.DetectTargets)
			{
				if (_enemyPlayerAttackabilityService.CanEnemyTargetPlayer(detectTarget.PlayerId) && _playerStateService.IsPlayerAlive(detectTarget.PlayerId) && (_busyPlayers == null || !_busyPlayers.BusyPlayers.Contains(detectTarget)) && _context.TryGetPlayerObject(detectTarget, out var playerObject) && _context.RaycastToTarget(playerObject))
				{
					_context.TargetObject = playerObject;
					target = detectTarget;
					return true;
				}
			}
			return false;
		}
	}
}
