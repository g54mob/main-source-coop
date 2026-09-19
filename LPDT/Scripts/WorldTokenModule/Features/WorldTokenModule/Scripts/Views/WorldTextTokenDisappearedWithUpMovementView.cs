using System;
using System.Collections;
using UnityEngine;

namespace Features.WorldTokenModule.Scripts.Views
{
	internal class WorldTextTokenDisappearedWithUpMovementView : WorldTokenViewBase
	{
		[SerializeField]
		private CanvasGroup _canvasGroup;

		[SerializeField]
		private AnimationCurve _movementCurveUp = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private float _movementAmplitude = 10f;

		[SerializeField]
		private float _movementDuration = 1f;

		[SerializeField]
		private float _randomAngle = 60f;

		[SerializeField]
		private float _initialOffsetAmount = 5f;

		[SerializeField]
		private float _initialOffsetRandomness = 2f;

		[SerializeField]
		private float _yPositionRandomnessAmount = 3f;

		[SerializeField]
		private float _yPositionRandomness = 1.5f;

		private Coroutine _movementCoroutine;

		public override void StartMovement()
		{
			if (_movementCoroutine != null)
			{
				StopCoroutine(_movementCoroutine);
			}
			_tokenVisual.anchoredPosition = Vector2.zero;
			_movementCoroutine = StartCoroutine(UpDownMovementCoroutine());
		}

		public override void StopMovement()
		{
			if (_movementCoroutine != null)
			{
				StopCoroutine(_movementCoroutine);
				_movementCoroutine = null;
			}
			base.IsActive = false;
			_tokenVisual.anchoredPosition = Vector2.zero;
			_canvasGroup.alpha = 1f;
		}

		private IEnumerator UpDownMovementCoroutine()
		{
			base.IsActive = true;
			Vector2 basePosition = _tokenVisual.anchoredPosition;
			float elapsed = 0f;
			float num = UnityEngine.Random.Range(0f - _randomAngle, _randomAngle);
			float f = (90f + num) * (MathF.PI / 180f);
			Vector2 randomDirection = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
			float num2 = UnityEngine.Random.Range(_initialOffsetAmount - _initialOffsetRandomness, _initialOffsetAmount + _initialOffsetRandomness);
			basePosition += randomDirection * num2;
			_tokenVisual.anchoredPosition = basePosition;
			float randomYOffset = UnityEngine.Random.Range(0f - _yPositionRandomnessAmount, _yPositionRandomnessAmount);
			while (elapsed < _movementDuration)
			{
				elapsed += Time.deltaTime;
				float num3 = Mathf.Clamp01(elapsed / _movementDuration);
				float num4 = _movementCurveUp.Evaluate(num3);
				Vector2 anchoredPosition = basePosition + randomDirection * (num4 * _movementAmplitude);
				anchoredPosition.y += randomYOffset;
				_tokenVisual.anchoredPosition = anchoredPosition;
				_canvasGroup.alpha = Mathf.Lerp(1f, 0f, num3);
				yield return null;
			}
			Vector2 anchoredPosition2 = basePosition + randomDirection * _movementAmplitude;
			anchoredPosition2.y += randomYOffset;
			_tokenVisual.anchoredPosition = anchoredPosition2;
			_canvasGroup.alpha = 0f;
			base.IsActive = false;
		}
	}
}
