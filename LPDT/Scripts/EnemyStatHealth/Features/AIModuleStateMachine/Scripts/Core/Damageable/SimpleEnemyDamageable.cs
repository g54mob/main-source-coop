using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.DamageableTrackModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.AIModuleStateMachine.Scripts.Core.Damageable
{
	[NetworkBehaviourWeaved(0)]
	public class SimpleEnemyDamageable : NetworkBehaviour, IDamageable
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		public NetworkObject NetworkObject => base.Object;

		public Transform Transform => base.transform;

		public bool IsDamageBlocked { get; set; }

		public bool IsActive { get; set; } = true;

		public float Health { get; }

		public bool IsFullHealth => false;

		public List<DamageableTag> DamageableTags { get; set; } = new List<DamageableTag>();

		public event Action<DamageData> OnDamaged;

		public event Action<bool> OnIsActiveChanged;

		public event Action<float> OnHealed;

		public void Damage(DamageData damage)
		{
			if (!IsDamageBlocked)
			{
				this.OnDamaged?.Invoke(damage);
			}
		}

		public void HealToFullValue()
		{
		}

		public void DamageRPC(float damage)
		{
			DamageRPC(damage, 0, default(DamageRpcSource));
		}

		public void DamageRPC(float damage, int dealerPlayerID)
		{
			DamageRPC(damage, dealerPlayerID, default(DamageRpcSource));
		}

		public void DamageRPC(float damage, int dealerPlayerID, DamageRpcSource rpcSource)
		{
			if (TryInvokeDamageRpc())
			{
				DamageRpc(damage, dealerPlayerID, rpcSource);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2912824898u)]
		private void DamageRpc([RpcPayload(4)] float damage, [RpcPayload(4)] int dealerPlayerID, [RpcPayload(208)] DamageRpcSource rpcSource)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(208);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2912824898u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Core.Damageable.SimpleEnemyDamageable::DamageRpc(System.Single,System.Int32,Features.DamageableTrackModule.Scripts.DamageRpcSource)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(damage, 4);
						writer.Write(dealerPlayerID, 4);
						writer.Write(rpcSource, 208);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			Damage(new DamageData
			{
				Damage = damage,
				DamageDealerPlayerID = dealerPlayerID
			});
		}

		private bool TryInvokeDamageRpc()
		{
			if (base.Object != null && base.Object.IsValid)
			{
				return true;
			}
			return false;
		}

		public void SetDamageableTags(List<DamageableTag> entityDamageableTags)
		{
			DamageableTags = entityDamageableTags;
		}

		public void AddRPCForce(float force, Vector3 direction, ForceMode forceMode)
		{
			if (_rigidbody != null && base.Object != null && base.Object.IsValid)
			{
				AddForceRPC(force, direction, forceMode);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 344659150u)]
		public void AddForceRPC([RpcPayload(4)] float force, [RpcPayload(12)] Vector3 direction, [RpcPayload(4)] ForceMode forceMode)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(344659150u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Core.Damageable.SimpleEnemyDamageable::AddForceRPC(System.Single,UnityEngine.Vector3,UnityEngine.ForceMode)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(force, 4);
						writer.Write(direction, 12);
						writer.Write(forceMode, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_rigidbody != null)
			{
				Vector3 force2 = direction * force;
				_rigidbody.AddForce(force2, forceMode);
			}
		}

		public void Heal(float value, bool isSynchronize = false)
		{
			this.OnHealed?.Invoke(value);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(2912824898u)]
		[Preserve]
		[WeaverGenerated]
		protected static void DamageRpc_0040Invoker2912824898([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out float value, 4);
			payloadReader.Read(out int value2, 4);
			payloadReader.Read(out DamageRpcSource value3, 208);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimpleEnemyDamageable)context.TargetBehaviour).DamageRpc(value, value2, value3);
		}

		[NetworkRpcWeavedInvoker(344659150u)]
		[Preserve]
		[WeaverGenerated]
		protected static void AddForceRPC_0040Invoker344659150([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out float value, 4);
			payloadReader.Read(out Vector3 value2, 12);
			payloadReader.Read(out ForceMode value3, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SimpleEnemyDamageable)context.TargetBehaviour).AddForceRPC(value, value2, value3);
		}
	}
}
