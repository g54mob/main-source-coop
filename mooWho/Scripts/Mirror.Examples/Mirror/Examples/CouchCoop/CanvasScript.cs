using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.CouchCoop
{
	public class CanvasScript : MonoBehaviour
	{
		public CouchPlayerManager couchPlayerManager;

		public Button buttonAddPlayer;

		public Button buttonRemovePlayer;

		private void Start()
		{
			buttonAddPlayer.onClick.AddListener(ButtonAddPlayer);
			buttonRemovePlayer.onClick.AddListener(ButtonRemovePlayer);
		}

		private void ButtonAddPlayer()
		{
			if (couchPlayerManager == null)
			{
				Debug.Log("Start game first.");
			}
			else
			{
				couchPlayerManager.CmdAddPlayer();
			}
		}

		private void ButtonRemovePlayer()
		{
			if (couchPlayerManager == null)
			{
				Debug.Log("Start game first.");
			}
			else
			{
				couchPlayerManager.CmdRemovePlayer();
			}
		}
	}
}
