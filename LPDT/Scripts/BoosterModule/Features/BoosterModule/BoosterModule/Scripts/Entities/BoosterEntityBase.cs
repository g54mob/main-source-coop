using System;
using UnityEngine;

namespace Features.BoosterModule.BoosterModule.Scripts.Entities
{
	[Serializable]
	public abstract class BoosterEntityBase<TBoosterSettings> : IBoosterEntity where TBoosterSettings : BoosterSettingsBase
	{
		private float _currentBoosterLifeTime;

		public float CurrentBoosterLifeTime
		{
			get
			{
				return _currentBoosterLifeTime;
			}
			set
			{
				_currentBoosterLifeTime = value;
				this.OnCurrentLifeTimeChanged?.Invoke(_currentBoosterLifeTime);
			}
		}

		public float BoosterLifeTime { get; private set; }

		public Sprite BoosterIcon => BoosterSettings.BoosterIcon;

		public TBoosterSettings BoosterSettings { get; internal set; }

		public bool IsLifeTimeBlocked { get; set; }

		public event Action<float> OnCurrentLifeTimeChanged;

		public void UpdateBoosterSettings(TBoosterSettings boosterSettings)
		{
			BoosterSettings = boosterSettings;
			CurrentBoosterLifeTime = BoosterSettings.StartedBoosterLifeTime;
			BoosterLifeTime = BoosterSettings.BoosterLifeTime;
		}

		public abstract void Activate();

		public abstract void Deactivate();

		public string GetIdentifier()
		{
			return BoosterSettings.GetIdentifier();
		}

		public bool IsIdentifierEqual(string identifier)
		{
			return BoosterSettings.GetIdentifier() == identifier;
		}
	}
}
