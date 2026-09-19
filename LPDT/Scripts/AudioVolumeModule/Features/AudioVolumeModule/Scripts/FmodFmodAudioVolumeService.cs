using FMOD.Studio;
using FMODUnity;

namespace Features.AudioVolumeModule.Scripts
{
	public class FmodFmodAudioVolumeService : IFmodAudioVolumeService
	{
		private const string ALL_BUS_NAME = "bus:/All";

		private const string MUSIC_BUS_NAME = "bus:/All/Music";

		private const string EFFECTS_BUS_NAME = "bus:/All/SFX";

		private Bus _allBus;

		private Bus _musicBus;

		private Bus _effectsBus;

		public void SetAllBusValue(float value)
		{
			if (!_allBus.isValid())
			{
				_allBus = RuntimeManager.GetBus("bus:/All");
			}
			if (value < 0f)
			{
				value = 0f;
			}
			if (value > 1f)
			{
				value = 1f;
			}
			_allBus.setVolume(value);
		}

		public void SetMusicValue(float value)
		{
			if (!_musicBus.isValid())
			{
				_musicBus = RuntimeManager.GetBus("bus:/All/Music");
			}
			if (value < 0f)
			{
				value = 0f;
			}
			if (value > 1f)
			{
				value = 1f;
			}
			_musicBus.setVolume(value);
		}

		public void SetEffectValue(float value)
		{
			if (!_effectsBus.isValid())
			{
				_effectsBus = RuntimeManager.GetBus("bus:/All/SFX");
			}
			if (value < 0f)
			{
				value = 0f;
			}
			if (value > 1f)
			{
				value = 1f;
			}
			_effectsBus.setVolume(value);
		}
	}
}
