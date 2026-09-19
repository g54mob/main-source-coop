#define DEBUG
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fusion
{
	public static class NetworkBehaviourUtils
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct ArrayInitializer<T>
		{
			public static implicit operator NetworkArray<T>(ArrayInitializer<T> arr)
			{
				throw new NotImplementedException("This is a special method that is meant to be used only for [Networked] properties inline initialization.");
			}

			public static implicit operator NetworkLinkedList<T>(ArrayInitializer<T> arr)
			{
				throw new NotImplementedException("This is a special method that is meant to be used only for [Networked] properties inline initialization.");
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct DictionaryInitializer<K, V>
		{
			public static implicit operator NetworkDictionary<K, V>(DictionaryInitializer<K, V> arr)
			{
				throw new NotImplementedException("This is a special method that is meant to be used only for [Networked] properties inline initialization.");
			}
		}

		public static bool InvokeRpc;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool ShouldNotifyRpcError(RpcInvokeInfo invokeInfo)
		{
			switch (invokeInfo.SendMessageResult)
			{
			case RpcSendMessageResult.InsufficientSourceAuthority:
				return true;
			case RpcSendMessageResult.PayloadSizeExceeded:
				return true;
			case RpcSendMessageResult.TargetPlayerNotAvailable:
				return true;
			default:
				if (invokeInfo.SendMessageResult == RpcSendMessageResult.TargetPlayerIsLocalPlayer)
				{
					switch (invokeInfo.LocalInvokeResult)
					{
					case RpcLocalInvokeResult.InsufficientTargetAuthority:
						return true;
					case RpcLocalInvokeResult.NotInvokableLocally:
						return true;
					}
				}
				return false;
			}
		}

		internal static void NotifyRpcError(ILogSource context, string rpc, RpcInvokeInfo invokeInfo, PlayerRef targetPlayer)
		{
			if (invokeInfo.SendMessageResult == RpcSendMessageResult.TargetPlayerIsLocalPlayer)
			{
				switch (invokeInfo.LocalInvokeResult)
				{
				case RpcLocalInvokeResult.InsufficientTargetAuthority:
					InternalLogStreams.LogError?.Log(context, rpc + ": Targeted local player, but local simulation is not allowed to handle this RPC");
					break;
				case RpcLocalInvokeResult.NotInvokableLocally:
					InternalLogStreams.LogWarn?.Log(context, $"{rpc} culled for target {targetPlayer}: player is local and InvokeLocal is set to false");
					break;
				}
			}
			switch (invokeInfo.SendMessageResult)
			{
			case RpcSendMessageResult.PayloadSizeExceeded:
				InternalLogStreams.LogError?.Log(context, $"{rpc}: payload is too large. Max allowed: {512} bytes)");
				break;
			case RpcSendMessageResult.InsufficientSourceAuthority:
				InternalLogStreams.LogError?.Log(context, rpc + ": Local simulation is not allowed to send this RPC.");
				break;
			case RpcSendMessageResult.TargetPlayerNotAvailable:
				InternalLogStreams.LogError?.Log(context, $"{rpc}: target {targetPlayer} not reachable.");
				break;
			}
		}

		internal static bool CheckInvokeRpc(NetworkBehaviour behaviour)
		{
			bool invokeRpc = InvokeRpc;
			InvokeRpc = false;
			ThrowIfBehaviourNotInitialized(behaviour);
			return invokeRpc;
		}

		internal static bool CheckInvokeRpc(NetworkRunner runner)
		{
			bool invokeRpc = InvokeRpc;
			InvokeRpc = false;
			ThrowIfRunnerNotSet(runner);
			return invokeRpc;
		}

		internal static void ResetStatics()
		{
			InvokeRpc = false;
		}

		public static int[] GetCustomSharedModeReadOnlyOffsets(Type type, int baseOffset = 0)
		{
			if (!NetworkTypesMeta.Instance.TryGetBehaviourMeta(type, out var meta))
			{
				return Array.Empty<int>();
			}
			List<int> list = null;
			foreach (NetworkPropertyMeta value in meta.Properties.Values)
			{
				if (value.Flags.HasNot(NetworkPropertyMetaFlags.PluginAuthority))
				{
					continue;
				}
				for (int i = 0; i < value.WordCount; i++)
				{
					if (list == null)
					{
						list = new List<int>();
					}
					list.Add(baseOffset + value.WordOffset + i);
				}
			}
			return list?.ToArray() ?? Array.Empty<int>();
		}

		public static int GetWordCount(NetworkBehaviour behaviour)
		{
			int? dynamicWordCount = behaviour.DynamicWordCount;
			if (dynamicWordCount.HasValue)
			{
				Assert.Always(dynamicWordCount.Value >= 0, "DynamicWordCount returned a negative value {0} {1}", dynamicWordCount.Value, LogUtils.GetDump(behaviour));
				return dynamicWordCount.Value;
			}
			int staticWordCount = GetStaticWordCount(behaviour.GetType());
			Assert.Always(staticWordCount >= 0, "GetStaticWordCount returned a negative value {0} {1}", staticWordCount, LogUtils.GetDump(behaviour));
			return staticWordCount;
		}

		public static bool HasStaticWordCount(Type type)
		{
			Assert.Check(typeof(NetworkBehaviour).IsAssignableFrom(type), "typeof(NetworkBehaviour).IsAssignableFrom(type)");
			SimulationBehaviourMeta meta;
			return NetworkTypesMeta.Instance.TryGetBehaviourMeta(type, out meta) && meta.StaticWordCount >= 0;
		}

		public static int GetStaticWordCount(Type type)
		{
			if (!NetworkTypesMeta.Instance.TryGetBehaviourMeta(type, out var meta))
			{
				throw new InvalidOperationException(string.Format("Type {0} has not been weaved. Has the assembly {1} been added to {2}?", type, type.Assembly.GetName().Name, "NetworkProjectConfig"));
			}
			return meta.StaticWordCount ?? throw new InvalidOperationException($"Static word count for {type} is null");
		}

		public static void ThrowIfBehaviourNotInitialized(NetworkBehaviour behaviour)
		{
			if (BehaviourUtils.IsNull(behaviour))
			{
				throw new ArgumentNullException("behaviour");
			}
			if (BehaviourUtils.IsNotAlive(behaviour.Object))
			{
				throw new InvalidOperationException("Behaviour not initialized: Object not set.");
			}
			if (BehaviourUtils.IsNotAlive(behaviour.Runner))
			{
				throw new InvalidOperationException("Behaviour not initialized: Runner not set.");
			}
		}

		public static void ThrowIfRunnerNotSet(NetworkRunner runner)
		{
			if (BehaviourUtils.IsNull(runner))
			{
				throw new ArgumentNullException("runner");
			}
		}

		public static void NotifyNetworkWrapFailed<T>(T value)
		{
			InternalLogStreams.LogWarn?.Log($"Failed to wrap {value}");
		}

		public static void NotifyNetworkWrapFailed<T>(T value, Type wrapperType)
		{
			InternalLogStreams.LogWarn?.Log($"Failed to wrap {value} as {wrapperType}");
		}

		public static void NotifyNetworkUnwrapFailed(NetworkBehaviourId id)
		{
			InternalLogStreams.LogError?.Log($"Failed to find behaviour {id.Behaviour} on {id.Object} during unwrapping of a [Networked] property. " + "This should never happen and may be due to prefabs are out of date, non-deterministic runtime baking or NetworkedBehaviours tampering.");
		}

		public static void InitializeNetworkArray<T>(NetworkArray<T> networkArray, T[] sourceArray, string name) where T : unmanaged
		{
			int num = ((sourceArray != null) ? sourceArray.Length : 0);
			if (num != 0)
			{
				if (networkArray.Length < num)
				{
					InternalLogStreams.LogError?.Log($"Source array is too long for {name} with capacity of {networkArray.Length}: {num}. Ignoring extra elements.");
					num = networkArray.Length;
				}
				networkArray.CopyFrom(sourceArray, 0, num);
			}
		}

		public static void CopyFromNetworkArray<T>(NetworkArray<T> networkArray, ref T[] dstArray) where T : unmanaged
		{
			if (dstArray?.Length != networkArray.Length)
			{
				dstArray = new T[networkArray.Length];
			}
			networkArray.CopyTo(dstArray);
		}

		public static T[] CloneArray<T>(T[] array)
		{
			if (array == null)
			{
				return Array.Empty<T>();
			}
			T[] array2 = new T[array.Length];
			Array.Copy(array, array2, array.Length);
			return array2;
		}

		public static void InitializeNetworkList<T>(NetworkLinkedList<T> networkList, T[] sourceArray, string name) where T : unmanaged
		{
			int num = ((sourceArray != null) ? sourceArray.Length : 0);
			if (num != 0)
			{
				if (networkList.Capacity < num)
				{
					InternalLogStreams.LogError?.Log($"Source array is too long for {name} with capacity of {networkList.Capacity}: {num}. Ignoring extra elements.");
					num = networkList.Capacity;
				}
				networkList.Clear();
				for (int i = 0; i < num; i++)
				{
					networkList.Add(sourceArray[i]);
				}
			}
		}

		public static void CopyFromNetworkList<T>(NetworkLinkedList<T> networkList, ref T[] dstArray) where T : unmanaged
		{
			if (dstArray?.Length != networkList.Count)
			{
				dstArray = new T[networkList.Count];
			}
			int num = 0;
			foreach (T item in networkList)
			{
				dstArray[num++] = item;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InternalOnDestroy(SimulationBehaviour obj)
		{
			obj.Flags |= SimulationBehaviourRuntimeFlags.IsUnityDestroyed;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InternalOnEnable(SimulationBehaviour obj)
		{
			obj.Flags &= ~SimulationBehaviourRuntimeFlags.IsUnityDisabled;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InternalOnDisable(SimulationBehaviour obj)
		{
			obj.Flags |= SimulationBehaviourRuntimeFlags.IsUnityDisabled;
		}

		public static void InitializeNetworkDictionary<D, K, V>(NetworkDictionary<K, V> networkDictionary, D dictionary, string name) where D : IDictionary<K, V> where K : unmanaged where V : unmanaged
		{
			int num = dictionary?.Count ?? 0;
			if (num == 0)
			{
				return;
			}
			if (num > networkDictionary.Capacity)
			{
				InternalLogStreams.LogError?.Log($"Source dictionary is too long for {name} with capacity of {networkDictionary.Capacity}: {num}. Ignoring extra elements.");
				num = networkDictionary.Capacity;
			}
			networkDictionary.Clear();
			foreach (KeyValuePair<K, V> item in dictionary)
			{
				if (--num < 0)
				{
					break;
				}
				networkDictionary.Add(item.Key, item.Value);
			}
		}

		public static void CopyFromNetworkDictionary<D, K, V>(NetworkDictionary<K, V> networkDictionary, ref D dictionary) where D : IDictionary<K, V>, new() where K : unmanaged where V : unmanaged
		{
			if (dictionary == null)
			{
				dictionary = new D();
			}
			else
			{
				dictionary.Clear();
			}
			foreach (KeyValuePair<K, V> item in networkDictionary)
			{
				dictionary.Add(item.Key, item.Value);
			}
		}

		public static SerializableDictionary<K, V> MakeSerializableDictionary<K, V>(Dictionary<K, V> dictionary) where K : unmanaged where V : unmanaged
		{
			return SerializableDictionary<K, V>.Wrap(dictionary);
		}
	}
}
