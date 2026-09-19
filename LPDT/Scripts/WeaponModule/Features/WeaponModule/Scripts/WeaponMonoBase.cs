using System;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.GrabModule.Scripts;
using Features.InputModule.Scripts.Generated;
using Features.MultiplayerSessionServices.Scripts;
using Features.NetworkInputModule.Scripts;
using Features.ObjectDespawnModule.Scripts;
using Fusion;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;
using Zenject;

namespace Features.WeaponModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class WeaponMonoBase : NetworkBehaviour, IWeapon
	{
		[SerializeField]
		protected ForceApplierBehaviour _forceApplierBehaviour;

		[SerializeField]
		protected SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		[Tooltip("Cooldown time in seconds")]
		private float _cooldownTime = 0.5f;

		[SerializeField]
		[Tooltip("Cooldown time in seconds")]
		private float _grabSafeTime;

		[SerializeField]
		private int _usageCount = 3;

		[SerializeField]
		private EventReference _weaponDestroySound;

		[SerializeField]
		private ParticleSystem _weaponDestroyParticle;

		[SerializeField]
		private Transform _weaponDestroyPoint;

		[SerializeField]
		private bool _isAttackOnInterract = true;

		[SerializeField]
		protected SoundSourceBehaviour _soundSourceBehaviour;

		private EventInstance _weaponDestroySoundInstance;

		private IInputService _inputService;

		private MultiplayerModel _multiplayerModel;

		private ObjectDespawnModel _objectDespawnModel;

		private IAudioService _audioService;

		private float _lastForceTime;

		private float _lastGrabTime;

		private bool _isSpawned;

		[WeaverGenerated]
		[DefaultForProperty("CurrentUsageCount", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _CurrentUsageCount;

		[Networked(AllowPrediction = true)]
		[NetworkedWeaved(0, 1)]
		private unsafe int CurrentUsageCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing WeaponMonoBase.CurrentUsageCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing WeaponMonoBase.CurrentUsageCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Inject]
		public void InjectDependencies(IInputService inputService, MultiplayerModel multiplayerModel, ObjectDespawnModel objectDespawnModel, IAudioService audioService)
		{
			_inputService = inputService;
			_multiplayerModel = multiplayerModel;
			_objectDespawnModel = objectDespawnModel;
			_audioService = audioService;
		}

		public override void Spawned()
		{
			base.Spawned();
			_isSpawned = true;
			_lastForceTime = 0f - _cooldownTime;
			if (!_weaponDestroySound.IsNull)
			{
				_weaponDestroySoundInstance = _audioService.CreateInstance(_weaponDestroySound);
			}
			if (_isAttackOnInterract)
			{
				InputDefaultActions itemInteract = _inputService.ItemInteract;
				itemInteract.Started = (Action)Delegate.Combine(itemInteract.Started, new Action(Attack));
			}
			_simplePointGrabable.OnGrab += ProcessGrabb;
		}

		private void ProcessGrabb()
		{
			_lastGrabTime = Time.time;
		}

		private void Update()
		{
			if (!(base.Runner == null) && _isSpawned && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient && CurrentUsageCount >= _usageCount)
			{
				CurrentUsageCount = 0;
				DespawnWeaponRPC();
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (_simplePointGrabable.GrabbedByPlayers.Contains(base.Runner.LocalPlayer.PlayerId) && GetInput<NetworkInputActions>(out var input) && input.ItemInteractPhase.IsSet(InputActionPhase.Started) && _isAttackOnInterract)
			{
				TryToAttack();
			}
		}

		public virtual bool TryToAttack()
		{
			if (IsOnCooldown())
			{
				return false;
			}
			if (IsOnGrabSafe())
			{
				return false;
			}
			_lastForceTime = Time.time;
			return true;
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1873777657u)]
		protected void UpdateUsageCountRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1873777657u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.WeaponModule.Scripts.WeaponMonoBase::UpdateUsageCountRPC()", invokeInfo, PlayerRef.None);
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
			CurrentUsageCount++;
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2476334294u)]
		private void DespawnWeaponRPC()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2476334294u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.WeaponModule.Scripts.WeaponMonoBase::DespawnWeaponRPC()", invokeInfo, PlayerRef.None);
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
			if (base.Object.HasStateAuthority)
			{
				_objectDespawnModel.AddObjectToDespawn(new DespawnObjectData(base.Object));
			}
		}

		public bool IsOnCooldown()
		{
			return Time.time - _lastForceTime < _cooldownTime;
		}

		public bool IsOnGrabSafe()
		{
			return Time.time - _lastGrabTime < _grabSafeTime;
		}

		public float GetRemainingCooldown()
		{
			float b = _cooldownTime - (Time.time - _lastForceTime);
			return Mathf.Max(0f, b);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_isSpawned = false;
			if (!runner.IsShutdown && _weaponDestroyPoint != null)
			{
				UnityEngine.Object.Instantiate(_weaponDestroyParticle, _weaponDestroyPoint.position, Quaternion.identity).Play();
				if (_weaponDestroySoundInstance.isValid())
				{
					_audioService.StartInstanceWith3DAttributes(_weaponDestroySoundInstance, _soundSourceBehaviour);
				}
			}
			if (_isAttackOnInterract)
			{
				InputDefaultActions itemInteract = _inputService.ItemInteract;
				itemInteract.Started = (Action)Delegate.Remove(itemInteract.Started, new Action(Attack));
			}
			_simplePointGrabable.OnGrab -= ProcessGrabb;
		}

		protected void Attack()
		{
			if (_simplePointGrabable.GrabbedByPlayers.Contains(base.Runner.LocalPlayer.PlayerId))
			{
				TryToAttack();
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			CurrentUsageCount = _CurrentUsageCount;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_CurrentUsageCount = CurrentUsageCount;
		}

		[NetworkRpcWeavedInvoker(1873777657u)]
		[Preserve]
		[WeaverGenerated]
		protected static void UpdateUsageCountRPC_0040Invoker1873777657([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((WeaponMonoBase)context.TargetBehaviour).UpdateUsageCountRPC();
		}

		[NetworkRpcWeavedInvoker(2476334294u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DespawnWeaponRPC_0040Invoker2476334294([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((WeaponMonoBase)context.TargetBehaviour).DespawnWeaponRPC();
		}
	}
}
