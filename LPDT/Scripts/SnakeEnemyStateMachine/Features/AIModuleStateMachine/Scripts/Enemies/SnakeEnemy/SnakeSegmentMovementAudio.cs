using FMOD;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy
{
	public class SnakeSegmentMovementAudio : MonoBehaviour, ITransformBasedSoundSource, ISoundSource
	{
		[FormerlySerializedAs("_loopSound")]
		[SerializeField]
		private EventReference _movementSound;

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

		private IAudioService _audioService;

		private PlayerMovableModel _playerMovableModel;

		private EventInstance _instance;

		private bool _isPlaying;

		public bool IsPlaying => _isPlaying;

		public Transform SoundSourceTransform => base.transform;

		public Vector3 SourcePosition
		{
			get
			{
				if (!(SoundSourceTransform != null))
				{
					return Vector3.zero;
				}
				return SoundSourceTransform.position;
			}
		}

		public SoundSourceKind Kind => SoundSourceKind.Unknown;

		public int AttributedPlayerId => -1;

		[Inject]
		public void InjectDependencies(IAudioService audioService, PlayerMovableModel playerMovableModel)
		{
			_audioService = audioService;
			_playerMovableModel = playerMovableModel;
		}

		public void Play()
		{
			if (_isPlaying || _movementSound.IsNull || _audioService == null)
			{
				return;
			}
			_instance = _audioService.CreateInstance(_movementSound);
			if (_instance.isValid())
			{
				_instance.set3DAttributes(base.transform.position.To3DAttributes());
				ApplyOcclusion(smooth: false);
				if (_instance.start() != RESULT.OK)
				{
					_instance.release();
				}
				else
				{
					_isPlaying = true;
				}
			}
		}

		private void LateUpdate()
		{
			if (!_isPlaying)
			{
				return;
			}
			if (!_instance.isValid())
			{
				_isPlaying = false;
				return;
			}
			if (_instance.getPlaybackState(out var state) != RESULT.OK)
			{
				ReleaseQuietly();
				return;
			}
			switch (state)
			{
			case PLAYBACK_STATE.STOPPED:
				ReleaseQuietly();
				break;
			case PLAYBACK_STATE.STOPPING:
				break;
			default:
				_instance.set3DAttributes(base.transform.position.To3DAttributes());
				ApplyOcclusion(smooth: true);
				break;
			}
		}

		public void StopImmediate()
		{
			if (_isPlaying)
			{
				if (_instance.isValid())
				{
					_instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
					_instance.release();
				}
				_isPlaying = false;
			}
		}

		private void OnDisable()
		{
			StopImmediate();
		}

		private void OnDestroy()
		{
			StopImmediate();
		}

		private void ReleaseQuietly()
		{
			if (_instance.isValid())
			{
				_instance.release();
			}
			_isPlaying = false;
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

		private void ApplyOcclusion(bool smooth)
		{
			if (_instance.isValid() && !(_playerMovableModel?.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 offset = base.transform.position - position;
				float magnitude = offset.magnitude;
				float num = 1f;
				if (Physics.Raycast(position, offset.normalized, out var _, magnitude, _occlusionLayerMask, QueryTriggerInteraction.Ignore))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(offset, _occlusionMaxDistance);
					num = Mathf.Lerp(1f, _lowPassMinValue, normalizedOcclusionDistance);
				}
				float value;
				if (!smooth)
				{
					_instance.setParameterByName(_voiceOcclusionLowPassParameterName, num, ignoreseekspeed: true);
				}
				else if (_instance.getParameterByName(_voiceOcclusionLowPassParameterName, out value) == RESULT.OK)
				{
					float value2 = Mathf.Lerp(value, num, Time.deltaTime * 5f);
					_instance.setParameterByName(_voiceOcclusionLowPassParameterName, value2, ignoreseekspeed: true);
				}
			}
		}
	}
}
