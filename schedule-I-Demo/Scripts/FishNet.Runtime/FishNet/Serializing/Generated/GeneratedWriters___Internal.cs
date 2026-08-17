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
	public static class GeneratedWriters___Internal
	{
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeOnce()
		{
			GenericWriter<byte>.Write = InstancedExtension___WriteByte;
			GenericWriter<byte[]>.Write = InstancedExtension___WriteBytesAndSize;
			GenericWriter<sbyte>.Write = InstancedExtension___WriteSByte;
			GenericWriter<char>.Write = InstancedExtension___WriteChar;
			GenericWriter<bool>.Write = InstancedExtension___WriteBoolean;
			GenericWriter<ushort>.Write = InstancedExtension___WriteUInt16;
			GenericWriter<short>.Write = InstancedExtension___WriteInt16;
			GenericWriter<int>.WriteAutoPack = InstancedExtension___WriteInt32;
			GenericWriter<uint>.WriteAutoPack = InstancedExtension___WriteUInt32;
			GenericWriter<long>.WriteAutoPack = InstancedExtension___WriteInt64;
			GenericWriter<ulong>.WriteAutoPack = InstancedExtension___WriteUInt64;
			GenericWriter<float>.WriteAutoPack = InstancedExtension___WriteSingle;
			GenericWriter<double>.Write = InstancedExtension___WriteDouble;
			GenericWriter<decimal>.Write = InstancedExtension___WriteDecimal;
			GenericWriter<string>.Write = InstancedExtension___WriteString;
			GenericWriter<ArraySegment<byte>>.Write = InstancedExtension___WriteArraySegmentAndSize;
			GenericWriter<Vector2>.Write = InstancedExtension___WriteVector2;
			GenericWriter<Vector3>.Write = InstancedExtension___WriteVector3;
			GenericWriter<Vector4>.Write = InstancedExtension___WriteVector4;
			GenericWriter<Vector2Int>.WriteAutoPack = InstancedExtension___WriteVector2Int;
			GenericWriter<Vector3Int>.WriteAutoPack = InstancedExtension___WriteVector3Int;
			GenericWriter<Color>.WriteAutoPack = InstancedExtension___WriteColor;
			GenericWriter<Color32>.Write = InstancedExtension___WriteColor32;
			GenericWriter<Quaternion>.WriteAutoPack = InstancedExtension___WriteQuaternion;
			GenericWriter<Rect>.Write = InstancedExtension___WriteRect;
			GenericWriter<Plane>.Write = InstancedExtension___WritePlane;
			GenericWriter<Ray>.Write = InstancedExtension___WriteRay;
			GenericWriter<Ray2D>.Write = InstancedExtension___WriteRay2D;
			GenericWriter<Matrix4x4>.Write = InstancedExtension___WriteMatrix4x4;
			GenericWriter<Guid>.Write = InstancedExtension___WriteGuidAllocated;
			GenericWriter<GameObject>.Write = InstancedExtension___WriteGameObject;
			GenericWriter<Transform>.Write = InstancedExtension___WriteTransform;
			GenericWriter<NetworkObject>.Write = InstancedExtension___WriteNetworkObject;
			GenericWriter<NetworkBehaviour>.Write = InstancedExtension___WriteNetworkBehaviour;
			GenericWriter<DateTime>.Write = InstancedExtension___WriteDateTime;
			GenericWriter<Channel>.Write = InstancedExtension___WriteChannel;
			GenericWriter<NetworkConnection>.Write = InstancedExtension___WriteNetworkConnection;
			GenericWriter<RigidbodyState>.Write = RigidbodyStateSerializers.WriteRigidbodyState;
			GenericWriter<Rigidbody2DState>.Write = RigidbodyStateSerializers.WriteRigidbody2DState;
			GenericWriter<PreciseTick>.Write = PreciseTickSerializer.WritePreciseTick;
			GenericWriter<ClientConnectionChangeBroadcast>.Write = Write___FishNet_002EManaging_002EServer_002EClientConnectionChangeBroadcastFishNet_002ESerializing_002EGenerated;
			GenericWriter<ConnectedClientsBroadcast>.Write = Write___FishNet_002EManaging_002EServer_002EConnectedClientsBroadcastFishNet_002ESerializing_002EGenerated;
			GenericWriter<List<int>>.Write = Write___System_002ECollections_002EGeneric_002EList_00601_003CSystem_002EInt32_003EFishNet_002ESerializing_002EGenerated;
			GenericWriter<EmptyStartScenesBroadcast>.Write = Write___FishNet_002EManaging_002EScened_002EEmptyStartScenesBroadcastFishNet_002ESerializing_002EGenerated;
			GenericWriter<LoadScenesBroadcast>.Write = Write___FishNet_002EManaging_002EScened_002ELoadScenesBroadcastFishNet_002ESerializing_002EGenerated;
			GenericWriter<LoadQueueData>.Write = Write___FishNet_002EManaging_002EScened_002ELoadQueueDataFishNet_002ESerializing_002EGenerated;
			GenericWriter<SceneLoadData>.Write = Write___FishNet_002EManaging_002EScened_002ESceneLoadDataFishNet_002ESerializing_002EGenerated;
			GenericWriter<SceneLookupData>.Write = Write___FishNet_002EManaging_002EScened_002ESceneLookupDataFishNet_002ESerializing_002EGenerated;
			GenericWriter<SceneLookupData[]>.Write = Write___FishNet_002EManaging_002EScened_002ESceneLookupData_005B_005DFishNet_002ESerializing_002EGenerated;
			GenericWriter<NetworkObject[]>.Write = Write___FishNet_002EObject_002ENetworkObject_005B_005DFishNet_002ESerializing_002EGenerated;
			GenericWriter<ReplaceOption>.Write = Write___FishNet_002EManaging_002EScened_002EReplaceOptionFishNet_002ESerializing_002EGenerated;
			GenericWriter<LoadParams>.Write = Write___FishNet_002EManaging_002EScened_002ELoadParamsFishNet_002ESerializing_002EGenerated;
			GenericWriter<LoadOptions>.Write = Write___FishNet_002EManaging_002EScened_002ELoadOptionsFishNet_002ESerializing_002EGenerated;
			GenericWriter<string[]>.Write = Write___System_002EString_005B_005DFishNet_002ESerializing_002EGenerated;
			GenericWriter<UnloadScenesBroadcast>.Write = Write___FishNet_002EManaging_002EScened_002EUnloadScenesBroadcastFishNet_002ESerializing_002EGenerated;
			GenericWriter<UnloadQueueData>.Write = Write___FishNet_002EManaging_002EScened_002EUnloadQueueDataFishNet_002ESerializing_002EGenerated;
			GenericWriter<SceneUnloadData>.Write = Write___FishNet_002EManaging_002EScened_002ESceneUnloadDataFishNet_002ESerializing_002EGenerated;
			GenericWriter<UnloadParams>.Write = Write___FishNet_002EManaging_002EScened_002EUnloadParamsFishNet_002ESerializing_002EGenerated;
			GenericWriter<UnloadOptions>.Write = Write___FishNet_002EManaging_002EScened_002EUnloadOptionsFishNet_002ESerializing_002EGenerated;
			GenericWriter<UnloadOptions.ServerUnloadMode>.Write = Write___FishNet_002EManaging_002EScened_002EUnloadOptions_002FServerUnloadModeFishNet_002ESerializing_002EGenerated;
			GenericWriter<ClientScenesLoadedBroadcast>.Write = Write___FishNet_002EManaging_002EScened_002EClientScenesLoadedBroadcastFishNet_002ESerializing_002EGenerated;
			GenericWriter<SynchronizedProperty>.Write = Write___FishNet_002EComponent_002ETransforming_002ESynchronizedPropertyFishNet_002ESerializing_002EGenerated;
		}

		public static void InstancedExtension___WriteByte(this Writer writer, byte value1)
		{
			writer.WriteByte(value1);
		}

		public static void InstancedExtension___WriteBytesAndSize(this Writer writer, byte[] value1)
		{
			writer.WriteBytesAndSize(value1);
		}

		public static void InstancedExtension___WriteSByte(this Writer writer, sbyte value1)
		{
			writer.WriteSByte(value1);
		}

		public static void InstancedExtension___WriteChar(this Writer writer, char value1)
		{
			writer.WriteChar(value1);
		}

		public static void InstancedExtension___WriteBoolean(this Writer writer, bool value1)
		{
			writer.WriteBoolean(value1);
		}

		public static void InstancedExtension___WriteUInt16(this Writer writer, ushort value1)
		{
			writer.WriteUInt16(value1);
		}

		public static void InstancedExtension___WriteInt16(this Writer writer, short value1)
		{
			writer.WriteInt16(value1);
		}

		public static void InstancedExtension___WriteInt32(this Writer writer, int value1, AutoPackType packType2 = AutoPackType.Packed)
		{
			writer.WriteInt32(value1, packType2);
		}

		public static void InstancedExtension___WriteUInt32(this Writer writer, uint value1, AutoPackType packType2 = AutoPackType.Packed)
		{
			writer.WriteUInt32(value1, packType2);
		}

		public static void InstancedExtension___WriteInt64(this Writer writer, long value1, AutoPackType packType2 = AutoPackType.Packed)
		{
			writer.WriteInt64(value1, packType2);
		}

		public static void InstancedExtension___WriteUInt64(this Writer writer, ulong value1, AutoPackType packType2 = AutoPackType.Packed)
		{
			writer.WriteUInt64(value1, packType2);
		}

		public static void InstancedExtension___WriteSingle(this Writer writer, float value1, AutoPackType packType2 = AutoPackType.Unpacked)
		{
			writer.WriteSingle(value1, packType2);
		}

		public static void InstancedExtension___WriteDouble(this Writer writer, double value1)
		{
			writer.WriteDouble(value1);
		}

		public static void InstancedExtension___WriteDecimal(this Writer writer, decimal value1)
		{
			writer.WriteDecimal(value1);
		}

		public static void InstancedExtension___WriteString(this Writer writer, string value1)
		{
			writer.WriteString(value1);
		}

		public static void InstancedExtension___WriteArraySegmentAndSize(this Writer writer, ArraySegment<byte> value1)
		{
			writer.WriteArraySegmentAndSize(value1);
		}

		public static void InstancedExtension___WriteVector2(this Writer writer, Vector2 value1)
		{
			writer.WriteVector2(value1);
		}

		public static void InstancedExtension___WriteVector3(this Writer writer, Vector3 value1)
		{
			writer.WriteVector3(value1);
		}

		public static void InstancedExtension___WriteVector4(this Writer writer, Vector4 value1)
		{
			writer.WriteVector4(value1);
		}

		public static void InstancedExtension___WriteVector2Int(this Writer writer, Vector2Int value1, AutoPackType packType2 = AutoPackType.Packed)
		{
			writer.WriteVector2Int(value1, packType2);
		}

		public static void InstancedExtension___WriteVector3Int(this Writer writer, Vector3Int value1, AutoPackType packType2 = AutoPackType.Packed)
		{
			writer.WriteVector3Int(value1, packType2);
		}

		public static void InstancedExtension___WriteColor(this Writer writer, Color value1, AutoPackType packType2 = AutoPackType.Packed)
		{
			writer.WriteColor(value1, packType2);
		}

		public static void InstancedExtension___WriteColor32(this Writer writer, Color32 value1)
		{
			writer.WriteColor32(value1);
		}

		public static void InstancedExtension___WriteQuaternion(this Writer writer, Quaternion value1, AutoPackType packType2 = AutoPackType.Packed)
		{
			writer.WriteQuaternion(value1, packType2);
		}

		public static void InstancedExtension___WriteRect(this Writer writer, Rect value1)
		{
			writer.WriteRect(value1);
		}

		public static void InstancedExtension___WritePlane(this Writer writer, Plane value1)
		{
			writer.WritePlane(value1);
		}

		public static void InstancedExtension___WriteRay(this Writer writer, Ray value1)
		{
			writer.WriteRay(value1);
		}

		public static void InstancedExtension___WriteRay2D(this Writer writer, Ray2D value1)
		{
			writer.WriteRay2D(value1);
		}

		public static void InstancedExtension___WriteMatrix4x4(this Writer writer, Matrix4x4 value1)
		{
			writer.WriteMatrix4x4(value1);
		}

		public static void InstancedExtension___WriteGuidAllocated(this Writer writer, Guid value1)
		{
			writer.WriteGuidAllocated(value1);
		}

		public static void InstancedExtension___WriteGameObject(this Writer writer, GameObject go1)
		{
			writer.WriteGameObject(go1);
		}

		public static void InstancedExtension___WriteTransform(this Writer writer, Transform t1)
		{
			writer.WriteTransform(t1);
		}

		public static void InstancedExtension___WriteNetworkObject(this Writer writer, NetworkObject nob1)
		{
			writer.WriteNetworkObject(nob1);
		}

		public static void InstancedExtension___WriteNetworkBehaviour(this Writer writer, NetworkBehaviour nb1)
		{
			writer.WriteNetworkBehaviour(nb1);
		}

		public static void InstancedExtension___WriteDateTime(this Writer writer, DateTime dt1)
		{
			writer.WriteDateTime(dt1);
		}

		public static void InstancedExtension___WriteChannel(this Writer writer, Channel channel1)
		{
			writer.WriteChannel(channel1);
		}

		public static void InstancedExtension___WriteNetworkConnection(this Writer writer, NetworkConnection connection1)
		{
			writer.WriteNetworkConnection(connection1);
		}

		public static void Write___FishNet_002EManaging_002EServer_002EClientConnectionChangeBroadcastFishNet_002ESerializing_002EGenerated(this Writer writer, ClientConnectionChangeBroadcast value)
		{
			InstancedExtension___WriteBoolean(writer, value.Connected);
			InstancedExtension___WriteInt32(writer, value.Id);
		}

		public static void Write___FishNet_002EManaging_002EServer_002EConnectedClientsBroadcastFishNet_002ESerializing_002EGenerated(this Writer writer, ConnectedClientsBroadcast value)
		{
			Write___System_002ECollections_002EGeneric_002EList_00601_003CSystem_002EInt32_003EFishNet_002ESerializing_002EGenerated(writer, value.Values);
		}

		public static void Write___System_002ECollections_002EGeneric_002EList_00601_003CSystem_002EInt32_003EFishNet_002ESerializing_002EGenerated(this Writer writer, List<int> value)
		{
			writer.WriteList(value);
		}

		public static void Write___FishNet_002EManaging_002EScened_002EEmptyStartScenesBroadcastFishNet_002ESerializing_002EGenerated(this Writer writer, EmptyStartScenesBroadcast value)
		{
		}

		public static void Write___FishNet_002EManaging_002EScened_002ELoadScenesBroadcastFishNet_002ESerializing_002EGenerated(this Writer writer, LoadScenesBroadcast value)
		{
			Write___FishNet_002EManaging_002EScened_002ELoadQueueDataFishNet_002ESerializing_002EGenerated(writer, value.QueueData);
		}

		public static void Write___FishNet_002EManaging_002EScened_002ELoadQueueDataFishNet_002ESerializing_002EGenerated(this Writer writer, LoadQueueData value)
		{
			if (value == null)
			{
				InstancedExtension___WriteBoolean(writer, value1: true);
				return;
			}
			InstancedExtension___WriteBoolean(writer, value1: false);
			Write___FishNet_002EManaging_002EScened_002ESceneLoadDataFishNet_002ESerializing_002EGenerated(writer, value.SceneLoadData);
			Write___System_002EString_005B_005DFishNet_002ESerializing_002EGenerated(writer, value.GlobalScenes);
		}

		public static void Write___FishNet_002EManaging_002EScened_002ESceneLoadDataFishNet_002ESerializing_002EGenerated(this Writer writer, SceneLoadData value)
		{
			if (value == null)
			{
				InstancedExtension___WriteBoolean(writer, value1: true);
				return;
			}
			InstancedExtension___WriteBoolean(writer, value1: false);
			Write___FishNet_002EManaging_002EScened_002ESceneLookupDataFishNet_002ESerializing_002EGenerated(writer, value.PreferredActiveScene);
			Write___FishNet_002EManaging_002EScened_002ESceneLookupData_005B_005DFishNet_002ESerializing_002EGenerated(writer, value.SceneLookupDatas);
			Write___FishNet_002EObject_002ENetworkObject_005B_005DFishNet_002ESerializing_002EGenerated(writer, value.MovedNetworkObjects);
			Write___FishNet_002EManaging_002EScened_002EReplaceOptionFishNet_002ESerializing_002EGenerated(writer, value.ReplaceScenes);
			Write___FishNet_002EManaging_002EScened_002ELoadParamsFishNet_002ESerializing_002EGenerated(writer, value.Params);
			Write___FishNet_002EManaging_002EScened_002ELoadOptionsFishNet_002ESerializing_002EGenerated(writer, value.Options);
		}

		public static void Write___FishNet_002EManaging_002EScened_002ESceneLookupDataFishNet_002ESerializing_002EGenerated(this Writer writer, SceneLookupData value)
		{
			if ((object)value == null)
			{
				InstancedExtension___WriteBoolean(writer, value1: true);
				return;
			}
			InstancedExtension___WriteBoolean(writer, value1: false);
			InstancedExtension___WriteInt32(writer, value.Handle);
			InstancedExtension___WriteString(writer, value.Name);
		}

		public static void Write___FishNet_002EManaging_002EScened_002ESceneLookupData_005B_005DFishNet_002ESerializing_002EGenerated(this Writer writer, SceneLookupData[] value)
		{
			writer.WriteArray(value);
		}

		public static void Write___FishNet_002EObject_002ENetworkObject_005B_005DFishNet_002ESerializing_002EGenerated(this Writer writer, NetworkObject[] value)
		{
			writer.WriteArray(value);
		}

		public static void Write___FishNet_002EManaging_002EScened_002EReplaceOptionFishNet_002ESerializing_002EGenerated(this Writer writer, ReplaceOption value)
		{
			InstancedExtension___WriteByte(writer, (byte)value);
		}

		public static void Write___FishNet_002EManaging_002EScened_002ELoadParamsFishNet_002ESerializing_002EGenerated(this Writer writer, LoadParams value)
		{
			if (value == null)
			{
				InstancedExtension___WriteBoolean(writer, value1: true);
				return;
			}
			InstancedExtension___WriteBoolean(writer, value1: false);
			InstancedExtension___WriteBytesAndSize(writer, value.ClientParams);
		}

		public static void Write___FishNet_002EManaging_002EScened_002ELoadOptionsFishNet_002ESerializing_002EGenerated(this Writer writer, LoadOptions value)
		{
			if (value == null)
			{
				InstancedExtension___WriteBoolean(writer, value1: true);
				return;
			}
			InstancedExtension___WriteBoolean(writer, value1: false);
			InstancedExtension___WriteBoolean(writer, value.ReloadScenes);
			InstancedExtension___WriteBoolean(writer, value.Addressables);
		}

		public static void Write___System_002EString_005B_005DFishNet_002ESerializing_002EGenerated(this Writer writer, string[] value)
		{
			writer.WriteArray(value);
		}

		public static void Write___FishNet_002EManaging_002EScened_002EUnloadScenesBroadcastFishNet_002ESerializing_002EGenerated(this Writer writer, UnloadScenesBroadcast value)
		{
			Write___FishNet_002EManaging_002EScened_002EUnloadQueueDataFishNet_002ESerializing_002EGenerated(writer, value.QueueData);
		}

		public static void Write___FishNet_002EManaging_002EScened_002EUnloadQueueDataFishNet_002ESerializing_002EGenerated(this Writer writer, UnloadQueueData value)
		{
			if (value == null)
			{
				InstancedExtension___WriteBoolean(writer, value1: true);
				return;
			}
			InstancedExtension___WriteBoolean(writer, value1: false);
			Write___FishNet_002EManaging_002EScened_002ESceneUnloadDataFishNet_002ESerializing_002EGenerated(writer, value.SceneUnloadData);
			Write___System_002EString_005B_005DFishNet_002ESerializing_002EGenerated(writer, value.GlobalScenes);
		}

		public static void Write___FishNet_002EManaging_002EScened_002ESceneUnloadDataFishNet_002ESerializing_002EGenerated(this Writer writer, SceneUnloadData value)
		{
			if (value == null)
			{
				InstancedExtension___WriteBoolean(writer, value1: true);
				return;
			}
			InstancedExtension___WriteBoolean(writer, value1: false);
			Write___FishNet_002EManaging_002EScened_002ESceneLookupDataFishNet_002ESerializing_002EGenerated(writer, value.PreferredActiveScene);
			Write___FishNet_002EManaging_002EScened_002ESceneLookupData_005B_005DFishNet_002ESerializing_002EGenerated(writer, value.SceneLookupDatas);
			Write___FishNet_002EManaging_002EScened_002EUnloadParamsFishNet_002ESerializing_002EGenerated(writer, value.Params);
			Write___FishNet_002EManaging_002EScened_002EUnloadOptionsFishNet_002ESerializing_002EGenerated(writer, value.Options);
		}

		public static void Write___FishNet_002EManaging_002EScened_002EUnloadParamsFishNet_002ESerializing_002EGenerated(this Writer writer, UnloadParams value)
		{
			if (value == null)
			{
				InstancedExtension___WriteBoolean(writer, value1: true);
				return;
			}
			InstancedExtension___WriteBoolean(writer, value1: false);
			InstancedExtension___WriteBytesAndSize(writer, value.ClientParams);
		}

		public static void Write___FishNet_002EManaging_002EScened_002EUnloadOptionsFishNet_002ESerializing_002EGenerated(this Writer writer, UnloadOptions value)
		{
			if (value == null)
			{
				InstancedExtension___WriteBoolean(writer, value1: true);
				return;
			}
			InstancedExtension___WriteBoolean(writer, value1: false);
			Write___FishNet_002EManaging_002EScened_002EUnloadOptions_002FServerUnloadModeFishNet_002ESerializing_002EGenerated(writer, value.Mode);
			InstancedExtension___WriteBoolean(writer, value.Addressables);
		}

		public static void Write___FishNet_002EManaging_002EScened_002EUnloadOptions_002FServerUnloadModeFishNet_002ESerializing_002EGenerated(this Writer writer, UnloadOptions.ServerUnloadMode value)
		{
			InstancedExtension___WriteInt32(writer, (int)value);
		}

		public static void Write___FishNet_002EManaging_002EScened_002EClientScenesLoadedBroadcastFishNet_002ESerializing_002EGenerated(this Writer writer, ClientScenesLoadedBroadcast value)
		{
			Write___FishNet_002EManaging_002EScened_002ESceneLookupData_005B_005DFishNet_002ESerializing_002EGenerated(writer, value.SceneLookupDatas);
		}

		public static void Write___FishNet_002EComponent_002ETransforming_002ESynchronizedPropertyFishNet_002ESerializing_002EGenerated(this Writer writer, SynchronizedProperty value)
		{
			InstancedExtension___WriteByte(writer, (byte)value);
		}
	}
}
