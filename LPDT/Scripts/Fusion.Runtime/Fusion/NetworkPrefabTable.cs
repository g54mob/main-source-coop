#define TRACE
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Fusion
{
	[Serializable]
	public class NetworkPrefabTable
	{
		private struct PrefabAcquireData
		{
			public uint RawValue;

			public int InstanceCount
			{
				readonly get
				{
					return (int)(RawValue & 0x7FFFFFFF);
				}
				set
				{
					RawValue = (RawValue & 0x80000000u) | (uint)(value & 0x7FFFFFFF);
				}
			}

			public bool IsSynchronous
			{
				readonly get
				{
					return (RawValue & 0x80000000u) != 0;
				}
				set
				{
					RawValue = (RawValue & 0x7FFFFFFF) | (uint)(value ? int.MinValue : 0);
				}
			}
		}

		public NetworkPrefabTableOptions Options = NetworkPrefabTableOptions.Default;

		private List<INetworkPrefabSource> _sources = new List<INetworkPrefabSource>();

		private BitSet64[] _acquireMask = Array.Empty<BitSet64>();

		private const int BitsPerMask = 64;

		private PrefabAcquireData[] _acquireData = Array.Empty<PrefabAcquireData>();

		private Dictionary<NetworkObjectGuid, int> _guidToIndex = new Dictionary<NetworkObjectGuid, int>();

		private Dictionary<string, int> _pathToIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

		private Dictionary<string, int> _nameToIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

		private int _version;

		public IReadOnlyList<INetworkPrefabSource> Prefabs => _sources;

		public int Version => _version;

		public IEnumerable<(NetworkPrefabId, INetworkPrefabSource)> GetEntries()
		{
			int i = 0;
			while (i < _sources.Count)
			{
				yield return (NetworkPrefabId.FromIndex(i), _sources[i]);
				int num = i + 1;
				i = num;
			}
		}

		public NetworkPrefabId AddSource(INetworkPrefabSource source)
		{
			if (!TryAddSource(source, out var id))
			{
				throw new ArgumentException($"Source with guid {source.AssetGuid} already exists: {id}", "source");
			}
			return id;
		}

		public bool TryAddSource(INetworkPrefabSource source, out NetworkPrefabId id)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			NetworkObjectGuid assetGuid = source.AssetGuid;
			if (assetGuid.IsValid)
			{
				if (_guidToIndex.TryGetValue(assetGuid, out var value))
				{
					id = NetworkPrefabId.FromIndex(value);
					return false;
				}
				_guidToIndex.Add(assetGuid, _sources.Count);
			}
			string assetPath = source.AssetPath;
			if (!string.IsNullOrEmpty(assetPath))
			{
				if (_pathToIndex.TryGetValue(assetPath, out var value2))
				{
					id = NetworkPrefabId.FromIndex(value2);
					return false;
				}
				_pathToIndex.Add(assetPath, _sources.Count);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assetPath);
				if (!string.IsNullOrEmpty(fileNameWithoutExtension) && !_nameToIndex.TryAdd(fileNameWithoutExtension, _sources.Count))
				{
					InternalLogStreams.LogTracePrefab?.Warn($"Prefab {source.Description} not added to name lookup: there already is one with the same name ({NetworkPrefabId.FromIndex(_nameToIndex[fileNameWithoutExtension])})");
				}
			}
			_sources.Add(source);
			if (_acquireMask.Length < GetBitSetCapacity(_sources.Capacity))
			{
				Array.Resize(ref _acquireMask, GetBitSetCapacity(_sources.Capacity));
			}
			if (_acquireData.Length < _sources.Capacity)
			{
				Array.Resize(ref _acquireData, _sources.Capacity);
			}
			id = NetworkPrefabId.FromIndex(_sources.Count - 1);
			InternalLogStreams.LogTracePrefab?.Log($"Added prefab source {id}: {source.Description}");
			_version++;
			return true;
		}

		public INetworkPrefabSource GetSource(NetworkObjectGuid guid)
		{
			if (_guidToIndex.TryGetValue(guid, out var value))
			{
				return _sources[value];
			}
			return null;
		}

		public INetworkPrefabSource GetSource(NetworkPrefabId prefabId)
		{
			if (!TryDecodePrefabId(prefabId, out var index))
			{
				return null;
			}
			return _sources[index];
		}

		public NetworkPrefabId GetId(NetworkObjectGuid guid)
		{
			if (_guidToIndex.TryGetValue(guid, out var value))
			{
				return NetworkPrefabId.FromIndex(value);
			}
			return default(NetworkPrefabId);
		}

		public NetworkPrefabId GetId(string nameOrPath)
		{
			if (_pathToIndex.TryGetValue(nameOrPath, out var value))
			{
				return NetworkPrefabId.FromIndex(value);
			}
			if (_nameToIndex.TryGetValue(nameOrPath, out value))
			{
				return NetworkPrefabId.FromIndex(value);
			}
			return default(NetworkPrefabId);
		}

		public NetworkObjectGuid GetGuid(NetworkPrefabId prefabId)
		{
			if (!TryDecodePrefabId(prefabId, out var index))
			{
				return default(NetworkObjectGuid);
			}
			return _sources[index].AssetGuid;
		}

		public int GetInstancesCount(NetworkPrefabId prefabId)
		{
			if (!TryDecodePrefabId(prefabId, out var index) || !IsAcquired(index))
			{
				return 0;
			}
			return _acquireData[index].InstanceCount;
		}

		public int AddInstance(NetworkPrefabId prefabId)
		{
			if (!TryDecodePrefabId(prefabId, out var index) || !IsAcquired(index))
			{
				return 0;
			}
			InternalLogStreams.LogTracePrefab?.Log($"Increasing {prefabId} instance count (to {_acquireData[index].InstanceCount + 1})");
			_version++;
			return ++_acquireData[index].InstanceCount;
		}

		public int RemoveInstance(NetworkPrefabId prefabId)
		{
			if (!TryDecodePrefabId(prefabId, out var index) || !IsAcquired(index))
			{
				return 0;
			}
			if (_acquireData[index].InstanceCount == 0)
			{
				InternalLogStreams.LogTracePrefab?.Warn($"Can't decrease {prefabId} instance count below zero");
				return 0;
			}
			InternalLogStreams.LogTracePrefab?.Log($"Decreasing {prefabId} instance count (to {_acquireData[index].InstanceCount - 1})");
			int num = --_acquireData[index].InstanceCount;
			if (num == 0 && Options.UnloadPrefabOnReleasingLastInstance)
			{
				UnloadInternal(index);
			}
			_version++;
			return num;
		}

		public bool Contains(NetworkPrefabId prefabId)
		{
			int index;
			return TryDecodePrefabId(prefabId, out index);
		}

		public bool IsAcquired(NetworkPrefabId prefabId)
		{
			if (!TryDecodePrefabId(prefabId, out var index))
			{
				return false;
			}
			return IsAcquired(index);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsAcquired(int index)
		{
			int num = index / 64;
			return _acquireMask[num][index & 0x3F];
		}

		private void SetAcquired(int index, bool value)
		{
			int num = index / 64;
			_acquireMask[num][index & 0x3F] = value;
			_version++;
		}

		public NetworkObject Load(NetworkPrefabId prefabId, bool isSynchronous)
		{
			int num = DecodePrefabId(prefabId);
			INetworkPrefabSource networkPrefabSource = _sources[num];
			if (!IsAcquired(num))
			{
				InternalLogStreams.LogTracePrefab?.Log($"Acquiring {prefabId} ({networkPrefabSource.Description})");
				networkPrefabSource.Acquire(isSynchronous);
				SetAcquired(num, value: true);
				_acquireData[num] = new PrefabAcquireData
				{
					IsSynchronous = isSynchronous
				};
			}
			if (!networkPrefabSource.IsCompleted)
			{
				if (!isSynchronous || _acquireData[num].IsSynchronous)
				{
					return null;
				}
				networkPrefabSource.Release();
				networkPrefabSource.Acquire(synchronous: true);
				_acquireData[num].IsSynchronous = true;
				if (!networkPrefabSource.IsCompleted)
				{
					return null;
				}
			}
			NetworkObject networkObject = networkPrefabSource.WaitForResult();
			Assert.Always(networkObject, "Source for {0} returned null", prefabId);
			return networkObject;
		}

		public bool Unload(NetworkPrefabId prefabId)
		{
			if (!TryDecodePrefabId(prefabId, out var index))
			{
				return false;
			}
			if (!IsAcquired(index))
			{
				return false;
			}
			UnloadInternal(index);
			return true;
		}

		public int UnloadUnreferenced(bool includeIncompleteLoads = false)
		{
			int result = 0;
			for (int i = 0; i < _sources.Count; i += 64)
			{
				BitSet64 bitSet = _acquireMask[i / 64];
				if (!bitSet.Any())
				{
					continue;
				}
				foreach (int item in bitSet)
				{
					if (i + item >= _sources.Count)
					{
						break;
					}
					int index = i + item;
					if (!includeIncompleteLoads && _sources[index].IsCompleted)
					{
						InternalLogStreams.LogTracePrefab?.Log($"Not unloading {NetworkPrefabId.FromIndex(index)}: incomplete load");
					}
					else
					{
						UnloadInternal(i + item);
					}
				}
			}
			return result;
		}

		public void UnloadAll()
		{
			for (int i = 0; i < _sources.Count; i++)
			{
				Unload(NetworkPrefabId.FromIndex(i));
			}
		}

		public void Clear()
		{
			UnloadAll();
			Array.Clear(_acquireData, 0, _sources.Count);
			_acquireMask = Array.Empty<BitSet64>();
			_sources.Clear();
			_guidToIndex.Clear();
			_nameToIndex.Clear();
			_pathToIndex.Clear();
			_version = 0;
		}

		private void UnloadInternal(int index)
		{
			InternalLogStreams.LogTracePrefab?.Log($"Unloading {NetworkPrefabId.FromIndex(index)}");
			INetworkPrefabSource networkPrefabSource = _sources[index];
			networkPrefabSource.Release();
			SetAcquired(index, value: false);
			_version++;
		}

		private int DecodePrefabId(NetworkPrefabId prefabId)
		{
			if (!prefabId.IsValid)
			{
				throw new ArgumentException("Invalid prefab id", "prefabId");
			}
			if (prefabId.AsIndex >= _sources.Count)
			{
				throw new ArgumentException($"Unknown prefab id: {prefabId}", "prefabId");
			}
			return prefabId.AsIndex;
		}

		private bool TryDecodePrefabId(NetworkPrefabId prefabId, out int index)
		{
			if (!prefabId.IsValid)
			{
				index = 0;
				return false;
			}
			if (prefabId.AsIndex >= _sources.Count)
			{
				index = 0;
				return false;
			}
			index = prefabId.AsIndex;
			return true;
		}

		private int GetBitSetCapacity(int length)
		{
			return (_sources.Capacity + 63) / 64;
		}
	}
}
