using MessagePack;

namespace Features.NetworkTelemetry.Scripts
{
	[MessagePackObject(false)]
	public struct FrameTimingSample
	{
		[Key(0)]
		public int SessionTimeMs { get; set; }

		[Key(1)]
		public int PingMs { get; set; }

		[Key(2)]
		public float Fps { get; set; }

		[Key(3)]
		public double GpuFrameTimeMs { get; set; }

		[Key(4)]
		public double CpuMainThreadFrameTimeMs { get; set; }

		[Key(5)]
		public double CpuRenderThreadFrameTimeMs { get; set; }

		[Key(6)]
		public double CpuMainThreadPresentWaitTimeMs { get; set; }

		[Key(7)]
		public float ProcessCpuUsagePct { get; set; }

		[Key(8)]
		public long ProcessRamUsageBytes { get; set; }
	}
}
