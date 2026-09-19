using System;
using Features.AIModule.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.GameModeModule.Scripts.Views
{
	public abstract class EnemyConfigurationViewBase : ViewBehaviour
	{
		public abstract event Action<string> OnEnemyConfigurationChanged;

		public abstract void RefreshEnemyConfigurationDropdown(EnemySpawnConfigurationType activeEnemyConfiguration);

		public abstract void SetEnemyConfigurationContainerActive(bool isActive);
	}
}
