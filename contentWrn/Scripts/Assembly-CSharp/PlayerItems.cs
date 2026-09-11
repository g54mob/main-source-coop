using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItems : MonoBehaviour
{
	public InputActionReference item1Action;

	public InputActionReference item2Action;

	public InputActionReference item3Action;

	private Player player;

	private Bodypart itemBodypart;

	private Transform targetHandGizmo;

	public int m_displayingSlot = -1;

	private float m_fixedWeightTime;

	private float shakeTime;

	private void Start()
	{
		player = GetComponent<Player>();
		m_fixedWeightTime = Time.fixedDeltaTime * 10f;
	}

	private void FixedUpdate()
	{
		if ((bool)player.data.currentItem && player.data.emoteTime <= 0f)
		{
			player.data.currentItem.ConfigItemPosition(player);
			player.refs.IK_Hand_R.transform.SetPositionAndRotation(targetHandGizmo.transform.position - player.data.lookDirection * (player.data.throwCharge * 0.15f), targetHandGizmo.transform.rotation);
			player.refs.IK_Right.weight = Mathf.Lerp(player.refs.IK_Right.weight, 1f, m_fixedWeightTime);
		}
		else
		{
			player.refs.IK_Right.weight = Mathf.Lerp(player.refs.IK_Right.weight, 0f, m_fixedWeightTime);
		}
	}

	private void Update()
	{
		if (player.data.isLocal && player.data.physicsAreReady && !player.HasLockedInput() && GlobalInputHandler.CanTakeInput())
		{
			for (int i = 0; i < 4; i++)
			{
				CheckKey(i);
			}
		}
		if (m_displayingSlot != player.data.selectedItemSlot)
		{
			ChangeToSlot(player.data.selectedItemSlot);
		}
		if (m_displayingSlot != -1 && player.TryGetInventory(out var o) && o.TryGetSlot(m_displayingSlot, out var slot))
		{
			if (slot.ItemInSlot.item != null && player.data.currentItem == null)
			{
				EquipItem(slot.ItemInSlot);
			}
			else if (slot.ItemInSlot.item == null && player.data.currentItem != null)
			{
				Unequip();
			}
		}
		if ((player.input.dropItemWasReleased || player.data.dropItemsFor > 0f) && player.refs.view.IsMine)
		{
			DropItem(player.data.selectedItemSlot);
		}
		if (player.input.dropItemIsPressed && player.data.currentItem != null)
		{
			player.data.throwCharge = Mathf.MoveTowards(player.data.throwCharge, 1f, Time.deltaTime);
			if (player.data.isHangingUpsideDown)
			{
				player.data.throwCharge *= 0f;
			}
			if (Time.time > shakeTime + 0.1f)
			{
				GamefeelHandler.instance.perlin.AddShake(player.data.throwCharge);
				shakeTime = Time.time;
			}
		}
		else
		{
			player.data.throwCharge = Mathf.MoveTowards(player.data.throwCharge, 0f, Time.deltaTime);
		}
		void CheckKey(int slotID)
		{
			InputActionReference inputActionReference = null;
			GlobalInputHandler.InputKey inputKey = null;
			switch (slotID)
			{
			case 0:
				inputActionReference = item1Action;
				inputKey = GlobalInputHandler.SelectItem1Key;
				break;
			case 1:
				inputActionReference = item2Action;
				inputKey = GlobalInputHandler.SelectItem2Key;
				break;
			case 2:
				inputActionReference = item3Action;
				inputKey = GlobalInputHandler.SelectItem3Key;
				break;
			}
			if (!(inputActionReference == null) && (inputActionReference.action.WasPressedThisFrame() || inputKey.GetKeyDown()))
			{
				if (slotID == player.data.selectedItemSlot)
				{
					player.data.selectedItemSlot = -1;
				}
				else
				{
					player.data.selectedItemSlot = slotID;
				}
			}
		}
	}

	private void ChangeToSlot(int slotID)
	{
		if (player.TryGetInventory(out var o))
		{
			if (m_displayingSlot != slotID && o.TryGetItemInSlot(m_displayingSlot, out var item) && item.item != null && item.data.TryGetEntry<StashAbleEntry>(out var t) && !t.isStashAble)
			{
				Debug.Log("Dropping because " + item.item.name + " it's not stashable");
				player.data.throwCharge = 0.3f;
				DropItem(m_displayingSlot, notCurrentItem: true);
			}
			m_displayingSlot = slotID;
			if (o.TryGetItemInSlot(slotID, out var item2))
			{
				EquipItem(item2);
			}
			else
			{
				Unequip();
			}
		}
	}

	public void DropItem(int slotID, bool notCurrentItem = false)
	{
		if ((!notCurrentItem && player.data.currentItem == null) || !player.TryGetInventory(out var o))
		{
			return;
		}
		_ = player.data.currentItem.item.id;
		Vector3 vector = player.data.currentItem.transform.position;
		Vector3 vector2 = player.data.currentItem.transform.position + player.data.lookDirection * 0.2f;
		if (!HelperFunctions.LineCheck(vector, vector2, HelperFunctions.LayerType.TerrainProp).transform)
		{
			vector = vector2;
		}
		Quaternion rotation = player.data.currentItem.transform.rotation;
		Vector3 linearVelocity = player.data.currentItem.rig.linearVelocity;
		Vector3 angularVelocity = player.data.currentItem.rig.angularVelocity;
		linearVelocity = Vector3.Lerp(linearVelocity, player.data.lookDirection * 15f, player.data.throwCharge);
		angularVelocity += Vector3.Cross(Vector3.up, linearVelocity) * 0.1f;
		if (o.TryRemoveItemFromSlot(slotID, out var item))
		{
			if (player.data.dropItemsFor > 0f)
			{
				Debug.LogError("DROPPING ITEM BECAUSE OF SNATCHO!!");
			}
			VerboseDebug.Log($"Requesting pickup for {item.item.name} in slot {player.data.selectedItemSlot} throwcharge {player.data.throwCharge}");
			player.RequestCreatePickup(item.item.id, item.data, vector, rotation, linearVelocity, angularVelocity);
		}
		else
		{
			Debug.LogError("Failed to remove item from slot");
		}
	}

	internal void EquipItem(ItemDescriptor item)
	{
		bool flag = player.data.currentItem != null && player.data.currentItem.item == item.item;
		Unequip();
		if (!(item.item == null || flag))
		{
			Equip(item);
		}
	}

	private void Equip(ItemDescriptor itemDescriptor)
	{
		Item item = itemDescriptor.item;
		Vector3 spawnPos = GetSpawnPos(item);
		Quaternion spawnRot = GetSpawnRot(item);
		GameObject gameObject = Object.Instantiate(item.itemObject, spawnPos, spawnRot, base.transform);
		player.data.currentItem = gameObject.GetComponent<ItemInstance>();
		player.data.currentItem.InitItem(item, itemDescriptor.data, player.refs.view, null);
		player.data.currentItem.gameObject.AddComponent<FixedJoint>().connectedBody = player.refs.ragdoll.GetBodypart(BodypartType.Hand_R).rig;
		Collider[] componentsInChildren = player.data.currentItem.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		player.refs.ragdoll.AddItem(player.data.currentItem);
		itemBodypart = player.refs.ragdoll.GetBodypart(BodypartType.Item);
		player.data.currentItem.SetItemBody(itemBodypart);
		Vector3 relativePosition_Anim = player.GetRelativePosition_Anim(BodypartType.Torso, Vector3.forward);
		Quaternion spawnRot2 = Quaternion.LookRotation(player.data.lookDirection);
		GameObject gameObject2 = player.refs.animRefHandler.AddItem(player.data.currentItem, relativePosition_Anim, spawnRot2);
		targetHandGizmo = gameObject2.GetComponentInChildren<HandGizmo>().transform;
		player.data.currentItem.onItemEquipped?.Invoke(player);
		GameAPI.instance.objectSpawnedAction?.Invoke(player.data.currentItem.gameObject);
	}

	private Quaternion GetSpawnRot(Item item)
	{
		Quaternion quaternion = Quaternion.Inverse(item.itemObject.GetComponentInChildren<HandGizmo>().transform.rotation) * item.itemObject.transform.rotation;
		return player.refs.ragdoll.GetBodypart(BodypartType.Hand_R).transform.rotation * quaternion;
	}

	private Vector3 GetSpawnPos(Item item)
	{
		_ = Vector3.zero;
		Vector3 position = item.itemObject.GetComponentInChildren<HandGizmo>().transform.InverseTransformPoint(item.itemObject.transform.position);
		return player.refs.ragdoll.GetBodypart(BodypartType.Hand_R).transform.TransformPoint(position);
	}

	private void Unequip()
	{
		if (player.data.currentItem != null)
		{
			player.data.currentItem.onUnequip?.Invoke(player);
			Object.Destroy(player.refs.ragdoll.GetBodypart(BodypartType.Item).animationTarget.gameObject);
			player.refs.ragdoll.RemoveItem(player.data.currentItem);
			Object.Destroy(player.data.currentItem.gameObject);
		}
		itemBodypart = null;
		player.data.currentItem = null;
	}
}
