using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Serializing.Helping;
using FishNet.Transporting;
using FishNet.Utility.Performance;
using GameKit.Utilities;
using UnityEngine;

namespace FishNet.Serializing
{
	public class Reader
	{
		public enum DataSource
		{
			Unset = 0,
			Server = 1,
			Client = 2
		}

		public DataSource Source;

		public NetworkManager NetworkManager;

		public int Position;

		private byte[] _buffer;

		public int Capacity => _buffer.Length;

		public int Offset { get; private set; }

		public int Length { get; private set; }

		public int Remaining => Length + Offset - Position;

		public NetworkConnection NetworkConnection { get; private set; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Reader(byte[] bytes, NetworkManager networkManager, NetworkConnection networkConnection = null, DataSource source = DataSource.Unset)
		{
			Initialize(bytes, networkManager, networkConnection, source);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Reader(ArraySegment<byte> segment, NetworkManager networkManager, NetworkConnection networkConnection = null, DataSource source = DataSource.Unset)
		{
			Initialize(segment, networkManager, networkConnection, source);
		}

		public override string ToString()
		{
			return $"Position: {Position}, Length: {Length}, Buffer: {BitConverter.ToString(_buffer, Offset, Length)}.";
		}

		public string RemainingToString()
		{
			string arg = ((Remaining > 0) ? BitConverter.ToString(_buffer, Position, Remaining) : "null");
			return $"Remaining: {Remaining}, Length: {Length}, Buffer: {arg}.";
		}

		public ArraySegment<byte> GetRemainingData()
		{
			if (Remaining == 0)
			{
				return default(ArraySegment<byte>);
			}
			return new ArraySegment<byte>(_buffer, Position, Remaining);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Initialize(ArraySegment<byte> segment, NetworkManager networkManager, DataSource source = DataSource.Unset)
		{
			Initialize(segment, networkManager, null, source);
		}

		internal void Initialize(ArraySegment<byte> segment, NetworkManager networkManager, NetworkConnection networkConnection = null, DataSource source = DataSource.Unset)
		{
			_buffer = segment.Array;
			if (_buffer == null)
			{
				_buffer = new byte[0];
			}
			Position = segment.Offset;
			Offset = segment.Offset;
			Length = segment.Count;
			NetworkManager = networkManager;
			NetworkConnection = networkConnection;
			Source = source;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Initialize(byte[] bytes, NetworkManager networkManager, DataSource source = DataSource.Unset)
		{
			Initialize(new ArraySegment<byte>(bytes), networkManager, null, source);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Initialize(byte[] bytes, NetworkManager networkManager, NetworkConnection networkConnection = null, DataSource source = DataSource.Unset)
		{
			Initialize(new ArraySegment<byte>(bytes), networkManager, networkConnection, source);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		[Obsolete("Use ReadDictionaryAllocated.")]
		public Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>()
		{
			return ReadDictionaryAllocated<TKey, TValue>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public Dictionary<TKey, TValue> ReadDictionaryAllocated<TKey, TValue>()
		{
			if (ReadBoolean())
			{
				return null;
			}
			int num = ReadInt32();
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(num);
			for (int i = 0; i < num; i++)
			{
				TKey key = Read<TKey>();
				TValue value = Read<TValue>();
				dictionary.Add(key, value);
			}
			return dictionary;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		internal int ReadLength()
		{
			return ReadInt32();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		internal PacketId ReadPacketId()
		{
			return (PacketId)ReadUInt16();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		internal PacketId PeekPacketId()
		{
			int position = Position;
			PacketId result = ReadPacketId();
			Position = position;
			return result;
		}

		internal byte PeekByte()
		{
			return _buffer[Position];
		}

		[CodegenExclude]
		public void Skip(int value)
		{
			if (value >= 1 && Remaining >= value)
			{
				Position += value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public void Clear()
		{
			if (Remaining > 0)
			{
				Skip(Remaining);
			}
		}

		public ArraySegment<byte> GetArraySegmentBuffer()
		{
			return new ArraySegment<byte>(_buffer, Offset, Length);
		}

		public byte[] GetByteBuffer()
		{
			return _buffer;
		}

		public byte[] GetByteBufferAllocated()
		{
			byte[] array = new byte[Length];
			Buffer.BlockCopy(_buffer, Offset, array, 0, Length);
			return array;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlockCopy(ref byte[] target, int targetOffset, int count)
		{
			Buffer.BlockCopy(_buffer, Position, target, targetOffset, count);
			Position += count;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte ReadByte()
		{
			byte result = _buffer[Position];
			Position++;
			return result;
		}

		[CodegenExclude]
		public void ReadBytes(ref byte[] buffer, int count)
		{
			if (buffer == null)
			{
				throw new EndOfStreamException("Target is null.");
			}
			if (count > buffer.Length)
			{
				throw new EndOfStreamException($"Count of {count} exceeds target length of {buffer.Length}.");
			}
			BlockCopy(ref buffer, 0, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public ArraySegment<byte> ReadArraySegment(int count)
		{
			ArraySegment<byte> result = new ArraySegment<byte>(_buffer, Position, count);
			Position += count;
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public sbyte ReadSByte()
		{
			return (sbyte)ReadByte();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public char ReadChar()
		{
			return (char)ReadUInt16();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool ReadBoolean()
		{
			if (ReadByte() != 1)
			{
				return false;
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ushort ReadUInt16()
		{
			return (ushort)((ushort)(0 | _buffer[Position++]) | (ushort)(_buffer[Position++] << 8));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public short ReadInt16()
		{
			return (short)ReadUInt16();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint ReadUInt32(AutoPackType packType = AutoPackType.Packed)
		{
			if (packType == AutoPackType.Packed)
			{
				return (uint)ReadPackedWhole();
			}
			return (uint)(0 | _buffer[Position++] | (_buffer[Position++] << 8) | (_buffer[Position++] << 16) | (_buffer[Position++] << 24));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int ReadInt32(AutoPackType packType = AutoPackType.Packed)
		{
			if (packType == AutoPackType.Packed)
			{
				return (int)ZigZagDecode(ReadPackedWhole());
			}
			return (int)ReadUInt32(packType);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public long ReadInt64(AutoPackType packType = AutoPackType.Packed)
		{
			if (packType == AutoPackType.Packed)
			{
				return (long)ZigZagDecode(ReadPackedWhole());
			}
			return (long)ReadUInt64(packType);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong ReadUInt64(AutoPackType packType = AutoPackType.Packed)
		{
			if (packType == AutoPackType.Packed)
			{
				return ReadPackedWhole();
			}
			return 0uL | (ulong)_buffer[Position++] | ((ulong)_buffer[Position++] << 8) | ((ulong)_buffer[Position++] << 16) | ((ulong)_buffer[Position++] << 24) | ((ulong)_buffer[Position++] << 32) | ((ulong)_buffer[Position++] << 40) | ((ulong)_buffer[Position++] << 48) | ((ulong)_buffer[Position++] << 56);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float ReadSingle(AutoPackType packType = AutoPackType.Unpacked)
		{
			if (packType == AutoPackType.Unpacked)
			{
				UIntFloat uIntFloat = new UIntFloat
				{
					UIntValue = ReadUInt32(AutoPackType.Unpacked)
				};
				return uIntFloat.FloatValue;
			}
			return (float)(long)ReadPackedWhole() / 100f;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double ReadDouble()
		{
			UIntDouble uIntDouble = new UIntDouble
			{
				LongValue = ReadUInt64(AutoPackType.Unpacked)
			};
			return uIntDouble.DoubleValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public decimal ReadDecimal()
		{
			UIntDecimal uIntDecimal = new UIntDecimal
			{
				LongValue1 = ReadUInt64(AutoPackType.Unpacked),
				LongValue2 = ReadUInt64(AutoPackType.Unpacked)
			};
			return uIntDecimal.DecimalValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ReadString()
		{
			int num = ReadInt32();
			switch (num)
			{
			case -1:
				return null;
			case 0:
				return string.Empty;
			default:
				if (!CheckAllocationAttack(num))
				{
					return string.Empty;
				}
				return ReaderStatics.GetString(ReadArraySegment(num));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte[] ReadBytesAndSizeAllocated()
		{
			int num = ReadInt32();
			if (num == -1)
			{
				return null;
			}
			return ReadBytesAllocated(num);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public int ReadBytesAndSize(ref byte[] target)
		{
			int num = ReadInt32();
			if (num > 0)
			{
				ReadBytes(ref target, num);
			}
			return num;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ArraySegment<byte> ReadArraySegmentAndSize()
		{
			int num = ReadInt32();
			if (num == -1)
			{
				return default(ArraySegment<byte>);
			}
			return ReadArraySegment(num);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2 ReadVector2()
		{
			return new Vector2(ReadSingle(), ReadSingle());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3 ReadVector3()
		{
			return new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4 ReadVector4()
		{
			return new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2Int ReadVector2Int(AutoPackType packType = AutoPackType.Packed)
		{
			return new Vector2Int(ReadInt32(packType), ReadInt32(packType));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3Int ReadVector3Int(AutoPackType packType = AutoPackType.Packed)
		{
			return new Vector3Int(ReadInt32(packType), ReadInt32(packType), ReadInt32(packType));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Color ReadColor(AutoPackType packType = AutoPackType.Packed)
		{
			float r;
			float g;
			float b;
			float a;
			if (packType == AutoPackType.Unpacked)
			{
				r = ReadSingle();
				g = ReadSingle();
				b = ReadSingle();
				a = ReadSingle();
			}
			else
			{
				r = (float)(int)ReadByte() / 100f;
				g = (float)(int)ReadByte() / 100f;
				b = (float)(int)ReadByte() / 100f;
				a = (float)(int)ReadByte() / 100f;
			}
			return new Color(r, g, b, a);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Color32 ReadColor32()
		{
			return new Color32(ReadByte(), ReadByte(), ReadByte(), ReadByte());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Quaternion ReadQuaternion(AutoPackType packType = AutoPackType.Packed)
		{
			return packType switch
			{
				AutoPackType.Packed => Quaternion32Compression.Decompress(ReadUInt32(AutoPackType.Unpacked)), 
				AutoPackType.PackedLess => Quaternion64Compression.Decompress(ReadUInt64(AutoPackType.Unpacked)), 
				_ => new Quaternion(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle()), 
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rect ReadRect()
		{
			return new Rect(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Plane ReadPlane()
		{
			return new Plane(ReadVector3(), ReadSingle());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Ray ReadRay()
		{
			Vector3 origin = ReadVector3();
			Vector3 direction = ReadVector3();
			return new Ray(origin, direction);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Ray2D ReadRay2D()
		{
			Vector3 vector = ReadVector2();
			return new Ray2D(direction: ReadVector2(), origin: vector);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Matrix4x4 ReadMatrix4x4()
		{
			return new Matrix4x4
			{
				m00 = ReadSingle(),
				m01 = ReadSingle(),
				m02 = ReadSingle(),
				m03 = ReadSingle(),
				m10 = ReadSingle(),
				m11 = ReadSingle(),
				m12 = ReadSingle(),
				m13 = ReadSingle(),
				m20 = ReadSingle(),
				m21 = ReadSingle(),
				m22 = ReadSingle(),
				m23 = ReadSingle(),
				m30 = ReadSingle(),
				m31 = ReadSingle(),
				m32 = ReadSingle(),
				m33 = ReadSingle()
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public byte[] ReadBytesAllocated(int count)
		{
			byte[] buffer = new byte[count];
			ReadBytes(ref buffer, count);
			return buffer;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Guid ReadGuid()
		{
			byte[] buffer = ReaderStatics.GetGuidBuffer();
			ReadBytes(ref buffer, 16);
			return new Guid(buffer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public uint ReadTickUnpacked()
		{
			return ReadUInt32(AutoPackType.Unpacked);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GameObject ReadGameObject()
		{
			byte b = ReadByte();
			GameObject result;
			switch (b)
			{
			case 0:
				result = null;
				break;
			case 1:
			{
				NetworkObject networkObject = ReadNetworkObject();
				result = ((networkObject == null) ? null : networkObject.gameObject);
				break;
			}
			case 2:
			{
				NetworkBehaviour networkBehaviour = ReadNetworkBehaviour();
				result = ((networkBehaviour == null) ? null : networkBehaviour.gameObject);
				break;
			}
			default:
				result = null;
				LogError($"Unhandled ReadGameObject type of {b}.");
				break;
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Transform ReadTransform()
		{
			NetworkObject networkObject = ReadNetworkObject();
			if (!(networkObject == null))
			{
				return networkObject.transform;
			}
			return null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public NetworkObject ReadNetworkObject()
		{
			int objectOrPrefabId;
			return ReadNetworkObject(out objectOrPrefabId);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public NetworkObject ReadNetworkObject(out int objectOrPrefabId, HashSet<int> readSpawningObjects = null)
		{
			objectOrPrefabId = ReadNetworkObjectId();
			if (objectOrPrefabId == 65535)
			{
				return null;
			}
			bool flag = ReadBoolean();
			bool started = NetworkManager.ServerManager.Started;
			bool started2 = NetworkManager.ClientManager.Started;
			NetworkObject value;
			if (flag)
			{
				value = null;
				if (started2)
				{
					NetworkManager.ClientManager.Objects.Spawned.TryGetValueIL2CPP(objectOrPrefabId, out value);
				}
				if (value == null && started)
				{
					NetworkManager.ServerManager.Objects.Spawned.TryGetValueIL2CPP(objectOrPrefabId, out value);
				}
				if (value == null && !started && (readSpawningObjects == null || !readSpawningObjects.Contains(objectOrPrefabId)))
				{
					LogWarning($"Spawned NetworkObject was expected to exist but does not for Id {objectOrPrefabId}. This may occur if you sent a NetworkObject reference which does not exist, be it destroyed or if the client does not have visibility.");
				}
			}
			else
			{
				bool asServer = !started2;
				value = NetworkManager.GetPrefab(objectOrPrefabId, asServer);
			}
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public int ReadNetworkObjectId()
		{
			return ReadUInt16();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		internal int ReadNetworkObjectForSpawn(out sbyte initializeOrder, out ushort collectionid, out bool spawned)
		{
			int num = ReadNetworkObjectId();
			if (num == 65535)
			{
				initializeOrder = 0;
				collectionid = 0;
				spawned = false;
				return num;
			}
			collectionid = ReadUInt16();
			initializeOrder = ReadSByte();
			spawned = ReadBoolean();
			return num;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		internal int ReadNetworkObjectForDepawn(out DespawnType dt)
		{
			int result = ReadNetworkObjectId();
			dt = (DespawnType)ReadByte();
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		internal byte ReadNetworkBehaviourId(out int objectId)
		{
			objectId = ReadNetworkObjectId();
			if (objectId != 65535)
			{
				return ReadByte();
			}
			return 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public NetworkBehaviour ReadNetworkBehaviour(out int objectId, out byte componentIndex, HashSet<int> readSpawningObjects = null)
		{
			NetworkObject networkObject = ReadNetworkObject(out objectId, readSpawningObjects);
			componentIndex = ReadByte();
			if (networkObject == null)
			{
				return null;
			}
			if (componentIndex >= networkObject.NetworkBehaviours.Length)
			{
				NetworkManager.LogError($"ComponentIndex of {componentIndex} is out of bounds on {networkObject.gameObject.name} [id {networkObject.ObjectId}]. This may occur if you have modified your gameObject/prefab without saving it, or the scene.");
				return null;
			}
			return networkObject.NetworkBehaviours[componentIndex];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public NetworkBehaviour ReadNetworkBehaviour()
		{
			int objectId;
			byte componentIndex;
			return ReadNetworkBehaviour(out objectId, out componentIndex);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DateTime ReadDateTime()
		{
			return DateTime.FromBinary(ReadInt64());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Channel ReadChannel()
		{
			return (Channel)ReadByte();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public int ReadNetworkConnectionId()
		{
			return ReadInt16();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public NetworkConnection ReadNetworkConnection()
		{
			int num = ReadNetworkConnectionId();
			if (num == -1)
			{
				return NetworkManager.EmptyConnection;
			}
			if (NetworkManager.IsServer)
			{
				if (NetworkManager.ServerManager.Clients.TryGetValueIL2CPP(num, out var value))
				{
					return value;
				}
				if (NetworkManager.IsClient)
				{
					if (NetworkManager.ClientManager.Clients.TryGetValueIL2CPP(num, out value))
					{
						return value;
					}
					return new NetworkConnection(NetworkManager, num, -1, asServer: true);
				}
				NetworkManager.LogWarning($"Unable to find connection for read Id {num}. An empty connection will be returned.");
				return NetworkManager.EmptyConnection;
			}
			if (num == NetworkManager.ClientManager.Connection.ClientId)
			{
				return NetworkManager.ClientManager.Connection;
			}
			if (NetworkManager.ClientManager.Clients.TryGetValueIL2CPP(num, out var value2))
			{
				return value2;
			}
			return new NetworkConnection(NetworkManager, num, -1, asServer: true);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool CheckAllocationAttack(int size)
		{
			if (size != -1 && size < 0)
			{
				NetworkManager.LogError($"Size of {size} is invalid.");
				return false;
			}
			if (size > Remaining)
			{
				NetworkManager.LogError($"Read size of {size} is larger than remaining data of {Remaining}.");
				return false;
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		internal void ReadStateUpdatePacket(out uint clientTick)
		{
			clientTick = ReadTickUnpacked();
		}

		public ulong ZigZagDecode(ulong value)
		{
			ulong num = value << 63;
			if (num != 0)
			{
				return ~(value >> 1) | num;
			}
			return value >> 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public ulong ReadPackedWhole()
		{
			byte b = ReadByte();
			ulong num = (ulong)(b & 0x7F);
			if ((b & 0x80) == 0)
			{
				return num;
			}
			b = ReadByte();
			num |= (ulong)((long)(b & 0x7F) << 7);
			if ((b & 0x80) == 0)
			{
				return num;
			}
			b = ReadByte();
			num |= (ulong)((long)(b & 0x7F) << 14);
			if ((b & 0x80) == 0)
			{
				return num;
			}
			b = ReadByte();
			num |= (ulong)((long)(b & 0x7F) << 21);
			if ((b & 0x80) == 0)
			{
				return num;
			}
			b = ReadByte();
			num |= (ulong)((long)(b & 0xF) << 28);
			switch (b >> 4)
			{
			case 1:
				num |= (ulong)ReadByte() << 32;
				break;
			case 2:
				num |= (ulong)ReadByte() << 32;
				num |= (ulong)ReadByte() << 40;
				break;
			case 3:
				num |= (ulong)ReadByte() << 32;
				num |= (ulong)ReadByte() << 40;
				num |= (ulong)ReadByte() << 48;
				break;
			case 4:
				num |= (ulong)ReadByte() << 32;
				num |= (ulong)ReadByte() << 40;
				num |= (ulong)ReadByte() << 48;
				num |= (ulong)ReadByte() << 56;
				break;
			}
			return num;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		internal int ReadReplicate<T>(ref T[] collection, uint tick) where T : IReplicateData
		{
			int num = ReadByte();
			if (collection == null || collection.Length < num)
			{
				collection = new T[num];
			}
			tick -= (uint)(num - 1);
			int num2 = ReadByte();
			if (num2 > 0)
			{
				T val;
				switch (num2)
				{
				case 4:
					val = default(T);
					break;
				case 3:
					val = Read<T>();
					break;
				default:
					val = default(T);
					NetworkManager?.LogError($"Unhandled Replicate pack type {num2}.");
					break;
				}
				for (int i = 0; i < num; i++)
				{
					collection[i] = val;
					collection[i].SetTick(tick + (uint)i);
				}
			}
			else
			{
				T val2 = default(T);
				for (int j = 0; j < num; j++)
				{
					T val3 = default(T);
					switch (ReadByte())
					{
					case 1:
						val3 = val2;
						break;
					case 2:
						val3 = Read<T>();
						val2 = val3;
						break;
					case 0:
						val3 = default(T);
						break;
					}
					val3.SetTick(tick + (uint)j);
					collection[j] = val3;
				}
			}
			return num;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public ListCache<T> ReadListCacheAllocated<T>()
		{
			List<T> collection = ReadListAllocated<T>();
			return new ListCache<T>
			{
				Collection = collection
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public int ReadListCache<T>(ref ListCache<T> listCache)
		{
			listCache.Collection = ReadListAllocated<T>();
			return listCache.Collection.Count;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public List<T> ReadListAllocated<T>()
		{
			List<T> collection = null;
			ReadList(ref collection);
			return collection;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public int ReadList<T>(ref List<T> collection, bool allowNullification = false)
		{
			int num = ReadInt32();
			if (num == -1)
			{
				if (allowNullification)
				{
					collection = null;
				}
				return -1;
			}
			if (collection == null)
			{
				collection = new List<T>(num);
			}
			else
			{
				collection.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				collection.Add(Read<T>());
			}
			return num;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public T[] ReadArrayAllocated<T>()
		{
			T[] collection = null;
			ReadArray(ref collection);
			return collection;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public int ReadArray<T>(ref T[] collection)
		{
			int num = ReadInt32();
			switch (num)
			{
			case -1:
				return 0;
			case 0:
				if (collection == null)
				{
					collection = new T[0];
				}
				return 0;
			default:
			{
				if (collection == null)
				{
					collection = new T[num];
				}
				else if (collection.Length < num)
				{
					Array.Resize(ref collection, num);
				}
				for (int i = 0; i < num; i++)
				{
					collection[i] = Read<T>();
				}
				return num;
			}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenExclude]
		public T Read<T>()
		{
			Type type = typeof(T);
			if (IsAutoPackType(type, out var packType))
			{
				Func<Reader, AutoPackType, T> readAutoPack = GenericReader<T>.ReadAutoPack;
				if (readAutoPack == null)
				{
					LogError(GetLogMessage());
					return default(T);
				}
				return readAutoPack(this, packType);
			}
			Func<Reader, T> read = GenericReader<T>.Read;
			if (read == null)
			{
				LogError(GetLogMessage());
				return default(T);
			}
			return read(this);
			string GetLogMessage()
			{
				return "Read method not found for " + type.FullName + ". Use a supported type or create a custom serializer.";
			}
		}

		private void LogWarning(string msg)
		{
			if (NetworkManager == null)
			{
				NetworkManager.StaticLogWarning(msg);
			}
			else
			{
				NetworkManager.LogWarning(msg);
			}
		}

		private void LogError(string msg)
		{
			if (NetworkManager == null)
			{
				NetworkManager.StaticLogError(msg);
			}
			else
			{
				NetworkManager.LogError(msg);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool IsAutoPackType<T>(out AutoPackType packType)
		{
			return Writer.IsAutoPackType<T>(out packType);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool IsAutoPackType(Type type, out AutoPackType packType)
		{
			return Writer.IsAutoPackType(type, out packType);
		}
	}
}
