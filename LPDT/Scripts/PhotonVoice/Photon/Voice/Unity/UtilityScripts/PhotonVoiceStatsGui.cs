using Photon.Client;
using UnityEngine;

namespace Photon.Voice.Unity.UtilityScripts
{
	public class PhotonVoiceStatsGui : MonoBehaviour
	{
		private bool statsWindowOn = true;

		private bool healthStatsVisible = true;

		private bool trafficStatsOn = true;

		private bool buttonsOn = true;

		private bool voiceStatsOn = true;

		private Rect statsRect = new Rect(0f, 100f, 300f, 50f);

		private int windowId = 200;

		private PhotonPeer peer;

		private VoiceConnection voiceConnection;

		private VoiceClient voiceClient;

		private TrafficStatsSnapshot statsSnapshot;

		private void Start()
		{
			VoiceConnection[] components = GetComponents<VoiceConnection>();
			if (components == null || components.Length == 0)
			{
				Debug.LogError("No VoiceConnection component found, PhotonVoiceStatsGui disabled", this);
				base.enabled = false;
				return;
			}
			if (components.Length > 1)
			{
				Debug.LogWarningFormat(this, "Multiple VoiceConnection components found, using first occurrence attached to GameObject {0}", components[0].name);
			}
			voiceConnection = components[0];
			voiceClient = voiceConnection.VoiceClient;
			peer = voiceConnection.Client.RealtimePeer;
			statsSnapshot = peer.Stats.ToSnapshot();
			peer.Stats.ResetMaximumCounters();
			if (statsRect.x <= 0f)
			{
				statsRect.x = (float)Screen.width - statsRect.width;
			}
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
			{
				statsWindowOn = !statsWindowOn;
			}
		}

		private void OnGUI()
		{
			if (statsWindowOn)
			{
				statsRect = GUILayout.Window(windowId, statsRect, TrafficStatsWindow, "Voice Client Messages (shift+tab)");
			}
		}

		private void TrafficStatsWindow(int windowId)
		{
			bool flag = false;
			TrafficStatsDelta trafficStatsDelta = new TrafficStatsDelta(statsSnapshot, peer.Stats.ToSnapshot());
			GUILayout.BeginHorizontal();
			buttonsOn = GUILayout.Toggle(buttonsOn, "buttons");
			healthStatsVisible = GUILayout.Toggle(healthStatsVisible, "health");
			trafficStatsOn = GUILayout.Toggle(trafficStatsOn, "traffic");
			voiceStatsOn = GUILayout.Toggle(voiceStatsOn, "voice stats");
			GUILayout.EndHorizontal();
			string text = $"Out {trafficStatsDelta.PackagesOut,4} | In {trafficStatsDelta.PackagesIn,4} | Sum {trafficStatsDelta.PackagesOut + trafficStatsDelta.PackagesIn,4}";
			string text2 = $"{trafficStatsDelta.DeltaTime / 1000} sec average:";
			string text3 = ((trafficStatsDelta.DeltaTime > 0) ? $"Out {trafficStatsDelta.PackagesOut * 1000 / trafficStatsDelta.DeltaTime,4} | In {trafficStatsDelta.PackagesIn * 1000 / trafficStatsDelta.DeltaTime,4} | Sum {(trafficStatsDelta.PackagesOut + trafficStatsDelta.PackagesIn) * 1000 / trafficStatsDelta.DeltaTime,4}" : "");
			GUILayout.Label(text);
			GUILayout.Label(text2);
			GUILayout.Label(text3);
			if (buttonsOn)
			{
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Reset"))
				{
					statsSnapshot = peer.Stats.ToSnapshot();
					peer.Stats.ResetMaximumCounters();
				}
				flag = GUILayout.Button("To Log");
				GUILayout.EndHorizontal();
			}
			if (trafficStatsOn)
			{
				GUILayout.Label(trafficStatsDelta.ToString(udpValues: true, rttValues: true, callValues: true));
			}
			string text4 = string.Empty;
			if (healthStatsVisible)
			{
				GUILayout.Box("Voice Client Health Stats");
				text4 = $"ping: {peer.Stats.RoundtripTime}|{voiceClient.RoundTripTime}[+/-{peer.Stats.RoundtripTimeVariance}|{voiceClient.RoundTripTimeVariance}]ms resent:{trafficStatsDelta.UdpReliableCommandsResent} \n\nmax ms between\nsend: {peer.Stats.LongestDeltaBetweenSendOutgoingCalls,4} \ndispatch: {peer.Stats.LongestDeltaBetweenDispatchCalls,4}";
				GUILayout.Label(text4);
			}
			_ = string.Empty;
			if (voiceStatsOn)
			{
				GUILayout.Box("Voice Frames Stats");
				GUILayout.Label($"received: {voiceClient.FramesReceived}, {voiceConnection.FramesReceivedPerSecond:F2}/s\nlost: {voiceClient.FramesLost}, {voiceConnection.FramesLostPerSecond:F2}/s ({voiceConnection.FramesLostPercent:F2}%) \nfec: {voiceClient.FramesRecovered}, frag part: {voiceClient.FramesFragPart}\nsent: {voiceClient.FramesSent} ({voiceClient.FramesSentBytes} bytes)");
			}
			if (flag)
			{
				Debug.Log($"{text}\n{text2}\n{text3}\n{peer.Stats.ToString()}\n{text4}");
			}
			if (GUI.changed)
			{
				statsRect.height = 100f;
			}
			GUI.DragWindow();
		}
	}
}
