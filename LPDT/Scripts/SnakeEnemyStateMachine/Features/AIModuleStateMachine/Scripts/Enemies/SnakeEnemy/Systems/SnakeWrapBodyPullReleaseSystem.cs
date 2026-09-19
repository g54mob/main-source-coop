using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.States;
using Features.GrabModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.SnakeModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class SnakeWrapBodyPullReleaseSystem : NetworkBehaviour, ISnakeFrameLateTick
	{
		[SerializeField]
		private SnakeController _snakeController;

		[Tooltip("Radial excess beyond wrap orbit radius to end Wrap (ally grab only).")]
		[SerializeField]
		private float _wrapReleasePullDistance = 0.75f;

		private SnakeEnemy _enemy;

		private SnakeEnemyContext _context;

		private LineArmsModel _lineArmsModel;

		private SnakeFrameOrchestrator _frameOrchestrator;

		private bool _isEscapeRequested;

		[Inject]
		public void InjectDependencies(SnakeEnemy enemy, SnakeEnemyContext context, LineArmsModel lineArmsModel)
		{
			_enemy = enemy;
			_context = context;
			_lineArmsModel = lineArmsModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			TryRegisterWithOrchestrator();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			UnregisterFromOrchestrator();
			base.Despawned(runner, hasState);
		}

		public void TickLate()
		{
			if (_enemy == null || _context == null || _snakeController == null || !base.Object || !base.Object.IsValid || base.Runner == null)
			{
				return;
			}
			if (!_context.IsWrapOrbitActive)
			{
				_isEscapeRequested = false;
			}
			else
			{
				if (_isEscapeRequested || _wrapReleasePullDistance <= 0f)
				{
					return;
				}
				int playerId = base.Runner.LocalPlayer.PlayerId;
				if (_context.WrapTargetPlayerId != playerId && TryGetLocalGrabOrbitPullExcess(out var excess) && !(excess < _wrapReleasePullDistance))
				{
					_isEscapeRequested = true;
					ReleaseLocalBodyGrabs();
					if (base.HasStateAuthority)
					{
						TryFinishWrapOnAuthority(playerId);
					}
					else
					{
						RequestWrapEscapeRpc(playerId);
					}
				}
			}
		}

		private bool TryGetLocalGrabOrbitPullExcess(out float excess)
		{
			excess = 0f;
			Dictionary<LineArmType, LineArmControllerBase> allLineArmsForPlayer = _lineArmsModel.GetAllLineArmsForPlayer(base.Runner.LocalPlayer.PlayerId);
			if (allLineArmsForPlayer == null)
			{
				return false;
			}
			Vector3 wrapOrbitCenter = _context.WrapOrbitCenter;
			float num = Mathf.Max(0.01f, _context.StepAggroWrapOrbitRadius);
			bool flag = false;
			foreach (LineArmControllerBase value in allLineArmsForPlayer.Values)
			{
				if (value == null)
				{
					continue;
				}
				IReadOnlyList<IPointGrabable> currentGrabbables = value.CurrentGrabbables;
				for (int i = 0; i < currentGrabbables.Count; i++)
				{
					IPointGrabable pointGrabable = currentGrabbables[i];
					if (pointGrabable != null && !(pointGrabable.GameObject == null) && _snakeController.IsBodyPartGrabable(pointGrabable))
					{
						float num2 = Vector3.Distance(pointGrabable.GameObject.transform.position, wrapOrbitCenter) - num;
						if (!flag || num2 > excess)
						{
							excess = num2;
							flag = true;
						}
					}
				}
			}
			return flag;
		}

		private void TryFinishWrapOnAuthority(int pullerPlayerId)
		{
			if (base.HasStateAuthority && !(_enemy == null) && !(_context == null) && (bool)_context.IsWrapOrbitActive && _enemy.CurrentStateId == SnakeStateId.Wrap && _context.WrapTargetPlayerId != pullerPlayerId)
			{
				ForceReleaseBodyGrabsRpc();
				_context.BeginWrapEscapeCooldown();
				_context.SetPriorityPlayer(null);
				_enemy.TriggerEvent(SnakeEvent.OnWrapFinished);
			}
		}

		private void TryRegisterWithOrchestrator()
		{
			if (!(_snakeController == null))
			{
				_frameOrchestrator = _snakeController.GetComponent<SnakeFrameOrchestrator>();
				if (!(_frameOrchestrator == null))
				{
					_frameOrchestrator.RegisterLateTick(this);
				}
			}
		}

		private void UnregisterFromOrchestrator()
		{
			if (!(_frameOrchestrator == null))
			{
				_frameOrchestrator.UnregisterLateTick(this);
				_frameOrchestrator = null;
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 3095044628u)]
		private void RequestWrapEscapeRpc([RpcPayload(4)] int pullerPlayerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3095044628u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems.SnakeWrapBodyPullReleaseSystem::RequestWrapEscapeRpc(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(pullerPlayerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			TryFinishWrapOnAuthority(pullerPlayerId);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3298652086u)]
		private void ForceReleaseBodyGrabsRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3298652086u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems.SnakeWrapBodyPullReleaseSystem::ForceReleaseBodyGrabsRpc()", invokeInfo, PlayerRef.None);
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
			ReleaseLocalBodyGrabs();
		}

		private void ReleaseLocalBodyGrabs()
		{
			if (_snakeController == null || _lineArmsModel == null || base.Runner == null)
			{
				return;
			}
			Dictionary<LineArmType, LineArmControllerBase> allLineArmsForPlayer = _lineArmsModel.GetAllLineArmsForPlayer(base.Runner.LocalPlayer.PlayerId);
			if (allLineArmsForPlayer == null)
			{
				return;
			}
			foreach (LineArmControllerBase value in allLineArmsForPlayer.Values)
			{
				if (!(value == null))
				{
					value.UnJoinAllMatching(IsSnakeBodyGrabable);
				}
			}
		}

		private bool IsSnakeBodyGrabable(IPointGrabable grabable)
		{
			if (_snakeController != null)
			{
				return _snakeController.IsBodyPartGrabable(grabable);
			}
			return false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(3095044628u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RequestWrapEscapeRpc_0040Invoker3095044628([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SnakeWrapBodyPullReleaseSystem)context.TargetBehaviour).RequestWrapEscapeRpc(value);
		}

		[NetworkRpcWeavedInvoker(3298652086u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ForceReleaseBodyGrabsRpc_0040Invoker3298652086([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SnakeWrapBodyPullReleaseSystem)context.TargetBehaviour).ForceReleaseBodyGrabsRpc();
		}
	}
}
