#define TRACE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Fusion.Matchmaking.Extensions;
using Fusion.Photon.Realtime;
using Photon.Client;
using Photon.Client.Encryption;
using Photon.Realtime;

namespace Fusion.Matchmaking
{
	public static class RealtimeClientExtensions
	{
		public const string FusionPluginName = "FusionPlugin21";

		private const string ServerHostCn = "ns.photonengine.cn";

		private const string RegionCnID = "cn";

		private static readonly SendOptions OptionsUnreliable = new SendOptions
		{
			Channel = 0,
			DeliveryMode = DeliveryMode.UnreliableUnsequenced
		};

		private static readonly SendOptions OptionsReliable = new SendOptions
		{
			Channel = 1,
			DeliveryMode = DeliveryMode.Reliable
		};

		internal static bool IsEncryptionEnabled(this RealtimeClient client)
		{
			return client.EncryptionMode != EncryptionMode.PayloadEncryption && client.RealtimePeer.EncryptorType != null;
		}

		internal static AppSettings SetupAppSettings(this AppSettings appSettings)
		{
			AppSettings appSettings2 = new AppSettings(appSettings);
			if (appSettings2.FixedRegion.ToLower().Equals("cn") && string.IsNullOrEmpty(appSettings2.Server?.Trim()))
			{
				appSettings2.Server = "ns.photonengine.cn";
			}
			return appSettings2;
		}

		public static RealtimeClient SetupForFusion(this RealtimeClient client, FusionAppSettings appSettings = null)
		{
			if (client == null)
			{
				return null;
			}
			if (appSettings == null)
			{
				appSettings = PhotonAppSettings.Global.AppSettings;
			}
			client.ClientType = ClientAppType.Fusion;
			client.RealtimePeer.PingInterval = 200;
			client.RealtimePeer.UseByteArraySlicePoolForEvents = true;
			client.RealtimePeer.ReuseEventInstance = true;
			client.RealtimePeer.QuickResendAttempts = 8;
			client.RealtimePeer.MaxResends *= 10;
			client.RealtimePeer.DisconnectTimeout = 15000;
			client.RealtimePeer.SendWindowSize /= 3;
			if (appSettings != null && appSettings.encryptionMode == EncryptionMode.DatagramEncryptionGCM)
			{
				client.EncryptionMode = appSettings.encryptionMode;
				PhotonPeer realtimePeer = client.RealtimePeer;
				if ((object)realtimePeer.EncryptorType == null)
				{
					Type type = (realtimePeer.EncryptorType = LoadPhotonEncryptorType());
				}
			}
			return client;
		}

		internal unsafe static bool SendEvent(this RealtimeClient client, int target, byte eventCode, byte* buffer, int bufferLength, bool reliable)
		{
			if (!client.IsConnectedAndReady || !client.InRoom)
			{
				return false;
			}
			ByteArraySlice byteArraySlice = client.RealtimePeer.ByteArraySlicePool.Acquire(bufferLength);
			if (buffer != null)
			{
				fixed (byte* buffer2 = byteArraySlice.Buffer)
				{
					FusionUnsafe.Copy(buffer2, buffer, bufferLength);
				}
			}
			byteArraySlice.Count = bufferLength;
			bool result = client.OpRaiseEvent(eventCode, byteArraySlice, new RaiseEventArgs
			{
				TargetActors = new int[1] { target }
			}, reliable ? OptionsReliable : OptionsUnreliable);
			if (client.RealtimePeer.SendOutgoingCommands())
			{
				client.RealtimePeer.SendOutgoingCommands();
			}
			return result;
		}

		internal static (EnterRoomArgs enterRoomArgs, JoinRandomRoomArgs joinRoomArgs) BuildRoomArgs(this RealtimeClient client, TypedLobby typedLobby, string roomName, int maxPlayers, Dictionary<string, SessionProperty> customProperties, bool isOpen, bool isVisible, int emptyRoomTtl, bool extendedTtl, MatchmakingMode matchmakingMode)
		{
			BuildSessionCustomPropertyHolders(customProperties, out var sessionCustomProperties, out var publicSessionProperties);
			EnterRoomArgs enterRoomArgs = new EnterRoomArgs();
			enterRoomArgs.RoomName = roomName;
			enterRoomArgs.Lobby = typedLobby;
			enterRoomArgs.RoomOptions = new RoomOptions
			{
				MaxPlayers = maxPlayers,
				IsOpen = isOpen,
				IsVisible = isVisible,
				DeleteNullProperties = true,
				PlayerTtl = (extendedTtl ? 15000 : 0),
				EmptyRoomTtl = emptyRoomTtl,
				Plugins = new string[1] { "FusionPlugin21" },
				SuppressRoomEvents = false,
				SuppressPlayerInfo = false,
				PublishUserId = true,
				CustomRoomProperties = sessionCustomProperties,
				CustomRoomPropertiesForLobby = publicSessionProperties
			};
			EnterRoomArgs item = enterRoomArgs;
			JoinRandomRoomArgs item2 = new JoinRandomRoomArgs
			{
				MatchingType = matchmakingMode,
				Lobby = typedLobby,
				ExpectedCustomRoomProperties = sessionCustomProperties
			};
			return (enterRoomArgs: item, joinRoomArgs: item2);
		}

