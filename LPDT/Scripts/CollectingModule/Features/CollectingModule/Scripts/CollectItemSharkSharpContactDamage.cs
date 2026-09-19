using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.GrabModule.Scripts;
using Features.ItemCollisionModule.Scripts;
using Features.ItemDamageModule.Scripts;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.CollectingModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class CollectItemSharkSharpContactDamage : NetworkBehaviour
	{
		[SerializeField]
		private Collider _damageCollider;

		[SerializeField]
		private MonoItem _monoItem;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private Rigidbody _rigidbody;

		private IItemCostReduceService _itemCostReduceService;

		private IAudioService _audioService;

		private CollectItemSharkSharpConfig _config;

		private float _nextHitTime;

		private float _lastGrabTime;

		[Inject]
		public void InjectDependencies(IItemCostReduceService itemCostReduceService, IAudioService audioService)
		{
			_itemCostReduceService = itemCostReduceService;
			_audioService = audioService;
		}

		private void Awake()
		{
			_config = (CollectItemSharkSharpConfig)_monoItem.DefaultConfig;
			_lastGrabTime = 0f - _config.GrabSafeTime;
		}

		public override void Spawned()
		{
			base.Spawned();
			_simplePointGrabable.OnGrab += ProcessGrab;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_simplePointGrabable.OnGrab -= ProcessGrab;
		}

		private void ProcessGrab()
		{
			_lastGrabTime = Time.time;
		}

		private void OnCollisionEnter(Collision collision)
		{
			TryHit(collision);
		}

		private void TryHit(Collision collision)
		{
			if (!base.HasStateAuthority || Time.time < _nextHitTime || Time.time - _lastGrabTime < _config.GrabSafeTime || !IsBladeHit(collision))
			{
				return;
			}
			Collider collider = collision.collider;
			Rigidbody attachedRigidbody = collider.attachedRigidbody;
			if (attachedRigidbody == _rigidbody)
			{
				return;
			}
			Vector3 center = _damageCollider.bounds.center;
			Vector3 vector = collider.ClosestPoint(center) - center;
			if (vector.sqrMagnitude < 1E-06f)
			{
				vector = collider.transform.position - center;
			}
			MonoItem component2;
			if (FindComponent<IDamageable>(collider.gameObject, out var component))
			{
				if (!IsHolderDamageable(component))
				{
					component.DamageRPC(_config.EnemiesDamage, base.Object.StateAuthority.PlayerId, DamageDataSourceExtensions.ToRpc(DamageDataSourceExtensions.ForPlayerAttack(DamageType.Melee)));
					component.AddRPCForce(_config.ForceStrength, vector, ForceMode.Impulse);
					_nextHitTime = Time.time + _config.HitCooldown;
					PlayHitDamageableSoundRpc();
				}
			}
			else if (FindComponent<MonoItem>(collider.gameObject, out component2))
			{
				if (!_itemCostReduceService.ProcessItemCollisionData(new ItemCollisionData(component2, _config.ItemsDamage, 3f, _damageCollider), base.Object.StateAuthority, isIgnoreLimits: true) && !component2.IsDespawned)
				{
					component2.AddForce(_config.ForceStrength, vector, ForceMode.Impulse);
				}
				_nextHitTime = Time.time + _config.HitCooldown;
				PlayHitEnvironmentSoundRpc();
			}
			else
			{
				if (attachedRigidbody != null)
				{
					attachedRigidbody.AddForce(vector * _config.ForceStrength, ForceMode.Impulse);
				}
				_nextHitTime = Time.time + _config.HitCooldown;
				PlayHitEnvironmentSoundRpc();
			}
		}

		private bool IsBladeHit(Collision collision)
		{
			int contactCount = collision.contactCount;
			for (int i = 0; i < contactCount; i++)
			{
				if (collision.GetContact(i).thisCollider == _damageCollider)
				{
					return true;
				}
			}
			return false;
		}

		private bool IsHolderDamageable(IDamageable damageable)
		{
			if (_simplePointGrabable.GrabbedByPlayers.Count == 0)
			{
				return false;
			}
			if (!(damageable is PlayerDamageable))
			{
				return false;
			}
			if (damageable.NetworkObject == null || !damageable.NetworkObject.IsValid)
			{
				return false;
			}
			return _simplePointGrabable.GrabbedByPlayers.Contains(damageable.NetworkObject.InputAuthority.PlayerId);
		}

		private bool FindComponent<T>(GameObject other, out T component)
		{
			component = other.GetComponent<T>();
			T val = component;
			if (val == null)
			{
				component = other.GetComponentInParent<T>();
			}
			val = component;
			if (val == null)
			{
				component = other.GetComponentInChildren<T>();
			}
			return component != null;
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2875037926u)]
		private void PlayHitDamageableSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2875037926u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CollectingModule.Scripts.CollectItemSharkSharpContactDamage::PlayHitDamageableSoundRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (!_config.HitDamageableSound.IsNull)
			{
				_audioService.PlayOneShotAttached(_config.HitDamageableSound, _soundSourceBehaviour);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 4265411290u)]
		private void PlayHitEnvironmentSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4265411290u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CollectingModule.Scripts.CollectItemSharkSharpContactDamage::PlayHitEnvironmentSoundRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (!_config.HitEnvironmentSound.IsNull)
			{
				_audioService.PlayOneShotAttached(_config.HitEnvironmentSound, _soundSourceBehaviour);
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

		[NetworkRpcWeavedInvoker(2875037926u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayHitDamageableSoundRpc_0040Invoker2875037926([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CollectItemSharkSharpContactDamage)context.TargetBehaviour).PlayHitDamageableSoundRpc();
		}

		[NetworkRpcWeavedInvoker(4265411290u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayHitEnvironmentSoundRpc_0040Invoker4265411290([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CollectItemSharkSharpContactDamage)context.TargetBehaviour).PlayHitEnvironmentSoundRpc();
		}
	}
}
