using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.CompositeItemModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class CompositeItemGroup : NetworkBehaviour
	{
		[SerializeField]
		private float _minImpactForce = 2f;

		[SerializeField]
		private float _setBonusMultiplier = 1.5f;

		[SerializeField]
		private EventReference _breakSound;

		[SerializeField]
		private List<CompositeItemEntry> _items = new List<CompositeItemEntry>();

		[WeaverGenerated]
		[DefaultForProperty("BrokenPartsMaskInternal", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _BrokenPartsMaskInternal;

		private readonly Dictionary<CompositeItem, int> _indexByItem = new Dictionary<CompositeItem, int>();

		private float[] _accumulatedDamage;

		private int _appliedBrokenMask;

		private int[] _parentIndex;

		private GrabObject[] _grabObjects;

		private NetworkObject[] _memberObjects;

		private SimplePointGrabable[] _grabables;

		private NetworkObjectComposite[] _composites;

		private Collider[][] _solidColliders;

		private MonoItem[] _monoItems;

		private int[] _clusterRoot;

		private bool[] _weldArmed;

		private IAudioService _audioService;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe int BrokenPartsMaskInternal
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CompositeItemGroup.BrokenPartsMaskInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CompositeItemGroup.BrokenPartsMaskInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		public IReadOnlyList<CompositeItemEntry> Items => _items;

		public event Action<CompositeItem> OnItemBroken;

		[Inject]
		public void InjectDependencies(IAudioService audioService)
		{
			_audioService = audioService;
		}

		public bool TryGetPieceItemConfigs(List<ItemConfig> outConfigs)
		{
			outConfigs.Clear();
			for (int i = 0; i < _items.Count; i++)
			{
				CompositeItem item = _items[i].Item;
				if (!(item == null))
				{
					MonoItem component = item.GetComponent<MonoItem>();
					if (!(component == null) && !(component.DefaultConfig == null))
					{
						outConfigs.Add(component.DefaultConfig);
					}
				}
			}
			return outConfigs.Count > 0;
		}

		public bool TryGetPieceMonoItems(List<MonoItem> outItems)
		{
			outItems.Clear();
			for (int i = 0; i < _items.Count; i++)
			{
				CompositeItem item = _items[i].Item;
				if (!(item == null))
				{
					MonoItem component = item.GetComponent<MonoItem>();
					if (!(component == null) && !(component.DefaultConfig == null))
					{
						outItems.Add(component);
					}
				}
			}
			return outItems.Count > 0;
		}

		private void Awake()
		{
			_accumulatedDamage = new float[_items.Count];
			for (int i = 0; i < _items.Count; i++)
			{
				if (_items[i].Item != null)
				{
					_indexByItem[_items[i].Item] = i;
				}
			}
			CaptureAssemblyTopology();
		}

		public override void Spawned()
		{
			Array.Clear(_weldArmed, 0, _weldArmed.Length);
			ApplyBrokenState(playBreakSound: false);
			RefreshClusterMasses();
			for (int i = 0; i < _items.Count; i++)
			{
				if (_monoItems[i] != null)
				{
					_monoItems[i].OnDespawn += OnMemberLost;
				}
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			for (int i = 0; i < _items.Count; i++)
			{
				if (_monoItems[i] != null)
				{
					_monoItems[i].OnDespawn -= OnMemberLost;
				}
			}
		}

		public override void Render()
		{
			ApplyBrokenState(playBreakSound: true);
		}

		private void FixedUpdate()
		{
			if (_weldArmed == null || base.Object == null || !base.Object.IsValid)
			{
				return;
			}
			for (int i = 0; i < _items.Count; i++)
			{
				NetworkObject networkObject = _memberObjects[i];
				CompositeItem item = _items[i].Item;
				if (!(networkObject == null) && networkObject.IsValid && !(item == null) && !(item.Rigidbody == null))
				{
					if (!networkObject.HasStateAuthority || item.Rigidbody.isKinematic)
					{
						_weldArmed[i] = false;
					}
					else if (!_weldArmed[i])
					{
						RearmWeld(i);
						_weldArmed[i] = true;
					}
				}
			}
		}

		public bool IsEntryBroken(CompositeItem item)
		{
			if (_indexByItem.TryGetValue(item, out var value))
			{
				return (BrokenPartsMaskInternal & (1 << value)) != 0;
			}
			return false;
		}

		public float AccumulatedDamageOf(CompositeItem item)
		{
			if (!_indexByItem.TryGetValue(item, out var value))
			{
				return 0f;
			}
			return _accumulatedDamage[value];
		}

		public bool TryGetClusterCurrency(CompositeItem item, out int currency)
		{
			currency = 0;
			if (_clusterRoot == null || !_indexByItem.TryGetValue(item, out var value))
			{
				return false;
			}
			int num = _clusterRoot[value];
			int num2 = 0;
			int[] clusterRoot = _clusterRoot;
			for (int i = 0; i < clusterRoot.Length; i++)
			{
				if (clusterRoot[i] == num)
				{
					num2++;
				}
			}
			if (num2 <= 1)
			{
				return false;
			}
			for (int j = 0; j < _items.Count; j++)
			{
				if (_clusterRoot[j] == num && _monoItems[j] != null)
				{
					currency += _monoItems[j].CurrencyValue;
				}
			}
			if (BrokenPartsMaskInternal == 0)
			{
				currency = Mathf.RoundToInt((float)currency * _setBonusMultiplier);
			}
			return true;
		}

		public void ReportImpact(CompositeItem item, float impactForce)
		{
			if (base.HasStateAuthority && !(impactForce < _minImpactForce) && _indexByItem.TryGetValue(item, out var value))
			{
				AccumulateImpact(value, impactForce);
			}
		}

		private void OnMemberLost(IItem item)
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			for (int i = 0; i < _items.Count; i++)
			{
				if (_monoItems[i] != item)
				{
					continue;
				}
				int num = 1 << i;
				for (int j = 0; j < _items.Count; j++)
				{
					if (_parentIndex[j] == i)
					{
						num |= 1 << j;
					}
				}
				BrokenPartsMaskInternal |= num;
				break;
			}
		}

		private void AccumulateImpact(int index, float impactForce)
		{
			CompositeItemEntry compositeItemEntry = _items[index];
			if (compositeItemEntry.Behavior == CompositeItemBreakBehaviorType.JointBreak && (BrokenPartsMaskInternal & (1 << index)) == 0)
			{
				_accumulatedDamage[index] += impactForce;
				if (_accumulatedDamage[index] >= compositeItemEntry.DamageToBreak)
				{
					BrokenPartsMaskInternal |= 1 << index;
				}
			}
		}

		public object ClusterKeyOf(CompositeItem item)
		{
			if (_clusterRoot == null || !_indexByItem.TryGetValue(item, out var value))
			{
				return null;
			}
			int num = _clusterRoot[value];
			int num2 = 0;
			int[] clusterRoot = _clusterRoot;
			for (int i = 0; i < clusterRoot.Length; i++)
			{
				if (clusterRoot[i] == num)
				{
					num2++;
				}
			}
			if (num2 <= 1)
			{
				return null;
			}
			return (GetInstanceID(), num);
		}

		private void ApplyBrokenState(bool playBreakSound)
		{
			int num = BrokenPartsMaskInternal & ~_appliedBrokenMask;
			if (num == 0)
			{
				return;
			}
			for (int i = 0; i < _items.Count; i++)
			{
				if ((num & (1 << i)) != 0)
				{
					BreakEntryLocal(i, playBreakSound);
				}
			}
			_appliedBrokenMask = BrokenPartsMaskInternal;
			RefreshClusterMasses();
		}

		private void CaptureAssemblyTopology()
		{
			_parentIndex = new int[_items.Count];
			_grabObjects = new GrabObject[_items.Count];
			_weldArmed = new bool[_items.Count];
			_memberObjects = new NetworkObject[_items.Count];
			_grabables = new SimplePointGrabable[_items.Count];
			_composites = new NetworkObjectComposite[_items.Count];
			_solidColliders = new Collider[_items.Count][];
			_monoItems = new MonoItem[_items.Count];
			for (int i = 0; i < _items.Count; i++)
			{
				_parentIndex[i] = -1;
				CompositeItemEntry compositeItemEntry = _items[i];
				if (compositeItemEntry.Item == null)
				{
					continue;
				}
				_grabObjects[i] = compositeItemEntry.Item.GetComponent<GrabObject>();
				_memberObjects[i] = compositeItemEntry.Item.GetComponent<NetworkObject>();
				_grabables[i] = compositeItemEntry.Item.GetComponent<SimplePointGrabable>();
				_composites[i] = compositeItemEntry.Item.GetComponent<NetworkObjectComposite>();
				_monoItems[i] = compositeItemEntry.Item.GetComponent<MonoItem>();
				List<Collider> list = new List<Collider>();
				Collider[] componentsInChildren = compositeItemEntry.Item.GetComponentsInChildren<Collider>(includeInactive: true);
				foreach (Collider collider in componentsInChildren)
				{
					if (!collider.isTrigger)
					{
						list.Add(collider);
					}
				}
				_solidColliders[i] = list.ToArray();
				if (compositeItemEntry.JointToBreak == null || compositeItemEntry.JointToBreak.connectedBody == null)
				{
					continue;
				}
				Rigidbody connectedBody = compositeItemEntry.JointToBreak.connectedBody;
				for (int k = 0; k < _items.Count; k++)
				{
					if (k != i && _items[k].Item != null && _items[k].Item.Rigidbody == connectedBody)
					{
						_parentIndex[i] = k;
					}
				}
			}
		}

		private void RefreshClusterMasses()
		{
			if (_parentIndex == null)
			{
				return;
			}
			int[] array = new int[_items.Count];
			for (int i = 0; i < _items.Count; i++)
			{
				int num = i;
				while (_parentIndex[num] >= 0 && (BrokenPartsMaskInternal & (1 << num)) == 0)
				{
					num = _parentIndex[num];
				}
				array[i] = num;
			}
			_clusterRoot = array;
			float[] array2 = new float[_items.Count];
			for (int j = 0; j < _items.Count; j++)
			{
				if (_items[j].Item != null && _items[j].Item.Rigidbody != null)
				{
					array2[array[j]] += _items[j].Item.Rigidbody.mass;
				}
			}
			for (int k = 0; k < _items.Count; k++)
			{
				if (!(_grabObjects[k] == null))
				{
					float num2 = array2[array[k]];
					bool flag = array[k] == k && ClusterSize(array, k) == 1;
					_grabObjects[k].MassOverride = (flag ? 0f : num2);
				}
			}
			RefreshIntraGroupCollisions(array);
			RefreshClusterConnections(array);
		}

		private void RefreshClusterConnections(int[] clusterRoot)
		{
			List<SimplePointGrabable> list = new List<SimplePointGrabable>();
			List<NetworkObject> list2 = new List<NetworkObject>();
			for (int i = 0; i < _items.Count; i++)
			{
				if (_grabables[i] == null)
				{
					continue;
				}
				list.Clear();
				list2.Clear();
				for (int j = 0; j < _items.Count; j++)
				{
					if (clusterRoot[j] == clusterRoot[i])
					{
						if (j != i && _grabables[j] != null)
						{
							list.Add(_grabables[j]);
						}
						if (_memberObjects[j] != null)
						{
							list2.Add(_memberObjects[j]);
						}
					}
				}
				if (base.Object != null)
				{
					list2.Add(base.Object);
				}
				_grabables[i].SetConnectedGrabables(list.ToArray());
				if (_composites[i] != null)
				{
					_composites[i].SetNetworkObjects(list2.ToArray());
				}
			}
		}

		private void RefreshIntraGroupCollisions(int[] clusterRoot)
		{
			for (int i = 0; i < _items.Count; i++)
			{
				if (_solidColliders[i] == null)
				{
					continue;
				}
				for (int j = i + 1; j < _items.Count; j++)
				{
					if (_solidColliders[j] == null)
					{
						continue;
					}
					bool ignore = clusterRoot[i] == clusterRoot[j];
					Collider[] array = _solidColliders[i];
					foreach (Collider collider in array)
					{
						Collider[] array2 = _solidColliders[j];
						foreach (Collider collider2 in array2)
						{
							if (collider != null && collider2 != null)
							{
								Physics.IgnoreCollision(collider, collider2, ignore);
							}
						}
					}
				}
			}
		}

		private static int ClusterSize(int[] clusterRoot, int root)
		{
			int num = 0;
			for (int i = 0; i < clusterRoot.Length; i++)
			{
				if (clusterRoot[i] == root)
				{
					num++;
				}
			}
			return num;
		}

		private void BreakEntryLocal(int index, bool playBreakSound)
		{
			CompositeItemEntry compositeItemEntry = _items[index];
			Vector3 position = ((compositeItemEntry.Item != null) ? compositeItemEntry.Item.transform.position : base.transform.position);
			if (compositeItemEntry.JointToBreak != null)
			{
				UnityEngine.Object.Destroy(compositeItemEntry.JointToBreak);
			}
			if (compositeItemEntry.Item != null)
			{
				compositeItemEntry.Item.NotifyBroken();
			}
			this.OnItemBroken?.Invoke(compositeItemEntry.Item);
			if (playBreakSound)
			{
				TryPlayBreakSound(position);
			}
		}

		private void TryPlayBreakSound(Vector3 position)
		{
			if (base.HasStateAuthority && !_breakSound.IsNull)
			{
				PlayBreakSoundRpc(position);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2685359672u)]
		private void PlayBreakSoundRpc([RpcPayload(12)] Vector3 position)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2685359672u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CompositeItemModule.Scripts.CompositeItemGroup::PlayBreakSoundRpc(UnityEngine.Vector3)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(position, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (!_breakSound.IsNull && _audioService != null)
			{
				_audioService.PlayOneShot(_breakSound, new GenericSoundSource(position, base.transform.GetInstanceID()));
			}
		}

		private void RearmWeld(int index)
		{
			if ((BrokenPartsMaskInternal & (1 << index)) != 0)
			{
				return;
			}
			Joint jointToBreak = _items[index].JointToBreak;
			if (!(jointToBreak == null))
			{
				Rigidbody connectedBody = jointToBreak.connectedBody;
				if (!(connectedBody == null))
				{
					jointToBreak.connectedBody = null;
					jointToBreak.connectedBody = connectedBody;
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			BrokenPartsMaskInternal = _BrokenPartsMaskInternal;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_BrokenPartsMaskInternal = BrokenPartsMaskInternal;
		}

		[NetworkRpcWeavedInvoker(2685359672u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayBreakSoundRpc_0040Invoker2685359672([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out Vector3 value, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CompositeItemGroup)context.TargetBehaviour).PlayBreakSoundRpc(value);
		}
	}
}
