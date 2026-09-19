using System.Collections;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.GrabModule.Scripts;
using Features.Movement.Scripts;
using Features.VoiceOcclusionModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.GuillotineModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class GuillotineActivationController : NetworkBehaviour
	{
		[SerializeField]
		private GuillotineBehaviour _guillotineBehaviour;

		[SerializeField]
		private SimplePointGrabable _leverGrabbable;

		[SerializeField]
		private float _activationRotationThreshold = 60f;

		[SerializeField]
		private float _deactivationDelay = 2f;

		[SerializeField]
		private EventReference _guillotineActivationEvent;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private GuillotineInitializeController _guillotineInitializeController;

		private Quaternion _initialLeverRotation;

		private bool _isActivated;

		private bool _isArmed = true;

		private bool _isInitialized;

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

		public override void Spawned()
		{
			base.Spawned();
			if (_guillotineInitializeController.IsGuillotineInitialized)
			{
				InitializeGuillotineActivation();
			}
			else
			{
				_guillotineInitializeController.OnGuillotineInitialized += InitializeGuillotineActivation;
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_guillotineInitializeController.OnGuillotineInitialized -= InitializeGuillotineActivation;
			StopAllCoroutines();
		}

		private void InitializeGuillotineActivation()
		{
			_guillotineInitializeController.OnGuillotineInitialized -= InitializeGuillotineActivation;
			_initialLeverRotation = _leverGrabbable.Rigidbody.rotation;
			_isArmed = true;
			_isInitialized = true;
			ResetGuillotine(0f);
		}

		private void Update()
		{
			if (_isInitialized && !_isActivated)
			{
				if (Quaternion.Angle(_initialLeverRotation, _leverGrabbable.Rigidbody.rotation) < _activationRotationThreshold)
				{
					_isArmed = true;
				}
				else if (_isArmed)
				{
					_isArmed = false;
					_guillotineBehaviour.ActivateGuillotine();
					_isActivated = true;
					PlayOneShot(_guillotineActivationEvent);
					ResetGuillotine(_deactivationDelay);
				}
			}
		}

		private void ResetGuillotine(float deactivationDelay)
		{
			StartCoroutine(GuillotineDeactivationRoutine(deactivationDelay, _guillotineBehaviour.DeactivationAnimationTime));
		}

		private IEnumerator GuillotineDeactivationRoutine(float deactivationDelay, float deactivationTime)
		{
			yield return new WaitForSeconds(deactivationDelay);
			_guillotineBehaviour.DeactivateGuillotine();
			_leverGrabbable.GrabObject.GrabbingPhysicsBlocked = true;
			float levelDeactivationTimer = 0f;
			Quaternion currentLeverRotation = _leverGrabbable.Rigidbody.rotation;
			while (levelDeactivationTimer < deactivationTime)
			{
				float t = Mathf.Clamp01(levelDeactivationTimer / deactivationTime);
				Quaternion rotation = Quaternion.Lerp(currentLeverRotation, _initialLeverRotation, t);
				if (_leverGrabbable.NetworkObject.HasStateAuthority)
				{
					_leverGrabbable.Rigidbody.rotation = rotation;
				}
				levelDeactivationTimer += Time.deltaTime;
				yield return null;
			}
			if (_leverGrabbable.NetworkObject.HasStateAuthority)
			{
				_leverGrabbable.Rigidbody.rotation = _initialLeverRotation;
			}
			_isActivated = false;
			_leverGrabbable.GrabObject.GrabbingPhysicsBlocked = false;
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

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
