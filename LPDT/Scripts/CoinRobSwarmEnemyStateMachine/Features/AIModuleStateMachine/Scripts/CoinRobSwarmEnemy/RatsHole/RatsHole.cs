using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using DG.Tweening;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.LevelObjectSpawnModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole
{
	[NetworkBehaviourWeaved(193)]
	public class RatsHole : NetworkBehaviour
	{
		private const int CONTENT_CAPACITY = 32;

		[SerializeField]
		private Transform _absorbPoint;

		[SerializeField]
		private RatsHoleAbsorbSettings _absorbSettings;

		private readonly HashSet<NetworkId> _absorbingItemIds = new HashSet<NetworkId>();

		private readonly Dictionary<NetworkId, Sequence> _absorbSequences = new Dictionary<NetworkId, Sequence>();

		private RatsHoleRegistry _registry;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ContentCount", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _ContentCount;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Content", 1, 192)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private RatsHoleContentSlot[] _Content;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe int ContentCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHole.ContentCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHole.ContentCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[Capacity(32)]
		[NetworkedWeaved(1, 192)]
		[NetworkedWeavedArray(32, 6, typeof(ElementReaderWriterUnmanaged<RatsHoleContentSlot, MetaConstant6>))]
		public unsafe NetworkArray<RatsHoleContentSlot> Content
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RatsHole.Content. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkArray<RatsHoleContentSlot>((byte*)Ptr + 4, 32, ElementReaderWriterUnmanaged<RatsHoleContentSlot, MetaConstant6>.GetInstance());
			}
		}

		public bool IsReady
		{
			get
			{
				if (base.Object != null)
				{
					return base.Object.IsValid;
				}
				return false;
			}
		}

		public Transform AbsorbPoint => _absorbPoint;

		public Vector3 AbsorbWorldPosition => AbsorbPoint.position;

		public float AbsorbTriggerDistance => _absorbSettings.AbsorbTriggerDistance;

		public float AbsorbPointRadius => _absorbSettings.AbsorbPointRadius;

		[Inject]
		public void InjectDependencies(RatsHoleRegistry registry)
		{
			_registry = registry;
		}

		public override void Spawned()
		{
			base.Spawned();
			_registry.Register(this);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			KillAllAbsorbSequences();
			_absorbingItemIds.Clear();
			_registry.Unregister(this);
			base.Despawned(runner, hasState);
		}

		public bool TryAbsorbItem(IItem item)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			if (!IsReady)
			{
				return false;
			}
			if (ContentCount >= 32)
			{
				return false;
			}
			if (item == null || item.IsDespawned || item.NetworkObject == null)
			{
				return false;
			}
			NetworkId id = item.NetworkObject.Id;
			if (!id.IsValid || _absorbingItemIds.Contains(id))
			{
				return false;
			}
			if (!item.NetworkObject.TryGetComponent<MonoItem>(out var component))
			{
				return false;
			}
			if (!TryBuildContentSlot(component, out var contentSlot))
			{
				return false;
			}
			Content.Set(ContentCount, contentSlot);
			ContentCount++;
			_absorbingItemIds.Add(id);
			PlayAbsorbVisualRpc(id);
			return true;
		}

		public bool TryAddContentEntry(RatsHoleContentEntry entry)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			if (!IsReady)
			{
				return false;
			}
			if (ContentCount >= 32)
			{
				return false;
			}
			if (entry == null || !entry.PrefabId.IsValid)
			{
				return false;
			}
			Content.Set(ContentCount, entry.ToSlot());
			ContentCount++;
			return true;
		}

		public bool TryTakeContentEntry(RatsHoleEjectSelectionMode selectionMode, out RatsHoleContentEntry entry)
		{
			entry = null;
			if (!base.HasStateAuthority || ContentCount <= 0)
			{
				return false;
			}
			int index = selectionMode switch
			{
				RatsHoleEjectSelectionMode.NewestFirst => ContentCount - 1, 
				RatsHoleEjectSelectionMode.Random => UnityEngine.Random.Range(0, ContentCount), 
				_ => 0, 
			};
			entry = Content[index].ToEntry();
			RemoveContentAt(index);
			return true;
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1566636692u)]
		private void PlayAbsorbVisualRpc([RpcPayload(4)] NetworkId itemId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1566636692u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole.RatsHole::PlayAbsorbVisualRpc(Fusion.NetworkId)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(itemId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (itemId.IsValid && !(base.Runner == null) && base.Runner.TryFindObject(itemId, out var networkObject) && networkObject.TryGetComponent<MonoItem>(out var component) && !component.IsDespawned)
			{
				PrepareItemForAbsorb(component);
				PlayAbsorbTween(component, itemId);
			}
		}

		private bool TryBuildContentSlot(MonoItem monoItem, out RatsHoleContentSlot contentSlot)
		{
			contentSlot = default(RatsHoleContentSlot);
			NetworkObject networkObject = monoItem.Object;
			if (networkObject == null)
			{
				return false;
			}
			NetworkObjectTypeId networkTypeId = networkObject.NetworkTypeId;
			if (!networkTypeId.IsPrefab)
			{
				return false;
			}
			LevelObjectType levelObjectType = LevelObjectType.None;
			if (monoItem.TryGetComponent<LevelObjectMarker>(out var component))
			{
				levelObjectType = component.Type;
			}
			contentSlot = new RatsHoleContentSlot
			{
				PrefabId = networkTypeId.AsPrefabId,
				LevelObjectType = levelObjectType,
				ItemType = monoItem.Type,
				CurrencyValue = monoItem.CurrencyValue,
				MaxCurrencyValue = monoItem.MaxCurrencyValue,
				IsCollectable = monoItem.IsCollectable
			};
			return true;
		}

		private void RemoveContentAt(int index)
		{
			int num = ContentCount - 1;
			for (int i = index; i < num; i++)
			{
				Content.Set(i, Content[i + 1]);
			}
			Content.Set(num, default(RatsHoleContentSlot));
			ContentCount = num;
		}

		private void PrepareItemForAbsorb(MonoItem monoItem)
		{
			monoItem.AvailableForEnemy = false;
			monoItem.SetDespawnEffectsSuppressed(suppressed: true);
			if (monoItem.TryGetComponent<SimplePointGrabable>(out var component))
			{
				component.LocalGrabBlocked = true;
				if (component.HasStateAuthority)
				{
					component.BlockGrabRPC();
				}
				if (component.GrabObject != null && component.Rigidbody != null)
				{
					component.Rigidbody.isKinematic = true;
					component.Rigidbody.detectCollisions = false;
					component.Rigidbody.linearVelocity = Vector3.zero;
					component.Rigidbody.angularVelocity = Vector3.zero;
				}
			}
			Collider[] componentsInChildren = monoItem.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			monoItem.transform.SetParent(null, worldPositionStays: true);
		}

		private void PlayAbsorbTween(MonoItem monoItem, NetworkId itemId)
		{
			KillAbsorbSequence(itemId);
			Transform transform = monoItem.transform;
			Vector3 absorbWorldPosition = AbsorbWorldPosition;
			float distance = Vector3.Distance(transform.position, absorbWorldPosition);
			float duration = ResolveFlightDuration(distance);
			Vector3 endValue = transform.localScale * _absorbSettings.AbsorbEndScale;
			transform.DOKill();
			Sequence sequence = DOTween.Sequence().SetLink(transform.gameObject);
			sequence.Join(transform.DOMove(absorbWorldPosition, duration).SetEase(_absorbSettings.FlightEase));
			sequence.Join(transform.DOScale(endValue, duration).SetEase(_absorbSettings.ScaleEase));
			sequence.OnComplete(delegate
			{
				CompleteAbsorb(monoItem, itemId);
			});
			sequence.OnKill(delegate
			{
				_absorbSequences.Remove(itemId);
			});
			_absorbSequences[itemId] = sequence;
		}

		private float ResolveFlightDuration(float distance)
		{
			if (_absorbSettings.FlightSpeed <= 0.0001f)
			{
				return Mathf.Clamp(_absorbSettings.FlightDuration, _absorbSettings.MinFlightDuration, _absorbSettings.MaxFlightDuration);
			}
			return Mathf.Clamp(distance / _absorbSettings.FlightSpeed, _absorbSettings.MinFlightDuration, _absorbSettings.MaxFlightDuration);
		}

		private void CompleteAbsorb(MonoItem monoItem, NetworkId itemId)
		{
			_absorbSequences.Remove(itemId);
			if (itemId.IsValid)
			{
				_absorbingItemIds.Remove(itemId);
			}
			if (base.HasStateAuthority && monoItem != null && !monoItem.IsDespawned)
			{
				monoItem.DespawnItem();
			}
		}

		private void KillAbsorbSequence(NetworkId itemId)
		{
			if (_absorbSequences.TryGetValue(itemId, out var value))
			{
				_absorbSequences.Remove(itemId);
				value.Kill();
			}
		}

		private void KillAllAbsorbSequences()
		{
			if (_absorbSequences.Count != 0)
			{
				Sequence[] array = new Sequence[_absorbSequences.Count];
				_absorbSequences.Values.CopyTo(array, 0);
				_absorbSequences.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Kill();
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			ContentCount = _ContentCount;
			NetworkBehaviourUtils.InitializeNetworkArray(Content, _Content, "Content");
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_ContentCount = ContentCount;
			NetworkBehaviourUtils.CopyFromNetworkArray(Content, ref _Content);
		}

		[NetworkRpcWeavedInvoker(1566636692u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayAbsorbVisualRpc_0040Invoker1566636692([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out NetworkId value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RatsHole)context.TargetBehaviour).PlayAbsorbVisualRpc(value);
		}
	}
}
