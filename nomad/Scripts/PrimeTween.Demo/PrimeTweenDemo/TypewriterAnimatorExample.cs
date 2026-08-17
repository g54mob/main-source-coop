using System;
using JetBrains.Annotations;
using PrimeTween;
using TMPro;
using UnityEngine;

namespace PrimeTweenDemo
{
	[PublicAPI]
	public class TypewriterAnimatorExample : MonoBehaviour
	{
		private enum AnimationType
		{
			Simple = 0,
			WithPunctuations = 1,
			ByWords = 2
		}

		[SerializeField]
		private AnimationType animationType = AnimationType.WithPunctuations;

		[SerializeField]
		private float charsPerSecond = 40f;

		[SerializeField]
		private int pauseAfterPunctuation = 20;

		private TextMeshProUGUI text;

		private void Awake()
		{
			text = base.gameObject.AddComponent<TextMeshProUGUI>();
			text.maxVisibleCharacters = 0;
			text.alignment = TextAlignmentOptions.TopLeft;
			text.fontSize = 12f;
			text.color = Color.black * 0.8f;
			text.text = "This text is <color=orange>animated</color> with <b>zero allocations</b>, see <i>'TypewriterAnimatorExample'</i> script for more details.\n\nPrimeTween rocks!";
		}

		public Tween Animate()
		{
			if (!Application.isPlaying)
			{
				PrimeTweenConfig.warnZeroDuration = false;
				Tween result = Tween.Delay(0f);
				PrimeTweenConfig.warnZeroDuration = true;
				return result;
			}
			return animationType switch
			{
				AnimationType.Simple => TypewriterAnimationSimple(), 
				AnimationType.WithPunctuations => TypewriterAnimationWithPunctuations(), 
				AnimationType.ByWords => TypewriterAnimationByWords(), 
				_ => throw new Exception(), 
			};
		}

		public Tween TypewriterAnimationSimple()
		{
			text.ForceMeshUpdate();
			int characterCount = text.textInfo.characterCount;
			float duration = (float)characterCount / charsPerSecond;
			return Tween.TextMaxVisibleCharacters(text, 0, characterCount, duration, Ease.Linear);
		}

		public Tween TypewriterAnimationWithPunctuations()
		{
			text.ForceMeshUpdate();
			RemapWithPunctuations(text, 2147483647, out var remappedCount, out var _);
			float duration = (float)remappedCount / charsPerSecond;
			return Tween.Custom(this, 0f, remappedCount, duration, delegate(TypewriterAnimatorExample t, float x)
			{
				t.UpdateMaxVisibleCharsWithPunctuation(x);
			}, Ease.Linear);
		}

		private void UpdateMaxVisibleCharsWithPunctuation(float progress)
		{
			int remappedEndIndex = Mathf.RoundToInt(progress);
			RemapWithPunctuations(text, remappedEndIndex, out var _, out var visibleCharsCount);
			if (text.maxVisibleCharacters != visibleCharsCount)
			{
				text.maxVisibleCharacters = visibleCharsCount;
			}
		}

		private void RemapWithPunctuations([NotNull] TMP_Text text, int remappedEndIndex, out int remappedCount, out int visibleCharsCount)
		{
			remappedCount = 0;
			visibleCharsCount = 0;
			int characterCount = text.textInfo.characterCount;
			TMP_CharacterInfo[] characterInfo = text.textInfo.characterInfo;
			for (int i = 0; i < characterCount; i++)
			{
				if (remappedCount >= remappedEndIndex)
				{
					break;
				}
				remappedCount++;
				visibleCharsCount++;
				if (IsPunctuationChar(characterInfo[i].character))
				{
					int num = i + 1;
					if (num != characterCount && !IsPunctuationChar(characterInfo[num].character))
					{
						remappedCount += Mathf.Max(0, pauseAfterPunctuation);
					}
				}
			}
			static bool IsPunctuationChar(char c)
			{
				return ".,:;!?".IndexOf(c) != -1;
			}
		}

		public Tween TypewriterAnimationByWords()
		{
			text.ForceMeshUpdate();
			RemapWords(text, 2147483647, out var remappedCount, out var _);
			float duration = (float)text.textInfo.characterCount / charsPerSecond;
			return Tween.Custom(this, 0f, remappedCount, duration, delegate(TypewriterAnimatorExample t, float x)
			{
				t.UpdateVisibleWords(x);
			}, Ease.Linear);
		}

		private void UpdateVisibleWords(float progress)
		{
			int remappedEndIndex = Mathf.RoundToInt(progress);
			RemapWords(text, remappedEndIndex, out var _, out var visibleCharsCount);
			if (text.maxVisibleCharacters != visibleCharsCount)
			{
				text.maxVisibleCharacters = visibleCharsCount;
			}
		}

		private static void RemapWords([NotNull] TMP_Text text, int remappedEndIndex, out int remappedCount, out int visibleCharsCount)
		{
			visibleCharsCount = 0;
			int characterCount = text.textInfo.characterCount;
			if (characterCount == 0)
			{
				remappedCount = 0;
				return;
			}
			remappedCount = 1;
			TMP_CharacterInfo[] characterInfo = text.textInfo.characterInfo;
			for (int i = 0; i < characterCount; i++)
			{
				if (remappedCount >= remappedEndIndex)
				{
					return;
				}
				remappedCount++;
				if (IsWordSeparatorChar(characterInfo[i].character))
				{
					int num = i + 1;
					if (num == characterCount || !IsWordSeparatorChar(characterInfo[num].character))
					{
						remappedCount++;
						visibleCharsCount = num;
					}
				}
			}
			visibleCharsCount = characterCount;
			static bool IsWordSeparatorChar(char ch)
			{
				return " \n".IndexOf(ch) != -1;
			}
		}
	}
}
