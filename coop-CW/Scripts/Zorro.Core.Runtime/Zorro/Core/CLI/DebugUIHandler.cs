using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Zorro.Core.CLI
{
	public class DebugUIHandler : Singleton<DebugUIHandler>
	{
		public VisualTreeAsset m_consoleUXMLAsset;

		public VisualTreeAsset m_logEntryUXMLAsset;

		private VisualElement m_contentRoot;

		private UIDocument m_document;

		private List<ConsoleLogEntry> m_logEntries = new List<ConsoleLogEntry>();

		private DebugPage m_currentPage;

		private List<Button> m_pageButtons = new List<Button>();

		private VisualElement m_toolbar;

		private List<(string, Func<DebugPage>)> m_customPages = new List<(string, Func<DebugPage>)>();

		private HashSet<string> m_uniquePages = new HashSet<string>();

		public DebugPage CurrentPage => m_currentPage;

		public bool IsOpen => m_document.enabled;

		protected override void Awake()
		{
			base.Awake();
			Application.logMessageReceived += ApplicationOnlogMessageReceived;
		}

		private void OnDestroy()
		{
			Application.logMessageReceived -= ApplicationOnlogMessageReceived;
		}

		public void RegisterPage(string text, Func<DebugPage> page)
		{
			Debug.Log("Registering page: " + text);
			if (m_uniquePages.Add(text))
			{
				m_customPages.Add((text, page));
			}
		}

		private void ApplicationOnlogMessageReceived(string condition, string stacktrace, LogType type)
		{
			if (type == LogType.Warning)
			{
				return;
			}
			if (condition.Contains(Environment.NewLine))
			{
				string[] array = condition.Split(Environment.NewLine);
				foreach (string log in array)
				{
					AddEntry(new ConsoleLogEntry
					{
						Log = log,
						LogTime = DateTime.Now,
						IsError = (type != LogType.Log)
					});
				}
			}
			else if (condition.Contains("\n"))
			{
				string[] array = condition.Split("\n");
				foreach (string log2 in array)
				{
					AddEntry(new ConsoleLogEntry
					{
						Log = log2,
						LogTime = DateTime.Now,
						IsError = (type != LogType.Log)
					});
				}
			}
			else
			{
				AddEntry(new ConsoleLogEntry
				{
					Log = condition,
					LogTime = DateTime.Now,
					IsError = (type != LogType.Log)
				});
			}
			if (m_currentPage is ConsolePage consolePage)
			{
				consolePage.LogRecieved();
			}
		}

		public void AddEntry(ConsoleLogEntry entry)
		{
			m_logEntries.Add(entry);
		}

		private void Start()
		{
			m_document = GetComponent<UIDocument>();
			m_document.enabled = false;
		}

		private void Update()
		{
			if (IsOpen)
			{
				if (m_currentPage != null)
				{
					m_currentPage.Update();
					return;
				}
				BuildToolbar();
				GoToConsole();
			}
		}

		private void BuildToolbar()
		{
			m_toolbar = m_document.rootVisualElement.Q<VisualElement>("Toolbar");
			Button button = new Button(GoToConsole)
			{
				text = "Console"
			};
			m_toolbar.Add(button);
			m_pageButtons.Add(button);
			foreach (var customPage in m_customPages)
			{
				string item = customPage.Item1;
				Button button2 = new Button(delegate
				{
					OpenPage(customPage.Item2());
				})
				{
					text = item
				};
				m_toolbar.Add(button2);
				m_pageButtons.Add(button2);
			}
		}

		private void GoToConsole()
		{
			OpenPage(new ConsolePage(m_logEntries, this, m_consoleUXMLAsset, m_logEntryUXMLAsset));
		}

		public void Hide()
		{
			m_document.enabled = false;
			if (m_currentPage != null)
			{
				m_contentRoot.Remove(m_currentPage);
				m_currentPage = null;
			}
			if (m_toolbar != null)
			{
				m_pageButtons.ForEach(delegate(Button x)
				{
					m_toolbar.Remove(x);
				});
				m_pageButtons.Clear();
			}
			m_currentPage = null;
			m_toolbar = null;
			EventSystem current = EventSystem.current;
			if (current != null)
			{
				current.sendNavigationEvents = true;
			}
		}

		public void Show()
		{
			m_document.enabled = true;
			EventSystem current = EventSystem.current;
			if (current != null)
			{
				current.sendNavigationEvents = false;
			}
		}

		private void OpenPage(DebugPage page)
		{
			if (m_currentPage == null)
			{
				m_contentRoot = m_document.rootVisualElement.Q<VisualElement>("PageRoot");
				m_contentRoot.styleSheets.Add(SingletonAsset<CoreGlobalDependencies>.Instance.DebugPageStyleSheets);
			}
			else
			{
				m_contentRoot.Remove(m_currentPage);
			}
			m_currentPage = page;
			m_contentRoot.Add(page);
			page.StretchToParentSize();
		}
	}
}
