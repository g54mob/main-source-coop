using System;
using System.Collections.Generic;
using Edgegap;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.EdgegapLobby
{
	public class UILobbyList : MonoBehaviour
	{
		public UILobbyCreate Create;

		public GameObject EntryPrefab;

		public Transform LobbyContent;

		public GameObject Loading;

		public Button RefreshButton;

		public InputField SearchInput;

		public Button CreateButton;

		public Text Error;

		private List<UILobbyEntry> _entries = new List<UILobbyEntry>();

		private EdgegapLobbyKcpTransport _transport => (EdgegapLobbyKcpTransport)NetworkManager.singleton.transport;

		private void Awake()
		{
			SearchInput.onValueChanged.AddListener(delegate
			{
				SetLobbies(_transport.Api.Lobbies);
			});
			RefreshButton.onClick.AddListener(Refresh);
			CreateButton.onClick.AddListener(delegate
			{
				Create.gameObject.SetActive(value: true);
				base.gameObject.SetActive(value: false);
			});
		}

		public void Start()
		{
			Refresh();
		}

		private void Refresh()
		{
			Loading.SetActive(value: true);
			_transport.Api.RefreshLobbies(SetLobbies, delegate(string s)
			{
				Error.text = s;
				Loading.SetActive(value: false);
			});
		}

		public void Join(LobbyBrief lobby)
		{
			NetworkManager.singleton.networkAddress = lobby.lobby_id;
			NetworkManager.singleton.StartClient();
		}

		public void SetLobbies(LobbyBrief[] lobbies)
		{
			Loading.SetActive(value: false);
			Error.text = "";
			for (int i = _entries.Count; i < lobbies.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(EntryPrefab, LobbyContent);
				_entries.Add(gameObject.GetComponent<UILobbyEntry>());
			}
			string text = SearchInput.text;
			for (int j = 0; j < lobbies.Length; j++)
			{
				_entries[j].Init(this, lobbies[j], text.Length == 0 || lobbies[j].name.Contains(text, StringComparison.InvariantCultureIgnoreCase));
			}
			for (int k = lobbies.Length; k < _entries.Count; k++)
			{
				_entries[k].gameObject.SetActive(value: false);
			}
		}
	}
}
