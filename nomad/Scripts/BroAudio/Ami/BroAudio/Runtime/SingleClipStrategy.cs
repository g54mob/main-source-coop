using Ami.BroAudio.Data;

namespace Ami.BroAudio.Runtime
{
	public class SingleClipStrategy : IClipSelectionStrategy
	{
		public IBroAudioClip SelectClip(BroAudioClip[] clips, ClipSelectionContext context, out int index)
		{
			index = 0;
			return clips[0];
		}

		public void Reset()
		{
		}
	}
}
