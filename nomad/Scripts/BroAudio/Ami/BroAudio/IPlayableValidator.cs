using UnityEngine;

namespace Ami.BroAudio
{
	public interface IPlayableValidator
	{
		bool IsPlayable(SoundID id, Vector3 position);

		void OnGetPlayer(IAudioPlayer player);
	}
}
