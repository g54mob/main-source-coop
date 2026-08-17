using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EvilCore.UI.GameMenu.Panels
{
	public class PlayerListEntryUI : MonoBehaviour
	{
		[Header("Player Info")]
		[SerializeField]
		private Image rankImage;

		[SerializeField]
		private TextMeshProUGUI playerNameText;

		[SerializeField]
		private TextMeshProUGUI pingText;

		[SerializeField]
		private Image pingIcon;

		[Header("Host Actions")]
		[SerializeField]
		private Button kickButton;

		[SerializeField]
		private Button banButton;

		[Header("Rank Sprites")]
		[SerializeField]
		private Sprite hostSprite;

		[SerializeField]
		private Sprite guestSprite;

		[Header("Ping Colors")]
		[SerializeField]
		private Color goodPingColor = Color.green;

		[SerializeField]
		private Color mediumPingColor = Color.yellow;

		[SerializeField]
		private Color badPingColor = Color.red;

		private string _memberId;

		private Action<string> _onKick;

		private Action<string> _onBan;

		public void Setup(string memberId, string playerName, int pingMs, bool isHost, bool isLocalHost, Action<string> onKick, Action<string> onBan)
		{
			_memberId = memberId;
			_onKick = onKick;
			_onBan = onBan;
			rankImage.color = (isHost ? mediumPingColor : Color.white);
			playerNameText.text = playerName;
			UpdatePing(pingMs);
			bool active = isLocalHost && !isHost;
			kickButton.gameObject.SetActive(active);
			banButton.gameObject.SetActive(active);
			kickButton.onClick.AddListener(delegate
			{
				_onKick?.Invoke(_memberId);
			});
			banButton.onClick.AddListener(delegate
			{
				_onBan?.Invoke(_memberId);
			});
		}

		public void UpdatePing(int pingMs)
		{
			if (pingMs < 0)
			{
				pingText.text = "---";
				pingIcon.color = Color.gray;
			}
			else
			{
				pingText.text = $"{pingMs} ms";
				pingIcon.color = ((pingMs <= 80) ? goodPingColor : ((pingMs <= 150) ? mediumPingColor : badPingColor));
			}
		}

		private void OnDestroy()
		{
			kickButton?.onClick.RemoveAllListeners();
			banButton?.onClick.RemoveAllListeners();
		}
	}
}
