using UnityEngine;

public class StandingForceModifier : MonoBehaviour
{
	public PlayerController movement;

	public float mod = 2f;

	private void Update()
	{
		movement.standForce = mod;
	}
}
