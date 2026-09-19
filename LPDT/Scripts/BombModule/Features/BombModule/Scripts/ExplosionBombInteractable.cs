using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.InteractModule.Scripts;
using Features.ItemCollisionModule.Scripts;
using Features.ItemDamageModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.ScreenShakeModule.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.BombModule.Scripts
{
	[NetworkBehaviourWeaved(57)]
	public class ExplosionBombInteractable : InteractableBase
	{
		[SerializeField]
		private ParticleSystem _particleSystem;

		[SerializeField]
		private float _enemiesDamage = 10f;

		[SerializeField]
		private float _itemsDamage = 10f;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private float _forceRange;

		[SerializeField]
		private float _forceStrength;

		[SerializeField]
		private float _playerThrowForce = 250f;

		[SerializeField]
		private AnimationCurve _forceFalloffCurve;

		[SerializeField]
		private AnimationCurve _damageFalloffCurve;

		[SerializeField]
		private Collider _collider;

		[SerializeField]
		private float _delayDuration = 2f;

		[SerializeField]
		private float _playerKnockbackUpBias = 0.6f;

		[SerializeField]
		private EventReference _fuseSound;

		[SerializeField]
		private EventReference _explosionSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		public CinemachineImpulseSource _cinemachineImpulseSource;

		[SerializeField]
		public ScreenShakeData _screenShakeDataOnSoot;

		private EventInstance _fuseSoundInstance;

		private IItemCostReduceService _itemCostReduceService;

		private IAudioService _audioService;

		private PlayerStatesConfiguration _playerStatesConfiguration;

		private IDamageable _ownerDamageable;

		private float _ownerDamageMultiplier = 1f;

		[WeaverGenerated]
		[DefaultForProperty("IsInteracted", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsInteracted;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedAttribution", 1, 52)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private DamageRpcSource _NetworkedAttribution;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedDespawnPosition", 53, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _NetworkedDespawnPosition;

		[WeaverGenerated]
		[DefaultForProperty("HasNetworkedDespawnPosition", 56, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _HasNetworkedDespawnPosition;

		private bool _isPendingInteraction;

		private bool _hasPendingDespawn;

		private Vector3 _pendingDespawnPosition;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe bool IsInteracted
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ExplosionBombInteractable.IsInteracted. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ExplosionBombInteractable.IsInteracted. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(1, 52)]
		private unsafe DamageRpcSource NetworkedAttribution
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ExplosionBombInteractable.NetworkedAttribution. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(DamageRpcSource*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ExplosionBombInteractable.NetworkedAttribution. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(DamageRpcSource*)(Ptr + 1) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(53, 3)]
		private unsafe Vector3 NetworkedDespawnPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ExplosionBombInteractable.NetworkedDespawnPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 53);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ExplosionBombInteractable.NetworkedDespawnPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 53) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(56, 1)]
		private unsafe NetworkBool HasNetworkedDespawnPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ExplosionBombInteractable.HasNetworkedDespawnPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 56);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ExplosionBombInteractable.HasNetworkedDespawnPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 56) = value;
			}
		}

		public override void Spawned()
		{
			_fuseSoundInstance = _audioService.CreateInstance(_fuseSound);
			if (base.HasStateAuthority && NetworkedAttribution.IsDefault)
			{
				InitializeDamageAttribution(DamageDataSourceExtensions.ForPlayerAttack(DamageType.Explosion));
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			TryPlayDespawnVfx();
			_audioService.StopInstance(_fuseSoundInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			_audioService.ReleaseInstance(_fuseSoundInstance);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _hasPendingDespawn)
			{
				if (!HasNetworkedDespawnPosition)
				{
					NetworkedDespawnPosition = _pendingDespawnPosition;
					HasNetworkedDespawnPosition = true;
				}
				else
				{
					base.Object.DespawnHierarchy();
				}
			}
		}

		[Inject]
		private void InjectDependencies(IItemCostReduceService itemCostReduceService, IAudioService audioService, PlayerStatesConfiguration playerStatesConfiguration)
		{
			_itemCostReduceService = itemCostReduceService;
			_audioService = audioService;
			_playerStatesConfiguration = playerStatesConfiguration;
		}

		public override void Interact()
		{
			if (IsInteractable && !IsInteracted)
			{
				if (base.Object.HasStateAuthority)
				{
					ExecuteInteractionLogic();
					return;
				}
				_isPendingInteraction = true;
				base.Object.RequestStateAuthority();
			}
		}

		public override void StateAuthorityChanged()
		{
			ClearLocalPendingInteractionUnlessAuthority(ref _isPendingInteraction);
			base.StateAuthorityChanged();
			if (base.HasStateAuthority && _isPendingInteraction)
			{
				_isPendingInteraction = false;
				ExecuteInteractionLogic();
			}
		}

		private void ExecuteInteractionLogic()
		{
			if (!IsInteracted)
			{
				IsInteracted = true;
				StartCoroutine(DelayedExplosion());
			}
		}

		public void InitializeOwnerDamage(IDamageable ownerDamageable, float ownerDamageMultiplier)
		{
			_ownerDamageable = ownerDamageable;
			_ownerDamageMultiplier = Mathf.Max(0f, ownerDamageMultiplier);
		}

		public void InitializeDamageAttribution(DamageSource source)
		{
			if (base.HasStateAuthority)
			{
				NetworkedAttribution = DamageDataSourceExtensions.ToRpc(source);
			}
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

		private void Update()
		{
			if (_fuseSoundInstance.isValid())
			{
				_fuseSoundInstance.set3DAttributes(_soundSourceBehaviour.SoundSourceTransform.position.To3DAttributes());
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2353449039u)]
		private void PlayFuseSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2353449039u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BombModule.Scripts.ExplosionBombInteractable::PlayFuseSoundRpc()", invokeInfo, PlayerRef.None);
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
			_audioService.StartInstanceWith3DAttributes(_fuseSoundInstance, _soundSourceBehaviour);
		}

		private static Vector3 ClosestPointOn(Collider collider, Vector3 position)
		{
			if (!(collider is BoxCollider) && !(collider is SphereCollider) && !(collider is CapsuleCollider) && !(collider is MeshCollider { convex: not false }))
			{
				return collider.bounds.ClosestPoint(position);
			}
			return collider.ClosestPoint(position);
		}

		private IEnumerator DelayedExplosion()
		{
			PlayFuseSoundRpc();
			float timer = 0f;
			while (timer < _delayDuration)
			{
				timer += Time.deltaTime;
				yield return null;
			}
			try
			{
				Vector3 position = base.transform.position;
				Collider[] array = Physics.OverlapSphere(position, _forceRange);
				Dictionary<GameObject, Collider> dictionary = new Dictionary<GameObject, Collider>();
				Collider[] array2 = array;
				foreach (Collider collider in array2)
				{
					if (collider != null)
					{
						dictionary.TryAdd(collider.gameObject, collider);
					}
				}
				List<IDamageable> damageableTargets = new List<IDamageable>();
				foreach (Collider value in dictionary.Values)
				{
					if (!(value == null))
					{
						try
						{
							ApplyBlastToHit(value, position, damageableTargets);
						}
						catch (Exception arg)
						{
							Debug.LogWarning($"[ExplosionBombInteractable] blast hit failed, skipping it: {arg}");
						}
					}
				}
			}
			catch (Exception arg2)
			{
				Debug.LogError($"[ExplosionBombInteractable] blast aborted early: {arg2}");
			}
			finally
			{
				DespawnRpc();
			}
		}

		private void ApplyBlastToHit(Collider hit, Vector3 origin, List<IDamageable> damageableTargets)
		{
			Rigidbody attachedRigidbody = hit.attachedRigidbody;
			if (attachedRigidbody == _rigidbody)
			{
				return;
			}
			Vector3 vector = ClosestPointOn(hit, origin);
			float num = Mathf.Clamp01(Vector3.Distance(origin, vector) / Mathf.Max(0.0001f, _forceRange));
			float num2 = ((_forceFalloffCurve != null) ? _forceFalloffCurve.Evaluate(1f - num) : (1f - num));
			float falloffDamage = ((_damageFalloffCurve != null) ? _damageFalloffCurve.Evaluate(1f - num) : (1f - num));
			Vector3 vector2 = vector - origin;
			if (vector2.sqrMagnitude < 1E-06f)
			{
				vector2 = hit.transform.position - origin;
			}
			vector2 = vector2.normalized;
			MonoItem component2;
			if (FindComponent<IDamageable>(hit.gameObject, out var component) && !damageableTargets.Contains(component))
			{
				float damageAmount = GetDamageAmount(component, falloffDamage);
				DamageSource source = (NetworkedAttribution.IsDefault ? DamageDataSourceExtensions.ForPlayerAttack(DamageType.Explosion) : DamageDataSourceExtensions.FromRpc(NetworkedAttribution));
				float stunThrowMultiplier = ((_playerStatesConfiguration != null) ? _playerStatesConfiguration.StunThrowMultiplier : 1f);
				component.ApplyStunHit(damageAmount, vector2, _playerThrowForce * num2, base.Object.StateAuthority.PlayerId, NetworkedAttribution, source, stunPlayers: true, stunThrowMultiplier, _playerKnockbackUpBias);
				damageableTargets.Add(component);
			}
			else if (FindComponent<MonoItem>(hit.gameObject, out component2))
			{
				if (component2.Object == null || !component2.Object.IsValid)
				{
					if (attachedRigidbody != null)
					{
						attachedRigidbody.AddForce(vector2 * _forceStrength * num2, ForceMode.Impulse);
					}
				}
				else if (attachedRigidbody != null && !_itemCostReduceService.ProcessItemCollisionData(new ItemCollisionData(component2, _itemsDamage, 3f, _collider), base.Runner.LocalPlayer, isIgnoreLimits: true))
				{
					component2.AddForce(_forceStrength * num2, vector2, ForceMode.Impulse);
				}
			}
			else if (attachedRigidbody != null)
			{
				Vector3 force = vector2 * _forceStrength * num2;
				attachedRigidbody.AddForce(force, ForceMode.Impulse);
			}
		}

		private float GetDamageAmount(IDamageable damageable, float falloffDamage)
		{
			float num = _enemiesDamage * falloffDamage;
			if (damageable != _ownerDamageable)
			{
				return num;
			}
			return num * _ownerDamageMultiplier;
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 2742956975u)]
		private void DespawnRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2742956975u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BombModule.Scripts.ExplosionBombInteractable::DespawnRpc()", invokeInfo, PlayerRef.None);
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
			if (!_hasPendingDespawn && !HasNetworkedDespawnPosition)
			{
				_pendingDespawnPosition = base.transform.position;
				_hasPendingDespawn = true;
			}
		}

		private void TryPlayDespawnVfx()
		{
			Vector3 networkedDespawnPosition = NetworkedDespawnPosition;
			if (_cinemachineImpulseSource != null)
			{
				_cinemachineImpulseSource.ImpulseDefinition.TimeEnvelope.AttackTime = _screenShakeDataOnSoot.InTime;
				_cinemachineImpulseSource.ImpulseDefinition.TimeEnvelope.DecayTime = _screenShakeDataOnSoot.OutTime;
				_cinemachineImpulseSource.ImpulseDefinition.FrequencyGain = _screenShakeDataOnSoot.FrequencyGain;
				_cinemachineImpulseSource.GenerateImpulseWithForce(_screenShakeDataOnSoot.Force);
			}
			if (_particleSystem != null)
			{
				UnityEngine.Object.Instantiate(_particleSystem, networkedDespawnPosition, base.transform.rotation).Play(withChildren: true);
			}
			_audioService.PlayOneShotAttached(_explosionSound, _soundSourceBehaviour);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			IsInteracted = _IsInteracted;
			NetworkedAttribution = _NetworkedAttribution;
			NetworkedDespawnPosition = _NetworkedDespawnPosition;
			HasNetworkedDespawnPosition = _HasNetworkedDespawnPosition;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_IsInteracted = IsInteracted;
			_NetworkedAttribution = NetworkedAttribution;
			_NetworkedDespawnPosition = NetworkedDespawnPosition;
			_HasNetworkedDespawnPosition = HasNetworkedDespawnPosition;
		}

		[NetworkRpcWeavedInvoker(2353449039u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayFuseSoundRpc_0040Invoker2353449039([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((ExplosionBombInteractable)context.TargetBehaviour).PlayFuseSoundRpc();
		}

		[NetworkRpcWeavedInvoker(2742956975u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DespawnRpc_0040Invoker2742956975([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((ExplosionBombInteractable)context.TargetBehaviour).DespawnRpc();
		}
	}
}
