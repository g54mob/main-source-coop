using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.Audio.BroAdapter;
using FIMSpace.FProceduralAnimation;
using Mirror.Discovery;
using NWH.VehiclePhysics2;
using NomadDrive.Features.Consumables;
using NomadDrive.Features.Cooking;
using NomadDrive.Features.Furnitures;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.Objectives.Networking;
using NomadDrive.Features.Player;
using NomadDrive.Features.Player.PlayerStateMachine;
using NomadDrive.Features.Vehicle.Collision;
using NomadDrive.Features.Vehicle.Networking;
using NomadDrive.Features.WorldGeneration.ObjectSpawning;
using NomadDrive.Managers.GameTime;
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

		public static void _Write_NomadDrive_002EManagers_002EGameTime_002EDayTimePart(NetworkWriter writer, DayTimePart value)
		{
			writer.WriteVarInt(value.Hours);
			writer.WriteVarInt(value.Minutes);
			writer.WriteVarInt(value.Seconds);
		}

		public static DayTimePart _Read_NomadDrive_002EManagers_002EGameTime_002EDayTimePart(NetworkReader reader)
		{
			return new DayTimePart
			{
				Hours = reader.ReadVarInt(),
				Minutes = reader.ReadVarInt(),
				Seconds = reader.ReadVarInt()
			};
		}

		public static void _Write_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ELootSpawnDataNetwork_005B_005D(NetworkWriter writer, LootSpawnDataNetwork[] value)
		{
			writer.WriteArray(value);
		}

		public static void _Write_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ELootSpawnDataNetwork(NetworkWriter writer, LootSpawnDataNetwork value)
		{
			writer.WriteString(value.lootPrefabGuid);
			writer.WriteFloat(value.chance);
			writer.WriteVector3(value.initialRotationEuler);
			_Write_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ERotationAxis(writer, value.randomRotationAxis);
			writer.WriteFloat(value.minRotationAngle);
			writer.WriteFloat(value.maxRotationAngle);
			writer.WriteBool(value.ignoreCollisionCorrection);
		}

		public static void _Write_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ERotationAxis(NetworkWriter writer, NomadDrive.Features.WorldGeneration.ObjectSpawning.RotationAxis value)
		{
			writer.WriteVarInt((int)value);
		}

		public static LootSpawnDataNetwork[] _Read_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ELootSpawnDataNetwork_005B_005D(NetworkReader reader)
		{
			return reader.ReadArray<LootSpawnDataNetwork>();
		}

		public static LootSpawnDataNetwork _Read_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ELootSpawnDataNetwork(NetworkReader reader)
		{
			return new LootSpawnDataNetwork
			{
				lootPrefabGuid = reader.ReadString(),
				chance = reader.ReadFloat(),
				initialRotationEuler = reader.ReadVector3(),
				randomRotationAxis = _Read_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ERotationAxis(reader),
				minRotationAngle = reader.ReadFloat(),
				maxRotationAngle = reader.ReadFloat(),
				ignoreCollisionCorrection = reader.ReadBool()
			};
		}

		public static NomadDrive.Features.WorldGeneration.ObjectSpawning.RotationAxis _Read_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ERotationAxis(NetworkReader reader)
		{
			return (NomadDrive.Features.WorldGeneration.ObjectSpawning.RotationAxis)reader.ReadVarInt();
		}

		public static void _Write_NomadDrive_002EFeatures_002EObjectPlacement_002EPlacementNetworkData(NetworkWriter writer, PlacementNetworkData value)
		{
			writer.WriteVarUInt(value.ParentNetworkID);
			NetworkWriterExtensions.WriteByte(writer, value.SnappingPlaneIndex);
		}

		public static PlacementNetworkData _Read_NomadDrive_002EFeatures_002EObjectPlacement_002EPlacementNetworkData(NetworkReader reader)
		{
			return new PlacementNetworkData
			{
				ParentNetworkID = reader.ReadVarUInt(),
				SnappingPlaneIndex = NetworkReaderExtensions.ReadByte(reader)
			};
		}

		public static void _Write_NomadDrive_002EFeatures_002EPlayer_002EPlayerStateMachine_002EPlayerState(NetworkWriter writer, PlayerState value)
		{
			writer.WriteVarInt((int)value);
		}

		public static PlayerState _Read_NomadDrive_002EFeatures_002EPlayer_002EPlayerStateMachine_002EPlayerState(NetworkReader reader)
		{
			return (PlayerState)reader.ReadVarInt();
		}

		public static void _Write_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(NetworkWriter writer, SurfaceType value)
		{
			writer.WriteVarInt((int)value);
		}

		public static SurfaceType _Read_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType(NetworkReader reader)
		{
			return (SurfaceType)reader.ReadVarInt();
		}

		public static void _Write_FIMSpace_002EFProceduralAnimation_002ELegsAnimator_002FEGlueMode(NetworkWriter writer, LegsAnimator.EGlueMode value)
		{
			writer.WriteVarInt((int)value);
		}

		public static LegsAnimator.EGlueMode _Read_FIMSpace_002EFProceduralAnimation_002ELegsAnimator_002FEGlueMode(NetworkReader reader)
		{
			return (LegsAnimator.EGlueMode)reader.ReadVarInt();
		}

		public static void _Write_System_002EString_005B_005D(NetworkWriter writer, string[] value)
		{
			writer.WriteArray(value);
		}

		public static string[] _Read_System_002EString_005B_005D(NetworkReader reader)
		{
			return reader.ReadArray<string>();
		}

		public static StepSourceRecord _Read_NomadDrive_002EFeatures_002EObjectives_002ENetworking_002EStepSourceRecord(NetworkReader reader)
		{
			return new StepSourceRecord
			{
				ObjectiveId = reader.ReadString(),
				StepId = reader.ReadString(),
				SourceId = reader.ReadString()
			};
		}

		public static void _Write_NomadDrive_002EFeatures_002EObjectives_002ENetworking_002EStepSourceRecord(NetworkWriter writer, StepSourceRecord value)
		{
			writer.WriteString(value.ObjectiveId);
			writer.WriteString(value.StepId);
			writer.WriteString(value.SourceId);
		}

		public static void _Write_NomadDrive_002EFeatures_002ELiquidTransferSystem_002ELiquidType(NetworkWriter writer, LiquidType value)
		{
			writer.WriteVarInt((int)value);
		}

		public static LiquidType _Read_NomadDrive_002EFeatures_002ELiquidTransferSystem_002ELiquidType(NetworkReader reader)
		{
			return (LiquidType)reader.ReadVarInt();
		}

		public static void _Write_NomadDrive_002EFeatures_002EInteraction_002EHandleState(NetworkWriter writer, HandleState value)
		{
			writer.WriteVarInt((int)value);
		}

		public static HandleState _Read_NomadDrive_002EFeatures_002EInteraction_002EHandleState(NetworkReader reader)
		{
			return (HandleState)reader.ReadVarInt();
		}

		public static void _Write_NomadDrive_002EFeatures_002EInteraction_002ELightSwitchState(NetworkWriter writer, LightSwitchState value)
		{
			NetworkWriterExtensions.WriteByte(writer, (byte)value);
		}

		public static LightSwitchState _Read_NomadDrive_002EFeatures_002EInteraction_002ELightSwitchState(NetworkReader reader)
		{
			return (LightSwitchState)NetworkReaderExtensions.ReadByte(reader);
		}

		public static void _Write_NomadDrive_002EFeatures_002EInteraction_002EToggleState(NetworkWriter writer, ToggleState value)
		{
			writer.WriteVarInt((int)value);
		}

		public static ToggleState _Read_NomadDrive_002EFeatures_002EInteraction_002EToggleState(NetworkReader reader)
		{
			return (ToggleState)reader.ReadVarInt();
		}

		public static StaticInteractableState _Read_NomadDrive_002EFeatures_002EInteraction_002EStaticInteractableState(NetworkReader reader)
		{
			return new StaticInteractableState
			{
				id = reader.ReadVarInt(),
				stateData = NetworkReaderExtensions.ReadByte(reader),
				timestamp = reader.ReadFloat()
			};
		}

		public static void _Write_NomadDrive_002EFeatures_002EInteraction_002EStaticInteractableState(NetworkWriter writer, StaticInteractableState value)
		{
			writer.WriteVarInt(value.id);
			NetworkWriterExtensions.WriteByte(writer, value.stateData);
			writer.WriteFloat(value.timestamp);
		}

		public static StaticInteractableExtendedState _Read_NomadDrive_002EFeatures_002EInteraction_002EStaticInteractableExtendedState(NetworkReader reader)
		{
			return new StaticInteractableExtendedState
			{
				id = reader.ReadVarInt(),
				stateData = NetworkReaderExtensions.ReadByte(reader),
				normalizedValue = NetworkReaderExtensions.ReadByte(reader),
				timestamp = reader.ReadFloat()
			};
		}

		public static void _Write_NomadDrive_002EFeatures_002EInteraction_002EStaticInteractableExtendedState(NetworkWriter writer, StaticInteractableExtendedState value)
		{
			writer.WriteVarInt(value.id);
			NetworkWriterExtensions.WriteByte(writer, value.stateData);
			NetworkWriterExtensions.WriteByte(writer, value.normalizedValue);
			writer.WriteFloat(value.timestamp);
		}

		public static void _Write_NomadDrive_002EFeatures_002EFurnitures_002EDoorState(NetworkWriter writer, DoorState value)
		{
			writer.WriteVarInt((int)value);
		}

		public static DoorState _Read_NomadDrive_002EFeatures_002EFurnitures_002EDoorState(NetworkReader reader)
		{
			return (DoorState)reader.ReadVarInt();
		}

		public static void _Write_NomadDrive_002EFeatures_002EFurnitures_002ESittableSurface_002FStandingPointType(NetworkWriter writer, SittableSurface.StandingPointType value)
		{
			writer.WriteVarInt((int)value);
		}

		public static SittableSurface.StandingPointType _Read_NomadDrive_002EFeatures_002EFurnitures_002ESittableSurface_002FStandingPointType(NetworkReader reader)
		{
			return (SittableSurface.StandingPointType)reader.ReadVarInt();
		}

		public static void _Write_NWH_002EVehiclePhysics2_002EVehicleController_002FMultiplayerState(NetworkWriter writer, VehicleController.MultiplayerState value)
		{
			writer.WriteVarInt(value.lightState);
			writer.WriteFloat(value.engineAngularVelocity);
			writer.WriteFloat(value.steering);
			writer.WriteFloat(value.throttle);
			writer.WriteFloat(value.clutch);
			writer.WriteFloat(value.handbrake);
			writer.WriteVarInt(value.shiftInto);
			writer.WriteBool(value.shiftUp);
			writer.WriteBool(value.shiftDown);
			writer.WriteBool(value.trailerAttachDetach);
			writer.WriteBool(value.horn);
			writer.WriteBool(value.engineStartStop);
			writer.WriteBool(value.cruiseControl);
			writer.WriteBool(value.boost);
			writer.WriteBool(value.flipOver);
		}

		public static VehicleController.MultiplayerState _Read_NWH_002EVehiclePhysics2_002EVehicleController_002FMultiplayerState(NetworkReader reader)
		{
			return new VehicleController.MultiplayerState
			{
				lightState = reader.ReadVarInt(),
				engineAngularVelocity = reader.ReadFloat(),
				steering = reader.ReadFloat(),
				throttle = reader.ReadFloat(),
				clutch = reader.ReadFloat(),
				handbrake = reader.ReadFloat(),
				shiftInto = reader.ReadVarInt(),
				shiftUp = reader.ReadBool(),
				shiftDown = reader.ReadBool(),
				trailerAttachDetach = reader.ReadBool(),
				horn = reader.ReadBool(),
				engineStartStop = reader.ReadBool(),
				cruiseControl = reader.ReadBool(),
				boost = reader.ReadBool(),
				flipOver = reader.ReadBool()
			};
		}

		public static void _Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EDriverDisplayInputs(NetworkWriter writer, DriverDisplayInputs value)
		{
			writer.WriteFloat(value.SteeringAngleDeg);
			writer.WriteFloat(value.Throttle);
			writer.WriteFloat(value.Brakes);
			writer.WriteBool(value.Horn);
		}

		public static DriverDisplayInputs _Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EDriverDisplayInputs(NetworkReader reader)
		{
			return new DriverDisplayInputs
			{
				SteeringAngleDeg = reader.ReadFloat(),
				Throttle = reader.ReadFloat(),
				Brakes = reader.ReadFloat(),
				Horn = reader.ReadBool()
			};
		}

		public static void _Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelVisualState(NetworkWriter writer, WheelVisualState value)
		{
			writer.WriteFloat(value.AngVelFL);
			writer.WriteFloat(value.AngVelFR);
			writer.WriteFloat(value.AngVelRL);
			writer.WriteFloat(value.AngVelRR);
			writer.WriteFloat(value.SteerFL);
			writer.WriteFloat(value.SteerFR);
			writer.WriteFloat(value.SteerRL);
			writer.WriteFloat(value.SteerRR);
			writer.WriteFloat(value.SuspFL);
			writer.WriteFloat(value.SuspFR);
			writer.WriteFloat(value.SuspRL);
			writer.WriteFloat(value.SuspRR);
		}

		public static WheelVisualState _Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelVisualState(NetworkReader reader)
		{
			return new WheelVisualState
			{
				AngVelFL = reader.ReadFloat(),
				AngVelFR = reader.ReadFloat(),
				AngVelRL = reader.ReadFloat(),
				AngVelRR = reader.ReadFloat(),
				SteerFL = reader.ReadFloat(),
				SteerFR = reader.ReadFloat(),
				SteerRL = reader.ReadFloat(),
				SteerRR = reader.ReadFloat(),
				SuspFL = reader.ReadFloat(),
				SuspFR = reader.ReadFloat(),
				SuspRL = reader.ReadFloat(),
				SuspRR = reader.ReadFloat()
			};
		}

		public static void _Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EEngineAudioState(NetworkWriter writer, EngineAudioState value)
		{
			writer.WriteBool(value.IsRunning);
			writer.WriteFloat(value.AngularVelocity);
		}

		public static EngineAudioState _Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EEngineAudioState(NetworkReader reader)
		{
			return new EngineAudioState
			{
				IsRunning = reader.ReadBool(),
				AngularVelocity = reader.ReadFloat()
			};
		}

		public static void _Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelFxState(NetworkWriter writer, WheelFxState value)
		{
			writer.WriteBool(value.GroundedFL);
			writer.WriteBool(value.GroundedFR);
			writer.WriteBool(value.GroundedRL);
			writer.WriteBool(value.GroundedRR);
			writer.WriteFloat(value.LatSlipFL);
			writer.WriteFloat(value.LatSlipFR);
			writer.WriteFloat(value.LatSlipRL);
			writer.WriteFloat(value.LatSlipRR);
			writer.WriteFloat(value.LonSlipFL);
			writer.WriteFloat(value.LonSlipFR);
			writer.WriteFloat(value.LonSlipRL);
			writer.WriteFloat(value.LonSlipRR);
			writer.WriteVarInt(value.SurfFL);
			writer.WriteVarInt(value.SurfFR);
			writer.WriteVarInt(value.SurfRL);
			writer.WriteVarInt(value.SurfRR);
			writer.WriteFloat(value.LoadFL);
			writer.WriteFloat(value.LoadFR);
			writer.WriteFloat(value.LoadRL);
			writer.WriteFloat(value.LoadRR);
			writer.WriteFloat(value.Speed);
			writer.WriteFloat(value.AngularVelMag);
		}

		public static WheelFxState _Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelFxState(NetworkReader reader)
		{
			return new WheelFxState
			{
				GroundedFL = reader.ReadBool(),
				GroundedFR = reader.ReadBool(),
				GroundedRL = reader.ReadBool(),
				GroundedRR = reader.ReadBool(),
				LatSlipFL = reader.ReadFloat(),
				LatSlipFR = reader.ReadFloat(),
				LatSlipRL = reader.ReadFloat(),
				LatSlipRR = reader.ReadFloat(),
				LonSlipFL = reader.ReadFloat(),
				LonSlipFR = reader.ReadFloat(),
				LonSlipRL = reader.ReadFloat(),
				LonSlipRR = reader.ReadFloat(),
				SurfFL = reader.ReadVarInt(),
				SurfFR = reader.ReadVarInt(),
				SurfRL = reader.ReadVarInt(),
				SurfRR = reader.ReadVarInt(),
				LoadFL = reader.ReadFloat(),
				LoadFR = reader.ReadFloat(),
				LoadRL = reader.ReadFloat(),
				LoadRR = reader.ReadFloat(),
				Speed = reader.ReadFloat(),
				AngularVelMag = reader.ReadFloat()
			};
		}

		public static TreeDestructionOperation _Read_NomadDrive_002EFeatures_002EVehicle_002ECollision_002ETreeDestructionOperation(NetworkReader reader)
		{
			return new TreeDestructionOperation
			{
				worldPosition = reader.ReadVector3(),
				impactVelocity = reader.ReadVector3(),
				chunkX = reader.ReadVarInt(),
				chunkZ = reader.ReadVarInt()
			};
		}

		public static void _Write_NomadDrive_002EFeatures_002EVehicle_002ECollision_002ETreeDestructionOperation(NetworkWriter writer, TreeDestructionOperation value)
		{
			writer.WriteVector3(value.worldPosition);
			writer.WriteVector3(value.impactVelocity);
			writer.WriteVarInt(value.chunkX);
			writer.WriteVarInt(value.chunkZ);
		}

		public static void _Write_NomadDrive_002EFeatures_002ECooking_002ECookingPotSlotState(NetworkWriter writer, CookingPotSlotState value)
		{
			writer.WriteVarInt((int)value);
		}

		public static CookingPotSlotState _Read_NomadDrive_002EFeatures_002ECooking_002ECookingPotSlotState(NetworkReader reader)
		{
			return (CookingPotSlotState)reader.ReadVarInt();
		}

		public static void _Write_NomadDrive_002EFeatures_002EConsumables_002EFoodDurabilityType(NetworkWriter writer, FoodDurabilityType value)
		{
			writer.WriteVarInt((int)value);
		}

		public static FoodDurabilityType _Read_NomadDrive_002EFeatures_002EConsumables_002EFoodDurabilityType(NetworkReader reader)
		{
			return (FoodDurabilityType)reader.ReadVarInt();
		}

		public static void _Write_NomadDrive_002EFeatures_002EConsumables_002ECookedLevel(NetworkWriter writer, CookedLevel value)
		{
			writer.WriteVarInt((int)value);
		}

		public static CookedLevel _Read_NomadDrive_002EFeatures_002EConsumables_002ECookedLevel(NetworkReader reader)
		{
			return (CookedLevel)reader.ReadVarInt();
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
			Writer<SoundID>.write = SoundIDSerializer.WriteSoundID;
			Writer<SyncData>.write = SyncDataReaderWriter.WriteSyncData;
			Writer<PredictedSyncData>.write = PredictedSyncDataReadWrite.WritePredictedSyncData;
			Writer<ServerRequest>.write = _Write_Mirror_002EDiscovery_002EServerRequest;
			Writer<ServerResponse>.write = _Write_Mirror_002EDiscovery_002EServerResponse;
			Writer<DayTimePart>.write = _Write_NomadDrive_002EManagers_002EGameTime_002EDayTimePart;
			Writer<LootSpawnDataNetwork[]>.write = _Write_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ELootSpawnDataNetwork_005B_005D;
			Writer<LootSpawnDataNetwork>.write = _Write_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ELootSpawnDataNetwork;
			Writer<NomadDrive.Features.WorldGeneration.ObjectSpawning.RotationAxis>.write = _Write_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ERotationAxis;
			Writer<PlacementNetworkData>.write = _Write_NomadDrive_002EFeatures_002EObjectPlacement_002EPlacementNetworkData;
			Writer<PlayerState>.write = _Write_NomadDrive_002EFeatures_002EPlayer_002EPlayerStateMachine_002EPlayerState;
			Writer<SurfaceType>.write = _Write_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType;
			Writer<SittableSurface>.write = NetworkWriterExtensions.WriteNetworkBehaviour;
			Writer<LegsAnimator.EGlueMode>.write = _Write_FIMSpace_002EFProceduralAnimation_002ELegsAnimator_002FEGlueMode;
			Writer<string[]>.write = _Write_System_002EString_005B_005D;
			Writer<StepSourceRecord>.write = _Write_NomadDrive_002EFeatures_002EObjectives_002ENetworking_002EStepSourceRecord;
			Writer<LiquidContainerComponent>.write = NetworkWriterExtensions.WriteNetworkBehaviour;
			Writer<LiquidType>.write = _Write_NomadDrive_002EFeatures_002ELiquidTransferSystem_002ELiquidType;
			Writer<HandleState>.write = _Write_NomadDrive_002EFeatures_002EInteraction_002EHandleState;
			Writer<LightSwitchState>.write = _Write_NomadDrive_002EFeatures_002EInteraction_002ELightSwitchState;
			Writer<ToggleState>.write = _Write_NomadDrive_002EFeatures_002EInteraction_002EToggleState;
			Writer<StaticInteractableState>.write = _Write_NomadDrive_002EFeatures_002EInteraction_002EStaticInteractableState;
			Writer<StaticInteractableExtendedState>.write = _Write_NomadDrive_002EFeatures_002EInteraction_002EStaticInteractableExtendedState;
			Writer<DoorState>.write = _Write_NomadDrive_002EFeatures_002EFurnitures_002EDoorState;
			Writer<SittableSurface.StandingPointType>.write = _Write_NomadDrive_002EFeatures_002EFurnitures_002ESittableSurface_002FStandingPointType;
			Writer<VehicleController.MultiplayerState>.write = _Write_NWH_002EVehiclePhysics2_002EVehicleController_002FMultiplayerState;
			Writer<DriverDisplayInputs>.write = _Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EDriverDisplayInputs;
			Writer<WheelVisualState>.write = _Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelVisualState;
			Writer<EngineAudioState>.write = _Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EEngineAudioState;
			Writer<WheelFxState>.write = _Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelFxState;
			Writer<TreeDestructionOperation>.write = _Write_NomadDrive_002EFeatures_002EVehicle_002ECollision_002ETreeDestructionOperation;
			Writer<CookingPotSlotState>.write = _Write_NomadDrive_002EFeatures_002ECooking_002ECookingPotSlotState;
			Writer<FoodDurabilityType>.write = _Write_NomadDrive_002EFeatures_002EConsumables_002EFoodDurabilityType;
			Writer<CookedLevel>.write = _Write_NomadDrive_002EFeatures_002EConsumables_002ECookedLevel;
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
			Reader<SoundID>.read = SoundIDSerializer.ReadSoundID;
			Reader<SyncData>.read = SyncDataReaderWriter.ReadSyncData;
			Reader<PredictedSyncData>.read = PredictedSyncDataReadWrite.ReadPredictedSyncData;
			Reader<ServerRequest>.read = _Read_Mirror_002EDiscovery_002EServerRequest;
			Reader<ServerResponse>.read = _Read_Mirror_002EDiscovery_002EServerResponse;
			Reader<DayTimePart>.read = _Read_NomadDrive_002EManagers_002EGameTime_002EDayTimePart;
			Reader<LootSpawnDataNetwork[]>.read = _Read_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ELootSpawnDataNetwork_005B_005D;
			Reader<LootSpawnDataNetwork>.read = _Read_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ELootSpawnDataNetwork;
			Reader<NomadDrive.Features.WorldGeneration.ObjectSpawning.RotationAxis>.read = _Read_NomadDrive_002EFeatures_002EWorldGeneration_002EObjectSpawning_002ERotationAxis;
			Reader<PlacementNetworkData>.read = _Read_NomadDrive_002EFeatures_002EObjectPlacement_002EPlacementNetworkData;
			Reader<PlayerState>.read = _Read_NomadDrive_002EFeatures_002EPlayer_002EPlayerStateMachine_002EPlayerState;
			Reader<SurfaceType>.read = _Read_NomadDrive_002EFeatures_002EPlayer_002ESurfaceType;
			Reader<SittableSurface>.read = NetworkReaderExtensions.ReadNetworkBehaviour<SittableSurface>;
			Reader<LegsAnimator.EGlueMode>.read = _Read_FIMSpace_002EFProceduralAnimation_002ELegsAnimator_002FEGlueMode;
			Reader<string[]>.read = _Read_System_002EString_005B_005D;
			Reader<StepSourceRecord>.read = _Read_NomadDrive_002EFeatures_002EObjectives_002ENetworking_002EStepSourceRecord;
			Reader<LiquidContainerComponent>.read = NetworkReaderExtensions.ReadNetworkBehaviour<LiquidContainerComponent>;
			Reader<LiquidType>.read = _Read_NomadDrive_002EFeatures_002ELiquidTransferSystem_002ELiquidType;
			Reader<HandleState>.read = _Read_NomadDrive_002EFeatures_002EInteraction_002EHandleState;
			Reader<LightSwitchState>.read = _Read_NomadDrive_002EFeatures_002EInteraction_002ELightSwitchState;
			Reader<ToggleState>.read = _Read_NomadDrive_002EFeatures_002EInteraction_002EToggleState;
			Reader<StaticInteractableState>.read = _Read_NomadDrive_002EFeatures_002EInteraction_002EStaticInteractableState;
			Reader<StaticInteractableExtendedState>.read = _Read_NomadDrive_002EFeatures_002EInteraction_002EStaticInteractableExtendedState;
			Reader<DoorState>.read = _Read_NomadDrive_002EFeatures_002EFurnitures_002EDoorState;
			Reader<SittableSurface.StandingPointType>.read = _Read_NomadDrive_002EFeatures_002EFurnitures_002ESittableSurface_002FStandingPointType;
			Reader<VehicleController.MultiplayerState>.read = _Read_NWH_002EVehiclePhysics2_002EVehicleController_002FMultiplayerState;
			Reader<DriverDisplayInputs>.read = _Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EDriverDisplayInputs;
			Reader<WheelVisualState>.read = _Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelVisualState;
			Reader<EngineAudioState>.read = _Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EEngineAudioState;
			Reader<WheelFxState>.read = _Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelFxState;
			Reader<TreeDestructionOperation>.read = _Read_NomadDrive_002EFeatures_002EVehicle_002ECollision_002ETreeDestructionOperation;
			Reader<CookingPotSlotState>.read = _Read_NomadDrive_002EFeatures_002ECooking_002ECookingPotSlotState;
			Reader<FoodDurabilityType>.read = _Read_NomadDrive_002EFeatures_002EConsumables_002EFoodDurabilityType;
			Reader<CookedLevel>.read = _Read_NomadDrive_002EFeatures_002EConsumables_002ECookedLevel;
		}
	}
}
