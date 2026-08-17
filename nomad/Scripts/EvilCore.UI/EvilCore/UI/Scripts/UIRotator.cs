using PrimeTween;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public class UIRotator : MonoBehaviour
	{
		[Header("Rotation")]
		[SerializeField]
		private float degreesPerSecond = 180f;

		[SerializeField]
		private bool clockwise = true;

		[SerializeField]
		private bool playOnEnable = true;

		private Quaternion _baseRotation;

		private Tween _tween;

		private bool _cached;

		private void Awake()
		{
			CacheRotation();
		}

		private void CacheRotation()
		{
			if (!_cached)
			{
				_baseRotation = base.transform.localRotation;
				_cached = true;
			}
		}

		private void OnEnable()
		{
			if (playOnEnable)
			{
				Play();
			}
		}

		public void Play()
		{
			CacheRotation();
			_tween.Stop();
			if (!(degreesPerSecond <= 0f))
			{
				float z = (clockwise ? (-360f) : 360f);
				float duration = 360f / degreesPerSecond;
				_tween = Tween.LocalEulerAngles(base.transform, Vector3.zero, new Vector3(0f, 0f, z), duration, Ease.Linear, -1, CycleMode.Incremental, 0f, 0f, useUnscaledTime: true);
			}
		}

		public void StopSpin()
		{
			_tween.Stop();
		}

		private void OnDisable()
		{
			_tween.Stop();
			if (_cached)
			{
				base.transform.localRotation = _baseRotation;
			}
		}
	}
}
