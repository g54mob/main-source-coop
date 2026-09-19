using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.ScreenShakeModule.Scripts;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.WeaponModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class PistolBehaviour : WeaponMonoBase
	{
		[SerializeField]
		[Header("Force Settings")]
		[Tooltip("Direction of the force in local or world space")]
		private Vector3 _forceDirection = Vector3.forward;

		[SerializeField]
		[Tooltip("Magnitude of the force to apply")]
		[Range(0f, 1000f)]
		private float _forceMagnitude = 10f;

		[SerializeField]
		private Vector3 _playerForceDirectionMultiplier = Vector3.one;

		[SerializeField]
		private Vector3 _additionalPlayerForceDirection = Vector3.up;

		[SerializeField]
		[Tooltip("Magnitude of the force to apply on player")]
		[Range(0f, 1000f)]
		private float _playerForceMagnitude = 10f;

		[SerializeField]
		[Tooltip("Type of force to apply")]
		private ForceMode _forceMode = ForceMode.Impulse;

		[SerializeField]
		private ParticleSystem _shootParticle;

		[SerializeField]
		private ProjectileBehaviourBase _bullet;

		[SerializeField]
		private Transform _shootPoint;

		[SerializeField]
		private EventReference _shootSound;

		private EventInstance _shootSoundInstance;

		private PlayerDamageablesTrackModel _playerDamageablesTrackModel;

		private IScreenShakeService _screenShakeService;

		private IAudioService _audioService;

		[field: SerializeField]
		public CinemachineImpulseSource CinemachineImpulseSource { get; private set; }

		[field: SerializeField]
		public ScreenShakeData ScreenShakeDataOnSoot { get; private set; }

		[Inject]
		public void InjectDependencies(PlayerDamageablesTrackModel playerDamageablesTrackModel, IScreenShakeService screenShakeService, IAudioService audioService)
		{
			_playerDamageablesTrackModel = playerDamageablesTrackModel;
			_screenShakeService = screenShakeService;
			_audioService = audioService;
		}

		public override void Spawned()
		{
			base.Spawned();
			_shootSoundInstance = _audioService.CreateInstance(_shootSound);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (!base.HasStateAuthority && !runner.IsShutdown)
			{
				PlayParticleRpc();
			}
			_audioService.ReleaseInstance(_shootSoundInstance);
		}

		public override bool TryToAttack()
		{
			if (!base.TryToAttack())
			{
				return false;
			}
			_forceApplierBehaviour.ApplyForceCustom(_forceDirection, _forceMagnitude, _forceMode);
			Vector3 vector = base.transform.rotation * Vector3.back;
			vector = new Vector3(vector.x * _playerForceDirectionMultiplier.x + _additionalPlayerForceDirection.x, vector.y * _playerForceDirectionMultiplier.y + _additionalPlayerForceDirection.y, vector.z * _playerForceDirectionMultiplier.z + _additionalPlayerForceDirection.z);
			_playerDamageablesTrackModel.AllPlayerDamageables[base.Object.StateAuthority.PlayerId].AddRPCForce(_playerForceMagnitude, vector, _forceMode);
			SpawnBullet();
			return true;
		}

		private async UniTask SpawnBullet()
		{
			await base.Runner.SpawnAsync(_bullet, _shootPoint.position, _shootPoint.rotation);
			PlayParticleRpc();
			UpdateUsageCountRPC();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 4239246492u)]
		private void PlayParticleRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4239246492u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.WeaponModule.Scripts.PistolBehaviour::PlayParticleRpc()", invokeInfo, PlayerRef.None);
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
				_screenShakeService.TriggerScreenShake(CinemachineImpulseSource, ScreenShakeDataOnSoot);
			}
			_shootParticle.Play();
			_audioService.StartInstanceWith3DAttributes(_shootSoundInstance, _soundSourceBehaviour);
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

		[NetworkRpcWeavedInvoker(4239246492u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayParticleRpc_0040Invoker4239246492([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PistolBehaviour)context.TargetBehaviour).PlayParticleRpc();
		}
	}
}
