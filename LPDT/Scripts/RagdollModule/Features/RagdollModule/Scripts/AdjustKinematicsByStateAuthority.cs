using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.RagdollModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class AdjustKinematicsByStateAuthority : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface
	{
		[SerializeField]
		private List<KinematicsAdjustData> _affectedRigidbodies;

		[SerializeField]
		private bool _isForecastEnabled;

		private bool _isPhysicsDisabled;

		private void Awake()
		{
			ClampUntilSpawned();
		}

		public override void Spawned()
		{
			base.Spawned();
			ApplyKinematics();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			ClampUntilSpawned();
		}

		public void StateAuthorityChanged()
		{
			ApplyKinematics();
		}

		public void SetPhysicsActive(bool active)
		{
			SetPhysicsActiveRpc(active);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 4030853837u)]
		private void SetPhysicsActiveRpc([RpcPayload(4)] bool active)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(active);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4030853837u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RagdollModule.Scripts.AdjustKinematicsByStateAuthority::SetPhysicsActiveRpc(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(active);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_isPhysicsDisabled = !active;
			if (_affectedRigidbodies == null)
			{
				return;
			}
			if (!active)
			{
				foreach (KinematicsAdjustData affectedRigidbody in _affectedRigidbodies)
				{
					if (!(affectedRigidbody.Rigidbody == null))
					{
						affectedRigidbody.Rigidbody.isKinematic = true;
					}
				}
				return;
			}
			foreach (KinematicsAdjustData affectedRigidbody2 in _affectedRigidbodies)
			{
				if (!(affectedRigidbody2.Rigidbody == null))
				{
					affectedRigidbody2.Rigidbody.linearVelocity = Vector3.zero;
					affectedRigidbody2.Rigidbody.angularVelocity = Vector3.zero;
				}
			}
			ApplyKinematics();
		}

		private void ClampUntilSpawned()
		{
			if (_affectedRigidbodies == null)
			{
				return;
			}
			foreach (KinematicsAdjustData affectedRigidbody in _affectedRigidbodies)
			{
				if (!(affectedRigidbody.Rigidbody == null))
				{
					affectedRigidbody.Rigidbody.isKinematic = true;
				}
			}
		}

		protected void ApplyKinematics()
		{
			if (_affectedRigidbodies == null || _isPhysicsDisabled)
			{
				return;
			}
			bool flag = !_isForecastEnabled && !base.HasStateAuthority;
			foreach (KinematicsAdjustData affectedRigidbody in _affectedRigidbodies)
			{
				if (!(affectedRigidbody.Rigidbody == null))
				{
					bool flag2 = flag || affectedRigidbody.AuthorityKinematicsDetector.IsPhysicsKinematics();
					if (affectedRigidbody.Rigidbody.isKinematic != flag2)
					{
						affectedRigidbody.Rigidbody.isKinematic = flag2;
					}
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

		[NetworkRpcWeavedInvoker(4030853837u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetPhysicsActiveRpc_0040Invoker4030853837([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((AdjustKinematicsByStateAuthority)context.TargetBehaviour).SetPhysicsActiveRpc(value);
		}
	}
}
