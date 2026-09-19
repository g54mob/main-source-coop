using Features.AIModule.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.DebugModule.Scripts
{
	public class EnemyDespawnDebugPresenter : PresenterBehaviour<EnemyDespawnDebugViewBase>
	{
		private readonly IEnemyForceDespawnService _enemyForceDespawnService;

		public EnemyDespawnDebugPresenter(IEnemyForceDespawnService enemyForceDespawnService)
		{
			_enemyForceDespawnService = enemyForceDespawnService;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.DespawnEnemiesButton.onClick.AddListener(DespawnEnemies);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.DespawnEnemiesButton.onClick.RemoveListener(DespawnEnemies);
		}

		private void DespawnEnemies()
		{
			int num = _enemyForceDespawnService.ForceDespawnAllLiveEnemies();
			Debug.Log($"Force enemy despawn debug action despawned {num} enemies.");
		}
	}
}
