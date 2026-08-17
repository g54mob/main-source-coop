using System;
using Ami.BroAudio.Data;

namespace Ami.BroAudio.Runtime
{
	public class LayeredClipStrategy : IClipSelectionStrategy
	{
		public IBroAudioClip SelectClip(BroAudioClip[] clips, ClipSelectionContext context, out int index)
		{
			throw new NotImplementedException();
		}

		public void Reset()
		{
		}
	}
}
