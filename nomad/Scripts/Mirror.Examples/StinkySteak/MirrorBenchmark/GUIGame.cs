using Mirror;
using StinkySteak.NetcodeBenchmark;
using UnityEngine;

namespace StinkySteak.MirrorBenchmark
{
	public class GUIGame : BaseGUIGame
	{
		[SerializeField]
		private NetworkManager _networkManagerPrefab;

		private NetworkManager _networkManager;

		protected override void Initialize()
		{
			base.Initialize();
			_networkManager = Object.Instantiate(_networkManagerPrefab);
			RegisterPrefabs(new StressTestEssential[3] { _test_1, _test_2, _test_3 });
		}

		private void RegisterPrefabs(StressTestEssential[] stressTestEssential)
		{
			for (int i = 0; i < stressTestEssential.Length; i++)
			{
				_networkManager.spawnPrefabs.Add(stressTestEssential[i].Prefab);
			}
		}

		protected override void OnCustomGUI()
		{
			if (GUILayout.Button("Start Client"))
			{
				_networkManager.StartClient();
			}
			if (GUILayout.Button("Start Server"))
			{
				_networkManager.StartServer();
			}
		}

		protected override void StressTest(StressTestEssential stressTest)
		{
			for (int i = 0; i < stressTest.SpawnCount; i++)
			{
				NetworkServer.Spawn(Object.Instantiate(stressTest.Prefab));
			}
		}

		protected override void UpdateNetworkStats()
		{
			if (!(_networkManager == null) && _networkManager.isNetworkActive)
			{
				if (_networkManager.mode == NetworkManagerMode.ServerOnly)
				{
					_textLatency = "Latency: 0ms (Server)";
				}
				else
				{
					_textLatency = $"Latency: {NetworkTime.rtt * 1000.0}ms";
				}
			}
		}
	}
}
