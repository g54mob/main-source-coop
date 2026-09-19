using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AnimationModule.Scripts;
using Features.GrabModule.Scripts;
using Features.KrakenModule.Scripts.Data;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.KrakenModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class KrakenAppearTentacleController : NetworkBehaviour
	{
		private const string DefaultAppearTrigger = "Appear";

		private const string DefaultDisappearTrigger = "Disappear";

		[SerializeField]
		private GameObject _appearTentacleContainer;

		[SerializeField]
		private GameObject _appearTentacleRoot;

		[SerializeField]
		private NetworkedAnimationController _appearTentacleAnimator;

		[SerializeField]
		private List<SimplePointGrabable> _appearTentacleGrabbables = new List<SimplePointGrabable>();

		[SerializeField]
		private string _appearTrigger = "Appear";

		[SerializeField]
		private string _disappearTrigger = "Disappear";

		[SerializeField]
		private float _disappearDeactivateDelay = 1.5f;

		[SerializeField]
		private KrakenController _krakenController;

		[SerializeField]
		private KrakenTentacleEmotesController _tentacleEmotesController;

		private Coroutine _visibilityAnimationCoroutine;

		private KrakenRuntimeModel _runtimeModel;

		private KrakenStateId _lastRenderedKrakenState = (KrakenStateId)(-1);

		private bool? _lastAppliedShowState;

		private bool? _lastPlayedShowState;

		private bool _isPlayingVisibilityAnimation;

		[Inject]
		private void InjectDependencies(KrakenRuntimeModel runtimeModel)
		{
			_runtimeModel = runtimeModel;
		}

		public void BindController(KrakenController krakenController)
		{
			_krakenController = krakenController;
		}

		public void SyncWithKrakenState(KrakenStateId krakenState)
		{
			if (base.HasStateAuthority)
			{
				ApplyVisibilityForKrakenState(krakenState, playAnimation: true);
			}
		}

		public override void Spawned()
		{
			if (_krakenController == null && _runtimeModel.TryGetController(out var controller))
			{
				BindController(controller);
			}
			ResolveReferencesIfNeeded();
			_runtimeModel.OnControllerChanged += OnKrakenControllerChanged;
			SubscribeGrabbables();
			KrakenStateId krakenState = (_lastRenderedKrakenState = ((_krakenController != null) ? _krakenController.CurrentState : KrakenStateId.Hidden));
			_lastAppliedShowState = null;
			_lastPlayedShowState = null;
			if (base.HasStateAuthority)
			{
				ApplyVisibilityForKrakenState(krakenState, playAnimation: true);
			}
		}

		public void NotifyKrakenBodyHiddenLocally()
		{
			EnsureKrakenControllerBound();
			if (!(_krakenController == null) && ShouldShowAppearTentacle(_krakenController.CurrentState) && _lastPlayedShowState != true)
			{
				ValidateLocalAppearTentacleVisibility(shouldShow: true);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_runtimeModel.OnControllerChanged -= OnKrakenControllerChanged;
			UnsubscribeGrabbables();
			_lastAppliedShowState = null;
			_lastPlayedShowState = null;
			_lastRenderedKrakenState = (KrakenStateId)(-1);
			CancelVisibilityAnimation();
		}

		public override void Render()
		{
			EnsureKrakenControllerBound();
			if (!(_krakenController == null))
			{
				KrakenStateId currentState = _krakenController.CurrentState;
				if (currentState != _lastRenderedKrakenState)
				{
					_lastRenderedKrakenState = currentState;
				}
				if (!base.HasStateAuthority)
				{
					ValidateLocalAppearTentacleVisibility(ShouldShowAppearTentacle(currentState));
				}
			}
		}

		private void OnKrakenControllerChanged(KrakenController controller)
		{
			UnsubscribeGrabbables();
			BindController(controller);
			SubscribeGrabbables();
		}

		private void SubscribeGrabbables()
		{
			foreach (SimplePointGrabable appearTentacleGrabbable in _appearTentacleGrabbables)
			{
				appearTentacleGrabbable.LocalOnGrab += OnAppearTentacleGrabbed;
			}
		}

		private void UnsubscribeGrabbables()
		{
			foreach (SimplePointGrabable appearTentacleGrabbable in _appearTentacleGrabbables)
			{
				appearTentacleGrabbable.LocalOnGrab -= OnAppearTentacleGrabbed;
			}
		}

		private void OnAppearTentacleGrabbed(int _)
		{
			if (!(_krakenController == null) && !_krakenController.IsVisible)
			{
				if (base.HasStateAuthority)
				{
					_krakenController.NotifyInteractableTentacleActivated();
				}
				else
				{
					RequestKrakenAppearRpc();
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 2733500745u)]
		private void RequestKrakenAppearRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2733500745u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.KrakenModule.Scripts.KrakenAppearTentacleController::RequestKrakenAppearRpc()", invokeInfo, PlayerRef.None);
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
			if (!(_krakenController == null) && !_krakenController.IsVisible)
			{
				_krakenController.NotifyInteractableTentacleActivated();
			}
		}

		private void ApplyVisibilityForKrakenState(KrakenStateId krakenState, bool playAnimation)
		{
			bool flag = ShouldShowAppearTentacle(krakenState);
			if (!_lastAppliedShowState.HasValue || _lastAppliedShowState.Value != flag)
			{
				_lastAppliedShowState = flag;
				ApplyAppearTentacleVisibilityRpc(flag, playAnimation);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3952728040u)]
		private void ApplyAppearTentacleVisibilityRpc([RpcPayload(4)] bool show, [RpcPayload(4)] bool playAnimation)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(show);
				payloadSize += Fusion.RpcDataWriter.GetPayloadSize(playAnimation);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3952728040u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.KrakenModule.Scripts.KrakenAppearTentacleController::ApplyAppearTentacleVisibilityRpc(System.Boolean,System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(show);
						writer.Write(playAnimation);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			ApplyLocalShowState(show, playAnimation);
		}

		private void ApplyLocalShowState(bool show, bool playAnimation)
		{
			if (!_lastPlayedShowState.HasValue || _lastPlayedShowState.Value != show || _isPlayingVisibilityAnimation)
			{
				CancelVisibilityAnimation();
				if (!playAnimation)
				{
					_lastPlayedShowState = show;
					return;
				}
				_lastPlayedShowState = show;
				_visibilityAnimationCoroutine = StartCoroutine(PlayShowStateAnimationCoroutine(show));
			}
		}

		private IEnumerator PlayShowStateAnimationCoroutine(bool show)
		{
			_isPlayingVisibilityAnimation = true;
			if (show)
			{
				PlayAppearTentacleTrigger();
			}
			else
			{
				PlayDisappearTentacleTrigger();
			}
			yield return new WaitForSeconds(Mathf.Max(0.01f, _disappearDeactivateDelay));
			_isPlayingVisibilityAnimation = false;
			_visibilityAnimationCoroutine = null;
		}

		private void EnsureKrakenControllerBound()
		{
			if (!(_krakenController != null))
			{
				_runtimeModel.TryGetController(out _krakenController);
			}
		}

		private void ResolveReferencesIfNeeded()
		{
			if (_appearTentacleContainer == null && _appearTentacleRoot != null)
			{
				_appearTentacleContainer = _appearTentacleRoot.transform.parent?.gameObject;
			}
		}

		private bool IsKrakenBodyStillVisibleLocally()
		{
			if (_tentacleEmotesController != null)
			{
				return _tentacleEmotesController.IsLocalKrakenBodyVisible;
			}
			return false;
		}

		private void ValidateLocalAppearTentacleVisibility(bool shouldShow)
		{
			if (!_isPlayingVisibilityAnimation && (!shouldShow || !IsKrakenBodyStillVisibleLocally()) && (!_lastPlayedShowState.HasValue || _lastPlayedShowState.Value != shouldShow))
			{
				ApplyLocalShowState(shouldShow, playAnimation: true);
			}
		}

		private void CancelVisibilityAnimation()
		{
			if (_visibilityAnimationCoroutine != null)
			{
				StopCoroutine(_visibilityAnimationCoroutine);
				_visibilityAnimationCoroutine = null;
				_isPlayingVisibilityAnimation = false;
			}
		}

		private void PlayAppearTentacleTrigger()
		{
			if (!(_appearTentacleAnimator == null))
			{
				if (base.HasStateAuthority)
				{
					_appearTentacleAnimator.SwapTrigger(_disappearTrigger, _appearTrigger);
				}
				else
				{
					_appearTentacleAnimator.SwapTriggerLocal(_disappearTrigger, _appearTrigger);
				}
			}
		}

		private void PlayDisappearTentacleTrigger()
		{
			if (!(_appearTentacleAnimator == null))
			{
				if (base.HasStateAuthority)
				{
					_appearTentacleAnimator.SwapTrigger(_appearTrigger, _disappearTrigger);
				}
				else
				{
					_appearTentacleAnimator.SwapTriggerLocal(_appearTrigger, _disappearTrigger);
				}
			}
		}

		private static bool ShouldShowAppearTentacle(KrakenStateId state)
		{
			if (state != KrakenStateId.Hidden)
			{
				return state == KrakenStateId.Disappearing;
			}
			return true;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(2733500745u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RequestKrakenAppearRpc_0040Invoker2733500745([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((KrakenAppearTentacleController)context.TargetBehaviour).RequestKrakenAppearRpc();
		}

		[NetworkRpcWeavedInvoker(3952728040u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ApplyAppearTentacleVisibilityRpc_0040Invoker3952728040([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out bool value);
			payloadReader.Read(out bool value2);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((KrakenAppearTentacleController)context.TargetBehaviour).ApplyAppearTentacleVisibilityRpc(value, value2);
		}
	}
}
