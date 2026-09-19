using System.Collections;
using Features.CameraModelModule;
using Features.QuotaModule.Scripts;
using TMPro;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts
{
	public class CurrentWallet3DView : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _currentWalletText;

		[SerializeField]
		private Transform _transformToRotateOnCamera;

		[SerializeField]
		private float _moneyLerpSpeed = 5f;

		[SerializeField]
		private AnimationCurve _moneyLerpCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private Color _failureColor = Color.red;

		[SerializeField]
		private float _failureColorDuration = 0.5f;

		[SerializeField]
		private AnimationCurve _failureColorCurveIn = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private AnimationCurve _failureColorCurveOut = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		private CurrentWalletSynchronizedModel _currentWalletSynchronizedModel;

		private StoreDraftMoneyModel _storeDraftMoneyModel;

		private CameraModel _cameraModel;

		private float _displayedMoney;

		private float _targetMoney;

		private Coroutine _moneyLerpCoroutine;

		private Coroutine _failureColorCoroutine;

		private Color _originalTextColor;

		[Inject]
		public void InjectDependencies(CurrentWalletSynchronizedModel currentWalletSynchronizedModel, StoreDraftMoneyModel storeDraftMoneyModel, CameraModel cameraModel)
		{
			_currentWalletSynchronizedModel = currentWalletSynchronizedModel;
			_storeDraftMoneyModel = storeDraftMoneyModel;
			_cameraModel = cameraModel;
		}

		private void OnEnable()
		{
			_originalTextColor = _currentWalletText.color;
			_storeDraftMoneyModel.OnDraftMoneyChanged += UpdateCurrentWalletText;
			_storeDraftMoneyModel.OnDraftFailure += SetTextFailure;
			_currentWalletSynchronizedModel.OnCurrentSessionMoneyChanged += UpdateCurrencyWalletText;
			UpdateCurrentWalletText(_storeDraftMoneyModel.DraftMoney);
		}

		private void OnDisable()
		{
			_storeDraftMoneyModel.OnDraftMoneyChanged -= UpdateCurrentWalletText;
			_storeDraftMoneyModel.OnDraftFailure -= SetTextFailure;
			_currentWalletSynchronizedModel.OnCurrentSessionMoneyChanged -= UpdateCurrencyWalletText;
		}

		private void LateUpdate()
		{
			if (!(_cameraModel.CameraObject == null))
			{
				Vector3 vector = _cameraModel.CameraObject.transform.position - _transformToRotateOnCamera.position;
				float y = Vector3.SignedAngle(to: new Vector3(vector.x, 0f, vector.z), from: Vector3.forward, axis: Vector3.up);
				_transformToRotateOnCamera.rotation = Quaternion.Euler(0f, y, 0f);
			}
		}

		public void UpdateCurrencyWalletText(float _)
		{
			UpdateCurrentWalletText(_storeDraftMoneyModel.DraftMoney);
		}

		private void UpdateCurrentWalletText(float money)
		{
			_targetMoney = _currentWalletSynchronizedModel.CurrentSessionMoney - money;
			if (_moneyLerpCoroutine != null)
			{
				StopCoroutine(_moneyLerpCoroutine);
			}
			_moneyLerpCoroutine = StartCoroutine(LerpMoneyDisplay());
		}

		private IEnumerator LerpMoneyDisplay()
		{
			float elapsedTime = 0f;
			float duration = 1f / _moneyLerpSpeed;
			while (elapsedTime < duration)
			{
				elapsedTime += Time.deltaTime;
				float time = Mathf.Clamp01(elapsedTime / duration);
				float t = _moneyLerpCurve.Evaluate(time);
				_displayedMoney = Mathf.Lerp(_displayedMoney, _targetMoney, t);
				_currentWalletText.SetText($"{_displayedMoney:F0}¢");
				yield return null;
			}
			_displayedMoney = _targetMoney;
			_currentWalletText.SetText($"{_displayedMoney:F0}¢");
		}

		private void SetTextFailure()
		{
			if (_failureColorCoroutine != null)
			{
				StopCoroutine(_failureColorCoroutine);
			}
			_failureColorCoroutine = StartCoroutine(LerpFailureColor());
		}

		private IEnumerator LerpFailureColor()
		{
			float elapsedTime = 0f;
			while (elapsedTime < _failureColorDuration)
			{
				elapsedTime += Time.deltaTime;
				float time = Mathf.Clamp01(elapsedTime / _failureColorDuration);
				float t = _failureColorCurveIn.Evaluate(time);
				_currentWalletText.color = Color.Lerp(_originalTextColor, _failureColor, t);
				yield return null;
			}
			elapsedTime = 0f;
			while (elapsedTime < _failureColorDuration)
			{
				elapsedTime += Time.deltaTime;
				float time2 = Mathf.Clamp01(elapsedTime / _failureColorDuration);
				float t2 = _failureColorCurveOut.Evaluate(time2);
				_currentWalletText.color = Color.Lerp(_failureColor, _originalTextColor, t2);
				yield return null;
			}
			_currentWalletText.color = _originalTextColor;
		}
	}
}
