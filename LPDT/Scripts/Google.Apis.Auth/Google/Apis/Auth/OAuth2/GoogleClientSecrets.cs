using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Json;
using Newtonsoft.Json;

namespace Google.Apis.Auth.OAuth2
{
	public sealed class GoogleClientSecrets
	{
		[JsonProperty("installed")]
		private ClientSecrets Installed { get; set; }

		[JsonProperty("web")]
		private ClientSecrets Web { get; set; }

		public ClientSecrets Secrets
		{
			get
			{
				if (Installed == null && Web == null)
				{
					throw new InvalidOperationException("At least one client secrets (Installed or Web) should be set");
				}
				return Installed ?? Web;
			}
		}

		[Obsolete("Please use GoogleClientSecrets.FromStream which is an equivalent alternative.")]
		public static GoogleClientSecrets Load(Stream stream)
		{
			return FromStream(stream);
		}

		public static GoogleClientSecrets FromStream(Stream stream)
		{
			return NewtonsoftJsonSerializer.Instance.Deserialize<GoogleClientSecrets>(stream);
		}

		public static Task<GoogleClientSecrets> FromStreamAsync(Stream stream, CancellationToken cancellationToken = default(CancellationToken))
		{
			return NewtonsoftJsonSerializer.Instance.DeserializeAsync<GoogleClientSecrets>(stream, cancellationToken);
		}

		public static GoogleClientSecrets FromFile(string clientSecretsFilePath)
		{
			using FileStream stream = new FileStream(clientSecretsFilePath, FileMode.Open, FileAccess.Read);
			return FromStream(stream);
		}

		public static async Task<GoogleClientSecrets> FromFileAsync(string clientSecretsFilePath, CancellationToken cancellationToken = default(CancellationToken))
		{
			using FileStream stream = new FileStream(clientSecretsFilePath, FileMode.Open, FileAccess.Read);
			return await FromStreamAsync(stream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
