using Mirror;
using Mirror.FizzySteam;
using UnityEngine;
using kcp2k;

public class TransportSwitcher : MonoBehaviour
{
	[Header("Transports")]
	public KcpTransport kcp;

	public FizzySteamworks fizzy;

	[Header("Mode")]
	public TransportMode editorMode;

	public TransportMode buildMode = TransportMode.FizzySteam;

	private MyNetworkManager nm;

	private NetworkManagerHUD hud;

	public static TransportMode CurrentMode { get; private set; }

	private void Awake()
	{
		nm = GetComponent<MyNetworkManager>();
		hud = GetComponent<NetworkManagerHUD>();
		Apply(buildMode);
	}

	private void Apply(TransportMode mode)
	{
		CurrentMode = mode;
		if (mode == TransportMode.KCP)
		{
			nm.transport = kcp;
			kcp.gameObject.SetActive(value: true);
			fizzy.gameObject.SetActive(value: false);
			if (hud != null)
			{
				hud.enabled = true;
			}
			Debug.Log("Transport: KCP");
		}
		else
		{
			nm.transport = fizzy;
			fizzy.gameObject.SetActive(value: true);
			kcp.gameObject.SetActive(value: false);
			if (hud != null)
			{
				hud.enabled = false;
			}
			Debug.Log("Transport: FizzySteamworks");
		}
	}
}
