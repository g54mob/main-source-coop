using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using FMODUnity;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.GrabModule.Scripts.PhysGrab.CartGrabber
{
	[NetworkBehaviourWeaved(0)]
	public class CartV2CargoRegistry : NetworkBehaviour, ICartItemsContainer
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private SimplePointGrabable _cartGrabable;

		[SerializeField]
		private LayerMask _ignoreLayerMask;

		[SerializeField]
		private EventReference _itemAddedSound;

		[SerializeField]
		private float _audioCooldown = 0.5f;

		private readonly HashSet<IPointGrabable> _items = new HashSet<IPointGrabable>();

		private readonly Dictionary<IPointGrabable, int> _overlapCounts = new Dictionary<IPointGrabable, int>();

		private readonly Dictionary<IPointGrabable, int> _adderPlayerId = new Dictionary<IPointGrabable, int>();

		private readonly HashSet<IPointGrabable> _creditPending = new HashSet<IPointGrabable>();

		private readonly List<IPointGrabable> _creditFireScratch = new List<IPointGrabable>();

		private float _lastAudioPlayTime;

		private CartItemAddedEventClass _cartItemAddedEventClass;

		private ICargoControlRank _controlRank;

		private bool _controlRankResolved;

		private ICargoControlRank ControlRank
		{
			get
			{
				if (!_controlRankResolved)
				{
					_controlRankResolved = true;
					_controlRank = _rigidbody.GetComponent<ICargoControlRank>();
				}
				return _controlRank;
			}
		}

		public HashSet<IPointGrabable> Items => _items;

		public IPointGrabable CartGrabbable => _cartGrabable;

		Transform ICartItemsContainer.transform => base.transform;

		public event Action<NetworkId, Vector3> OnStrikeRelayed;

		[Inject]
		public void InjectDependencies(CartItemAddedEventClass cartItemAddedEventClass)
		{
			_cartItemAddedEventClass = cartItemAddedEventClass;
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2716613449u)]
		public void RelayStrikeRpc([RpcPayload(4)] NetworkId playerId, [RpcPayload(12)] Vector3 impulse)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2716613449u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.GrabModule.Scripts.PhysGrab.CartGrabber.CartV2CargoRegistry::RelayStrikeRpc(Fusion.NetworkId,UnityEngine.Vector3)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						writer.Write(impulse, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			this.OnStrikeRelayed?.Invoke(playerId, impulse);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (((1 << other.gameObject.layer) & _ignoreLayerMask.value) != 0)
			{
				return;
			}
			IPointGrabable pointGrabable = FindComponent<IPointGrabable>(other.gameObject);
			if (pointGrabable == null || pointGrabable.IgnoredByCart || _cartGrabable == pointGrabable || (ControlRank != null && !ControlRank.ControlsOverCarrierOf(other.gameObject)))
			{
				return;
			}
			_overlapCounts.TryGetValue(pointGrabable, out var value);
			value++;
			_overlapCounts[pointGrabable] = value;
			if (value <= 1)
			{
				pointGrabable.InCart = true;
				pointGrabable.Carts.Add(_rigidbody.gameObject);
				_items.Add(pointGrabable);
				int num = ResolveAdderPlayerId(pointGrabable);
				_adderPlayerId[pointGrabable] = num;
				if (pointGrabable.GrabbedByPlayers.Count == 0 && pointGrabable.GrabbedBySomethingCount == 0)
				{
					FireCredit(pointGrabable, num);
				}
				else
				{
					_creditPending.Add(pointGrabable);
				}
			}
		}

		private void Update()
		{
			if (_creditPending.Count == 0)
			{
				return;
			}
			_creditFireScratch.Clear();
			foreach (IPointGrabable item in _creditPending)
			{
				if (item == null || item.NetworkObject == null)
				{
					_creditFireScratch.Add(item);
				}
				else if (item.GrabbedByPlayers.Count > 0 || item.GrabbedBySomethingCount > 0)
				{
					_adderPlayerId[item] = ResolveAdderPlayerId(item);
				}
				else
				{
					_creditFireScratch.Add(item);
				}
			}
			foreach (IPointGrabable item2 in _creditFireScratch)
			{
				_creditPending.Remove(item2);
				if (item2 == null || item2.NetworkObject == null)
				{
					PruneDestroyed(item2);
					continue;
				}
				int value;
				int adderPlayerId = (_adderPlayerId.TryGetValue(item2, out value) ? value : ResolveAdderPlayerId(item2));
				FireCredit(item2, adderPlayerId);
			}
		}

		private void PruneDestroyed(IPointGrabable grabbable)
		{
			_items.Remove(grabbable);
			_adderPlayerId.Remove(grabbable);
			_overlapCounts.Remove(grabbable);
		}

		private void FireCredit(IPointGrabable grabbable, int adderPlayerId)
		{
			PlayAudio(grabbable, adderPlayerId);
			if (grabbable.Carts.Count > 0 && grabbable.Carts[0] == _rigidbody.gameObject)
			{
				_cartItemAddedEventClass.InvokeOnGrabableAddedInCart(this, grabbable, adderPlayerId);
			}
		}

		private int ResolveAdderPlayerId(IPointGrabable grabbable)
		{
			if (grabbable.GrabbedByPlayers.Count > 0)
			{
				return grabbable.GrabbedByPlayers[0];
			}
			return grabbable.NetworkObject.StateAuthority.PlayerId;
		}

		private void OnTriggerExit(Collider other)
		{
			IPointGrabable pointGrabable = FindComponent<IPointGrabable>(other.gameObject);
			if (pointGrabable == null || pointGrabable.IgnoredByCart || !_overlapCounts.TryGetValue(pointGrabable, out var value))
			{
				return;
			}
			value = Mathf.Max(0, value - 1);
			if (value > 0)
			{
				_overlapCounts[pointGrabable] = value;
				return;
			}
			_overlapCounts.Remove(pointGrabable);
			if (_items.Remove(pointGrabable))
			{
				_adderPlayerId.Remove(pointGrabable);
				_creditPending.Remove(pointGrabable);
				pointGrabable.Carts.Remove(_rigidbody.gameObject);
				if (pointGrabable.Carts.Count == 0)
				{
					pointGrabable.InCart = false;
				}
			}
		}

		private T FindComponent<T>(GameObject other)
		{
			T val = other.GetComponent<T>();
			if (val == null)
			{
				val = other.GetComponentInParent<T>();
			}
			if (val == null)
			{
				val = other.GetComponentInChildren<T>();
			}
			return val;
		}

		private void PlayAudio(IPointGrabable grabbable, int adderPlayerId)
		{
			if (base.Runner.LocalPlayer.PlayerId == adderPlayerId && !(Time.time - _lastAudioPlayTime < _audioCooldown))
			{
				_lastAudioPlayTime = Time.time;
				RuntimeManager.PlayOneShot(_itemAddedSound, base.transform.position);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(2716613449u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RelayStrikeRpc_0040Invoker2716613449([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out NetworkId value, 4);
			payloadReader.Read(out Vector3 value2, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CartV2CargoRegistry)context.TargetBehaviour).RelayStrikeRpc(value, value2);
		}
	}
}
