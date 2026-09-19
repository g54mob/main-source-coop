using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.Movement.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerPhysics : NetworkBehaviour
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private Collider _playerCollider;

		private PlayerMovableModel _playerMovableModel;

		private PlayersRagdollModel _playersRagdollModel;

		public float PlayerInitialMass { get; private set; }

		public PhysicsMaterial PlayerPhysicsMaterial { get; private set; }

		public Rigidbody RootRb
		{
			get
			{
				if (!(base.Object != null) || !_playersRagdollModel.TryGetPlayerRagdoll(base.Object.InputAuthority.PlayerId, out var ragdoll) || !ragdoll.IsSimulated)
				{
					return _rigidbody;
				}
				return ragdoll.RootPhysData.RigidBody;
			}
		}

		[Inject]
		public void InjectDependencies(PlayerMovableModel playerMovableModel, PlayersRagdollModel playersRagdollModel)
		{
			_playerMovableModel = playerMovableModel;
			_playersRagdollModel = playersRagdollModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.HasInputAuthority)
			{
				_playerMovableModel.MovablePhysics = this;
			}
			_playerMovableModel.AllMovablePhysics[base.Object.InputAuthority] = this;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if ((object)_playerMovableModel.MovablePhysics == this)
			{
				_playerMovableModel.MovablePhysics = null;
			}
		}

		private void Start()
		{
			PlayerPhysicsMaterial = _playerCollider.material;
			PlayerInitialMass = _rigidbody.mass;
		}

		public void SetPlayerMass(float mass)
		{
			SetPlayerMassRpc(mass);
		}

		public void ChangePlayerPhysMaterial(PhysicsMaterial physicsMaterial)
		{
			_playerCollider.material = physicsMaterial;
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2939120621u)]
		private void SetPlayerMassRpc([RpcPayload(4)] float mass)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2939120621u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.Movement.Scripts.PlayerPhysics::SetPlayerMassRpc(System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(mass, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_rigidbody.mass = mass;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(2939120621u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetPlayerMassRpc_0040Invoker2939120621([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out float value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PlayerPhysics)context.TargetBehaviour).SetPlayerMassRpc(value);
		}
	}
}
