using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Fusion;
using NetworkServices.ObjectsProvider;
using QuickOutline.Scripts;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Shared
{
	[NetworkBehaviourWeaved(1)]
	public class HeadCrabHome : NetworkBehaviour
	{
		private static readonly int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");

		[SerializeField]
		private Outline _outline;

		[SerializeField]
		private Renderer _nestRenderer;

		[SerializeField]
		private float _dissolveDuration = 1f;

		[SerializeField]
		private float _despawnDelay = 2f;

		[WeaverGenerated]
		[DefaultForProperty("DissolveStartTick", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _DissolveStartTick;

		private Material _dissolveInstance;

		private bool _isDissolveStarted;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe int DissolveStartTick
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadCrabHome.DissolveStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing HeadCrabHome.DissolveStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		private void Awake()
		{
			_dissolveInstance = _nestRenderer.material;
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			UnityEngine.Object.Destroy(_dissolveInstance);
		}

		public override void Spawned()
		{
			base.Spawned();
			_outline.enabled = false;
			_isDissolveStarted = false;
			_dissolveInstance.SetFloat(_dissolveAmount, 0f);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Object.HasStateAuthority && DissolveStartTick != 0 && GetDissolveElapsedTime() >= _despawnDelay)
			{
				base.Object.DespawnHierarchy();
			}
		}

		public override void Render()
		{
			if (DissolveStartTick != 0)
			{
				if (!_isDissolveStarted)
				{
					StartDissolve();
				}
				_dissolveInstance.SetFloat(_dissolveAmount, Mathf.Clamp01(GetDissolveElapsedTime() / _dissolveDuration));
			}
		}

		public void SetOutlineActiveForPlayer(bool isActive, int playerId)
		{
			SetOutlineActiveForPlayerRPC(isActive, playerId);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3505788816u)]
		private void SetOutlineActiveForPlayerRPC([RpcPayload(4)] bool isActive, [RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(isActive);
				payloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3505788816u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Shared.HeadCrabHome::SetOutlineActiveForPlayerRPC(System.Boolean,System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(isActive);
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
				_outline.enabled = isActive;
			}
		}

		public void StartDespawnLoop()
		{
			if (base.Object.HasStateAuthority)
			{
				DissolveStartTick = base.Runner.Tick;
			}
		}

		private void StartDissolve()
		{
			_isDissolveStarted = true;
			_outline.enabled = false;
		}

		private float GetDissolveElapsedTime()
		{
			return (float)((int)base.Runner.Tick - DissolveStartTick) * base.Runner.DeltaTime;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			DissolveStartTick = _DissolveStartTick;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_DissolveStartTick = DissolveStartTick;
		}

		[NetworkRpcWeavedInvoker(3505788816u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetOutlineActiveForPlayerRPC_0040Invoker3505788816([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out bool value);
			payloadReader.Read(out int value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((HeadCrabHome)context.TargetBehaviour).SetOutlineActiveForPlayerRPC(value, value2);
		}
	}
}
