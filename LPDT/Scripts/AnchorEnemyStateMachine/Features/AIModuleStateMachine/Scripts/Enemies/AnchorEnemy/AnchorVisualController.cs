using System;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using Features.AIModuleStateMachine.Scripts.Core.Damageable;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class AnchorVisualController : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface
	{
		private static readonly int _prepareOnThrowHash = Animator.StringToHash("PrepareOnThrow");

		private static readonly int _anchorThrowingHash = Animator.StringToHash("AnchorThrowing");

		private static readonly int _throwUpHash = Animator.StringToHash("ThrowUp");

		private static readonly int _throwDownHash = Animator.StringToHash("ThrowDown");

		private static readonly int _isGrabbingHash = Animator.StringToHash("IsGrabbing");

		private static readonly int _playerThrowingHash = Animator.StringToHash("PlayerThrowing");

		private static readonly int _meleeAttackHash = Animator.StringToHash("MeleAttack");

		private static readonly int _moveSpeedNormalizedHash = Animator.StringToHash("MoveSpeedNormalized");

		private static readonly int _movementAnimSpeedHash = Animator.StringToHash("MovementAnimSpeed");

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private GameObject _animationAnchor;

		[SerializeField]
		private SoundSourceBehaviour _soundSource;

		[SerializeField]
		private EventReference _anchorThrowSound;

		[SerializeField]
		private EventReference _anchorChaseStart;

		[SerializeField]
		private EventReference _anchorPickupSound;

		[SerializeField]
		private EventReference _damageSound;

		[SerializeField]
		private SimpleEnemyDamageable[] _damageables;

		[SerializeField]
		private float _movementAnimSpeedPerUnitMoveSpeed = 1.5f;

		[SerializeField]
		private LayerMask _occlusionLayerMask;

		[SerializeField]
		private float _occlusionMaxDistance = 3f;

		[SerializeField]
		private float _verticalDistanceMultiplier = 3f;

		[SerializeField]
		private float _verticalFalloffExponent = 2f;

		[SerializeField]
		private float _lowPassMinValue = 0.35f;

		[SerializeField]
		private string _voiceOcclusionLowPassParameterName = "VoiceOcclusionLowPass";

		private AnchorEnemyContext _context;

		private IAudioService _audioService;

		private PlayerMovableModel _playerMovableModel;

		private AnchorVisualState _lastState;

		private bool _animationAnchorHidden;

		private bool _isSubscribedToDamage;

		[Inject]
		public void InjectDependencies(AnchorEnemyContext context, IAudioService audioService, PlayerMovableModel playerMovableModel)
		{
			_context = context;
			_audioService = audioService;
			_playerMovableModel = playerMovableModel;
		}

		public override void Spawned()
		{
			if (base.HasStateAuthority)
			{
				SubscribeDamageables();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			UnsubscribeDamageables();
			base.Despawned(runner, hasState);
		}

		public void StateAuthorityChanged()
		{
			if (base.HasStateAuthority)
			{
				SubscribeDamageables();
			}
			else
			{
				UnsubscribeDamageables();
			}
		}

		public override void Render()
		{
			if (!(_context == null) && !(base.Object == null) && base.Object.IsValid)
			{
				UpdateLocomotion();
				UpdateAnchorSwap();
				AnchorVisualState visualState = _context.VisualState;
				if (visualState != _lastState)
				{
					ApplyAnimatorState(visualState);
					PlayStateTransitionSounds(visualState);
					_lastState = visualState;
				}
			}
		}

		private void UpdateLocomotion()
		{
			if (!(_animator == null))
			{
				float moveSpeed = _context.MoveSpeed;
				float value = ((moveSpeed > 0f) ? Mathf.Clamp01(_context.SmoothedVelocity / moveSpeed) : 0f);
				_animator.SetFloat(_moveSpeedNormalizedHash, value);
				float value2 = ((moveSpeed > 0.01f) ? (moveSpeed * _movementAnimSpeedPerUnitMoveSpeed) : 1f);
				_animator.SetFloat(_movementAnimSpeedHash, value2);
			}
		}

		private void UpdateAnchorSwap()
		{
			if (_animationAnchor == null)
			{
				return;
			}
			bool isAnchorThrown = _context.IsAnchorThrown;
			if (isAnchorThrown != _animationAnchorHidden)
			{
				bool animationAnchorHidden = _animationAnchorHidden;
				_animationAnchorHidden = isAnchorThrown;
				_animationAnchor.SetActive(!isAnchorThrown);
				if (animationAnchorHidden && !isAnchorThrown)
				{
					PlayOneShot(_anchorPickupSound);
				}
			}
		}

		private void PlayStateTransitionSounds(AnchorVisualState state)
		{
			if (state == AnchorVisualState.ThrowUp)
			{
				PlayOneShot(_anchorThrowSound);
			}
			if (state == AnchorVisualState.ThrowAnchor)
			{
				PlayOneShot(_anchorThrowSound);
			}
			if (state == AnchorVisualState.Chase)
			{
				PlayOneShot(_anchorChaseStart);
			}
		}

		private void ApplyAnimatorState(AnchorVisualState state)
		{
			if (!(_animator == null))
			{
				bool value = false;
				bool value2 = false;
				bool value3 = false;
				bool value4 = false;
				bool value5 = false;
				bool value6 = false;
				bool value7 = false;
				switch (state)
				{
				case AnchorVisualState.ThrowAnchor:
					value = true;
					value2 = true;
					break;
				case AnchorVisualState.ThrowUp:
					value3 = true;
					break;
				case AnchorVisualState.Reel:
				case AnchorVisualState.ThrowDown:
					value4 = true;
					break;
				case AnchorVisualState.GrabPlayer:
					value5 = true;
					break;
				case AnchorVisualState.ReleasePlayer:
					value6 = true;
					break;
				case AnchorVisualState.MeleeAttack:
					value7 = true;
					break;
				}
				_animator.SetBool(_prepareOnThrowHash, value);
				_animator.SetBool(_anchorThrowingHash, value2);
				_animator.SetBool(_throwUpHash, value3);
				_animator.SetBool(_throwDownHash, value4);
				_animator.SetBool(_isGrabbingHash, value5);
				_animator.SetBool(_playerThrowingHash, value6);
				_animator.SetBool(_meleeAttackHash, value7);
			}
		}

		private void SubscribeDamageables()
		{
			if (_isSubscribedToDamage || _damageables == null)
			{
				return;
			}
			SimpleEnemyDamageable[] damageables = _damageables;
			foreach (SimpleEnemyDamageable simpleEnemyDamageable in damageables)
			{
				if (simpleEnemyDamageable != null)
				{
					simpleEnemyDamageable.OnDamaged += OnDamaged;
				}
			}
			_isSubscribedToDamage = true;
		}

		private void UnsubscribeDamageables()
		{
			if (!_isSubscribedToDamage || _damageables == null)
			{
				return;
			}
			SimpleEnemyDamageable[] damageables = _damageables;
			foreach (SimpleEnemyDamageable simpleEnemyDamageable in damageables)
			{
				if (simpleEnemyDamageable != null)
				{
					simpleEnemyDamageable.OnDamaged -= OnDamaged;
				}
			}
			_isSubscribedToDamage = false;
		}

		private void OnDamaged(DamageData damageData)
		{
			if (base.HasStateAuthority && !(base.Object == null) && base.Object.IsValid)
			{
				RpcPlayDamageSound();
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = true, Key = 513558126u)]
		private void RpcPlayDamageSound()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(513558126u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.AnchorVisualController::RpcPlayDamageSound()", invokeInfo, PlayerRef.None);
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
			PlayOneShot(_damageSound);
		}

		private void PlayOneShot(EventReference reference)
		{
			if (_audioService != null && !reference.IsNull && !(_soundSource == null))
			{
				EventInstance eventInstance = _audioService.CreateInstance(reference);
				SetSoundImmediately(eventInstance);
				_audioService.StartInstanceWith3DAttributes(eventInstance, _soundSource);
				_audioService.ReleaseInstance(eventInstance);
			}
		}

		private float GetWeightedOcclusionDistance(Vector3 offset)
		{
			float magnitude = new Vector2(offset.x, offset.z).magnitude;
			float num = Mathf.Abs(offset.y) * _verticalDistanceMultiplier;
			return Mathf.Sqrt(magnitude * magnitude + num * num);
		}

		private float GetNormalizedOcclusionDistance(Vector3 offset, float maxDistance)
		{
			float weightedOcclusionDistance = GetWeightedOcclusionDistance(offset);
			float num = Mathf.Clamp01(weightedOcclusionDistance / Mathf.Max(0.01f, maxDistance));
			if (weightedOcclusionDistance <= Mathf.Epsilon)
			{
				return num;
			}
			float t = Mathf.Abs(offset.y) * _verticalDistanceMultiplier / weightedOcclusionDistance;
			float p = Mathf.Lerp(1f, 1f / Mathf.Max(0.01f, _verticalFalloffExponent), t);
			return Mathf.Pow(num, p);
		}

		private void SetSoundImmediately(EventInstance eventInstance)
		{
			if (!(_playerMovableModel?.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 offset = ((_soundSource.SoundSourceTransform != null) ? _soundSource.SoundSourceTransform.position : base.transform.position) - position;
				float magnitude = offset.magnitude;
				if (Physics.Raycast(position, offset.normalized, out var _, magnitude, _occlusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(offset, _occlusionMaxDistance);
					float value = Mathf.Lerp(1f, _lowPassMinValue, normalizedOcclusionDistance);
					eventInstance.setParameterByName(_voiceOcclusionLowPassParameterName, value);
				}
				else
				{
					eventInstance.setParameterByName(_voiceOcclusionLowPassParameterName, 1f);
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

		[NetworkRpcWeavedInvoker(513558126u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RpcPlayDamageSound_0040Invoker513558126([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((AnchorVisualController)context.TargetBehaviour).RpcPlayDamageSound();
		}
	}
}
