using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FishNet.Component.Prediction;
using FishNet.Component.Transforming;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Managing.Server;
using FishNet.Managing.Timing;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;

namespace FishNet.Serializing.Generated
{
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public static class GeneratedReaders___Internal
	{
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeOnce()
		{
			GenericReader<byte>.Read = InstancedExtension___ReadByte;
			GenericReader<sbyte>.Read = InstancedExtension___ReadSByte;
			GenericReader<char>.Read = InstancedExtension___ReadChar;
			GenericReader<bool>.Read = InstancedExtension___ReadBoolean;
			GenericReader<ushort>.Read = InstancedExtension___ReadUInt16;
			GenericReader<short>.Read = InstancedExtension___ReadInt16;
			GenericReader<uint>.ReadAutoPack = InstancedExtension___ReadUInt32;
			GenericReader<int>.ReadAutoPack = InstancedExtension___ReadInt32;
			GenericReader<long>.ReadAutoPack = InstancedExtension___ReadInt64;
			GenericReader<ulong>.ReadAutoPack = InstancedExtension___ReadUInt64;
			GenericReader<float>.ReadAutoPack = InstancedExtension___ReadSingle;
			GenericReader<double>.Read = InstancedExtension___ReadDouble;
			GenericReader<decimal>.Read = InstancedExtension___ReadDecimal;
			GenericReader<string>.Read = InstancedExtension___ReadString;
			GenericReader<byte[]>.Read = InstancedExtension___ReadBytesAndSizeAllocated;
			GenericReader<ArraySegment<byte>>.Read = InstancedExtension___ReadArraySegmentAndSize;
			GenericReader<Vector2>.Read = InstancedExtension___ReadVector2;
			GenericReader<Vector3>.Read = InstancedExtension___ReadVector3;
			GenericReader<Vector4>.Read = InstancedExtension___ReadVector4;
			GenericReader<Vector2Int>.ReadAutoPack = InstancedExtension___ReadVector2Int;
			GenericReader<Vector3Int>.ReadAutoPack = InstancedExtension___ReadVector3Int;
			GenericReader<Color>.ReadAutoPack = InstancedExtension___ReadColor;
			GenericReader<Color32>.Read = InstancedExtension___ReadColor32;
			GenericReader<Quaternion>.ReadAutoPack = InstancedExtension___ReadQuaternion;
			GenericReader<Rect>.Read = InstancedExtension___ReadRect;
			GenericReader<Plane>.Read = InstancedExtension___ReadPlane;
			GenericReader<Ray>.Read = InstancedExtension___ReadRay;
			GenericReader<Ray2D>.Read = InstancedExtension___ReadRay2D;
			GenericReader<Matrix4x4>.Read = InstancedExtension___ReadMatrix4x4;
			GenericReader<Guid>.Read = InstancedExtension___ReadGuid;
			GenericReader<GameObject>.Read = InstancedExtension___ReadGameObject;
			GenericReader<Transform>.Read = InstancedExtension___ReadTransform;
			GenericReader<NetworkObject>.Read = InstancedExtension___ReadNetworkObject;
			GenericReader<NetworkBehaviour>.Read = InstancedExtension___ReadNetworkBehaviour;
			GenericReader<DateTime>.Read = InstancedExtension___ReadDateTime;
			GenericReader<Channel>.Read = InstancedExtension___ReadChannel;
			GenericReader<NetworkConnection>.Read = InstancedExtension___ReadNetworkConnection;
			GenericReader<RigidbodyState>.Read = RigidbodyStateSerializers.ReadRigidbodyState;
			GenericReader<Rigidbody2DState>.Read = RigidbodyStateSerializers.ReadRigidbody2DState;
			GenericReader<PreciseTick>.Read = PreciseTickSerializer.ReadPreciseTick;
			GenericReader<ClientConnectionChangeBroadcast>.Read = Read___FishNet_002EManaging_002EServer_002EClientConnectionChangeBroadcastFishNet_002ESerializing_002EGenerateds;
			GenericReader<ConnectedClientsBroadcast>.Read = Read___FishNet_002EManaging_002EServer_002EConnectedClientsBroadcastFishNet_002ESerializing_002EGenerateds;
			GenericReader<List<int>>.Read = Read___System_002ECollections_002EGeneric_002EList_00601_003CSystem_002EInt32_003EFishNet_002ESerializing_002EGenerateds;
			GenericReader<EmptyStartScenesBroadcast>.Read = Read___FishNet_002EManaging_002EScened_002EEmptyStartScenesBroadcastFishNet_002ESerializing_002EGenerateds;
			GenericReader<LoadScenesBroadcast>.Read = Read___FishNet_002EManaging_002EScened_002ELoadScenesBroadcastFishNet_002ESerializing_002EGenerateds;
			GenericReader<LoadQueueData>.Read = Read___FishNet_002EManaging_002EScened_002ELoadQueueDataFishNet_002ESerializing_002EGenerateds;
			GenericReader<SceneLoadData>.Read = Read___FishNet_002EManaging_002EScened_002ESceneLoadDataFishNet_002ESerializing_002EGenerateds;
			GenericReader<SceneLookupData>.Read = Read___FishNet_002EManaging_002EScened_002ESceneLookupDataFishNet_002ESerializing_002EGenerateds;
			GenericReader<SceneLookupData[]>.Read = Read___FishNet_002EManaging_002EScened_002ESceneLookupData_005B_005DFishNet_002ESerializing_002EGenerateds;
			GenericReader<NetworkObject[]>.Read = Read___FishNet_002EObject_002ENetworkObject_005B_005DFishNet_002ESerializing_002EGenerateds;
			GenericReader<ReplaceOption>.Read = Read___FishNet_002EManaging_002EScened_002EReplaceOptionFishNet_002ESerializing_002EGenerateds;
			GenericReader<LoadParams>.Read = Read___FishNet_002EManaging_002EScened_002ELoadParamsFishNet_002ESerializing_002EGenerateds;
			GenericReader<LoadOptions>.Read = Read___FishNet_002EManaging_002EScened_002ELoadOptionsFishNet_002ESerializing_002EGenerateds;
			GenericReader<string[]>.Read = Read___System_002EString_005B_005DFishNet_002ESerializing_002EGenerateds;
			GenericReader<UnloadScenesBroadcast>.Read = Read___FishNet_002EManaging_002EScened_002EUnloadScenesBroadcastFishNet_002ESerializing_002EGenerateds;
			GenericReader<UnloadQueueData>.Read = Read___FishNet_002EManaging_002EScened_002EUnloadQueueDataFishNet_002ESerializing_002EGenerateds;
			GenericReader<SceneUnloadData>.Read = Read___FishNet_002EManaging_002EScened_002ESceneUnloadDataFishNet_002ESerializing_002EGenerateds;
			GenericReader<UnloadParams>.Read = Read___FishNet_002EManaging_002EScened_002EUnloadParamsFishNet_002ESerializing_002EGenerateds;
			GenericReader<UnloadOptions>.Read = Read___FishNet_002EManaging_002EScened_002EUnloadOptionsFishNet_002ESerializing_002EGenerateds;
			GenericReader<UnloadOptions.ServerUnloadMode>.Read = Read___FishNet_002EManaging_002EScened_002EUnloadOptions_002FServerUnloadModeFishNet_002ESerializing_002EGenerateds;
			GenericReader<ClientScenesLoadedBroadcast>.Read = Read___FishNet_002EManaging_002EScened_002EClientScenesLoadedBroadcastFishNet_002ESerializing_002EGenerateds;
			GenericReader<SynchronizedProperty>.Read = Read___FishNet_002EComponent_002ETransforming_002ESynchronizedPropertyFishNet_002ESerializing_002EGenerateds;
		}

		public static byte InstancedExtension___ReadByte(this Reader reader)
		{
			return reader.ReadByte();
		}

		public static sbyte InstancedExtension___ReadSByte(this Reader reader)
		{
			return reader.ReadSByte();
		}

		public static char InstancedExtension___ReadChar(this Reader reader)
		{
			return reader.ReadChar();
		}

		public static bool InstancedExtension___ReadBoolean(this Reader reader)
		{
			return reader.ReadBoolean();
		}

		public static ushort InstancedExtension___ReadUInt16(this Reader reader)
		{
			return reader.ReadUInt16();
		}

		public static short InstancedExtension___ReadInt16(this Reader reader)
		{
			return reader.ReadInt16();
		}

		public static uint InstancedExtension___ReadUInt32(this Reader reader, AutoPackType packType1 = AutoPackType.Packed)
		{
			return reader.ReadUInt32(packType1);
		}

		public static int InstancedExtension___ReadInt32(this Reader reader, AutoPackType packType1 = AutoPackType.Packed)
		{
			return reader.ReadInt32(packType1);
		}

		public static long InstancedExtension___ReadInt64(this Reader reader, AutoPackType packType1 = AutoPackType.Packed)
		{
			return reader.ReadInt64(packType1);
		}

		public static ulong InstancedExtension___ReadUInt64(this Reader reader, AutoPackType packType1 = AutoPackType.Packed)
		{
			return reader.ReadUInt64(packType1);
		}

		public static float InstancedExtension___ReadSingle(this Reader reader, AutoPackType packType1 = AutoPackType.Unpacked)
		{
			return reader.ReadSingle(packType1);
		}

		public static double InstancedExtension___ReadDouble(this Reader reader)
		{
			return reader.ReadDouble();
		}

		public static decimal InstancedExtension___ReadDecimal(this Reader reader)
		{
			return reader.ReadDecimal();
		}

		public static string InstancedExtension___ReadString(this Reader reader)
		{
			return reader.ReadString();
		}

		public static byte[] InstancedExtension___ReadBytesAndSizeAllocated(this Reader reader)
		{
			return reader.ReadBytesAndSizeAllocated();
		}

		public static ArraySegment<byte> InstancedExtension___ReadArraySegmentAndSize(this Reader reader)
		{
			return reader.ReadArraySegmentAndSize();
		}

		public static Vector2 InstancedExtension___ReadVector2(this Reader reader)
		{
			return reader.ReadVector2();
		}

		public static Vector3 InstancedExtension___ReadVector3(this Reader reader)
		{
			return reader.ReadVector3();
		}

		public static Vector4 InstancedExtension___ReadVector4(this Reader reader)
		{
			return reader.ReadVector4();
		}

		public static Vector2Int InstancedExtension___ReadVector2Int(this Reader reader, AutoPackType packType1 = AutoPackType.Packed)
		{
			return reader.ReadVector2Int(packType1);
		}

		public static Vector3Int InstancedExtension___ReadVector3Int(this Reader reader, AutoPackType packType1 = AutoPackType.Packed)
		{
			return reader.ReadVector3Int(packType1);
		}

		public static Color InstancedExtension___ReadColor(this Reader reader, AutoPackType packType1 = AutoPackType.Packed)
		{
			return reader.ReadColor(packType1);
		}

		public static Color32 InstancedExtension___ReadColor32(this Reader reader)
		{
			return reader.ReadColor32();
		}

		public static Quaternion InstancedExtension___ReadQuaternion(this Reader reader, AutoPackType packType1 = AutoPackType.Packed)
		{
			return reader.ReadQuaternion(packType1);
		}

		public static Rect InstancedExtension___ReadRect(this Reader reader)
		{
			return reader.ReadRect();
		}

		public static Plane InstancedExtension___ReadPlane(this Reader reader)
		{
			return reader.ReadPlane();
		}

		public static Ray InstancedExtension___ReadRay(this Reader reader)
		{
			return reader.ReadRay();
		}

		public static Ray2D InstancedExtension___ReadRay2D(this Reader reader)
		{
			return reader.ReadRay2D();
		}

		public static Matrix4x4 InstancedExtension___ReadMatrix4x4(this Reader reader)
		{
			return reader.ReadMatrix4x4();
		}

		public static Guid InstancedExtension___ReadGuid(this Reader reader)
		{
			return reader.ReadGuid();
		}

		public static GameObject InstancedExtension___ReadGameObject(this Reader reader)
		{
			return reader.ReadGameObject();
		}

		public static Transform InstancedExtension___ReadTransform(this Reader reader)
		{
			return reader.ReadTransform();
		}

		public static NetworkObject InstancedExtension___ReadNetworkObject(this Reader reader)
		{
			return reader.ReadNetworkObject();
		}

		public static NetworkBehaviour InstancedExtension___ReadNetworkBehaviour(this Reader reader)
		{
			return reader.ReadNetworkBehaviour();
		}

		public static DateTime InstancedExtension___ReadDateTime(this Reader reader)
		{
			return reader.ReadDateTime();
		}

		public static Channel InstancedExtension___ReadChannel(this Reader reader)
		{
			return reader.ReadChannel();
		}

		public static NetworkConnection InstancedExtension___ReadNetworkConnection(this Reader reader)
		{
			return reader.ReadNetworkConnection();
		}

		public static ClientConnectionChangeBroadcast Read___FishNet_002EManaging_002EServer_002EClientConnectionChangeBroadcastFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return new ClientConnectionChangeBroadcast
			{
				Connected = InstancedExtension___ReadBoolean(reader),
				Id = InstancedExtension___ReadInt32(reader)
			};
		}

