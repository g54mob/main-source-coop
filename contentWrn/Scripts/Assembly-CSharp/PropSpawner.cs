using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
	public enum SamplerType
	{
		Sphere = 0,
		Box = 1
	}

	public enum CollisionType
	{
		none = 0,
		any = 1,
		sameSpawner = 2
	}

	public bool useFirstChild = true;

	public GameObject[] objectsToSpawn;

	public int nrOfSpawns = 100;

	public SamplerType samplerType;

	public BoxSampler boxSampler;

	public SphereSampler sphereSampler;

	public bool groundPosition;

	public CollisionType collisionType;

	public float allowedPenetration = 0.01f;

	public float minHeight = float.NegativeInfinity;

	public bool usePerlin;

	public PerlinSampler perlin;

	public float minScale = 1f;

	public float maxScale = 1.2f;

	public float scalePow = 2f;

	[Range(0f, 1f)]
	public float randomRotation = 1f;

	private void Spawn()
	{
		Clear();
		for (int i = 0; i < nrOfSpawns; i++)
		{
			if (!SpawnObject())
			{
				break;
			}
		}
	}

	private void Clear()
	{
		for (int num = base.transform.childCount - 1; num >= 0; num--)
		{
			if (num != 0 || !useFirstChild)
			{
				Object.DestroyImmediate(base.transform.GetChild(num).gameObject);
			}
		}
	}

	private bool SpawnObject()
	{
		GameObject gameObject = HelperFunctions.InstantiatePrefab(GetSourceObject(), base.transform);
		gameObject.transform.localScale *= Mathf.Lerp(minScale, maxScale, Mathf.Pow(Random.value, scalePow));
		gameObject.SetActive(value: true);
		int num = 1000;
		while (num > 0)
		{
			num--;
			ConfigTransform(gameObject);
			Physics.SyncTransforms();
			bool flag = true;
			if (collisionType != CollisionType.none && Collides(gameObject))
			{
				flag = false;
			}
			if (usePerlin && !perlin.Sample(gameObject.transform.position.XZ()))
			{
				flag = false;
			}
			if (gameObject.transform.position.y < minHeight)
			{
				flag = false;
			}
			if (flag)
			{
				break;
			}
		}
		if (num <= 0)
		{
			Object.DestroyImmediate(gameObject);
			return false;
		}
		return true;
	}

	public bool Collides(GameObject spawned)
	{
		Bounds totalBounds = HelperFunctions.GetTotalBounds(spawned);
		Collider[] array = Physics.OverlapBox(totalBounds.center, totalBounds.size * 0.5f);
		List<Collider> list = new List<Collider>(spawned.GetComponentsInChildren<Collider>());
		for (int i = 0; i < array.Length; i++)
		{
			if (list.Contains(array[i]))
			{
				continue;
			}
			Collider collider = array[i];
			if (collisionType == CollisionType.sameSpawner && !collider.transform.IsChildOf(base.transform))
			{
				continue;
			}
			for (int j = 0; j < list.Count; j++)
			{
				Collider collider2 = list[j];
				Physics.ComputePenetration(collider2, collider2.transform.position, collider2.transform.rotation, collider, collider.transform.position, collider.transform.rotation, out var _, out var distance);
				if (distance > allowedPenetration)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void ConfigTransform(GameObject spawned)
	{
		spawned.transform.position = GetPosition();
		spawned.transform.rotation = GetRotation(spawned.transform.rotation);
	}

	private Quaternion GetRotation(Quaternion currentRotation)
	{
		return Quaternion.Lerp(currentRotation, Random.rotation, randomRotation);
	}

	private Vector3 GetPosition()
	{
		Vector3 zero = Vector3.zero;
		zero = ((samplerType != SamplerType.Box) ? sphereSampler.GetPosition(base.transform.position) : boxSampler.GetPosition(base.transform.position));
		if (groundPosition)
		{
			zero = HelperFunctions.GetGroundPos(zero, HelperFunctions.LayerType.Terrain);
		}
		return zero;
	}

	private GameObject GetSourceObject()
	{
		if (useFirstChild)
		{
			return base.transform.GetChild(0).gameObject;
		}
		return objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
	}

	private void OnDrawGizmosSelected()
	{
		if (samplerType == SamplerType.Box)
		{
			Gizmos.DrawWireCube(base.transform.position, boxSampler.size);
		}
		if (samplerType == SamplerType.Sphere)
		{
			Gizmos.DrawWireSphere(base.transform.position, sphereSampler.radius);
		}
	}
}
