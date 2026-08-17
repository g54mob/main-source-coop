using Ami.BroAudio.Data;

namespace Ami.BroAudio.Runtime
{
	public class SequenceClipStrategy : IClipSelectionStrategy
	{
		private int _sequenceIndex = -1;

		public IBroAudioClip SelectClip(BroAudioClip[] clips, ClipSelectionContext context, out int currentIndex)
		{
			int num = 0;
			if (_sequenceIndex > -1)
			{
				num = _sequenceIndex + 1;
				num = ((num < clips.Length) ? num : 0);
			}
			else if (clips[0].IsSet)
			{
				num = 0;
			}
			if (clips[num].IsSet)
			{
				_sequenceIndex = num;
				currentIndex = num;
			}
			else
			{
				_sequenceIndex = -1;
				currentIndex = -1;
			}
			return clips[_sequenceIndex];
		}

		public void Reset()
		{
			_sequenceIndex = -1;
		}
	}
}
