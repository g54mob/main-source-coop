using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.GrabModule.Scripts;
using Features.InteractModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.BlackjackBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class BlackjackDeckBellInteractable : InteractableBase
	{
		[SerializeField]
		private BlackjackBeachInteractableBehaviour _table;

		[SerializeField]
		private SimplePointGrabable _grabable;

		[SerializeField]
		private Transform _buttonTransform;

		private BlackjackConfiguration _configuration;

		private Coroutine _pressRoutine;

		[Inject]
		public void InjectDependencies(BlackjackConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void BindTable(BlackjackBeachInteractableBehaviour table)
		{
			_table = table;
		}

		public override void Spawned()
		{
			base.Spawned();
			SetButtonLocalY(_configuration.BellButtonRestLocalY);
			_grabable.LocalOnGrab += OnLocalGrab;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_grabable.LocalOnGrab -= OnLocalGrab;
			StopPressRoutine();
			SetButtonLocalY(_configuration.BellButtonRestLocalY);
			base.Despawned(runner, hasState);
		}

		public override void Interact()
		{
			base.Interact();
			if (IsInteractable)
			{
				RequestPressAndDraw();
			}
		}

		private void OnLocalGrab(int playerId)
		{
			if (!(base.Runner == null) && base.Runner.LocalPlayer.PlayerId == playerId)
			{
				RequestPressAndDraw();
			}
		}

		private void RequestPressAndDraw()
		{
			PlayButtonPressRpc();
			_table.RequestDrawCard();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 423392638u)]
		private void PlayButtonPressRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(423392638u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BlackjackBeachInteractableModule.Scripts.BlackjackDeckBellInteractable::PlayButtonPressRpc()", invokeInfo, PlayerRef.None);
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
			StopPressRoutine();
			_pressRoutine = StartCoroutine(AnimateButtonPress());
		}

		private IEnumerator AnimateButtonPress()
		{
			float restY = _configuration.BellButtonRestLocalY;
			float pressedY = _configuration.BellButtonPressedLocalY;
			float duration = Mathf.Max(0.01f, _configuration.BellButtonPressDuration);
			float releaseDuration = Mathf.Max(0.01f, _configuration.BellButtonReleaseDuration);
			yield return MoveButtonY(restY, pressedY, duration);
			yield return MoveButtonY(pressedY, restY, releaseDuration);
			SetButtonLocalY(restY);
			_pressRoutine = null;
		}

		private IEnumerator MoveButtonY(float fromY, float toY, float duration)
		{
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float t = Mathf.Clamp01(elapsed / duration);
				SetButtonLocalY(Mathf.Lerp(fromY, toY, t));
				yield return null;
			}
			SetButtonLocalY(toY);
		}

		private void SetButtonLocalY(float localY)
		{
			Vector3 localPosition = _buttonTransform.localPosition;
			localPosition.y = localY;
			_buttonTransform.localPosition = localPosition;
		}

		private void StopPressRoutine()
		{
			if (_pressRoutine != null)
			{
				StopCoroutine(_pressRoutine);
				_pressRoutine = null;
			}
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

		[NetworkRpcWeavedInvoker(423392638u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayButtonPressRpc_0040Invoker423392638([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BlackjackDeckBellInteractable)context.TargetBehaviour).PlayButtonPressRpc();
		}
	}
}
