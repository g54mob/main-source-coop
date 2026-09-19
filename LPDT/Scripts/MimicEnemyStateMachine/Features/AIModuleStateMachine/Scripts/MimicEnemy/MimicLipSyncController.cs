using System;
using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.MimicEnemy.Settings;
using Features.AnimationModule.Scripts;
using Features.VoiceSpeakersModule.Scripts.MimicVoice;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy
{
	[NetworkBehaviourWeaved(2)]
	public class MimicLipSyncController : NetworkBehaviour
	{
		private const int OPEN_AMOUNT_CHANGE_TOLERANCE = 3;

		private const float TONGUE_STRENGTH_WHEN_CLOSED = 0f;

		private const float TONGUE_STRENGTH_WHEN_OPEN = 1f;

		private static readonly int TongueStrengthHash = Animator.StringToHash("TongueStrength");

		[SerializeField]
		private MimicFacialAnimationSettings _settings;

		[SerializeField]
		private CompositeAnimator _animator;

		[SerializeField]
		private List<SkinnedMeshRenderer> _skinnedMeshRenderers = new List<SkinnedMeshRenderer>();

		[SerializeField]
		private ParticleSystem _waterParticle;

		[SerializeField]
		private float _volumeMultiplier = 40f;

		[SerializeField]
		private float _fallDuration = 0.2f;

		[SerializeField]
		private float _openSpeed = 10f;

		[SerializeField]
		private float _closeSpeed = 15f;

		[SerializeField]
		private float _waterParticleTrashHold = 20f;

		[SerializeField]
		private int _silenceFloorThreshold = 92;

		[SerializeField]
		private int _silenceClampValue = 99;

		[SerializeField]
		private int _volumeChangeTolerance = 3;

		private IMimicVoicePlaybackService _mimicVoicePlaybackService;

		private bool _isSpawned;

		private float _previousRawVolume;

		private float _fallTimer;

		private bool _isFalling;

		private float _smoothedVolume;

		private float _targetVolume;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CurrentOpenAmount", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _CurrentOpenAmount;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsMouthOpenWithTongue", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsMouthOpenWithTongue;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe int CurrentOpenAmount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicLipSyncController.CurrentOpenAmount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicLipSyncController.CurrentOpenAmount. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe NetworkBool IsMouthOpenWithTongue
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicLipSyncController.IsMouthOpenWithTongue. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 1);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MimicLipSyncController.IsMouthOpenWithTongue. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = value;
			}
		}

		[Inject]
		private void InjectDependencies(IMimicVoicePlaybackService mimicVoicePlaybackService)
		{
			_mimicVoicePlaybackService = mimicVoicePlaybackService;
		}

		public override void Spawned()
		{
			base.Spawned();
			_isSpawned = true;
			if (base.HasStateAuthority)
			{
				CurrentOpenAmount = 0;
				IsMouthOpenWithTongue = false;
			}
			ApplyMouthState();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_isSpawned = false;
			StopWaterParticle();
			base.Despawned(runner, hasState);
		}

		public void SetMouthVolumeFromNormalized(float normalizedOpenAmount)
		{
			if (_settings.EnableLipSync && base.HasStateAuthority)
			{
				IsMouthOpenWithTongue = false;
				int num = Mathf.RoundToInt(Mathf.Clamp01(normalizedOpenAmount) * 100f);
				if (Mathf.Abs(num - CurrentOpenAmount) >= 3)
				{
					CurrentOpenAmount = num;
				}
			}
		}

		public void SetMouthOpenWithTongue()
		{
			if (_settings.EnableLipSync && base.HasStateAuthority)
			{
				IsMouthOpenWithTongue = true;
				CurrentOpenAmount = 100;
				ResetVoiceLipSyncState();
			}
		}

		public void SetMouthClosed()
		{
			if (_settings.EnableLipSync && base.HasStateAuthority)
			{
				IsMouthOpenWithTongue = false;
				CurrentOpenAmount = 0;
				ResetVoiceLipSyncState();
			}
		}

		public override void FixedUpdateNetwork()
		{
			base.FixedUpdateNetwork();
			if (_isSpawned && _settings.EnableLipSync && base.HasStateAuthority && !IsMouthOpenWithTongue)
			{
				float loudness = 0f;
				if (_mimicVoicePlaybackService != null)
				{
					_mimicVoicePlaybackService.TryGetNormalizedLoudness(base.transform, out loudness);
				}
				UpdateMouthFromLoudness(loudness);
			}
		}

		private void Update()
		{
			if (_isSpawned)
			{
				ApplyMouthState();
				UpdateWaterParticle();
			}
		}

		private void ApplyMouthState()
		{
			if ((bool)IsMouthOpenWithTongue)
			{
				ApplyClosedMouthBlendShapesToZero();
				ApplyTongueAnimation(1f);
			}
			else
			{
				ApplyTongueAnimation(0f);
				ApplyMouthOpenAmount(CurrentOpenAmount);
			}
		}

		private void UpdateWaterParticle()
		{
			if (_waterParticle == null)
			{
				return;
			}
			if ((float)((!IsMouthOpenWithTongue) ? (100 - CurrentOpenAmount) : 0) < _waterParticleTrashHold)
			{
				if (!_waterParticle.isPlaying)
				{
					_waterParticle.Play();
				}
			}
			else
			{
				StopWaterParticle();
			}
		}

		private void StopWaterParticle()
		{
			if (_waterParticle != null && _waterParticle.isPlaying)
			{
				_waterParticle.Stop();
			}
		}

		private void ApplyMouthOpenAmount(int openAmount)
		{
			float t = (float)openAmount / 100f;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in _skinnedMeshRenderers)
			{
				ReadOnlySpan<MouthBlendShapeWeight> closedMouthBlendShapes = _settings.ClosedMouthBlendShapes;
				for (int i = 0; i < closedMouthBlendShapes.Length; i++)
				{
					MouthBlendShapeWeight mouthBlendShapeWeight = closedMouthBlendShapes[i];
					float value = Mathf.Lerp(mouthBlendShapeWeight.ClosedWeight, 0f, t);
					skinnedMeshRenderer.SetBlendShapeWeight(mouthBlendShapeWeight.Index, value);
				}
			}
		}

		private void ApplyClosedMouthBlendShapesToZero()
		{
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in _skinnedMeshRenderers)
			{
				ReadOnlySpan<MouthBlendShapeWeight> closedMouthBlendShapes = _settings.ClosedMouthBlendShapes;
				for (int i = 0; i < closedMouthBlendShapes.Length; i++)
				{
					MouthBlendShapeWeight mouthBlendShapeWeight = closedMouthBlendShapes[i];
					skinnedMeshRenderer.SetBlendShapeWeight(mouthBlendShapeWeight.Index, 0f);
				}
			}
		}

		private void ApplyTongueAnimation(float tongueStrength)
		{
			_animator.SetFloat(TongueStrengthHash, tongueStrength);
			_animator.SetLayerWeight("TongueLayer", (tongueStrength > 0f) ? 1f : 0f);
		}

		private void UpdateMouthFromLoudness(float normalizedLoudness)
		{
			float num = Mathf.Clamp01(normalizedLoudness) * _volumeMultiplier;
			if (num < _previousRawVolume)
			{
				_isFalling = true;
				_fallTimer = 0f;
			}
			if (_isFalling)
			{
				_fallTimer += base.Runner.DeltaTime;
				float num2 = Mathf.Clamp01(_fallTimer / Mathf.Max(0.001f, _fallDuration));
				_targetVolume = Mathf.Lerp(_previousRawVolume, 0f, num2);
				if (num2 >= 1f)
				{
					_isFalling = false;
				}
			}
			else
			{
				_targetVolume = num;
			}
			float num3 = ((_targetVolume > _smoothedVolume) ? _openSpeed : _closeSpeed);
			_smoothedVolume = Mathf.Lerp(_smoothedVolume, _targetVolume, base.Runner.DeltaTime * num3);
			int num4 = (int)((1f - Mathf.Clamp01(_smoothedVolume)) * 100f);
			if (num4 >= _silenceFloorThreshold)
			{
				num4 = _silenceClampValue;
			}
			int num5 = 100 - num4;
			if (Mathf.Abs(num5 - CurrentOpenAmount) >= _volumeChangeTolerance)
			{
				CurrentOpenAmount = num5;
			}
			_previousRawVolume = num;
		}

		private void ResetVoiceLipSyncState()
		{
			_previousRawVolume = 0f;
			_fallTimer = 0f;
			_isFalling = false;
			_smoothedVolume = 0f;
			_targetVolume = 0f;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			CurrentOpenAmount = _CurrentOpenAmount;
			IsMouthOpenWithTongue = _IsMouthOpenWithTongue;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_CurrentOpenAmount = CurrentOpenAmount;
			_IsMouthOpenWithTongue = IsMouthOpenWithTongue;
		}
	}
}
