using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using Features.VoiceOcclusionModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.GuillotineModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class GuillotineBehaviour : NetworkBehaviour
	{
		private static readonly int IsActivatedHash = Animator.StringToHash("IsActivated");

		private static readonly int MAX_RUNTIME_EXECUTABLES_TO_PROCESS = 100;

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private GuillotineAnimationFunctionReactor _animationFunctionReactor;

		[SerializeField]
		private AnimationClip _deactivationAnimation;

		[SerializeField]
		private BoxCollider _executionAreaCollider;

		[SerializeField]
		private LayerMask _executeLayerMask;

		[SerializeField]
		private EventReference _onDownSound;

		[SerializeField]
		private EventReference _onUpSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private GuillotineExecuteDataHolder _guillotineExecuteDataHolder;

		private IAudioService _audioService;

		private PlayerMovableModel _playerMovableModel;

		private VoiceOcclusionConfiguration _voiceOcclusionConfiguration;

		private readonly RaycastHit[] _occlusionHits = new RaycastHit[16];

		private bool _bladeSoundsEnabled;

		private readonly Collider[] _runtimeExecutablesBuffer = new Collider[MAX_RUNTIME_EXECUTABLES_TO_PROCESS];

		private readonly HashSet<GameObject> _runtimeExecutableGameObjects = new HashSet<GameObject>();

		public float DeactivationAnimationTime => _deactivationAnimation.length;

		[Inject]
		public void InjectDependencies(GuillotineExecuteDataHolder guillotineExecuteDataHolder, IAudioService audioService, PlayerMovableModel playerMovableModel, VoiceOcclusionConfiguration voiceOcclusionConfiguration)
		{
			_guillotineExecuteDataHolder = guillotineExecuteDataHolder;
			_audioService = audioService;
			_playerMovableModel = playerMovableModel;
			_voiceOcclusionConfiguration = voiceOcclusionConfiguration;
		}

		public override void Spawned()
		{
			base.Spawned();
			_animationFunctionReactor.OnExecuted += ExecuteDamageablesInRange;
			_animationFunctionReactor.OnDown += PlayBladeDownSound;
			_animationFunctionReactor.OnUp += PlayBladeUpSound;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_animationFunctionReactor.OnExecuted -= ExecuteDamageablesInRange;
			_animationFunctionReactor.OnDown -= PlayBladeDownSound;
			_animationFunctionReactor.OnUp -= PlayBladeUpSound;
		}

		private void PlayBladeUpSound()
		{
			if (_bladeSoundsEnabled)
			{
				PlayOneShot(_onUpSound);
			}
		}

		private void PlayBladeDownSound()
		{
			if (_bladeSoundsEnabled)
			{
				PlayOneShot(_onDownSound);
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

		public void ActivateGuillotine()
		{
			_bladeSoundsEnabled = true;
			_animator.SetBool(IsActivatedHash, value: true);
		}

		public void DeactivateGuillotine()
		{
			_animator.SetBool(IsActivatedHash, value: false);
		}

		private void ExecuteDamageablesInRange()
		{
			foreach (IGuillotineExecutable item in _guillotineExecuteDataHolder.GuillotineExecutablesInRange)
			{
				if (!(item?.NetworkObject == null) && item.NetworkObject.IsValid)
				{
					if (item.NetworkObject.HasStateAuthority)
					{
						item.Execute();
					}
					item.PlayExecutionSound();
				}
			}
			int num = Physics.OverlapBoxNonAlloc(_executionAreaCollider.transform.position, _executionAreaCollider.size / 2f, _runtimeExecutablesBuffer, _executionAreaCollider.transform.rotation, _executeLayerMask);
			_runtimeExecutableGameObjects.Clear();
			for (int i = 0; i < num; i++)
			{
				Collider collider = _runtimeExecutablesBuffer[i];
				if (!(collider == null) && _runtimeExecutableGameObjects.Add(collider.gameObject) && collider.TryGetComponent<IGuillotineExecutable>(out var component) && !(component?.NetworkObject == null) && component.NetworkObject.IsValid)
				{
					component.PlayExecutionSound();
					if (component.IsRuntime && component.NetworkObject.HasStateAuthority)
					{
						component.Execute();
					}
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
