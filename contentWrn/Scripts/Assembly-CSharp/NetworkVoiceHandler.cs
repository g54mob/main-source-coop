using System;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

public class NetworkVoiceHandler : MonoBehaviour
{
	private const byte ALIVE_CHANNEL = 1;

	private const byte DEAD_CHANNEL = 2;

	private static VoiceConnection m_VoiceConnection;

	private static Recorder m_LocalRecorder;

	private void Awake()
	{
		PunVoiceClient component = GetComponent<PunVoiceClient>();
		if (component == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (PunVoiceClient.Instance != component)
		{
			Debug.Log("Already Found VoiceClient, Destroying...");
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			base.transform.SetParent(null);
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	private void Start()
	{
		m_VoiceConnection = GetComponent<VoiceConnection>();
		if (m_VoiceConnection.Client.State != ClientState.Joined)
		{
			m_VoiceConnection.Client.StateChanged += OnStateChanged;
		}
		else
		{
			InitNetworkVoice();
		}
	}

	public static void LocalPlayerAssigned(Player p)
	{
		m_LocalRecorder = p.GetComponentInChildren<Recorder>();
	}

	private void OnStateChanged(ClientState state, ClientState toState)
	{
		if (toState == ClientState.Joined)
		{
			InitNetworkVoice();
		}
	}

	public static void InitNetworkVoice()
	{
		if (!(m_VoiceConnection == null) && m_VoiceConnection.Client.State == ClientState.Joined)
		{
			if (!PhotonGameLobbyHandler.IsSurface)
			{
				TalkToAlive();
			}
			else
			{
				ListenAndSendToAll();
			}
		}
	}

	public static void ListenAndSendToAll()
	{
		Debug.Log("Changing Voice Channels Should Talk to and Hear Everyone!");
		m_VoiceConnection.Client.LoadBalancingPeer.OpChangeGroups(Array.Empty<byte>(), Array.Empty<byte>());
		m_LocalRecorder.InterestGroup = 0;
	}

	public static void TalkToDead()
	{
		if (PhotonGameLobbyHandler.IsSurface)
		{
			Debug.LogWarning("TalkToDead called in surface scene! shouldn't happen, calling ListenAndSendToAll instead");
			ListenAndSendToAll();
			return;
		}
		Debug.Log("Changing Voice Channels Should Talk to and Hear DEAD And ALIVE!");
		byte[] groupsToAdd = new byte[2] { 2, 1 };
		m_VoiceConnection.Client.LoadBalancingPeer.OpChangeGroups(Array.Empty<byte>(), groupsToAdd);
		m_LocalRecorder.InterestGroup = 2;
	}

	public static void TalkToAlive()
	{
		if (PhotonGameLobbyHandler.IsSurface)
		{
			Debug.LogWarning("TalkToAlive called in surface scene! shouldn't happen, calling ListenAndSendToAll instead");
			ListenAndSendToAll();
			return;
		}
		Debug.Log("Changing Voice Channels Should Talk to and Hear ALIVE!");
		byte[] groupsToAdd = new byte[1] { 1 };
		m_VoiceConnection.Client.LoadBalancingPeer.OpChangeGroups(Array.Empty<byte>(), groupsToAdd);
		m_LocalRecorder.InterestGroup = 1;
	}
}
