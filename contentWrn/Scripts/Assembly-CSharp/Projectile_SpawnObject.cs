using System;
using Photon.Pun;
using UnityEngine;

public class Projectile_SpawnObject : MonoBehaviour
{
	public enum DirectionType
	{
		NormalIsForwrd = 0,
		NormalIsUp = 1
	}

	public bool photonSpawn;

	public bool useInstance;

	public GameObject objectToSpawn;

	private GameObject objectInstance;

	public DirectionType directionType;

	private void Awake()
	{
		Projectile component = GetComponent<Projectile>();
		component.hitAction = (Action<RaycastHit>)Delegate.Combine(component.hitAction, new Action<RaycastHit>(Hit));
		if (useInstance)
		{
			objectInstance = UnityEngine.Object.Instantiate(objectToSpawn);
			objectInstance.SetActive(value: false);
		}
	}

	private void OnDestroy()
	{
		if (useInstance)
		{
			UnityEngine.Object.Destroy(objectInstance);
		}
	}

	private void Hit(RaycastHit hit)
	{
		if (photonSpawn)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				PhotonNetwork.Instantiate(objectToSpawn.name, hit.point, GetRotation(hit.normal), 0);
			}
		}
		else if (useInstance)
		{
			objectInstance.transform.position = hit.point;
			objectInstance.transform.rotation = GetRotation(hit.normal);
			objectInstance.SetActive(value: true);
		}
		else
		{
			UnityEngine.Object.Instantiate(objectToSpawn, hit.point, GetRotation(hit.normal));
		}
	}

	private Quaternion GetRotation(Vector3 normal)
	{
		if (directionType == DirectionType.NormalIsForwrd)
		{
			return Quaternion.LookRotation(normal);
		}
		return HelperFunctions.GetRandomRotationWithUp(normal);
	}
}
