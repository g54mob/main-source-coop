namespace Features.AudioVolumeModule.Scripts
{
	public interface IFmodAudioVolumeService
	{
		void SetAllBusValue(float value);

		void SetMusicValue(float value);

		void SetEffectValue(float value);
	}
}
