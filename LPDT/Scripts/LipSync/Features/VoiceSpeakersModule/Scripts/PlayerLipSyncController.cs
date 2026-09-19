using System;
using System.Collections.Generic;
using Features.AnimationModule.Scripts;
using Features.AudioDevicesModule.Scripts;
using Features.AudioServiceModule.Scripts;
using Features.EntitiesSoundOcclusionModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using Photon.Voice.Unity;
using UnityEngine;
using Zenject;

namespace Features.VoiceSpeakersModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class PlayerLipSyncController : NetworkBehaviour
	{
		private const float VOICE_OCCLUSION_INTERVAL = 0.15f;

		private int _tongueStrengthHash = Animator.StringToHash("TongueStrength");

		[SerializeField]
		private ParticleSystem _waterParticle;

		[SerializeField]
		private List<SkinnedMeshRenderer> _skinnedMeshRenderer;

		[SerializeField]
		private CompositeAnimator _animator;

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

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private MultiplayerModel _multiplayerModel;

		private float _previousRawVolume;

		private float _fallTimer;

		private bool _isFalling;

		private float _smoothedVolume;

		private float _targetVolume;

		private IPlayerStateService _playerStateService;

		private MicrophoneModel _microphoneModel;

		private IEntitiesSoundOcclusionService _entitiesSoundOcclusionService;

		private EntitiesSoundOcclusionModel _entitiesSoundOcclusionModel;

		private EntitiesSoundOcclusionConfiguration _entitiesSoundOcclusionConfiguration;

		private float _voiceOcclusionTimer;

		private bool _isSpawned;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CurrentVolume", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _CurrentVolume;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe int CurrentVolume
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLipSyncController.CurrentVolume. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerLipSyncController.CurrentVolume. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Inject]
		public void InjectDependencies(MultiplayerModel multiplayerModel, IPlayerStateService playerStateService, MicrophoneModel microphoneModel, IEntitiesSoundOcclusionService entitiesSoundOcclusionService, EntitiesSoundOcclusionModel entitiesSoundOcclusionModel, EntitiesSoundOcclusionConfiguration entitiesSoundOcclusionConfiguration)
		{
			_multiplayerModel = multiplayerModel;
			_playerStateService = playerStateService;
			_microphoneModel = microphoneModel;
			_entitiesSoundOcclusionService = entitiesSoundOcclusionService;
			_entitiesSoundOcclusionModel = entitiesSoundOcclusionModel;
			_entitiesSoundOcclusionConfiguration = entitiesSoundOcclusionConfiguration;
		}

		public override void Spawned()
		{
			base.Spawned();
			_isSpawned = true;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_isSpawned = false;
		}

		private void Update()
		{
			if (!_isSpawned || base.Object == null || _multiplayerModel.NetworkRunner == null || _playerStateService.IsPlayerDead(base.Object.InputAuthority.PlayerId))
			{
				return;
			}
			TryEmitVoiceOcclusion();
			foreach (SkinnedMeshRenderer item in _skinnedMeshRenderer)
			{
				item.SetBlendShapeWeight(3, CurrentVolume);
			}
			if (_multiplayerModel.NetworkRunner.LocalPlayer != base.Object.InputAuthority)
			{
				if ((float)CurrentVolume < _waterParticleTrashHold)
				{
					if (!_waterParticle.isPlaying)
					{
						_waterParticle.Play();
					}
				}
				else
				{
					_waterParticle.Stop();
				}
			}
			_animator.SetFloat(_tongueStrengthHash, 1f - (float)CurrentVolume / 100f);
		}

		public override void FixedUpdateNetwork()
		{
			base.FixedUpdateNetwork();
			if (!base.HasInputAuthority)
			{
				return;
			}
			if (_playerStateService.IsPlayerDead(base.Object.InputAuthority.PlayerId))
			{
				ResetVoiceVolumeToSilence();
				return;
			}
			Recorder playerRecorder = _microphoneModel.PlayerRecorder;
			if (playerRecorder == null || playerRecorder.LevelMeter == null || !_microphoneModel.IsMicrophoneEnabled || !playerRecorder.RecordingEnabled || !playerRecorder.TransmitEnabled)
			{
				ResetVoiceVolumeToSilence();
				return;
			}
			float num = playerRecorder.LevelMeter.CurrentAvgAmp * _volumeMultiplier;
			if (num < _previousRawVolume)
			{
				_isFalling = true;
				_fallTimer = 0f;
			}
			if (_isFalling)
			{
				_fallTimer += base.Runner.DeltaTime;
				float num2 = Mathf.Clamp01(_fallTimer / _fallDuration);
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
			if (Mathf.Abs(num4 - CurrentVolume) >= _volumeChangeTolerance)
			{
				CurrentVolume = num4;
			}
			_previousRawVolume = num;
		}

		private void ResetVoiceVolumeToSilence()
		{
			_previousRawVolume = 0f;
			_fallTimer = 0f;
			_isFalling = false;
			_smoothedVolume = 0f;
			_targetVolume = 0f;
			if (CurrentVolume != _silenceClampValue)
			{
				CurrentVolume = _silenceClampValue;
			}
		}

		private void TryEmitVoiceOcclusion()
		{
			if (!_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			_voiceOcclusionTimer += Time.deltaTime;
			if (!(_voiceOcclusionTimer < 0.15f))
			{
				_voiceOcclusionTimer = 0f;
				float num = 1f - Mathf.Clamp01((float)CurrentVolume / 100f);
				float voiceDistanceMultiplier = _entitiesSoundOcclusionModel.GetVoiceDistanceMultiplier(base.Object.InputAuthority.PlayerId);
				float num2 = num * _entitiesSoundOcclusionConfiguration.MaxVoiceHearingDistance * voiceDistanceMultiplier;
				float minVoiceLoudnessForEntityOcclusion = _entitiesSoundOcclusionConfiguration.MinVoiceLoudnessForEntityOcclusion;
				if (num >= minVoiceLoudnessForEntityOcclusion && num2 > 0f)
				{
					_entitiesSoundOcclusionService.TriggerOcclusionForEntities(base.transform.position, num2, _soundSourceBehaviour);
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			CurrentVolume = _CurrentVolume;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_CurrentVolume = CurrentVolume;
		}
	}
}
