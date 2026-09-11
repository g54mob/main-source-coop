using System;
using UnityEngine;

public class Bot_SimpleMovement : MonoBehaviour
{
	public float movementSpeed = 20f;

	private SphereCollider col;

	private Bot bot;

	private void Start()
	{
		bot = GetComponent<Bot>();
		GameObject gameObject = new GameObject("Collider");
		gameObject.transform.SetParent(base.transform.root);
		col = gameObject.AddComponent<SphereCollider>();
		col.enabled = false;
		Bot obj = bot;
		obj.teleportAction = (Action<Vector3>)Delegate.Combine(obj.teleportAction, new Action<Vector3>(Teleport));
	}

	private void LateUpdate()
	{
		Move();
		Collision();
	}

	private void Collision()
	{
		Collider[] array = Physics.OverlapSphere(bot.Center(), 1f, HelperFunctions.GetMask(HelperFunctions.LayerType.TerrainProp));
		for (int i = 0; i < array.Length; i++)
		{
			col.enabled = true;
			col.transform.position = bot.Center();
			Physics.ComputePenetration(col, col.transform.position, col.transform.rotation, array[i], array[i].transform.position, array[i].transform.rotation, out var direction, out var distance);
			base.transform.position += direction * distance;
			col.enabled = false;
		}
	}

	private void Move()
	{
		RaycastHit groundPosRaycast = HelperFunctions.GetGroundPosRaycast(base.transform.root.position + Vector3.up, HelperFunctions.LayerType.TerrainProp);
		Vector3 forward = bot.syncData.lookDireciton.Flat();
		Vector3 vector = (base.transform.forward * bot.syncData.movementInput.y + base.transform.right * bot.syncData.movementInput.x).Flat();
		if ((bool)groundPosRaycast.transform)
		{
			base.transform.root.position = groundPosRaycast.point;
		}
		base.transform.root.position += movementSpeed * Time.deltaTime * vector;
		base.transform.root.rotation = Quaternion.LookRotation(forward);
	}

	internal void Teleport(Vector3 position)
	{
		base.transform.root.position += position - bot.GroundPos();
	}
}
