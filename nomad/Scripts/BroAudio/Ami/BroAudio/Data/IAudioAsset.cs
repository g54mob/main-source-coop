namespace Ami.BroAudio.Data
{
	public interface IAudioAsset
	{
		PlaybackGroup PlaybackGroup { get; }

		void LinkPlaybackGroup(PlaybackGroup upperGroup);
	}
}
