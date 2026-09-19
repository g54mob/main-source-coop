using System;
using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public abstract class TipEntity : MonoBehaviour, ITipEntity
	{
		protected static readonly int DissolveId = Shader.PropertyToID("_DissolveAmount");

		private bool _isInitialized;

		private bool _isDissolveAnimating;

		private float _dissolveFrom;

		private float _dissolveTo;

		private float _dissolveDuration;

		private float _dissolveTimer;

		private Action _dissolveOnComplete;

		public Transform Transform => base.transform;

		public bool IsInitialized
		{
			get
			{
				return _isInitialized;
			}
			set
			{
				_isInitialized = value;
				this.OnInitialized?.Invoke(value);
			}
		}

		public event Action<bool> OnInitialized;

		public void Enable()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Disable()
		{
			base.gameObject.SetActive(value: false);
		}

		private void Start()
		{
			InitializeRenderer();
		}

		private void Update()
		{
			UpdateDissolve();
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

		protected virtual void InitializeRenderer()
		{
		}

		protected abstract void ApplyDissolve(float amount);

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
	}
}
