using Newtonsoft.Json;

namespace Edgegap.Editor.Api.Models.Requests
{
	public class UpdateAppVersionRequest
	{
		public class SessionConfigData
		{
			[JsonProperty("kind")]
			public string Kind { get; set; } = "Seat";

			[JsonProperty("sockets")]
			public int Sockets { get; set; } = 10;

			[JsonProperty("autodeploy")]
			public bool Autodeploy { get; set; } = true;

			[JsonProperty("empty_ttl")]
			public int EmptyTtl { get; set; } = 60;

			[JsonProperty("session_max_duration")]
			public int SessionMaxDuration { get; set; } = 60;
		}

		public class ProbeData
		{
			[JsonProperty("optimal_ping")]
			public int OptimalPing { get; set; } = 60;

			[JsonProperty("rejected_ping")]
			public int RejectedPing { get; set; } = 180;
		}

		public class EnvsData
		{
			[JsonProperty("key")]
			public string Key { get; set; }

			[JsonProperty("value")]
			public string Value { get; set; }

			[JsonProperty("is_secret")]
			public bool IsSecret { get; set; } = true;
		}

		[JsonIgnore]
		public string AppName { get; set; }

		[JsonIgnore]
		public string VersionName { get; set; } = "latest";

		[JsonProperty("ports")]
		public AppPortsData[] Ports { get; set; } = new AppPortsData[0];

		[JsonProperty("docker_repository")]
		public string DockerRepository { get; set; } = "";

		[JsonProperty("docker_image")]
		public string DockerImage { get; set; } = "";

		[JsonProperty("docker_tag")]
		public string DockerTag { get; set; } = "latest";

		[JsonProperty("is_active")]
		public bool IsActive { get; set; } = true;

		[JsonProperty("private_username")]
		public string PrivateUsername { get; set; } = "";

		[JsonProperty("private_token")]
		public string PrivateToken { get; set; } = "";

		[JsonProperty("max_duration")]
		public int MaxDuration { get; set; } = 60;

		[JsonProperty("use_telemetry")]
		public bool UseTelemetry { get; set; } = true;

		[JsonProperty("inject_context_env")]
		public bool InjectContextEnv { get; set; } = true;

		[JsonProperty("whitelisting_active")]
		public bool WhitelistingActive { get; set; }

		[JsonProperty("force_cache")]
		public bool ForceCache { get; set; }

		[JsonProperty("cache_min_hour")]
		public int CacheMinHour { get; set; }

		[JsonProperty("cache_max_hour")]
		public int CacheMaxHour { get; set; }

		[JsonProperty("time_to_deploy")]
		public int TimeToDeploy { get; set; } = 120;

		[JsonProperty("enable_all_locations")]
		public bool EnableAllLocations { get; set; }

		[JsonProperty("termination_grace_period_seconds")]
		public int TerminationGracePeriodSeconds { get; set; } = 5;

		[JsonProperty("command")]
		public string Command { get; set; }

		[JsonProperty("arguments")]
		public string Arguments { get; set; }

		[JsonProperty("probe")]
		public ProbeData Probe { get; set; } = new ProbeData();

		[JsonProperty("envs")]
		public EnvsData[] Envs { get; set; } = new EnvsData[0];

		public UpdateAppVersionRequest()
		{
		}

		public UpdateAppVersionRequest(string appName)
		{
			AppName = appName;
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
