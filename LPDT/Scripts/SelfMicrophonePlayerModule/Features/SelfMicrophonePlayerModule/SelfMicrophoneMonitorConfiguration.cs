using UnityEngine;

namespace Features.SelfMicrophonePlayerModule
{
	[CreateAssetMenu(fileName = "SelfMicrophoneMonitorConfiguration_Default", menuName = "Configurations/AudioModule/SelfMicrophoneMonitorConfiguration")]
	public class SelfMicrophoneMonitorConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public int FallbackDriverId { get; private set; }

		[field: SerializeField]
		public string MonitorBusPath { get; private set; } = "bus:/SelfMicro";

		[field: SerializeField]
		public uint LatencyMs { get; private set; } = 50u;

		[field: SerializeField]
		public uint DriftMs { get; private set; } = 1u;
	}
}
