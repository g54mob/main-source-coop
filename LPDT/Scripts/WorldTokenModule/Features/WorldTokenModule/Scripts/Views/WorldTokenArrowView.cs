using System.Collections;
using UnityEngine;

namespace Features.WorldTokenModule.Scripts.Views
{
	internal class WorldTokenArrowView : WorldTokenViewBase
	{
		[SerializeField]
		private AnimationCurve _movementCurveUp = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private AnimationCurve _movementCurveDown = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private float _movementAmplitude = 10f;

		[SerializeField]
		private float _movementDuration = 1f;

		private Coroutine _movementCoroutine;

		public override void StartMovement()
		{
			if (_movementCoroutine != null)
			{
				StopCoroutine(_movementCoroutine);
			}
			base.IsActive = true;
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
		}

		private IEnumerator UpDownMovementCoroutine()
		{
			Vector2 basePosition = _tokenVisual.anchoredPosition;
			while (true)
			{
				yield return MovementPhase(basePosition, _movementCurveUp, 0f, 1f);
				yield return MovementPhase(basePosition, _movementCurveDown, 1f, 0f);
			}
		}

		private IEnumerator MovementPhase(Vector2 basePosition, AnimationCurve curve, float from, float to)
		{
			float elapsed = 0f;
			while (elapsed < _movementDuration)
			{
				elapsed += Time.deltaTime;
				float time = Mathf.Clamp01(elapsed / _movementDuration);
				float t = curve.Evaluate(time);
				float num = Mathf.Lerp(from, to, t);
				_tokenVisual.anchoredPosition = basePosition + Vector2.up * (num * _movementAmplitude);
				yield return null;
			}
		}
	}
}
