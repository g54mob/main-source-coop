using System;
using System.Linq;
using Features.SettingsMenuModule.Scripts.Data;
using Features.VoiceSpeakersModule.Scripts.Data;
using PlayerCustomization;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class PlayersSoundSettingsItemPresenter : PresenterBehaviour<PlayersSoundSettingsItemViewBase>
	{
		private PlayersVolumeData _playersVolumeData;

		private PlayerCustomizationModel _playerCustomizationModel;

		private int _targetPlayer;

		[Inject]
		public void InjectDependencies(PlayersVolumeData playersVolumeData, PlayerCustomizationModel playerCustomizationModel, SpawnedVoiceModel spawnedVoiceModel)
		{
			_playersVolumeData = playersVolumeData;
			_playerCustomizationModel = playerCustomizationModel;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			PlayersSoundSettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<float>)Delegate.Combine(view.OnValueChanged, new Action<float>(OnPlayerVolumeValueChanged));
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			PlayersSoundSettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<float>)Delegate.Remove(view.OnValueChanged, new Action<float>(OnPlayerVolumeValueChanged));
		}

		public void DestroyView()
		{
			UnityEngine.Object.Destroy(base.View.gameObject);
		}

		public void SetTargetPlayer(int targetPlayer)
		{
			_targetPlayer = targetPlayer;
			base.View.SetValue(_playersVolumeData.PlayersVolumeInfo[_targetPlayer].Volume);
			PlayerCustomizationSlotData playerCustomizationSlotData = _playerCustomizationModel.Slots.FirstOrDefault((PlayerCustomizationSlotData slotData) => slotData.PlayerId == _targetPlayer);
			if (playerCustomizationSlotData != null)
			{
				base.View.SetPlayerInfo(playerCustomizationSlotData);
			}
		}

		public Selectable GetPrioritySelectable()
		{
			return base.View.PrioritySelectable;
		}

		private void OnPlayerVolumeValueChanged(float value)
		{
			if (_playersVolumeData.PlayersVolumeInfo.TryGetValue(_targetPlayer, out var value2))
			{
				value2.UpdateVolume(value);
			}
		}
	}
}
