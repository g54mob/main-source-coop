using System;
using StinkySteak.SimulationTimer;
using UnityEngine;

namespace StinkySteak.NetcodeBenchmark
{
	public class BaseGUIGame : MonoBehaviour
	{
		[Serializable]
		public struct StressTestEssential
		{
			public int SpawnCount;

			public GameObject Prefab;
		}

		[Space]
		protected string _textLatency = "";

		[SerializeField]
		private float _updateLatencyTextInterval = 1f;

		private StinkySteak.SimulationTimer.SimulationTimer _timerUpdateLatencyText;

		[Header("Stress Test 1: Move Y")]
		[SerializeField]
		protected StressTestEssential _test_1;

		[Header("Stress Test 2: Move All Axis")]
		[SerializeField]
		protected StressTestEssential _test_2;

		[Header("Stress Test 3: Move Wander")]
		[SerializeField]
		protected StressTestEssential _test_3;

		private void Start()
		{
			Initialize();
		}

		protected virtual void Initialize()
		{
		}

		protected virtual void OnCustomGUI()
		{
		}

		protected virtual void OnGUI()
		{
			GUILayout.BeginArea(new Rect(100f, 100f, 300f, 400f));
			if (GUILayout.Button("Stress Test 1"))
			{
				StressTest_1();
			}
			if (GUILayout.Button("Stress Test 2"))
			{
				StressTest_2();
			}
			if (GUILayout.Button("Stress Test 3"))
			{
				StressTest_3();
			}
			OnCustomGUI();
			GUILayout.Label(_textLatency);
			GUILayout.EndArea();
		}

		protected virtual void StartClient()
		{
		}

		protected virtual void StartServer()
		{
		}

		private void StressTest_1()
		{
			StressTest(_test_1);
		}

		private void StressTest_2()
		{
			StressTest(_test_2);
		}

		private void StressTest_3()
		{
			StressTest(_test_3);
		}

		protected virtual void StressTest(StressTestEssential stressTest)
		{
		}

		private void LateUpdate()
		{
			if (_timerUpdateLatencyText.IsExpiredOrNotRunning())
			{
				UpdateNetworkStats();
				_timerUpdateLatencyText = StinkySteak.SimulationTimer.SimulationTimer.CreateFromSeconds(_updateLatencyTextInterval);
			}
		}

		protected virtual void UpdateNetworkStats()
		{
		}
	}
}
