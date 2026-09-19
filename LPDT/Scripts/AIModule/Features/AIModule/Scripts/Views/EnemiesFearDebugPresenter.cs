using Features.EnemyFearingModule.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.AIModule.Scripts.Views
{
	[PublicAPI]
	public class EnemiesFearDebugPresenter : PresenterBehaviour<EnemiesFearDebugView>
	{
		private readonly IEnemyFearService _enemyFearService;

		public EnemiesFearDebugPresenter(IEnemyFearService enemyFearService)
		{
			_enemyFearService = enemyFearService;
		}

		protected override void OnViewSet()
		{
			base.View.SetupEnemyDropdown();
			base.View.SetupFearEnemiesDropdown();
			base.View.FearAllEnemiesButton.onClick.AddListener(FearAllEnemies);
			base.View.FearEnemyButton.onClick.AddListener(FearSelectedEnemy);
			base.View.FearEnemiesButton.onClick.AddListener(FearSelectedEnemiesList);
			base.View.FearEnemiesDropdown.onValueChanged.AddListener(ToggleFearEnemySelection);
		}

		protected override void OnDisposed()
		{
			base.View.FearAllEnemiesButton.onClick.RemoveListener(FearAllEnemies);
			base.View.FearEnemyButton.onClick.RemoveListener(FearSelectedEnemy);
			base.View.FearEnemiesButton.onClick.RemoveListener(FearSelectedEnemiesList);
			base.View.FearEnemiesDropdown.onValueChanged.RemoveListener(ToggleFearEnemySelection);
		}

		private void FearAllEnemies()
		{
			_enemyFearService.FearAllEnemy();
		}

		private void FearSelectedEnemy()
		{
			_enemyFearService.FearEnemy(base.View.GetSelectedEnemyType());
		}

		private void FearSelectedEnemiesList()
		{
			_enemyFearService.FearEnemies(base.View.GetSelectedFearEnemiesList());
		}

		private void ToggleFearEnemySelection(int selectedIndex)
		{
			base.View.ToggleFearEnemySelectionByDropdownIndex(selectedIndex);
		}
	}
}
