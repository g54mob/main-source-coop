using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace UnityEditor.Localization.Samples
{
	public class LocalizedStringTableExample : MonoBehaviour
	{
		public LocalizedStringTable stringTable = new LocalizedStringTable
		{
			TableReference = "My Strings"
		};

		private string m_TranslatedStringHello;

		private string m_TranslatedStringGoodbye;

		private string m_TranslatedStringThisIsATest;

		private void OnEnable()
		{
			stringTable.TableChanged += LoadStrings;
		}

		private void OnDisable()
		{
			stringTable.TableChanged -= LoadStrings;
		}

		private void LoadStrings(StringTable stringTable)
		{
			m_TranslatedStringHello = GetLocalizedString(stringTable, "Hello");
			m_TranslatedStringGoodbye = GetLocalizedString(stringTable, "Goodbye");
			m_TranslatedStringThisIsATest = GetLocalizedString(stringTable, "This is a test");
		}

		private static string GetLocalizedString(StringTable table, string entryName)
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
