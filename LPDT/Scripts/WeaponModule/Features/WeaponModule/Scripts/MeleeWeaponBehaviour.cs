using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.GrabModule.Scripts;
using Features.ItemCollisionModule.Scripts;
using Features.ItemDamageModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.ScreenShakeModule.Scripts;
using Fusion;
using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;
using Zenject;

namespace Features.WeaponModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class MeleeWeaponBehaviour : WeaponMonoBase
	{
		[SerializeField]
		private float _attackRange = 0.5f;

		[SerializeField]
		protected float _searchRange = 0.3f;

		[SerializeField]
		protected LayerMask _targetLayers = -1;

		[SerializeField]
		private NetworkObject _hitParticle;

		[SerializeField]
		protected Transform _attackOrigin;

		[FormerlySerializedAs("_hitSound")]
		[SerializeField]
		private EventReference _hitDamageableSound;

		[SerializeField]
		private EventReference _hitEnvironmentSound;

		[SerializeField]
		private float _enemiesDamage = 5f;

		[SerializeField]
		private float _itemsDamage = 5f;

		[SerializeField]
		private float _velocityToAttack = 10f;

		[SerializeField]
		private float _velocityToAttackOnGamepads = 5f;

		[SerializeField]
		private AnimationCurve _forceFalloffCurve;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private Collider _collider;

		[SerializeField]
		private float _forceRange;

		[SerializeField]
		private float _forceStrength;

		[SerializeField]
		private bool _isSingleHitParticle;

		private EventInstance _hitDamageableSoundInstance;

		private EventInstance _environmentHitSoundInstance;

		private IItemCostReduceService _itemCostReduceService;

		private IScreenShakeService _screenShakeService;

		private IAudioService _audioService;

		private IInputDeviceService _inputDeviceService;

		protected bool _waitingForCollision;

		[field: SerializeField]
		public CinemachineImpulseSource CinemachineImpulseSource { get; private set; }

		[field: SerializeField]
		public ScreenShakeData ScreenShakeDataOnHit { get; private set; }

		private float CurrentVelocityToAttack
		{
			get
			{
				if (!_inputDeviceService.IsCurrentActiveDeviceGamepad() && !_inputDeviceService.IsCurrentActiveDeviceJoystick())
				{
					return _velocityToAttack;
				}
				return _velocityToAttackOnGamepads;
			}
		}

		[Inject]
		public void InjectDependencies(IItemCostReduceService itemCostReduceService, IScreenShakeService screenShakeService, IAudioService audioService, IInputDeviceService inputDeviceService)
		{
			_itemCostReduceService = itemCostReduceService;
			_screenShakeService = screenShakeService;
			_audioService = audioService;
			_inputDeviceService = inputDeviceService;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (!_hitDamageableSound.IsNull)
			{
				_hitDamageableSoundInstance = _audioService.CreateInstance(_hitDamageableSound);
			}
			if (!_hitEnvironmentSound.IsNull)
			{
				_environmentHitSoundInstance = _audioService.CreateInstance(_hitEnvironmentSound);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (!base.HasStateAuthority && !runner.IsShutdown)
			{
				try
				{
					PlayHitEnvironmentSoundRpc();
				}
				catch (InvalidOperationException)
				{
				}
			}
			if (_hitDamageableSoundInstance.isValid())
			{
				_audioService.ReleaseInstance(_hitDamageableSoundInstance);
			}
			if (_environmentHitSoundInstance.isValid())
			{
				_audioService.ReleaseInstance(_environmentHitSoundInstance);
			}
		}

		private void FixedUpdate()
		{
			if (_rigidbody.linearVelocity.magnitude > CurrentVelocityToAttack)
			{
				Attack();
			}
		}

		public override bool TryToAttack()
		{
			if (!base.TryToAttack())
			{
				return false;
			}
			if (!_waitingForCollision)
			{
				WaitForCollisionAsync().Forget();
			}
			return true;
		}

		private async UniTaskVoid WaitForCollisionAsync()
		{
			_waitingForCollision = true;
			float elapsed = 0f;
			float timeout = 0.5f;
			while (elapsed < timeout)
			{
				elapsed += Time.deltaTime;
				if (Physics.OverlapSphere((_attackOrigin != null) ? _attackOrigin.position : base.transform.position, _searchRange, _targetLayers).Any((Collider c) => c.gameObject != base.gameObject))
				{
					AttackLogic();
					break;
				}
				await UniTask.Yield(PlayerLoopTiming.Update);
			}
			_waitingForCollision = false;
		}

		protected async void AttackLogic()
		{
			if (base.Object == null)
			{
				return;
			}
			Vector3 attackPosition = ((_attackOrigin != null) ? _attackOrigin.position : base.transform.position);
			List<Collider> list = (from c in Physics.OverlapSphere(attackPosition, _attackRange, _targetLayers)
				where c.gameObject != base.gameObject
				select c).ToList();
			List<IDamageable> damageableTargets = new List<IDamageable>();
			bool hadNonDamageableHit = false;
			int hitParticleCount = 0;
			foreach (Collider item in list)
			{
				Rigidbody attachedRigidbody = item.attachedRigidbody;
				if (attachedRigidbody == _rigidbody)
				{
					continue;
				}
				Vector3 position = base.transform.position;
				Vector3 vector = item.ClosestPoint(position);
				float num = Mathf.Clamp01(Vector3.Distance(position, vector) / Mathf.Max(0.0001f, _forceRange));
				float num2 = ((_forceFalloffCurve != null) ? _forceFalloffCurve.Evaluate(1f - num) : (1f - num));
				Vector3 vector2 = vector - position;
				if (vector2.sqrMagnitude < 1E-06f)
				{
					vector2 = item.transform.position - position;
				}
				MonoItem component2;
				if (FindComponent<IDamageable>(item.gameObject, out var component) && !damageableTargets.Contains(component))
				{
					if (IsHolderDamageable(component))
					{
						continue;
					}
					component.DamageRPC(_enemiesDamage, base.Object.StateAuthority.PlayerId, DamageDataSourceExtensions.ToRpc(DamageDataSourceExtensions.ForPlayerAttack(DamageType.Melee)));
					component.AddRPCForce(_forceStrength * num2, vector2, ForceMode.Impulse);
					damageableTargets.Add(component);
				}
				else if (FindComponent<MonoItem>(item.gameObject, out component2))
				{
					if (IsContainerCargo(item.gameObject))
					{
						continue;
					}
					hadNonDamageableHit = true;
					if (attachedRigidbody != null && !_itemCostReduceService.ProcessItemCollisionData(new ItemCollisionData(component2, _itemsDamage, 3f, _collider), base.Object.StateAuthority, isIgnoreLimits: true) && !component2.IsDespawned)
					{
						component2.AddForce(_forceStrength * num2, vector2, ForceMode.Impulse);
					}
				}
				else
				{
					hadNonDamageableHit = true;
					if (attachedRigidbody != null)
					{
						Vector3 force = vector2 * _forceStrength * num2;
						attachedRigidbody.AddForce(force, ForceMode.Impulse);
					}
				}
				if (_hitParticle != null && (!_isSingleHitParticle || hitParticleCount < 1))
				{
					Quaternion value = Quaternion.LookRotation((attackPosition - vector).normalized);
					hitParticleCount++;
					if (base.Runner != null)
					{
						await base.Runner.SpawnAsync(_hitParticle, vector, value);
					}
				}
			}
			if (damageableTargets.Count != 0)
			{
				UpdateUsageCountRPC();
				PlayHitDamageableSoundRpc();
			}
			else if (hadNonDamageableHit)
			{
				PlayHitEnvironmentSoundRpc();
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1760199708u)]
		private void PlayHitDamageableSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1760199708u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.WeaponModule.Scripts.MeleeWeaponBehaviour::PlayHitDamageableSoundRpc()", invokeInfo, PlayerRef.None);
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
			if (CinemachineImpulseSource != null)
			{
				_screenShakeService.TriggerScreenShake(CinemachineImpulseSource, ScreenShakeDataOnHit);
			}
			if (_hitDamageableSoundInstance.isValid())
			{
				_audioService.StartInstanceWith3DAttributes(_hitDamageableSoundInstance, _soundSourceBehaviour);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 907748080u)]
		private void PlayHitEnvironmentSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(907748080u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.WeaponModule.Scripts.MeleeWeaponBehaviour::PlayHitEnvironmentSoundRpc()", invokeInfo, PlayerRef.None);
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
			if (CinemachineImpulseSource != null)
			{
				_screenShakeService.TriggerScreenShake(CinemachineImpulseSource, ScreenShakeDataOnHit);
			}
			if (_environmentHitSoundInstance.isValid())
			{
				_audioService.StartInstanceWith3DAttributes(_environmentHitSoundInstance, _soundSourceBehaviour);
			}
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

		private static bool IsContainerCargo(GameObject candidate)
		{
			IPointGrabable componentInParent = candidate.GetComponentInParent<IPointGrabable>();
			if (componentInParent == null)
			{
				return false;
			}
			if (!componentInParent.InCart)
			{
				if (componentInParent.Carts != null)
				{
					return componentInParent.Carts.Count > 0;
				}
				return false;
			}
			return true;
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

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}

		[NetworkRpcWeavedInvoker(1760199708u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayHitDamageableSoundRpc_0040Invoker1760199708([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MeleeWeaponBehaviour)context.TargetBehaviour).PlayHitDamageableSoundRpc();
		}

		[NetworkRpcWeavedInvoker(907748080u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayHitEnvironmentSoundRpc_0040Invoker907748080([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MeleeWeaponBehaviour)context.TargetBehaviour).PlayHitEnvironmentSoundRpc();
		}
	}
}
