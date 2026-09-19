using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Dissonance.Audio.Playback
{
	public static class RemoteSpeakerDeathState
	{
		private static readonly HashSet<string> _deadPlayerIds;

		static RemoteSpeakerDeathState()
		{
			_deadPlayerIds = new HashSet<string>();
			SceneManager.sceneLoaded += delegate
			{
				_deadPlayerIds.Clear();
			};
		}

		public static void SetDead(string playerId, bool dead)
		{
			if (!string.IsNullOrEmpty(playerId))
			{
				if (dead)
				{
					_deadPlayerIds.Add(playerId);
				}
				else
				{
					_deadPlayerIds.Remove(playerId);
				}
			}
		}

		public static bool IsDead(string playerId)
		{
			if (!string.IsNullOrEmpty(playerId))
			{
				return _deadPlayerIds.Contains(playerId);
			}
			return false;
		}
	}
}
