using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LobbyZone : MonoBehaviour
{
	public LobbyZoneType zoneType;

	private void Reset()
	{
		GetComponent<BoxCollider>().isTrigger = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		MyClient componentInParent = other.GetComponentInParent<MyClient>();
		if (!(componentInParent == null) && componentInParent.isLocalPlayer)
		{
			componentInParent.OnEnterZone(zoneType);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		MyClient componentInParent = other.GetComponentInParent<MyClient>();
		if (!(componentInParent == null) && componentInParent.isLocalPlayer)
		{
			componentInParent.OnExitZone(zoneType);
		}
	}
}
