using System.Collections.Generic;
using Ami.BroAudio.Data;
using UnityEngine;

namespace Ami.BroAudio.Runtime
{
	public class ShuffleClipStrategy : IClipSelectionStrategy
	{
		private BroAudioClip _lastUsed;

		private readonly HashSet<BroAudioClip> _used = new HashSet<BroAudioClip>();

		public IBroAudioClip SelectClip(BroAudioClip[] clips, ClipSelectionContext context, out int index)
		{
			index = Random.Range(0, clips.Length);
			if (Use(clips, index, out var result, checkLastUsed: true))
			{
				bool flag = false;
				foreach (BroAudioClip item in clips)
				{
					if (!_used.Contains(item))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					Reset();
					_lastUsed = result;
				}
				return result;
			}
			int num = ((Random.Range(0, 2) != 0) ? 1 : (-1));
			bool flag2 = false;
			for (int j = 0; j < clips.Length; j++)
			{
				index += num;
				index = (index + clips.Length) % clips.Length;
				if (flag2)
				{
					if (!_used.Contains(clips[index]))
					{
						return result;
					}
				}
				else if (Use(clips, index, out result))
				{
					flag2 = true;
				}
			}
			_lastUsed = result;
			Reset();
			return result;
		}

		private bool Use(BroAudioClip[] clips, int index, out BroAudioClip result, bool checkLastUsed = false)
		{
			result = clips[index];
			if (result == _lastUsed && checkLastUsed)
			{
				_lastUsed = null;
				return false;
			}
			if (result != _lastUsed && result.IsSet)
			{
				_used.Add(result);
				return true;
			}
			result = null;
			return false;
		}

		public void Reset()
		{
			_used.Clear();
		}
	}
}
