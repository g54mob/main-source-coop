using PrimeTween;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public class UIShake : MonoBehaviour
	{
		[Header("Shake")]
		[SerializeField]
		private Vector3 strength = new Vector3(12f, 12f, 0f);

		[SerializeField]
		private float duration = 0.3f;

		[SerializeField]
		private float frequency = 18f;

		private Vector3 _basePosition;

		private bool _cached;

		private void Awake()
		{
			CachePosition();
		}

		private void CachePosition()
		{
			if (!_cached)
			{
				_basePosition = base.transform.localPosition;
				_cached = true;
			}
		}

		public void Shake()
		{
			CachePosition();
			Tween.CompleteAll(base.transform);
			base.transform.localPosition = _basePosition;
			Tween.ShakeLocalPosition(base.transform, strength, duration, frequency, enableFalloff: true, Ease.Default, 0f, 1, 0f, 0f, useUnscaledTime: true);
		}

		private void OnDisable()
		{
			Tween.CompleteAll(base.transform);
			if (_cached)
			{
				base.transform.localPosition = _basePosition;
			}
		}
	}
}
