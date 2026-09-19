using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using Features.VoiceOcclusionModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.KrakenModule.Scripts
{
	public class KrakenVisibilityAudioController : MonoBehaviour
	{
		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private EventReference _appearSound;

		[SerializeField]
		private EventReference _disappearSound;

		[SerializeField]
		private EventReference _idleLoopSound;

		private EventInstance _idleLoopInstance;

		private IAudioService _audioService;

		private PlayerMovableModel _playerMovableModel;

		private VoiceOcclusionConfiguration _voiceOcclusionConfiguration;

		private readonly RaycastHit[] _occlusionHits = new RaycastHit[16];

		[Inject]
		public void InjectDependencies(IAudioService audioService, PlayerMovableModel playerMovableModel, VoiceOcclusionConfiguration voiceOcclusionConfiguration)
		{
			_audioService = audioService;
			_playerMovableModel = playerMovableModel;
			_voiceOcclusionConfiguration = voiceOcclusionConfiguration;
		}

		private void Start()
		{
			StartIdleLoop();
		}

		public void InvokeOnAppear()
		{
			PlayOneShot(_appearSound);
			StartIdleLoop();
		}

		public void InvokeOnDisappear()
		{
			PlayOneShot(_disappearSound);
			StopIdleLoop();
		}

		private void Update()
		{
			if (_idleLoopInstance.isValid())
			{
				_idleLoopInstance.set3DAttributes(_soundSourceBehaviour.SoundSourceTransform.position.To3DAttributes());
				ProcessSoundOcclusion(_idleLoopInstance);
			}
		}

		private void OnDisable()
		{
			StopIdleLoop();
		}

		private void StartIdleLoop()
		{
			if (!_idleLoopSound.IsNull && !_idleLoopInstance.isValid())
			{
				_idleLoopInstance = _audioService.CreateInstance(_idleLoopSound);
				SetSoundImmediately(_idleLoopInstance);
				_audioService.StartInstanceWith3DAttributes(_idleLoopInstance, _soundSourceBehaviour);
			}
		}

		private void StopIdleLoop()
		{
			if (_idleLoopInstance.isValid())
			{
				_audioService.StopInstance(_idleLoopInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				_audioService.ReleaseInstance(_idleLoopInstance);
				_idleLoopInstance.clearHandle();
			}
		}

		private void PlayOneShot(EventReference reference)
		{
			if (!reference.IsNull)
			{
				EventInstance eventInstance = _audioService.CreateInstance(reference);
				SetSoundImmediately(eventInstance);
				_audioService.StartInstanceWith3DAttributes(eventInstance, _soundSourceBehaviour);
				_audioService.ReleaseInstance(eventInstance);
			}
		}

		private void ProcessSoundOcclusion(EventInstance eventInstance)
		{
			if (eventInstance.isValid() && !(_voiceOcclusionConfiguration == null) && !(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null) && !(_soundSourceBehaviour == null) && !(_soundSourceBehaviour.SoundSourceTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 vector = _soundSourceBehaviour.SoundSourceTransform.position - position;
				float magnitude = vector.magnitude;
				eventInstance.getParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, out var value);
				if (IsOccluded(position, vector, magnitude, _voiceOcclusionConfiguration.OcclusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(vector, _voiceOcclusionConfiguration.MaxDistance);
					float b = Mathf.Lerp(1f, _voiceOcclusionConfiguration.MinLowPassValue, normalizedOcclusionDistance);
					float value2 = Mathf.Lerp(value, b, Time.deltaTime * 5f);
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, value2);
				}
				else
				{
					float value3 = Mathf.Lerp(value, 1f, Time.deltaTime * 5f);
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, value3);
				}
			}
		}

		private void SetSoundImmediately(EventInstance eventInstance)
		{
			if (!(_voiceOcclusionConfiguration == null) && !(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null) && !(_soundSourceBehaviour == null) && !(_soundSourceBehaviour.SoundSourceTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 vector = _soundSourceBehaviour.SoundSourceTransform.position - position;
				float magnitude = vector.magnitude;
				if (IsOccluded(position, vector, magnitude, _voiceOcclusionConfiguration.OcclusionLayerMask))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(vector, _voiceOcclusionConfiguration.MaxDistance);
					float value = Mathf.Lerp(1f, _voiceOcclusionConfiguration.MinLowPassValue, normalizedOcclusionDistance);
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, value);
				}
				else
				{
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, 1f);
				}
			}
		}

		private float GetWeightedOcclusionDistance(Vector3 offset)
		{
			float magnitude = new Vector2(offset.x, offset.z).magnitude;
			float num = Mathf.Abs(offset.y) * _voiceOcclusionConfiguration.VerticalDistanceMultiplier;
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
			float t = Mathf.Abs(offset.y) * _voiceOcclusionConfiguration.VerticalDistanceMultiplier / weightedOcclusionDistance;
			float p = Mathf.Lerp(1f, 1f / Mathf.Max(0.01f, _voiceOcclusionConfiguration.VerticalFalloffExponent), t);
			return Mathf.Pow(num, p);
		}

		private bool IsOccluded(Vector3 listenerPosition, Vector3 direction, float distance, LayerMask occlusionMask)
		{
			if (distance <= Mathf.Epsilon)
			{
				return false;
			}
			int num = Physics.RaycastNonAlloc(listenerPosition, direction.normalized, _occlusionHits, distance, occlusionMask, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				if (!IsBeachInteractableHit(_occlusionHits[i].collider))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsBeachInteractableHit(Collider hitCollider)
		{
			Transform parent = hitCollider.transform;
			while (parent != null)
			{
				if (parent.CompareTag("BeachInteractable"))
				{
					return true;
				}
				parent = parent.parent;
			}
			return false;
		}
	}
}
