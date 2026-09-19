using Features.AudioServiceModule.Scripts;
using UnityEngine;

namespace Features.EntitiesSoundOcclusionModule.Scripts.EnemiesSoundOcclusion
{
	public readonly struct HeardSound
	{
		public Vector3 Position { get; }

		public float Loudness { get; }

		public int SourceId { get; }

		public string SoundPath { get; }

		public SoundSourceKind SourceKind { get; }

		public int PlayerId { get; }

		public bool IsPlayerSource
		{
			get
			{
				if (SourceKind == SoundSourceKind.Player)
				{
					return PlayerId >= 0;
				}
				return false;
			}
		}

		public HeardSound(Vector3 position, float loudness, int sourceId, SoundSourceKind sourceKind = SoundSourceKind.Unknown, int playerId = -1, string soundPath = null)
		{
			Position = position;
			Loudness = loudness;
			SourceId = sourceId;
			SoundPath = soundPath;
			SourceKind = sourceKind;
			PlayerId = ((sourceKind == SoundSourceKind.Player) ? playerId : (-1));
		}
	}
}
