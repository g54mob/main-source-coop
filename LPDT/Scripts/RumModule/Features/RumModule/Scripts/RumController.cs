using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.CollectingModule.Scripts;
using Features.CoroutineUtils.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.RumModule.Scripts
{
	[NetworkBehaviourWeaved(2)]
	public class RumController : NetworkBehaviour, IItemCollisionBreakGate
	{
		[SerializeField]
		private RumVisualController _rumVisualController;

		[SerializeField]
		private RumCollisionHandler _rumCollisionHandler;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabbable;

		[SerializeField]
		private RumType _rumType;

		[SerializeField]
		private RumBottleInteractable _rumBottleInteractable;

		[SerializeField]
		private float _angleThreshold = 70f;

		[SerializeField]
		private float _timeToDrink = 3f;

		[SerializeField]
		private RumRewardBase _rumReward;

		[SerializeField]
		private MonoItem _monoItem;

		[SerializeField]
		private float _relativeVelocityToHit = 5f;

		[SerializeField]
		private float _hitDamage = 10f;

		[SerializeField]
		private EventReference _drinkingEventReference;

		[SerializeField]
		private EventReference _stopDrinkingEventReference;

		[SerializeField]
		private EventReference _playerHitEventReference;

		[SerializeField]
		private EventReference _anotherBottleHitEventReference;

		[SerializeField]
		private EventReference _hitEventReference;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private float _drinkingSoundFadeTime = 1f;

		[SerializeField]
		private float _clickingAnotherBottleVelocityThreshold = 1f;

		[SerializeField]
		private float _bottleHitSoundCooldown = 1f;

		[SerializeField]
		private float _hitSoundRelativeVelocityThreshold = 5f;

		private float _bottleHitCooldownTimer;

		private bool _isInitialized;

		private EventInstance _drinkInstance;

		private bool _isSoundPlaying;

		private bool _isRewardApplied;

		private ICoroutineRunner _coroutineRunner;

		private IAudioService _audioService;

		[WeaverGenerated]
		[DefaultForProperty("IsUsed", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsUsed;

		[WeaverGenerated]
		[DefaultForProperty("Timer", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _Timer;

		public SimplePointGrabable SimplePointGrabable => _simplePointGrabbable;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe bool IsUsed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RumController.IsUsed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RumController.IsUsed. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe float Timer
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RumController.Timer. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RumController.Timer. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)(Ptr + 1) = value;
			}
		}

		public bool CanBreakFromCollision => IsUsed;

		public bool IsBottleUsed => IsUsed;

		public bool IsRewardApplied
		{
			get
			{
				return _isRewardApplied;
			}
			set
			{
				_isRewardApplied = value;
				this.OnIsRewardAppliedChanged?.Invoke(value);
			}
		}

		public RumBottleInteractable RumBottleInteractable => _rumBottleInteractable;

		public event Action<bool> OnIsRewardAppliedChanged;

		[Inject]
		private void InjectDependencies(ICoroutineRunner coroutineRunner, IAudioService audioService)
		{
			_coroutineRunner = coroutineRunner;
			_audioService = audioService;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			IsRewardApplied = false;
			_audioService.StopInstance(_drinkInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			_audioService.ReleaseInstance(_drinkInstance);
		}

		public void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			_drinkInstance = _audioService.CreateInstance(_drinkingEventReference);
			_rumCollisionHandler.OnPlayerHited += HandleHit;
			_rumCollisionHandler.OnEnemyHited += HandleEnemyHit;
			_rumCollisionHandler.OnCollectableHited += HandleCollectableHit;
			_rumCollisionHandler.OnHited += HandleHitSound;
		}

		public void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			_rumCollisionHandler.OnPlayerHited -= HandleHit;
			_rumCollisionHandler.OnEnemyHited -= HandleEnemyHit;
			_rumCollisionHandler.OnCollectableHited -= HandleCollectableHit;
			_rumCollisionHandler.OnHited -= HandleHitSound;
		}

		private void HandleHitSound(RumHitData rumHitData)
		{
			if (base.HasStateAuthority && _isInitialized && !(rumHitData.RelativeVelocity.magnitude < _hitSoundRelativeVelocityThreshold) && !IsUsed)
			{
				PlayHitSoundRpc();
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 4207871332u)]
		private void PlayHitSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4207871332u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RumModule.Scripts.RumController::PlayHitSoundRpc()", invokeInfo, PlayerRef.None);
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
			_audioService.PlayOneShotAttached(_hitEventReference, _soundSourceBehaviour);
		}

		private void HandleCollectableHit(RumHitData rumHitData)
		{
			if (base.HasStateAuthority && !(_bottleHitCooldownTimer < _bottleHitSoundCooldown) && rumHitData.GameObject.TryGetComponent<RumController>(out var component) && _simplePointGrabbable.GrabbedByPlayers.Count > 0 && component.SimplePointGrabable.GrabbedByPlayers.Count > 0)
			{
				_bottleHitCooldownTimer = 0f;
				PlayAnotherBottleHitSoundRpc();
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1802471677u)]
		private void PlayAnotherBottleHitSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1802471677u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RumModule.Scripts.RumController::PlayAnotherBottleHitSoundRpc()", invokeInfo, PlayerRef.None);
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
			_audioService.PlayOneShotAttached(_anotherBottleHitEventReference, _soundSourceBehaviour);
		}

		private void HandleEnemyHit(RumHitData rumHitData)
		{
			if (base.HasStateAuthority && !(rumHitData.RelativeVelocity.magnitude < _relativeVelocityToHit))
			{
				rumHitData.GameObject.GetComponent<IDamageable>().DamageRPC(_hitDamage, base.Object.StateAuthority.PlayerId, DamageDataSourceExtensions.ToRpc(DamageDataSourceExtensions.ForPlayerAttack(DamageType.Explosion)));
			}
		}

		private void HandleHit(RumHitData rumHitData)
		{
			if (_isInitialized && base.HasStateAuthority && !IsUsed && !(rumHitData.RelativeVelocity.magnitude < _relativeVelocityToHit))
			{
				int playerId = rumHitData.GameObject.GetComponent<NetworkObject>().InputAuthority.PlayerId;
				rumHitData.GameObject.GetComponent<IDamageable>().DamageRPC(_hitDamage, base.Object.StateAuthority.PlayerId, DamageDataSourceExtensions.ToRpc(DamageDataSourceExtensions.ForPlayerAttack(DamageType.Explosion)));
				IsUsed = true;
				ApplyRewardRpc(playerId);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3600663838u)]
		private void ApplyRewardRpc([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3600663838u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RumModule.Scripts.RumController::ApplyRewardRpc(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_audioService.PlayOneShotAttached(_playerHitEventReference, _soundSourceBehaviour);
			ApplyRewardForLocalPlayer(playerId);
			if (base.Runner.LocalPlayer.PlayerId == playerId)
			{
				RequestDespawnRpc();
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 260814200u)]
		private void ApplyDrinkRewardRpc([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(260814200u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RumModule.Scripts.RumController::ApplyDrinkRewardRpc(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			ApplyRewardForLocalPlayer(playerId);
		}

		private void ApplyRewardForLocalPlayer(int playerId)
		{
			if (base.Runner.LocalPlayer.PlayerId == playerId)
			{
				Debug.Log($"[Drunk] ApplyRewardForLocalPlayer rumType={_rumType} playerId={playerId}");
				_rumReward?.ApplyReward(_rumType);
				IsRewardApplied = true;
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 2145060247u)]
		private void RequestDespawnRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2145060247u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RumModule.Scripts.RumController::RequestDespawnRpc()", invokeInfo, PlayerRef.None);
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
			base.Object.DespawnHierarchy();
		}

		public override void Spawned()
		{
			_simplePointGrabbable.IsReadyToQuota = false;
			_simplePointGrabbable.AddIgnoreItemsCollisionRequest(base.gameObject.GetHashCode());
			_monoItem.IsNeedToShowPrice = false;
			_isInitialized = true;
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2651063397u)]
		private void PlayStopDrinkingSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2651063397u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RumModule.Scripts.RumController::PlayStopDrinkingSoundRpc()", invokeInfo, PlayerRef.None);
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
			_audioService.PlayOneShotAttached(_stopDrinkingEventReference, _soundSourceBehaviour);
		}

		public void Update()
		{
			_bottleHitCooldownTimer += Time.deltaTime;
			if (_bottleHitCooldownTimer >= _bottleHitSoundCooldown)
			{
				_bottleHitCooldownTimer = _bottleHitSoundCooldown;
			}
			if (_drinkInstance.isValid())
			{
				_drinkInstance.set3DAttributes(_soundSourceBehaviour.SoundSourceTransform.To3DAttributes());
			}
			if (!_isInitialized)
			{
				return;
			}
			HandleDrink();
			if (Timer > 0f)
			{
				if (!_isSoundPlaying)
				{
					_isSoundPlaying = true;
					_audioService.StartInstanceWith3DAttributes(_drinkInstance, _soundSourceBehaviour);
				}
			}
			else
			{
				_isSoundPlaying = false;
				_coroutineRunner.StartCoroutine(DrinkSoundFade());
			}
			if (IsUsed)
			{
				_simplePointGrabbable.IsReadyToQuota = true;
				_simplePointGrabbable.RemoveIgnoreItemsCollisionRequest(base.gameObject.GetHashCode());
				_monoItem.IsNeedToShowPrice = true;
				_rumVisualController.SetProgress(1f);
				_rumVisualController.DisableRenderer(disable: false);
				if (_isSoundPlaying)
				{
					_isSoundPlaying = false;
					_coroutineRunner.StartCoroutine(DrinkSoundFade());
				}
			}
			else
			{
				_rumVisualController.SetProgress(Timer / _timeToDrink);
			}
		}

		private void HandleDrink()
		{
			if (!_isInitialized || !base.HasStateAuthority || IsUsed)
			{
				return;
			}
			if (_simplePointGrabbable.GrabbedByPlayers.Count > 1)
			{
				_rumBottleInteractable.OnInteractEnd();
			}
			if (!_rumBottleInteractable.IsInInteraction)
			{
				Timer = 0f;
				return;
			}
			if (Vector3.Angle(base.transform.up, Vector3.down) < _angleThreshold)
			{
				Timer += Time.deltaTime;
			}
			else
			{
				Timer = 0f;
			}
			if (Timer >= _timeToDrink)
			{
				PlayStopDrinkingSoundRpc();
				Timer = 0f;
				_rumBottleInteractable.Consume();
				int num = ResolveDrinkerPlayerId();
				Debug.Log($"[Drunk] drink complete rumType={_rumType} drinkerId={num}");
				ApplyDrinkRewardRpc(num);
				IsUsed = true;
			}
		}

		private int ResolveDrinkerPlayerId()
		{
			if (_simplePointGrabbable != null && _simplePointGrabbable.GrabbedByPlayers.Count > 0)
			{
				return _simplePointGrabbable.GrabbedByPlayers[0];
			}
			if (base.Object.InputAuthority.IsRealPlayer)
			{
				return base.Object.InputAuthority.PlayerId;
			}
			return base.Runner.LocalPlayer.PlayerId;
		}

		private IEnumerator DrinkSoundFade()
		{
			EventInstance eventInstance = _drinkInstance;
			_drinkInstance = _audioService.CreateInstance(_drinkingEventReference);
			float timer = 0f;
			float time = _drinkingSoundFadeTime;
			while (timer < time)
			{
				timer += Time.deltaTime;
				eventInstance.setVolume(Mathf.Lerp(1f, 0f, timer / time));
				yield return null;
			}
			_audioService.StopInstance(eventInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			_audioService.ReleaseInstance(eventInstance);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsUsed = _IsUsed;
			Timer = _Timer;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsUsed = IsUsed;
			_Timer = Timer;
		}

		[NetworkRpcWeavedInvoker(4207871332u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayHitSoundRpc_0040Invoker4207871332([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RumController)context.TargetBehaviour).PlayHitSoundRpc();
		}

		[NetworkRpcWeavedInvoker(1802471677u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayAnotherBottleHitSoundRpc_0040Invoker1802471677([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RumController)context.TargetBehaviour).PlayAnotherBottleHitSoundRpc();
		}

		[NetworkRpcWeavedInvoker(3600663838u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ApplyRewardRpc_0040Invoker3600663838([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RumController)context.TargetBehaviour).ApplyRewardRpc(value);
		}

		[NetworkRpcWeavedInvoker(260814200u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ApplyDrinkRewardRpc_0040Invoker260814200([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RumController)context.TargetBehaviour).ApplyDrinkRewardRpc(value);
		}

		[NetworkRpcWeavedInvoker(2145060247u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RequestDespawnRpc_0040Invoker2145060247([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RumController)context.TargetBehaviour).RequestDespawnRpc();
		}

		[NetworkRpcWeavedInvoker(2651063397u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayStopDrinkingSoundRpc_0040Invoker2651063397([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RumController)context.TargetBehaviour).PlayStopDrinkingSoundRpc();
		}
	}
}
