using Features.NavigationModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Sensors
{
	public class NavigationAwareEnemyTargetDetector : EnemyTargetDetector
	{
		private INavigationService _navigationService;

		[Inject]
		private void InjectNavigationService(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		public override float GetDistanceToPlayer(PlayerRef player)
		{
			if (_navigationService.TryGetPlayerTrackingPosition(player, out var position))
			{
				return Vector3.Distance(position, new Vector3(base.transform.position.x, position.y, base.transform.position.z));
			}
			return base.GetDistanceToPlayer(player);
		}
	}
}
