using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.CartUpgradesModule.Scripts.Core;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.InteractModule.Scripts;
using Features.ScreenShakeModule.Scripts;
using Features.WeaponModule.Scripts;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.CartCannonModule.Scripts
{
	[NetworkBehaviourWeaved(2)]
	public class CartCannonWeapon : InteractableBase
	{
		[SerializeField]
		private CartCannonMount _mount;

		[SerializeField]
		private CartGrabObject _cartGrabObject;

		[SerializeField]
		private SimplePointGrabable _cartGrabable;

		[SerializeField]
		private ProjectileBehaviourBase _bullet;

		[SerializeField]
		private Transform _shootPoint;

		[SerializeField]
		private float _cooldownSeconds = 1.5f;

		[SerializeField]
		private CartGrabObject.FacingDirection _requiredFacingDirection;

		[Header("Feedback")]
		[SerializeField]
		private ParticleSystem _shootParticle;

		[SerializeField]
		private EventReference _shootSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private CinemachineImpulseSource _cinemachineImpulseSource;

		[SerializeField]
		private ScreenShakeData _screenShakeDataOnShoot;

		[Header("Depletion")]
		[SerializeField]
		private GameObject _cannonVisual;

		[SerializeField]
		private ParticleSystem _depletionParticle;

		[SerializeField]
		private EventReference _depletionSound;

		[WeaverGenerated]
		[DefaultForProperty("RemainingShots", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _RemainingShots;

		[WeaverGenerated]
		[DefaultForProperty("CooldownTimer", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private TickTimer _CooldownTimer;

		private CartUpgradeConfiguration _cartUpgradeConfiguration;

		private ICartUpgradeService _cartUpgradeService;

		private IScreenShakeService _screenShakeService;

		private IAudioService _audioService;

		private bool _isPendingInteraction;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe int RemainingShots
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CartCannonWeapon.RemainingShots. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CartCannonWeapon.RemainingShots. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe TickTimer CooldownTimer
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CartCannonWeapon.CooldownTimer. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(TickTimer*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CartCannonWeapon.CooldownTimer. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(TickTimer*)(Ptr + 1) = value;
			}
		}

		public int Shots => RemainingShots;

		[Inject]
		public void InjectDependencies(CartUpgradeConfiguration cartUpgradeConfiguration, ICartUpgradeService cartUpgradeService, IScreenShakeService screenShakeService, IAudioService audioService)
		{
			_cartUpgradeConfiguration = cartUpgradeConfiguration;
			_cartUpgradeService = cartUpgradeService;
			_screenShakeService = screenShakeService;
			_audioService = audioService;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.HasStateAuthority)
			{
				RemainingShots = _cartUpgradeConfiguration.CannonMaxShots;
			}
			ApplyDepletedState(RemainingShots <= 0);
		}

		public bool CanShootLocally()
		{
			if (base.Runner == null || !IsInteractable)
			{
				return false;
			}
			if (_cartUpgradeService == null || !_cartUpgradeService.HasModule(CartUpgradeModule.Cannon))
			{
				return false;
			}
			if (_mount != null && _mount.IsLowered)
			{
				return false;
			}
			if (RemainingShots <= 0)
			{
				return false;
			}
			if (CooldownTimer.IsRunning && !CooldownTimer.Expired(base.Runner))
			{
				return false;
			}
			return IsLocalPlayerOnRequiredHandle();
		}

		public override void Interact()
		{
			if (CanShootLocally())
			{
				if (base.Object.HasStateAuthority)
				{
					Fire();
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
				if (CanShootLocally())
				{
					Fire();
				}
			}
		}

		private bool IsLocalPlayerOnRequiredHandle()
		{
			if (_cartGrabObject == null || _cartGrabable == null)
			{
				return false;
			}
			int playerId = base.Runner.LocalPlayer.PlayerId;
			if (!_cartGrabable.GrabbedByPlayers.Contains(playerId))
			{
				return false;
			}
			if (!_cartGrabObject.TryGetGrabberOfPlayer(playerId, out var grabber))
			{
				return false;
			}
			if (_cartGrabObject.TryGetHeldDirection(grabber, out var direction))
			{
				return direction == _requiredFacingDirection;
			}
			return false;
		}

		private void Fire()
		{
			RemainingShots = Mathf.Max(0, RemainingShots - 1);
			CooldownTimer = TickTimer.CreateFromSeconds(base.Runner, _cooldownSeconds);
			SpawnBullet(RemainingShots <= 0).Forget();
		}

		private async UniTaskVoid SpawnBullet(bool isLastShot)
		{
			await base.Runner.SpawnAsync(_bullet, _shootPoint.position, _shootPoint.rotation);
			if (!(this == null))
			{
				PlayShotRpc();
				if (isLastShot)
				{
					PlayDepletionRpc();
				}
			}
		}

		private void ApplyDepletedState(bool isDepleted)
		{
			if (_cannonVisual != null)
			{
				_cannonVisual.SetActive(!isDepleted);
			}
		}

		private void PlayDepletionFeedback()
		{
			if (_depletionParticle != null && _cannonVisual != null)
			{
				ParticleSystem particleSystem = UnityEngine.Object.Instantiate(_depletionParticle, _cannonVisual.transform.position, Quaternion.identity);
				ParticleSystem.MainModule main = particleSystem.main;
				main.stopAction = ParticleSystemStopAction.Destroy;
				particleSystem.Play();
			}
			if (!_depletionSound.IsNull && _soundSourceBehaviour != null)
			{
				_audioService.PlayOneShotAttached(_depletionSound, _soundSourceBehaviour);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 323320430u)]
		private void PlayShotRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(323320430u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CartCannonModule.Scripts.CartCannonWeapon::PlayShotRpc()", invokeInfo, PlayerRef.None);
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
				_screenShakeService.TriggerScreenShake(_cinemachineImpulseSource, _screenShakeDataOnShoot);
			}
			if (_shootParticle != null)
			{
				_shootParticle.Play();
			}
			if (!_shootSound.IsNull && _soundSourceBehaviour != null)
			{
				_audioService.PlayOneShotAttached(_shootSound, _soundSourceBehaviour);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2242143170u)]
		private void PlayDepletionRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2242143170u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CartCannonModule.Scripts.CartCannonWeapon::PlayDepletionRpc()", invokeInfo, PlayerRef.None);
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
			if (!(_cannonVisual != null) || _cannonVisual.activeSelf)
			{
				ApplyDepletedState(isDepleted: true);
				PlayDepletionFeedback();
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			RemainingShots = _RemainingShots;
			CooldownTimer = _CooldownTimer;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_RemainingShots = RemainingShots;
			_CooldownTimer = CooldownTimer;
		}

		[NetworkRpcWeavedInvoker(323320430u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayShotRpc_0040Invoker323320430([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CartCannonWeapon)context.TargetBehaviour).PlayShotRpc();
		}

		[NetworkRpcWeavedInvoker(2242143170u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayDepletionRpc_0040Invoker2242143170([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CartCannonWeapon)context.TargetBehaviour).PlayDepletionRpc();
		}
	}
}
