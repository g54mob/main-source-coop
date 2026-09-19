using System;

namespace Photon.Realtime
{
	[Serializable]
	public class MatchmakingReconnectInformation
	{
		public string Room;

		public string Region;

		public string AppVersion;

		public string UserId;

		public long TimeoutInTicks;

		public TimeSpan DefaultTimeout = TimeSpan.FromSeconds(20.0);

		public DateTime Timeout
		{
			get
			{
				return new DateTime(TimeoutInTicks);
			}
			set
			{
				TimeoutInTicks = value.Ticks;
			}
		}

		public bool HasTimedOut => Timeout < DateTime.Now;

		public virtual void Set(RealtimeClient client)
		{
			Set(client, DefaultTimeout);
		}

		public void Set(RealtimeClient client, TimeSpan timeSpan)
		{
			if (client != null)
			{
				Room = client.CurrentRoom.Name;
				Region = client.CurrentRegion;
				Timeout = DateTime.Now + timeSpan;
				UserId = client.UserId;
				AppVersion = client.AppSettings.AppVersion;
			}
		}

		public override string ToString()
		{
			return $"Room '{Room}' Region '{Region}' Timeout {Timeout}' AppVersion '{AppVersion}' UserId '{UserId}'";
		}
	}
}
