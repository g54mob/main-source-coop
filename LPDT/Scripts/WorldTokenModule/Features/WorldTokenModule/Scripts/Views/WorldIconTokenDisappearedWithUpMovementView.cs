using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Features.WorldTokenModule.Scripts.Views
{
	internal class WorldIconTokenDisappearedWithUpMovementView : BigButtWorldTokenViewBase
	{
		[SerializeField]
		private CanvasGroup _canvasGroup;

		[SerializeField]
		private AnimationCurve _movementCurveUp = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		[SerializeField]
		private float _movementAmplitude = 40f;

		[SerializeField]
		private float _movementDuration = 0.8f;

		[SerializeField]
		private float _randomAngle = 45f;

		[SerializeField]
		private float _initialOffsetAmount = 5f;

		[SerializeField]
		private float _initialOffsetRandomness = 2f;

		[SerializeField]
		private float _scalePunch = 1.2f;

		private Coroutine _movementCoroutine;

		private Graphic[] _coloredGraphics;

		private Vector3 _initialScale;

		private Vector2 _movementDirection = Vector2.up;

		private void Awake()
		{
			_initialScale = _tokenVisual.localScale;
			_coloredGraphics = GetComponentsInChildren<Graphic>(includeInactive: true);
		}

		public override void SetColor(Color color)
		{
			if (_coloredGraphics == null)
			{
				_coloredGraphics = GetComponentsInChildren<Graphic>(includeInactive: true);
			}
			Graphic[] coloredGraphics = _coloredGraphics;
			for (int i = 0; i < coloredGraphics.Length; i++)
			{
				coloredGraphics[i].color = color;
			}
		}

		public override void SetMovementDirection(Vector2 direction)
		{
			_movementDirection = ((direction.sqrMagnitude > 0.0001f) ? direction.normalized : Vector2.up);
		}

		public override void StartMovement()
		{
			if (_movementCoroutine != null)
			{
				StopCoroutine(_movementCoroutine);
			}
			_tokenVisual.anchoredPosition = Vector2.zero;
			_tokenVisual.localScale = _initialScale;
			_canvasGroup.alpha = 1f;
			_movementCoroutine = StartCoroutine(MovementCoroutine());
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
			_tokenVisual.localScale = _initialScale;
			_canvasGroup.alpha = 1f;
		}

		private IEnumerator MovementCoroutine()
		{
			base.IsActive = true;
			Vector2 basePosition = _tokenVisual.anchoredPosition;
			float degrees = UnityEngine.Random.Range(0f - _randomAngle, _randomAngle);
			Vector2 randomDirection = Rotate(_movementDirection, degrees);
			float num = UnityEngine.Random.Range(_initialOffsetAmount - _initialOffsetRandomness, _initialOffsetAmount + _initialOffsetRandomness);
			basePosition += randomDirection * num;
			float elapsed = 0f;
			while (elapsed < _movementDuration)
			{
				elapsed += Time.deltaTime;
				float num2 = Mathf.Clamp01(elapsed / _movementDuration);
				float num3 = _movementCurveUp.Evaluate(num2);
				_tokenVisual.anchoredPosition = basePosition + randomDirection * (num3 * _movementAmplitude);
				_tokenVisual.localScale = _initialScale * Mathf.Lerp(_scalePunch, 1f, num2);
				_canvasGroup.alpha = Mathf.Lerp(1f, 0f, num2);
				yield return null;
			}
			_canvasGroup.alpha = 0f;
			base.IsActive = false;
			_movementCoroutine = null;
		}

		private static Vector2 Rotate(Vector2 direction, float degrees)
		{
			float f = degrees * (MathF.PI / 180f);
			float num = Mathf.Sin(f);
			float num2 = Mathf.Cos(f);
			return new Vector2(direction.x * num2 - direction.y * num, direction.x * num + direction.y * num2).normalized;
		}
	}
}
