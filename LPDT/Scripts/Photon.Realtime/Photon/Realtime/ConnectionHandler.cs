using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace Photon.Realtime
{
	public class ConnectionHandler : MonoBehaviour
	{
		public string Id;

		[Obsolete("After the KeepAliveInBackground, the client will always properly disconnect with DisconnectCause.ClientServiceInactivity.")]
		public bool DisconnectAfterKeepAlive;

		public int KeepAliveInBackground = 60000;

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

		private bool didSendAcks;

		private bool didWarnAboutMissingService;

		private int timeWarnAboutMissingService = 5000;

		private readonly Stopwatch backgroundStopwatch = new Stopwatch();

		private Timer stateTimer;

		private static GameObject go;

		public RealtimeClient Client { get; set; }

		public int CountSendAcksOnly { get; private set; }

		public bool FallbackThreadRunning { get; private set; }

		public static ConnectionHandler BuildInstance(RealtimeClient client, string id = null)
		{
			if (go == null)
			{
				go = new GameObject("ConnectionHandler");
				if (Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(go);
				}
			}
			ConnectionHandler connectionHandler = go.AddComponent<ConnectionHandler>();
			connectionHandler.Id = id;
			connectionHandler.Client = client;
			return connectionHandler;
		}

		public void RemoveInstance()
		{
			UnityEngine.Object.Destroy(this);
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void StaticReset()
		{
			go = null;
			AppQuits = false;
			AppPause = false;
			AppPauseRecent = false;
			AppOutOfFocus = false;
			AppOutOfFocusRecent = false;
		}

		protected virtual void Start()
		{
			if (Client == null)
			{
				UnityEngine.Debug.LogError("A ConnectionHandler should not be put into a scene. It is created by RealtimeClient.ConnectUsingSettings().", this);
			}
		}

		protected virtual void OnEnable()
		{
			StartFallbackSendAckThread();
		}

		protected virtual void OnDisable()
		{
			StopFallbackSendAckThread();
		}

		protected void OnApplicationQuit()
		{
			AppQuits = true;
			if (Client != null && Client.IsConnected)
			{
				Client.Disconnect(DisconnectCause.ApplicationQuit);
				Client.RealtimePeer.IsSimulationEnabled = false;
			}
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
			if (stateTimer == null)
			{
				stateTimer = new Timer(RealtimeFallback, null, 50, 50);
				FallbackThreadRunning = true;
			}
		}

		public void StopFallbackSendAckThread()
		{
			if (stateTimer != null)
			{
				stateTimer.Dispose();
				stateTimer = null;
			}
			FallbackThreadRunning = false;
		}

		public void RealtimeFallbackInvoke()
		{
			RealtimeFallback();
		}

		public void RealtimeFallback(object state = null)
		{
			if (Client == null)
			{
				return;
			}
			if (Client.IsConnected && Client.RealtimePeer.ConnectionTime - Client.RealtimePeer.Stats.LastSendOutgoingTimestamp > 100)
			{
				if (!didSendAcks)
				{
					backgroundStopwatch.Restart();
				}
				if (backgroundStopwatch.ElapsedMilliseconds > KeepAliveInBackground)
				{
					Client.Disconnect(DisconnectCause.ClientServiceInactivity);
					StopFallbackSendAckThread();
					UnityEngine.Object.Destroy(this);
					return;
				}
				didSendAcks = true;
				CountSendAcksOnly++;
				if (!didWarnAboutMissingService && backgroundStopwatch.ElapsedMilliseconds > timeWarnAboutMissingService)
				{
					didWarnAboutMissingService = true;
					_ = Client.State;
					_ = 13;
				}
				Client.RealtimePeer.SendAcksOnly();
			}
			else
			{
				if (backgroundStopwatch.IsRunning)
				{
					backgroundStopwatch.Reset();
				}
				didSendAcks = false;
			}
		}
	}
}
