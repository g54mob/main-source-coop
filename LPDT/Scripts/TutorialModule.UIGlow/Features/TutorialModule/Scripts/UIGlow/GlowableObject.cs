using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Features.TutorialModule.Scripts.UIGlow
{
	public class GlowableObject : MonoBehaviour, IGlowableObject
	{
		[SerializeField]
		private GlowableObjectEnum _glowableObjectEnum;

		[SerializeField]
		private Image _outlineImage;

		[SerializeField]
		private float _minScale = 1f;

		[SerializeField]
		private float _maxScale = 1.2f;

		[SerializeField]
		private Color _minColor = Color.white;

		[SerializeField]
		private Color _maxColor = Color.yellow;

		[SerializeField]
		private float _duration = 1f;

		private Sequence _pulseSequence;

		public GlowableObjectEnum GlowableObjectEnum => _glowableObjectEnum;

		private void OnEnable()
		{
			Pulse();
			SetGlow();
		}

		private void OnDisable()
		{
			_pulseSequence.Kill();
		}

		public void SetGlow()
		{
			_outlineImage.enabled = true;
		}

		public void UnSetGlow()
		{
			_outlineImage.enabled = false;
		}

		private void Pulse()
		{
			_pulseSequence.Kill();
			_outlineImage.rectTransform.localScale = Vector3.one * _minScale;
			_outlineImage.color = _minColor;
			_pulseSequence = DOTween.Sequence();
			_pulseSequence.Append(_outlineImage.rectTransform.DOScale(_maxScale, _duration));
			_pulseSequence.Join(DOTween.To(() => _outlineImage.color, delegate(Color color)
			{
				_outlineImage.color = color;
			}, _maxColor, _duration));
			_pulseSequence.SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetUpdate(isIndependentUpdate: true);
		}
	}
}
