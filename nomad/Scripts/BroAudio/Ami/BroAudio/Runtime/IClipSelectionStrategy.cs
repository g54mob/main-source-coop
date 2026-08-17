using Ami.BroAudio.Data;

namespace Ami.BroAudio.Runtime
{
	public interface IClipSelectionStrategy
	{
		IBroAudioClip SelectClip(BroAudioClip[] clips, ClipSelectionContext context, out int index);

		void Reset();
	}
}
