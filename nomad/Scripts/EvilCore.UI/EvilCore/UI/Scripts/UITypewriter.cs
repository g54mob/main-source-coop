using PrimeTween;
using TMPro;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public class UITypewriter : MonoBehaviour
	{
		[Header("Target")]
		[SerializeField]
		private TextMeshProUGUI text;

		[Header("Settings")]
		[SerializeField]
		private float charactersPerSecond = 40f;

		[SerializeField]
		private bool playOnEnable;

		private Tween _tween;

		private void Awake()
		{
			if (text == null)
			{
				text = GetComponent<TextMeshProUGUI>();
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
			Play(null);
		}

		public void Play(string newText)
		{
			if (!(text == null))
			{
				if (newText != null)
				{
					text.text = newText;
				}
				text.ForceMeshUpdate();
				int characterCount = text.textInfo.characterCount;
				_tween.Stop();
				text.maxVisibleCharacters = 0;
				float duration = ((charactersPerSecond > 0f) ? ((float)characterCount / charactersPerSecond) : 0f);
				_tween = Tween.Custom(this, 0f, characterCount, duration, delegate(UITypewriter writer, float value)
				{
					writer.text.maxVisibleCharacters = Mathf.RoundToInt(value);
				}, Ease.Linear, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime: true);
			}
		}

		public void Complete()
		{
			if (!(text == null))
			{
				_tween.Stop();
				text.maxVisibleCharacters = 2147483647;
			}
		}

		private void OnDisable()
		{
			_tween.Stop();
		}
	}
}
