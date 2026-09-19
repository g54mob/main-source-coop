using System;
using System.Threading;
using System.Threading.Tasks;

namespace Photon.Realtime
{
	public static class MatchmakingExtensions
	{
		public static Task<RealtimeClient> ConnectToRoomAsync(this RealtimeClient client, MatchmakingArguments arguments)
		{
			return ConnectToRoomAsync(arguments, client);
		}

		public static Task<RealtimeClient> ConnectToRoomAsync(MatchmakingArguments arguments, RealtimeClient client = null)
		{
			arguments.Validate();
			_ = arguments.PhotonSettings;
			AsyncConfig asyncConfig = arguments.AsyncConfig ?? AsyncConfig.Global;
			return asyncConfig.TaskFactory.StartNew(async delegate
			{
				client = client ?? arguments.NetworkClient ?? new RealtimeClient();
				bool isRandom = string.IsNullOrEmpty(arguments.RoomName);
				bool canCreate = !arguments.CanOnlyJoin;
				if (!client.IsConnected)
				{
					if (arguments.AuthValues != null)
					{
						client.AuthValues = arguments.AuthValues.CopyTo(new AuthenticationValues());
					}
					client.RealtimePeer.CrcEnabled = arguments.EnableCrc;
				}
				await client.ConnectUsingSettingsAsync(arguments.PhotonSettings, asyncConfig);
				short num = (isRandom ? ((!canCreate) ? (await client.JoinRandomRoomAsync(arguments.BuildJoinRandomRoomArgs(), throwOnError: true, asyncConfig)) : (await client.JoinRandomOrCreateRoomAsync(arguments.BuildJoinRandomRoomArgs(), arguments.BuildEnterRoomArgs(), throwOnError: true, asyncConfig))) : ((!canCreate) ? (await client.JoinRoomAsync(arguments.BuildEnterRoomArgs(), throwOnError: true, asyncConfig)) : (await client.JoinOrCreateRoomAsync(arguments.BuildEnterRoomArgs(), throwOnError: true, asyncConfig))));
				if (num == 0)
				{
					if (arguments.ReconnectInformation != null)
					{
						arguments.ReconnectInformation.Set(client);
					}
					return client;
				}
				throw new OperationException(num, $"Failed to connect and join with error '{num}'");
			}).Unwrap();
		}

		public static Task<RealtimeClient> ReconnectToRoomAsync(this RealtimeClient client, MatchmakingArguments arguments)
		{
			return ReconnectToRoomAsync(arguments, client);
		}

