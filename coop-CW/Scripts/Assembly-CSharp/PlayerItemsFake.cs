using System;
using UnityEngine;

public class PlayerItemsFake : MonoBehaviour
{
	private Player player;

	private Bodypart itemBodypart;

	private Transform targetHandGizmo;

	public FakeFlashLight fakeFlashLight;

	private bool once;

	public GameObject itemCopy;

	public ItemInstance CurrentItem => fakeFlashLight.itemInstance;

	private void Start()
	{
		player = GetComponent<Player>();
	}

	private void LateUpdate()
	{
		if (!once)
		{
			once = true;
			if (fakeFlashLight.gameObject == null)
			{
				Debug.LogError("No fake flashlight set on player items fake!");
			}
			ItemDescriptor itemDescriptor = new ItemDescriptor(fakeFlashLight.itemInstance.item, new ItemInstanceData(Guid.NewGuid()));
			Debug.LogError($" itemDescriptor: {itemDescriptor} fakeFlashLight.gameObject: {fakeFlashLight.gameObject}");
			if (fakeFlashLight.gameObject != null)
			{
				Equip(itemDescriptor, fakeFlashLight.gameObject);
			}
			else
			{
				Debug.LogError("shit is null somehow");
			}
		}
	}

	private void FixedUpdate()
	{
		player.data.lookDirection = player.refs.ragdoll.GetBodypart(BodypartType.Torso).transform.forward;
		if (itemCopy != null)
		{
			CurrentItem.ConfigItemPosition(player);
			player.refs.IK_Hand_R.transform.position = targetHandGizmo.transform.position + -player.data.lookDirection * (player.data.throwCharge * 0.15f);
			player.refs.IK_Hand_R.transform.rotation = targetHandGizmo.transform.rotation;
			player.refs.IK_Right.weight = Mathf.Lerp(player.refs.IK_Right.weight, 1f, Time.fixedDeltaTime * 10f);
		}
		else
		{
			player.refs.IK_Right.weight = Mathf.Lerp(player.refs.IK_Right.weight, 0f, Time.fixedDeltaTime * 10f);
		}
	}

	private void Equip(ItemDescriptor itemDescriptor, GameObject fakeItemGo)
	{
		Item item = itemDescriptor.item;
		Vector3 spawnPos = GetSpawnPos(item);
		Quaternion spawnRot = GetSpawnRot(item);
		CurrentItem.InitItem(item, itemDescriptor.data, player.refs.view, null);
		fakeItemGo.transform.position = spawnPos;
		fakeItemGo.transform.rotation = spawnRot;
		CurrentItem.gameObject.AddComponent<FixedJoint>().connectedBody = player.refs.ragdoll.GetBodypart(BodypartType.Hand_R).rig;
		player.refs.ragdoll.AddItem(CurrentItem);
		itemBodypart = player.refs.ragdoll.GetBodypart(BodypartType.Item);
		CurrentItem.SetItemBody(itemBodypart);
		Vector3 relativePosition_Anim = player.GetRelativePosition_Anim(BodypartType.Torso, Vector3.forward);
		Quaternion spawnRot2 = Quaternion.LookRotation(player.data.lookDirection);
		itemCopy = player.refs.animRefHandler.AddItem(CurrentItem, relativePosition_Anim, spawnRot2);
		targetHandGizmo = itemCopy.GetComponentInChildren<HandGizmo>().transform;
		Debug.LogError("FakeItemEquip");
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
}
