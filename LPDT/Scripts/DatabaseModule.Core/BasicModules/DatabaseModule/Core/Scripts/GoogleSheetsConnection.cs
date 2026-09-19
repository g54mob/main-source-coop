using System;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using UnityEngine;

namespace BasicModules.DatabaseModule.Core.Scripts
{
	public class GoogleSheetsConnection : IGoogleSheetsConnection
	{
		private const string JSON_DATA_DOES_NOT_REPRESENT_SERVICE_ACCOUNT_EXCEPTION = "JSON data does not represent a valid service account credential.";

		private const string JSON_FILE_DOESNT_EXIST_EXCEPTION = "JSON file doesn't exist or empty.";

		private const string TOKEN_JSON = "token.json";

		private const string USER = "user";

		private const string REQUEST_FIELDS = "name, version";

		private readonly string[] _scopes = new string[1] { DriveService.Scope.DriveMetadataReadonly };

		public SheetsService ConnectToGoogleSheetsService(IDataBaseSettings dataBaseSettings)
		{
			string text = System.IO.File.ReadAllText(dataBaseSettings.GetJsonFullPath());
			if (text == string.Empty)
			{
				throw new InvalidOperationException("JSON file doesn't exist or empty.");
			}
			ServiceAccountCredential serviceAccountCredentialFromJson = GetServiceAccountCredentialFromJson(text);
			return new SheetsService(new BaseClientService.Initializer
			{
				HttpClientInitializer = serviceAccountCredentialFromJson
			});
		}

		public string GetSheetCurrentVersion(IDataBaseSettings dataBaseSettings)
		{
			UserCredential result = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.FromFile(dataBaseSettings.GetJsonFullPath()).Secrets, _scopes, "user", CancellationToken.None, new FileDataStore("token.json", fullPath: true)).Result;
			Debug.LogError("Credential file saved to: token.json");
			FilesResource.GetRequest getRequest = new DriveService(new BaseClientService.Initializer
			{
				HttpClientInitializer = result,
				ApplicationName = Application.productName
			}).Files.Get(dataBaseSettings.GetConnectionString());
			getRequest.Fields = "name, version";
			Google.Apis.Drive.v3.Data.File file = getRequest.Execute();
			return $"{file.Name} {file.Version}";
		}

		private ServiceAccountCredential GetServiceAccountCredentialFromJson(string json)
		{
			if (GoogleCredential.FromJson(json).UnderlyingCredential is ServiceAccountCredential result)
			{
				return result;
			}
			throw new InvalidOperationException("JSON data does not represent a valid service account credential.");
		}
	}
}
