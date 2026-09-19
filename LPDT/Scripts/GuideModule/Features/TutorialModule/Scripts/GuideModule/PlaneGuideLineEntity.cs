using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class PlaneGuideLineEntity : MonoBehaviour, IGuideLineEntity
	{
		private const int SAMPLE_COUNT = 50;

		private static readonly int DissolveId = Shader.PropertyToID("_DissolveAmount");

		[SerializeField]
		private LineRenderer _lineRenderer;

		[SerializeField]
		private float _groundOffset = 0.1f;

		[SerializeField]
		private float _lerpSpeed = 10f;

		private readonly Vector3[] _current = new Vector3[50];

		private readonly Vector3[] _target = new Vector3[50];

		private bool _hasTarget;

		private MaterialPropertyBlock _propertyBlock;

		private bool _isDissolveAnimating;

		private float _dissolveFrom;

		private float _dissolveTo;

		private float _dissolveDuration;

		private float _dissolveTimer;

		private Action _dissolveOnComplete;

		private void Start()
		{
			_lineRenderer.material = new Material(_lineRenderer.material);
		}

		public void SetPoints(IReadOnlyList<Vector3> points)
		{
			if (points != null && points.Count != 0)
			{
				Resample(points, _target, _groundOffset);
				if (!_hasTarget)
				{
					Array.Copy(_target, _current, 50);
					_hasTarget = true;
				}
			}
		}

		private void Update()
		{
			UpdateDissolve();
			if (_hasTarget)
			{
				float t = 1f - Mathf.Exp((0f - _lerpSpeed) * Time.deltaTime);
				for (int i = 0; i < 50; i++)
				{
					_current[i] = Vector3.Lerp(_current[i], _target[i], t);
				}
				_lineRenderer.positionCount = 50;
				_lineRenderer.SetPositions(_current);
			}
		}

		public void SetDissolve(float amount)
		{
			ApplyDissolve(amount);
		}

		public void AnimateDissolve(float from, float to, float duration, Action onComplete)
		{
			_dissolveFrom = from;
			_dissolveTo = to;
			_dissolveDuration = duration;
			_dissolveTimer = 0f;
			_dissolveOnComplete = onComplete;
			_isDissolveAnimating = true;
			ApplyDissolve(from);
			if (duration <= 0f)
			{
				CompleteDissolve();
			}
		}

		public void StopDissolveAnimation()
		{
			_isDissolveAnimating = false;
			_dissolveOnComplete = null;
		}

		private void UpdateDissolve()
		{
			if (_isDissolveAnimating)
			{
				_dissolveTimer += Time.unscaledDeltaTime;
				float num = ((_dissolveDuration <= 0f) ? 1f : Mathf.Clamp01(_dissolveTimer / _dissolveDuration));
				ApplyDissolve(Mathf.Lerp(_dissolveFrom, _dissolveTo, num));
				if (num >= 1f)
				{
					CompleteDissolve();
				}
			}
		}

		private void CompleteDissolve()
		{
			_isDissolveAnimating = false;
			Action dissolveOnComplete = _dissolveOnComplete;
			_dissolveOnComplete = null;
			dissolveOnComplete?.Invoke();
		}

		private void ApplyDissolve(float amount)
		{
			if (_propertyBlock == null)
			{
				_propertyBlock = new MaterialPropertyBlock();
			}
			_lineRenderer.GetPropertyBlock(_propertyBlock);
			_propertyBlock.SetFloat(DissolveId, amount);
			_lineRenderer.SetPropertyBlock(_propertyBlock);
		}

		private void Resample(IReadOnlyList<Vector3> src, Vector3[] dst, float yOffset)
		{
			int num = dst.Length;
			if (src.Count == 1)
			{
				for (int i = 0; i < num; i++)
				{
					dst[i] = Off(src[0], yOffset);
				}
				return;
			}
			float num2 = 0f;
			for (int j = 1; j < src.Count; j++)
			{
				num2 += Vector3.Distance(src[j - 1], src[j]);
			}
			if (num2 <= 0.0001f)
			{
				for (int k = 0; k < num; k++)
				{
					dst[k] = Off(src[0], yOffset);
				}
				return;
			}
			float num3 = num2 / (float)(num - 1);
			dst[0] = Off(src[0], yOffset);
			int l = 0;
			float num4 = 0f;
			for (int m = 1; m < num - 1; m++)
			{
				float num5;
				for (num5 = num3 * (float)m; l < src.Count - 2 && num4 + Vector3.Distance(src[l], src[l + 1]) < num5; l++)
				{
					num4 += Vector3.Distance(src[l], src[l + 1]);
				}
				float num6 = Vector3.Distance(src[l], src[l + 1]);
				float t = ((num6 > 0f) ? ((num5 - num4) / num6) : 0f);
				dst[m] = Off(Vector3.Lerp(src[l], src[l + 1], t), yOffset);
			}
			dst[num - 1] = Off(src[src.Count - 1], yOffset);
		}

		private Vector3 Off(Vector3 p, float y)
		{
			return new Vector3(p.x, p.y + y, p.z);
		}

		public void Enable()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Disable()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
