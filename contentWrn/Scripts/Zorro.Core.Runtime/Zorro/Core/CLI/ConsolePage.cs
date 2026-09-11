using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Zorro.Core.CLI
{
	public class ConsolePage : DebugPage
	{
		private List<ConsoleLogEntry> m_logEntries;

		private DebugUIHandler m_debugUI;

		private VisualTreeAsset m_logEntryUXMLAsset;

		private VisualTreeAsset m_consoleUXMLAsset;

		private Optionable<byte> m_selectedSuggestion = Optionable<byte>.None;

		private List<Label> m_suggestionsTexts;

		private List<Suggestion> m_suggestions = new List<Suggestion>();

		private int m_selectedHistory = -1;

		private ListView m_listView;

		private int nextEntry;

		private Label m_entryLabel;

		private string m_currentInput;

		public ConsolePage(List<ConsoleLogEntry> logEntries, DebugUIHandler debugUI, VisualTreeAsset consoleUxmlAsset, VisualTreeAsset logEntryUxmlAsset)
		{
			m_consoleUXMLAsset = consoleUxmlAsset;
			m_logEntryUXMLAsset = logEntryUxmlAsset;
			m_debugUI = debugUI;
			m_logEntries = logEntries;
			m_consoleUXMLAsset.CloneTree(this);
			ConstructConsole(this);
			m_listView.RefreshItems();
			m_listView.ScrollToItem(m_logEntries.Count - 1);
			FindSuggestions(m_currentInput);
		}

		private void ConstructConsole(VisualElement root)
		{
			VisualElement visualElement = root.Q("Background");
			ListView listView = new ListView(m_logEntries, 16f, MakeLogEntry, BindEntry)
			{
				selectionType = SelectionType.None
			};
			listView.StretchToParentSize();
			listView.style.flexGrow = 1f;
			visualElement.Add(listView);
			m_entryLabel = root.Q<Label>("InputField");
			m_listView = listView;
			m_suggestionsTexts = root.Q("Suggestions").Query<Label>().ToList();
			foreach (Label suggestionsText in m_suggestionsTexts)
			{
				suggestionsText.text = "";
			}
		}

		private VisualElement MakeLogEntry()
		{
			return m_logEntryUXMLAsset.CloneTree();
		}

		private void BindEntry(VisualElement element, int index)
		{
			ConsoleLogEntry consoleLogEntry = m_logEntries[index];
			Label label = element.Q<Label>("Time");
			Label label2 = element.Q<Label>("Log");
			label.text = string.Format("[{0} f:{1}]", consoleLogEntry.LogTime.ToString("HH:mm"), Time.frameCount);
			label2.text = GetColoredText(consoleLogEntry.Log, consoleLogEntry.IsError ? "#fc5347" : "#cccaca");
		}

		private string GetColoredText(string entryLog, string color)
		{
			return "<color=" + color + ">" + entryLog + "</color>";
		}

		private void FindSuggestions(string input)
		{
			m_suggestions = ConsoleHandler.FindSuggestions(input).ToList();
			for (int i = 0; i < m_suggestionsTexts.Count; i++)
			{
				if (m_suggestions.Count <= i)
				{
					m_suggestionsTexts[i].text = "";
				}
				else
				{
					m_suggestionsTexts[i].text = m_suggestions[i].ToString();
				}
			}
		}

		private void AttemptParseCommand(string command)
		{
			ConsoleHandler.AddToHistory(command);
			m_debugUI.AddEntry(new ConsoleLogEntry
			{
				LogTime = DateTime.Now,
				IsError = false,
				Log = ">" + command
			});
			if (m_listView != null)
			{
				m_listView.RefreshItems();
				m_listView.ScrollToItem(m_logEntries.Count - 1);
			}
			if (!ConsoleHandler.ProcessCommand(command))
			{
				Debug.LogError("Command not found: " + command);
			}
		}

		public void LogRecieved()
		{
			if (m_listView != null)
			{
				m_listView.RefreshItems();
				m_listView.ScrollToItem(m_logEntries.Count - 1);
			}
		}

		public override void Update()
		{
			base.Update();
			string text = Input.inputString;
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				text = string.Empty;
			}
			bool flag = false;
			string text2 = text;
			for (int i = 0; i < text2.Length; i++)
			{
				char c = text2[i];
				m_selectedHistory = -1;
				flag = true;
				switch (c)
				{
				case '\b':
					if (!string.IsNullOrEmpty(m_currentInput) && m_currentInput.Length != 0)
					{
						m_currentInput = m_currentInput.Substring(0, m_currentInput.Length - 1);
					}
					break;
				case '\n':
				case '\r':
					AttemptParseCommand(m_currentInput);
					m_currentInput = "";
					break;
				default:
					m_currentInput += c;
					break;
				}
			}
			bool flag2 = Time.unscaledTime * 2f % 2f < 1f;
			m_entryLabel.text = m_currentInput + (flag2 ? "" : "|");
			if (Input.GetKeyDown(KeyCode.Tab) && m_suggestions.Count > 0)
			{
				Suggestion nextSelectedSuggestion = GetNextSelectedSuggestion();
				if (nextSelectedSuggestion != null)
				{
					m_currentInput = nextSelectedSuggestion.GetInputValue();
				}
			}
			List<string> history = ConsoleHandler.GetHistory();
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				m_selectedHistory = Mathf.Clamp(m_selectedHistory + 1, -1, history.Count - 1);
				flag = true;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				m_selectedHistory = Mathf.Clamp(m_selectedHistory - 1, -1, history.Count - 1);
				if (m_selectedHistory == -1)
				{
					m_currentInput = "";
				}
				flag = true;
			}
			if (m_selectedHistory >= 0)
			{
				m_currentInput = history[history.Count - 1 - m_selectedHistory];
			}
			if (flag)
			{
				m_selectedSuggestion = Optionable<byte>.None;
				FindSuggestions(m_currentInput);
			}
		}

		private Suggestion GetNextSelectedSuggestion()
		{
			for (int i = 0; i < 10; i++)
			{
				if (m_selectedSuggestion.IsNone)
				{
					m_selectedSuggestion = Optionable<byte>.Some(0);
				}
				else
				{
					m_selectedSuggestion = Optionable<byte>.Some((byte)((m_selectedSuggestion.Value + 1) % m_suggestions.Count));
				}
				if (m_suggestions[m_selectedSuggestion.Value].CanBeSelected())
				{
					return m_suggestions[m_selectedSuggestion.Value];
				}
			}
			return null;
		}
	}
}
