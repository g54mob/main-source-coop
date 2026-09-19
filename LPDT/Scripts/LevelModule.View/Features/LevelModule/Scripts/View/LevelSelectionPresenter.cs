using System;
using Features.MultiplayerSessionServices.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.LevelModule.Scripts.View
{
	public class LevelSelectionPresenter : PresenterBehaviour<LevelSelectionViewBase>
	{
		private readonly LevelModel _levelModel;

		private readonly MultiplayerModel _multiplayerModel;

		public LevelSelectionPresenter(LevelModel levelModel, MultiplayerModel multiplayerModel)
		{
			_levelModel = levelModel;
			_multiplayerModel = multiplayerModel;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.OnSelectedLevelChanged += ChangeSelectedLevel;
			base.View.SetLevelSelectionActive(Debug.isDebugBuild && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient);
			base.View.InitializeLevelSelectionDropdown();
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			base.View.OnSelectedLevelChanged -= ChangeSelectedLevel;
		}

		private void ChangeSelectedLevel(string selectedLevelText)
		{
			if (!Enum.TryParse<LevelType>(selectedLevelText.Replace(" ", ""), out var result))
			{
				return;
			}
			if (result == LevelType.PhysicsSandbox)
			{
				_levelModel.SetSelectedSequenceSet(LevelSequenceSet.PhysicsVolumeSandbox);
				_levelModel.SetSelectedChapter(0);
				base.View.RefreshLevelSelectionDropdown(result);
				return;
			}
			if (_levelModel.SelectedSequenceSet == LevelSequenceSet.PhysicsVolumeSandbox)
			{
				_levelModel.SetSelectedSequenceSet(LevelSequenceSet.Default);
			}
			_levelModel.SetSelectedLevel(result);
			base.View.RefreshLevelSelectionDropdown(result);
		}
	}
}
