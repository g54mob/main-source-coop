using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.Movement.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.TrampolineModule.Scripts
{
	[RequireComponent(typeof(Rigidbody))]
	[NetworkBehaviourWeaved(0)]
	public class TrampolineBehaviour : NetworkBehaviour
	{
		private enum SurfaceAnimPhase
		{
			Idle = 0,
			Compressing = 1,
			Releasing = 2
		}

		private struct BounceState
		{
			public int BounceCount;

			public float NextAllowedTime;

			public float LastBounceTime;
		}

		private struct QueuedBounce
		{
			public PlayerCharacterMovableBase Player;

			public Vector3 Velocity;
		}

		private static readonly int CompressionHash = Animator.StringToHash("Compression");

		private static readonly int StretchStateHash = Animator.StringToHash("Stretch");

		[Header("Animation")]
		[SerializeField]
		private Animator _animator;

		private TrampolineConfiguration _configuration;

		private readonly Dictionary<int, BounceState> _bounceStateByPlayerId = new Dictionary<int, BounceState>();

		private readonly Dictionary<int, QueuedBounce> _queuedBouncesByPlayerId = new Dictionary<int, QueuedBounce>();

		private SurfaceAnimPhase _surfacePhase;

		private float _surfaceDisplayed;

		private float _surfacePhaseFrom;

		private float _surfacePhaseTo;

		private float _surfacePhaseElapsed;

		private float _surfacePhaseDuration;

		[Inject]
		public void InjectDependencies(TrampolineConfiguration configuration)
		{
			_configuration = configuration;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (_animator == null)
			{
				_animator = GetComponentInChildren<Animator>(includeInactive: true);
			}
			EnsureStretchStateReady();
			ApplySurfaceCompression(0f);
		}

		private void Update()
		{
			if (_configuration == null || _surfacePhase == SurfaceAnimPhase.Idle)
			{
				return;
			}
			float num = Mathf.Max(0.0001f, _surfacePhaseDuration);
			_surfacePhaseElapsed += Time.deltaTime;
			float num2 = Mathf.Clamp01(_surfacePhaseElapsed / num);
			float t = num2 * num2 * (3f - 2f * num2);
			_surfaceDisplayed = Mathf.Lerp(_surfacePhaseFrom, _surfacePhaseTo, t);
			ApplySurfaceCompression(_surfaceDisplayed);
			if (!(num2 < 1f))
			{
				if (_surfacePhase == SurfaceAnimPhase.Compressing)
				{
					BeginReleasePhase();
					return;
				}
				_surfacePhase = SurfaceAnimPhase.Idle;
				_surfaceDisplayed = 0f;
				ApplySurfaceCompression(0f);
			}
		}

		private void FixedUpdate()
		{
			if (_queuedBouncesByPlayerId.Count == 0)
			{
				return;
			}
			List<QueuedBounce> list = new List<QueuedBounce>(_queuedBouncesByPlayerId.Count);
			foreach (KeyValuePair<int, QueuedBounce> item in _queuedBouncesByPlayerId)
			{
				list.Add(item.Value);
			}
			_queuedBouncesByPlayerId.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				ApplyBounceImmediate(list[i].Player, list[i].Velocity);
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (_configuration == null || !TryGetBounceTarget(collision, out var player, out var playerId) || player.Object == null || !player.Object.HasStateAuthority)
			{
				return;
			}
			if (!_bounceStateByPlayerId.TryGetValue(playerId, out var value))
			{
				value = default(BounceState);
			}
			if (value.BounceCount > 0 && value.LastBounceTime > 0f && Time.time - value.LastBounceTime > _configuration.BounceChainResetTime)
			{
				value.BounceCount = 0;
			}
			if ((!_configuration.UseBounceCountLimit || value.BounceCount < Mathf.Max(1, _configuration.MaxBounceCount)) && !(Time.time < value.NextAllowedTime) && !(collision.relativeVelocity.magnitude < _configuration.MinImpactSpeed))
			{
				float num = ResolveBounceHeight(value.BounceCount);
				float num2 = HeightToUpSpeed(num);
				if (!(num2 <= 0f))
				{
					value.BounceCount++;
					value.NextAllowedTime = Time.time + _configuration.CooldownPerPlayer;
					value.LastBounceTime = Time.time;
					_bounceStateByPlayerId[playerId] = value;
					QueueBounce(playerId, player, Vector3.up * num2);
					RequestSurfaceCompression(MapHeightToCompression(num));
				}
			}
		}

		private void QueueBounce(int playerId, PlayerCharacterMovableBase player, Vector3 bounceVelocity)
		{
			if (!_queuedBouncesByPlayerId.ContainsKey(playerId))
			{
				_queuedBouncesByPlayerId[playerId] = new QueuedBounce
				{
					Player = player,
					Velocity = bounceVelocity
				};
			}
		}

		private void ApplyBounceImmediate(PlayerCharacterMovableBase player, Vector3 bounceVelocity)
		{
			if (player == null)
			{
				return;
			}
			PlayerRagdollEntity componentInChildren = player.GetComponentInChildren<PlayerRagdollEntity>(includeInactive: true);
			if (componentInChildren != null && componentInChildren.IsInitialized && componentInChildren.IsSimulated)
			{
				componentInChildren.ApplyWholeBodyBounce(bounceVelocity);
				return;
			}
			Rigidbody rigidbody = player.Rigidbody;
			if (!(rigidbody == null) && !rigidbody.isKinematic)
			{
				rigidbody.WakeUp();
				rigidbody.linearVelocity = bounceVelocity;
				rigidbody.angularVelocity = Vector3.zero;
			}
		}

		private bool TryGetBounceTarget(Collision collision, out PlayerCharacterMovableBase player, out int playerId)
		{
			player = null;
			playerId = 0;
			LayerMask affectedLayers = _configuration.AffectedLayers;
			if (affectedLayers.value != 0 && (affectedLayers.value & (1 << collision.gameObject.layer)) == 0)
			{
				return false;
			}
			player = collision.collider.GetComponentInParent<PlayerCharacterMovableBase>();
			if (player == null && collision.rigidbody != null)
			{
				player = collision.rigidbody.GetComponentInParent<PlayerCharacterMovableBase>();
			}
			if (player == null || player.Object == null)
			{
				return false;
			}
			playerId = player.Object.InputAuthority.PlayerId;
			if (playerId < 0)
			{
				playerId = player.Object.Id.GetHashCode();
			}
			if (!_configuration.RequireRagdoll)
			{
				return true;
			}
			PlayerRagdollEntity componentInChildren = player.GetComponentInChildren<PlayerRagdollEntity>(includeInactive: true);
			if (componentInChildren != null && componentInChildren.IsInitialized)
			{
				return componentInChildren.IsSimulated;
			}
			return false;
		}

		private void RequestSurfaceCompression(float targetCompression)
		{
			if (!(base.Object == null) && base.Object.IsValid)
			{
				PlaySurfaceCompressionRpc(targetCompression);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2640329475u)]
		private void PlaySurfaceCompressionRpc([RpcPayload(4)] float targetCompression)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2640329475u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.TrampolineModule.Scripts.TrampolineBehaviour::PlaySurfaceCompressionRpc(System.Single)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(targetCompression, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			BeginCompressPhase(targetCompression);
		}

		private void BeginCompressPhase(float targetCompression)
		{
			if (!(_configuration == null))
			{
				float num = Mathf.Clamp01(targetCompression);
				if (num < _configuration.MinVisualCompression && num > 0f)
				{
					num = _configuration.MinVisualCompression;
				}
				if (_surfacePhase == SurfaceAnimPhase.Compressing)
				{
					num = Mathf.Max(num, _surfacePhaseTo);
				}
				_surfacePhaseFrom = _surfaceDisplayed;
				_surfacePhaseTo = Mathf.Max(num, _surfacePhaseFrom);
				_surfacePhaseElapsed = 0f;
				_surfacePhaseDuration = Mathf.Max(0.01f, _configuration.CompressDuration);
				_surfacePhase = SurfaceAnimPhase.Compressing;
				EnsureStretchStateReady();
				ApplySurfaceCompression(_surfaceDisplayed);
			}
		}

		private void BeginReleasePhase()
		{
			if (_configuration == null)
			{
				_surfacePhase = SurfaceAnimPhase.Idle;
				return;
			}
			_surfacePhaseFrom = _surfaceDisplayed;
			_surfacePhaseTo = 0f;
			_surfacePhaseElapsed = 0f;
			_surfacePhaseDuration = Mathf.Max(0.01f, _configuration.ReleaseDuration);
			_surfacePhase = SurfaceAnimPhase.Releasing;
		}

		private void EnsureStretchStateReady()
		{
			if (!(_animator == null))
			{
				if (!_animator.isInitialized)
				{
					_animator.Rebind();
				}
				_animator.speed = 0f;
				_animator.Play(StretchStateHash, 0, _surfaceDisplayed);
				_animator.Update(0f);
			}
		}

		private void ApplySurfaceCompression(float compression01)
		{
			if (!(_animator == null))
			{
				float num = Mathf.Clamp01(compression01);
				_animator.SetFloat(CompressionHash, num);
				_animator.Play(StretchStateHash, 0, num);
				_animator.Update(0f);
			}
		}

		private float ResolveBounceHeight(int bounceIndexInChain)
		{
			return Mathf.Min(_configuration.InitialBounceHeight + _configuration.BounceHeightGrowthPerStep * (float)Mathf.Max(0, bounceIndexInChain), _configuration.MaxBounceHeight);
		}

		private static float HeightToUpSpeed(float heightMeters)
		{
			float num = Mathf.Abs(Physics.gravity.y);
			if (num < 0.01f)
			{
				num = 9.81f;
			}
			return Mathf.Sqrt(2f * num * Mathf.Max(0f, heightMeters));
		}

		private float MapHeightToCompression(float bounceHeightMeters)
		{
			float num = Mathf.Max(0.01f, _configuration.MaxBounceHeight);
			float num2 = Mathf.Clamp01(bounceHeightMeters / num);
			if (num2 > 0f && num2 < _configuration.MinVisualCompression)
			{
				return _configuration.MinVisualCompression;
			}
			return num2;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(2640329475u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlaySurfaceCompressionRpc_0040Invoker2640329475([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out float value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((TrampolineBehaviour)context.TargetBehaviour).PlaySurfaceCompressionRpc(value);
		}
	}
}
