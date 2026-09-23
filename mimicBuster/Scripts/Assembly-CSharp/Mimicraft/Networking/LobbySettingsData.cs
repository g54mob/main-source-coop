using System;
using Unity.Collections;
using Unity.Netcode;

namespace Mimicraft.Networking
{
	public struct LobbySettingsData : INetworkSerializable, IEquatable<LobbySettingsData>
	{
		public FixedString64Bytes LobbyName;

		public int PrepSeconds;

		public int HuntSeconds;

		public int RoundEndSeconds;

		public int MinHiders;

		public int MinHunters;

		public int TauntIntervalSeconds;

		public int SelfDamagePercent;

		public int HunterSharePercent;

		public FixedString32Bytes MapId;

		public FixedString32Bytes ModeId;

		public bool HasPassword;

		public int MinPlayers;

		public int ScoreLimit;

		public int TimeLimitSeconds;

		public int ExtraWarmupSeconds;

		public int RespawnSeconds;

		public int MaxPlayers;

		public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
		{
			serializer.SerializeValue(ref LobbyName, default(FastBufferWriter.ForFixedStrings));
			serializer.SerializeValue(ref PrepSeconds, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref HuntSeconds, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref RoundEndSeconds, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref MinHiders, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref MinHunters, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref TauntIntervalSeconds, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref SelfDamagePercent, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref HunterSharePercent, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref MapId, default(FastBufferWriter.ForFixedStrings));
			serializer.SerializeValue(ref ModeId, default(FastBufferWriter.ForFixedStrings));
			serializer.SerializeValue(ref HasPassword, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref MinPlayers, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref ScoreLimit, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref TimeLimitSeconds, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref ExtraWarmupSeconds, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref RespawnSeconds, default(FastBufferWriter.ForPrimitives));
			serializer.SerializeValue(ref MaxPlayers, default(FastBufferWriter.ForPrimitives));
		}

		public bool Equals(LobbySettingsData other)
		{
			if (LobbyName.Equals(other.LobbyName) && PrepSeconds == other.PrepSeconds && HuntSeconds == other.HuntSeconds && RoundEndSeconds == other.RoundEndSeconds && MinHiders == other.MinHiders && MinHunters == other.MinHunters && TauntIntervalSeconds == other.TauntIntervalSeconds && SelfDamagePercent == other.SelfDamagePercent && HunterSharePercent == other.HunterSharePercent && MapId.Equals(other.MapId) && ModeId.Equals(other.ModeId) && HasPassword == other.HasPassword && MinPlayers == other.MinPlayers && ScoreLimit == other.ScoreLimit && TimeLimitSeconds == other.TimeLimitSeconds && ExtraWarmupSeconds == other.ExtraWarmupSeconds && RespawnSeconds == other.RespawnSeconds)
			{
				return MaxPlayers == other.MaxPlayers;
			}
			return false;
		}
	}
}
