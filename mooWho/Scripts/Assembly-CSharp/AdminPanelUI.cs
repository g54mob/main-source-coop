using Mirror;
using UnityEngine;

public class AdminPanelUI : MonoBehaviour
{
	private bool _open;

	private Vector2 _scroll;

	private int _spawnAnimalIndex;

	public static AdminPanelUI Instance { get; private set; }

	public static bool IsOpen
	{
		get
		{
			if (Instance != null)
			{
				return Instance._open;
			}
			return false;
		}
	}

	private PlayerRoleData LocalRoleData
	{
		get
		{
			if (NetworkClient.localPlayer == null)
			{
				return null;
			}
			return NetworkClient.localPlayer.GetComponent<PlayerRoleData>();
		}
	}

	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
	}

	private void Toggle()
	{
		_open = !_open;
		if (_open)
		{
			CursorManager.Instance?.PushUI();
		}
		else
		{
			CursorManager.Instance?.PopUI();
		}
	}
}
