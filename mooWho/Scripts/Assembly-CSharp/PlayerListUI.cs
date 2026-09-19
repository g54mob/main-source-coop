using System.Collections.Generic;
using UnityEngine;

public class PlayerListUI : MonoBehaviour
{
	[Header("Liste")]
	public Transform itemContainer;

	public GameObject playerItemPrefab;

	[Header("Profil Sprite")]
	[Tooltip("Avcı profil resmi")]
	public Sprite hunterSprite;

	[Tooltip("GameManager veya sprite kaynağı (GetAnimalProfileSprite için)")]
	public RoleAssignmentUI spriteSource;

	private readonly Dictionary<MyClient, PlayerItem> _items = new Dictionary<MyClient, PlayerItem>();

	private float _rescanTimer;

	private const float RescanInterval = 0.5f;

	public static PlayerListUI Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
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
		foreach (KeyValuePair<MyClient, PlayerItem> item in _items)
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
			if (myClient.isLocalPlayer)
			{
				continue;
			}
			PlayerRoleData component = myClient.GetComponent<PlayerRoleData>();
			if (!(component == null) && component.RolesLocked)
			{
				if (!_items.ContainsKey(myClient))
				{
					CreateItem(myClient, component);
				}
				else
				{
					UpdateItem(_items[myClient], myClient, component);
				}
			}
		}
		ReorderItems();
	}

	private void CreateItem(MyClient client, PlayerRoleData roleData)
	{
		if (itemContainer == null || playerItemPrefab == null)
		{
			return;
		}
		PlayerItem component = Object.Instantiate(playerItemPrefab, itemContainer).GetComponent<PlayerItem>();
		if (!(component == null))
		{
			bool isHunter = roleData.Role == PlayerRole.Hunter;
			Sprite profileSprite = GetProfileSprite(roleData, isHunter);
			component.Setup(client, roleData, profileSprite, isHunter);
			_items[client] = component;
			Health component2 = client.GetComponent<Health>();
			if (component2 != null)
			{
				component.SetDead(component2.IsDead, animate: false);
			}
		}
	}

	private void UpdateItem(PlayerItem item, MyClient client, PlayerRoleData roleData)
	{
		if (!(item == null))
		{
			bool isHunter = roleData.Role == PlayerRole.Hunter;
			Sprite profileSprite = GetProfileSprite(roleData, isHunter);
			item.Setup(client, roleData, profileSprite, isHunter);
			Health component = client.GetComponent<Health>();
			if (component != null)
			{
				item.SetDead(component.IsDead);
			}
		}
	}

	public void RefreshClient(MyClient client)
	{
		if (client == null || client.isLocalPlayer)
		{
			return;
		}
		PlayerRoleData component = client.GetComponent<PlayerRoleData>();
		if (!(component == null) && component.RolesLocked)
		{
			if (!_items.TryGetValue(client, out var value) || value == null)
			{
				CreateItem(client, component);
				ReorderItems();
			}
			else
			{
				UpdateItem(value, client, component);
			}
		}
	}

	private Sprite GetProfileSprite(PlayerRoleData roleData, bool isHunter)
	{
		if (isHunter)
		{
			return hunterSprite;
		}
		if (spriteSource != null)
		{
			return spriteSource.GetAnimalProfileSprite(roleData.AssignedAnimal);
		}
		return null;
	}

	private void ReorderItems()
	{
		int num = 0;
		foreach (KeyValuePair<MyClient, PlayerItem> item in _items)
		{
			if (item.Value != null && item.Value.IsHunter)
			{
				item.Value.transform.SetSiblingIndex(num++);
			}
		}
		foreach (KeyValuePair<MyClient, PlayerItem> item2 in _items)
		{
			if (item2.Value != null && !item2.Value.IsHunter)
			{
				item2.Value.transform.SetSiblingIndex(num++);
			}
		}
	}
}
