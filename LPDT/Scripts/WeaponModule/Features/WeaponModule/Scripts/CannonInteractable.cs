using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.InteractModule.Scripts;
using Features.ScreenShakeModule.Scripts;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.WeaponModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class CannonInteractable : InteractableBase
	{
		[SerializeField]
		private NetworkObject _bullet;

		[SerializeField]
		private Transform _shootPoint;

		[SerializeField]
		private ParticleSystem _shootParticle;

		[SerializeField]
		private ParticleSystem _fuseParticle;

		[SerializeField]
		private EventReference _shootSound;

		[SerializeField]
		private EventReference _fuseSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private float _delayDuration = 2f;

		[SerializeField]
		public CinemachineImpulseSource _cinemachineImpulseSource;

		[SerializeField]
		public ScreenShakeData _screenShakeDataOnSoot;

		private EventInstance _fuseSoundInstance;

		[WeaverGenerated]
		[DefaultForProperty("IsInInteraction", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsInInteraction;

		private bool _isPendingInteraction;

		private IAudioService _audioService;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe bool IsInInteraction
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CannonInteractable.IsInInteraction. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CannonInteractable.IsInInteraction. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		[Inject]
		public void InjectDependencies(IAudioService audioService)
		{
			_audioService = audioService;
		}

		public override void Spawned()
		{
			_fuseSoundInstance = _audioService.CreateInstance(_fuseSound);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			StopFuseParticle();
			_audioService.StopInstance(_fuseSoundInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			_audioService.ReleaseInstance(_fuseSoundInstance);
		}

		public override void Interact()
		{
			if (IsInteractable && !IsInInteraction)
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
			IsInInteraction = true;
			StartCoroutine(DelayedExplosion());
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
			SpawnBullet();
		}

		private async UniTask SpawnBullet()
		{
			await base.Runner.SpawnAsync(_bullet, _shootPoint.position, _shootPoint.rotation);
			PlayParticleRpc();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 325979307u)]
		private void PlayFuseSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(325979307u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.WeaponModule.Scripts.CannonInteractable::PlayFuseSoundRpc()", invokeInfo, PlayerRef.None);
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
			PlayFuseParticle();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2590308817u)]
		private void PlayParticleRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2590308817u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.WeaponModule.Scripts.CannonInteractable::PlayParticleRpc()", invokeInfo, PlayerRef.None);
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
			if (_cinemachineImpulseSource != null)
			{
				_cinemachineImpulseSource.ImpulseDefinition.TimeEnvelope.AttackTime = _screenShakeDataOnSoot.InTime;
				_cinemachineImpulseSource.ImpulseDefinition.TimeEnvelope.DecayTime = _screenShakeDataOnSoot.OutTime;
				_cinemachineImpulseSource.ImpulseDefinition.FrequencyGain = _screenShakeDataOnSoot.FrequencyGain;
				_cinemachineImpulseSource.GenerateImpulseWithForce(_screenShakeDataOnSoot.Force);
			}
			_shootParticle.Play();
			StartCoroutine(FuseSoundFadeOut());
			_audioService.PlayOneShotAttached(_shootSound, _soundSourceBehaviour);
		}

		private IEnumerator FuseSoundFadeOut()
		{
			float timer = 0f;
			while (timer < 0.5f)
			{
				timer += Time.deltaTime;
				_fuseSoundInstance.setVolume(Mathf.Lerp(1f, 0f, timer / 0.5f));
				yield return null;
			}
			_audioService.StopInstance(_fuseSoundInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			_fuseSoundInstance.setVolume(1f);
			StopFuseParticle();
		}

		private void PlayFuseParticle()
		{
			if (!(_fuseParticle == null))
			{
				_fuseParticle.Play(withChildren: true);
			}
		}

		private void StopFuseParticle()
		{
			if (!(_fuseParticle == null))
			{
				_fuseParticle.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			IsInInteraction = _IsInInteraction;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_IsInInteraction = IsInInteraction;
		}

		[NetworkRpcWeavedInvoker(325979307u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayFuseSoundRpc_0040Invoker325979307([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CannonInteractable)context.TargetBehaviour).PlayFuseSoundRpc();
		}

		[NetworkRpcWeavedInvoker(2590308817u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayParticleRpc_0040Invoker2590308817([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CannonInteractable)context.TargetBehaviour).PlayParticleRpc();
		}
	}
}
