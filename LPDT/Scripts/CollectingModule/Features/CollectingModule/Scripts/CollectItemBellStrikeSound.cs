using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AudioServiceModule.Scripts;
using Features.ItemsModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.CollectingModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class CollectItemBellStrikeSound : NetworkBehaviour
	{
		[SerializeField]
		private Transform _innerColliderRoot;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private MonoItem _monoItem;

		private IAudioService _audioService;

		private CollectItemBellConfig _bellConfig;

		private float _nextPlayTime;

		[Inject]
		public void InjectDependencies(IAudioService audioService)
		{
			_audioService = audioService;
		}

		private void Awake()
		{
			_bellConfig = (CollectItemBellConfig)_monoItem.DefaultConfig;
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (base.HasStateAuthority && !_bellConfig.BellSound.IsNull && !(Time.time < _nextPlayTime))
			{
				float num = collision.impulse.magnitude / Time.fixedDeltaTime;
				if (num < _bellConfig.ForceThreshold)
				{
					num = collision.relativeVelocity.magnitude * _bellConfig.ForceThreshold;
				}
				if (!(num < _bellConfig.ForceThreshold) && IsInnerBellHit(collision))
				{
					_nextPlayTime = Time.time + _bellConfig.PlayCooldown;
					PlayBellSoundRpc();
				}
			}
		}

		private bool IsInnerBellHit(Collision collision)
		{
			int contactCount = collision.contactCount;
			for (int i = 0; i < contactCount; i++)
			{
				Collider thisCollider = collision.GetContact(i).thisCollider;
				if (!(thisCollider == null) && IsUnderInnerRoot(thisCollider.transform))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsUnderInnerRoot(Transform target)
		{
			if (!(target == _innerColliderRoot))
			{
				return target.IsChildOf(_innerColliderRoot);
			}
			return true;
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3718820363u)]
		private void PlayBellSoundRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3718820363u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CollectingModule.Scripts.CollectItemBellStrikeSound::PlayBellSoundRpc()", invokeInfo, PlayerRef.None);
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
			_audioService.PlayOneShotAttached(_bellConfig.BellSound, _soundSourceBehaviour);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(3718820363u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayBellSoundRpc_0040Invoker3718820363([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((CollectItemBellStrikeSound)context.TargetBehaviour).PlayBellSoundRpc();
		}
	}
}
