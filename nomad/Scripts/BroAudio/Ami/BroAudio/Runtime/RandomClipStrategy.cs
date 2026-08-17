using Ami.BroAudio.Data;
using UnityEngine;

namespace Ami.BroAudio.Runtime
{
	public class RandomClipStrategy : IClipSelectionStrategy
	{
		public IBroAudioClip SelectClip(BroAudioClip[] clips, ClipSelectionContext context, out int index)
		{
			index = 0;
			int num = 0;
			foreach (BroAudioClip broAudioClip in clips)
			{
				num += broAudioClip.Weight;
			}
			if (num == 0)
			{
				index = Random.Range(0, clips.Length);
				return clips[index];
			}
			int num2 = Random.Range(0, num);
			int num3 = 0;
			for (int j = 0; j < clips.Length; j++)
			{
				num3 += clips[j].Weight;
				if (num2 < num3)
				{
					index = j;
					return clips[j];
				}
			}
			return null;
		}

		public void Reset()
		{
		}
	}
}
