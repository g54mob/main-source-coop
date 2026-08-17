using Newtonsoft.Json;

namespace Edgegap.Editor.Api.Models
{
	public class AppPortsData
	{
		[JsonProperty("port")]
		public int Port { get; set; } = 7770;

		[JsonProperty("protocol")]
		public string ProtocolStr { get; set; } = ProtocolType.UDP.ToString();

		[JsonProperty("to_check")]
		public bool ToCheck { get; set; } = true;

		[JsonProperty("tls_upgrade")]
		public bool TlsUpgrade { get; set; }

		[JsonProperty("name")]
		public string PortName { get; set; } = "Game Port";
	}
}
