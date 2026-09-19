using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RaceZone : MonoBehaviour
{
	public RaceTeam team;

	public RaceGameController controller;

	private void Reset()
	{
		GetComponent<BoxCollider>().isTrigger = true;
	}

	private void Awake()
	{
		if (controller == null)
		{
			controller = GetComponentInParent<RaceGameController>();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		RaceGameParticipant componentInParent = other.GetComponentInParent<RaceGameParticipant>();
		if (!(componentInParent == null))
		{
			componentInParent.EnterZone(team, controller);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		RaceGameParticipant componentInParent = other.GetComponentInParent<RaceGameParticipant>();
		if (!(componentInParent == null))
		{
			componentInParent.ExitZone(team);
		}
	}
}
