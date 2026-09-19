using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.GrabModule.Scripts;
using Features.Movement.Scripts;
using Features.VoiceOcclusionModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.BasketballHoopModule.Scripts
{
	public sealed class BasketballHoopRingPassController : MonoBehaviour
	{
		[SerializeField]
		private BasketballHoopTriggerZone _aboveRing;

		[SerializeField]
		private BasketballHoopTriggerZone _insideRing;

		[SerializeField]
		private BasketballHoopTriggerZone _belowRing;

		[SerializeField]
		private ParticleSystem _swishVfx;

		[SerializeField]
		private EventReference _swishSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private readonly HashSet<Transform> _overlapAbove = new HashSet<Transform>();

		private readonly HashSet<Transform> _overlapInside = new HashSet<Transform>();

		private readonly HashSet<Transform> _overlapBelow = new HashSet<Transform>();

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

		private void Awake()
		{
			SetInsideColliderActive(enabled: false);
			if (_belowRing != null)
			{
				_belowRing.SetColliderEnabled(enable: false);
			}
		}

		private void OnEnable()
		{
			_aboveRing.BallEntered += OnBallEntered;
			_aboveRing.BallExited += OnBallExited;
			_insideRing.BallEntered += OnBallEntered;
			_insideRing.BallExited += OnBallExited;
			_belowRing.BallEntered += OnBallEntered;
			_belowRing.BallExited += OnBallExited;
		}

		private void OnDisable()
		{
			_aboveRing.BallEntered -= OnBallEntered;
			_aboveRing.BallExited -= OnBallExited;
			_insideRing.BallEntered -= OnBallEntered;
			_insideRing.BallExited -= OnBallExited;
			_belowRing.BallEntered -= OnBallEntered;
			_belowRing.BallExited -= OnBallExited;
		}

		private void OnBallEntered(BasketballHoopTriggerBand band, Collider other)
		{
			Transform ballRoot = GetBallRoot(other);
			switch (band)
			{
			case BasketballHoopTriggerBand.AboveRing:
				_overlapAbove.Add(ballRoot);
				SetInsideColliderActive(enabled: true);
				break;
			case BasketballHoopTriggerBand.InsideRing:
				_overlapInside.Add(ballRoot);
				if (_belowRing != null)
				{
					_belowRing.SetColliderEnabled(enable: true);
				}
				break;
			case BasketballHoopTriggerBand.BelowRing:
				_overlapBelow.Add(ballRoot);
				break;
			}
		}

		private void OnBallExited(BasketballHoopTriggerBand band, Collider other)
		{
			Transform ballRoot = GetBallRoot(other);
			switch (band)
			{
			case BasketballHoopTriggerBand.AboveRing:
				_overlapAbove.Remove(ballRoot);
				break;
			case BasketballHoopTriggerBand.InsideRing:
				if (_overlapInside.Remove(ballRoot) && _overlapBelow.Contains(ballRoot) && !IsGrabbedByAnyPlayer(ballRoot))
				{
					TryRegisterHoopPass(ballRoot);
				}
				break;
			case BasketballHoopTriggerBand.BelowRing:
				_overlapBelow.Remove(ballRoot);
				break;
			}
		}

		private Transform GetBallRoot(Collider other)
		{
			Rigidbody attachedRigidbody = other.attachedRigidbody;
			if (!(attachedRigidbody != null))
			{
				return other.transform;
			}
			return attachedRigidbody.transform;
		}

		private bool IsGrabbedByAnyPlayer(Transform ballRoot)
		{
			if (!ballRoot.TryGetComponent<IPointGrabable>(out var component))
			{
				component = ballRoot.GetComponentInParent<IPointGrabable>();
			}
			if (component != null)
			{
				return component.GrabbedByPlayers.Count > 0;
			}
			return false;
		}

		private void TryRegisterHoopPass(Transform ballRoot)
		{
			PlaySwishFeedback();
			ResetSequenceAfterScore();
			if (_overlapAbove.Contains(ballRoot))
			{
				SetInsideColliderActive(enabled: true);
			}
		}

		private void ResetSequenceAfterScore()
		{
			_overlapInside.Clear();
			_overlapBelow.Clear();
			SetInsideColliderActive(enabled: false);
			_belowRing.SetColliderEnabled(enable: false);
		}

		private void SetInsideColliderActive(bool enabled)
		{
			_insideRing.SetColliderEnabled(enabled);
		}

		private void PlaySwishFeedback()
		{
			if (!_swishSound.IsNull)
			{
				PlayOneShot(_swishSound);
			}
			if (!(_swishVfx == null))
			{
				_swishVfx.Play();
			}
		}

		private void PlayOneShot(EventReference reference)
		{
			if (!(_soundSourceBehaviour == null) && !reference.IsNull)
			{
				EventInstance eventInstance = _audioService.CreateInstance(reference);
				SetSoundImmediately(eventInstance);
				_audioService.StartInstanceWith3DAttributes(eventInstance, _soundSourceBehaviour);
				_audioService.ReleaseInstance(eventInstance);
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
