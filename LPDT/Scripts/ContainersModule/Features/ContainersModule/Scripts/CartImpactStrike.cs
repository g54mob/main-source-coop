using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab.CartGrabber;
using Features.Movement.Scripts;
using Features.PhysicsVolumeModule.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.ContainersModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class CartImpactStrike : NetworkBehaviour
	{
		[SerializeField]
		private float _recoverySeconds = 2.5f;

		[SerializeField]
		private float _forwardBoost = 1.5f;

		[SerializeField]
		private float _lateralBoost = 1.5f;

		[SerializeField]
		private float _upBoost = 2f;

		private readonly Collider[] _hits = new Collider[32];

		private readonly Dictionary<NetworkId, float> _recentlyStruck = new Dictionary<NetworkId, float>();

		private PhysicsInfluenceVolume _volume;

		private IPointGrabable _cartGrabable;

		private CartFlowDriver _flowDriver;

		private CartV2CargoRegistry _registry;

		private CartImpactZone[] _zones;

		private CancellationTokenSource _cts;

		private void Awake()
		{
			_volume = GetComponent<PhysicsInfluenceVolume>();
			_cartGrabable = GetComponent<IPointGrabable>();
			_flowDriver = GetComponent<CartFlowDriver>();
			_registry = GetComponentInChildren<CartV2CargoRegistry>(includeInactive: true);
			_zones = GetComponentsInChildren<CartImpactZone>(includeInactive: true);
		}

		public override void Spawned()
		{
			_cts = new CancellationTokenSource();
			if (_registry != null)
			{
				_registry.OnStrikeRelayed += HandleStrike;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_registry != null)
			{
				_registry.OnStrikeRelayed -= HandleStrike;
			}
			_cts?.Cancel();
			_cts?.Dispose();
			_cts = null;
		}

		public override void FixedUpdateNetwork()
		{
			if (_volume == null || _registry == null || _zones == null)
			{
				return;
			}
			bool hasStateAuthority = base.Object.HasStateAuthority;
			bool isDriven = IsActivelyGrabbed(_cartGrabable) || (_flowDriver != null && _flowDriver.IsFlowDriving);
			for (int i = 0; i < _zones.Length; i++)
			{
				CartImpactZone cartImpactZone = _zones[i];
				if (cartImpactZone == null)
				{
					continue;
				}
				if (!cartImpactZone.isActiveAndEnabled)
				{
					cartImpactZone.ResetTracking();
					continue;
				}
				cartImpactZone.Evaluate(base.Runner.DeltaTime, isDriven);
				if (hasStateAuthority && cartImpactZone.IsArmed)
				{
					StrikeInZone(cartImpactZone);
				}
			}
		}

		private void StrikeInZone(CartImpactZone zone)
		{
			Vector3 velocity = zone.Velocity;
			int num = Physics.OverlapBoxNonAlloc(zone.WorldCenter, zone.HalfExtents, _hits, zone.transform.rotation, -1, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				Rigidbody attachedRigidbody = _hits[i].attachedRigidbody;
				if (attachedRigidbody == null)
				{
					continue;
				}
				PlayerCharacterMovableBase componentInParent = attachedRigidbody.GetComponentInParent<PlayerCharacterMovableBase>();
				if (!(componentInParent == null) && !(componentInParent.Object == null) && !IsCartGrabber(componentInParent) && !_volume.HoldsRiderSeat(componentInParent) && !_volume.IsCarryingRagdoll(componentInParent) && (!_recentlyStruck.TryGetValue(componentInParent.Object.Id, out var value) || !(Time.time < value)))
				{
					Vector3 lhs = velocity - ((componentInParent.Rigidbody != null) ? componentInParent.Rigidbody.linearVelocity : Vector3.zero);
					lhs.y = 0f;
					if (!(Vector3.Dot(lhs, zone.StrikeDirection) < zone.MinSpeed))
					{
						Vector3 vector = Vector3.Cross(Vector3.up, velocity.normalized);
						float num2 = ((UnityEngine.Random.value < 0.5f) ? (-1f) : 1f);
						Vector3 impulse = velocity * _forwardBoost + vector * (num2 * _lateralBoost);
						impulse.y += _upBoost;
						_recentlyStruck[componentInParent.Object.Id] = Time.time + _recoverySeconds + 1f;
						_registry.RelayStrikeRpc(componentInParent.Object.Id, impulse);
					}
				}
			}
		}

		private void HandleStrike(NetworkId playerId, Vector3 impulse)
		{
			if (base.Runner == null || !base.Runner.TryFindObject(playerId, out var networkObject) || networkObject == null)
			{
				return;
			}
			PlayerCharacterMovableBase playerCharacterMovableBase = networkObject.GetComponent<PlayerCharacterMovableBase>();
			if (playerCharacterMovableBase == null)
			{
				playerCharacterMovableBase = networkObject.GetComponentInChildren<PlayerCharacterMovableBase>();
			}
			if (!(playerCharacterMovableBase == null))
			{
				if (_volume != null)
				{
					_volume.ExcludeFromCarry(playerCharacterMovableBase, _recoverySeconds + 1f);
				}
				StrikePlayer(playerCharacterMovableBase, impulse, (_cts != null) ? _cts.Token : CancellationToken.None).Forget();
			}
		}

		private async UniTaskVoid StrikePlayer(PlayerCharacterMovableBase player, Vector3 impulse, CancellationToken token)
		{
			RagdollEntity ragdoll = player.GetComponentInChildren<RagdollEntity>(includeInactive: true);
			if (ragdoll == null || !ragdoll.HasStateAuthority)
			{
				return;
			}
			bool forced = false;
			if (!ragdoll.IsSimulated)
			{
				ragdoll.AddSimulationReason(RagdollSimulationReasonEnum.HitByCart);
				forced = true;
			}
			float deadline = Time.time + 0.5f;
			while (!ragdoll.IsSimulated && Time.time < deadline)
			{
				if (await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token).SuppressCancellationThrow())
				{
					return;
				}
			}
			if (ragdoll.IsSimulated)
			{
				Rigidbody[] componentsInChildren = ragdoll.GetComponentsInChildren<Rigidbody>();
				foreach (Rigidbody rigidbody in componentsInChildren)
				{
					if (!rigidbody.isKinematic)
					{
						rigidbody.linearVelocity += impulse;
					}
				}
			}
			if (forced && !(await UniTask.Delay(TimeSpan.FromSeconds(_recoverySeconds), DelayType.DeltaTime, PlayerLoopTiming.Update, token).SuppressCancellationThrow()) && !(ragdoll == null))
			{
				ragdoll.RemoveSimulationReason(RagdollSimulationReasonEnum.HitByCart);
			}
		}

		private static bool IsActivelyGrabbed(IPointGrabable grabable)
		{
			if (grabable != null)
			{
				if (grabable.GrabbedByPlayersCount <= 0)
				{
					return grabable.GrabbedBySomethingCount > 0;
				}
				return true;
			}
			return false;
		}

		private bool IsCartGrabber(PlayerCharacterMovableBase player)
		{
			if (_cartGrabable == null || player == null || player.Object == null)
			{
				return false;
			}
			return _cartGrabable.GrabbedByPlayers.Contains(player.Object.StateAuthority.PlayerId);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
