using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.CameraModelModule;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.EmotesModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class EmotesController : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface
	{
		private const int BLEND_SHAPE_WEIGHT_SHIFT = 1;

		private const float VELOCITY_THRESHOLD = 0.1f;

		private const string EMOTES_LAYER = "EmotesLayer";

		private const string EMOTE_TAG = "Emote";

		private static readonly int _emotingInterruptionHash = Animator.StringToHash("EmotingInterruption");

		private static readonly int _emotesLayerDefaultStateHash = Animator.StringToHash("Blend Tree");

		[SerializeField]
		private NetworkedCompositeAnimator _networkedCompositeAnimator;

		[SerializeField]
		private List<SkinnedMeshRenderer> _skinnedMeshRenderer;

		private EmotesConfiguration _emotesConfiguration;

		private PlayerMovableModel _playerMovableModel;

		private EmotesTriggerModel _emotesTriggerModel;

		private IPlayerStateService _playerStateService;

		private CameraModel _cameraModel;

		private MultiplayerModel _multiplayerModel;

		private PlayersRagdollModel _playersRagdollModel;

		private PlayerBodyEmoteNetworkEvent _playerBodyEmoteNetworkEvent;

		private Coroutine _faceEmoteCoroutine;

		private bool _isEmoting;

		private bool _isFaceEmoting;

		private bool _isEmotingInterrupted;

		private bool _checkForEmoteEnd;

		private int _currentEmoteHash;

		private BodyEmoteType _currentBodyEmoteType;

		private FaceEmoteType? _lastFaceEmoteIndex;

		private bool _isEmotingVisualActive;

		[Inject]
		public void InjectDependencies(EmotesConfiguration emotesConfiguration, PlayerMovableModel playerMovableModel, EmotesTriggerModel emotesTriggerModel, IPlayerStateService playerStateService, CameraModel cameraModel, MultiplayerModel multiplayerModel, PlayersRagdollModel playersRagdollModel, PlayerBodyEmoteNetworkEvent playerBodyEmoteNetworkEvent)
		{
			_emotesConfiguration = emotesConfiguration;
			_playerMovableModel = playerMovableModel;
			_emotesTriggerModel = emotesTriggerModel;
			_playerStateService = playerStateService;
			_cameraModel = cameraModel;
			_multiplayerModel = multiplayerModel;
			_playersRagdollModel = playersRagdollModel;
			_playerBodyEmoteNetworkEvent = playerBodyEmoteNetworkEvent;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.HasStateAuthority)
			{
				_emotesTriggerModel.OnBodyEmoteTriggered += StartEmoting;
				_emotesTriggerModel.OnFaceEmoteTriggered += SetFaceEmoting;
				_emotesTriggerModel.OnBodyEmoteInterruptRequested += InterruptBodyEmote;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (base.HasStateAuthority)
			{
				_emotesTriggerModel.OnBodyEmoteTriggered -= StartEmoting;
				_emotesTriggerModel.OnFaceEmoteTriggered -= SetFaceEmoting;
				_emotesTriggerModel.OnBodyEmoteInterruptRequested -= InterruptBodyEmote;
				StopBodyEmoteNetworkEvent();
				ExitEmotingLocal();
			}
		}

		private void Update()
		{
			if (!base.HasStateAuthority || (!_isEmoting && !_isFaceEmoting))
			{
				return;
			}
			if (ShouldInterruptEmoting())
			{
				if (_isFaceEmoting)
				{
					ExitFaceEmotingLocal();
				}
				if (_isEmoting && !_isEmotingInterrupted)
				{
					InterruptEmoting(IsLocalRagdollSimulated());
					return;
				}
			}
			CheckEmoteEnd();
		}

		public void StateAuthorityChanged()
		{
			if (!base.HasStateAuthority)
			{
				_isEmoting = false;
				_checkForEmoteEnd = false;
				_isEmotingInterrupted = false;
				ExitEmotingLocal();
			}
			else if (IsAnimatorEmoting())
			{
				ForceEmoteLayerToDefault();
				StopBodyEmoteNetworkEvent();
				_isEmoting = true;
				_isEmotingInterrupted = true;
				_checkForEmoteEnd = true;
				ExitEmotingLocal();
			}
		}

		private bool IsAnimatorEmoting()
		{
			AnimatorStateInfo currentAnimatorStateInfo = _networkedCompositeAnimator.CompositeAnimator.GetCurrentAnimatorStateInfo("EmotesLayer");
			AnimatorStateInfo nextAnimatorStateInfo = _networkedCompositeAnimator.CompositeAnimator.GetNextAnimatorStateInfo("EmotesLayer");
			if (!currentAnimatorStateInfo.IsTag("Emote"))
			{
				return nextAnimatorStateInfo.IsTag("Emote");
			}
			return true;
		}

		private void CheckEmoteEnd()
		{
			if (_checkForEmoteEnd && !IsAnimatorEmoting())
			{
				StopBodyEmoteNetworkEvent();
				_isEmoting = false;
				_checkForEmoteEnd = false;
				_isEmotingInterrupted = false;
				if (_currentEmoteHash != 0)
				{
					_networkedCompositeAnimator.CompositeAnimator.ResetTrigger(_currentEmoteHash);
				}
				_networkedCompositeAnimator.CompositeAnimator.ResetTrigger(_emotingInterruptionHash);
				if (!_isFaceEmoting)
				{
					ExitEmotingLocal();
				}
			}
		}

		private void InterruptBodyEmote()
		{
			if (_isEmoting && !_isEmotingInterrupted)
			{
				InterruptEmoting(forceToDefault: true);
			}
		}

		private void InterruptEmoting(bool forceToDefault)
		{
			if (forceToDefault)
			{
				ForceEmoteLayerToDefault();
			}
			else
			{
				_networkedCompositeAnimator.SetTrigger(_emotingInterruptionHash);
			}
			StopBodyEmoteNetworkEvent();
			_isEmotingInterrupted = true;
			_isFaceEmoting = false;
			ExitEmotingLocal();
		}

		private void ForceEmoteLayerToDefault()
		{
			int layerIndex = _networkedCompositeAnimator.CompositeAnimator.GetLayerIndex("EmotesLayer");
			_networkedCompositeAnimator.Play(_emotesLayerDefaultStateHash, layerIndex, 0f);
		}

		private void StartEmoting(BodyEmoteType bodyEmoteType)
		{
			if (!(base.Object == null) && !(_playerMovableModel.CurrentMovementInput != Vector3.zero) && _playerMovableModel.LocalMovable.MovementState != MovementState.Crouching && !IsLocalRagdollSimulated())
			{
				if (_isEmoting)
				{
					_checkForEmoteEnd = false;
				}
				string text = _emotesConfiguration.EmotesPool[bodyEmoteType];
				_currentEmoteHash = Animator.StringToHash(text);
				_networkedCompositeAnimator.SetTrigger(_currentEmoteHash);
				_isEmoting = true;
				_currentBodyEmoteType = bodyEmoteType;
				_isEmotingInterrupted = false;
				StartCoroutine(EnableCheckForEmoteEnd());
				EnterEmotingLocal();
				_playerBodyEmoteNetworkEvent.SendEvent(base.Object.InputAuthority.PlayerId, bodyEmoteType, isStarted: true);
			}
		}

		private void StopBodyEmoteNetworkEvent()
		{
			if (_isEmoting && _currentBodyEmoteType != BodyEmoteType.None && !(base.Object == null))
			{
				_playerBodyEmoteNetworkEvent.SendEvent(base.Object.InputAuthority.PlayerId, _currentBodyEmoteType, isStarted: false);
				_currentBodyEmoteType = BodyEmoteType.None;
			}
		}

		private IEnumerator EnableCheckForEmoteEnd()
		{
			yield return null;
			_checkForEmoteEnd = true;
		}

		private void SetFaceEmoting(FaceEmoteType faceEmoteType, float percent)
		{
			if (!(base.Object == null))
			{
				if (faceEmoteType != _lastFaceEmoteIndex)
				{
					SetFaceEmotingRPC(faceEmoteType, percent);
				}
				if (_playerStateService.IsPlayerAlive(base.Object.InputAuthority.PlayerId) && !IsLocalRagdollSimulated())
				{
					_isFaceEmoting = true;
					EnterEmotingLocal();
				}
			}
		}

		private bool ShouldInterruptEmoting()
		{
			if (!IsLocalRagdollSimulated() && !(_playerMovableModel.LocalMovable.GetVelocity().magnitude >= 0.1f))
			{
				return _playerMovableModel.LocalMovable.MovementState == MovementState.Crouching;
			}
			return true;
		}

		private void ExitFaceEmotingLocal()
		{
			_isFaceEmoting = false;
			if (!_isEmoting || _isEmotingInterrupted)
			{
				ExitEmotingLocal();
			}
		}

		private bool IsLocalRagdollSimulated()
		{
			if (_playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				return ragdoll.IsSimulated;
			}
			return false;
		}

		private void EnterEmotingLocal()
		{
			if (!(base.Object == null) && !(base.Object.InputAuthority != _multiplayerModel.NetworkRunner.LocalPlayer) && !_isEmotingVisualActive)
			{
				_isEmotingVisualActive = true;
				_emotesTriggerModel.TriggerEmotingStarted();
				_cameraModel.AddPerlinDisableReason(PerlinDisableReasonEnum.Emoting);
				_cameraModel.AddCameraReason(Features.CameraModelModule.CameraType.TPCamera, CameraReasonEnum.Emoting);
				if (_cameraModel.Cameras.TryGetValue(Features.CameraModelModule.CameraType.TPCamera, out var value))
				{
					value.SetVerticalLookEnabled(isEnabled: false);
				}
				_playerMovableModel.IsLookingAtCamera = true;
				_playerMovableModel.AddOnlyHeadRotationReason(OnlyHeadRotationReasonEnum.Emoting);
			}
		}

		private void ExitEmotingLocal()
		{
			if (!(base.Object == null) && !(base.Object.InputAuthority != _multiplayerModel.NetworkRunner.LocalPlayer) && _isEmotingVisualActive)
			{
				_isEmotingVisualActive = false;
				_emotesTriggerModel.TriggerEmotingStopped();
				_cameraModel.RemoveCameraReason(Features.CameraModelModule.CameraType.TPCamera, CameraReasonEnum.Emoting);
				_playerMovableModel.RemoveOnlyHeadRotationReason(OnlyHeadRotationReasonEnum.Emoting);
				_playerMovableModel.IsLookingAtCamera = false;
				if (_cameraModel.Cameras.TryGetValue(Features.CameraModelModule.CameraType.TPCamera, out var value))
				{
					value.SetVerticalLookEnabled(isEnabled: true);
				}
				_cameraModel.RemovePerlinDisableReason(PerlinDisableReasonEnum.Emoting);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 941717578u)]
		private void SetFaceEmotingRPC([RpcPayload(4)] FaceEmoteType faceEmoteType, [RpcPayload(4)] float percent)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(941717578u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.EmotesModule.Scripts.EmotesController::SetFaceEmotingRPC(Features.EmotesModule.Scripts.FaceEmoteType,System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(faceEmoteType, 4);
						writer.Write(percent, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_faceEmoteCoroutine == null)
			{
				_faceEmoteCoroutine = StartCoroutine(LerpFaceEmote(_lastFaceEmoteIndex, faceEmoteType, percent));
			}
		}

		private IEnumerator LerpFaceEmote(FaceEmoteType? previousIndex, FaceEmoteType faceEmoteType, float percent)
		{
			float duration = 0.2f;
			float elapsed = 0f;
			float startPreviousWeight = (previousIndex.HasValue ? _skinnedMeshRenderer[0].GetBlendShapeWeight((int)previousIndex.Value) : 0f);
			float startNextWeight = _skinnedMeshRenderer[0].GetBlendShapeWeight((int)faceEmoteType);
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float t = Mathf.Clamp01(elapsed / duration);
				foreach (SkinnedMeshRenderer item in _skinnedMeshRenderer)
				{
					if (previousIndex.HasValue)
					{
						item.SetBlendShapeWeight((int)(previousIndex.Value - 1), Mathf.Lerp(startPreviousWeight, 0f, t));
					}
					item.SetBlendShapeWeight((int)(faceEmoteType - 1), Mathf.Lerp(startNextWeight, percent, t));
				}
				yield return null;
			}
			foreach (SkinnedMeshRenderer item2 in _skinnedMeshRenderer)
			{
				if (previousIndex.HasValue)
				{
					item2.SetBlendShapeWeight((int)(previousIndex.Value - 1), 0f);
				}
				item2.SetBlendShapeWeight((int)(faceEmoteType - 1), percent);
			}
			_lastFaceEmoteIndex = faceEmoteType;
			_faceEmoteCoroutine = null;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(941717578u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetFaceEmotingRPC_0040Invoker941717578([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out FaceEmoteType value, 4);
			payloadReader.Read(out float value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((EmotesController)context.TargetBehaviour).SetFaceEmotingRPC(value, value2);
		}
	}
}
