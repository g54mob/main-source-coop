#define DEBUG
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace Fusion
{
	[ScriptHelp(BackColor = ScriptHeaderBackColor.Blue)]
	[HelpURL("https://doc.photonengine.com/fusion/v2/manual/network-object#networkbehaviour")]
	public abstract class NetworkBehaviour : SimulationBehaviour, ISpawned, IPublicFacingInterface, IDespawned, IElementReaderWriter<NetworkObject>, IElementReaderWriter<NetworkBehaviour>
	{
		public struct ArrayReader<T>
		{
			internal int Offset;

			internal int Capacity;

			internal IElementReaderWriter<T> ReaderWriter;

			public unsafe NetworkArrayReadOnly<T> Read(NetworkBehaviourBuffer first)
			{
				return new NetworkArrayReadOnly<T>((byte*)first._ptr + (nint)Offset * (nint)4, Capacity, ReaderWriter);
			}
		}

		public struct LinkListReader<T>
		{
			internal int Offset;

			internal int Capacity;

			internal IElementReaderWriter<T> ReaderWriter;

			public unsafe NetworkLinkedListReadOnly<T> Read(NetworkBehaviourBuffer first)
			{
				return new NetworkLinkedListReadOnly<T>((byte*)first._ptr + (nint)Offset * (nint)4, Capacity, ReaderWriter);
			}
		}

		public struct DictionaryReader<K, V>
		{
			internal int Offset;

			internal int Capacity;

			internal IElementReaderWriter<K> KeyReaderWriter;

			internal IElementReaderWriter<V> ValueReaderWriter;

			public unsafe NetworkDictionaryReadOnly<K, V> Read(NetworkBehaviourBuffer first)
			{
				return new NetworkDictionaryReadOnly<K, V>(first._ptr + Offset, Capacity, KeyReaderWriter, ValueReaderWriter);
			}
		}

		public struct BehaviourReader<T> where T : NetworkBehaviour
		{
			internal NetworkRunner Runner;

			internal PropertyReader<NetworkBehaviourId> Reader;

			public T Read(NetworkBehaviourBuffer first)
			{
				NetworkBehaviour behaviour;
				return Runner.TryFindBehaviour(Reader.Read(first), out behaviour) ? (behaviour as T) : null;
			}

			public (T, T) Read(NetworkBehaviourBuffer first, NetworkBehaviourBuffer second)
			{
				return (Read(first), Read(second));
			}
		}

		public struct PropertyReader<T> where T : unmanaged
		{
			internal int Offset;

			public PropertyReader(int offset)
			{
				Offset = offset;
			}

			public T Read(NetworkBehaviourBuffer first)
			{
				return first.Read(this);
			}

			public (T, T) Read(NetworkBehaviourBuffer first, NetworkBehaviourBuffer second)
			{
				return (first.Read(this), second.Read(this));
			}
		}

		public class ChangeDetector
		{
			public enum Source
			{
				SimulationState = 0,
				SnapshotFrom = 1,
				SnapshotTo = 2,
				SnapshotNearest = 3
			}

			private struct PropertyData
			{
				public MemberInfo PropertyInfo;

				public NetworkedWeavedAttribute WeavedAttribute;

				public OnChangedCallbackWrapper OnChanged;

				public OnChangedPrevCallbackWrapper OnChangedPrev;
			}

			internal delegate void OnChangedPrevCallback<T>(T b, NetworkBehaviourBuffer prev) where T : NetworkBehaviour;

			internal delegate void OnChangedPrevCallbackWrapper(NetworkBehaviour b, NetworkBehaviourBuffer prev);

			internal delegate void OnChangedCallback<T>(T b) where T : NetworkBehaviour;

			internal delegate void OnChangedCallbackWrapper(NetworkBehaviour b);

			public struct Enumerable
			{
				private string[] _changed;

				private int _count;

				internal Enumerable(string[] changed, int count)
				{
					_changed = changed;
					_count = count;
				}

				public Enumerator GetEnumerator()
				{
					return new Enumerator(_changed, _count);
				}

				public bool Changed(string name)
				{
					return _count > 0 && Array.IndexOf(_changed, name, 0) >= 0;
				}
			}

			public struct Enumerator
			{
				private string[] _changed;

				private int _count;

				private int _current;

				public readonly string Current => _changed[_current];

				internal Enumerator(string[] changed, int count)
				{
					_changed = changed;
					_count = count;
					_current = -1;
				}

				public void Reset()
				{
					_current = -1;
				}

				public bool MoveNext()
				{
					return ++_current < _count;
				}
			}

			private static Dictionary<Type, PropertyData[]> _propertyMappings = new Dictionary<Type, PropertyData[]>();

			private static Dictionary<Type, bool> _hasChangeCallbacks = new Dictionary<Type, bool>();

			private int? _instance;

			private int[] _words;

			private unsafe int* _wordsPrevious;

			private Source _source;

			private Tick _sourceTick;

			private string[] _changed;

			private PropertyData[] _changedProperty;

			internal bool InvokeCallbacks;

			internal static bool HasChangeCallbacks(Type type)
			{
				if (type.IsGenericTypeDefinition)
				{
					return false;
				}
				if (!_propertyMappings.ContainsKey(type))
				{
					GetPropertyMappping(type);
				}
				bool value;
				return _hasChangeCallbacks.TryGetValue(type, out value) && value;
			}

			private static OnChangedPrevCallbackWrapper GetWrapperPrev<T>(MethodInfo methodInfo) where T : NetworkBehaviour
			{
				OnChangedPrevCallback<T> callback = (OnChangedPrevCallback<T>)Delegate.CreateDelegate(typeof(OnChangedPrevCallback<T>), null, methodInfo);
				return delegate(NetworkBehaviour behaviour, NetworkBehaviourBuffer prev)
				{
					callback((T)behaviour, prev);
				};
			}

			private static OnChangedCallbackWrapper GetWrapper<T>(MethodInfo methodInfo) where T : NetworkBehaviour
			{
				OnChangedCallback<T> callback = (OnChangedCallback<T>)Delegate.CreateDelegate(typeof(OnChangedCallback<T>), null, methodInfo);
				return delegate(NetworkBehaviour behaviour)
				{
					callback((T)behaviour);
				};
			}

			private static PropertyData[] GetPropertyMappping(Type type)
			{
				if (_propertyMappings == null)
				{
					_propertyMappings = new Dictionary<Type, PropertyData[]>();
				}
				if (!_propertyMappings.TryGetValue(type, out var value))
				{
					List<PropertyData> list = new List<PropertyData>();
					AddPropertiesToMappingForType(type, list, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, out var hasChangeCallbacks);
					Type baseType = type.BaseType;
					while (baseType != null && baseType != typeof(NetworkBehaviour))
					{
						AddPropertiesToMappingForType(baseType, list, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic, out var hasChangeCallbacks2);
						hasChangeCallbacks = hasChangeCallbacks2 || hasChangeCallbacks;
						baseType = baseType.BaseType;
					}
					if (hasChangeCallbacks)
					{
						_hasChangeCallbacks[type] = true;
					}
					_propertyMappings.Add(type, value = list.ToArray());
				}
				return value;
			}

			private static void AddPropertiesToMappingForType(Type type, List<PropertyData> result, BindingFlags bindingFlags, out bool hasChangeCallbacks)
			{
				hasChangeCallbacks = false;
				PropertyInfo[] properties = type.GetProperties(bindingFlags);
				foreach (PropertyInfo propertyInfo in properties)
				{
					NetworkedWeavedAttribute customAttribute = propertyInfo.GetCustomAttribute<NetworkedWeavedAttribute>();
					if (customAttribute == null)
					{
						continue;
					}
					PropertyData item = new PropertyData
					{
						PropertyInfo = propertyInfo,
						WeavedAttribute = customAttribute
					};
					OnChangedRenderAttribute customAttribute2 = propertyInfo.GetCustomAttribute<OnChangedRenderAttribute>();
					if (customAttribute2 != null)
					{
						Type type2 = type;
						MethodInfo method;
						do
						{
							method = type2.GetMethod(customAttribute2.MethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
							type2 = type2.BaseType;
						}
						while (method == null && type2 != null);
						if (method == null)
						{
							InternalLogStreams.LogError?.Log("Change Detector was not able to find any method named (" + customAttribute2.MethodName + ") on type (" + type.Name + ") or any base class.");
							continue;
						}
						if (method.GetParameters().Length == 1)
						{
							item.OnChangedPrev = (OnChangedPrevCallbackWrapper)typeof(ChangeDetector).GetMethod("GetWrapperPrev", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type).Invoke(null, new object[1] { method });
						}
						else
						{
							item.OnChanged = (OnChangedCallbackWrapper)typeof(ChangeDetector).GetMethod("GetWrapper", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(type).Invoke(null, new object[1] { method });
						}
						hasChangeCallbacks = true;
					}
					result.Add(item);
				}
			}

			unsafe ~ChangeDetector()
			{
				FusionUnsafe.Free(ref _wordsPrevious);
			}

			public unsafe void Init(NetworkBehaviour networkBehaviour, Source source, bool copyInitial = true)
			{
				if (networkBehaviour.WordCount == 0)
				{
					InternalLogStreams.LogError?.Log("Change detector cannot be bound to a behaviour with zero network properties.");
					return;
				}
				if ((_words?.Length).GetValueOrDefault() < networkBehaviour.WordCount)
				{
					_words = new int[networkBehaviour.WordCount];
				}
				else
				{
					Array.Clear(_words, 0, _words.Length);
				}
				int num = GetPropertyMappping(networkBehaviour.GetType()).Length;
				if (_changed == null || _changed.Length < num)
				{
					_changed = new string[num];
					_changedProperty = new PropertyData[num];
				}
				_instance = networkBehaviour.GetInstanceID();
				_source = source;
				_sourceTick = default(Tick);
				if (copyInitial)
				{
					fixed (int* words = _words)
					{
						FusionUnsafe.Copy(words, networkBehaviour.Ptr, _words.Length * 4);
					}
				}
			}

			public Enumerable DetectChanges(NetworkBehaviour b, out NetworkBehaviourBuffer previous, out NetworkBehaviourBuffer current, bool copyChanges = true)
			{
				return DetectChangesInternal(b, out previous, out current, copyChanges);
			}

			public Enumerable DetectChanges(NetworkBehaviour b, bool copyChanges = true)
			{
				NetworkBehaviourBuffer previous;
				NetworkBehaviourBuffer current;
				return DetectChangesInternal(b, out previous, out current, copyChanges);
			}

			private unsafe Enumerable DetectChangesInternal(NetworkBehaviour b, out NetworkBehaviourBuffer previous, out NetworkBehaviourBuffer current, bool copyChanges = true)
			{
				if (b.GetInstanceID() != _instance.GetValueOrDefault())
				{
					InternalLogStreams.LogError?.Log("This behaviour is not bound to this change detector");
					current = default(NetworkBehaviourBuffer);
					previous = default(NetworkBehaviourBuffer);
					return default(Enumerable);
				}
				int num = 0;
				int* ptr = b.Ptr;
				PropertyData[] propertyMappping = GetPropertyMappping(b.GetType());
				Tick sourceTick = _sourceTick;
				if (_source != Source.SimulationState)
				{
					RenderTimeline.GetRenderBuffers(b, out var from, out var to, out var alpha);
					NetworkBehaviourBuffer networkBehaviourBuffer = to;
					if (_source == Source.SnapshotNearest)
					{
						networkBehaviourBuffer = ((alpha < 0.5f) ? from : to);
					}
					else if (_source == Source.SnapshotFrom)
					{
						networkBehaviourBuffer = from;
					}
					if (networkBehaviourBuffer._ptr == null)
					{
						current = default(NetworkBehaviourBuffer);
						previous = default(NetworkBehaviourBuffer);
						return default(Enumerable);
					}
					if (_sourceTick == networkBehaviourBuffer.Tick)
					{
						current = default(NetworkBehaviourBuffer);
						previous = default(NetworkBehaviourBuffer);
						return default(Enumerable);
					}
					_sourceTick = networkBehaviourBuffer.Tick;
					ptr = networkBehaviourBuffer._ptr;
				}
				else
				{
					_sourceTick = b.Object.Runner.Tick;
				}
				fixed (int* words = _words)
				{
					if (_wordsPrevious == null)
					{
						_wordsPrevious = FusionUnsafe.AllocAndClearArray<int>(_words.Length, 8, "Fusion\\Fusion.Runtime\\Components\\NetworkBehaviourChangeDetector.cs", 361);
					}
					FusionUnsafe.Copy(_wordsPrevious, words, _words.Length * 4);
					PropertyData[] array = propertyMappping;
					for (int i = 0; i < array.Length; i++)
					{
						PropertyData propertyData = array[i];
						int wordOffset = propertyData.WeavedAttribute.WordOffset;
						int wordCount = propertyData.WeavedAttribute.WordCount;
						for (int j = wordOffset; j < wordOffset + wordCount; j++)
						{
							if (words[j] != ptr[j])
							{
								_changedProperty[num] = propertyData;
								_changed[num++] = propertyData.PropertyInfo.Name;
								if (copyChanges)
								{
									FusionUnsafe.Copy(words + wordOffset, ptr + wordOffset, wordCount * 4);
								}
								break;
							}
						}
					}
					if (num > 0)
					{
						previous = new NetworkBehaviourBuffer(sourceTick, _wordsPrevious, _words.Length);
					}
					else
					{
						previous = default(NetworkBehaviourBuffer);
					}
				}
				if (InvokeCallbacks && _changedProperty != null)
				{
					for (int k = 0; k < num && k < _changedProperty.Length; k++)
					{
						PropertyData propertyData2 = _changedProperty[k];
						if (propertyData2.OnChangedPrev != null)
						{
							try
							{
								propertyData2.OnChangedPrev(b, previous);
							}
							catch (Exception error)
							{
								InternalLogStreams.LogException?.Log(error);
							}
						}
						if (propertyData2.OnChanged != null)
						{
							try
							{
								propertyData2.OnChanged(b);
							}
							catch (Exception error2)
							{
								InternalLogStreams.LogException?.Log(error2);
							}
						}
					}
				}
				current = new NetworkBehaviourBuffer(_sourceTick, ptr, _words.Length);
				return new Enumerable(_changed, num);
			}
		}

		[Preserve]
		[Obsolete("Internal Fusion plumbing. Do not use directly.", true)]
		public unsafe int* Ptr;

		internal int WordOffset;

		internal int WordCount;

		internal int ObjectIndex;

		internal bool DefaultReplicated = true;

		private ChangeDetector _onRenderCallbacksDetector;

		internal unsafe int* InternalPtr => Ptr;

		internal unsafe Span<int> RawWords
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new Span<int>(Ptr, WordCount);
			}
		}

		public unsafe bool StateBufferIsValid
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Ptr != null;
			}
		}

		public unsafe NetworkBehaviourBuffer StateBuffer => new NetworkBehaviourBuffer(base.Runner.Simulation.Tick, Ptr, WordCount);

		public (int offset, int count) WordInfo => (offset: WordOffset, count: WordCount);

		public Tick ChangedTick => base.Object.BehaviourChangedTickArray[ObjectIndex];

		public NetworkBehaviourId Id
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new NetworkBehaviourId
				{
					Object = (BehaviourUtils.IsNotNull(base.Object) ? base.Object.Id : default(NetworkId)),
					Behaviour = ObjectIndex
				};
			}
		}

		public bool HasInputAuthority
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return BehaviourUtils.IsAlive(base.Object) && base.Object.HasInputAuthority;
			}
		}

		public bool HasStateAuthority
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return BehaviourUtils.IsAlive(base.Object) && base.Object.HasStateAuthority;
			}
		}

		public bool IsProxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return BehaviourUtils.IsAlive(base.Object) && base.Object.IsProxy;
			}
		}

		public virtual int? DynamicWordCount => null;

		internal bool IsEditorWritable
		{
			get
			{
				if (BehaviourUtils.IsNotAlive(base.Object))
				{
					return true;
				}
				if (!base.Object.IsValid)
				{
					return true;
				}
				if (base.Object.HasStateAuthority)
				{
					return true;
				}
				return false;
			}
		}

		internal unsafe void Init(int* ptr, int wordOffset, int wordCount)
		{
			Ptr = ptr + wordOffset;
			WordOffset = wordOffset;
			WordCount = wordCount;
		}

		internal unsafe int* InternalSwap(int* ptr)
		{
			int* ptr2 = Ptr;
			Ptr = ptr;
			return ptr2;
		}

		internal unsafe int InternalCompare(int* ptr)
		{
			return FusionUnsafe.Compare(Ptr, ptr, WordCount * 4);
		}

		public int GetLocalAuthorityMask()
		{
			if (BehaviourUtils.IsNotAlive(base.Runner))
			{
				return 0;
			}
			return AuthorityMasks.Create(HasStateAuthority, HasInputAuthority);
		}

		public void ReplicateTo(PlayerRef player, bool replicate)
		{
			base.Runner.SetBehaviourReplicateTo(this, player, replicate);
		}

		public void ReplicateToAll(bool replicate)
		{
			base.Runner.SetBehaviourReplicateToAll(this, replicate);
		}

		public void CopyStateFrom(NetworkBehaviour source)
		{
			Assert.Check(BehaviourUtils.IsAlive(base.Object), "IsAlive(Object)");
			Assert.Check(source, "source");
			if (GetType() == source.GetType())
			{
				FusionUnsafe.CopyToExact(RawWords, source.RawWords);
			}
		}

		public override void FixedUpdateNetwork()
		{
		}

		public unsafe void ResetState()
		{
			Assert.Check(BehaviourUtils.IsAlive(base.Object), "IsAlive(Object)");
			FusionUnsafe.Clear(Ptr, WordCount * 4);
			CopyBackingFieldsToState(firstTime: false);
		}

		public virtual void CopyBackingFieldsToState(bool firstTime)
		{
		}

		public virtual void CopyStateToBackingFields()
		{
		}

		internal override void PreRender()
		{
			_onRenderCallbacksDetector?.DetectChanges(this);
		}

		internal void PreSpawned()
		{
			if (ChangeDetector.HasChangeCallbacks(GetType()))
			{
				_onRenderCallbacksDetector = GetChangeDetector(ChangeDetector.Source.SnapshotNearest);
				_onRenderCallbacksDetector.InvokeCallbacks = true;
			}
		}

		public virtual void Spawned()
		{
		}

		public virtual void Despawned(NetworkRunner runner, bool hasState)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref T ReinterpretState<T>(int offset = 0) where T : unmanaged
		{
			Assert.Check(StateBufferIsValid, "StateBufferIsValid");
			return ref FusionUnsafe.ReinterpretWords<T>(RawWords, offset);
		}

		public BehaviourReader<T> GetBehaviourReader<T>(string property) where T : NetworkBehaviour
		{
			return GetBehaviourReader<T>(base.Runner, GetType(), property);
		}

		public static BehaviourReader<T> GetBehaviourReader<T>(NetworkRunner runner, Type behaviourType, string property) where T : NetworkBehaviour
		{
			return new BehaviourReader<T>
			{
				Runner = runner,
				Reader = GetPropertyReader<NetworkBehaviourId>(behaviourType, property)
			};
		}

		public static BehaviourReader<TProperty> GetBehaviourReader<TBehaviour, TProperty>(NetworkRunner runner, string property) where TBehaviour : NetworkBehaviour where TProperty : NetworkBehaviour
		{
			return new BehaviourReader<TProperty>
			{
				Runner = runner,
				Reader = GetPropertyReader<NetworkBehaviourId>(typeof(TBehaviour), property)
			};
		}

		public PropertyReader<T> GetPropertyReader<T>(string property) where T : unmanaged
		{
			return GetPropertyReader<T>(GetType(), property);
		}

		public static PropertyReader<TProperty> GetPropertyReader<TBehaviour, TProperty>(string property) where TBehaviour : NetworkBehaviour where TProperty : unmanaged
		{
			return GetPropertyReader<TProperty>(typeof(TBehaviour), property);
		}

		public static PropertyReader<T> GetPropertyReader<T>(Type behaviourType, string property) where T : unmanaged
		{
			if (!NetworkTypesMeta.Instance.TryGetPropertyMeta(behaviourType, property, out var propertyMeta))
			{
				throw new ArgumentOutOfRangeException($"Property {property} not found in {behaviourType}", "property");
			}
			Type type = propertyMeta.PropertyType;
			if (typeof(NetworkObject).IsAssignableFrom(type))
			{
				type = typeof(NetworkId);
			}
			else if (typeof(NetworkBehaviour).IsAssignableFrom(type))
			{
				type = typeof(NetworkBehaviourId);
			}
			if (type != typeof(T) && ((!type.IsByRef && !type.IsPointer) || !(type.GetElementType() == typeof(T))))
			{
				throw new InvalidOperationException($"Property {property} in {behaviourType} is not of type {typeof(T)}");
			}
			return new PropertyReader<T>(propertyMeta.WordOffset);
		}

		public ArrayReader<T> GetArrayReader<T>(string property)
		{
			return GetArrayReader(GetType(), property, this as IElementReaderWriter<T>);
		}

		public static ArrayReader<T> GetArrayReader<T>(Type behaviourType, string property, IElementReaderWriter<T> readerWriter = null)
		{
			if (!NetworkTypesMeta.Instance.TryGetPropertyMeta(behaviourType, property, out var propertyMeta))
			{
				throw new ArgumentOutOfRangeException($"Property {property} not found in {behaviourType}", "property");
			}
			if (propertyMeta.PropertyType != typeof(NetworkArray<T>))
			{
				throw new InvalidOperationException($"Property {property} in {behaviourType} is not of type {typeof(NetworkArray<T>)}");
			}
			return new ArrayReader<T>
			{
				Offset = propertyMeta.WordOffset,
				Capacity = propertyMeta.Capacity,
				ReaderWriter = (readerWriter ?? ReaderWriterCache.Get<T>(propertyMeta.ValueReaderWriterType))
			};
		}

		public LinkListReader<T> GetLinkListReader<T>(string property)
		{
			return GetLinkListReader(GetType(), property, this as IElementReaderWriter<T>);
		}

		public static LinkListReader<T> GetLinkListReader<T>(Type behaviourType, string property, IElementReaderWriter<T> readerWriter = null)
		{
			if (!NetworkTypesMeta.Instance.TryGetPropertyMeta(behaviourType, property, out var propertyMeta))
			{
				throw new ArgumentOutOfRangeException($"Property {property} not found in {behaviourType}", "property");
			}
			if (propertyMeta.PropertyType != typeof(NetworkLinkedList<T>))
			{
				throw new InvalidOperationException($"Property {property} in {behaviourType} is not of type {typeof(NetworkLinkedList<T>)}");
			}
			return new LinkListReader<T>
			{
				Offset = propertyMeta.WordOffset,
				Capacity = propertyMeta.Capacity,
				ReaderWriter = (readerWriter ?? ReaderWriterCache.Get<T>(propertyMeta.ValueReaderWriterType))
			};
		}

		public DictionaryReader<K, V> GetDictionaryReader<K, V>(string property)
		{
			return GetDictionaryReader(GetType(), property, this as IElementReaderWriter<K>, this as IElementReaderWriter<V>);
		}

		public static DictionaryReader<K, V> GetDictionaryReader<K, V>(Type behaviourType, string property, IElementReaderWriter<K> keyReaderWriter = null, IElementReaderWriter<V> valueReaderWriter = null)
		{
			if (!NetworkTypesMeta.Instance.TryGetPropertyMeta(behaviourType, property, out var propertyMeta))
			{
				throw new ArgumentOutOfRangeException($"Property {property} not found in {behaviourType}", "property");
			}
			if (propertyMeta.PropertyType != typeof(NetworkDictionary<K, V>))
			{
				throw new InvalidOperationException($"Property {property} in {behaviourType} is not of type {typeof(NetworkDictionary<K, V>)}");
			}
			return new DictionaryReader<K, V>
			{
				Offset = propertyMeta.WordOffset,
				Capacity = propertyMeta.Capacity,
				KeyReaderWriter = (keyReaderWriter ?? ReaderWriterCache.Get<K>(propertyMeta.KeyReaderWriterType)),
				ValueReaderWriter = (valueReaderWriter ?? ReaderWriterCache.Get<V>(propertyMeta.ValueReaderWriterType))
			};
		}

		public ChangeDetector GetChangeDetector(ChangeDetector.Source source, bool copyInitial = true)
		{
			ChangeDetector changeDetector = new ChangeDetector();
			changeDetector.Init(this, source, copyInitial);
			return changeDetector;
		}

		public bool TryGetSnapshotsBuffers(out NetworkBehaviourBuffer from, out NetworkBehaviourBuffer to, out float alpha)
		{
			RenderTimeline.GetRenderBuffers(this, out from, out to, out alpha);
			return from.Valid && to.Valid;
		}

		[Obsolete("Not called anymore, used ReplicateTo(PlayerRef, bool) instead")]
		protected virtual bool ReplicateTo(PlayerRef player)
		{
			return true;
		}

		public T? GetInput<T>() where T : unmanaged, INetworkInput
		{
			if (BehaviourUtils.IsAlive(base.Object))
			{
				Simulation simulation = base.Object.Simulation;
				if (simulation != null && simulation.PlayerValid(base.Object.Meta.InputAuthority))
				{
					return base.Object.Runner.GetInputForPlayer<T>(base.Object.Meta.InputAuthority);
				}
			}
			return null;
		}

		public bool GetInput<T>(out T input) where T : unmanaged, INetworkInput
		{
			if (BehaviourUtils.IsAlive(base.Object))
			{
				Simulation simulation = base.Object.Simulation;
				if (simulation != null && simulation.PlayerValid(base.Object.Meta.InputAuthority))
				{
					return base.Object.Runner.TryGetInputForPlayer<T>(base.Object.Meta.InputAuthority, out input);
				}
			}
			input = default(T);
			return false;
		}

		[Obsolete("Use NetworkWrap(NetworkBehaviour) instead", true)]
		public unsafe static int NetworkSerialize(NetworkRunner runner, NetworkBehaviour obj, byte* data)
		{
			throw new NotImplementedException();
		}

		[Obsolete("Use NetworkUnwrap(NetworkRunner, NetworkBehaviourId) instead", true)]
		public unsafe static int NetworkDeserialize(NetworkRunner runner, byte* data, ref NetworkBehaviour result)
		{
			throw new NotImplementedException();
		}

		[Obsolete("Use NetworkWrap(NetworkBehaviour) instead")]
		public static NetworkBehaviourId NetworkWrap(NetworkRunner runner, NetworkBehaviour obj)
		{
			return NetworkWrap(obj);
		}

		[NetworkSerializeMethod]
		public static NetworkBehaviourId NetworkWrap(NetworkBehaviour obj)
		{
			if (BehaviourUtils.IsNotAlive(obj))
			{
				return default(NetworkBehaviourId);
			}
			if (BehaviourUtils.IsNotAlive(obj.Object))
			{
				return default(NetworkBehaviourId);
			}
			return new NetworkBehaviourId
			{
				Object = obj.Object.Id,
				Behaviour = obj.ObjectIndex
			};
		}

		[NetworkDeserializeMethod]
		public static NetworkBehaviour NetworkUnwrap(NetworkRunner runner, NetworkBehaviourId wrapper)
		{
			if (!wrapper.Object.IsValid)
			{
				return null;
			}
			if (!runner.TryFindObject(wrapper.Object, out var networkObject))
			{
				runner.InvokeFailedToResolveNetworkObject(wrapper.Object);
				return null;
			}
			if (wrapper.Behaviour < 0 || wrapper.Behaviour >= networkObject.NetworkedBehaviours.Length)
			{
				NetworkBehaviourUtils.NotifyNetworkUnwrapFailed(wrapper);
				return null;
			}
			return networkObject.NetworkedBehaviours[wrapper.Behaviour];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator NetworkBehaviourId(NetworkBehaviour behaviour)
		{
			return BehaviourUtils.IsAlive(behaviour.Runner) ? NetworkWrap(behaviour) : default(NetworkBehaviourId);
		}

		protected internal static void InvokeWeavedCode()
		{
		}

		public static ref T MakeRef<T>() where T : unmanaged
		{
			throw new NotImplementedException("This is a special method that is meant to be used only for [Networked] properties inline initialization.");
		}

		public static ref T MakeRef<T>(T defaultValue) where T : unmanaged
		{
			throw new NotImplementedException("This is a special method that is meant to be used only for [Networked] properties inline initialization.");
		}

		public unsafe static T* MakePtr<T>() where T : unmanaged
		{
			throw new NotImplementedException("This is a special method that is meant to be used only for [Networked] properties inline initialization.");
		}

		public unsafe static T* MakePtr<T>(T defaultValue) where T : unmanaged
		{
			throw new NotImplementedException("This is a special method that is meant to be used only for [Networked] properties inline initialization.");
		}

		public static NetworkBehaviourUtils.ArrayInitializer<T> MakeInitializer<T>(T[] array)
		{
			throw new NotImplementedException("This is a special method that is meant to be used only for [Networked] properties inline initialization.");
		}

		public static NetworkBehaviourUtils.DictionaryInitializer<K, V> MakeInitializer<K, V>(Dictionary<K, V> dictionary)
		{
			throw new NotImplementedException("This is a special method that is meant to be used only for [Networked] properties inline initialization.");
		}

		internal void MakeOwned(NetworkRunner runner, NetworkObject obj, int index)
		{
			MakeOwned(runner, obj);
			ObjectIndex = index;
		}

		internal new void MakeUnowned()
		{
			base.MakeUnowned();
			ObjectIndex = 0;
		}

		int IElementReaderWriter<NetworkBehaviour>.GetElementHashCode(NetworkBehaviour element)
		{
			return NetworkWrap(element).GetHashCode();
		}

		int IElementReaderWriter<NetworkBehaviour>.GetElementWordCount()
		{
			return 2;
		}

		unsafe NetworkBehaviour IElementReaderWriter<NetworkBehaviour>.Read(byte* data, int index)
		{
			return NetworkUnwrap(base.Runner, *(NetworkBehaviourId*)(data + index * 8));
		}

		unsafe ref NetworkBehaviour IElementReaderWriter<NetworkBehaviour>.ReadRef(byte* data, int index)
		{
			throw new NotSupportedException("Only supported for trivially copyable types. Fusion.NetworkBehaviour is not trivially copyable.");
		}

		unsafe void IElementReaderWriter<NetworkBehaviour>.Write(byte* data, int index, NetworkBehaviour element)
		{
			*(NetworkBehaviourId*)(data + index * 8) = NetworkWrap(element);
		}

		int IElementReaderWriter<NetworkObject>.GetElementWordCount()
		{
			return 1;
		}

		int IElementReaderWriter<NetworkObject>.GetElementHashCode(NetworkObject element)
		{
			return NetworkObject.NetworkWrap(element).GetHashCode();
		}

		unsafe NetworkObject IElementReaderWriter<NetworkObject>.Read(byte* data, int index)
		{
			NetworkObject result = null;
			NetworkObject.NetworkUnwrap(base.Runner, *(NetworkId*)(data + index * 4), ref result);
			return result;
		}

		unsafe ref NetworkObject IElementReaderWriter<NetworkObject>.ReadRef(byte* data, int index)
		{
			throw new NotSupportedException("Only supported for trivially copyable types. Fusion.NetworkObject is not trivially copyable.");
		}

		unsafe void IElementReaderWriter<NetworkObject>.Write(byte* data, int index, NetworkObject element)
		{
			*(NetworkId*)(data + index * 4) = NetworkObject.NetworkWrap(element);
		}
	}
}
