using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Fusion;
using Obi;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.StrechArmsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class RopeLengthController : NetworkBehaviour
	{
		[SerializeField]
		private float _speedAdd = 10f;

		[SerializeField]
		private float _speedRemove = 5f;

		[SerializeField]
		private ObiRopeCursor _cursor;

		[SerializeField]
		private ObiRope _rope;

		private float _currentLength;

		private bool _isLengthChangeBlocked;

		public float CurrentLength => _currentLength;

		public void AddLength(float customSpeed = -1f)
		{
			if (!_isLengthChangeBlocked)
			{
				_currentLength += ((customSpeed > 0f) ? (customSpeed * Time.fixedDeltaTime) : (_speedAdd * Time.fixedDeltaTime));
				_cursor.ChangeLength((customSpeed > 0f) ? (customSpeed * Time.fixedDeltaTime) : (_speedAdd * Time.fixedDeltaTime));
			}
		}

		public void AddLengthWithCustomDelta(float deltaTime, float customSpeed = -1f)
		{
			if (!_isLengthChangeBlocked)
			{
				_currentLength += ((customSpeed > 0f) ? (customSpeed * deltaTime) : (_speedAdd * deltaTime));
				_cursor.ChangeLength((customSpeed > 0f) ? (customSpeed * deltaTime) : (_speedAdd * deltaTime));
			}
		}

		public void RemoveLengthWithCustomDelta(float deltaTime, float customSpeed = -1f)
		{
			if (!_isLengthChangeBlocked && !(_currentLength <= 0f))
			{
				if (_currentLength - _speedRemove * deltaTime <= 0f)
				{
					_currentLength -= _currentLength * deltaTime;
					_cursor.ChangeLength((0f - _currentLength) * deltaTime);
				}
				else
				{
					_currentLength -= _speedRemove * deltaTime;
					_cursor.ChangeLength((0f - _speedRemove) * deltaTime);
				}
			}
		}

		public void RemoveLength()
		{
			if (!_isLengthChangeBlocked && !(_currentLength <= 0f))
			{
				if (_currentLength - _speedRemove * Time.fixedDeltaTime <= 0f)
				{
					_currentLength -= _currentLength * Time.fixedDeltaTime;
					_cursor.ChangeLength((0f - _currentLength) * Time.fixedDeltaTime);
				}
				else
				{
					_currentLength -= _speedRemove * Time.fixedDeltaTime;
					_cursor.ChangeLength((0f - _speedRemove) * Time.fixedDeltaTime);
				}
			}
		}

		public void BlockLengthChange()
		{
			SwitchLengthChangeBlockedRpc(isBlocked: true);
		}

		public void UnBlockLengthChange()
		{
			SwitchLengthChangeBlockedRpc(isBlocked: false);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1955651173u)]
		private void SwitchLengthChangeBlockedRpc([RpcPayload(4)] bool isBlocked)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(isBlocked);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1955651173u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.StrechArmsModule.Scripts.RopeLengthController::SwitchLengthChangeBlockedRpc(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(isBlocked);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_isLengthChangeBlocked = isBlocked;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(1955651173u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SwitchLengthChangeBlockedRpc_0040Invoker1955651173([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RopeLengthController)context.TargetBehaviour).SwitchLengthChangeBlockedRpc(value);
		}
	}
}
