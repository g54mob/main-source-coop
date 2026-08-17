using UnityEngine;

namespace Mirror.Examples.PredictionBenchmark
{
	[AddComponentMenu("")]
	public class NetworkManagerPredictionBenchmark : NetworkManager
	{
		[Header("Spawns")]
		public int spawnAmount = 1000;

		public GameObject spawnPrefab;

		public Bounds spawnArea = new Bounds(new Vector3(0f, 2.5f, 0f), new Vector3(10f, 5f, 10f));

		public override void Awake()
		{
			base.Awake();
			QualitySettings.vSyncCount = 0;
		}

		private void SpawnAll()
		{
			for (int i = 0; i < spawnAmount; i++)
			{
				float x = Random.Range(spawnArea.min.x, spawnArea.max.x);
				float y = Random.Range(spawnArea.min.y, spawnArea.max.y);
				float z = Random.Range(spawnArea.min.z, spawnArea.max.z);
				Vector3 position = new Vector3(x, y, z);
				GameObject obj = Object.Instantiate(spawnPrefab);
				obj.transform.position = position;
				NetworkServer.Spawn(obj);
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			SpawnAll();
			if (base.mode == NetworkManagerMode.ServerOnly)
			{
				Camera.main.enabled = false;
			}
		}
	}
}
