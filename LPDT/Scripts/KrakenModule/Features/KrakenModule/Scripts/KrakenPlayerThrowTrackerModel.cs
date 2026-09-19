using System;
using System.Collections.Generic;
using Features.LevelModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.KrakenModule.Scripts
{
	public class KrakenPlayerThrowTrackerModel : ILevelCleanup
	{
		private readonly struct ThrowRecord
		{
			public PlayerRef Thrower { get; }

			public float Timestamp { get; }

			public ThrowRecord(PlayerRef thrower, float timestamp)
			{
				Thrower = thrower;
				Timestamp = timestamp;
			}
		}

		private readonly KrakenAggressiveThrowConfiguration _configuration;

		private readonly List<ThrowRecord> _throws = new List<ThrowRecord>();

		public int TotalCountInWindow
		{
			get
			{
				PruneExpiredThrows();
				return _throws.Count;
			}
		}

		public event Action OnThrowRecorded;

		public KrakenPlayerThrowTrackerModel(KrakenAggressiveThrowConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void RecordThrow(PlayerRef thrower)
		{
			if (!(thrower == PlayerRef.None) && thrower.IsRealPlayer)
			{
				PruneExpiredThrows();
				_throws.Add(new ThrowRecord(thrower, Time.time));
				this.OnThrowRecorded?.Invoke();
			}
		}

		public bool TryGetTopThrower(out PlayerRef topThrower)
		{
			topThrower = PlayerRef.None;
			PruneExpiredThrows();
			if (_throws.Count == 0)
			{
				return false;
			}
			Dictionary<int, (PlayerRef, int)> dictionary = new Dictionary<int, (PlayerRef, int)>();
			foreach (ThrowRecord @throw in _throws)
			{
				int playerId = @throw.Thrower.PlayerId;
				if (!dictionary.TryGetValue(playerId, out var value))
				{
					value = (@throw.Thrower, 0);
				}
				dictionary[playerId] = (@throw.Thrower, value.Item2 + 1);
			}
			int num = 0;
			foreach (KeyValuePair<int, (PlayerRef, int)> item in dictionary)
			{
				if (item.Value.Item2 > num)
				{
					num = item.Value.Item2;
					topThrower = item.Value.Item1;
				}
			}
			if (topThrower != PlayerRef.None)
			{
				return topThrower.IsRealPlayer;
			}
			return false;
		}

		public void ClearWindow()
		{
			_throws.Clear();
		}

		public void Cleanup()
		{
			ClearWindow();
		}

		private void PruneExpiredThrows()
		{
			float b = ((_configuration != null) ? _configuration.ThrowWindowSeconds : 30f);
			float num = Time.time - Mathf.Max(0.01f, b);
			for (int num2 = _throws.Count - 1; num2 >= 0; num2--)
			{
				if (_throws[num2].Timestamp < num)
				{
					_throws.RemoveAt(num2);
				}
			}
		}
	}
}
