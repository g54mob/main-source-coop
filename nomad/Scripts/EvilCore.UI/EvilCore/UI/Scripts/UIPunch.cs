using PrimeTween;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public class UIPunch : MonoBehaviour
	{
		[Header("Punch")]
		[SerializeField]
		private Vector3 strength = new Vector3(0.2f, 0.2f, 0f);

		[SerializeField]
		private float duration = 0.4f;

		[SerializeField]
		private float frequency = 10f;

		private Vector3 _baseScale;

		private bool _cached;

		private void Awake()
		{
			CacheScale();
		}

		private void CacheScale()
		{
			if (!_cached)
			{
				_baseScale = base.transform.localScale;
				_cached = true;
			}
		}

		public void Punch()
		{
			CacheScale();
			Tween.CompleteAll(base.transform);
			base.transform.localScale = _baseScale;
			Tween.PunchScale(base.transform, strength, duration, frequency, enableFalloff: true, Ease.Default, 0f, 1, 0f, 0f, useUnscaledTime: true);
		}

		private void OnDisable()
		{
			Tween.CompleteAll(base.transform);
			if (_cached)
			{
				base.transform.localScale = _baseScale;
			}
		}
	}
}
