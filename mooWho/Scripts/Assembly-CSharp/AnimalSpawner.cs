using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class AnimalSpawner : NetworkBehaviour
{
	[Header("Test Spawn Ayarları")]
	[Tooltip("Sağ tık menüsünden spawnlanacak hayvan türü")]
	public AnimalType spawnType = AnimalType.Cow;

	[Tooltip("Tek seferde kaç adet spawnlanacak")]
	public int spawnCount = 1;

	public float spawnRadius = 25f;

	[Header("Prefab Kaynağı")]
	[Tooltip("Boşsa GameManager.animalPrefabs'tan türü bulur")]
	public GameObject overridePrefab;

	[Header("Ground")]
	public LayerMask groundMask;

	private readonly List<GameObject> _spawnedAnimals = new List<GameObject>();

	public static AnimalSpawner Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	[ContextMenu("Spawn Selected Animal")]
	public void SpawnSelectedAnimal()
	{
		if (!Application.isPlaying)
		{
			Debug.LogWarning("[AnimalSpawner] Sadece Play mode'da spawnlanabilir.");
			return;
		}
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[AnimalSpawner] Sadece server/host spawnlayabilir.");
			return;
		}
		GameObject gameObject = ResolvePrefab(spawnType);
		if (gameObject == null)
		{
			Debug.LogWarning($"[AnimalSpawner] Prefab bulunamadı: {spawnType}");
			return;
		}
		for (int i = 0; i < Mathf.Max(1, spawnCount); i++)
		{
			SpawnOne(gameObject);
		}
		Debug.Log($"[AnimalSpawner] {spawnCount}x {spawnType} spawnlandı.");
	}

	[ContextMenu("Clear Spawned Animals")]
	public void ClearSpawnedAnimals()
	{
		if (!Application.isPlaying || !NetworkServer.active)
		{
			return;
		}
		foreach (GameObject spawnedAnimal in _spawnedAnimals)
		{
			if (spawnedAnimal != null)
			{
				NetworkServer.Destroy(spawnedAnimal);
			}
		}
		_spawnedAnimals.Clear();
		Debug.Log("[AnimalSpawner] Spawnlanan hayvanlar temizlendi.");
	}

	[Server]
	private void SpawnOne(GameObject prefab)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AnimalSpawner::SpawnOne(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		Vector3 randomPoint = GetRandomPoint();
		GameObject gameObject = Object.Instantiate(prefab, randomPoint, Quaternion.identity);
		NetworkServer.Spawn(gameObject);
		_spawnedAnimals.Add(gameObject);
	}

	private GameObject ResolvePrefab(AnimalType type)
	{
		if (overridePrefab != null)
		{
			return overridePrefab;
		}
		if (GameManager.Instance != null && GameManager.Instance.animalPrefabs != null)
		{
			AnimalPrefabEntry[] animalPrefabs = GameManager.Instance.animalPrefabs;
			foreach (AnimalPrefabEntry animalPrefabEntry in animalPrefabs)
			{
				if (animalPrefabEntry != null && animalPrefabEntry.type == type && animalPrefabEntry.prefab != null)
				{
					return animalPrefabEntry.prefab;
				}
			}
		}
		return null;
	}

	private Vector3 GetRandomPoint()
	{
		Vector3 position = base.transform.position;
		for (int i = 0; i < 15; i++)
		{
			Vector2 vector = Random.insideUnitCircle * spawnRadius;
			if (NavMesh.SamplePosition(position + new Vector3(vector.x, 0f, vector.y), out var hit, 5f, -1))
			{
				return hit.position;
			}
		}
		return position;
	}

	public override bool Weaved()
	{
		return true;
	}
}
