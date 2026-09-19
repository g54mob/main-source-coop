#define TRACE
#define FUSION_UNITY
#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Fusion
{
	internal class NetworkTypesMeta
	{
		private static class InstanceHolder
		{
			public static readonly NetworkTypesMeta Instance = CreateFromLoadedAssemblies();
		}

		private class GenericRpcInvoker
		{
			private Dictionary<Type, RpcInvokeDelegate> Cache;

			public GenericRpcInvoker(MethodInfo method)
			{
				_003Cmethod_003EP = method;
				Cache = new Dictionary<Type, RpcInvokeDelegate>();
				base._002Ector();
			}

			public void Invoke(in RpcInvokeContext context)
			{
				if (context.TargetType == null)
				{
					throw new NotSupportedException("Target type is unknown, can not close generic method " + _003Cmethod_003EP.DeclaringType?.FullName + "." + _003Cmethod_003EP.Name);
				}
				if (Cache.TryGetValue(context.TargetType, out var value))
				{
					value(in context);
					return;
				}
				Type type = context.TargetType;
				while (type != null)
				{
					if (type.IsGenericType && !(type.GetGenericTypeDefinition() != _003Cmethod_003EP.DeclaringType))
					{
						MethodInfo methodInfo = type.GetMethod(_003Cmethod_003EP.Name, BindingFlags.Static | BindingFlags.NonPublic);
						value = (RpcInvokeDelegate)Delegate.CreateDelegate(typeof(RpcInvokeDelegate), methodInfo);
						Cache.Add(context.TargetType, value);
						value(in context);
						return;
					}
					type = type.BaseType;
				}
				throw new InvalidOperationException("Failed to resolve " + _003Cmethod_003EP.Name + " for " + context.TargetType.FullName);
			}
		}

		public readonly Dictionary<Type, SimulationBehaviourMeta> Behaviours = new Dictionary<Type, SimulationBehaviourMeta>();

		public readonly SortedList<uint, RpcMeta> Rpcs = new SortedList<uint, RpcMeta>();

		public readonly Dictionary<Type, NetworkStructMeta> Structs = new Dictionary<Type, NetworkStructMeta>();

		public readonly SortedList<uint, NetworkInputMeta> Inputs = new SortedList<uint, NetworkInputMeta>();

		public static NetworkTypesMeta Instance
		{
			[return: NotNull]
			get
			{
				return InstanceHolder.Instance;
			}
		}

		public bool TryGetBehaviourMeta(Type type, out SimulationBehaviourMeta meta)
		{
			return Behaviours.TryGetValue(type, out meta);
		}

		public bool TryGetRpcMeta(uint key, out RpcMeta meta)
		{
			return Rpcs.TryGetValue(key, out meta);
		}

		public int GetMaxInputWordCount()
		{
			int num = 0;
			IList<NetworkInputMeta> values = Inputs.Values;
			for (int i = 0; i < values.Count; i++)
			{
				if (values[i].WordCount > num)
				{
					num = values[i].WordCount;
				}
			}
			return num + 1;
		}

		public bool TryGetInputWordCount(Type type, out int wordCount)
		{
			IList<NetworkInputMeta> values = Inputs.Values;
			for (int i = 0; i < values.Count; i++)
			{
				if (values[i].Type == type)
				{
					wordCount = values[i].WordCount;
					return true;
				}
			}
			wordCount = 0;
			return false;
		}

		public bool TryGetInputTypeKey(Type type, out int key)
		{
			IList<NetworkInputMeta> values = Inputs.Values;
			for (int i = 0; i < values.Count; i++)
			{
				if (values[i].Type == type)
				{
					key = i + 1;
					return true;
				}
			}
			key = 0;
			return false;
		}

		public bool TryGetInputType(int key, out Type type)
		{
			int num = key - 1;
			if (num < 0 || num >= Inputs.Count)
			{
				type = null;
				return false;
			}
			type = Inputs.Values[num].Type;
			return true;
		}

		public int GetStructWordCount<T>() where T : unmanaged, INetworkStruct
		{
			return GetStructWordCount(typeof(T));
		}

		public int GetStructWordCount(Type type)
		{
			if (Structs.TryGetValue(type, out var value))
			{
				return value.WordCount;
			}
			throw new ArgumentOutOfRangeException("type");
		}

		public bool TryGetPropertyMeta(Type behaviourType, string propertyName, out NetworkPropertyMeta propertyMeta)
		{
			if (!TryGetBehaviourMeta(behaviourType, out var meta))
			{
				propertyMeta = default(NetworkPropertyMeta);
				return false;
			}
			return meta.Properties.TryGetValue(propertyName, out propertyMeta);
		}

		public static NetworkTypesMeta CreateFromLoadedAssemblies()
		{
			Dictionary<uint, RpcAttribute> rpcs = new Dictionary<uint, RpcAttribute>();
			Dictionary<uint, (MethodInfo, NetworkRpcWeavedInvokerAttribute)> invokers = new Dictionary<uint, (MethodInfo, NetworkRpcWeavedInvokerAttribute)>();
			HashSet<uint> partialInvokers = new HashSet<uint>();
			try
			{
				NetworkTypesMeta networkTypesMeta = new NetworkTypesMeta();
				Stopwatch stopwatch = Stopwatch.StartNew();
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly assembly in assemblies)
				{
					if (assembly.GetCustomAttribute<NetworkAssemblyIgnoreAttribute>() != null)
					{
						continue;
					}
					Type[] types;
					try
					{
						types = assembly.GetTypes();
					}
					catch (ReflectionTypeLoadException ex)
					{
						types = ex.Types;
						InternalLogStreams.LogDebug?.Warn($"Error while loading types from Assembly: {assembly.FullName}. Still going to process {types.Length} types that " + "loaded successfully.");
					}
					Type[] array = types;
					foreach (Type type in array)
					{
						if (type == null)
						{
							continue;
						}
						if (typeof(SimulationBehaviour).IsAssignableFrom(type))
						{
							if (!(type == typeof(NetworkBehaviour)) && !(type == typeof(SimulationBehaviour)))
							{
								networkTypesMeta.AddBehaviour(type);
								AddRpcs(type, networkTypesMeta);
							}
						}
						else if (type.IsValueType && !type.IsGenericTypeDefinition)
						{
							if (typeof(INetworkInput).IsAssignableFrom(type))
							{
								networkTypesMeta.AddInput(type);
							}
							else if (typeof(INetworkStruct).IsAssignableFrom(type))
							{
								networkTypesMeta.AddStruct(type);
							}
						}
					}
				}
				InternalLogStreams.LogTraceObject?.Log($"Importing meta took {stopwatch.Elapsed}");
				return networkTypesMeta;
			}
			catch (Exception error)
			{
				InternalLogStreams.LogException?.Log(error);
				throw;
			}
			void AddRpcs(Type type2, NetworkTypesMeta meta)
			{
				partialInvokers.Clear();
				invokers.Clear();
				rpcs.Clear();
				MethodInfo[] methods = type2.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				MethodInfo[] array2 = methods;
				foreach (MethodInfo methodInfo in array2)
				{
					RpcAttributeBase customAttribute = methodInfo.GetCustomAttribute<RpcAttributeBase>();
					if (customAttribute != null)
					{
						if (customAttribute is NetworkRpcWeavedInvokerAttribute networkRpcWeavedInvokerAttribute)
						{
							if (!invokers.TryAdd(networkRpcWeavedInvokerAttribute.Key, (methodInfo, networkRpcWeavedInvokerAttribute)))
							{
								InternalLogStreams.LogError?.Log($"Duplicated NetworkRpcWeavedInvokerAttribute key: {networkRpcWeavedInvokerAttribute.Key} ({customAttribute}) (for: {type2.FullName}.{methodInfo.Name})");
							}
						}
						else if (customAttribute is RpcAttribute rpcAttribute)
						{
							if (rpcAttribute.Key != 0 && !rpcs.TryAdd(rpcAttribute.Key, rpcAttribute))
							{
								InternalLogStreams.LogError?.Log($"Duplicated RpcAttribute key: {rpcAttribute.Key} ({customAttribute}) (for: {type2.FullName}.{methodInfo.Name})\"");
							}
						}
						else if (customAttribute is NetworkRpcPartialInvoker networkRpcPartialInvoker)
						{
							if (!partialInvokers.Add(networkRpcPartialInvoker.Key))
							{
								InternalLogStreams.LogError?.Log($"Duplicated NetworkRpcPartialInvoker key: {networkRpcPartialInvoker.Key} ({customAttribute}) (for: {type2.FullName}.{methodInfo.Name})\"");
							}
						}
						else
						{
							InternalLogStreams.LogError?.Log(string.Format("Unknown {0} attribute: {1} on {2}", "RpcAttributeBase", customAttribute, methodInfo.Name));
						}
					}
				}
				if (invokers.Count <= 0)
				{
					return;
				}
				foreach (KeyValuePair<uint, (MethodInfo, NetworkRpcWeavedInvokerAttribute)> item in invokers)
				{
					item.Deconstruct(out var key, out var value);
					(MethodInfo, NetworkRpcWeavedInvokerAttribute) tuple = value;
					uint num = key;
					var (methodInfo2, networkRpcWeavedInvokerAttribute2) = tuple;
					if (!rpcs.Remove(num, out var value2))
					{
						InternalLogStreams.LogError?.Log($"Missing Rpc for the invoker marked with {num}");
					}
					else
					{
						bool flag = true;
						if (networkRpcWeavedInvokerAttribute2.HasPartialInvoker && !partialInvokers.Remove(num))
						{
							flag = false;
						}
						RpcInvokeDelegate invoke = null;
						if (flag)
						{
							invoke = ((!type2.IsGenericTypeDefinition) ? ((RpcInvokeDelegate)Delegate.CreateDelegate(typeof(RpcInvokeDelegate), methodInfo2)) : new RpcInvokeDelegate(new GenericRpcInvoker(methodInfo2).Invoke));
						}
						RpcMeta value3 = new RpcMeta(value2.Key, methodInfo2.DeclaringType, value2.Sources, value2.Targets, value2.Channel, value2.InvokeLocalMode, value2.TickAligned, invoke);
						meta.Rpcs.Add(value3.Key, value3);
					}
				}
			}
		}

		public void AddInput(Type type)
		{
			NetworkInputWeavedAttribute customAttributeOrThrow = type.GetCustomAttributeOrThrow<NetworkInputWeavedAttribute>(inherit: false);
			NetworkInputMeta value = new NetworkInputMeta(customAttributeOrThrow.Key, customAttributeOrThrow.WordCount, type);
			Inputs.Add(value.Key, value);
		}

		public void AddBehaviour(Type type)
		{
			int? staticWordCount = null;
			if (typeof(NetworkBehaviour).IsAssignableFrom(type))
			{
				NetworkBehaviourWeavedAttribute customAttribute = type.GetCustomAttribute<NetworkBehaviourWeavedAttribute>(inherit: false);
				if (customAttribute == null)
				{
					if (!string.Equals(type.FullName, "Fusion.Addons.SimpleKCC.KCC", StringComparison.Ordinal) && !string.Equals(type.FullName, "Fusion.Addons.SimpleKCC.SimpleKCC", StringComparison.Ordinal))
					{
						NetworkBehaviourWeavedAttribute inheritedWeavedAttribute = GetInheritedWeavedAttribute(type);
						if (inheritedWeavedAttribute == null)
						{
							InternalLogStreams.LogError?.Log(string.Format("Type {0} has not been weaved. Has the assembly {1} been added to {2}?", type, type.Assembly.GetName().Name, "NetworkProjectConfig"));
						}
						else if (inheritedWeavedAttribute.WordCount >= 0)
						{
							staticWordCount = inheritedWeavedAttribute.WordCount;
						}
					}
				}
				else if (customAttribute.WordCount >= 0)
				{
					staticWordCount = customAttribute.WordCount;
				}
			}
			SimulationBehaviourAttribute customAttribute2 = type.GetCustomAttribute<SimulationBehaviourAttribute>(inherit: true);
			SimulationBehaviourMeta value = new SimulationBehaviourMeta(customAttribute2?.Stages ?? (SimulationStages.Forward | SimulationStages.Resimulate), customAttribute2?.Modes ?? (SimulationModes.Server | SimulationModes.Host | SimulationModes.Client), customAttribute2?.Topologies ?? (Topologies.ClientServer | Topologies.Shared), staticWordCount);
			if (typeof(NetworkBehaviour).IsAssignableFrom(type))
			{
				List<(PropertyInfo, NetworkedWeavedAttribute)> list = new List<(PropertyInfo, NetworkedWeavedAttribute)>();
				Type type2 = type;
				while (type2 != null && type2 != typeof(NetworkBehaviour))
				{
					PropertyInfo[] properties = type2.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					PropertyInfo[] array = properties;
					foreach (PropertyInfo propertyInfo in array)
					{
						NetworkedWeavedAttribute customAttribute3 = propertyInfo.GetCustomAttribute<NetworkedWeavedAttribute>();
						if (customAttribute3 != null)
						{
							list.Add((propertyInfo, customAttribute3));
						}
					}
					type2 = type2.BaseType;
				}
				foreach (var item3 in list)
				{
					PropertyInfo item = item3.Item1;
					NetworkedWeavedAttribute item2 = item3.Item2;
					int capacity = 1;
					Type valueReaderWriterType = null;
					Type keyReaderWriterType = null;
					NetworkPropertyMetaFlags networkPropertyMetaFlags = (NetworkPropertyMetaFlags)0;
					if (item.PropertyType.IsGenericType)
					{
						Type genericTypeDefinition = item.PropertyType.GetGenericTypeDefinition();
						if (genericTypeDefinition == typeof(NetworkArray<>))
						{
							NetworkedWeavedArrayAttribute customAttribute4 = item.GetCustomAttribute<NetworkedWeavedArrayAttribute>();
							capacity = customAttribute4.Capacity;
							valueReaderWriterType = customAttribute4.ElementReaderWriterType;
						}
						else if (genericTypeDefinition == typeof(NetworkLinkedList<>))
						{
							NetworkedWeavedLinkedListAttribute customAttribute5 = item.GetCustomAttribute<NetworkedWeavedLinkedListAttribute>();
							capacity = customAttribute5.Capacity;
							valueReaderWriterType = customAttribute5.ElementReaderWriterType;
						}
						else if (genericTypeDefinition == typeof(NetworkDictionary<, >))
						{
							NetworkedWeavedDictionaryAttribute customAttribute6 = item.GetCustomAttribute<NetworkedWeavedDictionaryAttribute>();
							capacity = customAttribute6.Capacity;
							valueReaderWriterType = customAttribute6.ValueReaderWriterType;
							keyReaderWriterType = customAttribute6.KeyReaderWriterType;
						}
					}
					else if (item.PropertyType == typeof(string))
					{
						NetworkedWeavedStringAttribute customAttribute7 = item.GetCustomAttribute<NetworkedWeavedStringAttribute>();
						capacity = customAttribute7.Capacity;
					}
					NetworkedAttribute customAttribute8 = item.GetCustomAttribute<NetworkedAttribute>();
					if (customAttribute8.PluginAuthority)
					{
						networkPropertyMetaFlags |= NetworkPropertyMetaFlags.PluginAuthority;
					}
					value.Properties.Add(item.Name, new NetworkPropertyMeta(item2.WordOffset, item2.WordCount, capacity, item.PropertyType, valueReaderWriterType, keyReaderWriterType, networkPropertyMetaFlags));
				}
			}
			Behaviours.Add(type, value);
		}

		private static NetworkBehaviourWeavedAttribute GetInheritedWeavedAttribute(Type type)
		{
			Type type2 = type;
			while (type2 != null && type2 != typeof(NetworkBehaviour))
			{
				PropertyInfo[] properties = type2.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (PropertyInfo element in properties)
				{
					if (element.GetCustomAttribute<NetworkedAttribute>() != null)
					{
						return null;
					}
				}
				NetworkBehaviourWeavedAttribute networkBehaviourWeavedAttribute = type2.BaseType?.GetCustomAttribute<NetworkBehaviourWeavedAttribute>(inherit: false);
				if (networkBehaviourWeavedAttribute != null)
				{
					return networkBehaviourWeavedAttribute;
				}
				type2 = type2.BaseType;
			}
			return null;
		}

		[Conditional("FUSION_UNITY")]
		public void AddStruct(Type type)
		{
			if (type.GetInterface(typeof(INetworkStruct).FullName) == null)
			{
				throw new InvalidOperationException(string.Format("Type {0} does not implement {1}", type, "INetworkStruct"));
			}
			int num = FusionUnsafe.SizeOf(type);
			int num2 = GetWordCount(type);
			int num3 = num2 * 4;
			if (num3 < num)
			{
				Assert.AlwaysFail($"Size of {type} is invalid, expected size {num3} but was size {num}");
			}
			NetworkStructMeta value = new NetworkStructMeta(num2);
			Structs.Add(type, value);
			int GetWordCount(Type t)
			{
				NetworkStructWeavedAttribute customAttributeOrThrow = t.GetCustomAttributeOrThrow<NetworkStructWeavedAttribute>(inherit: false);
				int num4 = customAttributeOrThrow.WordCount;
				if (customAttributeOrThrow.IsGenericComposite)
				{
					Assert.Always(type.IsGenericType, "Type not generic: {0}", type);
					Type[] genericArguments = type.GetGenericArguments();
					foreach (Type type2 in genericArguments)
					{
						if (typeof(INetworkStruct).IsAssignableFrom(type2))
						{
							num4 += GetWordCount(type2);
						}
					}
				}
				return num4;
			}
		}
	}
}