		public static ConnectedClientsBroadcast Read___FishNet_002EManaging_002EServer_002EConnectedClientsBroadcastFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return new ConnectedClientsBroadcast
			{
				Values = Read___System_002ECollections_002EGeneric_002EList_00601_003CSystem_002EInt32_003EFishNet_002ESerializing_002EGenerateds(reader)
			};
		}

		public static List<int> Read___System_002ECollections_002EGeneric_002EList_00601_003CSystem_002EInt32_003EFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return reader.ReadListAllocated<int>();
		}

		public static EmptyStartScenesBroadcast Read___FishNet_002EManaging_002EScened_002EEmptyStartScenesBroadcastFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return default(EmptyStartScenesBroadcast);
		}

		public static LoadScenesBroadcast Read___FishNet_002EManaging_002EScened_002ELoadScenesBroadcastFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return new LoadScenesBroadcast
			{
				QueueData = Read___FishNet_002EManaging_002EScened_002ELoadQueueDataFishNet_002ESerializing_002EGenerateds(reader)
			};
		}

		public static LoadQueueData Read___FishNet_002EManaging_002EScened_002ELoadQueueDataFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			if (InstancedExtension___ReadBoolean(reader))
			{
				return null;
			}
			LoadQueueData loadQueueData = new LoadQueueData();
			loadQueueData.SceneLoadData = Read___FishNet_002EManaging_002EScened_002ESceneLoadDataFishNet_002ESerializing_002EGenerateds(reader);
			loadQueueData.GlobalScenes = Read___System_002EString_005B_005DFishNet_002ESerializing_002EGenerateds(reader);
			return loadQueueData;
		}

		public static SceneLoadData Read___FishNet_002EManaging_002EScened_002ESceneLoadDataFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			if (InstancedExtension___ReadBoolean(reader))
			{
				return null;
			}
			SceneLoadData sceneLoadData = new SceneLoadData();
			sceneLoadData.PreferredActiveScene = Read___FishNet_002EManaging_002EScened_002ESceneLookupDataFishNet_002ESerializing_002EGenerateds(reader);
			sceneLoadData.SceneLookupDatas = Read___FishNet_002EManaging_002EScened_002ESceneLookupData_005B_005DFishNet_002ESerializing_002EGenerateds(reader);
			sceneLoadData.MovedNetworkObjects = Read___FishNet_002EObject_002ENetworkObject_005B_005DFishNet_002ESerializing_002EGenerateds(reader);
			sceneLoadData.ReplaceScenes = Read___FishNet_002EManaging_002EScened_002EReplaceOptionFishNet_002ESerializing_002EGenerateds(reader);
			sceneLoadData.Params = Read___FishNet_002EManaging_002EScened_002ELoadParamsFishNet_002ESerializing_002EGenerateds(reader);
			sceneLoadData.Options = Read___FishNet_002EManaging_002EScened_002ELoadOptionsFishNet_002ESerializing_002EGenerateds(reader);
			return sceneLoadData;
		}

		public static SceneLookupData Read___FishNet_002EManaging_002EScened_002ESceneLookupDataFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			if (InstancedExtension___ReadBoolean(reader))
			{
				return null;
			}
			SceneLookupData sceneLookupData = new SceneLookupData();
			sceneLookupData.Handle = InstancedExtension___ReadInt32(reader);
			sceneLookupData.Name = InstancedExtension___ReadString(reader);
			return sceneLookupData;
		}

		public static SceneLookupData[] Read___FishNet_002EManaging_002EScened_002ESceneLookupData_005B_005DFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return reader.ReadArrayAllocated<SceneLookupData>();
		}

		public static NetworkObject[] Read___FishNet_002EObject_002ENetworkObject_005B_005DFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return reader.ReadArrayAllocated<NetworkObject>();
		}

		public static ReplaceOption Read___FishNet_002EManaging_002EScened_002EReplaceOptionFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return (ReplaceOption)InstancedExtension___ReadByte(reader);
		}

		public static LoadParams Read___FishNet_002EManaging_002EScened_002ELoadParamsFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			if (InstancedExtension___ReadBoolean(reader))
			{
				return null;
			}
			LoadParams loadParams = new LoadParams();
			loadParams.ClientParams = InstancedExtension___ReadBytesAndSizeAllocated(reader);
			return loadParams;
		}

		public static LoadOptions Read___FishNet_002EManaging_002EScened_002ELoadOptionsFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			if (InstancedExtension___ReadBoolean(reader))
			{
				return null;
			}
			LoadOptions loadOptions = new LoadOptions();
			loadOptions.ReloadScenes = InstancedExtension___ReadBoolean(reader);
			loadOptions.Addressables = InstancedExtension___ReadBoolean(reader);
			return loadOptions;
		}

		public static string[] Read___System_002EString_005B_005DFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return reader.ReadArrayAllocated<string>();
		}

		public static UnloadScenesBroadcast Read___FishNet_002EManaging_002EScened_002EUnloadScenesBroadcastFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return new UnloadScenesBroadcast
			{
				QueueData = Read___FishNet_002EManaging_002EScened_002EUnloadQueueDataFishNet_002ESerializing_002EGenerateds(reader)
			};
		}

		public static UnloadQueueData Read___FishNet_002EManaging_002EScened_002EUnloadQueueDataFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			if (InstancedExtension___ReadBoolean(reader))
			{
				return null;
			}
			UnloadQueueData unloadQueueData = new UnloadQueueData();
			unloadQueueData.SceneUnloadData = Read___FishNet_002EManaging_002EScened_002ESceneUnloadDataFishNet_002ESerializing_002EGenerateds(reader);
			unloadQueueData.GlobalScenes = Read___System_002EString_005B_005DFishNet_002ESerializing_002EGenerateds(reader);
			return unloadQueueData;
		}

		public static SceneUnloadData Read___FishNet_002EManaging_002EScened_002ESceneUnloadDataFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			if (InstancedExtension___ReadBoolean(reader))
			{
				return null;
			}
			SceneUnloadData sceneUnloadData = new SceneUnloadData();
			sceneUnloadData.PreferredActiveScene = Read___FishNet_002EManaging_002EScened_002ESceneLookupDataFishNet_002ESerializing_002EGenerateds(reader);
			sceneUnloadData.SceneLookupDatas = Read___FishNet_002EManaging_002EScened_002ESceneLookupData_005B_005DFishNet_002ESerializing_002EGenerateds(reader);
			sceneUnloadData.Params = Read___FishNet_002EManaging_002EScened_002EUnloadParamsFishNet_002ESerializing_002EGenerateds(reader);
			sceneUnloadData.Options = Read___FishNet_002EManaging_002EScened_002EUnloadOptionsFishNet_002ESerializing_002EGenerateds(reader);
			return sceneUnloadData;
		}

		public static UnloadParams Read___FishNet_002EManaging_002EScened_002EUnloadParamsFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			if (InstancedExtension___ReadBoolean(reader))
			{
				return null;
			}
			UnloadParams unloadParams = new UnloadParams();
			unloadParams.ClientParams = InstancedExtension___ReadBytesAndSizeAllocated(reader);
			return unloadParams;
		}

		public static UnloadOptions Read___FishNet_002EManaging_002EScened_002EUnloadOptionsFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			if (InstancedExtension___ReadBoolean(reader))
			{
				return null;
			}
			UnloadOptions unloadOptions = new UnloadOptions();
			unloadOptions.Mode = Read___FishNet_002EManaging_002EScened_002EUnloadOptions_002FServerUnloadModeFishNet_002ESerializing_002EGenerateds(reader);
			unloadOptions.Addressables = InstancedExtension___ReadBoolean(reader);
			return unloadOptions;
		}

		public static UnloadOptions.ServerUnloadMode Read___FishNet_002EManaging_002EScened_002EUnloadOptions_002FServerUnloadModeFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return (UnloadOptions.ServerUnloadMode)InstancedExtension___ReadInt32(reader);
		}

		public static ClientScenesLoadedBroadcast Read___FishNet_002EManaging_002EScened_002EClientScenesLoadedBroadcastFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return new ClientScenesLoadedBroadcast
			{
				SceneLookupDatas = Read___FishNet_002EManaging_002EScened_002ESceneLookupData_005B_005DFishNet_002ESerializing_002EGenerateds(reader)
			};
		}

		public static SynchronizedProperty Read___FishNet_002EComponent_002ETransforming_002ESynchronizedPropertyFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return (SynchronizedProperty)InstancedExtension___ReadByte(reader);
		}
	}
}