		private static void BuildSessionCustomPropertyHolders(Dictionary<string, SessionProperty> customProperties, out PhotonHashtable sessionCustomProperties, out object[] publicSessionProperties)
		{
			sessionCustomProperties = null;
			publicSessionProperties = null;
			if (customProperties != null && customProperties.Count > 0)
			{
				sessionCustomProperties = customProperties.ConvertToHashtable();
				publicSessionProperties = new List<object>(customProperties.Keys).ToArray();
			}
		}

		internal static bool UpdateRoomProperties(this RealtimeClient client, Dictionary<string, SessionProperty> customProperties)
		{
			if (customProperties == null || customProperties.Count == 0 || customProperties.Count > 10)
			{
				return false;
			}
			if (client.CurrentRoom.IsOffline)
			{
				return false;
			}
			PhotonHashtable photonHashtable = new PhotonHashtable();
			photonHashtable.Merge(client.CurrentRoom.CustomProperties);
			bool flag = false;
			foreach (string key in customProperties.Keys)
			{
				if (photonHashtable.ContainsKey(key))
				{
					if (!photonHashtable[key].Equals(customProperties[key].PropertyValue))
					{
						photonHashtable[key] = customProperties[key].PropertyValue;
						flag = true;
					}
				}
				else
				{
					InternalLogStreams.LogWarn?.Log("Invalid custom property key [" + key + "], ignore. Only existing custom properties can be updated.");
				}
			}
			int num = 0;
			foreach (DictionaryEntry item in photonHashtable)
			{
				if (item.Key is string && ++num > 10)
				{
					InternalLogStreams.LogWarn?.Log("Max number of Custom Session Properties reached, only 10 properties are allowed.");
					return false;
				}
			}
			int num2 = photonHashtable.CalculateTotalSize();
			if (num2 > 500)
			{
				InternalLogStreams.LogWarn?.Log($"Max size of Custom Session Properties reached, current size of {num2} bytes, max 500 bytes are allowed.");
				return false;
			}
			return flag && client.CurrentRoom.SetCustomProperties(photonHashtable);
		}

		internal static bool UpdateRoomIsVisible(this RealtimeClient client, bool value)
		{
			if (client.CurrentRoom.IsOffline)
			{
				return false;
			}
			if (client.CurrentRoom.IsVisible == value)
			{
				return false;
			}
			client.CurrentRoom.IsVisible = value;
			return true;
		}

		internal static bool UpdateRoomIsOpen(this RealtimeClient client, bool value)
		{
			if (client.CurrentRoom.IsOffline)
			{
				return false;
			}
			if (client.CurrentRoom.IsOpen == value)
			{
				return false;
			}
			client.CurrentRoom.IsOpen = value;
			return true;
		}

		private static Type LoadPhotonEncryptorType()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			bool flag = true;
			while (true)
			{
				Assembly[] array = assemblies;
				foreach (Assembly assembly in array)
				{
					string text = assembly.FullName.ToLower();
					if (flag && !text.Contains("assembly-csharp") && !text.Contains("fusion") && !text.Contains("photon"))
					{
						continue;
					}
					Type[] types = assembly.GetTypes();
					foreach (Type type in types)
					{
						if (!typeof(IPhotonEncryptor).IsAssignableFrom(type) || type.IsInterface || type.IsAbstract)
						{
							continue;
						}
						try
						{
							IPhotonEncryptor photonEncryptor = (IPhotonEncryptor)Activator.CreateInstance(type);
							photonEncryptor.Init(Array.Empty<byte>(), Array.Empty<byte>());
							(photonEncryptor as IDisposable)?.Dispose();
						}
						catch
						{
							continue;
						}
						InternalLogStreams.LogTraceRealtime?.Info($"Encryption IPhotonEncryptor Type: {type.FullName}/{type.Assembly}");
						return type;
					}
				}
				if (!flag)
				{
					break;
				}
				flag = false;
			}
			throw new InvalidOperationException("No implementation of IPhotonEncryptor found. Make sure to include a Photon Realtime Encryption Library in your project.");
		}
	}
}
