using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEditor.Localization.Samples
{
	public class StringDatabaseGetLocalizedStringExample : MonoBehaviour
	{
		public bool useCoroutine;

		private void OnEnable()
		{
			LocalizationSettings.SelectedLocaleChanged += SelectedLocaleChanged;
			UpdateString();
		}

		private void OnDisable()
		{
			LocalizationSettings.SelectedLocaleChanged -= SelectedLocaleChanged;
		}

		private void SelectedLocaleChanged(Locale locale)
		{
			UpdateString();
		}

		private void UpdateString()
		{
			AsyncOperationHandle<string> localizedStringAsync = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "Start Game", null, FallbackBehavior.UseProjectSettings);
			if (localizedStringAsync.IsDone)
			{
				SetString(localizedStringAsync);
			}
			if (useCoroutine)
			{
				StartCoroutine(LoadStringWithCoroutine(localizedStringAsync));
			}
			else
			{
				localizedStringAsync.Completed += SetString;
			}
		}

		private IEnumerator LoadStringWithCoroutine(AsyncOperationHandle<string> stringOperation)
		{
			yield return stringOperation;
			SetString(stringOperation);
		}

		private void SetString(AsyncOperationHandle<string> stringOperation)
		{
			if (stringOperation.Status == AsyncOperationStatus.Failed)
			{
				Debug.LogError("Failed to load string");
			}
			else
			{
				Debug.Log("Loaded String: " + stringOperation.Result);
			}
		}
	}
}
