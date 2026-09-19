using Features.BeachInteractableCommonModule.Scripts;
using Features.GameUpdaterModule;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.PlayersStatisticsModule.Scripts.Views.SpawnedNewItem
{
	public class SpawnedNewItemPresenter : PresenterBehaviour<SpawnedNewItemViewBase>
	{
		private readonly LevelPlayersGameStatisticsModel _levelPlayersGameStatisticsModel;

		private readonly BeachInteractableIconsConfiguration _beachInteractableIconsConfiguration;

		private readonly IGameUpdater _gameUpdater;

		public SpawnedNewItemPresenter(LevelPlayersGameStatisticsModel levelPlayersGameStatisticsModel, BeachInteractableIconsConfiguration beachInteractableIconsConfiguration, IGameUpdater gameUpdater)
		{
			_levelPlayersGameStatisticsModel = levelPlayersGameStatisticsModel;
			_beachInteractableIconsConfiguration = beachInteractableIconsConfiguration;
			_gameUpdater = gameUpdater;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			UpdateView();
			_gameUpdater.OnUpdate += UpdateView;
		}

		protected override void OnViewDisabled()
		{
			_gameUpdater.OnUpdate -= UpdateView;
			base.OnViewDisabled();
		}

		protected override void OnDisposed()
		{
			_gameUpdater.OnUpdate -= UpdateView;
		}

		private void UpdateView()
		{
			BeachInteractableType spawnedNewItem = _levelPlayersGameStatisticsModel.SpawnedNewItem;
			if (spawnedNewItem == BeachInteractableType.None || !_beachInteractableIconsConfiguration.TryGetIcon(spawnedNewItem, out var icon))
			{
				base.View.SetVisible(visible: false);
				return;
			}
			base.View.SetVisible(visible: true);
			base.View.SetImage(icon);
		}
	}
}
