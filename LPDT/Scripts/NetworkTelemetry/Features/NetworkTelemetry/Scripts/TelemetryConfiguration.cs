namespace Features.NetworkTelemetry.Scripts
{
	public class TelemetryConfiguration
	{
		public bool IsEnabled = true;

		public bool CollectPositions = true;

		public bool CollectNetworkMetrics = true;

		public bool CollectFrameTimings = true;

		public bool CollectFusionTelemetry = true;

		public bool CollectPhotonTransport = true;

		public bool CollectLogs = true;

		public bool CollectEvents = true;

		public bool CollectEnemyPositions = true;

		public bool CollectGrabObjectPositions = true;

		public bool CollectNetworkObjectBandwidth = true;

		public bool CaptureLogMessages = true;

		public bool CaptureWarnings;

		public bool CaptureErrors = true;

		public bool CaptureExceptions = true;

		public float FlushIntervalSeconds = 5f;

		public float PositionSampleRateHz = 3f;

		public float EnemyPositionSampleRateHz = 2f;

		public float GrabObjectPositionSampleRateHz = 2f;

		public float NetworkObjectBandwidthSubmissionRateHz = 0.2f;

		public int BufferPoolInitialSize = 2;

		public string ApiEndpoint = "http://46.225.56.96:8080/ingest";

		public float HttpTimeoutSeconds = 10f;
	}
}
