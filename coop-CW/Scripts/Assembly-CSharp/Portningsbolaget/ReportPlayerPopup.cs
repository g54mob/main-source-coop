using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Portningsbolaget
{
	[DefaultExecutionOrder(-1)]
	public class ReportPlayerPopup : MonoBehaviour
	{
		private static ReportPlayerPopup s_instance;

		public TMP_InputField m_playerField;

		public TMP_InputField m_textField;

		public Button m_closeButton;

		public Button m_writeButton;

		public Button m_sendButton;

		public GameObject m_overlay;

		public GameObject m_popupPanel;

		public GameObject m_togglePanel;

		public InputActionReference m_close;

		private bool m_assignedPlayer;

		private string m_playerName = string.Empty;

		private string m_playerAccountId = string.Empty;

		private string m_playerPlatform = string.Empty;

		private bool m_assignedOffender;

		private string m_offenderAccountId = string.Empty;

		private string m_offenderPlatform = string.Empty;

		private string[] m_tagLabels;

		private Toggle[] m_tagToggles;

		public static ReportPlayerPopup Instance => s_instance;

		public bool IsVisible => m_popupPanel.activeSelf;

		public event Action OnReport;

		private void Awake()
		{
			base.gameObject.SetActive(value: false);
		}

		private void Start()
		{
			Show(visible: false);
		}

		private void OnDisable()
		{
			Show(visible: false);
		}

		private void Update()
		{
			if (m_close.action.WasPressedThisFrame())
			{
				StartCoroutine(ShowDelayed(visible: false));
			}
		}

		public void Toggle()
		{
			Show(!IsVisible);
		}

		private IEnumerator ShowDelayed(bool visible)
		{
			yield return null;
			Show(visible);
		}

		private void Show(bool visible)
		{
			if (IsVisible != visible)
			{
				Debug.Log((visible ? "Showing" : "Hiding") + " Report Popup");
				m_overlay.SetActive(visible);
				m_popupPanel.SetActive(visible);
				if (visible)
				{
					m_tagToggles[1].Select();
				}
				else
				{
					ResetInfo();
				}
			}
		}

		public void AssignPlayer(string nickname, string accountId, string platform)
		{
			if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(platform))
			{
				Debug.LogError("Invalid Player Info: " + nickname + " " + accountId + " " + platform);
			}
			else
			{
				m_playerName = nickname;
				m_playerAccountId = accountId;
				m_playerPlatform = platform;
				m_assignedPlayer = true;
			}
		}

		public void AssignOffender(string nickname, string accountId, string platform)
		{
			if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(platform))
			{
				Debug.LogError("Invalid Offender Info: " + nickname + " " + accountId + " " + platform);
			}
			else
			{
				m_playerField.text = nickname;
				m_offenderAccountId = accountId;
				m_offenderPlatform = platform;
				m_assignedOffender = true;
			}
		}

		private void ResetInfo()
		{
			m_playerField.text = string.Empty;
			m_textField.text = string.Empty;
			m_offenderAccountId = string.Empty;
			m_offenderPlatform = string.Empty;
			m_assignedOffender = false;
			for (int i = 0; i < m_tagToggles.Length; i++)
			{
				m_tagToggles[i].isOn = false;
			}
		}

		private void WriteReport()
		{
		}

		private void SendReport()
		{
			string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.PlayerCell_ReportButton);
			string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_AreYouSure);
			string localizedString3 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Yes);
			string localizedString4 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Cancel);
			Modal.Show(localizedString + " " + m_playerField.text, localizedString2, new ModalOption[2]
			{
				new ModalOption(localizedString3, OnReportPlayer),
				new ModalOption(localizedString4, delegate
				{
					m_sendButton.Select();
				})
			});
		}

		private void OnReportPlayer()
		{
			string tags = GetTags();
			if (!m_assignedPlayer || !m_assignedOffender || string.IsNullOrEmpty(tags))
			{
				Debug.LogError("Invalid Report Form");
				return;
			}
			DateTime now = DateTime.Now;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Player:");
			stringBuilder.AppendLine("Nickname: " + m_playerName);
			stringBuilder.AppendLine("Account:  " + m_playerAccountId);
			stringBuilder.AppendLine("Platform: " + m_playerPlatform);
			stringBuilder.AppendLine("Offender:");
			stringBuilder.AppendLine("Nickname: " + m_playerField.text);
			stringBuilder.AppendLine("Account:  " + m_offenderAccountId);
			stringBuilder.AppendLine("Platform: " + m_offenderPlatform);
			stringBuilder.AppendLine($"DateTime: {now:HH:mm dd-MM-yyyy}");
			stringBuilder.AppendLine("Offences: " + tags);
			Debug.Log(stringBuilder.ToString());
			this.OnReport?.Invoke();
			Show(visible: false);
		}

		private string GetTags()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = m_tagToggles.Length;
			for (int i = 0; i < num; i++)
			{
				if (m_tagToggles[i].isOn)
				{
					stringBuilder.Append(m_tagLabels[i] + ", ");
				}
			}
			return stringBuilder.ToString();
		}
	}
}
