using System;
using Features.AIModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.GameModeModule.Scripts.Views
{
	public class EnemyConfigurationPresenter : PresenterBehaviour<EnemyConfigurationViewBase>
	{
		private readonly EnemySpawnConfigurationModel _configurationModel;

		private readonly MultiplayerModel _multiplayerModel;

		public EnemyConfigurationPresenter(EnemySpawnConfigurationModel configurationModel, MultiplayerModel multiplayerModel)
		{
			_configurationModel = configurationModel;
			_multiplayerModel = multiplayerModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.OnEnemyConfigurationChanged += ChangeConfiguration;
			base.View.SetEnemyConfigurationContainerActive(Debug.isDebugBuild && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient);
			base.View.RefreshEnemyConfigurationDropdown(_configurationModel.CurrentSpawnConfiguration);
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.OnEnemyConfigurationChanged -= ChangeConfiguration;
		}

		private void ChangeConfiguration(string selectedGameModeText)
		{
			if (Enum.TryParse<EnemySpawnConfigurationType>(selectedGameModeText, out var result))
			{
				_configurationModel.CurrentSpawnConfiguration = result;
				base.View.RefreshEnemyConfigurationDropdown(_configurationModel.CurrentSpawnConfiguration);
			}
		}
	}
}
