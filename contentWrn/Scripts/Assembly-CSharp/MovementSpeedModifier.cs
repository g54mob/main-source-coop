using UnityEngine;

public class MovementSpeedModifier : MonoBehaviour
{
	public PlayerController movement;

	public float mod;

	private void Update()
	{
		movement.movementForce = mod;
	}
}
