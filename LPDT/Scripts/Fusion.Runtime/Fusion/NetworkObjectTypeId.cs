using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Fusion.Sockets;

namespace Fusion
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	[InlineHelp]
	[NetworkStructWeaved(2)]
	public struct NetworkObjectTypeId : INetworkStruct, IEquatable<NetworkObjectTypeId>
	{
		public sealed class EqualityComparer : IEqualityComparer<NetworkObjectTypeId>
		{
			public bool Equals(NetworkObjectTypeId x, NetworkObjectTypeId y)
			{
				return x.Equals(y);
			}

			public int GetHashCode(NetworkObjectTypeId obj)
			{
				return obj.GetHashCode();
			}
		}

		public const int ALIGNMENT = 4;

		private const int KIND_MASK = 3;

		private const int KIND_BITS = 2;

		private const int SCENE_OBJECT_INDEX_SHIFT = 2;

		private const int SCENE_OBJECT_INDEX_BITS = 22;

		private const int SCENE_OBJECT_INDEX_MASK = 4194303;

		private const int SCENE_OBJECT_LOAD_ID_SHIFT = 24;

		private const int SCENE_OBJECT_LOAD_ID_BITS = 8;

		public const int MAX_SCENE_OBJECT_INDEX = 4194303;

		private const ushort STRUCT_TYPE_PLAYERDATA = 1;

		[FieldOffset(0)]
		private uint _value0;

		[FieldOffset(4)]
		private uint _value1;

		public const int SIZE = 8;

		public const int WORD_COUNT = 2;

		internal const int BYTE_OF__VALUE0 = 0;

		internal const int BYTE_COUNT_OF__VALUE0 = 4;

		internal const int BYTE_OF__VALUE1 = 4;

		internal const int BYTE_COUNT_OF__VALUE1 = 4;

		private const uint __STATIC_ASSERT_ENSURE_PERFECT_FIT = 1u;

		public static EqualityComparer Comparer { get; } = new EqualityComparer();

		public static NetworkObjectTypeId PlayerData => FromStruct(1);

		public readonly NetworkTypeIdKind Kind
		{
			get
			{
				if (_value0 == 0 && _value1 == 0)
				{
					return NetworkTypeIdKind.Invalid;
				}
				NetworkTypeIdKind networkTypeIdKind = (NetworkTypeIdKind)(_value1 & 3);
				if (networkTypeIdKind == NetworkTypeIdKind.Prefab)
				{
					return (_value1 != (_value1 & 3)) ? NetworkTypeIdKind.PrefabNested : NetworkTypeIdKind.Prefab;
				}
				return networkTypeIdKind;
			}
		}

		public readonly NetworkSceneObjectId AsSceneObjectId
		{
			get
			{
				if (!IsSceneObject)
				{
					throw new InvalidOperationException($"Invalid kind, got {Kind}, expected {NetworkTypeIdKind.SceneObject}");
				}
				SceneRef scene = SceneRef.FromRaw(_value0);
				int objectId = (int)((_value1 >> 2) & 0x3FFFFF);
				byte b = (byte)(_value1 >> 24);
				return new NetworkSceneObjectId
				{
					ObjectId = objectId,
					Scene = scene,
					LoadId = b
				};
			}
		}

		public readonly NetworkPrefabId AsPrefabId
		{
			get
			{
				if (!IsPrefab)
				{
					throw new InvalidOperationException($"Invalid kind, got {Kind}, expected {NetworkTypeIdKind.Prefab}");
				}
				return NetworkPrefabId.FromRaw(_value0);
			}
		}

		public readonly (NetworkPrefabId PrefabId, int Index) AsNestedPrefabId
		{
			get
			{
				if (!IsPrefabNestedObject)
				{
					throw new InvalidOperationException($"Invalid kind, got {Kind}, expected {NetworkTypeIdKind.PrefabNested}");
				}
				NetworkPrefabId item = NetworkPrefabId.FromRaw(_value0);
				int num = (int)((_value1 >> 2) & 0x3FFFFF);
				num--;
				return (PrefabId: item, Index: num);
			}
		}

		public readonly uint AsCustom
		{
			get
			{
				if (!IsCustom)
				{
					throw new InvalidOperationException($"Invalid kind, got {Kind}, expected {NetworkTypeIdKind.Custom}");
				}
				return _value0;
			}
		}

		public readonly ushort AsInternalStructId
		{
			get
			{
				if (!IsStruct)
				{
					throw new InvalidOperationException($"Invalid kind, got {Kind}, expected {NetworkTypeIdKind.InternalStruct}");
				}
				return (ushort)_value0;
			}
		}

		public readonly bool IsNone => Kind == NetworkTypeIdKind.Invalid;

		public readonly bool IsValid => Kind != NetworkTypeIdKind.Invalid;

		public readonly bool IsSceneObject => Kind == NetworkTypeIdKind.SceneObject;

		public readonly bool IsPrefab => Kind == NetworkTypeIdKind.Prefab;

		public readonly bool IsPrefabNestedObject => Kind == NetworkTypeIdKind.PrefabNested;

		public readonly bool IsStruct => Kind == NetworkTypeIdKind.InternalStruct;

		public readonly bool IsCustom => Kind == NetworkTypeIdKind.Custom;

		public static NetworkObjectTypeId FromSceneRefAndObjectIndex(SceneRef sceneRef, int objIndex, NetworkSceneLoadId loadId = default(NetworkSceneLoadId))
		{
			return FromSceneObjectId(new NetworkSceneObjectId
			{
				Scene = sceneRef,
				ObjectId = objIndex,
				LoadId = loadId
			});
		}

		public static NetworkObjectTypeId FromSceneObjectId(NetworkSceneObjectId sceneObjectId)
		{
			if (!sceneObjectId.Scene.IsValid)
			{
				throw new ArgumentException("SceneRef is not valid", "sceneObjectId");
			}
			if (sceneObjectId.ObjectId < 0 || sceneObjectId.ObjectId > 4194303)
			{
				throw new ArgumentException("ObjectId is out of range", "sceneObjectId");
			}
			NetworkObjectTypeId result = default(NetworkObjectTypeId);
			result._value0 = sceneObjectId.Scene.RawValue;
			result._value1 = (uint)(3 | (sceneObjectId.ObjectId << 2) | (sceneObjectId.LoadId.Value << 24));
			return result;
		}

		public static NetworkObjectTypeId FromPrefabId(NetworkPrefabId prefabId)
		{
			if (!prefabId.IsValid)
			{
				throw new ArgumentException("PrefabId is not valid", "prefabId");
			}
			NetworkObjectTypeId result = default(NetworkObjectTypeId);
			result._value0 = prefabId.RawValue;
			result._value1 = 0u;
			return result;
		}

		public static NetworkObjectTypeId FromPrefabNestedObject(NetworkPrefabId prefabId, int index)
		{
			if (!prefabId.IsValid)
			{
				throw new ArgumentException("PrefabId is not valid", "prefabId");
			}
			if (index < 0 || index > 4194303)
			{
				throw new ArgumentException("ObjectId is out of range", "index");
			}
			NetworkObjectTypeId result = default(NetworkObjectTypeId);
			result._value0 = prefabId.RawValue;
			result._value1 = (uint)(0 | (index + 1 << 2));
			return result;
		}

		public static NetworkObjectTypeId FromCustom(uint raw)
		{
			NetworkObjectTypeId result = default(NetworkObjectTypeId);
			result._value0 = raw;
			result._value1 = 1u;
			return result;
		}

		public static NetworkObjectTypeId FromStruct(ushort structId)
		{
			NetworkObjectTypeId result = default(NetworkObjectTypeId);
			result._value0 = structId;
			result._value1 = 2u;
			return result;
		}

		public readonly bool Equals(NetworkObjectTypeId other)
		{
			return _value0 == other._value0 && _value1 == other._value1;
		}

		public override readonly int GetHashCode()
		{
			int value = (int)_value0;
			return (value * 397) ^ (int)_value1;
		}

		public override readonly bool Equals(object obj)
		{
			return obj is NetworkObjectTypeId other && Equals(other);
		}

		public override readonly string ToString()
		{
			if (!IsValid)
			{
				return "[None]";
			}
			NetworkTypeIdKind kind = Kind;
			if (1 == 0)
			{
			}
			string result = kind switch
			{
				NetworkTypeIdKind.InternalStruct => $"[Struct 0x{AsInternalStructId:X4}]", 
				NetworkTypeIdKind.Custom => $"[Custom 0x{AsCustom:X8}]", 
				NetworkTypeIdKind.Prefab => $"[Prefab {AsPrefabId.AsIndex}]", 
				NetworkTypeIdKind.PrefabNested => $"[PrefabNested {AsNestedPrefabId.PrefabId.AsIndex}:{AsNestedPrefabId.Index}]", 
				NetworkTypeIdKind.SceneObject => AsSceneObjectId.ToString(), 
				_ => "[Invalid]", 
			};
			if (1 == 0)
			{
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(NetworkObjectTypeId a, NetworkObjectTypeId b)
		{
			return a._value0 == b._value0 && a._value1 == b._value1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(NetworkObjectTypeId a, NetworkObjectTypeId b)
		{
			return a._value0 != b._value0 || a._value1 != b._value1;
		}

		public static implicit operator NetworkObjectTypeId(NetworkPrefabId prefabId)
		{
			return FromPrefabId(prefabId);
		}

		internal unsafe static void WriteInternal(NetworkObjectTypeId typeId, NetBitBuffer* buffer, int blockSize)
		{
			buffer->WriteUInt32VarLength(typeId._value0, blockSize);
			buffer->WriteUInt32VarLength(typeId._value1, blockSize);
		}

		internal unsafe static NetworkObjectTypeId ReadInternal(NetBitBuffer* buffer, int blockSize)
		{
			return new NetworkObjectTypeId
			{
				_value0 = buffer->ReadUInt32VarLength(blockSize),
				_value1 = buffer->ReadUInt32VarLength(blockSize)
			};
		}
	}
}
