using System;
using System.IO;
using System.Text;
using HeathenEngineering.SteamworksIntegration.API;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
	[Serializable]
	public struct SteamGameServerConfiguration
	{
		public bool autoInitialize;

		public bool autoLogon;

		public uint ip;

		public ushort gamePort;

		public ushort queryPort;

		public ushort spectatorPort;

		public string serverVersion;

		public bool usingGameServerAuthApi;

		public bool enableHeartbeats;

		public bool supportSpectators;

		public string spectatorServerName;

		public bool anonymousServerLogin;

		public string gameServerToken;

		public bool isPasswordProtected;

		public string serverName;

		public string gameDescription;

		public string gameDirectory;

		public bool isDedicated;

		public int maxPlayerCount;

		public int botPlayerCount;

		public string mapName;

		public string gameData;

		public StringKeyValuePair[] rulePairs;

		public static SteamGameServerConfiguration Default => new SteamGameServerConfiguration
		{
			autoInitialize = true,
			autoLogon = true,
			ip = 0u,
			gamePort = 27015,
			queryPort = 27016,
			spectatorPort = 27017,
			serverVersion = "1.0.0.0",
			usingGameServerAuthApi = false,
			enableHeartbeats = true,
			supportSpectators = false,
			spectatorServerName = string.Empty,
			anonymousServerLogin = true,
			gameServerToken = string.Empty,
			isPasswordProtected = false,
			serverName = $"Must Not Be Empty | Must be Less than {64}",
			gameDescription = $"Must Not Be Empty | Must be Less than {64}",
			gameDirectory = $"Must Not Be Empty | Must be Less than {32}",
			isDedicated = false,
			maxPlayerCount = 4,
			botPlayerCount = 0,
			mapName = string.Empty,
			gameData = string.Empty,
			rulePairs = null
		};

		public string IpAddress
		{
			get
			{
				return Utilities.IPUintToString(ip);
			}
			set
			{
				ip = Utilities.IPStringToUint(value);
			}
		}

		public readonly bool DebugValidate()
		{
			bool result = true;
			if (string.IsNullOrEmpty(gameServerToken) && !anonymousServerLogin)
			{
				Debug.LogError("Non-anonymous login requires a game server token, no token was found.");
				result = false;
			}
			if (string.IsNullOrEmpty(serverName))
			{
				Debug.LogError("Server Name must be populated.");
				result = false;
			}
			if (serverName.Length > 64)
			{
				Debug.LogError($"Server Name {64} char or less.");
				result = false;
			}
			if (string.IsNullOrEmpty(spectatorServerName) && supportSpectators)
			{
				Debug.LogError("If Support Spectators is true then you must provide a Spectator Server Name.");
				result = false;
			}
			if ((spectatorPort == 0 || spectatorPort == ushort.MaxValue) && supportSpectators)
			{
				Debug.LogError("If Support Spectators is true then you must provide a valid Spectator Port value.");
				result = false;
			}
			if (supportSpectators && spectatorServerName.Length > 32)
			{
				Debug.LogError($"The Spectators Server Name must be {32} char or less.");
				result = false;
			}
			if (string.IsNullOrEmpty(gameDescription))
			{
				Debug.LogError("You must provide a Game Description.");
				result = false;
			}
			if (gameDescription.Length > 64)
			{
				Debug.LogError($"Game Description must be {64} char or less.");
				result = false;
			}
			if (string.IsNullOrEmpty(gameDirectory))
			{
				Debug.LogError("You must provide a Game Directory.");
				result = false;
			}
			if (gameDirectory.Length > 32)
			{
				Debug.LogError($"Game Directory must be {32} char or less.");
				result = false;
			}
			if (gamePort == 0 || gamePort == ushort.MaxValue)
			{
				Debug.LogError("You must provide a valid Game Port... default is 27015");
				result = false;
			}
			if (queryPort == 0 || queryPort == ushort.MaxValue)
			{
				Debug.LogError("You must provide a valid Query Port... default is 27016");
				result = false;
			}
			if (string.IsNullOrEmpty(serverVersion))
			{
				Debug.LogError("You must provide a Server Version string, the suggested form is major.minor.build.revision");
				result = false;
			}
			return result;
		}

		public static SteamGameServerConfiguration Get()
		{
			return App.Server.Configuration;
		}

		public static bool Get(FileInfo configFile, out SteamGameServerConfiguration config)
		{
			try
			{
				if (configFile.Exists)
				{
					config = JsonUtility.FromJson<SteamGameServerConfiguration>(File.ReadAllText(configFile.FullName));
					return true;
				}
				config = Default;
				return false;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				config = Default;
				return false;
			}
		}

		public static bool Get(string configFile, out SteamGameServerConfiguration config)
		{
			try
			{
				if (File.Exists(configFile))
				{
					config = JsonUtility.FromJson<SteamGameServerConfiguration>(File.ReadAllText(configFile));
					return true;
				}
				config = Default;
				return false;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				config = Default;
				return false;
			}
		}

		public static bool Get(byte[] serializedData, out SteamGameServerConfiguration config)
		{
			try
			{
				if (serializedData != null && serializedData.Length != 0)
				{
					config = JsonUtility.FromJson<SteamGameServerConfiguration>(Encoding.UTF8.GetString(serializedData));
					return true;
				}
				config = Default;
				return false;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				config = Default;
				return false;
			}
		}

		public override string ToString()
		{
			return JsonUtility.ToJson(this);
		}

		public byte[] ToBytes()
		{
			return Encoding.UTF8.GetBytes(ToString());
		}

		public bool SaveToDisk(string path)
		{
			try
			{
				File.WriteAllText(path, ToString());
				return true;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				return false;
			}
		}

		public static bool LoadFromDisk(string path, out SteamGameServerConfiguration config)
		{
			return Get(path, out config);
		}

		public bool SaveToDiskAsIni(string path)
		{
			try
			{
				File.WriteAllText(path, ToIniString(this));
				return true;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				return false;
			}
		}

		public static bool LoadFromDiskAsIni(string path, out SteamGameServerConfiguration config)
		{
			try
			{
				if (File.Exists(path))
				{
					config = ParseIniString(File.ReadAllText(path));
					return true;
				}
				config = Default;
				return false;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				config = Default;
				return false;
			}
		}

		public static SteamGameServerConfiguration ParseIniString(string iniData)
		{
			SteamGameServerConfiguration result = Default;
			string[] array = iniData.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string text in array)
			{
				string[] array2 = text.Split('=');
				if (array2.Length == 2)
				{
					string text2 = array2[0].Trim().ToLower();
					string text3 = array2[1].Trim();
					switch (text2)
					{
					case "autoinitialize":
					{
						if (bool.TryParse(text3, out var result3))
						{
							result.autoInitialize = result3;
						}
						break;
					}
					case "autologon":
					{
						if (bool.TryParse(text3, out var result11))
						{
							result.autoLogon = result11;
						}
						break;
					}
					case "ip":
						result.IpAddress = text3;
						break;
					case "gameport":
					{
						if (ushort.TryParse(text3, out var result13))
						{
							result.gamePort = result13;
						}
						break;
					}
					case "queryport":
					{
						if (ushort.TryParse(text3, out var result9))
						{
							result.queryPort = result9;
						}
						break;
					}
					case "spectatorport":
					{
						if (ushort.TryParse(text3, out var result6))
						{
							result.spectatorPort = result6;
						}
						break;
					}
					case "serverversion":
						result.serverVersion = text3;
						break;
					case "usinggameserverauthapi":
					{
						if (bool.TryParse(text3, out var result14))
						{
							result.usingGameServerAuthApi = result14;
						}
						break;
					}
					case "enableheartbeats":
					{
						if (bool.TryParse(text3, out var result12))
						{
							result.enableHeartbeats = result12;
						}
						break;
					}
					case "supportspectators":
					{
						if (bool.TryParse(text3, out var result10))
						{
							result.supportSpectators = result10;
						}
						break;
					}
					case "spectatorservername":
						result.spectatorServerName = text3;
						break;
					case "anonymousserverlogin":
					{
						if (bool.TryParse(text3, out var result8))
						{
							result.anonymousServerLogin = result8;
						}
						break;
					}
					case "gameservertoken":
						result.gameServerToken = text3;
						break;
					case "ispasswordprotected":
					{
						if (bool.TryParse(text3, out var result7))
						{
							result.isPasswordProtected = result7;
						}
						break;
					}
					case "servername":
						result.serverName = text3;
						break;
					case "gamedescription":
						result.gameDescription = text3;
						break;
					case "gamedirectory":
						result.gameDirectory = text3;
						break;
					case "isdedicated":
					{
						if (bool.TryParse(text3, out var result5))
						{
							result.isDedicated = result5;
						}
						break;
					}
					case "maxplayercount":
					{
						if (int.TryParse(text3, out var result4))
						{
							result.maxPlayerCount = result4;
						}
						break;
					}
					case "botplayercount":
					{
						if (int.TryParse(text3, out var result2))
						{
							result.botPlayerCount = result2;
						}
						break;
					}
					case "mapname":
						result.mapName = text3;
						break;
					case "gamedata":
						result.gameData = text3;
						break;
					default:
						Debug.LogWarning("Unknown key '" + text2 + "' in INI data.");
						break;
					}
				}
				else
				{
					Debug.LogWarning("Malformed line: '" + text + "' in INI data.");
				}
			}
			return result;
		}

		public static string ToIniString(SteamGameServerConfiguration config)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"autoInitialize = {config.autoInitialize}");
			stringBuilder.AppendLine($"autoLogon = {config.autoLogon}");
			stringBuilder.AppendLine("ip = " + config.IpAddress);
			stringBuilder.AppendLine($"gamePort = {config.gamePort}");
			stringBuilder.AppendLine($"queryPort = {config.queryPort}");
			stringBuilder.AppendLine($"spectatorPort = {config.spectatorPort}");
			stringBuilder.AppendLine("serverVersion = " + config.serverVersion);
			stringBuilder.AppendLine($"usingGameServerAuthApi = {config.usingGameServerAuthApi}");
			stringBuilder.AppendLine($"enableHeartbeats = {config.enableHeartbeats}");
			stringBuilder.AppendLine($"supportSpectators = {config.supportSpectators}");
			stringBuilder.AppendLine("spectatorServerName = " + config.spectatorServerName);
			stringBuilder.AppendLine($"anonymousServerLogin = {config.anonymousServerLogin}");
			stringBuilder.AppendLine("gameServerToken = " + config.gameServerToken);
			stringBuilder.AppendLine($"isPasswordProtected = {config.isPasswordProtected}");
			stringBuilder.AppendLine("serverName = " + config.serverName);
			stringBuilder.AppendLine("gameDescription = " + config.gameDescription);
			stringBuilder.AppendLine("gameDirectory = " + config.gameDirectory);
			stringBuilder.AppendLine($"isDedicated = {config.isDedicated}");
			stringBuilder.AppendLine($"maxPlayerCount = {config.maxPlayerCount}");
			stringBuilder.AppendLine($"botPlayerCount = {config.botPlayerCount}");
			stringBuilder.AppendLine("mapName = " + config.mapName);
			stringBuilder.AppendLine("gameData = " + config.gameData);
			return stringBuilder.ToString();
		}
	}
}
