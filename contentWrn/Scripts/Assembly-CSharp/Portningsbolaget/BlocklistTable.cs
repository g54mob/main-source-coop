using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Portningsbolaget.Platforms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Portningsbolaget
{
	public class BlocklistTable : MonoBehaviour
	{
		private static BlocklistTable s_instance;

		public BlocklistPopup m_popup;

		public BlockedPlayerUI m_prefab;

		public RectTransform m_content;

		public RectTransform m_savedGroup;

		public RectTransform m_platformGroup;

		public TMP_Text m_savedEmptyText;

		public TMP_Text m_platformEmptyText;

		private bool m_saveReady;

		private bool m_platformReady;

		private bool m_initialized;

		private bool m_initializing;

		private bool m_saving;

		private Coroutine m_updateRoutine;

		private MonoBehaviour m_routineRunner;

		private List<BlockedPlayerUI> m_savedHandlers = new List<BlockedPlayerUI>();

		private List<BlockedPlayerUI> m_platformHandlers = new List<BlockedPlayerUI>();

		private const string FILENAME = "Blocklist";

		private const int SAVE_SLOT = 3;

		public static BlocklistTable Instance => s_instance;

		public bool Initialised => m_initialized;

		public bool HasBlockedPlayers
		{
			get
			{
				List<BlockedPlayerUI> savedHandlers = m_savedHandlers;
				if (savedHandlers == null || savedHandlers.Count <= 0)
				{
					List<BlockedPlayerUI> platformHandlers = m_platformHandlers;
					if (platformHandlers == null)
					{
						return false;
					}
					return platformHandlers.Count > 0;
				}
				return true;
			}
		}

		public event Action<BlockedPlayer> OnBlocked;

		public event Action<BlockedPlayer> OnUnblocked;

		public void Initialise()
		{
			if (m_initialized || m_initializing || m_updateRoutine != null)
			{
				return;
			}
			Debug.Log("Initialising Blocklist");
			m_initializing = true;
			m_routineRunner = UnityEngine.Object.FindFirstObjectByType<EmptyBehaviour>();
			if (m_routineRunner == null)
			{
				Debug.LogError("Missing Coroutine Runner");
				m_initializing = false;
				return;
			}
			UpdateBlocklistEvents(subscribe: true);
			PlatformManager.Platform.RequestSaveExists("Blocklist", delegate(FileResult result)
			{
				if (result == FileResult.NotFound)
				{
					Debug.Log("Found No Blocklist...");
					SaveBlocklist();
				}
				Refresh();
			});
		}

		private void UpdateBlocklistEvents(bool subscribe)
		{
		}

		public void Refresh()
		{
			Debug.Log("Refreshing Blocklist...");
			if (m_updateRoutine != null)
			{
				m_routineRunner?.StopCoroutine(m_updateRoutine);
			}
			if (m_savedGroup != null)
			{
				for (int num = m_savedGroup.childCount - 1; num >= 0; num--)
				{
					UnityEngine.Object.Destroy(m_savedGroup.GetChild(num).gameObject);
				}
			}
			if (m_platformGroup != null)
			{
				for (int num2 = m_platformGroup.childCount - 1; num2 >= 0; num2--)
				{
					UnityEngine.Object.Destroy(m_platformGroup.GetChild(num2).gameObject);
				}
			}
			m_savedHandlers.Clear();
			m_platformHandlers.Clear();
			m_updateRoutine = m_routineRunner?.StartCoroutine(UpdateList());
		}

		private IEnumerator UpdateList()
		{
			m_saveReady = false;
			m_platformReady = false;
			m_initialized = false;
			List<BlockedPlayer> players = new List<BlockedPlayer>();
			yield return new WaitWhile(() => m_saving);
			FetchSavedPlayers(players);
			yield return new WaitUntil(() => m_saveReady);
			FetchPlatformPlayers(players);
			yield return new WaitUntil(() => m_platformReady);
			Debug.Log("Updating Blocklist");
			foreach (BlockedPlayer item in players)
			{
				Instantiate(item);
				if (!m_initialized)
				{
					this.OnBlocked?.Invoke(item);
				}
			}
			UpdateInfoText();
			RebuildLayout();
			m_initialized = true;
			m_initializing = false;
			m_updateRoutine = null;
		}

		private void FetchSavedPlayers(List<BlockedPlayer> players)
		{
			Debug.Log("Loading Saved Blocklist...");
			PlatformManager.Platform.RequestLoadAsync("Blocklist", delegate(FileResult result, byte[] data)
			{
				if (result != FileResult.Succeeded)
				{
					m_saveReady = true;
				}
				else
				{
					StringReader stringReader = new StringReader(Encoding.ASCII.GetString(data));
					while (stringReader.Peek() != -1)
					{
						string text = stringReader.ReadLine();
						if (!string.IsNullOrEmpty(text))
						{
							BlockedPlayer item = default(BlockedPlayer);
							item.Deserialize(text);
							Debug.Log("Adding Blocked Player: " + item.ToString());
							players.Add(item);
						}
					}
					m_saveReady = true;
				}
			});
		}

		private void FetchPlatformPlayers(List<BlockedPlayer> players)
		{
			Debug.Log("Loading Platform Blocklist...");
		}

		private BlockedPlayer GetPlayer(string nickname, int accountHash, bool onPlatform)
		{
			return new BlockedPlayer
			{
				Nickname = nickname,
				AccountHash = accountHash,
				OnPlatform = onPlatform
			};
		}

		private void Instantiate(BlockedPlayer player)
		{
			RectTransform parent = (player.OnPlatform ? m_platformGroup : m_savedGroup);
			BlockedPlayerUI blockedPlayerUI = UnityEngine.Object.Instantiate(m_prefab, parent);
			if (!player.OnPlatform)
			{
				if (m_savedHandlers.Count > 0)
				{
					List<BlockedPlayerUI> savedHandlers = m_savedHandlers;
					blockedPlayerUI.Connect(savedHandlers[savedHandlers.Count - 1].Button);
				}
				else
				{
					blockedPlayerUI.Connect(m_popup.m_closeButton);
				}
				m_savedHandlers.Add(blockedPlayerUI);
			}
			else
			{
				m_platformHandlers.Add(blockedPlayerUI);
			}
			blockedPlayerUI.Initialise(this, player);
		}

		public static int GetAccountHash(string accountId)
		{
			if (string.IsNullOrEmpty(accountId))
			{
				Debug.LogError("Invalid Account Id");
				return 0;
			}
			return accountId.GetHashCode();
		}

		public bool Contains(PhotonView playerView)
		{
			global::Photon.Realtime.Player controller = playerView.Controller;
			ExitGames.Client.Photon.Hashtable customProperties = controller.CustomProperties;
			int accountHash = GetAccountHash((string)customProperties["UserID"]);
			bool onPlatform = (string)customProperties["PlatformFamily"] == PlatformUtility.CurrentPlatform.ToString();
			return Contains(controller.NickName, accountHash, onPlatform);
		}

		public bool Contains(BlockedPlayer player)
		{
			return Contains(player.Nickname, player.AccountHash, player.OnPlatform);
		}

		public bool Contains(string nickname, int accountHash, bool onPlatform)
		{
			if (!m_initialized && m_updateRoutine == null)
			{
				Debug.LogWarning("Blocklist is not Initialized");
				Initialise();
				return false;
			}
			if (string.IsNullOrEmpty(nickname) || accountHash == 0)
			{
				Debug.LogError($"Invalid Player Info: Nickname {nickname} Account {accountHash}");
				return false;
			}
			return (onPlatform ? m_platformHandlers : m_savedHandlers).Exists((BlockedPlayerUI p) => p.Player.Has(nickname, accountHash, onPlatform));
		}

		public void BlockPlayer(string nickname, int accountHash, bool onPlatform)
		{
			if (string.IsNullOrEmpty(nickname) || accountHash == 0)
			{
				Debug.LogError($"Failed Blocking Player: Nickname {nickname} Account {accountHash}");
				return;
			}
			Debug.Log($"Blocking Player: Nickname {nickname} Account {accountHash}");
			BlockedPlayer player = GetPlayer(nickname, accountHash, onPlatform);
			Instantiate(player);
			UpdateInfoText();
			RebuildLayout();
			if (!onPlatform)
			{
				SaveBlocklist();
			}
			this.OnBlocked?.Invoke(player);
		}

		public void UnblockPlayer(string nickname, int accountHash, bool onPlatform)
		{
			if (string.IsNullOrEmpty(nickname) || accountHash == 0)
			{
				Debug.LogError($"Failed Unblocking Player: Nickname {nickname} Account {accountHash}");
				return;
			}
			BlockedPlayerUI blockedPlayer = (onPlatform ? m_platformHandlers : m_savedHandlers).Find((BlockedPlayerUI h) => h.Player.Has(nickname, accountHash, onPlatform));
			UnblockPlayer(blockedPlayer);
		}

		public void UnblockPlayer(BlockedPlayerUI blockedPlayer)
		{
			if (blockedPlayer == null)
			{
				Debug.LogError("Failed Unblocking Player");
				return;
			}
			BlockedPlayer player = blockedPlayer.Player;
			List<BlockedPlayerUI> list = (player.OnPlatform ? m_platformHandlers : m_savedHandlers);
			int num = list.IndexOf(blockedPlayer);
			if (num < 0)
			{
				Debug.LogError($"Failed Unblocking Player: Nickname {player.Nickname} Account {player.AccountHash}");
				return;
			}
			Debug.Log($"Unblocking Player: Nickname {player.Nickname} Account {player.AccountHash}");
			BlockedPlayerUI blockedPlayerUI = ((num > 0) ? list[num - 1] : null);
			BlockedPlayerUI blockedPlayerUI2 = ((num < list.Count - 1) ? list[num + 1] : null);
			if (blockedPlayerUI2 != null && !blockedPlayerUI2.Player.OnPlatform)
			{
				if (blockedPlayerUI != null)
				{
					blockedPlayerUI2.Connect(blockedPlayerUI.Button);
				}
				else
				{
					blockedPlayerUI2.Connect(m_popup.m_closeButton);
				}
			}
			blockedPlayer.SelectNeighbour();
			list.RemoveAt(num);
			UnityEngine.Object.Destroy(blockedPlayer.gameObject);
			UpdateInfoText();
			RebuildLayout();
			if (!player.OnPlatform)
			{
				SaveBlocklist();
			}
			this.OnUnblocked?.Invoke(player);
		}

		private void SaveBlocklist()
		{
			Debug.Log("Saving Blocklist...");
			StringBuilder stringBuilder = new StringBuilder();
			m_saving = true;
			foreach (BlockedPlayerUI savedHandler in m_savedHandlers)
			{
				stringBuilder.AppendLine(savedHandler.Player.Serialize());
			}
			byte[] bytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());
			PlatformManager.Platform.RequestSave("Blocklist", bytes, delegate(FileResult result)
			{
				if (result == FileResult.Succeeded)
				{
					Debug.Log("Successfully Saved Blocklist");
				}
				else
				{
					Debug.Log("Failed Saving Blocklist");
				}
				m_saving = false;
			});
		}

		public void SelectFirst()
		{
			if (m_savedHandlers.Count > 0)
			{
				m_savedHandlers[0].Select();
			}
			else
			{
				m_popup.m_closeButton.Select();
			}
		}

		private void UpdateInfoText()
		{
			m_savedEmptyText.gameObject.SetActive(m_savedHandlers.Count == 0);
			m_platformEmptyText.gameObject.SetActive(m_platformHandlers.Count == 0);
		}

		private void RebuildLayout()
		{
			if (m_popup.IsVisible)
			{
				m_routineRunner.StartCoroutine(RebuildLayoutRoutine());
			}
		}

		private IEnumerator RebuildLayoutRoutine()
		{
			yield return null;
			LayoutRebuilder.ForceRebuildLayoutImmediate(m_content);
		}
	}
}
