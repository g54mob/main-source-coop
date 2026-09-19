using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.GrabModule.Scripts;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.HingeModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class HingeAuthorityController : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface
	{
		[SerializeField]
		private NetworkObjectComposite _networkObjectComposite;

		private readonly List<PlayerCharacterMovableBase> _characterMovableBases = new List<PlayerCharacterMovableBase>();

		private bool _enabled;

		public override void Spawned()
		{
			_enabled = true;
		}

		public void StateAuthorityChanged()
		{
			if (base.HasStateAuthority)
			{
				UpdateDoorAuthority();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<PlayerCharacterMovableBase>(out var component) && !_characterMovableBases.Contains(component))
			{
				_characterMovableBases.Add(component);
				UpdateDoorAuthority();
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent<PlayerCharacterMovableBase>(out var component))
			{
				_characterMovableBases.Remove(component);
				UpdateDoorAuthority();
			}
		}

		public void Disable()
		{
			_enabled = false;
		}

		private void UpdateDoorAuthority()
		{
			if (_enabled && _characterMovableBases.Count != 0)
			{
				PlayerCharacterMovableBase closestPlayer = GetClosestPlayer();
				if (!(closestPlayer == null))
				{
					int playerId = closestPlayer.Object.InputAuthority.PlayerId;
					RequestDoorAuthorityRPC(playerId);
				}
			}
		}

		private PlayerCharacterMovableBase GetClosestPlayer()
		{
			if (_characterMovableBases.Count == 0)
			{
				return null;
			}
			Vector3 position = base.transform.position;
			PlayerCharacterMovableBase result = null;
			float num = float.MaxValue;
			foreach (PlayerCharacterMovableBase characterMovableBasis in _characterMovableBases)
			{
				if (!(characterMovableBasis == null))
				{
					float num2 = Vector3.Distance(position, characterMovableBasis.transform.position);
					if (num2 < num)
					{
						num = num2;
						result = characterMovableBasis;
					}
				}
			}
			return result;
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1093055402u)]
		private void RequestDoorAuthorityRPC([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1093055402u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.HingeModule.Scripts.HingeAuthorityController::RequestDoorAuthorityRPC(System.Int32)", invokeInfo, PlayerRef.None);
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
			if (base.Runner.LocalPlayer.PlayerId == playerId)
			{
				if (_networkObjectComposite != null)
				{
					_networkObjectComposite.RequestStateAuthority();
				}
				else
				{
					base.Object.RequestStateAuthority();
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(1093055402u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RequestDoorAuthorityRPC_0040Invoker1093055402([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HingeAuthorityController)context.TargetBehaviour).RequestDoorAuthorityRPC(value);
		}
	}
}
