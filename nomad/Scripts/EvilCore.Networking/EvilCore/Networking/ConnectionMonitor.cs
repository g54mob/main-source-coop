using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

namespace EvilCore.Networking
{
	public class ConnectionMonitor : MonoBehaviour
	{
		[Header("Monitoring Settings")]
		public bool enableMonitoring = true;

		public float monitorInterval = 2f;

		public int maxPingHistory = 30;

		[Header("Debug Display")]
		public bool showOnGUI = true;

		public bool logToConsole;

		private List<float> pingHistory = new List<float>();

		private float lastPing;

		private float avgPing;

		private float minPing = 3.4028235E+38f;

		private float maxPing;

		private int connectionTypeChanges;

		private string lastConnectionInfo = "";

		private float gameStartTime;

		private float _cachedStability;

		private void Start()
		{
			gameStartTime = Time.time;
			if (enableMonitoring)
			{
				InvokeRepeating("MonitorConnection", 1f, monitorInterval);
			}
		}

		private void MonitorConnection()
		{
			if (!NetworkClient.isConnected && !NetworkServer.active)
			{
				return;
			}
			float currentPing = GetCurrentPing();
			if (currentPing > 0f)
			{
				pingHistory.Add(currentPing);
				if (pingHistory.Count > maxPingHistory)
				{
					pingHistory.RemoveAt(0);
				}
				lastPing = currentPing;
				avgPing = pingHistory.Average();
				minPing = Mathf.Min(minPing, currentPing);
				maxPing = Mathf.Max(maxPing, currentPing);
				DetectConnectionTypeChange();
				_cachedStability = CalculatePingStability();
				if (logToConsole)
				{
					LogConnectionStats();
				}
			}
		}

		private float GetCurrentPing()
		{
			if (NetworkClient.isConnected)
			{
				return (float)(NetworkTime.rtt * 1000.0);
			}
			if (NetworkServer.active)
			{
				foreach (NetworkConnectionToClient value in NetworkServer.connections.Values)
				{
					if (value != null)
					{
						return (float)(value.rtt * 1000.0);
					}
				}
			}
			return -1f;
		}

		private void DetectConnectionTypeChange()
		{
			string connectionInfo = GetConnectionInfo();
			if (!string.IsNullOrEmpty(lastConnectionInfo) && lastConnectionInfo != connectionInfo)
			{
				connectionTypeChanges++;
			}
			lastConnectionInfo = connectionInfo;
		}

		private string GetConnectionInfo()
		{
			if (avgPing < 60f)
			{
				return "Likely Direct P2P";
			}
			if (avgPing > 80f)
			{
				return "Likely Steam Relay";
			}
			return "Unknown/Mixed";
		}

		private void LogConnectionStats()
		{
			_ = Time.time;
			_ = gameStartTime;
		}

		private float CalculatePingStability()
		{
			if (pingHistory.Count < 2)
			{
				return 0f;
			}
			float num = 0f;
			foreach (float item in pingHistory)
			{
				num += Mathf.Pow(item - avgPing, 2f);
			}
			return Mathf.Sqrt(num / (float)pingHistory.Count);
		}

		public bool IsConnectionStable()
		{
			if (connectionTypeChanges == 0)
			{
				return CalculatePingStability() < 20f;
			}
			return false;
		}

		public string GetConnectionReport()
		{
			return $"Game Duration: {Time.time - gameStartTime:F0}s\n" + $"Average Ping: {avgPing:F0}ms\n" + $"Connection Changes: {connectionTypeChanges}\n" + "Stability: " + (IsConnectionStable() ? "STABLE" : "UNSTABLE");
		}
	}
}
