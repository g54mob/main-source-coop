using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Edgegap.Editor.Api.Models.Requests
{
	public class CreateAppVersionRequest
	{
		[JsonIgnore]
		public string AppName { get; set; }

		[JsonProperty("name")]
		public string VersionName { get; set; } = "latest";

		[JsonProperty("docker_tag")]
		public string DockerTag { get; set; } = "latest";

		[JsonProperty("docker_image")]
		public string DockerImage { get; set; } = "";

		[JsonProperty("docker_repository")]
		public string DockerRepository { get; set; } = "";

		[JsonProperty("req_cpu")]
		public int ReqCpu { get; set; } = 256;

		[JsonProperty("req_memory")]
		public int ReqMemory { get; set; } = 256;

		[JsonProperty("ports")]
		public AppPortsData[] Ports { get; set; } = new AppPortsData[0];

		[JsonProperty("private_username")]
		public string PrivateUsername { get; set; } = "";

		[JsonProperty("private_token")]
		public string PrivateToken { get; set; } = "";

		public CreateAppVersionRequest()
		{
		}

		public CreateAppVersionRequest(string appName, string containerRegistryUsername, string containerRegistryPasswordToken, int portNum, ProtocolType protocolType)
		{
			AppName = appName;
			PrivateUsername = containerRegistryUsername;
			PrivateToken = containerRegistryPasswordToken;
			Ports = new AppPortsData[1]
			{
				new AppPortsData
				{
					Port = portNum,
					ProtocolStr = protocolType.ToString()
				}
			};
		}

		public static CreateAppVersionRequest FromUpdateRequest(UpdateAppVersionRequest updateRequest)
		{
			string value = JsonConvert.SerializeObject(updateRequest);
			CreateAppVersionRequest createAppVersionRequest = null;
			try
			{
				createAppVersionRequest = JsonConvert.DeserializeObject<CreateAppVersionRequest>(value);
				createAppVersionRequest.AppName = updateRequest.AppName;
				createAppVersionRequest.VersionName = updateRequest.VersionName;
				createAppVersionRequest.PrivateUsername = updateRequest.PrivateUsername;
				createAppVersionRequest.PrivateToken = updateRequest.PrivateToken;
				return createAppVersionRequest;
			}
			catch (Exception arg)
			{
				Debug.LogError($"Error (when parsing CreateAppVersionRequest from CreateAppVersionRequest): {arg}");
				throw;
			}
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
