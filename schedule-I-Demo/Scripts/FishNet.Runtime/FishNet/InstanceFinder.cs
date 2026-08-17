using System;
using System.Linq;
using FishNet.Component.ColliderRollback;
using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Managing.Predicting;
using FishNet.Managing.Scened;
using FishNet.Managing.Server;
using FishNet.Managing.Statistic;
using FishNet.Managing.Timing;
using FishNet.Managing.Transporting;
using GameKit.Utilities;
using UnityEngine;

namespace FishNet
{
	public static class InstanceFinder
	{
		private static NetworkManager _networkManager;

		public static NetworkManager NetworkManager
		{
			get
			{
				if (_networkManager == null)
				{
					int count = NetworkManager.Instances.Count;
					if (count > 0)
					{
						_networkManager = NetworkManager.Instances.First();
						if (count > 1)
						{
							_networkManager.LogWarning("Multiple NetworkManagers found, the first result will be returned. If you only wish to have one NetworkManager then uncheck 'Allow Multiple' within your NetworkManagers.");
						}
					}
					else
					{
						if (ApplicationState.IsQuitting())
						{
							return null;
						}
						Debug.Log("NetworkManager not found in any open scenes.");
					}
				}
				return _networkManager;
			}
		}

		public static ServerManager ServerManager
		{
			get
			{
				NetworkManager networkManager = NetworkManager;
				if (!(networkManager == null))
				{
					return networkManager.ServerManager;
				}
				return null;
			}
		}

		public static ClientManager ClientManager
		{
			get
			{
				NetworkManager networkManager = NetworkManager;
				if (!(networkManager == null))
				{
					return networkManager.ClientManager;
				}
				return null;
			}
		}

		public static TransportManager TransportManager
		{
			get
			{
				NetworkManager networkManager = NetworkManager;
				if (!(networkManager == null))
				{
					return networkManager.TransportManager;
				}
				return null;
			}
		}

		public static TimeManager TimeManager
		{
			get
			{
				NetworkManager networkManager = NetworkManager;
				if (!(networkManager == null))
				{
					return networkManager.TimeManager;
				}
				return null;
			}
		}

		public static SceneManager SceneManager
		{
			get
			{
				NetworkManager networkManager = NetworkManager;
				if (!(networkManager == null))
				{
					return networkManager.SceneManager;
				}
				return null;
			}
		}

		public static RollbackManager RollbackManager
		{
			get
			{
				NetworkManager networkManager = NetworkManager;
				if (!(networkManager == null))
				{
					return networkManager.RollbackManager;
				}
				return null;
			}
		}

		public static PredictionManager PredictionManager
		{
			get
			{
				NetworkManager networkManager = NetworkManager;
				if (!(networkManager == null))
				{
					return networkManager.PredictionManager;
				}
				return null;
			}
		}

		public static StatisticsManager StatisticsManager
		{
			get
			{
				NetworkManager networkManager = NetworkManager;
				if (!(networkManager == null))
				{
					return networkManager.StatisticsManager;
				}
				return null;
			}
		}

		public static bool IsServer
		{
			get
			{
				if (!(NetworkManager == null))
				{
					return NetworkManager.IsServer;
				}
				return false;
			}
		}

		public static bool IsServerOnly
		{
			get
			{
				if (!(NetworkManager == null))
				{
					return NetworkManager.IsServerOnly;
				}
				return false;
			}
		}

		public static bool IsClient
		{
			get
			{
				if (!(NetworkManager == null))
				{
					return NetworkManager.IsClient;
				}
				return false;
			}
		}

		public static bool IsClientOnly
		{
			get
			{
				if (!(NetworkManager == null))
				{
					return NetworkManager.IsClientOnly;
				}
				return false;
			}
		}

		public static bool IsHost
		{
			get
			{
				if (!(NetworkManager == null))
				{
					return NetworkManager.IsHost;
				}
				return false;
			}
		}

		public static bool IsOffline
		{
			get
			{
				if (!(_networkManager == null))
				{
					if (!NetworkManager.IsServer)
					{
						return !NetworkManager.IsClient;
					}
					return false;
				}
				return true;
			}
		}

		public static void RegisterInvokeOnInstance<T>(Action<UnityEngine.Component> handler) where T : UnityEngine.Component
		{
			NetworkManager?.RegisterInvokeOnInstance<T>(handler);
		}

		public static void UnregisterInvokeOnInstance<T>(Action<UnityEngine.Component> handler) where T : UnityEngine.Component
		{
			NetworkManager?.UnregisterInvokeOnInstance<T>(handler);
		}

		public static T GetInstance<T>() where T : UnityEngine.Component
		{
			NetworkManager networkManager = NetworkManager;
			if ((object)networkManager == null)
			{
				return null;
			}
			return networkManager.GetInstance<T>();
		}

		public static bool HasInstance<T>() where T : UnityEngine.Component
		{
			if (!(NetworkManager == null))
			{
				return NetworkManager.HasInstance<T>();
			}
			return false;
		}

		public static void RegisterInstance<T>(T component, bool replace = true) where T : UnityEngine.Component
		{
			NetworkManager?.RegisterInstance(component, replace);
		}

		public static bool TryRegisterInstance<T>(T component) where T : UnityEngine.Component
		{
			if (!(NetworkManager == null))
			{
				return NetworkManager.TryRegisterInstance(component);
			}
			return false;
		}

		public static bool TryGetInstance<T>(out T component) where T : UnityEngine.Component
		{
			return NetworkManager.TryGetInstance<T>(out component);
		}

		public static void UnregisterInstance<T>() where T : UnityEngine.Component
		{
			NetworkManager?.UnregisterInstance<T>();
		}
	}
}
