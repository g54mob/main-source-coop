using System;
using System.Collections.Generic;

namespace Mimicraft.Voice
{
	public static class VoiceSpeakers
	{
		public readonly struct Speaker
		{
			public readonly ulong ClientId;

			public readonly VoiceChannel Channel;

			public readonly VoiceReach Reach;

			public Speaker(ulong clientId, VoiceChannel channel, VoiceReach reach)
			{
				ClientId = clientId;
				Channel = channel;
				Reach = reach;
			}
		}

		private static readonly Dictionary<ulong, Speaker> speaking = new Dictionary<ulong, Speaker>();

		private static readonly Dictionary<ulong, float> levels = new Dictionary<ulong, float>();

		private static readonly List<Speaker> snapshot = new List<Speaker>();

		private static bool dirty = true;

		public static IReadOnlyList<Speaker> Active
		{
			get
			{
				if (!dirty)
				{
					return snapshot;
				}
				snapshot.Clear();
				foreach (KeyValuePair<ulong, Speaker> item in speaking)
				{
					snapshot.Add(item.Value);
				}
				dirty = false;
				return snapshot;
			}
		}

		public static event Action Changed;

		public static bool IsSpeaking(ulong clientId)
		{
			return speaking.ContainsKey(clientId);
		}

		public static bool TryGet(ulong clientId, out Speaker speaker)
		{
			return speaking.TryGetValue(clientId, out speaker);
		}

		public static void Set(ulong clientId, VoiceChannel channel, VoiceReach reach)
		{
			if (!speaking.TryGetValue(clientId, out var value) || value.Channel != channel || value.Reach != reach)
			{
				speaking[clientId] = new Speaker(clientId, channel, reach);
				dirty = true;
				VoiceSpeakers.Changed?.Invoke();
			}
		}

		public static void SetLevel(ulong clientId, float level)
		{
			levels[clientId] = level;
		}

		public static float LevelOf(ulong clientId)
		{
			if (!levels.TryGetValue(clientId, out var value))
			{
				return 0f;
			}
			return value;
		}

		public static void Clear(ulong clientId)
		{
			levels.Remove(clientId);
			if (speaking.Remove(clientId))
			{
				dirty = true;
				VoiceSpeakers.Changed?.Invoke();
			}
		}

		public static void ClearAll()
		{
			if (speaking.Count != 0)
			{
				speaking.Clear();
				levels.Clear();
				dirty = true;
				VoiceSpeakers.Changed?.Invoke();
			}
		}
	}
}
