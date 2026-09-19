using System;

namespace Features.SettingsMenuModule.Scripts.Data
{
	public class PlayerVolumeInfo
	{
		public float Volume { get; private set; }

		public event Action<float> OnPlayerVolumeUpdate;

		public PlayerVolumeInfo(float volume)
		{
			Volume = volume;
		}

		public void UpdateVolume(float volume)
		{
			Volume = volume;
			this.OnPlayerVolumeUpdate?.Invoke(Volume);
		}
	}
}
