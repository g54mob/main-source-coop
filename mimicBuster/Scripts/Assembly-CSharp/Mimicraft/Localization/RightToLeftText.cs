using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mimicraft.Localization
{
	public class RightToLeftText : MonoBehaviour
	{
		private sealed class Preprocessor : ITextPreprocessor
		{
			public string PreprocessText(string text)
			{
				if (string.IsNullOrEmpty(text))
				{
					return text;
				}
				if (!uiRightToLeft)
				{
					return ArabicText.ForLeftToRightLayout(text);
				}
				return ArabicText.ForRightToLeftLayout(text);
			}
		}

		private static readonly Preprocessor Shared = new Preprocessor();

		private static readonly HashSet<int> Seen = new HashSet<int>();

		private static readonly HashSet<int> Excluded = new HashSet<int>();

		private static readonly List<TMP_Text> Pending = new List<TMP_Text>();

		private static RightToLeftText runner;

		private static bool uiRightToLeft;

		public static bool UiIsRightToLeft => uiRightToLeft;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Install()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
			Loc.Changed -= OnLanguageChanged;
			Loc.Changed += OnLanguageChanged;
			Seen.Clear();
			Excluded.Clear();
			Pending.Clear();
			uiRightToLeft = Loc.IsRightToLeft;
			if (runner == null)
			{
				GameObject obj = new GameObject("RightToLeftText")
				{
					hideFlags = HideFlags.HideAndDontSave
				};
				Object.DontDestroyOnLoad(obj);
				runner = obj.AddComponent<RightToLeftText>();
			}
		}

		private static void OnTextChanged(Object changed)
		{
			if (!(changed is TMP_Text tMP_Text) || tMP_Text == null)
			{
				return;
			}
			int instanceID = tMP_Text.GetInstanceID();
			if (Excluded.Contains(instanceID))
			{
				return;
			}
			if (Seen.Add(instanceID))
			{
				if (IsInputText(tMP_Text))
				{
					Excluded.Add(instanceID);
					return;
				}
				if (tMP_Text.textPreprocessor != null && tMP_Text.textPreprocessor != Shared)
				{
					Excluded.Add(instanceID);
					return;
				}
				tMP_Text.textPreprocessor = Shared;
				if (uiRightToLeft || ArabicText.ContainsRtl(tMP_Text.text))
				{
					Queue(tMP_Text);
				}
			}
			if (tMP_Text.isRightToLeftText != uiRightToLeft)
			{
				tMP_Text.isRightToLeftText = uiRightToLeft;
			}
		}

		private static bool IsInputText(TMP_Text label)
		{
			TMP_InputField componentInParent = label.GetComponentInParent<TMP_InputField>(includeInactive: true);
			if (componentInParent != null)
			{
				if (!(componentInParent.textComponent == label))
				{
					return componentInParent.placeholder == label;
				}
				return true;
			}
			return false;
		}

		private static void Queue(TMP_Text label)
		{
			if (!Pending.Contains(label))
			{
				Pending.Add(label);
			}
		}

		private static void OnLanguageChanged()
		{
			bool isRightToLeft = Loc.IsRightToLeft;
			if (isRightToLeft == uiRightToLeft)
			{
				return;
			}
			uiRightToLeft = isRightToLeft;
			TMP_Text[] array = Object.FindObjectsByType<TMP_Text>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (TMP_Text tMP_Text in array)
			{
				if (!(tMP_Text == null) && !Excluded.Contains(tMP_Text.GetInstanceID()) && tMP_Text.textPreprocessor == Shared)
				{
					tMP_Text.isRightToLeftText = uiRightToLeft;
					Queue(tMP_Text);
				}
			}
		}

		private void LateUpdate()
		{
			if (Pending.Count == 0)
			{
				return;
			}
			TMP_Text[] array = Pending.ToArray();
			Pending.Clear();
			TMP_Text[] array2 = array;
			foreach (TMP_Text tMP_Text in array2)
			{
				if (tMP_Text != null && tMP_Text.isActiveAndEnabled)
				{
					tMP_Text.ForceMeshUpdate(ignoreActiveState: false, forceTextReparsing: true);
				}
			}
		}
	}
}
