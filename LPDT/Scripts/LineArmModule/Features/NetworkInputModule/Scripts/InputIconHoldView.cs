using System.Collections;
using Features.LineArmModule.Scripts.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using Zenject;

namespace Features.NetworkInputModule.Scripts
{
	internal class InputIconHoldView : InputIconHoldViewBase
	{
		[SerializeField]
		protected Image _holdFill;

		[SerializeField]
		private Image _pcHoldFill;

		[SerializeField]
		private GameObject _container;

		[SerializeField]
		private GameObject _defaultAim;

		[SerializeField]
		private GameObject _interactableAim;

		[SerializeField]
		private CanvasGroup _canvasGroup;

		private LocalInputActions _inputActions;

		private ArmInputModel _armInputModel;

		private InteractModel _interactModel;

		private Coroutine _stopHoldCoroutine;

		private Coroutine _startHoldCoroutine;

		private float _time;

		private InputAction _inputActionWithCorrectReference;

		[Inject]
		private void InjectDependencies(LocalInputActions inputActions, ArmInputModel armInputModel, InteractModel interactModel)
		{
			_armInputModel = armInputModel;
			_inputActions = inputActions;
			_interactModel = interactModel;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			_inputActionWithCorrectReference = _inputActions.asset.FindActionMap(_inputAction.action.actionMap.name, throwIfNotFound: true).FindAction(_inputAction.action.name, throwIfNotFound: true);
			_inputActionWithCorrectReference.started += StartHoldReload;
			_inputActionWithCorrectReference.canceled += StopHoldReload;
			_interactModel.OnCurrentInteractableChanged += ChangeCursor;
			_interactModel.OnCursorVisibilityChanged += ChangeCursorVisibility;
			_holdFill.fillAmount = 0f;
			if (_pcHoldFill != null)
			{
				_pcHoldFill.fillAmount = 0f;
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_inputActionWithCorrectReference.started -= StartHoldReload;
			_inputActionWithCorrectReference.canceled -= StopHoldReload;
			_interactModel.OnCurrentInteractableChanged -= ChangeCursor;
			_interactModel.OnCursorVisibilityChanged -= ChangeCursorVisibility;
			if (_startHoldCoroutine != null)
			{
				StopCoroutine(_startHoldCoroutine);
			}
			if (_stopHoldCoroutine != null)
			{
				StopCoroutine(_stopHoldCoroutine);
			}
		}

		private void StartHoldReload(InputAction.CallbackContext callbackContext)
		{
			if (_armInputModel.IsThrowEnabled && callbackContext.interaction is HoldInteraction holdInteraction)
			{
				_time = (((double)holdInteraction.duration > 0.0) ? holdInteraction.duration : InputSystem.settings.defaultHoldTime);
				if (_startHoldCoroutine != null)
				{
					StopCoroutine(_startHoldCoroutine);
				}
				if (_stopHoldCoroutine != null)
				{
					StopCoroutine(_stopHoldCoroutine);
				}
				_startHoldCoroutine = StartCoroutine(FillCoroutine());
			}
		}

		private void StopHoldReload(InputAction.CallbackContext _)
		{
			StopHoldReload();
		}

		private void StopHoldReload()
		{
			_container.SetActive(value: false);
			GetHoldFill().fillAmount = 0f;
		}

		private IEnumerator FillCoroutine()
		{
			_container.SetActive(value: true);
			float time = 0f;
			while (time < _time)
			{
				time += Time.unscaledDeltaTime;
				GetHoldFill().fillAmount = time / _time;
				yield return null;
			}
			StopHoldReload();
		}

		private IEnumerator UnFillCoroutine()
		{
			float time = GetHoldFill().fillAmount * _time;
			while (time >= 0f)
			{
				time -= Time.unscaledDeltaTime;
				GetHoldFill().fillAmount = time / _time;
				yield return null;
			}
			_container.SetActive(value: false);
		}

		private Image GetHoldFill()
		{
			if (_pcHoldFill == null)
			{
				return _holdFill;
			}
			if (_pcHoldFill.gameObject.activeSelf)
			{
				return _pcHoldFill;
			}
			return _holdFill;
		}

		private void ChangeCursor()
		{
			_defaultAim.SetActive(_interactModel.CurrentInteractable == null);
			_interactableAim.SetActive(_interactModel.CurrentInteractable != null);
		}

		private void ChangeCursorVisibility(bool isVisible)
		{
			_canvasGroup.alpha = (isVisible ? 1f : 0f);
		}
	}
}
