using System.Collections;
using Febucci.Attributes;
using Febucci.UI.Core;
using UnityEngine;

namespace Febucci.UI
{
	[HelpURL("https://www.febucci.com/text-animator-unity/docs/text-animator-players/")]
	[AddComponentMenu("Febucci/TextAnimator/TextAnimatorPlayer")]
	public class TextAnimatorPlayer : TAnimPlayerBase
	{
		[SerializeField]
		[CharsDisplayTime]
		[Tooltip("Wait time for normal letters")]
		public float waitForNormalChars = 0.03f;

		[SerializeField]
		[CharsDisplayTime]
		[Tooltip("Wait time for ! ? .")]
		public float waitLong = 0.6f;

		[SerializeField]
		[CharsDisplayTime]
		[Tooltip("Wait time for ; : ) - ,")]
		public float waitMiddle = 0.2f;

		[SerializeField]
		[Tooltip("-True: only the last punctuaction on a sequence waits for its category time.\n-False: each punctuaction will wait, regardless if it's in a sequence or not")]
		private bool avoidMultiplePunctuactionWait;

		[SerializeField]
		[Tooltip("True if you want the typewriter to wait for new line characters")]
		private bool waitForNewLines = true;

		[SerializeField]
		[Tooltip("True if you want the typewriter to wait for all characters, false if you want to skip waiting for the last one")]
		private bool waitForLastCharacter = true;

		[SerializeField]
		[Tooltip("True if you want to use the same typewriter's wait times for the disappearance progression, false if you want to use a different wait time")]
		private bool useTypewriterWaitForDisappearances = true;

		[SerializeField]
		[CharsDisplayTime]
		[Tooltip("Wait time for characters in the disappearance progression")]
		private float disappearanceWaitTime = 0.015f;

		[SerializeField]
		[MinValue(0.1f)]
		[Tooltip("How much faster/slower is the disappearance progression compared to the typewriter's typing speed")]
		private float disappearanceSpeedMultiplier = 1f;

		protected override float GetWaitAppearanceTimeOf(char character)
		{
			if (!waitForLastCharacter && base.textAnimator.allLettersShown)
			{
				return 0f;
			}
			if (avoidMultiplePunctuactionWait && char.IsPunctuation(character) && base.textAnimator.TryGetNextCharacter(out var result) && char.IsPunctuation(result.character))
			{
				return waitForNormalChars;
			}
			if (!waitForNewLines && !base.textAnimator.latestCharacterShown.isVisible && IsUnicodeNewLine(base.textAnimator.latestCharacterShown.textElement.unicode))
			{
				return 0f;
			}
			switch (character)
			{
			case ')':
			case ',':
			case '-':
			case ':':
			case ';':
				return waitMiddle;
			case '!':
			case '.':
			case '?':
				return waitLong;
			default:
				return waitForNormalChars;
			}
			static bool IsUnicodeNewLine(ulong unicode)
			{
				if (unicode != 10)
				{
					return unicode == 13;
				}
				return true;
			}
		}

		protected override float GetWaitDisappearanceTimeOf(char character)
		{
			if (useTypewriterWaitForDisappearances)
			{
				return base.GetWaitDisappearanceTimeOf(character) * (1f / disappearanceSpeedMultiplier);
			}
			return disappearanceWaitTime;
		}

		protected override IEnumerator WaitInput()
		{
			while (!Input.anyKeyDown)
			{
				yield return null;
			}
		}
	}
}
