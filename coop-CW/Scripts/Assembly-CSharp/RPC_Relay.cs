using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class RPC_Relay : MonoBehaviour
{
	public UnityEvent event_Simple;

	public UnityEvent event_Simple2;

	private PhotonView view;

	private void Start()
	{
		view = GetComponent<PhotonView>();
	}

	public void Call_Event_Simple()
	{
		view.RPC("RPCA_Event_Simple", RpcTarget.All);
	}

	[PunRPC]
	public void RPCA_Event_Simple()
	{
		event_Simple.Invoke();
	}

	public void Call_Event_Simple2()
	{
		view.RPC("RPCA_Event_Simple2", RpcTarget.All);
	}

	[PunRPC]
	public void RPCA_Event_Simple2()
	{
		event_Simple2.Invoke();
	}
}
