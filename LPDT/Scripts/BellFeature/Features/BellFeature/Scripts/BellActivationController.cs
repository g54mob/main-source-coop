using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.QuotaModule.Scripts;
using Features.ScreenShakeModule.Scripts;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Features.BellFeature.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class BellActivationController : NetworkBehaviour
	{
		private static readonly int IsBellActive = Animator.StringToHash("IsBellActive");

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private CinemachineImpulseSource _cinemachineImpulseSource;

		[SerializeField]
		private float _activationTime;

		[SerializeField]
		private float _deactivationTime = 0.1f;

		[SerializeField]
		private List<GameObject> _objectsToActivate;

		[SerializeField]
		private List<GameObject> _objectsToDeactivate;

		[SerializeField]
		private EventReference _bellActivationSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private ScreenShakeData _activationScreenShakeData;

		private QuotaCompletionModel _quotaCompletionModel;

		private QuotaSynchronizedModel _quotaSynchronizedModel;

		private IScreenShakeService _screenShakeService;

		private IAudioService _audioService;

		private Coroutine _transitionCoroutine;

		private bool _isUnlocked;

		private bool _hasAppliedState;

		[Inject]
		public void InjectDependencies(QuotaCompletionModel quotaCompletionModel, QuotaSynchronizedModel quotaSynchronizedModel, IScreenShakeService screenShakeService, IAudioService audioService)
		{
			_quotaCompletionModel = quotaCompletionModel;
			_quotaSynchronizedModel = quotaSynchronizedModel;
			_screenShakeService = screenShakeService;
			_audioService = audioService;
		}

		public override void Spawned()
		{
			base.Spawned();
			ApplyBellStateFromModels(playActivationEffects: false);
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			_quotaCompletionModel.OnQuotaCompleted += OnQuotaCompleted;
			_quotaCompletionModel.OnBellActivated += OnBellActivated;
			_quotaSynchronizedModel.OnQuotaChanged += OnQuotaProgressChanged;
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			_quotaCompletionModel.OnQuotaCompleted -= OnQuotaCompleted;
			_quotaCompletionModel.OnBellActivated -= OnBellActivated;
			_quotaSynchronizedModel.OnQuotaChanged -= OnQuotaProgressChanged;
		}

		private void OnQuotaCompleted(bool isCompleted)
		{
			ApplyBellStateFromModels(isCompleted);
		}

		private void OnBellActivated(bool isActivated)
		{
			if (!isActivated)
			{
				ApplyBellStateFromModels(playActivationEffects: false);
			}
		}

		private void OnQuotaProgressChanged(float _, float __)
		{
			ApplyBellStateFromModels(playActivationEffects: false);
		}

		private void ApplyBellStateFromModels(bool playActivationEffects)
		{
			bool flag = IsQuotaUnlocked();
			ApplyBellVisualState(flag, playActivationEffects && flag);
		}

		private bool IsQuotaUnlocked()
		{
			return QuotaBellReadiness.IsQuotaMet(_quotaCompletionModel, _quotaSynchronizedModel);
		}

		private void ApplyBellVisualState(bool isUnlocked, bool playActivationEffects)
		{
			bool isUnlocked2 = _isUnlocked;
			bool hasAppliedState = _hasAppliedState;
			_isUnlocked = isUnlocked;
			_hasAppliedState = true;
			if (!isUnlocked && hasAppliedState && !isUnlocked2)
			{
				return;
			}
			if (_transitionCoroutine != null)
			{
				StopCoroutine(_transitionCoroutine);
				_transitionCoroutine = null;
			}
			_animator.SetBool(IsBellActive, isUnlocked);
			if (isUnlocked)
			{
				if (playActivationEffects)
				{
					_audioService.PlayOneShotAttached(_bellActivationSound, _soundSourceBehaviour);
					_screenShakeService.TriggerScreenShake(_cinemachineImpulseSource, _activationScreenShakeData);
					_transitionCoroutine = StartCoroutine(ActivateObjectsForTime(_activationTime));
				}
				else
				{
					ApplyUnlockedObjectsImmediate();
				}
			}
			else
			{
				_transitionCoroutine = StartCoroutine(DeactivateObjectsForTime(_deactivationTime));
			}
		}

		private void ApplyUnlockedObjectsImmediate()
		{
			foreach (GameObject item in _objectsToDeactivate)
			{
				if (item != null)
				{
					item.SetActive(value: false);
				}
			}
			foreach (GameObject item2 in _objectsToActivate)
			{
				if (item2 != null)
				{
					item2.SetActive(value: true);
				}
			}
		}

		private IEnumerator ActivateObjectsForTime(float activationTime)
		{
			yield return new WaitForSeconds(activationTime);
			ApplyUnlockedObjectsImmediate();
			_transitionCoroutine = null;
		}

		private IEnumerator DeactivateObjectsForTime(float deactivationTime)
		{
			foreach (GameObject item in _objectsToDeactivate)
			{
				if (item != null)
				{
					item.SetActive(value: false);
				}
			}
			yield return new WaitForSeconds(deactivationTime);
			foreach (GameObject item2 in _objectsToDeactivate)
			{
				if (item2 != null)
				{
					item2.SetActive(value: true);
				}
			}
			foreach (GameObject item3 in _objectsToActivate)
			{
				if (item3 != null)
				{
					item3.SetActive(value: false);
				}
			}
			_transitionCoroutine = null;
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
