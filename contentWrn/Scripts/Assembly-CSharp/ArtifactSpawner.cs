using System;
using DefaultNamespace.Artifacts;
using Photon.Pun;
using UnityEngine;
using pworld.Scripts.Extensions;

public class ArtifactSpawner : MonoBehaviour
{
	public Item artifactToSpawn;

	public Pickup spawnedArtifact;

	public float spawnAbovePatrolPoint = 1f;

	public Vector2 minMaxThrowForce = new Vector2(5f, 30f);

	public float maxWaitForRest = 5f;

	private readonly int maxNrOfThrows = 10;

	private int nrOfThrows;

	private float timeSinceThrow;

	public bool debugDontMove;

	public Pickup completedSpawn;

	private void Start()
	{
		timeSinceThrow = float.MaxValue;
	}

	private void Update()
	{
		if (!PhotonNetwork.IsMasterClient || artifactToSpawn == null)
		{
			return;
		}
		timeSinceThrow += Time.deltaTime;
		if (nrOfThrows > maxNrOfThrows)
		{
			Debug.LogWarning("Max nr of throws reached DELETING ITEM AND SPAWNER");
			PhotonNetwork.Destroy(spawnedArtifact.gameObject);
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
		else if (timeSinceThrow > maxWaitForRest)
		{
			Vector3 position = base.transform.position;
			if (!debugDontMove)
			{
				base.transform.position = RoundArtifactSpawner.GetRandPointWithWeight();
			}
			Debug.DrawLine(position, base.transform.position, Color.red, 10f);
			Vector3 vector = base.transform.position + Vector3.up;
			if (spawnedArtifact == null)
			{
				spawnedArtifact = PickupHandler.CreatePickup(artifactToSpawn.id, new ItemInstanceData(Guid.NewGuid()), vector, UnityEngine.Random.rotation);
			}
			timeSinceThrow = 0f;
			nrOfThrows++;
			spawnedArtifact.Rigidbody.position = vector;
			spawnedArtifact.Rigidbody.AddForce(UnityEngine.Random.onUnitSphere * minMaxThrowForce.PRndRange(), ForceMode.Impulse);
			spawnedArtifact.Rigidbody.AddTorque(UnityEngine.Random.onUnitSphere * (minMaxThrowForce.PRndRange() * 0.2f));
		}
		else if (spawnedArtifact != null && spawnedArtifact.Rigidbody.IsSleeping())
		{
			spawnedArtifact.GetComponentInChildren<ISpawnedByArtifactSpawner>()?.OnFinishSpawning();
			completedSpawn = spawnedArtifact;
			spawnedArtifact = null;
			base.gameObject.SetActive(value: false);
		}
	}
}
