using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListUI : MonoBehaviour
{
	[Header("Liste")]
	public Transform listContainer;

	public GameObject lobbyItemPrefab;

	[Header("Sonuç Göstergesi")]
	public TextMeshProUGUI resultCountText;

	public Image resultStatusImage;

	public Color foundColor = new Color(0.3f, 0.8f, 0.3f);

	public Color emptyColor = new Color(0.85f, 0.3f, 0.3f);

	[Header("Butonlar")]
	public Button backButton;

	public Button refreshButton;

	public Button createButton;

	[Header("Kod ile Katılma")]
	public TMP_InputField joinCodeInput;

	public Button joinCodeButton;

	public TextMeshProUGUI joinCodeFeedback;

	private readonly List<GameObject> _spawnedItems = new List<GameObject>();

	public static LobbyListUI Instance { get; private set; }

	public bool IsScreenVisible
	{
		get
		{
			if (listContainer != null)
			{
				return listContainer.gameObject.activeInHierarchy;
			}
			return false;
		}
	}

	private void Awake()
	{
		Instance = this;
		if (backButton != null)
		{
			backButton.onClick.AddListener(OnBack);
		}
		if (refreshButton != null)
		{
			refreshButton.onClick.AddListener(OnRefresh);
		}
		if (createButton != null)
		{
			createButton.onClick.AddListener(OnCreate);
		}
		if (joinCodeButton != null)
		{
			joinCodeButton.onClick.AddListener(OnJoinByCode);
		}
	}

	private void OnEnable()
	{
		OnRefresh();
	}

	public void PopulateList(List<Lobby> lobbies)
	{
		ClearList();
		int num = lobbies?.Count ?? 0;
		if (resultCountText != null)
		{
			resultCountText.text = Localization.GetPlural("LOBBY_ROOMS_FOUND", num, num);
		}
		if (resultStatusImage != null)
		{
			resultStatusImage.color = ((num > 0) ? foundColor : emptyColor);
		}
		if (lobbies == null)
		{
			return;
		}
		foreach (Lobby lobby in lobbies)
		{
			if (listContainer == null || lobbyItemPrefab == null)
			{
				break;
			}
			GameObject gameObject = Object.Instantiate(lobbyItemPrefab, listContainer);
			LobbyListItem component = gameObject.GetComponent<LobbyListItem>();
			if (component != null)
			{
				component.Setup(lobby);
			}
			_spawnedItems.Add(gameObject);
		}
	}

	private void ClearList()
	{
		foreach (GameObject spawnedItem in _spawnedItems)
		{
			if (spawnedItem != null)
			{
				Object.Destroy(spawnedItem);
			}
		}
		_spawnedItems.Clear();
	}

	private void OnBack()
	{
		MenuManager.Instance?.ShowPlayScreenPublic();
	}

	private void OnRefresh()
	{
		if (SteamLobby.instance != null)
		{
			SteamLobby.instance.RefreshLobbyList();
		}
	}

	private void OnCreate()
	{
		MenuManager.Instance?.ShowCreateLobbyPublic();
	}

	private void OnJoinByCode()
	{
		if (joinCodeInput == null || SteamLobby.instance == null)
		{
			return;
		}
		string text = joinCodeInput.text;
		if (joinCodeFeedback != null)
		{
			joinCodeFeedback.text = Localization.Get("LOBBY_SEARCHING");
		}
		SteamLobby.instance.JoinByCode(text, delegate(bool success)
		{
			if (joinCodeFeedback != null)
			{
				joinCodeFeedback.text = (success ? Localization.Get("LOBBY_JOINING") : Localization.Get("LOBBY_CODE_NOT_FOUND"));
			}
		});
	}

	public void ShowJoinFeedback(string message)
	{
		if (joinCodeFeedback != null)
		{
			joinCodeFeedback.text = message;
		}
	}
}
