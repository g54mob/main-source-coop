using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Febucci.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Febucci.UI
{
	[HelpURL("https://www.febucci.com/text-animator-unity/docs/how-to-add-effects-to-your-texts/")]
	[AddComponentMenu("Febucci/TextAnimator/TextAnimator")]
	[RequireComponent(typeof(TMP_Text))]
	[DisallowMultipleComponent]
	public class TextAnimator : MonoBehaviour
	{
		[Serializable]
		public struct TimeData
		{
			public float timeSinceStart { get; private set; }

			public float deltaTime { get; private set; }

			internal void ResetData()
			{
				timeSinceStart = 0f;
			}

			internal void IncreaseTime()
			{
				timeSinceStart += deltaTime;
			}

			internal void UpdateDeltaTime(TimeScale timeScale)
			{
				deltaTime = ((timeScale == TimeScale.Unscaled) ? Time.unscaledDeltaTime : Time.deltaTime);
				if (deltaTime < 0f)
				{
					deltaTime = 0f;
				}
			}
		}

		[Serializable]
		private class AppearancesContainer
		{
			[SerializeField]
			[FormerlySerializedAs("tags")]
			public string[] tagsFallback_Appearances = new string[1] { "size" };

			public string[] tagsFallback_Disappearances = new string[1] { "size" };

			public AppearanceDefaultValues values = new AppearanceDefaultValues();
		}

		internal struct InternalAction
		{
			public TypewriterAction action;

			public int charIndex;

			public bool triggered;

			public int internalOrder;
		}

		private enum ShowTextMode : byte
		{
			Hidden = 0,
			Shown = 1,
			UserTyping = 2
		}

		public enum TimeScale
		{
			Scaled = 0,
			Unscaled = 1
		}

		public delegate void MessageEvent(string message);

		private TAnimPlayerBase _tAnimPlayer;

		[SerializeField]
		[Tooltip("If true, the typewriter is triggered automatically once the TMPro text changes (requires a TextAnimatorPlayer component). Otherwise, it shows the entire text instantly.")]
		private bool triggerAnimPlayerOnChange;

		[SerializeField]
		public float effectIntensityMultiplier = 50f;

		[FormerlySerializedAs("defaultAppearance")]
		[SerializeField]
		[Header("Text Appearance")]
		private AppearancesContainer appearancesContainer = new AppearancesContainer();

		[SerializeField]
		private string[] tags_fallbackBehaviors = new string[0];

		[SerializeField]
		private BehaviorDefaultValues behaviorValues = new BehaviorDefaultValues();

		[SerializeField]
		private BuiltinBehaviorsDataScriptable scriptable_globalBehaviorsValues;

		[SerializeField]
		private BuiltinAppearancesDataScriptable scriptable_globalAppearancesValues;

		[SerializeField]
		[Tooltip("True if you want effects to have the same intensities even if text is larger/smaller than default (example: when TMPro's AutoSize changes the size based on screen size)")]
		public bool useDynamicScaling;

		[SerializeField]
		[Tooltip("Used for scaling, represents the text's size where/when effects intensity behave like intended.")]
		public float referenceFontSize = -1f;

		[SerializeField]
		[Tooltip("True if you want effects time to be reset when a new text is set (default option), false otherwise.")]
		public bool isResettingEffectsOnNewText = true;

		private TMP_Text _tmproText;

		public TimeScale timeScale;

		private string latestText;

		private TimeData m_time;

		private bool forceMeshRefresh;

		private bool skipAppearanceEffects;

		private bool hasParentCanvas;

		private Canvas parentCanvas;

		private TMP_InputField attachedInputField;

		private bool autoSize;

		private Rect sourceRect;

		private Color sourceColor;

		private int _maxVisibleCharacters;

		private int _firstVisibleCharacter;

		private bool hasText;

		private int latestTriggeredEvent;

		private int latestTriggeredAction;

		private TMP_TextInfo textInfo;

		private Character[] characters = new Character[0];

		private List<BehaviorBase> behaviorEffects = new List<BehaviorBase>();

		private List<AppearanceBase> appearanceEffects = new List<AppearanceBase>();

		private List<AppearanceBase> disappearanceEffects = new List<AppearanceBase>();

		private AppearanceBase[] fallbackAppearanceEffects;

		private AppearanceBase[] fallbackDisappearanceEffects;

		private BehaviorBase[] fallbackBehaviorEffects;

		private List<InternalAction> typewriterActions = new List<InternalAction>();

		private List<EventMarker> eventMarkers = new List<EventMarker>();

		private static bool enabled_globalAppearances = true;

		private static bool enabled_globalBehaviors = true;

		private bool enabled_localBehaviors = true;

		private bool enabled_localAppearances = true;

		private bool databaseBuilt;

		private Dictionary<string, Type> localBehaviors = new Dictionary<string, Type>();

		private Dictionary<string, Type> localAppearances = new Dictionary<string, Type>();

		private const char m_closureSymbol = '/';

		private const char m_eventSymbol = '?';

		private const char m_disappearanceSymbol = '#';

		private bool noparseEnabled;

		private int internalEventActionIndex;

		private List<int> temp_effectsToApply = new List<int>();

		private int tmpFirstVisibleCharacter;

		private int tmpMaxVisibleCharacters;

		private TAnimPlayerBase tAnimPlayer
		{
			get
			{
				if (_tAnimPlayer != null)
				{
					return _tAnimPlayer;
				}
				if (!TryGetComponent<TAnimPlayerBase>(out _tAnimPlayer))
				{
					Debug.LogError("Text Animator component is null on GameObject " + base.gameObject.name);
				}
				return _tAnimPlayer;
			}
		}

		public TMP_Text tmproText
		{
			get
			{
				if (_tmproText != null)
				{
					return _tmproText;
				}
				if (!TryGetComponent<TMP_Text>(out _tmproText))
				{
					Debug.LogError("TextAnimator: TMproText component is null.");
				}
				return _tmproText;
			}
			private set
			{
				_tmproText = value;
			}
		}

		public string text
		{
			get
			{
				return latestText;
			}
			private set
			{
				latestText = value;
			}
		}

		public bool allLettersShown => _maxVisibleCharacters >= tmproText.textInfo.characterCount;

		public bool anyLetterVisible
		{
			get
			{
				if (characters.Length == 0)
				{
					return true;
				}
				if (IsCharacterVisible(0) || IsCharacterVisible(tmproText.textInfo.characterCount - 1))
				{
					return true;
				}
				for (int i = 1; i < tmproText.textInfo.characterCount - 1; i++)
				{
					if (IsCharacterVisible(i))
					{
						return true;
					}
				}
				return false;
				bool IsCharacterVisible(int index)
				{
					return characters[index].data.passedTime > 0f;
				}
			}
		}

		public TMP_CharacterInfo latestCharacterShown { get; private set; }

		public TimeData time => m_time;

		public int maxVisibleCharacters
		{
			get
			{
				return _maxVisibleCharacters;
			}
			set
			{
				if (_maxVisibleCharacters != value)
				{
					_maxVisibleCharacters = value;
					if (_maxVisibleCharacters < 0)
					{
						_maxVisibleCharacters = 0;
					}
					if (hasText && _maxVisibleCharacters <= textInfo.characterCount && _maxVisibleCharacters > 0)
					{
						latestCharacterShown = textInfo.characterInfo[_maxVisibleCharacters - 1];
					}
					AssertCharacterTimes();
				}
			}
		}

		public int firstVisibleCharacter
		{
			get
			{
				return _firstVisibleCharacter;
			}
			set
			{
				if (_firstVisibleCharacter != value)
				{
					_firstVisibleCharacter = value;
					AssertCharacterTimes();
				}
			}
		}

		internal bool hasActions { get; private set; }

		public static bool effectsBehaviorsEnabled => enabled_globalBehaviors;

		public static bool effectsAppearancesEnabled => enabled_globalAppearances;

		public event MessageEvent onEvent;

		private void Awake()
		{
			Canvas[] componentsInParent = base.gameObject.GetComponentsInParent<Canvas>(includeInactive: true);
			if (componentsInParent.Length != 0)
			{
				parentCanvas = componentsInParent[0];
				hasParentCanvas = parentCanvas != null;
			}
			base.gameObject.TryGetComponent<TMP_InputField>(out attachedInputField);
			if (triggerAnimPlayerOnChange)
			{
				tmproText.renderMode = TextRenderFlags.DontRender;
			}
			m_time.UpdateDeltaTime(timeScale);
		}

		private void AssertCharacterTimes()
		{
			for (int i = 0; i < characters.Length; i++)
			{
				characters[i].wantsToDisappear = !IsCharacterShown(i);
			}
			bool IsCharacterShown(int num)
			{
				if (num <= textInfo.characterCount && num >= _firstVisibleCharacter)
				{
					return num < _maxVisibleCharacters;
				}
				return false;
			}
		}

		public void SetText(string text, bool hideText)
		{
			_SetText(text, (!hideText) ? ShowTextMode.Shown : ShowTextMode.Hidden);
		}

		public void AppendText(string text, bool hideText)
		{
			if (!string.IsNullOrEmpty(text))
			{
				if (!hasText)
				{
					SetText(text, hideText);
				}
				else
				{
					_ApplyTextToCharacters(this.text + _FormatText(text, this.text.Length));
				}
			}
		}

		public bool TryGetNextCharacter(out TMP_CharacterInfo result)
		{
			if (_maxVisibleCharacters < textInfo.characterCount)
			{
				result = textInfo.characterInfo[_maxVisibleCharacters];
				return true;
			}
			result = default(TMP_CharacterInfo);
			return false;
		}

		[Obsolete("Please use 'maxVisibleCharacter++' instead.")]
		public char IncreaseVisibleChars()
		{
			maxVisibleCharacters++;
			return latestCharacterShown.character;
		}

		public void ShowAllCharacters(bool skipAppearanceEffects)
		{
			maxVisibleCharacters = textInfo.characterCount;
			this.skipAppearanceEffects = skipAppearanceEffects;
		}

		public void TriggerRemainingEvents()
		{
			if (eventMarkers.Count <= 0)
			{
				return;
			}
			for (int i = latestTriggeredEvent; i < eventMarkers.Count; i++)
			{
				if (!eventMarkers[i].triggered)
				{
					EventMarker eventMarker = eventMarkers[i];
					eventMarker.triggered = true;
					this.onEvent?.Invoke(eventMarkers[i].eventMessage);
				}
			}
			latestTriggeredEvent = eventMarkers.Count - 1;
		}

		public void ForceMeshRefresh()
		{
			forceMeshRefresh = true;
		}

		public void TriggerVisibleEvents()
		{
			TryTriggeringEvent(int.MaxValue);
		}

		public void ResetEffectsTime(bool skipAppearances)
		{
			if (skipAppearances)
			{
				for (int i = firstVisibleCharacter; i < maxVisibleCharacters; i++)
				{
					characters[i].isDisappearing = false;
					characters[i].data.passedTime = characters[i].appearancesMaxDuration;
				}
			}
			else
			{
				for (int j = firstVisibleCharacter; j < maxVisibleCharacters; j++)
				{
					characters[j].isDisappearing = false;
					characters[j].data.passedTime = 0f;
				}
			}
			m_time.ResetData();
		}

		public static void EnableAllEffects(bool enabled)
		{
			EnableAppearances(enabled);
			EnableBehaviors(enabled);
		}

		public static void EnableAppearances(bool enabled)
		{
			enabled_globalAppearances = enabled;
		}

		public static void EnableBehaviors(bool enabled)
		{
			enabled_globalBehaviors = enabled;
		}

		public void EnableBehaviorsLocally(bool value)
		{
			enabled_localBehaviors = value;
		}

		public void EnableAppearancesLocally(bool value)
		{
			enabled_localAppearances = value;
		}

		private void BuildTagsDatabase()
		{
			if (databaseBuilt)
			{
				return;
			}
			TAnimBuilder.InitializeGlobalDatabase();
			databaseBuilt = true;
			if ((bool)scriptable_globalAppearancesValues)
			{
				appearancesContainer.values.defaults = scriptable_globalAppearancesValues.effectValues;
			}
			if ((bool)scriptable_globalBehaviorsValues)
			{
				behaviorValues.defaults = scriptable_globalBehaviorsValues.effectValues;
			}
			for (int i = 0; i < behaviorValues.presets.Length; i++)
			{
				TAnimBuilder.TryAddingPresetToDictionary(ref localBehaviors, behaviorValues.presets[i].effectTag, typeof(PresetBehavior));
			}
			for (int j = 0; j < appearancesContainer.values.presets.Length; j++)
			{
				TAnimBuilder.TryAddingPresetToDictionary(ref localAppearances, appearancesContainer.values.presets[j].effectTag, typeof(PresetAppearance));
			}
			fallbackAppearanceEffects = GetFallbackAppearancesFromTag(appearancesContainer.tagsFallback_Appearances);
			fallbackDisappearanceEffects = GetFallbackAppearancesFromTag(appearancesContainer.tagsFallback_Disappearances);
			List<BehaviorBase> list = new List<BehaviorBase>();
			for (int k = 0; k < tags_fallbackBehaviors.Length; k++)
			{
				if (tags_fallbackBehaviors[k].Length <= 0)
				{
					continue;
				}
				string[] array = tags_fallbackBehaviors[k].Split(' ');
				string text = array[0];
				foreach (BehaviorBase item in list)
				{
					item.regionManager.entireRichTextTag.Equals(tags_fallbackBehaviors[k]);
				}
				if (TryGetBehaviorClassFromTag(text, tags_fallbackBehaviors[k], 0, out var effectBase))
				{
					effectBase.regionManager.AddRegion(0);
					TryProcessingModifier(array, ref effectBase);
					list.Add(effectBase);
				}
				else
				{
					Debug.LogError("TextAnimator: Behavior Tag '" + tags_fallbackBehaviors[k] + "' is not recognized.", base.gameObject);
				}
			}
			fallbackBehaviorEffects = list.ToArray();
			AppearanceBase[] GetFallbackAppearancesFromTag(string[] tagsToConvert)
			{
				List<AppearanceBase> list2 = new List<AppearanceBase>();
				for (int l = 0; l < tagsToConvert.Length; l++)
				{
					if (tagsToConvert[l].Length > 0)
					{
						string[] array2 = tagsToConvert[l].Split(' ');
						string text2 = array2[0];
						foreach (AppearanceBase item2 in list2)
						{
							item2.regionManager.entireRichTextTag.Equals(tagsToConvert[l]);
						}
						if (TryGetAppearingClassFromTag(text2, tagsToConvert[l], 0, out var effectBase2))
						{
							effectBase2.SetDefaultValues(appearancesContainer.values);
							TryProcessingModifier(array2, ref effectBase2);
							effectBase2.regionManager.AddRegion(0);
							list2.Add(effectBase2);
						}
						else
						{
							Debug.LogError("TextAnimator: Effect Tag '" + tagsToConvert[l] + "' is not recognized.", base.gameObject);
						}
					}
				}
				return list2.ToArray();
			}
		}

		private bool TryGetBehaviorClassFromTag(string tag, string entireRichTextTag, int regionStartIndex, out BehaviorBase effectBase)
		{
			if (TAnimBuilder.TryGetGlobalBehaviorFromTag(tag, entireRichTextTag, out effectBase))
			{
				effectBase.SetDefaultValues(behaviorValues);
				effectBase.regionManager.AddRegion(regionStartIndex);
				return true;
			}
			if (TAnimBuilder.TryGetEffectClassFromTag<BehaviorBase>(localBehaviors, tag, entireRichTextTag, out effectBase))
			{
				effectBase.SetDefaultValues(behaviorValues);
				effectBase.regionManager.AddRegion(regionStartIndex);
				return true;
			}
			effectBase = null;
			return false;
		}

		private bool TryGetAppearingClassFromTag(string tag, string entireRichTextTag, int startIndex, out AppearanceBase effectBase)
		{
			if (TAnimBuilder.TryGetGlobalAppearanceFromTag(tag, entireRichTextTag, out effectBase))
			{
				effectBase.regionManager.AddRegion(startIndex);
				return true;
			}
			if (TAnimBuilder.TryGetEffectClassFromTag<AppearanceBase>(localAppearances, tag, entireRichTextTag, out effectBase))
			{
				effectBase.regionManager.AddRegion(startIndex);
				return true;
			}
			effectBase = null;
			return false;
		}

		private bool TryProcessingAppearanceTag(string richTextTag, int realTextIndex)
		{
			if (richTextTag[0] == '/')
			{
				if (richTextTag.Length > 1 && richTextTag[1] == '#')
				{
					return disappearanceEffects.CloseSingleOrAllEffects(richTextTag.Substring(2, richTextTag.Length - 2), realTextIndex);
				}
				return appearanceEffects.CloseSingleOrAllEffects(richTextTag.Substring(1, richTextTag.Length - 1), realTextIndex);
			}
			if (richTextTag[0] == '#')
			{
				richTextTag = richTextTag.Substring(1, richTextTag.Length - 1);
				return ProcessOpeningTag(disappearanceEffects);
			}
			return ProcessOpeningTag(appearanceEffects);
			bool ProcessOpeningTag(List<AppearanceBase> effectsList)
			{
				for (int i = 0; i < effectsList.Count; i++)
				{
					if (effectsList[i].regionManager.TryReutilizingWithTag(richTextTag, realTextIndex))
					{
						return true;
					}
				}
				string[] array = richTextTag.Split(' ');
				if (TryGetAppearingClassFromTag(array[0], richTextTag, realTextIndex, out var effectBase))
				{
					effectBase.SetDefaultValues(appearancesContainer.values);
					TryProcessingModifier(array, ref effectBase);
					effectsList.TryAddingNewRegion(effectBase);
					return true;
				}
				return false;
			}
		}

		private void TryProcessingModifier<T>(string[] tags, ref T effect) where T : EffectsBase
		{
			for (int i = 1; i < tags.Length; i++)
			{
				int num = tags[i].IndexOf('=');
				if (num >= 0)
				{
					string modifierName = tags[i].Substring(0, num);
					string modifierValue = tags[i].Substring(num + 1);
					effect.SetModifier(modifierName, modifierValue);
				}
			}
		}

		private bool TryProcessingBehaviorTag(string richTextTag, string loweredRichTextTag, int realTextIndex, ref int internalEventActionIndex)
		{
			if (loweredRichTextTag[0] == '?')
			{
				richTextTag = richTextTag.Substring(1, richTextTag.Length - 1);
				if (richTextTag.Length == 0)
				{
					return false;
				}
				eventMarkers.Add(new EventMarker
				{
					charIndex = realTextIndex,
					eventMessage = richTextTag,
					internalOrder = internalEventActionIndex
				});
				internalEventActionIndex++;
				return true;
			}
			if (loweredRichTextTag[0] == '/')
			{
				loweredRichTextTag = loweredRichTextTag.Substring(1, loweredRichTextTag.Length - 1);
				bool result = false;
				if (loweredRichTextTag.Length <= 0)
				{
					for (int i = 0; i < behaviorEffects.Count; i++)
					{
						result = behaviorEffects.CloseElement(i, realTextIndex);
					}
				}
				else
				{
					result = behaviorEffects.CloseRegionNamed(loweredRichTextTag, realTextIndex);
				}
				return result;
			}
			for (int j = 0; j < behaviorEffects.Count; j++)
			{
				if (behaviorEffects[j].regionManager.TryReutilizingWithTag(loweredRichTextTag, realTextIndex))
				{
					return true;
				}
			}
			string[] array = loweredRichTextTag.Split(' ');
			if (TryGetBehaviorClassFromTag(array[0], loweredRichTextTag, realTextIndex, out var effectBase))
			{
				effectBase.SetDefaultValues(behaviorValues);
				TryProcessingModifier(array, ref effectBase);
				behaviorEffects.TryAddingNewRegion(effectBase);
				return true;
			}
			return false;
		}

		private bool TryProcessingActionTag(string entireTag, int realTextIndex, ref int internalEventActionIndex)
		{
			string text = entireTag.Substring(1, entireTag.Length - 2);
			int num = entireTag.IndexOf('=');
			if (num >= 0)
			{
				text = text.Substring(0, num - 1);
			}
			if (TAnimBuilder.IsDefaultAction(text) || TAnimBuilder.IsCustomAction(text))
			{
				hasActions = true;
				InternalAction item = default(InternalAction);
				item.action = default(TypewriterAction);
				item.action.actionID = text;
				item.charIndex = realTextIndex;
				item.action.parameters = new List<string>();
				if (num >= 0)
				{
					string text2 = entireTag.Substring(text.Length + 2);
					text2 = text2.Substring(0, text2.Length - 1);
					item.action.parameters = text2.Split(',').ToList();
				}
				item.internalOrder = internalEventActionIndex;
				typewriterActions.Add(item);
				internalEventActionIndex++;
				return true;
			}
			return false;
		}

		private void _SetText(string text, ShowTextMode showTextMode)
		{
			if (text.Length <= 0)
			{
				hasText = false;
				text = string.Empty;
				tmproText.text = string.Empty;
				tmproText.ClearMesh();
				return;
			}
			BuildTagsDatabase();
			skipAppearanceEffects = false;
			hasActions = false;
			noparseEnabled = false;
			if (isResettingEffectsOnNewText)
			{
				m_time.ResetData();
			}
			behaviorEffects.Clear();
			appearanceEffects.Clear();
			disappearanceEffects.Clear();
			eventMarkers.Clear();
			typewriterActions.Clear();
			latestTriggeredEvent = 0;
			latestTriggeredAction = 0;
			internalEventActionIndex = 0;
			for (int i = 0; i < fallbackAppearanceEffects.Length; i++)
			{
				appearanceEffects.Add(fallbackAppearanceEffects[i]);
			}
			for (int j = 0; j < fallbackDisappearanceEffects.Length; j++)
			{
				disappearanceEffects.Add(fallbackDisappearanceEffects[j]);
			}
			for (int k = 0; k < fallbackBehaviorEffects.Length; k++)
			{
				behaviorEffects.Add(fallbackBehaviorEffects[k]);
			}
			_ApplyTextToCharacters(_FormatText(text, 0));
			switch (showTextMode)
			{
			case ShowTextMode.Hidden:
				HideAllCharacters();
				break;
			case ShowTextMode.Shown:
				ShowAllCharacters();
				break;
			case ShowTextMode.UserTyping:
				maxVisibleCharacters = textInfo.characterCount + 1;
				if (_maxVisibleCharacters - 1 < characters.Length && _maxVisibleCharacters - 1 >= 0)
				{
					HideCharacter(_maxVisibleCharacters - 1);
				}
				break;
			}
			void HideAllCharacters()
			{
				_maxVisibleCharacters = 0;
				for (int l = 0; l < textInfo.characterCount; l++)
				{
					HideCharacter(l);
				}
				if (_maxVisibleCharacters <= 0 && characters.Length != 0)
				{
					HideCharacter(0);
				}
			}
			void HideCharacter(int num)
			{
				characters[num].data.passedTime = 0f;
				characters[num].isDisappearing = true;
				characters[num].wantsToDisappear = true;
				characters[num].Hide();
			}
			void ShowAllCharacters()
			{
				_maxVisibleCharacters = textInfo.characterCount;
				for (int l = 0; l < textInfo.characterCount; l++)
				{
					characters[l].data.passedTime = 0f;
					characters[l].isDisappearing = false;
					characters[l].wantsToDisappear = false;
				}
			}
		}

		private string _FormatText(string text, int startCharacterIndex)
		{
			StringBuilder temp_realText = new StringBuilder();
			temp_realText.Clear();
			int i = 0;
			int realTextIndex = startCharacterIndex;
			string entireTag;
			for (; i < text.Length; i++)
			{
				if (TryGetClosingCharacter(out var _closingCharacter))
				{
					int num = text.IndexOf(text[i], i + 1);
					int num2 = text.IndexOf(_closingCharacter, i + 1);
					if (num2 >= 0 && (num > num2 || num < 0))
					{
						entireTag = text.Substring(i, num2 - i + 1);
						string text2 = entireTag.Substring(1, entireTag.Length - 2);
						string text3 = text2.ToLower();
						if (text3.Length < 1)
						{
							AppendCurrentTagToText();
						}
						else if (_closingCharacter == TAnimBuilder.tag_appearances.charClosingTag)
						{
							if (noparseEnabled || !TryProcessingAppearanceTag(text3, realTextIndex))
							{
								AppendCurrentTagToText();
							}
						}
						else
						{
							switch (text3)
							{
							case "noparse":
								noparseEnabled = true;
								AppendCurrentTagToText();
								break;
							case "/noparse":
								noparseEnabled = false;
								AppendCurrentTagToText();
								break;
							default:
								if (noparseEnabled)
								{
									AppendCurrentTagToText();
								}
								else if (!TryProcessingBehaviorTag(text2, text3, realTextIndex, ref internalEventActionIndex) && !TryProcessingActionTag(entireTag, realTextIndex, ref internalEventActionIndex))
								{
									AppendCurrentTagToText();
								}
								break;
							}
						}
						i = num2;
					}
					else
					{
						AppendCurrentCharacterToText();
					}
				}
				else
				{
					AppendCurrentCharacterToText();
				}
			}
			return temp_realText.ToString();
			void AppendCurrentCharacterToText()
			{
				temp_realText.Append(text[i]);
				realTextIndex++;
			}
			void AppendCurrentTagToText()
			{
				temp_realText.Append(entireTag);
				realTextIndex += entireTag.Length;
			}
			bool TryGetClosingCharacter(out char reference)
			{
				if (text[i] == TAnimBuilder.tag_behaviors.charOpeningTag)
				{
					reference = TAnimBuilder.tag_behaviors.charClosingTag;
					return true;
				}
				if (text[i] == TAnimBuilder.tag_appearances.charOpeningTag)
				{
					reference = TAnimBuilder.tag_appearances.charClosingTag;
					return true;
				}
				reference = '\0';
				return false;
			}
		}

		private void _ApplyTextToCharacters(string text)
		{
			tmproText.renderMode = TextRenderFlags.DontRender;
			if ((bool)attachedInputField)
			{
				attachedInputField.text = text;
			}
			else
			{
				tmproText.text = text;
			}
			tmproText.ForceMeshUpdate(ignoreActiveState: true);
			textInfo = tmproText.GetTextInfo(tmproText.text);
			if (characters.Length < textInfo.characterCount)
			{
				Array.Resize(ref characters, textInfo.characterCount);
			}
			foreach (AppearanceBase appearanceEffect in appearanceEffects)
			{
				appearanceEffect.Initialize(characters.Length);
			}
			foreach (AppearanceBase disappearanceEffect in disappearanceEffects)
			{
				disappearanceEffect.Initialize(characters.Length);
			}
			foreach (BehaviorBase behaviorEffect in behaviorEffects)
			{
				behaviorEffect.Initialize(characters.Length);
			}
			int i;
			for (i = 0; i < textInfo.characterCount; i++)
			{
				characters[i].data.tmp_CharInfo = textInfo.characterInfo[i];
				if (!characters[i].initialized)
				{
					characters[i].sources.vertices = new Vector3[4];
					characters[i].sources.colors = new Color32[4];
					characters[i].data.vertices = new Vector3[4];
					characters[i].data.colors = new Color32[4];
				}
				SetEffectsDependency<BehaviorBase>(ref characters[i].indexBehaviorEffects, behaviorEffects, fallbackBehaviorEffects.Length);
				SetEffectsDependency<AppearanceBase>(ref characters[i].indexAppearanceEffects, appearanceEffects, fallbackAppearanceEffects.Length);
				SetEffectsDependency<AppearanceBase>(ref characters[i].indexDisappearanceEffects, disappearanceEffects, fallbackDisappearanceEffects.Length);
				AssignFallbackEffect<AppearanceBase>(fallbackAppearanceEffects, ref characters[i].indexAppearanceEffects);
				AssignFallbackEffect<BehaviorBase>(fallbackBehaviorEffects, ref characters[i].indexBehaviorEffects);
				AssignFallbackEffect<AppearanceBase>(fallbackDisappearanceEffects, ref characters[i].indexDisappearanceEffects);
				characters[i].disappearancesMaxDuration = CalculateAppearanceDuration(characters[i].indexDisappearanceEffects, disappearanceEffects);
				characters[i].appearancesMaxDuration = CalculateAppearanceDuration(characters[i].indexAppearanceEffects, appearanceEffects);
				if (textInfo.characterInfo[i].isVisible)
				{
					for (byte b = 0; b < 4; b++)
					{
						characters[i].sources.vertices[b] = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices[textInfo.characterInfo[i].vertexIndex + b];
						characters[i].sources.colors[b] = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].colors32[textInfo.characterInfo[i].vertexIndex + b];
					}
				}
			}
			for (int j = maxVisibleCharacters; j < characters.Length; j++)
			{
				characters[j].isDisappearing = true;
				characters[j].data.passedTime = 0f;
			}
			hasText = text.Length > 0;
			autoSize = tmproText.enableAutoSizing;
			this.text = tmproText.text;
			AssertCharacterTimes();
			tmproText.renderMode = TextRenderFlags.DontRender;
			CopyMeshSources();
			static void AssignFallbackEffect<T>(T[] effect, ref int[] indexes) where T : EffectsBase
			{
				if (effect.Length != 0 && indexes.Length == 0)
				{
					indexes = new int[effect.Length];
					for (int k = 0; k < effect.Length; k++)
					{
						indexes[k] = k;
					}
				}
			}
			static float CalculateAppearanceDuration(int[] effectsIndex, List<AppearanceBase> effects)
			{
				float num = 0f;
				foreach (int index in effectsIndex)
				{
					if (effects[index].effectDuration > num)
					{
						num = effects[index].effectDuration;
					}
				}
				return num;
			}
			void SetEffectsDependency<T>(ref int[] indexes, List<T> effects, int fallbackEffectsCount) where T : EffectsBase
			{
				temp_effectsToApply.Clear();
				for (int k = fallbackEffectsCount; k < effects.Count; k++)
				{
					if (effects[k].regionManager.IsCharInsideRegion(textInfo.characterInfo[i].index))
					{
						temp_effectsToApply.Add(k);
					}
				}
				indexes = new int[temp_effectsToApply.Count];
				for (int l = 0; l < temp_effectsToApply.Count; l++)
				{
					indexes[l] = temp_effectsToApply[l];
				}
			}
		}

		private void TryTriggeringEvent(int maxInternalOrder)
		{
			for (int i = latestTriggeredEvent; i < eventMarkers.Count; i++)
			{
				if (!eventMarkers[i].triggered && eventMarkers[i].charIndex <= textInfo.characterInfo[_maxVisibleCharacters].index && eventMarkers[i].internalOrder < maxInternalOrder)
				{
					EventMarker value = eventMarkers[i];
					value.triggered = true;
					eventMarkers[i] = value;
					latestTriggeredEvent = i;
					this.onEvent?.Invoke(eventMarkers[i].eventMessage);
				}
			}
		}

		internal bool TryGetAction(out TypewriterAction action)
		{
			if (_maxVisibleCharacters >= textInfo.characterCount)
			{
				action = default(TypewriterAction);
				return false;
			}
			for (int i = latestTriggeredAction; i < typewriterActions.Count; i++)
			{
				if (typewriterActions[i].charIndex == textInfo.characterInfo[_maxVisibleCharacters].index && !typewriterActions[i].triggered)
				{
					TryTriggeringEvent(typewriterActions[i].internalOrder);
					InternalAction value = typewriterActions[i];
					value.triggered = true;
					typewriterActions[i] = value;
					action = value.action;
					latestTriggeredAction = i;
					return true;
				}
			}
			action = default(TypewriterAction);
			return false;
		}

		private void UpdateEffectIntensityWithSize(float charSize)
		{
			float intensity = effectIntensityMultiplier;
			if (useDynamicScaling)
			{
				intensity *= charSize / referenceFontSize;
			}
			SetEffectsIntensity<BehaviorBase>(behaviorEffects);
			SetEffectsIntensity<AppearanceBase>(appearanceEffects);
			SetEffectsIntensity<AppearanceBase>(disappearanceEffects);
			void SetEffectsIntensity<T>(List<T> effects) where T : EffectsBase
			{
				foreach (T effect in effects)
				{
					effect.uniformIntensity = intensity;
				}
			}
		}

		private void CopyMeshSources()
		{
			forceMeshRefresh = false;
			autoSize = tmproText.enableAutoSizing;
			sourceRect = tmproText.rectTransform.rect;
			sourceColor = tmproText.color;
			tmpFirstVisibleCharacter = tmproText.firstVisibleCharacter;
			tmpMaxVisibleCharacters = tmproText.maxVisibleCharacters;
			for (int i = 0; i < textInfo.characterCount && i < characters.Length; i++)
			{
				characters[i].data.tmp_CharInfo = textInfo.characterInfo[i];
				if (textInfo.characterInfo[i].isVisible)
				{
					for (byte b = 0; b < 4; b++)
					{
						characters[i].sources.vertices[b] = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices[textInfo.characterInfo[i].vertexIndex + b];
					}
					for (byte b2 = 0; b2 < 4; b2++)
					{
						characters[i].sources.colors[b2] = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].colors32[textInfo.characterInfo[i].vertexIndex + b2];
					}
				}
			}
		}

		private void UpdateMesh()
		{
			for (int i = 0; i < textInfo.characterCount && i < characters.Length; i++)
			{
				if (textInfo.characterInfo[i].isVisible)
				{
					textInfo.characterInfo[i] = characters[i].data.tmp_CharInfo;
					for (byte b = 0; b < 4; b++)
					{
						textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices[textInfo.characterInfo[i].vertexIndex + b] = characters[i].data.vertices[b];
					}
					for (byte b2 = 0; b2 < 4; b2++)
					{
						textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].colors32[textInfo.characterInfo[i].vertexIndex + b2] = characters[i].data.colors[b2];
					}
				}
			}
			tmproText.UpdateVertexData();
		}

		private void Update()
		{
			if (!tmproText.text.Equals(text))
			{
				if (hasParentCanvas && !parentCanvas.isActiveAndEnabled)
				{
					return;
				}
				if (triggerAnimPlayerOnChange && tAnimPlayer != null)
				{
					if (tmproText.text.Length <= 0)
					{
						tAnimPlayer.ShowText("");
					}
					else
					{
						tAnimPlayer.ShowText("<noparse></noparse>" + tmproText.text);
					}
				}
				else
				{
					_SetText(tmproText.text, ShowTextMode.UserTyping);
				}
			}
			else if (hasText)
			{
				UpdateEffectsToMesh();
			}
		}

		private void UpdateEffectsToMesh()
		{
			m_time.UpdateDeltaTime(timeScale);
			m_time.IncreaseTime();
			for (int i = 0; i < behaviorEffects.Count; i++)
			{
				behaviorEffects[i].SetAnimatorData(in m_time);
				behaviorEffects[i].Calculate();
			}
			for (int j = 0; j < appearanceEffects.Count; j++)
			{
				appearanceEffects[j].Calculate();
			}
			for (int k = 0; k < disappearanceEffects.Count; k++)
			{
				disappearanceEffects[k].Calculate();
			}
			int l;
			for (l = 0; l < textInfo.characterCount && l < characters.Length; l++)
			{
				if (!textInfo.characterInfo[l].isVisible)
				{
					characters[l].data.passedTime = 0f;
					characters[l].Hide();
					continue;
				}
				if (characters[l].isDisappearing != characters[l].wantsToDisappear)
				{
					characters[l].isDisappearing = characters[l].wantsToDisappear;
					characters[l].data.passedTime = (characters[l].isDisappearing ? characters[l].disappearancesMaxDuration : 0f);
				}
				characters[l].ResetColors();
				characters[l].ResetVertices();
				UpdateEffectIntensityWithSize(textInfo.characterInfo[l].pointSize);
				if (!characters[l].isDisappearing)
				{
					TryApplyingBehaviors();
					if (enabled_globalAppearances && enabled_localAppearances && !skipAppearanceEffects)
					{
						int[] indexAppearanceEffects = characters[l].indexAppearanceEffects;
						foreach (int index in indexAppearanceEffects)
						{
							if (appearanceEffects[index].CanShowAppearanceOn(characters[l].data.passedTime))
							{
								appearanceEffects[index].ApplyEffect(ref characters[l].data, l);
							}
						}
					}
					characters[l].data.passedTime += m_time.deltaTime;
					continue;
				}
				if (characters[l].data.passedTime <= 0f)
				{
					characters[l].data.passedTime = 0f;
					characters[l].Hide();
					continue;
				}
				TryApplyingBehaviors();
				if (enabled_globalAppearances && enabled_localAppearances)
				{
					int[] indexAppearanceEffects = characters[l].indexDisappearanceEffects;
					foreach (int index2 in indexAppearanceEffects)
					{
						if (disappearanceEffects[index2].CanShowAppearanceOn(characters[l].data.passedTime))
						{
							disappearanceEffects[index2].ApplyEffect(ref characters[l].data, l);
						}
					}
				}
				characters[l].data.passedTime -= m_time.deltaTime;
			}
			UpdateMesh();
			if (tmproText.havePropertiesChanged || forceMeshRefresh || tmproText.enableAutoSizing != autoSize || tmproText.rectTransform.rect != sourceRect || tmproText.color != sourceColor || tmproText.firstVisibleCharacter != tmpFirstVisibleCharacter || tmproText.maxVisibleCharacters != tmpMaxVisibleCharacters)
			{
				tmproText.ForceMeshUpdate();
				CopyMeshSources();
			}
			void TryApplyingBehaviors()
			{
				if (enabled_globalBehaviors && enabled_localBehaviors)
				{
					int[] indexBehaviorEffects = characters[l].indexBehaviorEffects;
					foreach (int index3 in indexBehaviorEffects)
					{
						behaviorEffects[index3].ApplyEffect(ref characters[l].data, l);
					}
				}
			}
		}

		private void OnEnable()
		{
			forceMeshRefresh = true;
			textInfo = tmproText.textInfo;
			UpdateEffectsToMesh();
		}
	}
}
