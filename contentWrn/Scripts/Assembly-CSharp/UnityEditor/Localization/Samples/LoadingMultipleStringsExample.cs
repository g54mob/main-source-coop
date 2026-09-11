using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEditor.Localization.Samples
{
	public class LoadingMultipleStringsExample : MonoBehaviour
	{
		public string stringTableCollectionName = "My Strings";

		private string m_TranslatedStringHello;

		private string m_TranslatedStringGoodbye;

		private string m_TranslatedStringThisIsATest;

		private void OnEnable()
		{
			StartCoroutine(LoadStrings());
			LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
		}

		private void OnDisable()
		{
			LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
		}

		private void OnSelectedLocaleChanged(Locale obj)
		{
			StartCoroutine(LoadStrings());
		}

		private IEnumerator LoadStrings()
		{
			AsyncOperationHandle<StringTable> loadingOperation = LocalizationSettings.StringDatabase.GetTableAsync(stringTableCollectionName);
			yield return loadingOperation;
			if (loadingOperation.Status == AsyncOperationStatus.Succeeded)
			{
				StringTable result = loadingOperation.Result;
				m_TranslatedStringThisIsATest = GetLocalizedString(result, "This is a test");
				m_TranslatedStringHello = GetLocalizedString(result, "Hello");
				m_TranslatedStringGoodbye = GetLocalizedString(result, "Goodbye");
			}
			else
			{
				Debug.LogError("Could not load String Table\n" + loadingOperation.OperationException.ToString());
			}
		}

		private string GetLocalizedString(StringTable table, string entryName)
		{
			return table.GetEntry(entryName).GetLocalizedString();
		}

		private void OnGUI()
		{
			if (!LocalizationSettings.InitializationOperation.IsDone)
			{
				GUILayout.Label("Initializing Localization");
				return;
			}
			GUILayout.Label(m_TranslatedStringThisIsATest);
			GUILayout.Label(m_TranslatedStringHello);
			GUILayout.Label(m_TranslatedStringGoodbye);
		}
	}
}
