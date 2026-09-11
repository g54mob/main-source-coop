using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
	public UnityEvent triggerEvent_Enter;

	public UnityEvent triggerEvent_Exit;

	private void OnTriggerEnter(Collider other)
	{
		if ((bool)other.GetComponentInParent<Player>())
		{
			triggerEvent_Enter.Invoke();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if ((bool)other.GetComponentInParent<Player>())
		{
			triggerEvent_Exit.Invoke();
		}
	}
}
