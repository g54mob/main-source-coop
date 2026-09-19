using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.TutorialModule.Scripts.UIGlow
{
	public class GlowableTipObject : MonoBehaviour
	{
		[Header("Scale")]
		[SerializeField]
		private RectTransform _scaleContainer;

		[SerializeField]
		private float _minScale = 1f;

		[SerializeField]
		private float _maxScale = 1.2f;

		[Header("Image Color")]
		[SerializeField]
		private Image _image;

		[SerializeField]
		private Color _minImageColor = Color.white;

		[SerializeField]
		private Color _maxImageColor = Color.yellow;

		[Header("Text Color")]
		[SerializeField]
		private TMP_Text _text;

		[SerializeField]
		private Color _minTextColor = Color.white;

		[SerializeField]
		private Color _maxTextColor = Color.yellow;

		[Header("Timing")]
		[SerializeField]
		private float _duration = 1f;

		[SerializeField]
		private Ease _ease = Ease.Linear;

		[SerializeField]
		private bool _activateOnStart;

		private Sequence _pulseSequence;

		private void Start()
		{
			if (_activateOnStart)
			{
				EnablePulsating();
			}
		}

		[ContextMenu("Enable Pulsating")]
		public void EnablePulsating()
		{
			DisablePulsating();
			ApplyMinValues();
			_pulseSequence = DOTween.Sequence();
			if (_scaleContainer != null)
			{
				_pulseSequence.Join(_scaleContainer.DOScale(_maxScale, _duration).SetEase(_ease));
			}
			if (_image != null)
			{
				_pulseSequence.Join(DOTween.To(() => _image.color, delegate(Color color)
				{
					_image.color = color;
				}, _maxImageColor, _duration).SetEase(_ease));
			}
			if (_text != null)
			{
				_pulseSequence.Join(DOTween.To(() => _text.color, delegate(Color color)
				{
					_text.color = color;
				}, _maxTextColor, _duration).SetEase(_ease));
			}
			_pulseSequence.SetLoops(-1, LoopType.Yoyo).SetUpdate(isIndependentUpdate: true);
		}

		[ContextMenu("Disable Pulsating")]
		public void DisablePulsating()
		{
			_pulseSequence.Kill();
			_pulseSequence = null;
		}

		private void ApplyMinValues()
		{
			if (_scaleContainer != null)
			{
				_scaleContainer.localScale = Vector3.one * _minScale;
			}
			if (_image != null)
			{
				_image.color = _minImageColor;
			}
			if (_text != null)
			{
				_text.color = _minTextColor;
			}
		}
	}
}
