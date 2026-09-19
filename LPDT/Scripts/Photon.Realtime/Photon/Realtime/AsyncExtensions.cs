using System;
using System.Threading;
using System.Threading.Tasks;

namespace Photon.Realtime
{
	public static class AsyncExtensions
	{
		internal static AsyncConfig Resolve(this AsyncConfig config)
		{
			return config ?? AsyncConfig.Global;
		}

		public static Task ConnectUsingSettingsAsync(this RealtimeClient client, AppSettings appSettings, AsyncConfig config = null)
		{
			if (client.IsConnectedAndReady && client.Server == ServerConnection.MasterServer)
			{
				return Task.CompletedTask;
			}
			return config.Resolve().TaskFactory.StartNew(delegate
			{
				if (client.State != ClientState.Disconnected && client.State != ClientState.PeerCreated)
				{
					return Task.FromException(new OperationStartException("Client still connected"));
				}
				if (!client.ConnectUsingSettings(appSettings))
				{
					return Task.FromException(new OperationStartException("Failed to start connecting"));
				}
				AsyncOperationHandler handler = client.CreateConnectionHandler(throwOnErrors: true, config.Resolve());
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnDisconnectedMsg m)
				{
					handler.SetException(new DisconnectException(m.cause));
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnCustomAuthenticationFailedMsg m)
				{
					handler.SetException(new AuthenticationFailedException(m.debugMessage));
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual<OnConnectedToMasterMsg>(delegate
				{
					handler.SetResult(0);
				}));
				return handler.Task;
			}).Unwrap();
		}

