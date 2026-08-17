using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mirror.Examples.LagCompensationDemo
{
	public class ServerCube : MonoBehaviour
	{
		[Header("Components")]
		public ClientCube client;

		[FormerlySerializedAs("collider")]
		public BoxCollider col;

		[Header("Movement")]
		public float distance = 10f;

		public float speed = 3f;

		private Vector3 start;

		[Header("Snapshot Interpolation")]
		[Tooltip("Send N snapshots per second. Multiples of frame rate make sense.")]
		public int sendRate = 30;

		private float lastSendTime;

		[Header("Lag Compensation")]
		public LagCompensationSettings lagCompensationSettings = new LagCompensationSettings();

		private double lastCaptureTime;

		private Queue<KeyValuePair<double, Capture2D>> history = new Queue<KeyValuePair<double, Capture2D>>();

		public Color historyColor = Color.white;

		[Header("Debug")]
		public double resultDuration = 0.5;

		private double resultTime;

		private Capture2D resultBefore;

		private Capture2D resultAfter;

		private Capture2D resultInterpolated;

		[Header("Latency Simulation")]
		[Tooltip("Latency in seconds")]
		public float latency = 0.05f;

		[Tooltip("Latency jitter, randomly added to latency.")]
		[Range(0f, 1f)]
		public float jitter = 0.05f;

		[Tooltip("Packet loss in %")]
		[Range(0f, 1f)]
		public float loss = 0.1f;

		[Tooltip("Scramble % of unreliable messages, just like over the real network. Mirror unreliable is unordered.")]
		[Range(0f, 1f)]
		public float scramble = 0.1f;

		private System.Random random = new System.Random();

		private List<(double, Snapshot3D)> queue = new List<(double, Snapshot3D)>();

		public float sendInterval => 1f / (float)sendRate;

		private float SimulateLatency()
		{
			return latency + UnityEngine.Random.value * jitter;
		}

		private float AverageLatency()
		{
			return latency + 0.5f * jitter;
		}

		private void Start()
		{
			start = base.transform.position;
		}

		private void Update()
		{
			float num = Mathf.PingPong(Time.time * speed, distance);
			base.transform.position = new Vector3(start.x + num, start.y, start.z);
			if (Time.time >= lastSendTime + sendInterval)
			{
				Send(base.transform.position);
				lastSendTime = Time.time;
			}
			Flush();
			if (NetworkTime.localTime >= lastCaptureTime + (double)lagCompensationSettings.captureInterval)
			{
				lastCaptureTime = NetworkTime.localTime;
				Capture();
			}
		}

		private void Send(Vector3 position)
		{
			Snapshot3D item = new Snapshot3D(NetworkTime.localTime, 0.0, position);
			if (!(random.NextDouble() < (double)loss))
			{
				bool num = random.NextDouble() < (double)scramble;
				int count = queue.Count;
				int index = (num ? random.Next(0, count + 1) : count);
				float num2 = SimulateLatency();
				double item2 = NetworkTime.localTime + (double)num2;
				queue.Insert(index, (item2, item));
			}
		}

		private void Flush()
		{
			for (int i = 0; i < queue.Count; i++)
			{
				var (num, snap) = queue[i];
				if (NetworkTime.localTime >= num)
				{
					client.OnMessage(snap);
					queue.RemoveAt(i);
					i--;
				}
			}
		}

		private void Capture()
		{
			LagCompensation.Insert(capture: new Capture2D(NetworkTime.localTime, base.transform.position, col.size), history: history, historyLimit: lagCompensationSettings.historyLimit, timestamp: NetworkTime.localTime);
		}

		public bool CmdClicked(Vector2 position)
		{
			double rtt = AverageLatency() * 2f;
			double num = LagCompensation.EstimateTime(NetworkTime.localTime, rtt, client.bufferTime);
			double num2 = Math.Abs(num - client.localTimeline);
			Debug.Log($"CmdClicked: serverTime={NetworkTime.localTime:F3} clientTime={client.localTimeline:F3} estimatedTime={num:F3} estimationError={num2:F3} position={position}");
			if (LagCompensation.Sample(history, num, lagCompensationSettings.captureInterval, out resultBefore, out resultAfter, out var t))
			{
				resultInterpolated = Capture2D.Interpolate(resultBefore, resultAfter, t);
				resultTime = NetworkTime.localTime;
				if (new Bounds(resultInterpolated.position, resultInterpolated.size).Contains(position))
				{
					return true;
				}
				Debug.Log($"CmdClicked: interpolated={resultInterpolated} doesn't contain {position}");
			}
			else
			{
				Debug.Log($"CmdClicked: history doesn't contain {num:F3}");
			}
			return false;
		}

		private void OnDrawGizmos()
		{
			bool num = NetworkTime.localTime <= resultTime + resultDuration;
			if (num)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(resultInterpolated.position, resultInterpolated.size);
			}
			Gizmos.color = historyColor;
			LagCompensation.DrawGizmos(history);
			if (num)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireCube(resultBefore.position, resultBefore.size);
				Gizmos.DrawWireCube(resultAfter.position, resultAfter.size);
			}
		}
	}
}
