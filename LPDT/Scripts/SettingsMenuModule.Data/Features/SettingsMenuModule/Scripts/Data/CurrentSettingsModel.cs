using System;
using Features.ProgressSavingModule.Scripts.Implementation;
using Global.Modules.LocalizationModule.Scripts.Generated;
using RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.Data
{
	public class CurrentSettingsModel : ISavable
	{
		private const string SAVE_ID = "CurrentSettings";

		private SettingsDataHolder _settingsDataHolder = new SettingsDataHolder();

		private readonly ISavingManager _savingManager;

		public SettingsDataHolder SettingsDataHolder
		{
			get
			{
				return _settingsDataHolder;
			}
			set
			{
				_settingsDataHolder = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public float AllVolume
		{
			get
			{
				return SettingsDataHolder.AllVolume;
			}
			set
			{
				SettingsDataHolder.AllVolume = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public float MusicVolume
		{
			get
			{
				return SettingsDataHolder.MusicVolume;
			}
			set
			{
				SettingsDataHolder.MusicVolume = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public float EffectsVolume
		{
			get
			{
				return SettingsDataHolder.EffectsVolume;
			}
			set
			{
				SettingsDataHolder.EffectsVolume = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public string MicrophoneDevice
		{
			get
			{
				return SettingsDataHolder.MicrophoneDevice;
			}
			set
			{
				SettingsDataHolder.MicrophoneDevice = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public float MicrophoneSensitivity
		{
			get
			{
				return SettingsDataHolder.MicrophoneSensitivity;
			}
			set
			{
				SettingsDataHolder.MicrophoneSensitivity = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public bool MicrophoneEnabled
		{
			get
			{
				return SettingsDataHolder.MicrophoneEnabled;
			}
			set
			{
				SettingsDataHolder.MicrophoneEnabled = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public bool NoiseSuppressionEnabled
		{
			get
			{
				return SettingsDataHolder.NoiseSuppressionEnabled;
			}
			set
			{
				SettingsDataHolder.NoiseSuppressionEnabled = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public bool PushToTalkEnabled
		{
			get
			{
				return SettingsDataHolder.PushToTalkEnabled;
			}
			set
			{
				SettingsDataHolder.PushToTalkEnabled = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public bool YAxisInvertEnabled
		{
			get
			{
				return SettingsDataHolder.YAxisInvertEnabled;
			}
			set
			{
				SettingsDataHolder.YAxisInvertEnabled = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public bool VSyncEnabled
		{
			get
			{
				return SettingsDataHolder.VSyncEnabled;
			}
			set
			{
				SettingsDataHolder.VSyncEnabled = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public bool StreamerModeEnabled
		{
			get
			{
				return SettingsDataHolder.StreamerModeEnabled;
			}
			set
			{
				SettingsDataHolder.StreamerModeEnabled = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public int QualityLevel
		{
			get
			{
				return SettingsDataHolder.QualityLevel;
			}
			set
			{
				SettingsDataHolder.QualityLevel = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public float MouseSensitivity
		{
			get
			{
				return SettingsDataHolder.MouseSensitivity;
			}
			set
			{
				SettingsDataHolder.MouseSensitivity = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public float FieldOfView
		{
			get
			{
				return SettingsDataHolder.FieldOfView;
			}
			set
			{
				SettingsDataHolder.FieldOfView = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public float Brightness
		{
			get
			{
				return SettingsDataHolder.Brightness;
			}
			set
			{
				SettingsDataHolder.Brightness = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public ScreenMode ScreenMode
		{
			get
			{
				return (ScreenMode)SettingsDataHolder.ScreenMode;
			}
			set
			{
				SettingsDataHolder.ScreenMode = (int)value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public Vector2Int Resolution
		{
			get
			{
				return SettingsDataHolder.Resolution;
			}
			set
			{
				SettingsDataHolder.Resolution = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public int FPSLimit
		{
			get
			{
				return SettingsDataHolder.FPSLimit;
			}
			set
			{
				SettingsDataHolder.FPSLimit = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public float ScreenShakeIntensityNormalized
		{
			get
			{
				return SettingsDataHolder.ScreenShakeIntensityNormalized;
			}
			set
			{
				SettingsDataHolder.ScreenShakeIntensityNormalized = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public float HeadBobbingIntensityNormalized
		{
			get
			{
				return SettingsDataHolder.HeadBobbingIntensityNormalized;
			}
			set
			{
				SettingsDataHolder.HeadBobbingIntensityNormalized = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public Language Language
		{
			get
			{
				return (Language)SettingsDataHolder.Language;
			}
			set
			{
				SettingsDataHolder.Language = (int)value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public string FixedRegion
		{
			get
			{
				return SettingsDataHolder.FixedRegion;
			}
			set
			{
				SettingsDataHolder.FixedRegion = value;
				this.OnSettingsDataHolderChanged?.Invoke();
			}
		}

		public bool HasFixedRegion => !string.IsNullOrEmpty(SettingsDataHolder.FixedRegion);

		public bool HasSavedSettings { get; private set; }

		public SavingGroup SavingGroup => SavingGroup.Settings;

		public event Action OnSettingsDataHolderChanged;

		public CurrentSettingsModel(ISavingManager savingManager)
		{
			_savingManager = savingManager;
		}

		public void LoadData()
		{
			SettingsDataHolder settingsDataHolder = _savingManager.LoadDataForID<SettingsDataHolder>("CurrentSettings");
			HasSavedSettings = settingsDataHolder != null;
			if (HasSavedSettings)
			{
				SettingsDataHolder = settingsDataHolder;
			}
		}

		public void SaveData()
		{
			_savingManager.SaveDataWithID("CurrentSettings", SettingsDataHolder, SavingGroup.ToString());
		}
	}
}
