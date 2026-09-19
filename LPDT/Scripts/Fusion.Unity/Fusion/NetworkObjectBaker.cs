#define FUSION_LOGLEVEL_TRACE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Fusion
{
	public class NetworkObjectBaker
	{
		public readonly struct Result
		{
			public bool HadChanges { get; }

			public int ObjectCount { get; }

			public int BehaviourCount { get; }

			public Result(bool dirty, int objectCount, int behaviourCount)
			{
				HadChanges = dirty;
				ObjectCount = objectCount;
				BehaviourCount = behaviourCount;
			}
		}

		public readonly struct TransformPath
		{
			public struct _Indices
			{
				public unsafe fixed ushort Value[10];
			}

			public const int MaxDepth = 10;

			public readonly _Indices Indices;

			public readonly ushort Depth;

			public readonly ushort Next;

			internal unsafe TransformPath(ushort depth, ushort next, List<ushort> indices, int offset, int count)
			{
				Depth = depth;
				Next = next;
				for (int i = 0; i < count; i++)
				{
					Indices.Value[i] = indices[i + offset];
				}
			}

			public unsafe override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < Depth && i < 10; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append("/");
					}
					stringBuilder.Append(Indices.Value[i]);
				}
				if (Depth > 10)
				{
					stringBuilder.Append($"/...[{Depth - 10}]");
				}
				return stringBuilder.ToString();
			}
		}

		public sealed class TransformPathCache : IComparer<TransformPath>, IEqualityComparer<TransformPath>
		{
			private Dictionary<Transform, TransformPath> _cache = new Dictionary<Transform, TransformPath>();

			private List<ushort> _siblingIndexStack = new List<ushort>();

			private List<TransformPath> _nexts = new List<TransformPath>();

			public TransformPath Create(Transform transform)
			{
				if (_cache.TryGetValue(transform, out var value))
				{
					return value;
				}
				_siblingIndexStack.Clear();
				Transform transform2 = transform;
				ushort num;
				ushort next;
				checked
				{
					while (transform2 != null)
					{
						_siblingIndexStack.Add((ushort)transform2.GetSiblingIndex());
						transform2 = transform2.parent;
					}
					_siblingIndexStack.Reverse();
					num = (ushort)_siblingIndexStack.Count;
					next = 0;
				}
				if (num > 10)
				{
					for (int num2 = ((num % 10 == 0) ? (num - 10) : (num - num % 10)); num2 > 0; num2 -= 10)
					{
						checked
						{
							TransformPath item = new TransformPath((ushort)(num - num2), next, _siblingIndexStack, num2, Mathf.Min(10, num - num2));
							_nexts.Add(item);
							next = (ushort)_nexts.Count;
						}
					}
				}
				TransformPath transformPath = new TransformPath(num, next, _siblingIndexStack, 0, Mathf.Min(10, num));
				_cache.Add(transform, transformPath);
				return transformPath;
			}

			public void Clear()
			{
				_nexts.Clear();
				_cache.Clear();
				_siblingIndexStack.Clear();
			}

			public bool Equals(TransformPath x, TransformPath y)
			{
				if (x.Depth != y.Depth)
				{
					return false;
				}
				return CompareToDepthUnchecked(in x, in y, x.Depth) == 0;
			}

			public int GetHashCode(TransformPath obj)
			{
				int depth = obj.Depth;
				return GetHashCode(in obj, depth);
			}

			public int Compare(TransformPath x, TransformPath y)
			{
				int num = CompareToDepthUnchecked(in x, in y, Mathf.Min(x.Depth, y.Depth));
				if (num != 0)
				{
					return num;
				}
				return x.Depth - y.Depth;
			}

			private unsafe int CompareToDepthUnchecked(in TransformPath x, in TransformPath y, int depth)
			{
				for (int i = 0; i < depth && i < 10; i++)
				{
					int num = x.Indices.Value[i] - y.Indices.Value[i];
					if (num != 0)
					{
						return num;
					}
				}
				if (depth > 10)
				{
					return CompareToDepthUnchecked(_nexts[x.Next - 1], _nexts[y.Next - 1], depth - 10);
				}
				return 0;
			}

			private unsafe int GetHashCode(in TransformPath path, int hash)
			{
				for (int i = 0; i < path.Depth && i < 10; i++)
				{
					hash = hash * 31 + path.Indices.Value[i];
				}
				if (path.Depth > 10)
				{
					hash = GetHashCode(_nexts[path.Next - 1], hash);
				}
				return hash;
			}

			public bool IsAncestorOf(in TransformPath x, in TransformPath y)
			{
				if (x.Depth >= y.Depth)
				{
					return false;
				}
				return CompareToDepthUnchecked(in x, in y, x.Depth) == 0;
			}

			public bool IsEqualOrAncestorOf(in TransformPath x, in TransformPath y)
			{
				if (x.Depth > y.Depth)
				{
					return false;
				}
				return CompareToDepthUnchecked(in x, in y, x.Depth) == 0;
			}

			public string Dump(in TransformPath x)
			{
				StringBuilder stringBuilder = new StringBuilder();
				Dump(in x, stringBuilder);
				return stringBuilder.ToString();
			}

			private unsafe void Dump(in TransformPath x, StringBuilder builder)
			{
				for (int i = 0; i < x.Depth && i < 10; i++)
				{
					if (i > 0)
					{
						builder.Append("/");
					}
					builder.Append(x.Indices.Value[i]);
				}
				if (x.Depth > 10)
				{
					builder.Append("/");
					Dump(_nexts[x.Next - 1], builder);
				}
			}
		}

		private readonly List<NetworkObject> _allNetworkObjects = new List<NetworkObject>();

		private readonly List<TransformPath> _networkObjectsPaths = new List<TransformPath>();

		private readonly List<SimulationBehaviour> _allSimulationBehaviours = new List<SimulationBehaviour>();

		private readonly TransformPathCache _pathCache = new TransformPathCache();

		private readonly List<NetworkBehaviour> _arrayBufferNB = new List<NetworkBehaviour>();

		private readonly List<NetworkObject> _arrayBufferNO = new List<NetworkObject>();

		protected virtual void SetDirty(MonoBehaviour obj)
		{
		}

		protected virtual bool TryGetExecutionOrder(MonoBehaviour obj, out int order)
		{
			order = 0;
			return false;
		}

		protected virtual uint GetSortKey(NetworkObject obj)
		{
			return 0u;
		}

		protected virtual bool PostprocessBehaviour(SimulationBehaviour behaviour)
		{
			return false;
		}

		public Result Bake(GameObject root)
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}
			root.GetComponentsInChildren(includeInactive: true, _allNetworkObjects);
			_allNetworkObjects.RemoveAll((NetworkObject networkObject2) => networkObject2 == null);
			if (_allNetworkObjects.Count == 0)
			{
				return new Result(dirty: false, 0, 0);
			}
			try
			{
				foreach (NetworkObject allNetworkObject in _allNetworkObjects)
				{
					_networkObjectsPaths.Add(_pathCache.Create(allNetworkObject.transform));
				}
				bool dirty = false;
				_allNetworkObjects.Reverse();
				_networkObjectsPaths.Reverse();
				root.GetComponentsInChildren(includeInactive: true, _allSimulationBehaviours);
				_allSimulationBehaviours.RemoveAll((SimulationBehaviour simulationBehaviour2) => simulationBehaviour2 == null);
				int count = _allNetworkObjects.Count;
				int count2 = _allSimulationBehaviours.Count;
				for (int num = 0; num < _allNetworkObjects.Count; num++)
				{
					NetworkObject networkObject = _allNetworkObjects[num];
					NetworkObjectFlags flags = networkObject.Flags;
					flags &= ~NetworkObjectFlags.HasMainNetworkTRSP;
					bool flag = false;
					bool activeInHierarchy = networkObject.gameObject.activeInHierarchy;
					int? num2 = null;
					if (!activeInHierarchy)
					{
						if (TryGetExecutionOrder(networkObject, out var order))
						{
							num2 = order;
						}
						else
						{
							Log.Warn($"Unable to get execution order for {networkObject}. " + "Because the object is initially inactive, Fusion is unable to guarantee the script's Awake will be invoked before Spawned. Please implement TryGetExecutionOrder.");
						}
					}
					_arrayBufferNB.Clear();
					TransformPath x = _networkObjectsPaths[num];
					NetworkTRSP networkTRSP = null;
					for (int num3 = _allSimulationBehaviours.Count - 1; num3 >= 0; num3--)
					{
						SimulationBehaviour simulationBehaviour = _allSimulationBehaviours[num3];
						TransformPath y = _pathCache.Create(simulationBehaviour.transform);
						if (_pathCache.IsEqualOrAncestorOf(in x, in y))
						{
							if (simulationBehaviour is NetworkTRSP networkTRSP2)
							{
								if (!networkTRSP && networkObject.gameObject == networkTRSP2.gameObject)
								{
									networkTRSP = networkTRSP2;
								}
								else
								{
									_arrayBufferNB.Add(networkTRSP2);
								}
							}
							else if (simulationBehaviour is NetworkBehaviour item)
							{
								_arrayBufferNB.Add(item);
							}
							flag |= PostprocessBehaviour(simulationBehaviour);
							_allSimulationBehaviours.RemoveAt(num3);
							if (num2.HasValue)
							{
								if (TryGetExecutionOrder(simulationBehaviour, out var order2))
								{
									if (num2 <= order2)
									{
										Log.Warn($"{networkObject} execution order is less or equal than of the script {simulationBehaviour}. " + "Because the object is initially inactive, Spawned callback will be invoked before the script's Awake on activation.");
									}
								}
								else
								{
									Log.Warn($"Unable to get execution order for {simulationBehaviour}. " + "Because the object is initially inactive, Fusion is unable to guarantee the script's Awake will be invoked before Spawned. Please implement TryGetExecutionOrder.");
								}
							}
						}
						else if (_pathCache.Compare(x, y) >= 0)
						{
							break;
						}
					}
					if ((bool)networkTRSP)
					{
						flags |= NetworkObjectFlags.HasMainNetworkTRSP;
						_arrayBufferNB.Add(networkTRSP);
					}
					_arrayBufferNB.Reverse();
					flag |= Set(networkObject, ref networkObject.NetworkedBehaviours, _arrayBufferNB);
					if (!flags.IsVersionCurrent())
					{
						flags = flags.SetCurrentVersion();
					}
					flag |= Set(networkObject, ref networkObject.Flags, flags);
					_arrayBufferNO.Clear();
					int num4 = num - 1;
					while (num4 >= 0 && _pathCache.IsAncestorOf(in x, _networkObjectsPaths[num4]))
					{
						_arrayBufferNO.Add(_allNetworkObjects[num4]);
						num4--;
					}
					flag |= Set(networkObject, ref networkObject.NestedObjects, _arrayBufferNO);
					if (flag | Set(networkObject, ref networkObject.SortKey, GetSortKey(networkObject)))
					{
						SetDirty(networkObject);
						dirty = true;
					}
				}
				return new Result(dirty, count, count2);
			}
			finally
			{
				_pathCache.Clear();
				_allNetworkObjects.Clear();
				_allSimulationBehaviours.Clear();
				_networkObjectsPaths.Clear();
				_arrayBufferNB.Clear();
				_arrayBufferNO.Clear();
			}
		}

		private bool Set<T>(MonoBehaviour host, ref T field, T value)
		{
			if (!EqualityComparer<T>.Default.Equals(field, value))
			{
				field = value;
				return true;
			}
			return false;
		}

		private bool Set<T>(MonoBehaviour host, ref T[] field, List<T> value)
		{
			EqualityComparer<T> comparer = EqualityComparer<T>.Default;
			if (field == null || field.Length != value.Count || !field.SequenceEqual(value, comparer))
			{
				field = value.ToArray();
				return true;
			}
			return false;
		}
	}
}
