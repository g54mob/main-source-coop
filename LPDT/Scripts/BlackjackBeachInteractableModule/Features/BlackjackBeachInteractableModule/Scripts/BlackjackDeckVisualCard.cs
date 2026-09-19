using System.Collections;
using UnityEngine;

namespace Features.BlackjackBeachInteractableModule.Scripts
{
	public class BlackjackDeckVisualCard : MonoBehaviour
	{
		[SerializeField]
		private BlackjackCardDissolveEffect _dissolveEffect;

		private Coroutine _dissolveRoutine;

		private bool _isVisible = true;

		public Transform SpawnTransform => base.transform;

		public bool IsVisible => _isVisible;

		private void Awake()
		{
			_dissolveEffect.SetVisibleInstant(_isVisible);
		}

		public void SetVisibleInstant(bool visible)
		{
			StopDissolve();
			_isVisible = visible;
			base.gameObject.SetActive(visible);
			_dissolveEffect.SetVisibleInstant(visible);
		}

		public void SetVisibleAnimated(bool visible, Material dissolveTemplate, float duration)
		{
			if (_isVisible != visible || base.gameObject.activeSelf != visible)
			{
				StopDissolve();
				if (!base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(value: true);
				}
				_isVisible = visible;
				if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
				{
					SetVisibleInstant(visible);
				}
				else
				{
					_dissolveRoutine = StartCoroutine(AnimateVisibility(visible, dissolveTemplate, duration));
				}
			}
		}

		private IEnumerator AnimateVisibility(bool visible, Material dissolveTemplate, float duration)
		{
			if (visible)
			{
				yield return _dissolveEffect.PlayDissolveIn(duration, dissolveTemplate);
			}
			else
			{
				yield return _dissolveEffect.PlayDissolveOut(duration, dissolveTemplate);
				base.gameObject.SetActive(value: false);
				_dissolveEffect.SetVisibleInstant(visible: false);
			}
			_dissolveRoutine = null;
		}

		private void StopDissolve()
		{
			if (_dissolveRoutine != null)
			{
				StopCoroutine(_dissolveRoutine);
				_dissolveRoutine = null;
			}
		}
	}
}