		public static Task<RealtimeClient> ReconnectToRoomAsync(MatchmakingArguments arguments, RealtimeClient client = null)
		{
			arguments.Validate();
			if (arguments.ReconnectInformation == null)
			{
				throw new ArgumentException("ReconnectInformation missing");
			}
			if (arguments.ReconnectInformation.AppVersion != arguments.PhotonSettings.AppVersion)
			{
				throw new ArgumentException("AppVersion mismatch");
			}
			if (string.IsNullOrEmpty(arguments.ReconnectInformation.UserId))
			{
				throw new ArgumentException("UserId not set");
			}
			if (arguments.AuthValues != null && arguments.ReconnectInformation.UserId != arguments.AuthValues.UserId)
			{
				throw new ArgumentException("UserId mismatch");
			}
			_ = arguments.ReconnectInformation.HasTimedOut;
			AsyncConfig asyncConfig = arguments.AsyncConfig ?? AsyncConfig.Global;
			CancellationToken cancelToken = asyncConfig.CancellationToken;
			return asyncConfig.TaskFactory.StartNew(async delegate
			{
				object obj = client ?? arguments.NetworkClient;
				if (obj == null)
				{
					obj = new RealtimeClient();
				}
				client = (RealtimeClient)obj;
				short? result = null;
				if (arguments.CanRejoin && !arguments.FastReconnectDisabled)
				{
					try
					{
						result = await client.ReconnectAndRejoinAsync(arguments.Ticket, throwOnError: true, asyncConfig);
						if (result.HasValue && result.Value == 0)
						{
							arguments.ReconnectInformation?.Set(client);
							return client;
						}
					}
					catch (Exception)
					{
					}
				}
				if (client.IsConnected && client.State != ClientState.ConnectedToMasterServer && client.State != ClientState.JoinedLobby)
				{
					await client.DisconnectAsync(asyncConfig);
				}
				if (!client.IsConnected)
				{
					if (arguments.AuthValues != null)
					{
						client.AuthValues = arguments.AuthValues.CopyTo(new AuthenticationValues());
					}
					client.RealtimePeer.CrcEnabled = arguments.EnableCrc;
					arguments.PhotonSettings.FixedRegion = arguments.ReconnectInformation.Region;
					await client.ConnectUsingSettingsAsync(arguments.PhotonSettings, asyncConfig);
				}
				bool canRejoin = arguments.CanRejoin;
				int joinIterations = 0;
				while (joinIterations++ < 10)
				{
					if (result.HasValue)
					{
						if (result.Value == 0)
						{
							break;
						}
						if (result.Value == 32746)
						{
							await Task.Delay(1000, cancelToken);
						}
						else
						{
							if (result.Value != 32748)
							{
								throw new OperationException(result.Value, $"Failed to join the room with error '{result}'");
							}
							canRejoin = false;
						}
					}
					_ = joinIterations;
					_ = 1;
					if (cancelToken.IsCancellationRequested)
					{
						throw new TaskCanceledException();
					}
					result = ((!canRejoin) ? new short?(await client.JoinRoomAsync(arguments.BuildEnterRoomArgs().SetRoomName(arguments.ReconnectInformation.Room), throwOnError: false, asyncConfig)) : new short?(await client.RejoinRoomAsync(arguments.ReconnectInformation.Room, arguments.Ticket, throwOnError: false, asyncConfig)));
				}
				if (result.HasValue && result.Value == 0 && arguments.ReconnectInformation != null)
				{
					arguments.ReconnectInformation.Set(client);
				}
				return client;
			}).Unwrap();
		}

		private static EnterRoomArgs SetRoomName(this EnterRoomArgs args, string roomName)
		{
			args.RoomName = roomName;
			return args;
		}

		private static EnterRoomArgs BuildEnterRoomArgs(this MatchmakingArguments arguments)
		{
			EnterRoomArgs obj = new EnterRoomArgs
			{
				RoomName = arguments.RoomName,
				Lobby = arguments.Lobby,
				Ticket = arguments.Ticket,
				ExpectedUsers = arguments.ExpectedUsers
			};
			object obj2 = arguments.CustomRoomOptions;
			if (obj2 == null)
			{
				obj2 = new RoomOptions
				{
					MaxPlayers = (byte)arguments.MaxPlayers,
					IsOpen = (!arguments.IsRoomOpen.HasValue || arguments.IsRoomOpen.Value),
					IsVisible = (!arguments.IsRoomVisible.HasValue || arguments.IsRoomVisible.Value),
					DeleteNullProperties = true,
					PlayerTtl = arguments.PlayerTtlInSeconds * 1000,
					EmptyRoomTtl = arguments.EmptyRoomTtlInSeconds * 1000,
					Plugins = arguments.Plugins,
					SuppressRoomEvents = false,
					SuppressPlayerInfo = false,
					PublishUserId = true,
					CustomRoomProperties = arguments.CustomProperties
				};
				object obj3 = obj2;
				object[] customLobbyProperties = arguments.CustomLobbyProperties;
				((RoomOptions)obj3).CustomRoomPropertiesForLobby = customLobbyProperties;
			}
			obj.RoomOptions = (RoomOptions)obj2;
			return obj;
		}

		private static JoinRandomRoomArgs BuildJoinRandomRoomArgs(this MatchmakingArguments arguments)
		{
			return new JoinRandomRoomArgs
			{
				Ticket = arguments.Ticket,
				ExpectedUsers = arguments.ExpectedUsers,
				ExpectedCustomRoomProperties = arguments.CustomProperties,
				SqlLobbyFilter = arguments.SqlLobbyFilter,
				ExpectedMaxPlayers = arguments.MaxPlayers,
				Lobby = arguments.Lobby,
				MatchingType = arguments.RandomMatchingType
			};
		}
	}
}
