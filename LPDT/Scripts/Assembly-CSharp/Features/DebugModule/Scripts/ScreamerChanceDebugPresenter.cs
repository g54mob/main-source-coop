using Features.ChestScreamerModule.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.DebugModule.Scripts
{
	public class ScreamerChanceDebugPresenter : PresenterBehaviour<ScreamerChanceDebugViewBase>
	{
		private readonly ChestScreamerConfiguration _chestScreamerConfiguration;

		public ScreamerChanceDebugPresenter(ChestScreamerConfiguration chestScreamerConfiguration)
		{
			_chestScreamerConfiguration = chestScreamerConfiguration;
		}

		protected override void OnViewSet()
		{
			base.View.SetValue(_chestScreamerConfiguration.ProcChance);
			base.View.OnValueChanged += UpdateScreamerChance;
			base.View.OnApplyClicked += ApplyToCurrentLevelChests;
		}

		protected override void OnDisposed()
		{
			base.View.OnValueChanged -= UpdateScreamerChance;
			base.View.OnApplyClicked -= ApplyToCurrentLevelChests;
		}

		private void UpdateScreamerChance(float value)
		{
			_chestScreamerConfiguration.SetRuntimeProcChanceOverride(value);
		}

		private void ApplyToCurrentLevelChests()
		{
			ChestScreamerController[] array = Object.FindObjectsByType<ChestScreamerController>(FindObjectsSortMode.None);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DebugApplyScreamerChance();
			}
		}
	}
}
