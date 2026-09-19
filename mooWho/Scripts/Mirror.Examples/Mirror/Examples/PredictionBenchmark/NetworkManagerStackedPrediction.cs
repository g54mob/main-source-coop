using UnityEngine;

namespace Mirror.Examples.PredictionBenchmark
{
	[AddComponentMenu("")]
	public class NetworkManagerStackedPrediction : NetworkManager
	{
		[Header("Spawns")]
		public int spawnAmount = 1000;

		public GameObject spawnPrefab;

		public float interleave = 1f;

		[Tooltip("Stacked Cubes are only stable if solver iterations are high enough!\nDefault is 1, max is 255.")]
		public int solverIterations = 200;

		public int solverVelocityIterations = 1;

		public override void Awake()
		{
			base.Awake();
			QualitySettings.vSyncCount = 0;
			int defaultSolverIterations = Physics.defaultSolverIterations;
			Physics.defaultSolverIterations = solverIterations;
			Debug.Log($"Physics.defaultSolverIterations: {defaultSolverIterations} -> {Physics.defaultSolverIterations}");
			defaultSolverIterations = Physics.defaultSolverVelocityIterations;
			Physics.defaultSolverVelocityIterations = solverVelocityIterations;
			Debug.Log($"Physics.defaultSolverVelocityIterations: {defaultSolverIterations} -> {Physics.defaultSolverVelocityIterations}");
		}

		private void SpawnAll()
		{
			float num = Mathf.Sqrt(spawnAmount);
			float num2 = (0f - num) / 2f * interleave;
			int num3 = 0;
			for (int i = 0; (float)i < num; i++)
			{
				for (int j = 0; (float)j < num; j++)
				{
					if (num3 < spawnAmount)
					{
						float num4 = interleave + Physics.defaultContactOffset;
						float x = num2 + (float)i * num4;
						float y = (float)j * num4;
						GameObject obj = Object.Instantiate(spawnPrefab);
						obj.transform.position = new Vector3(x, y, 0f);
						NetworkServer.Spawn(obj);
						num3++;
					}
				}
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			SpawnAll();
		}
	}
}
