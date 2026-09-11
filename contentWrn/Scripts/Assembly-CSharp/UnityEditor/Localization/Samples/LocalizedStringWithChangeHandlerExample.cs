using UnityEngine;
using UnityEngine.Localization;

namespace UnityEditor.Localization.Samples
{
	public class LocalizedStringWithChangeHandlerExample : MonoBehaviour
	{
		public LocalizedString stringRef = new LocalizedString
		{
			TableReference = "My String Table",
			TableEntryReference = "Hello World"
		};

		private string m_TranslatedString;

		private void OnEnable()
		{
			stringRef.StringChanged += UpdateString;
		}

		private void OnDisable()
		{
			stringRef.StringChanged -= UpdateString;
		}

		private void UpdateString(string translatedValue)
		{
			m_TranslatedString = translatedValue;
			Debug.Log("Translated Value Updated: " + translatedValue);
		}

		private void OnGUI()
		{
			GUILayout.Label(m_TranslatedString);
		}
	}
}
