using System.Collections.Generic;
using UnityEngine;

public class GameSettingsPanelUI : MonoBehaviour
{
	public static bool HostCanPreviewSoundRecords = true;

	[Header("Liste")]
	public Transform itemContainer;

	public GameObject playerItemPrefab;

	private readonly Dictionary<MyClient, GameSettingsPlayerItem> _items = new Dictionary<MyClient, GameSettingsPlayerItem>();

	private float _rescanTimer;

	private const float RescanInterval = 0.5f;

	private void OnEnable()
	{
		_rescanTimer = 0f;
		RebuildIfNeeded();
	}

	private void Update()
	{
		_rescanTimer -= Time.deltaTime;
		if (_rescanTimer <= 0f)
		{
			_rescanTimer = 0.5f;
			RebuildIfNeeded();
		}
	}

	private void RebuildIfNeeded()
	{
		MyClient[] array = Object.FindObjectsOfType<MyClient>();
		List<MyClient> list = new List<MyClient>();
		foreach (KeyValuePair<MyClient, GameSettingsPlayerItem> item in _items)
		{
			if (item.Key == null)
			{
				list.Add(item.Key);
			}
		}
		foreach (MyClient item2 in list)
		{
			if (_items[item2] != null)
			{
				Object.Destroy(_items[item2].gameObject);
			}
			_items.Remove(item2);
		}
		MyClient[] array2 = array;
		foreach (MyClient myClient in array2)
		{
			PlayerRoleData component = myClient.GetComponent<PlayerRoleData>();
			if (!_items.ContainsKey(myClient))
			{
				CreateItem(myClient, component);
			}
			else
			{
				UpdateItem(_items[myClient], myClient, component);
			}
		}
		ReorderItems();
	}

	private void CreateItem(MyClient client, PlayerRoleData roleData)
	{
		if (!(itemContainer == null) && !(playerItemPrefab == null))
		{
			GameSettingsPlayerItem component = Object.Instantiate(playerItemPrefab, itemContainer).GetComponent<GameSettingsPlayerItem>();
			if (!(component == null))
			{
				component.Setup(client, roleData, GetProfileSprite(roleData));
				_items[client] = component;
			}
		}
	}

	private void UpdateItem(GameSettingsPlayerItem item, MyClient client, PlayerRoleData roleData)
	{
		if (!(item == null))
		{
			item.Setup(client, roleData, GetProfileSprite(roleData));
		}
	}

	private Sprite GetProfileSprite(PlayerRoleData roleData)
	{
		if (roleData == null || MenuManager.Instance == null)
		{
			return null;
		}
		return MenuManager.Instance.GetAvatarSprite(roleData);
	}

	private void ReorderItems()
	{
		int num = 0;
		foreach (KeyValuePair<MyClient, GameSettingsPlayerItem> item in _items)
		{
			if (item.Key != null && item.Key.isLocalPlayer)
			{
				item.Value.transform.SetSiblingIndex(num++);
			}
		}
		foreach (KeyValuePair<MyClient, GameSettingsPlayerItem> item2 in _items)
		{
			if (item2.Key != null && !item2.Key.isLocalPlayer)
			{
				item2.Value.transform.SetSiblingIndex(num++);
			}
		}
	}
}
