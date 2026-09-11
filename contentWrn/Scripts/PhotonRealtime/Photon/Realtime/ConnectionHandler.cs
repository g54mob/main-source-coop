using System;
using System.Diagnostics;
using ExitGames.Client.Photon;
using UnityEngine;

namespace Photon.Realtime
{
	public class ConnectionHandler : MonoBehaviour
	{
		public bool DisconnectAfterKeepAlive;

		public int KeepAliveInBackground = 60000;

		public bool ApplyDontDestroyOnLoad = true;

		[NonSerialized]
		public static bool AppQuits;

		[NonSerialized]
		public static bool AppPause;

		[NonSerialized]
		public static bool AppPauseRecent;

		[NonSerialized]
		public static bool AppOutOfFocus;

		[NonSerialized]
		public static bool AppOutOfFocusRecent;

		private byte fallbackThreadId = byte.MaxValue;

		private bool didSendAcks;

		private readonly Stopwatch backgroundStopwatch = new Stopwatch();

		public LoadBalancingClient Client { get; set; }

		public int CountSendAcksOnly { get; private set; }

		public bool FallbackThreadRunning => fallbackThreadId < byte.MaxValue;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void StaticReset()
		{
			AppQuits = false;
			AppPause = false;
			AppPauseRecent = false;
			AppOutOfFocus = false;
			AppOutOfFocusRecent = false;
		}

		protected virtual void Awake()
		{
			if (ApplyDontDestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		protected virtual void OnDisable()
		{
			StopFallbackSendAckThread();
			if (AppQuits)
			{
				if (Client != null && Client.IsConnected)
				{
					Client.Disconnect(DisconnectCause.ApplicationQuit);
					Client.LoadBalancingPeer.StopThread();
				}
				SupportClass.StopAllBackgroundCalls();
			}
		}

		public void OnApplicationQuit()
		{
			AppQuits = true;
		}

		public void OnApplicationPause(bool pause)
		{
			AppPause = pause;
			if (pause)
			{
				AppPauseRecent = true;
				CancelInvoke("ResetAppPauseRecent");
			}
			else
			{
				Invoke("ResetAppPauseRecent", 5f);
			}
		}

		private void ResetAppPauseRecent()
		{
			AppPauseRecent = false;
		}

		public void OnApplicationFocus(bool focus)
		{
			AppOutOfFocus = !focus;
			if (!focus)
			{
				AppOutOfFocusRecent = true;
				CancelInvoke("ResetAppOutOfFocusRecent");
			}
			else
			{
				Invoke("ResetAppOutOfFocusRecent", 5f);
			}
		}

		private void ResetAppOutOfFocusRecent()
		{
			AppOutOfFocusRecent = false;
		}

		public static bool IsNetworkReachableUnity()
		{
			return Application.internetReachability != NetworkReachability.NotReachable;
		}

		public void StartFallbackSendAckThread()
		{
			if (!FallbackThreadRunning)
			{
				fallbackThreadId = SupportClass.StartBackgroundCalls(RealtimeFallbackThread, 50, "RealtimeFallbackThread");
			}
		}

		public void StopFallbackSendAckThread()
		{
			if (FallbackThreadRunning)
			{
				SupportClass.StopBackgroundCalls(fallbackThreadId);
				fallbackThreadId = byte.MaxValue;
			}
		}

		public bool RealtimeFallbackThread()
		{
			if (Client != null)
			{
				if (!Client.IsConnected)
				{
					didSendAcks = false;
					return true;
				}
				if (Client.LoadBalancingPeer.ConnectionTime - Client.LoadBalancingPeer.LastSendOutgoingTime > 100)
				{
					if (!didSendAcks)
					{
						backgroundStopwatch.Reset();
						backgroundStopwatch.Start();
					}
					if (backgroundStopwatch.ElapsedMilliseconds > KeepAliveInBackground)
					{
						if (DisconnectAfterKeepAlive)
						{
							Client.Disconnect();
						}
						return true;
					}
					didSendAcks = true;
					CountSendAcksOnly++;
					Client.LoadBalancingPeer.SendAcksOnly();
				}
				else
				{
					didSendAcks = false;
				}
			}
			return true;
		}
	}
}