		public static Task<short> ReconnectAndRejoinAsync(this RealtimeClient client, object ticket = null, bool throwOnError = true, AsyncConfig config = null)
		{
			return config.Resolve().TaskFactory.StartNew(delegate
			{
				if (client.State != ClientState.Disconnected && client.State != ClientState.PeerCreated)
				{
					return Task.FromException<short>(new OperationStartException("Client still connected"));
				}
				if (!client.ReconnectAndRejoin(ticket))
				{
					return Task.FromException<short>(new OperationStartException("Failed to start reconnecting"));
				}
				AsyncOperationHandler handler = client.CreateConnectionHandler(throwOnError, config.Resolve());
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnDisconnectedMsg m)
				{
					handler.SetException(new DisconnectException(m.cause));
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual<OnJoinedRoomMsg>(delegate
				{
					handler.SetResult(0);
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnJoinRoomFailedMsg m)
				{
					if (throwOnError)
					{
						handler.SetException(new OperationException(m.returnCode, m.message));
					}
					else
					{
						handler.SetResult(m.returnCode);
					}
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnJoinRandomFailedMsg m)
				{
					if (throwOnError)
					{
						handler.SetException(new OperationException(m.returnCode, m.message));
					}
					else
					{
						handler.SetResult(m.returnCode);
					}
				}));
				return handler.Task;
			}).Unwrap();
		}

		[Obsolete("Use ReconnectAndRejoinAsync(this RealtimeClient client, object ticket, bool throwOnError, AsyncConfig config) instead")]
		public static Task<short> ReconnectAndRejoinAsync(this RealtimeClient client, bool throwOnError = true, AsyncConfig config = null)
		{
			return client.ReconnectAndRejoinAsync(null, throwOnError, config);
		}

		public static Task ReconnectToMasterAsync(this RealtimeClient client, AsyncConfig config = null)
		{
			return config.Resolve().TaskFactory.StartNew(delegate
			{
				if (client.State != ClientState.Disconnected && client.State != ClientState.PeerCreated)
				{
					return Task.FromException(new OperationStartException("Client still connected"));
				}
				if (!client.ReconnectToMaster())
				{
					return Task.FromException(new OperationStartException("Failed to start reconnecting"));
				}
				AsyncOperationHandler handler = client.CreateConnectionHandler(throwOnErrors: true, config.Resolve());
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnDisconnectedMsg m)
				{
					handler.SetException(new DisconnectException(m.cause));
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual<OnConnectedToMasterMsg>(delegate
				{
					handler.SetResult(0);
				}));
				return handler.Task;
			}).Unwrap();
		}

		public static Task DisconnectAsync(this RealtimeClient client, AsyncConfig config = null)
		{
			return config.Resolve().TaskFactory.StartNew(delegate
			{
				if (client == null)
				{
					return Task.CompletedTask;
				}
				if (client.State == ClientState.Disconnected || client.State == ClientState.PeerCreated)
				{
					return Task.CompletedTask;
				}
				AsyncOperationHandler handler = client.CreateConnectionHandler(throwOnErrors: true, config.Resolve());
				_ = client.LogLevel;
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual<OnDisconnectedMsg>(delegate
				{
					handler.SetResult(0);
				}));
				if (client.State != ClientState.Disconnecting)
				{
					client.Disconnect();
				}
				return handler.Task;
			}).Unwrap();
		}

		public static Task<RegionHandler> ConnectToNameserverAndWaitForRegionsAsync(this RealtimeClient client, AppSettings appSettings, bool pingRegions = true, AsyncConfig config = null)
		{
			AsyncConfig asyncConfig = config.Resolve();
			return asyncConfig.TaskFactory.StartNew(delegate
			{
				if (client.State != ClientState.ConnectedToNameServer && client.State != ClientState.ConnectingToNameServer)
				{
					if (client.State != ClientState.Disconnected && client.State != ClientState.PeerCreated)
					{
						return Task.FromException<RegionHandler>(new OperationStartException($"Client state ({client.State}) unuseable for name server connection."));
					}
					AppSettings appSettings2 = new AppSettings(appSettings)
					{
						FixedRegion = null
					};
					client.GetRegions(appSettings2, pingRegions);
				}
				if (client.RegionHandler?.EnabledRegions != null)
				{
					RegionHandler regionHandler = client.RegionHandler;
					if (regionHandler == null || regionHandler.EnabledRegions.Count > 0)
					{
						return Task.FromResult(client.RegionHandler);
					}
				}
				AsyncOperationHandler handler = client.CreateConnectionHandler(throwOnErrors: true, config.Resolve());
				client.CallbackMessage.ListenManual(delegate(OnRegionListReceivedMsg m)
				{
					if (pingRegions)
					{
						m.regionHandler.PingAvailableRegions(delegate
						{
							handler.SetResult(0);
						});
					}
					else
					{
						handler.SetResult(0);
					}
				});
				Task<RegionHandler> task = handler.Task.ContinueWith((Task<short> c) => client.RegionHandler, asyncConfig.TaskScheduler);
				task.ContinueWith((Task<RegionHandler> c) => client.DisconnectAsync(), asyncConfig.TaskScheduler);
				return task;
			}).Unwrap();
		}

		public static Task<short> CreateAndJoinRoomAsync(this RealtimeClient client, EnterRoomArgs enterRoomArgs, bool throwOnError = true, AsyncConfig config = null)
		{
			return config.Resolve().TaskFactory.StartNew(delegate
			{
				if (!client.OpCreateRoom(enterRoomArgs))
				{
					return Task.FromException<short>(new OperationStartException("Failed to send CreateRoom operation"));
				}
				AsyncOperationHandler handler = client.CreateConnectionHandler(throwOnError, config.Resolve());
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnDisconnectedMsg m)
				{
					handler.SetException(new DisconnectException(m.cause));
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual<OnJoinedRoomMsg>(delegate
				{
					handler.SetResult(0);
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnCreateRoomFailedMsg m)
				{
					if (throwOnError)
					{
						handler.SetException(new OperationException(m.returnCode, m.message));
					}
					else
					{
						handler.SetResult(m.returnCode);
					}
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnJoinRoomFailedMsg m)
				{
					if (throwOnError)
					{
						handler.SetException(new OperationException(m.returnCode, m.message));
					}
					else
					{
						handler.SetResult(m.returnCode);
					}
				}));
				return handler.Task;
			}).Unwrap();
		}

		public static Task<short> JoinRoomAsync(this RealtimeClient client, EnterRoomArgs enterRoomArgs, bool throwOnError = true, AsyncConfig config = null)
		{
			return config.Resolve().TaskFactory.StartNew(delegate
			{
				if (!client.OpJoinRoom(enterRoomArgs))
				{
					return Task.FromException<short>(new OperationStartException("Failed to send JoinRoom operation"));
				}
				AsyncOperationHandler handler = client.CreateConnectionHandler(throwOnError, config.Resolve());
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnDisconnectedMsg m)
				{
					handler.SetException(new DisconnectException(m.cause));
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual<OnJoinedRoomMsg>(delegate
				{
					handler.SetResult(0);
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnJoinRoomFailedMsg m)
				{
					if (throwOnError)
					{
						handler.SetException(new OperationException(m.returnCode, m.message));
					}
					else
					{
						handler.SetResult(m.returnCode);
					}
				}));
				return handler.Task;
			}).Unwrap();
		}

		public static Task<short> RejoinRoomAsync(this RealtimeClient client, string roomName, object ticket = null, bool throwOnError = true, AsyncConfig config = null)
		{
			return config.Resolve().TaskFactory.StartNew(delegate
			{
				if (client.State != ClientState.ConnectedToMasterServer)
				{
					return Task.FromException<short>(new OperationStartException("Must be connected to master server"));
				}
				if (!client.OpRejoinRoom(roomName, ticket))
				{
					return Task.FromException<short>(new OperationStartException("Failed to send RejoinRoom operation"));
				}
				AsyncOperationHandler handler = client.CreateConnectionHandler(throwOnError, config.Resolve());
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnDisconnectedMsg m)
				{
					handler.SetException(new DisconnectException(m.cause));
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual<OnJoinedRoomMsg>(delegate
				{
					handler.SetResult(0);
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnJoinRoomFailedMsg m)
				{
					if (throwOnError)
					{
						handler.SetException(new OperationException(m.returnCode, m.message));
					}
					else
					{
						handler.SetResult(m.returnCode);
					}
				}));
				return handler.Task;
			}).Unwrap();
		}

		public static Task<short> JoinOrCreateRoomAsync(this RealtimeClient client, EnterRoomArgs enterRoomArgs, bool throwOnError = true, AsyncConfig config = null)
		{
			return config.Resolve().TaskFactory.StartNew(delegate
			{
				if (!client.OpJoinOrCreateRoom(enterRoomArgs))
				{
					return Task.FromException<short>(new OperationStartException("Failed to send JoinRoom operation"));
				}
				AsyncOperationHandler handler = client.CreateConnectionHandler(throwOnError, config.Resolve());
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnDisconnectedMsg m)
				{
					handler.SetException(new DisconnectException(m.cause));
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual<OnJoinedRoomMsg>(delegate
				{
					handler.SetResult(0);
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnCreateRoomFailedMsg m)
				{
					if (throwOnError)
					{
						handler.SetException(new OperationException(m.returnCode, m.message));
					}
					else
					{
						handler.SetResult(m.returnCode);
					}
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnJoinRoomFailedMsg m)
				{
					if (throwOnError)
					{
						handler.SetException(new OperationException(m.returnCode, m.message));
					}
					else
					{
						handler.SetResult(m.returnCode);
					}
				}));
				return handler.Task;
			}).Unwrap();
		}

		public static Task<short> JoinRandomOrCreateRoomAsync(this RealtimeClient client, JoinRandomRoomArgs joinRandomRoomParams = null, EnterRoomArgs enterRoomArgs = null, bool throwOnError = true, AsyncConfig config = null)
		{
			return config.Resolve().TaskFactory.StartNew(delegate
			{
				if (!client.OpJoinRandomOrCreateRoom(joinRandomRoomParams, enterRoomArgs))
				{
					return Task.FromException<short>(new OperationStartException("Failed to send JoinRandomOrCreateRoom operation"));
				}
				AsyncOperationHandler handler = client.CreateConnectionHandler(throwOnError, config.Resolve());
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnDisconnectedMsg m)
				{
					handler.SetException(new DisconnectException(m.cause));
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual<OnJoinedRoomMsg>(delegate
				{
					handler.SetResult(0);
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnCreateRoomFailedMsg m)
				{
					if (throwOnError)
					{
						handler.SetException(new OperationException(m.returnCode, m.message));
					}
					else
					{
						handler.SetResult(m.returnCode);
					}
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnJoinRandomFailedMsg m)
				{
					if (throwOnError)
					{
						handler.SetException(new OperationException(m.returnCode, m.message));
					}
					else
					{
						handler.SetResult(m.returnCode);
					}
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnJoinRoomFailedMsg m)
				{
					if (throwOnError)
					{
						handler.SetException(new OperationException(m.returnCode, m.message));
					}
					else
					{
						handler.SetResult(m.returnCode);
					}
				}));
				return handler.Task;
			}).Unwrap();
		}

		public static Task<short> JoinRandomRoomAsync(this RealtimeClient client, JoinRandomRoomArgs joinRandomRoomParams = null, bool throwOnError = true, AsyncConfig config = null)
		{
			return config.Resolve().TaskFactory.StartNew(delegate
			{
				if (!client.OpJoinRandomRoom(joinRandomRoomParams))
				{
					return Task.FromException<short>(new OperationStartException("Failed to send JoinRandomRoom operation"));
				}
				AsyncOperationHandler handler = client.CreateConnectionHandler(throwOnError, config.Resolve());
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnDisconnectedMsg m)
				{
					handler.SetException(new DisconnectException(m.cause));
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual<OnJoinedRoomMsg>(delegate
				{
					handler.SetResult(0);
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnJoinRandomFailedMsg m)
				{
					if (throwOnError)
					{
						handler.SetException(new OperationException(m.returnCode, m.message));
					}
					else
					{
						handler.SetResult(m.returnCode);
					}
				}));
				return handler.Task;
			}).Unwrap();
		}

		public static Task LeaveRoomAsync(this RealtimeClient client, bool becomeInactive = false, bool throwOnError = true, AsyncConfig config = null)
		{
			return config.Resolve().TaskFactory.StartNew(delegate
			{
				if (client.State != ClientState.Joined)
				{
					return Task.FromException(new OperationStartException("Must be inside a room"));
				}
				if (!client.OpLeaveRoom(becomeInactive))
				{
					return Task.FromException(new OperationStartException("Failed to send LeaveRoom operation"));
				}
				AsyncOperationHandler handler = client.CreateConnectionHandler(throwOnError, config.Resolve());
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnDisconnectedMsg m)
				{
					handler.SetException(new DisconnectException(m.cause));
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual<OnConnectedToMasterMsg>(delegate
				{
					handler.SetResult(0);
				}));
				return handler.Task;
			}).Unwrap();
		}

		public static Task<short> JoinLobbyAsync(this RealtimeClient client, TypedLobby lobby = null, bool throwOnError = true, AsyncConfig config = null)
		{
			return config.Resolve().TaskFactory.StartNew(delegate
			{
				if (!client.OpJoinLobby(lobby))
				{
					return Task.FromException<short>(new OperationStartException("Failed to send JoinLobby operation"));
				}
				AsyncOperationHandler handler = client.CreateConnectionHandler(throwOnError, config.Resolve());
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnDisconnectedMsg m)
				{
					handler.SetException(new DisconnectException(m.cause));
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual<OnJoinedLobbyMsg>(delegate
				{
					handler.SetResult(0);
				}));
				return handler.Task;
			}).Unwrap();
		}

		public static Task<short> LeaveLobbyAsync(this RealtimeClient client, bool throwOnError = true, AsyncConfig config = null)
		{
			if (client.State == ClientState.ConnectedToMasterServer)
			{
				return Task.FromResult((short)0);
			}
			if (client.State != ClientState.JoinedLobby)
			{
				return Task.FromException<short>(new OperationStartException("Must be inside a lobby"));
			}
			return config.Resolve().TaskFactory.StartNew(delegate
			{
				if (!client.OpLeaveLobby())
				{
					return Task.FromException<short>(new OperationStartException("Failed to send LeaveLobby operation"));
				}
				AsyncOperationHandler handler = client.CreateConnectionHandler(throwOnError, config.Resolve());
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual(delegate(OnDisconnectedMsg m)
				{
					handler.SetException(new DisconnectException(m.cause));
				}));
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual<OnLeftLobbyMsg>(delegate
				{
					handler.SetResult(0);
				}));
				return handler.Task;
			}).Unwrap();
		}

		public static AsyncOperationHandler CreateConnectionHandler(this RealtimeClient client, bool throwOnErrors = true, AsyncConfig config = null)
		{
			AsyncOperationHandler asyncOperationHandler = new AsyncOperationHandler(config.Resolve().OperationTimeoutSec);
			client.CreateServiceTask(asyncOperationHandler.Token, asyncOperationHandler.CompletionSource, asyncOperationHandler, config.Resolve());
			return asyncOperationHandler;
		}

		public static void CreateServiceTask(this RealtimeClient client, CancellationToken token, TaskCompletionSource<short> completionSource = null, IDisposable disposable = null, AsyncConfig config = null)
		{
			_ = DateTime.Now;
			config.Resolve().TaskFactory.StartNew((Func<Task>)async delegate
			{
				CancellationTokenSource linkedCancellationSource = null;
				CancellationToken combinedToken = token;
				if (config.Resolve().CancellationToken != CancellationToken.None)
				{
					try
					{
						linkedCancellationSource = CancellationTokenSource.CreateLinkedTokenSource(token, config.Resolve().CancellationToken);
						combinedToken = linkedCancellationSource.Token;
					}
					catch (Exception exception)
					{
						completionSource?.TrySetException(exception);
					}
				}
				if (!config.Resolve().CreateServiceTask)
				{
					await (completionSource?.Task ?? Task.CompletedTask);
				}
				else
				{
					while (!combinedToken.IsCancellationRequested)
					{
						try
						{
							client.Service();
							await Task.Delay(config.Resolve().ServiceIntervalMs, combinedToken);
						}
						catch (OperationCanceledException)
						{
							break;
						}
						catch (Exception exception2)
						{
							completionSource?.TrySetException(exception2);
							break;
						}
					}
				}
				if (!token.IsCancellationRequested && completionSource != null)
				{
					switch (completionSource.Task.Status)
					{
					default:
						completionSource.TrySetException(new OperationCanceledException("Operation canceled"));
						break;
					case TaskStatus.RanToCompletion:
					case TaskStatus.Canceled:
					case TaskStatus.Faulted:
						break;
					}
				}
				disposable?.Dispose();
				linkedCancellationSource?.Dispose();
			}, TaskCreationOptions.LongRunning);
		}

		public static Task WaitForDisconnect(this RealtimeClient client, AsyncConfig config = null)
		{
			return config.Resolve().TaskFactory.StartNew(delegate
			{
				if (client == null)
				{
					return Task.CompletedTask;
				}
				if (client.State == ClientState.Disconnected || client.State == ClientState.PeerCreated)
				{
					return Task.CompletedTask;
				}
				AsyncOperationHandler handler = new AsyncOperationHandler();
				handler.Disposables.Enqueue(client.CallbackMessage.ListenManual<OnDisconnectedMsg>(delegate
				{
					handler.SetResult(0);
				}));
				return handler.Task;
			}).Unwrap();
		}
	}
}
