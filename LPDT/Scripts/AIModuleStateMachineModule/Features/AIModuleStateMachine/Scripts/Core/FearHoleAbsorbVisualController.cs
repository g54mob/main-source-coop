using DG.Tweening;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core
{
	public class FearHoleAbsorbVisualController : MonoBehaviour, IFearHoleAbsorbVisualController
	{
		private Tween _scaleTween;

		private Tween _moveTween;

		private Vector3 _originalLocalScale;

		private void Awake()
		{
			_originalLocalScale = base.transform.localScale;
		}

		private void OnDestroy()
		{
			Kill(resetScale: false);
		}

		public void ResetVisual()
		{
			Kill(resetScale: false);
			base.transform.localScale = _originalLocalScale;
		}

		public void Play(float duration, float endScale, int ease)
		{
			Play(duration, endScale, ease, null);
		}

		public void Play(float duration, float endScale, int ease, Vector3 worldTarget)
		{
			Play(duration, endScale, ease, (Vector3?)worldTarget);
		}

		private void Play(float duration, float endScale, int ease, Vector3? worldTarget)
		{
			Kill(resetScale: false);
			float num = Mathf.Max(0f, duration);
			Vector3 vector = _originalLocalScale * Mathf.Max(0f, endScale);
			if (num <= 0f)
			{
				base.transform.localScale = vector;
				if (worldTarget.HasValue)
				{
					base.transform.position = GetHorizontalTarget(worldTarget.Value);
				}
				return;
			}
			_scaleTween = base.transform.DOScale(vector, num).SetEase((Ease)ease).SetLink(base.gameObject)
				.OnKill(delegate
				{
					_scaleTween = null;
				});
			if (worldTarget.HasValue)
			{
				_moveTween = base.transform.DOMove(GetHorizontalTarget(worldTarget.Value), num).SetEase((Ease)ease).SetLink(base.gameObject)
					.OnKill(delegate
					{
						_moveTween = null;
					});
			}
		}

		public void Kill(bool resetScale)
		{
			if (_scaleTween != null)
			{
				_scaleTween.Kill();
				_scaleTween = null;
			}
			if (_moveTween != null)
			{
				_moveTween.Kill();
				_moveTween = null;
			}
			if (resetScale)
			{
				base.transform.localScale = _originalLocalScale;
			}
		}

		private Vector3 GetHorizontalTarget(Vector3 worldTarget)
		{
			return new Vector3(worldTarget.x, base.transform.position.y, worldTarget.z);
		}
	}
}
