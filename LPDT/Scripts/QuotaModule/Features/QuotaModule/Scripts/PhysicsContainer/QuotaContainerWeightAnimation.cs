using System.Collections;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.QuotaModule.Scripts.PhysicsContainer
{
	[NetworkBehaviourWeaved(0)]
	public class QuotaContainerWeightAnimation : NetworkBehaviour
	{
		[SerializeField]
		private float _maxSinkHeight = 2f;

		[SerializeField]
		private float _animationSpeed = 2f;

		private QuotaSynchronizedModel _quotaSynchronizedModel;

		private Coroutine _animationCoroutine;

		private Vector3 _initialPosition;

		private float _targetSinkAmount;

		private float _currentSinkAmount;

		[Inject]
		private void InjectDependencies(QuotaSynchronizedModel quotaSynchronizedModel)
		{
			_quotaSynchronizedModel = quotaSynchronizedModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_initialPosition = base.transform.position;
			OnQuotaChanged(_quotaSynchronizedModel.CurrentQuota.Value, _quotaSynchronizedModel.MaxQuota.Value);
			_quotaSynchronizedModel.OnQuotaChanged += OnQuotaChanged;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (_quotaSynchronizedModel != null)
			{
				_quotaSynchronizedModel.OnQuotaChanged -= OnQuotaChanged;
			}
			if (_animationCoroutine != null)
			{
				StopCoroutine(_animationCoroutine);
			}
		}

		private void OnQuotaChanged(float currentQuota, float maxQuota)
		{
			if (maxQuota <= 0f)
			{
				_targetSinkAmount = 0f;
				StartAnimation();
			}
			else
			{
				float num = Mathf.Clamp01(currentQuota / maxQuota);
				_targetSinkAmount = num * _maxSinkHeight;
				StartAnimation();
			}
		}

		private void StartAnimation()
		{
			if (_animationCoroutine != null)
			{
				StopCoroutine(_animationCoroutine);
			}
			_animationCoroutine = StartCoroutine(AnimateSink());
		}

		private IEnumerator AnimateSink()
		{
			while (!Mathf.Approximately(_currentSinkAmount, _targetSinkAmount))
			{
				_currentSinkAmount = Mathf.Lerp(_currentSinkAmount, _targetSinkAmount, Time.deltaTime * _animationSpeed);
				Vector3 initialPosition = _initialPosition;
				initialPosition.y -= _currentSinkAmount;
				base.transform.position = initialPosition;
				yield return null;
			}
			_currentSinkAmount = _targetSinkAmount;
			Vector3 initialPosition2 = _initialPosition;
			initialPosition2.y -= _currentSinkAmount;
			base.transform.position = initialPosition2;
			_animationCoroutine = null;
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
