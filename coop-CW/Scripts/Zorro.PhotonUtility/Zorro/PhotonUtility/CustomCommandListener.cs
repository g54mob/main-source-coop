using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.Collections;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.Serizalization;

namespace Zorro.PhotonUtility
{
	public class CustomCommandListener<CommandEnum> : Singleton<CustomCommandListener<CommandEnum>>, IConnectionCallbacks, IOnEventCallback where CommandEnum : struct, IConvertible
	{
		private bool isCallback;

		private static Dictionary<byte, Type> m_registeredPackages = new Dictionary<byte, Type>();

		private static Dictionary<ListenerHandle, Action<CustomCommandPackage<CommandEnum>>> m_listeners = new Dictionary<ListenerHandle, Action<CustomCommandPackage<CommandEnum>>>();

		private static Dictionary<Type, List<ListenerHandle>> m_listenerHandlesForType = new Dictionary<Type, List<ListenerHandle>>();

		protected override void OnCreated()
		{
			base.OnCreated();
			isCallback = true;
			PhotonNetwork.AddCallbackTarget(this);
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		private void OnDestroy()
		{
			if (isCallback)
			{
				PhotonNetwork.RemoveCallbackTarget(this);
			}
		}

		public void RegisterPackage<T>(T package) where T : CustomCommandPackage<CommandEnum>
		{
			byte commandEventCode = package.GetCommandEventCode();
			if (m_registeredPackages.ContainsKey(commandEventCode))
			{
				Debug.Log("Package with event code " + commandEventCode + " already registered");
				return;
			}
			Debug.Log("Registering package with event code " + commandEventCode + " and type " + package.GetType());
			m_registeredPackages.Add(commandEventCode, package.GetType());
		}

		public void OnEvent(EventData photonEvent)
		{
			byte code = photonEvent.Code;
			if (!m_registeredPackages.ContainsKey(code))
			{
				return;
			}
			Type type = m_registeredPackages[code];
			NetworkStatistics.AddEvent_Received(type.ToString());
			if (!m_listenerHandlesForType.ContainsKey(type))
			{
				Debug.LogError("No listeners for package type " + type);
				return;
			}
			CustomCommandPackage<CommandEnum> customCommandPackage = (CustomCommandPackage<CommandEnum>)Activator.CreateInstance(type);
			BinaryDeserializer binaryDeserializer = new BinaryDeserializer(new NativeArray<byte>((byte[])photonEvent.Parameters[photonEvent.CustomDataKey], Allocator.Temp));
			customCommandPackage.DeserializeData(binaryDeserializer);
			binaryDeserializer.Dispose();
			foreach (ListenerHandle item in m_listenerHandlesForType[type])
			{
				m_listeners[item]?.Invoke(customCommandPackage);
			}
		}

		public static ListenerHandle RegisterListener<T>(Action<T> onReceived) where T : CustomCommandPackage<CommandEnum>
		{
			ListenerHandle listenerHandle = ListenerHandle.Create();
			m_listeners.Add(listenerHandle, delegate(CustomCommandPackage<CommandEnum> package)
			{
				onReceived?.Invoke((T)package);
			});
			Type typeFromHandle = typeof(T);
			if (m_listenerHandlesForType.ContainsKey(typeFromHandle))
			{
				m_listenerHandlesForType.Remove(typeFromHandle);
				Debug.Log("Listener handle for type " + typeFromHandle?.ToString() + " already exists, removing old handles");
			}
			m_listenerHandlesForType.Add(typeFromHandle, new List<ListenerHandle>());
			m_listenerHandlesForType[typeFromHandle].Add(listenerHandle);
			return listenerHandle;
		}

		public static void UnregisterListener(ListenerHandle handle)
		{
			if (handle.id == Guid.Empty)
			{
				Debug.LogError("Listener handle is invalid");
				return;
			}
			if (m_listeners.ContainsKey(handle))
			{
				m_listeners.Remove(handle);
				{
					foreach (List<ListenerHandle> value in m_listenerHandlesForType.Values)
					{
						value.Remove(handle);
					}
					return;
				}
			}
			Debug.LogError($"Listener {handle} not found");
		}

		public void OnConnected()
		{
		}

		public void OnConnectedToMaster()
		{
		}

		public void OnDisconnected(DisconnectCause cause)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public void OnRegionListReceived(RegionHandler regionHandler)
		{
		}

		public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
		{
		}

		public void OnCustomAuthenticationFailed(string debugMessage)
		{
		}
	}
}
