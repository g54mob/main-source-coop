using Cysharp.Threading.Tasks;
using Features.AIModule.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.DebugModule.Scripts
{
	public class EnemySpawnDebugPresenter : PresenterBehaviour<EnemySpawnDebugView>
	{
		private readonly IEnemyManualSpawnService _enemyManualSpawnService;

		public EnemySpawnDebugPresenter(IEnemyManualSpawnService enemyManualSpawnService)
		{
			_enemyManualSpawnService = enemyManualSpawnService;
		}

		protected override void OnViewSet()
		{
			base.View.SetupEnemyDropdown();
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			if (base.View.SpawnEnemyButton != null)
			{
				base.View.SpawnEnemyButton.onClick.AddListener(SpawnSelectedEnemy);
			}
			if (base.View.SpawnEnemyNearPlayerButton != null)
			{
				base.View.SpawnEnemyNearPlayerButton.onClick.AddListener(SpawnSelectedEnemyNearPlayer);
			}
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			if (base.View.SpawnEnemyButton != null)
			{
				base.View.SpawnEnemyButton.onClick.RemoveListener(SpawnSelectedEnemy);
			}
			if (base.View.SpawnEnemyNearPlayerButton != null)
			{
				base.View.SpawnEnemyNearPlayerButton.onClick.RemoveListener(SpawnSelectedEnemyNearPlayer);
			}
		}

		private void SpawnSelectedEnemy()
		{
			SpawnSelectedEnemyAsync(spawnNearPlayer: false).Forget();
		}

		private void SpawnSelectedEnemyNearPlayer()
		{
			SpawnSelectedEnemyAsync(spawnNearPlayer: true).Forget();
		}

		private async UniTaskVoid SpawnSelectedEnemyAsync(bool spawnNearPlayer)
		{
			EnemyType selectedEnemyType = base.View.GetSelectedEnemyType();
			IEnemyBehaviour enemyBehaviour = ((!spawnNearPlayer) ? (await _enemyManualSpawnService.SpawnEnemy(selectedEnemyType)) : (await _enemyManualSpawnService.SpawnEnemyNearPlayer(selectedEnemyType)));
			IEnemyBehaviour enemyBehaviour2 = enemyBehaviour;
			if (enemyBehaviour2 != null)
			{
				Debug.Log($"Enemy debug spawn created {enemyBehaviour2.EnemyType}.");
			}
		}
	}
}
