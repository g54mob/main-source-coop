using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Febucci.UI.Core
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(TextAnimator))]
	public abstract class TAnimPlayerBase : MonoBehaviour
	{
		[Flags]
		private enum StartTypewriterMode
		{
			FromScriptOnly = 0,
			OnEnable = 1,
			OnShowText = 2,
			AutomaticallyFromAllEvents = 3
		}

		public enum DisappearanceOrientation
		{
			SameAsTypewriter = 0,
			Inverted = 1
		}

		private string textToShow = string.Empty;

		private TextAnimator _textAnimator;

		private bool isInsideRoutine;

		private bool isDisappearing;

		protected bool wantsToSkip;

		[Tooltip("True if you want to shows the text dynamically")]
		[SerializeField]
		public bool useTypeWriter = true;

		[SerializeField]
		[Tooltip("Controls from which method(s) the typewriter will automatically start/resume. Default is 'Automatic'")]
		private StartTypewriterMode startTypewriterMode = StartTypewriterMode.AutomaticallyFromAllEvents;

		[SerializeField]
		private bool canSkipTypewriter = true;

		[SerializeField]
		private bool hideAppearancesOnSkip;

		[SerializeField]
		[Tooltip("True = plays all remaining events once the typewriter has been skipped")]
		private bool triggerEventsOnSkip;

		[SerializeField]
		[Tooltip("True = resets the typewriter speed every time a new text is set/shown")]
		private bool resetTypingSpeedAtStartup = true;

		protected float typewriterPlayerSpeed = 1f;

		[SerializeField]
		public DisappearanceOrientation disappearanceOrientation;

		public UnityEvent onTextShowed;

		public UnityEvent onTypewriterStart;

		public UnityEvent onTextDisappeared;

		public CharacterEvent onCharacterVisible;

		public TextAnimator textAnimator
		{
			get
			{
				if (_textAnimator != null)
				{
					return _textAnimator;
				}
				if (!TryGetComponent<TextAnimator>(out _textAnimator))
				{
					Debug.LogError("TextAnimator: Text Animator component is null on GameObject " + base.gameObject.name);
				}
				return _textAnimator;
			}
		}

		protected bool isBaseInsideRoutine => isInsideRoutine;

		[HideInInspector]
		public bool isWaitingForPlayerInput { get; private set; }

		private IEnumerator ShowRemainingCharacters()
		{
			if (textAnimator.allLettersShown)
			{
				yield break;
			}
			isInsideRoutine = true;
			isWaitingForPlayerInput = false;
			isDisappearing = false;
			wantsToSkip = false;
			onTypewriterStart?.Invoke();
			if (resetTypingSpeedAtStartup)
			{
				typewriterPlayerSpeed = 1f;
			}
			float typewriterTagsSpeed = 1f;
			float timePassed = 0f;
			float deltaTime = default(float);
			UpdateDeltaTime();
			while (!textAnimator.allLettersShown)
			{
				if (textAnimator.hasActions)
				{
					TypewriterAction action;
					while (textAnimator.TryGetAction(out action))
					{
						switch (action.actionID)
						{
						case "waitfor":
						{
							FormatUtils.TryGetFloat(action.parameters, 0, 1f, out var result);
							yield return WaitTime(result);
							break;
						}
						case "waitinput":
							isWaitingForPlayerInput = true;
							yield return WaitInput();
							isWaitingForPlayerInput = false;
							break;
						case "speed":
							FormatUtils.TryGetFloat(action.parameters, 0, 1f, out typewriterTagsSpeed);
							if (typewriterTagsSpeed <= 0f)
							{
								typewriterTagsSpeed = 0.001f;
							}
							break;
						default:
							yield return DoCustomAction(action);
							break;
						}
					}
				}
				textAnimator.maxVisibleCharacters++;
				textAnimator.TriggerVisibleEvents();
				char character = textAnimator.latestCharacterShown.character;
				UpdateDeltaTime();
				if (character != ' ')
				{
					onCharacterVisible?.Invoke(character);
				}
				float timeToWait = GetWaitAppearanceTimeOf(character);
				if (timeToWait < deltaTime)
				{
					timePassed += timeToWait;
					if (timePassed >= deltaTime)
					{
						yield return null;
						timePassed %= deltaTime;
					}
				}
				else
				{
					while (timePassed < timeToWait && !HasSkipped())
					{
						OnTypewriterCharDelay();
						timePassed += deltaTime;
						yield return null;
						UpdateDeltaTime();
					}
					timePassed %= timeToWait;
				}
				if (HasSkipped())
				{
					textAnimator.ShowAllCharacters(hideAppearancesOnSkip);
					if (triggerEventsOnSkip)
					{
						textAnimator.TriggerRemainingEvents();
					}
					break;
				}
			}
			if (!canSkipTypewriter || !wantsToSkip)
			{
				textAnimator.TriggerRemainingEvents();
			}
			isInsideRoutine = false;
			isWaitingForPlayerInput = false;
			textToShow = string.Empty;
			onTextShowed?.Invoke();
			bool HasSkipped()
			{
				if (canSkipTypewriter)
				{
					return wantsToSkip;
				}
				return false;
			}
			void UpdateDeltaTime()
			{
				deltaTime = textAnimator.time.deltaTime * typewriterPlayerSpeed * typewriterTagsSpeed;
			}
			IEnumerator WaitTime(float time)
			{
				if (time > 0f)
				{
					float t = 0f;
					while (t <= time && !HasSkipped())
					{
						t += textAnimator.time.deltaTime;
						yield return null;
					}
				}
			}
		}

		public void ShowText(string text)
		{
			StopShowingText();
			if (string.IsNullOrEmpty(text))
			{
				textToShow = string.Empty;
				textAnimator.SetText(string.Empty, hideText: true);
				return;
			}
			textToShow = text;
			isWaitingForPlayerInput = false;
			wantsToSkip = false;
			textAnimator.SetText(textToShow, useTypeWriter);
			textAnimator.firstVisibleCharacter = 0;
			isDisappearing = false;
			if (!useTypeWriter)
			{
				onTextShowed?.Invoke();
			}
			else if (startTypewriterMode.HasFlag(StartTypewriterMode.OnShowText))
			{
				StartShowingText();
			}
		}

		private bool CanStartAnyCoroutine()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				Debug.LogWarning("TextAnimator: couldn't start coroutine because the gameobject is not active");
				return false;
			}
			return true;
		}

		public void StartShowingText(bool resetVisibleCharacters = false)
		{
			if (!useTypeWriter)
			{
				Debug.LogWarning("TextAnimator: couldn't start coroutine because 'useTypewriter' is disabled");
			}
			else if (CanStartAnyCoroutine())
			{
				if (resetVisibleCharacters)
				{
					textAnimator.firstVisibleCharacter = 0;
					textAnimator.maxVisibleCharacters = 0;
				}
				if (!isInsideRoutine)
				{
					StartCoroutine(ShowRemainingCharacters());
				}
			}
		}

		[ContextMenu("Skip Typewriter")]
		public void SkipTypewriter()
		{
			wantsToSkip = true;
		}

		[ContextMenu("Stop Showing Text")]
		public void StopShowingText()
		{
			if (isInsideRoutine)
			{
				isInsideRoutine = false;
				StopAllCoroutines();
			}
			textToShow = string.Empty;
		}

		[ContextMenu("Start Disappearing Text")]
		public void StartDisappearingText()
		{
			if (CanStartAnyCoroutine())
			{
				if (disappearanceOrientation == DisappearanceOrientation.Inverted && isInsideRoutine)
				{
					Debug.LogWarning("TextAnimatorPlayer: Can't start disappearance routine in the opposite direction of the typewriter, because you're still showing the text! (the typewriter might get stuck trying to show and override letters that keep disappearing)");
				}
				else
				{
					StartCoroutine(DisappearRoutine());
				}
			}
		}

		private IEnumerator DisappearRoutine()
		{
			isDisappearing = true;
			float t = 0f;
			float deltaTime = 0f;
			UpdateDeltaTime();
			if (disappearanceOrientation == DisappearanceOrientation.SameAsTypewriter)
			{
				TMP_CharacterInfo[] charInfo = textAnimator.tmproText.textInfo.characterInfo;
				while (CanDisappear() && textAnimator.firstVisibleCharacter < charInfo.Length)
				{
					textAnimator.firstVisibleCharacter++;
					float waitDisappearanceTimeOf = GetWaitDisappearanceTimeOf(charInfo[textAnimator.firstVisibleCharacter - 1].character);
					if (waitDisappearanceTimeOf < deltaTime)
					{
						t += waitDisappearanceTimeOf;
						if (t >= deltaTime)
						{
							yield return null;
							t %= deltaTime;
						}
					}
					else
					{
						yield return WaitFor(waitDisappearanceTimeOf);
					}
				}
			}
			else
			{
				while (CanDisappear())
				{
					textAnimator.maxVisibleCharacters--;
					float waitDisappearanceTimeOf2 = GetWaitDisappearanceTimeOf(textAnimator.latestCharacterShown.character);
					if (waitDisappearanceTimeOf2 < deltaTime)
					{
						t += waitDisappearanceTimeOf2;
						if (t >= deltaTime)
						{
							yield return null;
							t %= deltaTime;
						}
					}
					else
					{
						yield return WaitFor(waitDisappearanceTimeOf2);
					}
				}
			}
			while (textAnimator.anyLetterVisible)
			{
				yield return null;
			}
			if ((textAnimator.firstVisibleCharacter > textAnimator.maxVisibleCharacters && textAnimator.allLettersShown) || textAnimator.maxVisibleCharacters == 0)
			{
				onTextDisappeared.Invoke();
			}
			isDisappearing = false;
			bool CanDisappear()
			{
				if (isDisappearing && textAnimator.firstVisibleCharacter <= textAnimator.maxVisibleCharacters)
				{
					return textAnimator.maxVisibleCharacters > 0;
				}
				return false;
			}
			void UpdateDeltaTime()
			{
				deltaTime = textAnimator.time.deltaTime * typewriterPlayerSpeed;
			}
			IEnumerator WaitFor(float timeToWait)
			{
				if (!(timeToWait <= 0f))
				{
					while (t < timeToWait)
					{
						t += deltaTime;
						yield return null;
						UpdateDeltaTime();
					}
					t %= timeToWait;
				}
			}
		}

		[ContextMenu("Stop Disappearing Text")]
		public void StopDisappearingText()
		{
			isDisappearing = false;
		}

		public void SetTypewriterSpeed(float value)
		{
			typewriterPlayerSpeed = Mathf.Clamp(value, 0.001f, value);
		}

		protected abstract IEnumerator WaitInput();

		protected abstract float GetWaitAppearanceTimeOf(char character);

		[Obsolete("'WaitTimeOf' is obsolete and will be removed from the next versions. Pleaase use 'GetWaitAppearanceTimeOf' instead.")]
		protected virtual void WaitTimeOf(char character)
		{
			GetWaitAppearanceTimeOf(character);
		}

		protected virtual float GetWaitDisappearanceTimeOf(char character)
		{
			return GetWaitAppearanceTimeOf(character);
		}

		protected virtual IEnumerator DoCustomAction(TypewriterAction action)
		{
			throw new NotImplementedException("TextAnimator: Custom Action not implemented with type: " + action.actionID + ". If you did implement it, please do not call the base method from your overridden one.");
		}

		protected virtual void OnTypewriterCharDelay()
		{
		}

		protected virtual void OnDisable()
		{
			isInsideRoutine = false;
		}

		protected virtual void OnEnable()
		{
			if (useTypeWriter && startTypewriterMode.HasFlag(StartTypewriterMode.OnEnable))
			{
				StartShowingText();
			}
		}
	}
}
