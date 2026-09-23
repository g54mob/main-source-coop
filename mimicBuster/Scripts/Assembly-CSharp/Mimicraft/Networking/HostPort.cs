using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	public static class HostPort
	{
		public const int SearchSpan = 64;

		private const int MaxWaitFrames = 120;

		public static async Task ReleaseLocalServers()
		{
			NetworkManager[] managers = UnityEngine.Object.FindObjectsByType<NetworkManager>(FindObjectsSortMode.None);
			bool flag = false;
			NetworkManager[] array = managers;
			foreach (NetworkManager networkManager in array)
			{
				if (!(networkManager == null) && (networkManager.IsListening || networkManager.ShutdownInProgress))
				{
					flag = true;
					if (networkManager.IsListening)
					{
						networkManager.Shutdown();
					}
				}
			}
			if (!flag)
			{
				return;
			}
			for (int frame = 0; frame < 120; frame++)
			{
				if (!StillRunning(managers))
				{
					break;
				}
				await Task.Yield();
			}
		}

		private static bool StillRunning(NetworkManager[] managers)
		{
			foreach (NetworkManager networkManager in managers)
			{
				if (networkManager != null && (networkManager.IsListening || networkManager.ShutdownInProgress))
				{
					return true;
				}
			}
			return false;
		}

		public static ushort FindFree(ushort preferred)
		{
			if (IsFree(preferred))
			{
				return preferred;
			}
			for (int i = preferred + 1; i <= preferred + 64 && i <= 65535; i++)
			{
				if (IsFree((ushort)i))
				{
					return (ushort)i;
				}
			}
			return 0;
		}

		public static bool IsFree(ushort port)
		{
			if (port == 0)
			{
				return false;
			}
			if (!ListedAsBusy(port))
			{
				return CanBind(port);
			}
			return false;
		}

		private static bool CanBind(ushort port)
		{
			try
			{
				using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				socket.Bind(new IPEndPoint(IPAddress.Any, port));
				return true;
			}
			catch (SocketException)
			{
				return false;
			}
			catch (Exception)
			{
				return true;
			}
		}

		private static bool ListedAsBusy(ushort port)
		{
			try
			{
				IPEndPoint[] activeUdpListeners = IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners();
				for (int i = 0; i < activeUdpListeners.Length; i++)
				{
					if (activeUdpListeners[i].Port == port)
					{
						return true;
					}
				}
			}
			catch (Exception)
			{
			}
			return false;
		}
	}
}
