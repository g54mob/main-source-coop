using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEditor.Localization.Samples
{
	public class LocalizedStringGetLocalizedStringExample : MonoBehaviour
	{
		public LocalizedString stringRef = new LocalizedString
		{
			TableReference = "My String Table",
			TableEntryReference = "Hello World"
		};

		private void OnGUI()
		{
			AsyncOperationHandle<string> localizedStringAsync = stringRef.GetLocalizedStringAsync();
			if (localizedStringAsync.IsDone && localizedStringAsync.Status == AsyncOperationStatus.Succeeded)
			{
				GUILayout.Label(localizedStringAsync.Result);
			}
		}
	}
}
