using Ami.BroAudio.Data;

namespace Ami.BroAudio.Runtime
{
	public class VelocityClipStrategy : IClipSelectionStrategy
	{
		public IBroAudioClip SelectClip(BroAudioClip[] clips, ClipSelectionContext context, out int index)
		{
			index = 0;
			for (int i = 0; i < clips.Length; i++)
			{
				if (clips[i].Velocity > context.Value)
				{
					index = ((i != 0) ? (i - 1) : 0);
					return clips[index];
				}
			}
			if (clips.Length == 0)
			{
				return null;
			}
			return clips[clips.Length - 1];
		}

		public void Reset()
		{
		}
	}
}
