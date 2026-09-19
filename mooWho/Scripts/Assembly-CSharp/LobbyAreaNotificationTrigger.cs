using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LobbyAreaNotificationTrigger : MonoBehaviour
{
	public LobbyAreaNotification notification;

	private void Reset()
	{
		GetComponent<Collider>().isTrigger = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		MyClient componentInParent = other.GetComponentInParent<MyClient>();
		if (!(componentInParent == null) && componentInParent.isLocalPlayer)
		{
			notification?.NotifyEnter();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		MyClient componentInParent = other.GetComponentInParent<MyClient>();
		if (!(componentInParent == null) && componentInParent.isLocalPlayer)
		{
			notification?.NotifyExit();
		}
	}
}
