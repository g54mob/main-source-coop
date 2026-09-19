using System;
using Features.SettingsMenuModule.Scripts.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class SoundSettingsItemPresenter : PresenterBehaviour<SoundSettingsItemViewBase>
	{
		private NotAppliedSettingsModel _notAppliedSettingsModel;

		private CurrentSettingsModel _currentSettingsModel;

		[Inject]
		public void InjectDependencies(NotAppliedSettingsModel notAppliedSettingsModel, CurrentSettingsModel currentSettingsModel)
		{
			_currentSettingsModel = currentSettingsModel;
			_notAppliedSettingsModel = notAppliedSettingsModel;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			UpdateCurrentValue();
			SoundSettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<float, SoundSettingsType>)Delegate.Combine(view.OnValueChanged, new Action<float, SoundSettingsType>(OnSoundSettingsValueChanged));
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			SoundSettingsItemViewBase view = base.View;
			view.OnValueChanged = (Action<float, SoundSettingsType>)Delegate.Remove(view.OnValueChanged, new Action<float, SoundSettingsType>(OnSoundSettingsValueChanged));
		}

		private void UpdateCurrentValue()
		{
			float num = base.View.GetSoundType() switch
			{
				SoundSettingsType.All => _currentSettingsModel.AllVolume, 
				SoundSettingsType.Music => _currentSettingsModel.MusicVolume, 
				SoundSettingsType.Effects => _currentSettingsModel.EffectsVolume, 
				_ => 0f, 
			};
			base.View.SetValue(num * 100f);
		}

		private void OnSoundSettingsValueChanged(float value, SoundSettingsType soundType)
		{
			switch (soundType)
			{
			case SoundSettingsType.All:
				_notAppliedSettingsModel.AllVolume = value / 100f;
				break;
			case SoundSettingsType.Music:
				_notAppliedSettingsModel.MusicVolume = value / 100f;
				break;
			case SoundSettingsType.Effects:
				_notAppliedSettingsModel.EffectsVolume = value / 100f;
				break;
			}
		}
	}
}
