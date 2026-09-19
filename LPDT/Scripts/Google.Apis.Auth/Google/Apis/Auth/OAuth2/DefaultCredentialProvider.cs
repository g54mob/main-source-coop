using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Json;
using Google.Apis.Logging;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2
{
	internal class DefaultCredentialProvider
	{
		private static readonly ILogger Logger = ApplicationContext.Logger.ForType<DefaultCredentialProvider>();

		public const string CredentialEnvironmentVariable = "GOOGLE_APPLICATION_CREDENTIALS";

		private const string WellKnownCredentialsFile = "application_default_credentials.json";

		private const string AppdataEnvironmentVariable = "APPDATA";

		private const string HomeEnvironmentVariable = "HOME";

		private const string CloudSDKConfigDirectoryWindows = "gcloud";

		private const string HelpPermalink = "https://developers.google.com/accounts/docs/application-default-credentials";

		private static readonly string CloudSDKConfigDirectoryUnix = Path.Combine(".config", "gcloud");

		private readonly Lazy<Task<GoogleCredential>> cachedCredentialTask;

		public DefaultCredentialProvider()
		{
			cachedCredentialTask = new Lazy<Task<GoogleCredential>>(CreateDefaultCredentialAsync);
		}

		public Task<GoogleCredential> GetDefaultCredentialAsync()
		{
			return cachedCredentialTask.Value;
		}

		private async Task<GoogleCredential> CreateDefaultCredentialAsync()
		{
			string credentialPath = GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
			if (!string.IsNullOrWhiteSpace(credentialPath))
			{
				try
				{
					return await CreateDefaultCredentialFromFileAsync(credentialPath, default(CancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException(string.Format("Error reading credential file from location {0}: {1}\nPlease check the value of the Environment Variable {2}", credentialPath, ex.Message, "GOOGLE_APPLICATION_CREDENTIALS"), ex);
				}
			}
			credentialPath = GetWellKnownCredentialFilePath();
			if (!string.IsNullOrWhiteSpace(credentialPath))
			{
				try
				{
					return await CreateDefaultCredentialFromFileAsync(credentialPath, default(CancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (FileNotFoundException)
				{
					Logger.Debug("Well-known credential file {0} not found.", credentialPath);
				}
				catch (DirectoryNotFoundException)
				{
					Logger.Debug("Well-known credential file {0} not found.", credentialPath);
				}
				catch (Exception ex4)
				{
					throw new InvalidOperationException($"Error reading credential file from location {credentialPath}: {ex4.Message}\nPlease rerun 'gcloud auth login' to regenerate credentials file.", ex4);
				}
			}
			Logger.Debug("Checking whether the application is running on ComputeEngine.");
			if (await ComputeCredential.IsRunningOnComputeEngine().ConfigureAwait(continueOnCapturedContext: false))
			{
				Logger.Debug("ComputeEngine check passed. Using ComputeEngine Credentials.");
				return new GoogleCredential(new ComputeCredential());
			}
			throw new InvalidOperationException(string.Format("The Application Default Credentials are not available. They are available if running in Google Compute Engine. Otherwise, the environment variable {0} must be defined pointing to a file defining the credentials. See {1} for more information.", "GOOGLE_APPLICATION_CREDENTIALS", "https://developers.google.com/accounts/docs/application-default-credentials"));
		}

		private async Task<GoogleCredential> CreateDefaultCredentialFromFileAsync(string credentialPath, CancellationToken cancellationToken)
		{
			Logger.Debug("Loading Credential from file {0}", credentialPath);
			using Stream stream = GetStream(credentialPath);
			return await CreateDefaultCredentialFromStreamAsync(stream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		internal GoogleCredential CreateDefaultCredentialFromStream(Stream stream)
		{
			JsonCredentialParameters credentialParameters;
			try
			{
				credentialParameters = NewtonsoftJsonSerializer.Instance.Deserialize<JsonCredentialParameters>(stream);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException("Error deserializing JSON credential data.", innerException);
			}
			return CreateDefaultCredentialFromParameters(credentialParameters);
		}

		internal async Task<GoogleCredential> CreateDefaultCredentialFromStreamAsync(Stream stream, CancellationToken cancellationToken)
		{
			JsonCredentialParameters credentialParameters;
			try
			{
				credentialParameters = await NewtonsoftJsonSerializer.Instance.DeserializeAsync<JsonCredentialParameters>(stream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException("Error deserializing JSON credential data.", innerException);
			}
			return CreateDefaultCredentialFromParameters(credentialParameters);
		}

		internal GoogleCredential CreateDefaultCredentialFromJson(string json)
		{
			JsonCredentialParameters credentialParameters;
			try
			{
				credentialParameters = NewtonsoftJsonSerializer.Instance.Deserialize<JsonCredentialParameters>(json);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException("Error deserializing JSON credential data.", innerException);
			}
			return CreateDefaultCredentialFromParameters(credentialParameters);
		}

		internal GoogleCredential CreateDefaultCredentialFromParameters(JsonCredentialParameters credentialParameters)
		{
			return credentialParameters.ThrowIfNull("credentialParameters").Type switch
			{
				"authorized_user" => new GoogleCredential(CreateUserCredentialFromParameters(credentialParameters)), 
				"service_account" => GoogleCredential.FromServiceAccountCredential(CreateServiceAccountCredentialFromParameters(credentialParameters)), 
				_ => throw new InvalidOperationException("Error creating credential from JSON or JSON parameters. Unrecognized credential type " + credentialParameters.Type + "."), 
			};
		}

		private static UserCredential CreateUserCredentialFromParameters(JsonCredentialParameters credentialParameters)
		{
			if (credentialParameters.Type != "authorized_user" || string.IsNullOrEmpty(credentialParameters.ClientId) || string.IsNullOrEmpty(credentialParameters.ClientSecret))
			{
				throw new InvalidOperationException("JSON data does not represent a valid user credential.");
			}
			TokenResponse token = new TokenResponse
			{
				RefreshToken = credentialParameters.RefreshToken
			};
			return new UserCredential(new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
			{
				ClientSecrets = new ClientSecrets
				{
					ClientId = credentialParameters.ClientId,
					ClientSecret = credentialParameters.ClientSecret
				},
				ProjectId = credentialParameters.ProjectId
			}), "ApplicationDefaultCredentials", token, credentialParameters.QuotaProject);
		}

		private static ServiceAccountCredential CreateServiceAccountCredentialFromParameters(JsonCredentialParameters credentialParameters)
		{
			if (credentialParameters.Type != "service_account" || string.IsNullOrEmpty(credentialParameters.ClientEmail) || string.IsNullOrEmpty(credentialParameters.PrivateKey))
			{
				throw new InvalidOperationException("JSON data does not represent a valid service account credential.");
			}
			return new ServiceAccountCredential(new ServiceAccountCredential.Initializer(credentialParameters.ClientEmail)
			{
				ProjectId = credentialParameters.ProjectId,
				QuotaProject = credentialParameters.QuotaProject,
				KeyId = credentialParameters.PrivateKeyId
			}.FromPrivateKey(credentialParameters.PrivateKey));
		}

		private string GetWellKnownCredentialFilePath()
		{
			string environmentVariable = GetEnvironmentVariable("APPDATA");
			if (environmentVariable != null)
			{
				return Path.Combine(environmentVariable, "gcloud", "application_default_credentials.json");
			}
			string environmentVariable2 = GetEnvironmentVariable("HOME");
			if (environmentVariable2 != null)
			{
				return Path.Combine(environmentVariable2, CloudSDKConfigDirectoryUnix, "application_default_credentials.json");
			}
			return Path.Combine("gcloud", "application_default_credentials.json");
		}

		protected virtual string GetEnvironmentVariable(string variableName)
		{
			return Environment.GetEnvironmentVariable(variableName);
		}

		protected virtual Stream GetStream(string filePath)
		{
			return new FileStream(filePath, FileMode.Open, FileAccess.Read);
		}
	}
}
