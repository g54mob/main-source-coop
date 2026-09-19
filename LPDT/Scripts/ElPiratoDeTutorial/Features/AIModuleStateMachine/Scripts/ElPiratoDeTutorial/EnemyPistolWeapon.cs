using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using Features.AnimationModule.Scripts;
using Features.AudioServiceModule.Scripts;
using Features.WeaponModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial
{
	[NetworkBehaviourWeaved(0)]
	public class EnemyPistolWeapon : EnemyWeaponBase
	{
		[SerializeField]
		private float _sphereCastRadius;

		[SerializeField]
		private Transform _sphereCastPosition;

		[SerializeField]
		private AnimationType _defaultAnimationType;

		[SerializeField]
		private AnimationType _bottomAnimationType;

		[SerializeField]
		private bool _isWithAiming;

		[SerializeField]
		private Transform _shootPoint;

		[SerializeField]
		private ProjectileBehaviourBase _bullet;

		[SerializeField]
		private PirateEnemyContext _pirateEnemyContext;

		[SerializeField]
		private GameObject _weaponVisual;

		[SerializeField]
		private ParticleSystem _particleSystem;

		[SerializeField]
		private EventReference _shootSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private EventInstance _shootSoundInstance;

		private IAudioService _audioService;

		public override AnimationType DefaultAnimationType => _defaultAnimationType;

		public override AnimationType BottomAnimationType => _bottomAnimationType;

		public override float SphereCastRadius => _sphereCastRadius;

		public override Transform SphereCastPosition => _sphereCastPosition;

		public override bool IsWithAiming => _isWithAiming;

		[Inject]
		public void InjectDependencies(IAudioService audioService)
		{
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
			_audioService.ReleaseInstance(_shootSoundInstance);
		}

		public override void EnableWeaponVisual(bool enable)
		{
			_weaponVisual.SetActive(enable);
		}

		public override void Attack(Vector3 position)
		{
			SpawnBullet(position);
		}

		private async UniTask SpawnBullet(Vector3 position)
		{
			Quaternion value = Quaternion.LookRotation((position - _shootPoint.position).normalized);
			await base.Runner.SpawnAsync(_bullet, _shootPoint.position, value);
			PlayParticleRpc();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 347663264u)]
		private void PlayParticleRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(347663264u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.ElPiratoDeTutorial.EnemyPistolWeapon::PlayParticleRpc()", invokeInfo, PlayerRef.None);
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
			_particleSystem.Play();
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

		[NetworkRpcWeavedInvoker(347663264u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayParticleRpc_0040Invoker347663264([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EnemyPistolWeapon)context.TargetBehaviour).PlayParticleRpc();
		}
	}
}
