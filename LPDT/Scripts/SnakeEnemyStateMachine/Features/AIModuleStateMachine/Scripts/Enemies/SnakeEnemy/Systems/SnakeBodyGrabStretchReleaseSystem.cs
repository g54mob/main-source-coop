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
	public class SnakeBodyGrabStretchReleaseSystem : NetworkBehaviour, ISnakeFrameLateTick
	{
		[SerializeField]
		private SnakeController _snakeController;

		[Tooltip("World units a grabbed segment may stretch from trail rest before force-UnJoin.")]
		[SerializeField]
		private float _maxTrailStretch = 1.5f;

		[Tooltip("Seconds after a body grab latches before stretch UnJoin can fire. Covers enter soft-latch so PhysGrab cannot immediately exceed max stretch and release. 0 = no grace.")]
		[SerializeField]
		private float _stretchReleaseGrace = 0.35f;

		private SnakeEnemy _enemy;

		private SnakeEnemyContext _context;

		private LineArmsModel _lineArmsModel;

		private SnakeFrameOrchestrator _frameOrchestrator;

		private bool _wasAnyBodyPartGrabbed;

		private float _grabLatchTime = -1f;

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
			_wasAnyBodyPartGrabbed = false;
			_grabLatchTime = -1f;
			TryRegisterWithOrchestrator();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			UnregisterFromOrchestrator();
			_wasAnyBodyPartGrabbed = false;
			_grabLatchTime = -1f;
			base.Despawned(runner, hasState);
		}

		public void TickLate()
		{
			if (!base.HasStateAuthority || _snakeController == null || !base.Object || !base.Object.IsValid || (_enemy != null && (_enemy.CurrentStateId == SnakeStateId.Wrap || (_context != null && (bool)_context.IsWrapOrbitActive))) || _maxTrailStretch <= 0f)
			{
				return;
			}
			if (!_snakeController.TryGetMaxGrabbedTrailStretch(out var stretch))
			{
				_wasAnyBodyPartGrabbed = false;
				_grabLatchTime = -1f;
				return;
			}
			if (!_wasAnyBodyPartGrabbed)
			{
				_wasAnyBodyPartGrabbed = true;
				_grabLatchTime = ((base.Runner != null) ? base.Runner.SimulationTime : Time.time);
			}
			if (!IsWithinStretchReleaseGrace() && !(stretch < _maxTrailStretch))
			{
				ForceReleaseBodyGrabsRpc();
			}
		}

		private bool IsWithinStretchReleaseGrace()
		{
			if (_stretchReleaseGrace <= 0f || _grabLatchTime < 0f)
			{
				return false;
			}
			return ((base.Runner != null) ? base.Runner.SimulationTime : Time.time) - _grabLatchTime < _stretchReleaseGrace;
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

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 1453889350u)]
		private void ForceReleaseBodyGrabsRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1453889350u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems.SnakeBodyGrabStretchReleaseSystem::ForceReleaseBodyGrabsRpc()", invokeInfo, PlayerRef.None);
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

		[NetworkRpcWeavedInvoker(1453889350u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ForceReleaseBodyGrabsRpc_0040Invoker1453889350([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SnakeBodyGrabStretchReleaseSystem)context.TargetBehaviour).ForceReleaseBodyGrabsRpc();
		}
	}
}
