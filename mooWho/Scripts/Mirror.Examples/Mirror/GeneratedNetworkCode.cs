using System;
using System.Runtime.InteropServices;
using Mirror.Discovery;
using Mirror.Examples.CharacterSelection;
using Mirror.Examples.Chat;
using Mirror.Examples.MultipleMatch;
using Mirror.Examples.PickupsDropsChilds;
using UnityEngine;

namespace Mirror
{
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public static class GeneratedNetworkCode
	{
		public static TimeSnapshotMessage _Read_Mirror_002ETimeSnapshotMessage(NetworkReader reader)
		{
			return default(TimeSnapshotMessage);
		}

		public static void _Write_Mirror_002ETimeSnapshotMessage(NetworkWriter writer, TimeSnapshotMessage value)
		{
		}

		public static ReadyMessage _Read_Mirror_002EReadyMessage(NetworkReader reader)
		{
			return default(ReadyMessage);
		}

		public static void _Write_Mirror_002EReadyMessage(NetworkWriter writer, ReadyMessage value)
		{
		}

		public static NotReadyMessage _Read_Mirror_002ENotReadyMessage(NetworkReader reader)
		{
			return default(NotReadyMessage);
		}

		public static void _Write_Mirror_002ENotReadyMessage(NetworkWriter writer, NotReadyMessage value)
		{
		}

		public static AddPlayerMessage _Read_Mirror_002EAddPlayerMessage(NetworkReader reader)
		{
			return default(AddPlayerMessage);
		}

		public static void _Write_Mirror_002EAddPlayerMessage(NetworkWriter writer, AddPlayerMessage value)
		{
		}

		public static SceneMessage _Read_Mirror_002ESceneMessage(NetworkReader reader)
		{
			return new SceneMessage
			{
				sceneName = reader.ReadString(),
				sceneOperation = _Read_Mirror_002ESceneOperation(reader),
				customHandling = reader.ReadBool()
			};
		}

		public static SceneOperation _Read_Mirror_002ESceneOperation(NetworkReader reader)
		{
			return (SceneOperation)NetworkReaderExtensions.ReadByte(reader);
		}

		public static void _Write_Mirror_002ESceneMessage(NetworkWriter writer, SceneMessage value)
		{
			writer.WriteString(value.sceneName);
			_Write_Mirror_002ESceneOperation(writer, value.sceneOperation);
			writer.WriteBool(value.customHandling);
		}

		public static void _Write_Mirror_002ESceneOperation(NetworkWriter writer, SceneOperation value)
		{
			NetworkWriterExtensions.WriteByte(writer, (byte)value);
		}

		public static CommandMessage _Read_Mirror_002ECommandMessage(NetworkReader reader)
		{
			return new CommandMessage
			{
				netId = reader.ReadVarUInt(),
				componentIndex = NetworkReaderExtensions.ReadByte(reader),
				functionHash = reader.ReadUShort(),
				payload = reader.ReadArraySegmentAndSize()
			};
		}

		public static void _Write_Mirror_002ECommandMessage(NetworkWriter writer, CommandMessage value)
		{
			writer.WriteVarUInt(value.netId);
			NetworkWriterExtensions.WriteByte(writer, value.componentIndex);
			writer.WriteUShort(value.functionHash);
			writer.WriteArraySegmentAndSize(value.payload);
		}

		public static RpcMessage _Read_Mirror_002ERpcMessage(NetworkReader reader)
		{
			return new RpcMessage
			{
				netId = reader.ReadVarUInt(),
				componentIndex = NetworkReaderExtensions.ReadByte(reader),
				functionHash = reader.ReadUShort(),
				payload = reader.ReadArraySegmentAndSize()
			};
		}

		public static void _Write_Mirror_002ERpcMessage(NetworkWriter writer, RpcMessage value)
		{
			writer.WriteVarUInt(value.netId);
			NetworkWriterExtensions.WriteByte(writer, value.componentIndex);
			writer.WriteUShort(value.functionHash);
			writer.WriteArraySegmentAndSize(value.payload);
		}

		public static SpawnMessage _Read_Mirror_002ESpawnMessage(NetworkReader reader)
		{
			return new SpawnMessage
			{
				netId = reader.ReadVarUInt(),
				spawnFlags = _Read_Mirror_002ESpawnFlags(reader),
				sceneId = reader.ReadVarULong(),
				assetId = reader.ReadVarUInt(),
				position = reader.ReadVector3(),
				rotation = reader.ReadQuaternion(),
				scale = reader.ReadVector3(),
				payload = reader.ReadArraySegmentAndSize()
			};
		}

		public static SpawnFlags _Read_Mirror_002ESpawnFlags(NetworkReader reader)
		{
			return (SpawnFlags)NetworkReaderExtensions.ReadByte(reader);
		}

		public static void _Write_Mirror_002ESpawnMessage(NetworkWriter writer, SpawnMessage value)
		{
			writer.WriteVarUInt(value.netId);
			_Write_Mirror_002ESpawnFlags(writer, value.spawnFlags);
			writer.WriteVarULong(value.sceneId);
			writer.WriteVarUInt(value.assetId);
			writer.WriteVector3(value.position);
			writer.WriteQuaternion(value.rotation);
			writer.WriteVector3(value.scale);
			writer.WriteArraySegmentAndSize(value.payload);
		}

		public static void _Write_Mirror_002ESpawnFlags(NetworkWriter writer, SpawnFlags value)
		{
			NetworkWriterExtensions.WriteByte(writer, (byte)value);
		}

		public static ChangeOwnerMessage _Read_Mirror_002EChangeOwnerMessage(NetworkReader reader)
		{
			return new ChangeOwnerMessage
			{
				netId = reader.ReadVarUInt(),
				spawnFlags = _Read_Mirror_002ESpawnFlags(reader)
			};
		}

		public static void _Write_Mirror_002EChangeOwnerMessage(NetworkWriter writer, ChangeOwnerMessage value)
		{
			writer.WriteVarUInt(value.netId);
			_Write_Mirror_002ESpawnFlags(writer, value.spawnFlags);
		}

		public static ObjectSpawnStartedMessage _Read_Mirror_002EObjectSpawnStartedMessage(NetworkReader reader)
		{
			return default(ObjectSpawnStartedMessage);
		}

		public static void _Write_Mirror_002EObjectSpawnStartedMessage(NetworkWriter writer, ObjectSpawnStartedMessage value)
		{
		}

		public static ObjectSpawnFinishedMessage _Read_Mirror_002EObjectSpawnFinishedMessage(NetworkReader reader)
		{
			return default(ObjectSpawnFinishedMessage);
		}

		public static void _Write_Mirror_002EObjectSpawnFinishedMessage(NetworkWriter writer, ObjectSpawnFinishedMessage value)
		{
		}

		public static ObjectDestroyMessage _Read_Mirror_002EObjectDestroyMessage(NetworkReader reader)
		{
			return new ObjectDestroyMessage
			{
				netId = reader.ReadVarUInt()
			};
		}

		public static void _Write_Mirror_002EObjectDestroyMessage(NetworkWriter writer, ObjectDestroyMessage value)
		{
			writer.WriteVarUInt(value.netId);
		}

		public static ObjectHideMessage _Read_Mirror_002EObjectHideMessage(NetworkReader reader)
		{
			return new ObjectHideMessage
			{
				netId = reader.ReadVarUInt()
			};
		}

		public static void _Write_Mirror_002EObjectHideMessage(NetworkWriter writer, ObjectHideMessage value)
		{
			writer.WriteVarUInt(value.netId);
		}

		public static EntityStateMessage _Read_Mirror_002EEntityStateMessage(NetworkReader reader)
		{
			return new EntityStateMessage
			{
				netId = reader.ReadVarUInt(),
				payload = reader.ReadArraySegmentAndSize()
			};
		}

		public static void _Write_Mirror_002EEntityStateMessage(NetworkWriter writer, EntityStateMessage value)
		{
			writer.WriteVarUInt(value.netId);
			writer.WriteArraySegmentAndSize(value.payload);
		}

		public static NetworkPingMessage _Read_Mirror_002ENetworkPingMessage(NetworkReader reader)
		{
			return new NetworkPingMessage
			{
				localTime = reader.ReadDouble(),
				predictedTimeAdjusted = reader.ReadDouble()
			};
		}

		public static void _Write_Mirror_002ENetworkPingMessage(NetworkWriter writer, NetworkPingMessage value)
		{
			writer.WriteDouble(value.localTime);
			writer.WriteDouble(value.predictedTimeAdjusted);
		}

		public static NetworkPongMessage _Read_Mirror_002ENetworkPongMessage(NetworkReader reader)
		{
			return new NetworkPongMessage
			{
				localTime = reader.ReadDouble(),
				predictionErrorUnadjusted = reader.ReadDouble(),
				predictionErrorAdjusted = reader.ReadDouble()
			};
		}

		public static void _Write_Mirror_002ENetworkPongMessage(NetworkWriter writer, NetworkPongMessage value)
		{
			writer.WriteDouble(value.localTime);
			writer.WriteDouble(value.predictionErrorUnadjusted);
			writer.WriteDouble(value.predictionErrorAdjusted);
		}

		public static ServerRequest _Read_Mirror_002EDiscovery_002EServerRequest(NetworkReader reader)
		{
			return default(ServerRequest);
		}

		public static void _Write_Mirror_002EDiscovery_002EServerRequest(NetworkWriter writer, ServerRequest value)
		{
		}

		public static ServerResponse _Read_Mirror_002EDiscovery_002EServerResponse(NetworkReader reader)
		{
			return new ServerResponse
			{
				uri = reader.ReadUri(),
				serverId = reader.ReadVarLong()
			};
		}

		public static void _Write_Mirror_002EDiscovery_002EServerResponse(NetworkWriter writer, ServerResponse value)
		{
			writer.WriteUri(value.uri);
			writer.WriteVarLong(value.serverId);
		}

		public static ServerMatchMessage _Read_Mirror_002EExamples_002EMultipleMatch_002EServerMatchMessage(NetworkReader reader)
		{
			return new ServerMatchMessage
			{
				serverMatchOperation = _Read_Mirror_002EExamples_002EMultipleMatch_002EServerMatchOperation(reader),
				matchId = reader.ReadGuid()
			};
		}

		public static ServerMatchOperation _Read_Mirror_002EExamples_002EMultipleMatch_002EServerMatchOperation(NetworkReader reader)
		{
			return (ServerMatchOperation)NetworkReaderExtensions.ReadByte(reader);
		}

		public static void _Write_Mirror_002EExamples_002EMultipleMatch_002EServerMatchMessage(NetworkWriter writer, ServerMatchMessage value)
		{
			_Write_Mirror_002EExamples_002EMultipleMatch_002EServerMatchOperation(writer, value.serverMatchOperation);
			writer.WriteGuid(value.matchId);
		}

		public static void _Write_Mirror_002EExamples_002EMultipleMatch_002EServerMatchOperation(NetworkWriter writer, ServerMatchOperation value)
		{
			NetworkWriterExtensions.WriteByte(writer, (byte)value);
		}

		public static ClientMatchMessage _Read_Mirror_002EExamples_002EMultipleMatch_002EClientMatchMessage(NetworkReader reader)
		{
			return new ClientMatchMessage
			{
				clientMatchOperation = _Read_Mirror_002EExamples_002EMultipleMatch_002EClientMatchOperation(reader),
				matchId = reader.ReadGuid(),
				matchInfos = _Read_Mirror_002EExamples_002EMultipleMatch_002EMatchInfo_005B_005D(reader),
				playerInfos = _Read_Mirror_002EExamples_002EMultipleMatch_002EPlayerInfo_005B_005D(reader)
			};
		}

		public static ClientMatchOperation _Read_Mirror_002EExamples_002EMultipleMatch_002EClientMatchOperation(NetworkReader reader)
		{
			return (ClientMatchOperation)NetworkReaderExtensions.ReadByte(reader);
		}

		public static MatchInfo[] _Read_Mirror_002EExamples_002EMultipleMatch_002EMatchInfo_005B_005D(NetworkReader reader)
		{
			return reader.ReadArray<MatchInfo>();
		}

		public static MatchInfo _Read_Mirror_002EExamples_002EMultipleMatch_002EMatchInfo(NetworkReader reader)
		{
			return new MatchInfo
			{
				matchId = reader.ReadGuid(),
				players = NetworkReaderExtensions.ReadByte(reader),
				maxPlayers = NetworkReaderExtensions.ReadByte(reader)
			};
		}

		public static PlayerInfo[] _Read_Mirror_002EExamples_002EMultipleMatch_002EPlayerInfo_005B_005D(NetworkReader reader)
		{
			return reader.ReadArray<PlayerInfo>();
		}

		public static PlayerInfo _Read_Mirror_002EExamples_002EMultipleMatch_002EPlayerInfo(NetworkReader reader)
		{
			return new PlayerInfo
			{
				playerIndex = reader.ReadVarInt(),
				ready = reader.ReadBool(),
				matchId = reader.ReadGuid()
			};
		}

		public static void _Write_Mirror_002EExamples_002EMultipleMatch_002EClientMatchMessage(NetworkWriter writer, ClientMatchMessage value)
		{
			_Write_Mirror_002EExamples_002EMultipleMatch_002EClientMatchOperation(writer, value.clientMatchOperation);
			writer.WriteGuid(value.matchId);
			_Write_Mirror_002EExamples_002EMultipleMatch_002EMatchInfo_005B_005D(writer, value.matchInfos);
			_Write_Mirror_002EExamples_002EMultipleMatch_002EPlayerInfo_005B_005D(writer, value.playerInfos);
		}

		public static void _Write_Mirror_002EExamples_002EMultipleMatch_002EClientMatchOperation(NetworkWriter writer, ClientMatchOperation value)
		{
			NetworkWriterExtensions.WriteByte(writer, (byte)value);
		}

		public static void _Write_Mirror_002EExamples_002EMultipleMatch_002EMatchInfo_005B_005D(NetworkWriter writer, MatchInfo[] value)
		{
			writer.WriteArray(value);
		}

		public static void _Write_Mirror_002EExamples_002EMultipleMatch_002EMatchInfo(NetworkWriter writer, MatchInfo value)
		{
			writer.WriteGuid(value.matchId);
			NetworkWriterExtensions.WriteByte(writer, value.players);
			NetworkWriterExtensions.WriteByte(writer, value.maxPlayers);
		}

		public static void _Write_Mirror_002EExamples_002EMultipleMatch_002EPlayerInfo_005B_005D(NetworkWriter writer, PlayerInfo[] value)
		{
			writer.WriteArray(value);
		}

		public static void _Write_Mirror_002EExamples_002EMultipleMatch_002EPlayerInfo(NetworkWriter writer, PlayerInfo value)
		{
			writer.WriteVarInt(value.playerIndex);
			writer.WriteBool(value.ready);
			writer.WriteGuid(value.matchId);
		}

		public static ChatAuthenticator.AuthRequestMessage _Read_Mirror_002EExamples_002EChat_002EChatAuthenticator_002FAuthRequestMessage(NetworkReader reader)
		{
			return new ChatAuthenticator.AuthRequestMessage
			{
				authUsername = reader.ReadString()
			};
		}

		public static void _Write_Mirror_002EExamples_002EChat_002EChatAuthenticator_002FAuthRequestMessage(NetworkWriter writer, ChatAuthenticator.AuthRequestMessage value)
		{
			writer.WriteString(value.authUsername);
		}

		public static ChatAuthenticator.AuthResponseMessage _Read_Mirror_002EExamples_002EChat_002EChatAuthenticator_002FAuthResponseMessage(NetworkReader reader)
		{
			return new ChatAuthenticator.AuthResponseMessage
			{
				code = NetworkReaderExtensions.ReadByte(reader),
				message = reader.ReadString()
			};
		}

		public static void _Write_Mirror_002EExamples_002EChat_002EChatAuthenticator_002FAuthResponseMessage(NetworkWriter writer, ChatAuthenticator.AuthResponseMessage value)
		{
			NetworkWriterExtensions.WriteByte(writer, value.code);
			writer.WriteString(value.message);
		}

		public static NetworkManagerCharacterSelection.CreateCharacterMessage _Read_Mirror_002EExamples_002ECharacterSelection_002ENetworkManagerCharacterSelection_002FCreateCharacterMessage(NetworkReader reader)
		{
			return new NetworkManagerCharacterSelection.CreateCharacterMessage
			{
				playerName = reader.ReadString(),
				characterNumber = reader.ReadVarInt(),
				characterColour = reader.ReadColor()
			};
		}

		public static void _Write_Mirror_002EExamples_002ECharacterSelection_002ENetworkManagerCharacterSelection_002FCreateCharacterMessage(NetworkWriter writer, NetworkManagerCharacterSelection.CreateCharacterMessage value)
		{
			writer.WriteString(value.playerName);
			writer.WriteVarInt(value.characterNumber);
			writer.WriteColor(value.characterColour);
		}

		public static NetworkManagerCharacterSelection.ReplaceCharacterMessage _Read_Mirror_002EExamples_002ECharacterSelection_002ENetworkManagerCharacterSelection_002FReplaceCharacterMessage(NetworkReader reader)
		{
			return new NetworkManagerCharacterSelection.ReplaceCharacterMessage
			{
				createCharacterMessage = _Read_Mirror_002EExamples_002ECharacterSelection_002ENetworkManagerCharacterSelection_002FCreateCharacterMessage(reader)
			};
		}

		public static void _Write_Mirror_002EExamples_002ECharacterSelection_002ENetworkManagerCharacterSelection_002FReplaceCharacterMessage(NetworkWriter writer, NetworkManagerCharacterSelection.ReplaceCharacterMessage value)
		{
			_Write_Mirror_002EExamples_002ECharacterSelection_002ENetworkManagerCharacterSelection_002FCreateCharacterMessage(writer, value.createCharacterMessage);
		}

		public static void _Write_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItem(NetworkWriter writer, EquippedItem value)
		{
			NetworkWriterExtensions.WriteByte(writer, (byte)value);
		}

		public static EquippedItem _Read_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItem(NetworkReader reader)
		{
			return (EquippedItem)NetworkReaderExtensions.ReadByte(reader);
		}

		public static void _Write_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItemConfig(NetworkWriter writer, EquippedItemConfig value)
		{
			NetworkWriterExtensions.WriteByte(writer, value.usages);
			NetworkWriterExtensions.WriteByte(writer, value.maxUsages);
		}

		public static EquippedItemConfig _Read_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItemConfig(NetworkReader reader)
		{
			return new EquippedItemConfig
			{
				usages = NetworkReaderExtensions.ReadByte(reader),
				maxUsages = NetworkReaderExtensions.ReadByte(reader)
			};
		}

		public static MatchPlayerData _Read_Mirror_002EExamples_002EMultipleMatch_002EMatchPlayerData(NetworkReader reader)
		{
			return new MatchPlayerData
			{
				playerIndex = reader.ReadVarInt(),
				wins = reader.ReadVarInt(),
				currentScore = _Read_Mirror_002EExamples_002EMultipleMatch_002ECellValue(reader)
			};
		}

		public static CellValue _Read_Mirror_002EExamples_002EMultipleMatch_002ECellValue(NetworkReader reader)
		{
			return (CellValue)reader.ReadUShort();
		}

		public static void _Write_Mirror_002EExamples_002EMultipleMatch_002EMatchPlayerData(NetworkWriter writer, MatchPlayerData value)
		{
			writer.WriteVarInt(value.playerIndex);
			writer.WriteVarInt(value.wins);
			_Write_Mirror_002EExamples_002EMultipleMatch_002ECellValue(writer, value.currentScore);
		}

		public static void _Write_Mirror_002EExamples_002EMultipleMatch_002ECellValue(NetworkWriter writer, CellValue value)
		{
			writer.WriteUShort((ushort)value);
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void InitReadWriters()
		{
			Writer<byte>.write = NetworkWriterExtensions.WriteByte;
			Writer<byte?>.write = NetworkWriterExtensions.WriteByteNullable;
			Writer<sbyte>.write = NetworkWriterExtensions.WriteSByte;
			Writer<sbyte?>.write = NetworkWriterExtensions.WriteSByteNullable;
			Writer<char>.write = NetworkWriterExtensions.WriteChar;
			Writer<char?>.write = NetworkWriterExtensions.WriteCharNullable;
			Writer<bool>.write = NetworkWriterExtensions.WriteBool;
			Writer<bool?>.write = NetworkWriterExtensions.WriteBoolNullable;
			Writer<short>.write = NetworkWriterExtensions.WriteShort;
			Writer<short?>.write = NetworkWriterExtensions.WriteShortNullable;
			Writer<ushort>.write = NetworkWriterExtensions.WriteUShort;
			Writer<ushort?>.write = NetworkWriterExtensions.WriteUShortNullable;
			Writer<int>.write = NetworkWriterExtensions.WriteVarInt;
			Writer<int?>.write = NetworkWriterExtensions.WriteIntNullable;
			Writer<uint>.write = NetworkWriterExtensions.WriteVarUInt;
			Writer<uint?>.write = NetworkWriterExtensions.WriteUIntNullable;
			Writer<long>.write = NetworkWriterExtensions.WriteVarLong;
			Writer<long?>.write = NetworkWriterExtensions.WriteLongNullable;
			Writer<ulong>.write = NetworkWriterExtensions.WriteVarULong;
			Writer<ulong?>.write = NetworkWriterExtensions.WriteULongNullable;
			Writer<float>.write = NetworkWriterExtensions.WriteFloat;
			Writer<float?>.write = NetworkWriterExtensions.WriteFloatNullable;
			Writer<double>.write = NetworkWriterExtensions.WriteDouble;
			Writer<double?>.write = NetworkWriterExtensions.WriteDoubleNullable;
			Writer<decimal>.write = NetworkWriterExtensions.WriteDecimal;
			Writer<decimal?>.write = NetworkWriterExtensions.WriteDecimalNullable;
			Writer<Half>.write = NetworkWriterExtensions.WriteHalf;
			Writer<string>.write = NetworkWriterExtensions.WriteString;
			Writer<byte[]>.write = NetworkWriterExtensions.WriteBytesAndSize;
			Writer<ArraySegment<byte>>.write = NetworkWriterExtensions.WriteArraySegmentAndSize;
			Writer<Vector2>.write = NetworkWriterExtensions.WriteVector2;
			Writer<Vector2?>.write = NetworkWriterExtensions.WriteVector2Nullable;
			Writer<Vector3>.write = NetworkWriterExtensions.WriteVector3;
			Writer<Vector3?>.write = NetworkWriterExtensions.WriteVector3Nullable;
			Writer<Vector4>.write = NetworkWriterExtensions.WriteVector4;
			Writer<Vector4?>.write = NetworkWriterExtensions.WriteVector4Nullable;
			Writer<Vector2Int>.write = NetworkWriterExtensions.WriteVector2Int;
			Writer<Vector2Int?>.write = NetworkWriterExtensions.WriteVector2IntNullable;
			Writer<Vector3Int>.write = NetworkWriterExtensions.WriteVector3Int;
			Writer<Vector3Int?>.write = NetworkWriterExtensions.WriteVector3IntNullable;
			Writer<Color>.write = NetworkWriterExtensions.WriteColor;
			Writer<Color?>.write = NetworkWriterExtensions.WriteColorNullable;
			Writer<Color32>.write = NetworkWriterExtensions.WriteColor32;
			Writer<Color32?>.write = NetworkWriterExtensions.WriteColor32Nullable;
			Writer<Quaternion>.write = NetworkWriterExtensions.WriteQuaternion;
			Writer<Quaternion?>.write = NetworkWriterExtensions.WriteQuaternionNullable;
			Writer<Rect>.write = NetworkWriterExtensions.WriteRect;
			Writer<Rect?>.write = NetworkWriterExtensions.WriteRectNullable;
			Writer<Plane>.write = NetworkWriterExtensions.WritePlane;
			Writer<Plane?>.write = NetworkWriterExtensions.WritePlaneNullable;
			Writer<Ray>.write = NetworkWriterExtensions.WriteRay;
			Writer<Ray?>.write = NetworkWriterExtensions.WriteRayNullable;
			Writer<LayerMask>.write = NetworkWriterExtensions.WriteLayerMask;
			Writer<LayerMask?>.write = NetworkWriterExtensions.WriteLayerMaskNullable;
			Writer<Matrix4x4>.write = NetworkWriterExtensions.WriteMatrix4x4;
			Writer<Matrix4x4?>.write = NetworkWriterExtensions.WriteMatrix4x4Nullable;
			Writer<Guid>.write = NetworkWriterExtensions.WriteGuid;
			Writer<Guid?>.write = NetworkWriterExtensions.WriteGuidNullable;
			Writer<NetworkIdentity>.write = NetworkWriterExtensions.WriteNetworkIdentity;
			Writer<NetworkBehaviour>.write = NetworkWriterExtensions.WriteNetworkBehaviour;
			Writer<Transform>.write = NetworkWriterExtensions.WriteTransform;
			Writer<GameObject>.write = NetworkWriterExtensions.WriteGameObject;
			Writer<Uri>.write = NetworkWriterExtensions.WriteUri;
			Writer<Texture2D>.write = NetworkWriterExtensions.WriteTexture2D;
			Writer<Sprite>.write = NetworkWriterExtensions.WriteSprite;
			Writer<DateTime>.write = NetworkWriterExtensions.WriteDateTime;
			Writer<DateTime?>.write = NetworkWriterExtensions.WriteDateTimeNullable;
			Writer<TimeSnapshotMessage>.write = _Write_Mirror_002ETimeSnapshotMessage;
			Writer<ReadyMessage>.write = _Write_Mirror_002EReadyMessage;
			Writer<NotReadyMessage>.write = _Write_Mirror_002ENotReadyMessage;
			Writer<AddPlayerMessage>.write = _Write_Mirror_002EAddPlayerMessage;
			Writer<SceneMessage>.write = _Write_Mirror_002ESceneMessage;
			Writer<SceneOperation>.write = _Write_Mirror_002ESceneOperation;
			Writer<CommandMessage>.write = _Write_Mirror_002ECommandMessage;
			Writer<RpcMessage>.write = _Write_Mirror_002ERpcMessage;
			Writer<SpawnMessage>.write = _Write_Mirror_002ESpawnMessage;
			Writer<SpawnFlags>.write = _Write_Mirror_002ESpawnFlags;
			Writer<ChangeOwnerMessage>.write = _Write_Mirror_002EChangeOwnerMessage;
			Writer<ObjectSpawnStartedMessage>.write = _Write_Mirror_002EObjectSpawnStartedMessage;
			Writer<ObjectSpawnFinishedMessage>.write = _Write_Mirror_002EObjectSpawnFinishedMessage;
			Writer<ObjectDestroyMessage>.write = _Write_Mirror_002EObjectDestroyMessage;
			Writer<ObjectHideMessage>.write = _Write_Mirror_002EObjectHideMessage;
			Writer<EntityStateMessage>.write = _Write_Mirror_002EEntityStateMessage;
			Writer<NetworkPingMessage>.write = _Write_Mirror_002ENetworkPingMessage;
			Writer<NetworkPongMessage>.write = _Write_Mirror_002ENetworkPongMessage;
			Writer<SyncData>.write = SyncDataReaderWriter.WriteSyncData;
			Writer<PredictedSyncData>.write = PredictedSyncDataReadWrite.WritePredictedSyncData;
			Writer<ServerRequest>.write = _Write_Mirror_002EDiscovery_002EServerRequest;
			Writer<ServerResponse>.write = _Write_Mirror_002EDiscovery_002EServerResponse;
			Writer<ServerMatchMessage>.write = _Write_Mirror_002EExamples_002EMultipleMatch_002EServerMatchMessage;
			Writer<ServerMatchOperation>.write = _Write_Mirror_002EExamples_002EMultipleMatch_002EServerMatchOperation;
			Writer<ClientMatchMessage>.write = _Write_Mirror_002EExamples_002EMultipleMatch_002EClientMatchMessage;
			Writer<ClientMatchOperation>.write = _Write_Mirror_002EExamples_002EMultipleMatch_002EClientMatchOperation;
			Writer<MatchInfo[]>.write = _Write_Mirror_002EExamples_002EMultipleMatch_002EMatchInfo_005B_005D;
			Writer<MatchInfo>.write = _Write_Mirror_002EExamples_002EMultipleMatch_002EMatchInfo;
			Writer<PlayerInfo[]>.write = _Write_Mirror_002EExamples_002EMultipleMatch_002EPlayerInfo_005B_005D;
			Writer<PlayerInfo>.write = _Write_Mirror_002EExamples_002EMultipleMatch_002EPlayerInfo;
			Writer<ChatAuthenticator.AuthRequestMessage>.write = _Write_Mirror_002EExamples_002EChat_002EChatAuthenticator_002FAuthRequestMessage;
			Writer<ChatAuthenticator.AuthResponseMessage>.write = _Write_Mirror_002EExamples_002EChat_002EChatAuthenticator_002FAuthResponseMessage;
			Writer<NetworkManagerCharacterSelection.CreateCharacterMessage>.write = _Write_Mirror_002EExamples_002ECharacterSelection_002ENetworkManagerCharacterSelection_002FCreateCharacterMessage;
			Writer<NetworkManagerCharacterSelection.ReplaceCharacterMessage>.write = _Write_Mirror_002EExamples_002ECharacterSelection_002ENetworkManagerCharacterSelection_002FReplaceCharacterMessage;
			Writer<EquippedItem>.write = _Write_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItem;
			Writer<EquippedItemConfig>.write = _Write_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItemConfig;
			Writer<MatchPlayerData>.write = _Write_Mirror_002EExamples_002EMultipleMatch_002EMatchPlayerData;
			Writer<CellValue>.write = _Write_Mirror_002EExamples_002EMultipleMatch_002ECellValue;
			Reader<byte>.read = NetworkReaderExtensions.ReadByte;
			Reader<byte?>.read = NetworkReaderExtensions.ReadByteNullable;
			Reader<sbyte>.read = NetworkReaderExtensions.ReadSByte;
			Reader<sbyte?>.read = NetworkReaderExtensions.ReadSByteNullable;
			Reader<char>.read = NetworkReaderExtensions.ReadChar;
			Reader<char?>.read = NetworkReaderExtensions.ReadCharNullable;
			Reader<bool>.read = NetworkReaderExtensions.ReadBool;
			Reader<bool?>.read = NetworkReaderExtensions.ReadBoolNullable;
			Reader<short>.read = NetworkReaderExtensions.ReadShort;
			Reader<short?>.read = NetworkReaderExtensions.ReadShortNullable;
			Reader<ushort>.read = NetworkReaderExtensions.ReadUShort;
			Reader<ushort?>.read = NetworkReaderExtensions.ReadUShortNullable;
			Reader<int>.read = NetworkReaderExtensions.ReadVarInt;
			Reader<int?>.read = NetworkReaderExtensions.ReadIntNullable;
			Reader<uint>.read = NetworkReaderExtensions.ReadVarUInt;
			Reader<uint?>.read = NetworkReaderExtensions.ReadUIntNullable;
			Reader<long>.read = NetworkReaderExtensions.ReadVarLong;
			Reader<long?>.read = NetworkReaderExtensions.ReadLongNullable;
			Reader<ulong>.read = NetworkReaderExtensions.ReadVarULong;
			Reader<ulong?>.read = NetworkReaderExtensions.ReadULongNullable;
			Reader<float>.read = NetworkReaderExtensions.ReadFloat;
			Reader<float?>.read = NetworkReaderExtensions.ReadFloatNullable;
			Reader<double>.read = NetworkReaderExtensions.ReadDouble;
			Reader<double?>.read = NetworkReaderExtensions.ReadDoubleNullable;
			Reader<decimal>.read = NetworkReaderExtensions.ReadDecimal;
			Reader<decimal?>.read = NetworkReaderExtensions.ReadDecimalNullable;
			Reader<Half>.read = NetworkReaderExtensions.ReadHalf;
			Reader<string>.read = NetworkReaderExtensions.ReadString;
			Reader<byte[]>.read = NetworkReaderExtensions.ReadBytesAndSize;
			Reader<ArraySegment<byte>>.read = NetworkReaderExtensions.ReadArraySegmentAndSize;
			Reader<Vector2>.read = NetworkReaderExtensions.ReadVector2;
			Reader<Vector2?>.read = NetworkReaderExtensions.ReadVector2Nullable;
			Reader<Vector3>.read = NetworkReaderExtensions.ReadVector3;
			Reader<Vector3?>.read = NetworkReaderExtensions.ReadVector3Nullable;
			Reader<Vector4>.read = NetworkReaderExtensions.ReadVector4;
			Reader<Vector4?>.read = NetworkReaderExtensions.ReadVector4Nullable;
			Reader<Vector2Int>.read = NetworkReaderExtensions.ReadVector2Int;
			Reader<Vector2Int?>.read = NetworkReaderExtensions.ReadVector2IntNullable;
			Reader<Vector3Int>.read = NetworkReaderExtensions.ReadVector3Int;
			Reader<Vector3Int?>.read = NetworkReaderExtensions.ReadVector3IntNullable;
			Reader<Color>.read = NetworkReaderExtensions.ReadColor;
			Reader<Color?>.read = NetworkReaderExtensions.ReadColorNullable;
			Reader<Color32>.read = NetworkReaderExtensions.ReadColor32;
			Reader<Color32?>.read = NetworkReaderExtensions.ReadColor32Nullable;
			Reader<Quaternion>.read = NetworkReaderExtensions.ReadQuaternion;
			Reader<Quaternion?>.read = NetworkReaderExtensions.ReadQuaternionNullable;
			Reader<Rect>.read = NetworkReaderExtensions.ReadRect;
			Reader<Rect?>.read = NetworkReaderExtensions.ReadRectNullable;
			Reader<Plane>.read = NetworkReaderExtensions.ReadPlane;
			Reader<Plane?>.read = NetworkReaderExtensions.ReadPlaneNullable;
			Reader<Ray>.read = NetworkReaderExtensions.ReadRay;
			Reader<Ray?>.read = NetworkReaderExtensions.ReadRayNullable;
			Reader<LayerMask>.read = NetworkReaderExtensions.ReadLayerMask;
			Reader<LayerMask?>.read = NetworkReaderExtensions.ReadLayerMaskNullable;
			Reader<Matrix4x4>.read = NetworkReaderExtensions.ReadMatrix4x4;
			Reader<Matrix4x4?>.read = NetworkReaderExtensions.ReadMatrix4x4Nullable;
			Reader<Guid>.read = NetworkReaderExtensions.ReadGuid;
			Reader<Guid?>.read = NetworkReaderExtensions.ReadGuidNullable;
			Reader<NetworkIdentity>.read = NetworkReaderExtensions.ReadNetworkIdentity;
			Reader<NetworkBehaviour>.read = NetworkReaderExtensions.ReadNetworkBehaviour;
			Reader<NetworkBehaviourSyncVar>.read = NetworkReaderExtensions.ReadNetworkBehaviourSyncVar;
			Reader<Transform>.read = NetworkReaderExtensions.ReadTransform;
			Reader<GameObject>.read = NetworkReaderExtensions.ReadGameObject;
			Reader<Uri>.read = NetworkReaderExtensions.ReadUri;
			Reader<Texture2D>.read = NetworkReaderExtensions.ReadTexture2D;
			Reader<Sprite>.read = NetworkReaderExtensions.ReadSprite;
			Reader<DateTime>.read = NetworkReaderExtensions.ReadDateTime;
			Reader<DateTime?>.read = NetworkReaderExtensions.ReadDateTimeNullable;
			Reader<TimeSnapshotMessage>.read = _Read_Mirror_002ETimeSnapshotMessage;
			Reader<ReadyMessage>.read = _Read_Mirror_002EReadyMessage;
			Reader<NotReadyMessage>.read = _Read_Mirror_002ENotReadyMessage;
			Reader<AddPlayerMessage>.read = _Read_Mirror_002EAddPlayerMessage;
			Reader<SceneMessage>.read = _Read_Mirror_002ESceneMessage;
			Reader<SceneOperation>.read = _Read_Mirror_002ESceneOperation;
			Reader<CommandMessage>.read = _Read_Mirror_002ECommandMessage;
			Reader<RpcMessage>.read = _Read_Mirror_002ERpcMessage;
			Reader<SpawnMessage>.read = _Read_Mirror_002ESpawnMessage;
			Reader<SpawnFlags>.read = _Read_Mirror_002ESpawnFlags;
			Reader<ChangeOwnerMessage>.read = _Read_Mirror_002EChangeOwnerMessage;
			Reader<ObjectSpawnStartedMessage>.read = _Read_Mirror_002EObjectSpawnStartedMessage;
			Reader<ObjectSpawnFinishedMessage>.read = _Read_Mirror_002EObjectSpawnFinishedMessage;
			Reader<ObjectDestroyMessage>.read = _Read_Mirror_002EObjectDestroyMessage;
			Reader<ObjectHideMessage>.read = _Read_Mirror_002EObjectHideMessage;
			Reader<EntityStateMessage>.read = _Read_Mirror_002EEntityStateMessage;
			Reader<NetworkPingMessage>.read = _Read_Mirror_002ENetworkPingMessage;
			Reader<NetworkPongMessage>.read = _Read_Mirror_002ENetworkPongMessage;
			Reader<SyncData>.read = SyncDataReaderWriter.ReadSyncData;
			Reader<PredictedSyncData>.read = PredictedSyncDataReadWrite.ReadPredictedSyncData;
			Reader<ServerRequest>.read = _Read_Mirror_002EDiscovery_002EServerRequest;
			Reader<ServerResponse>.read = _Read_Mirror_002EDiscovery_002EServerResponse;
			Reader<ServerMatchMessage>.read = _Read_Mirror_002EExamples_002EMultipleMatch_002EServerMatchMessage;
			Reader<ServerMatchOperation>.read = _Read_Mirror_002EExamples_002EMultipleMatch_002EServerMatchOperation;
			Reader<ClientMatchMessage>.read = _Read_Mirror_002EExamples_002EMultipleMatch_002EClientMatchMessage;
			Reader<ClientMatchOperation>.read = _Read_Mirror_002EExamples_002EMultipleMatch_002EClientMatchOperation;
			Reader<MatchInfo[]>.read = _Read_Mirror_002EExamples_002EMultipleMatch_002EMatchInfo_005B_005D;
			Reader<MatchInfo>.read = _Read_Mirror_002EExamples_002EMultipleMatch_002EMatchInfo;
			Reader<PlayerInfo[]>.read = _Read_Mirror_002EExamples_002EMultipleMatch_002EPlayerInfo_005B_005D;
			Reader<PlayerInfo>.read = _Read_Mirror_002EExamples_002EMultipleMatch_002EPlayerInfo;
			Reader<ChatAuthenticator.AuthRequestMessage>.read = _Read_Mirror_002EExamples_002EChat_002EChatAuthenticator_002FAuthRequestMessage;
			Reader<ChatAuthenticator.AuthResponseMessage>.read = _Read_Mirror_002EExamples_002EChat_002EChatAuthenticator_002FAuthResponseMessage;
			Reader<NetworkManagerCharacterSelection.CreateCharacterMessage>.read = _Read_Mirror_002EExamples_002ECharacterSelection_002ENetworkManagerCharacterSelection_002FCreateCharacterMessage;
			Reader<NetworkManagerCharacterSelection.ReplaceCharacterMessage>.read = _Read_Mirror_002EExamples_002ECharacterSelection_002ENetworkManagerCharacterSelection_002FReplaceCharacterMessage;
			Reader<EquippedItem>.read = _Read_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItem;
			Reader<EquippedItemConfig>.read = _Read_Mirror_002EExamples_002EPickupsDropsChilds_002EEquippedItemConfig;
			Reader<MatchPlayerData>.read = _Read_Mirror_002EExamples_002EMultipleMatch_002EMatchPlayerData;
			Reader<CellValue>.read = _Read_Mirror_002EExamples_002EMultipleMatch_002ECellValue;
		}
	}
}
