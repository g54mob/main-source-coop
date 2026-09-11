using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Portningsbolaget
{
	public class BlockedPlayerUI : MonoBehaviour
	{
		public TMP_Text m_NicknameText;

		public Button m_unblockButton;

		private BlocklistTable m_table;

		private BlockedPlayer m_player;

		public Button Button => m_unblockButton;

		public BlockedPlayer Player => m_player;

		public void Initialise(BlocklistTable table, BlockedPlayer player)
		{
			m_table = table;
			m_player = player;
			base.gameObject.name = m_player.Nickname;
			m_NicknameText.text = m_player.Nickname;
			if (player.OnPlatform)
			{
				m_unblockButton.gameObject.SetActive(value: false);
			}
			else
			{
				m_unblockButton.onClick.AddListener(OnUnblockButton);
			}
		}

		public void Connect(Button previous)
		{
			if (!(previous == null))
			{
				Navigation navigation = m_unblockButton.navigation;
				navigation.selectOnUp = previous;
				m_unblockButton.navigation = navigation;
				navigation = previous.navigation;
				navigation.selectOnDown = m_unblockButton;
				previous.navigation = navigation;
			}
		}

		public void Select()
		{
			m_unblockButton?.Select();
		}

		public void SelectNeighbour()
		{
			Navigation navigation = m_unblockButton.navigation;
			if (navigation.selectOnUp != null)
			{
				navigation.selectOnUp.Select();
			}
			else if (navigation.selectOnDown != null)
			{
				navigation.selectOnDown.Select();
			}
			else
			{
				m_unblockButton?.Select();
			}
		}

		private void OnUnblockButton()
		{
			m_table.UnblockPlayer(this);
		}
	}
}
