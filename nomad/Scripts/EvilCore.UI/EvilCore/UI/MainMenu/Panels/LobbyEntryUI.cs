using System;
using EvilCore.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EvilCore.UI.MainMenu.Panels
{
	public class LobbyEntryUI : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI lobbyNameText;

		[SerializeField]
		private TextMeshProUGUI playerCountText;

		[SerializeField]
		private Button joinButton;

		[Header("Region (optional)")]
		[SerializeField]
		private TextMeshProUGUI regionText;

		[SerializeField]
		private TextMeshProUGUI pingText;

		private const int GoodPingMs = 80;

		private const int OkPingMs = 160;

		private static readonly Color GoodColor = new Color(0.4f, 0.85f, 0.45f);

		private static readonly Color OkColor = new Color(0.95f, 0.8f, 0.3f);

		private static readonly Color FarColor = new Color(0.9f, 0.45f, 0.4f);

		private static readonly Color UnknownColor = new Color(0.6f, 0.6f, 0.6f);

		private LobbySearchResult _lobby;

		private Action<LobbySearchResult> _onJoinClicked;

		public void Setup(LobbySearchResult lobby, Action<LobbySearchResult> onJoinClicked, string regionLabel, int estimatedPingMs)
		{
			_lobby = lobby;
			_onJoinClicked = onJoinClicked;
			lobbyNameText.text = lobby.LobbyName;
			playerCountText.text = $"{lobby.MemberCount}/{lobby.MaxMembers}";
			if (regionText != null)
			{
				regionText.text = regionLabel;
			}
			ApplyPingBadge(estimatedPingMs);
			bool flag = lobby.MemberCount >= lobby.MaxMembers;
			joinButton.interactable = !flag;
			joinButton.onClick.AddListener(delegate
			{
				_onJoinClicked?.Invoke(_lobby);
			});
		}

		private void ApplyPingBadge(int estimatedPingMs)
		{
			if (!(pingText == null))
			{
				if (estimatedPingMs < 0)
				{
					pingText.text = "?";
					pingText.color = UnknownColor;
				}
				else
				{
					pingText.text = $"~{estimatedPingMs} ms";
					pingText.color = ((estimatedPingMs <= 80) ? GoodColor : ((estimatedPingMs <= 160) ? OkColor : FarColor));
				}
			}
		}

		private void OnDestroy()
		{
			joinButton?.onClick.RemoveAllListeners();
		}
	}
}
