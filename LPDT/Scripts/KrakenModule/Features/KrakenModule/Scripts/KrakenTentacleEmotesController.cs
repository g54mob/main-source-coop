using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.KrakenModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class KrakenTentacleEmotesController : NetworkBehaviour
	{
		[SerializeField]
		private Animator _mainAnimator;

		[SerializeField]
		private Animator[] _animators;

		[SerializeField]
		private string _appearTriggerName = "Appear";

		[SerializeField]
		private string _disappearTriggerName = "Disappear";

		[SerializeField]
		private string _appearAnimationStateName = "A_Kraken_Appear";

		[SerializeField]
		private string _disappearAnimationStateName = "A_Kraken_Disappear";

		[SerializeField]
		private float _visibilityAnimationTimeout = 1.5f;

		[SerializeField]
		private KrakenController _krakenController;

		[SerializeField]
		private KrakenAppearTentacleController _appearTentacleController;

		private bool _localKrakenBodyVisible = true;

		private bool _isCorrectingVisibility;

		private Coroutine _visibilityCorrectionCoroutine;

		private bool _checkForEmotesEnd;

		private bool _isPlayingEmote;

		[WeaverGenerated]
		[DefaultForProperty("ActiveEmote", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private TentacleEmoteType _ActiveEmote;

		public bool IsPlayingEmote => _isPlayingEmote;

		public bool IsLocalKrakenBodyVisible => _localKrakenBodyVisible;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe TentacleEmoteType ActiveEmote
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing KrakenTentacleEmotesController.ActiveEmote. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(TentacleEmoteType*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing KrakenTentacleEmotesController.ActiveEmote. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(TentacleEmoteType*)((byte*)Ptr + 0) = value;
			}
		}

		public override void Spawned()
		{
			_localKrakenBodyVisible = true;
			ReplayActiveEmote();
		}

		public override void Render()
		{
			if (!(_krakenController == null) && !_isCorrectingVisibility)
			{
				bool isVisible = _krakenController.IsVisible;
				if (_localKrakenBodyVisible != isVisible)
				{
					BeginVisibilityCorrection(isVisible);
				}
			}
		}

		public void PlayEmote(TentacleEmoteType emoteType)
		{
			if (emoteType != TentacleEmoteType.None && base.HasStateAuthority)
			{
				if (emoteType == TentacleEmoteType.StopEmote)
				{
					StopEmote();
					return;
				}
				ActiveEmote = emoteType;
				PlayEmoteRpc(emoteType);
			}
		}

		public void StopEmote()
		{
			if (base.HasStateAuthority)
			{
				ActiveEmote = TentacleEmoteType.None;
				StopEmoteRpc();
			}
		}

		public void PlayAppear()
		{
			if (base.HasStateAuthority)
			{
				PlayVisibilityRpc(isVisible: true, _appearTriggerName);
			}
		}

		public void PlayDisappear()
		{
			if (base.HasStateAuthority)
			{
				PlayVisibilityRpc(isVisible: false, _disappearTriggerName);
			}
		}

		public void SetHidden()
		{
			if (base.HasStateAuthority)
			{
				SetVisibilityRpc(isVisible: false);
			}
		}

		public void SetVisible()
		{
			if (base.HasStateAuthority)
			{
				SetVisibilityRpc(isVisible: true);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 327413970u)]
		private void PlayEmoteRpc([RpcPayload(4)] TentacleEmoteType emoteType)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(327413970u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.KrakenModule.Scripts.KrakenTentacleEmotesController::PlayEmoteRpc(Features.KrakenModule.Scripts.TentacleEmoteType)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(emoteType, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (emoteType != TentacleEmoteType.None && _animators != null)
			{
				SetTrigger(emoteType.ToString());
				_isPlayingEmote = true;
				StartCoroutine(StartCheckForAttackEnd());
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3674708139u)]
		private void StopEmoteRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3674708139u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.KrakenModule.Scripts.KrakenTentacleEmotesController::StopEmoteRpc()", invokeInfo, PlayerRef.None);
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
			if (_animators != null)
			{
				SetTrigger(TentacleEmoteType.StopEmote.ToString());
				_checkForEmotesEnd = false;
				_isPlayingEmote = false;
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 2142630484u)]
		private void PlayVisibilityRpc([RpcPayload(4)] bool isVisible, [RpcPayload] string triggerName)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(isVisible);
				payloadSize += Fusion.RpcDataWriter.GetPayloadSize(triggerName);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2142630484u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.KrakenModule.Scripts.KrakenTentacleEmotesController::PlayVisibilityRpc(System.Boolean,System.String)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(isVisible);
						writer.Write(triggerName);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			CancelVisibilityCorrection();
			_localKrakenBodyVisible = isVisible;
			ResetTrigger(_appearTriggerName);
			ResetTrigger(_disappearTriggerName);
			SetTrigger(triggerName);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 4255686360u)]
		private void SetVisibilityRpc([RpcPayload(4)] bool isVisible)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(isVisible);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4255686360u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.KrakenModule.Scripts.KrakenTentacleEmotesController::SetVisibilityRpc(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(isVisible);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			CancelVisibilityCorrection();
			_localKrakenBodyVisible = isVisible;
		}

		private void BeginVisibilityCorrection(bool shouldBeVisible)
		{
			CancelVisibilityCorrection();
			_visibilityCorrectionCoroutine = StartCoroutine(shouldBeVisible ? CorrectToVisibleCoroutine() : CorrectToHiddenCoroutine());
		}

		private void CancelVisibilityCorrection()
		{
			if (_visibilityCorrectionCoroutine != null)
			{
				StopCoroutine(_visibilityCorrectionCoroutine);
				_visibilityCorrectionCoroutine = null;
				_isCorrectingVisibility = false;
			}
		}

		private IEnumerator CorrectToHiddenCoroutine()
		{
			_isCorrectingVisibility = true;
			SetTrigger(_disappearTriggerName);
			yield return WaitForVisibilityAnimationComplete(_disappearAnimationStateName);
			_localKrakenBodyVisible = false;
			_isCorrectingVisibility = false;
			_visibilityCorrectionCoroutine = null;
			_appearTentacleController?.NotifyKrakenBodyHiddenLocally();
		}

		private IEnumerator CorrectToVisibleCoroutine()
		{
			_isCorrectingVisibility = true;
			SetTrigger(_appearTriggerName);
			yield return WaitForVisibilityAnimationComplete(_appearAnimationStateName);
			_localKrakenBodyVisible = true;
			_isCorrectingVisibility = false;
			_visibilityCorrectionCoroutine = null;
		}

		private IEnumerator WaitForVisibilityAnimationComplete(string animationStateName)
		{
			float elapsed = 0f;
			float timeout = Mathf.Max(0.01f, _visibilityAnimationTimeout);
			bool animationStarted = string.IsNullOrEmpty(animationStateName);
			while (elapsed < timeout)
			{
				if (_mainAnimator != null && !string.IsNullOrEmpty(animationStateName))
				{
					AnimatorStateInfo currentAnimatorStateInfo = _mainAnimator.GetCurrentAnimatorStateInfo(0);
					AnimatorStateInfo nextAnimatorStateInfo = _mainAnimator.GetNextAnimatorStateInfo(0);
					if (currentAnimatorStateInfo.IsName(animationStateName) || nextAnimatorStateInfo.IsName(animationStateName))
					{
						animationStarted = true;
						if (currentAnimatorStateInfo.IsName(animationStateName) && currentAnimatorStateInfo.normalizedTime >= 1f)
						{
							break;
						}
					}
					else if (animationStarted)
					{
						break;
					}
				}
				elapsed += Time.deltaTime;
				yield return null;
			}
		}

		private void SetTrigger(string triggerName)
		{
			if (string.IsNullOrEmpty(triggerName) || _animators == null)
			{
				return;
			}
			Animator[] animators = _animators;
			foreach (Animator animator in animators)
			{
				if (animator != null)
				{
					animator.SetTrigger(triggerName);
				}
			}
		}

		private void ResetTrigger(string triggerName)
		{
			if (string.IsNullOrEmpty(triggerName) || _animators == null)
			{
				return;
			}
			Animator[] animators = _animators;
			foreach (Animator animator in animators)
			{
				if (animator != null)
				{
					animator.ResetTrigger(triggerName);
				}
			}
		}

		private IEnumerator StartCheckForAttackEnd()
		{
			yield return null;
			_checkForEmotesEnd = true;
		}

		private void ReplayActiveEmote()
		{
			if (ActiveEmote != TentacleEmoteType.None && _animators != null)
			{
				SetTrigger(ActiveEmote.ToString());
				_isPlayingEmote = true;
				StartCoroutine(StartCheckForAttackEnd());
			}
		}

		private void Update()
		{
			if (base.HasStateAuthority)
			{
				CheckAttackEnd();
			}
		}

		private void CheckAttackEnd()
		{
			if (_checkForEmotesEnd)
			{
				AnimatorStateInfo currentAnimatorStateInfo = GetCurrentAnimatorStateInfo("Emotes");
				AnimatorStateInfo nextAnimatorStateInfo = GetNextAnimatorStateInfo("Emotes");
				if (!currentAnimatorStateInfo.IsTag("Emote") && !nextAnimatorStateInfo.IsTag("Emote"))
				{
					_checkForEmotesEnd = false;
					_isPlayingEmote = false;
					ActiveEmote = TentacleEmoteType.None;
				}
			}
		}

		private AnimatorStateInfo GetCurrentAnimatorStateInfo(string layerName)
		{
			int layerIndex = _mainAnimator.GetLayerIndex(layerName);
			return _mainAnimator.GetCurrentAnimatorStateInfo(layerIndex);
		}

		private AnimatorStateInfo GetNextAnimatorStateInfo(string layerName)
		{
			int layerIndex = _mainAnimator.GetLayerIndex(layerName);
			return _mainAnimator.GetNextAnimatorStateInfo(layerIndex);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			ActiveEmote = _ActiveEmote;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_ActiveEmote = ActiveEmote;
		}

		[NetworkRpcWeavedInvoker(327413970u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayEmoteRpc_0040Invoker327413970([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out TentacleEmoteType value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((KrakenTentacleEmotesController)context.TargetBehaviour).PlayEmoteRpc(value);
		}

		[NetworkRpcWeavedInvoker(3674708139u)]
		[Preserve]
		[WeaverGenerated]
		protected static void StopEmoteRpc_0040Invoker3674708139([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((KrakenTentacleEmotesController)context.TargetBehaviour).StopEmoteRpc();
		}

		[NetworkRpcWeavedInvoker(2142630484u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayVisibilityRpc_0040Invoker2142630484([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out bool value);
			payloadReader.Read(out string value2);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((KrakenTentacleEmotesController)context.TargetBehaviour).PlayVisibilityRpc(value, value2);
		}

		[NetworkRpcWeavedInvoker(4255686360u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetVisibilityRpc_0040Invoker4255686360([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((KrakenTentacleEmotesController)context.TargetBehaviour).SetVisibilityRpc(value);
		}
	}
}
