using System.Collections.Generic;
using Mimicraft.Networking;
using UnityEngine;

namespace Mimicraft.Voice
{
	public static class VoiceMuteStore
	{
		private struct Entry
		{
			public float Volume;

			public bool Muted;
		}

		private const string VolumePrefix = "Mimicraft.Voice.Vol.";

		private const string MutePrefix = "Mimicraft.Voice.Mute.";

		public const float MaxVolume = 2f;

		private static readonly Dictionary<ulong, Entry> persistent = new Dictionary<ulong, Entry>();

		private static readonly Dictionary<ulong, Entry> session = new Dictionary<ulong, Entry>();

		private const float FlushIntervalSeconds = 1f;

		private static float nextFlush;

		private static bool pendingFlush;

		private static bool hookedQuit;

		public static int Version { get; private set; }

		private static Entry Default => new Entry
		{
			Volume = 1f,
			Muted = false
		};

		public static float VolumeFor(ulong clientId)
		{
			return Read(clientId).Volume;
		}

		public static bool IsMuted(ulong clientId)
		{
			return Read(clientId).Muted;
		}

		public static float GainFor(ulong clientId)
		{
			Entry entry = Read(clientId);
			if (!entry.Muted)
			{
				return entry.Volume;
			}
			return 0f;
		}

		public static void SetVolume(ulong clientId, float volume)
		{
			Entry entry = Read(clientId);
			entry.Volume = Mathf.Clamp(volume, 0f, 2f);
			Write(clientId, entry);
		}

		public static void SetMuted(ulong clientId, bool muted)
		{
			Entry entry = Read(clientId);
			entry.Muted = muted;
			Write(clientId, entry);
		}

		public static void Forget(ulong clientId)
		{
			if (session.Remove(clientId))
			{
				Version++;
			}
		}

		private static Entry Read(ulong clientId)
		{
			ulong num = SteamIdOf(clientId);
			if (num == 0L)
			{
				if (!session.TryGetValue(clientId, out var value))
				{
					return Default;
				}
				return value;
			}
			if (persistent.TryGetValue(num, out var value2))
			{
				return value2;
			}
			Entry entry = new Entry
			{
				Volume = PlayerPrefs.GetFloat("Mimicraft.Voice.Vol." + num, 1f),
				Muted = (PlayerPrefs.GetInt("Mimicraft.Voice.Mute." + num, 0) != 0)
			};
			persistent[num] = entry;
			return entry;
		}

		private static void Write(ulong clientId, Entry entry)
		{
			Version++;
			ulong num = SteamIdOf(clientId);
			if (num == 0L)
			{
				session[clientId] = entry;
				return;
			}
			persistent[num] = entry;
			if (Mathf.Approximately(entry.Volume, 1f))
			{
				PlayerPrefs.DeleteKey("Mimicraft.Voice.Vol." + num);
			}
			else
			{
				PlayerPrefs.SetFloat("Mimicraft.Voice.Vol." + num, entry.Volume);
			}
			if (entry.Muted)
			{
				PlayerPrefs.SetInt("Mimicraft.Voice.Mute." + num, 1);
			}
			else
			{
				PlayerPrefs.DeleteKey("Mimicraft.Voice.Mute." + num);
			}
			Flush();
		}

		private static void Flush()
		{
			HookQuit();
			pendingFlush = true;
			if (!(Time.realtimeSinceStartup < nextFlush))
			{
				nextFlush = Time.realtimeSinceStartup + 1f;
				pendingFlush = false;
				PlayerPrefs.Save();
			}
		}

		private static void HookQuit()
		{
			if (hookedQuit)
			{
				return;
			}
			hookedQuit = true;
			Application.quitting += delegate
			{
				if (pendingFlush)
				{
					PlayerPrefs.Save();
				}
			};
		}

		private static ulong SteamIdOf(ulong clientId)
		{
			GameModeController current = GameModeController.Current;
			if (!(current != null))
			{
				return 0uL;
			}
			return current.GetSteamId(clientId);
		}
	}
}
