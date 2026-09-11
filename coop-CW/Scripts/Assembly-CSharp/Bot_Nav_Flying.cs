using UnityEngine;

public class Bot_Nav_Flying : MonoBehaviour
{
	private Bot bot;

	private void Start()
	{
		bot = GetComponent<Bot>();
	}

	private void Update()
	{
		Vector3 normalized = (Player.localPlayer.Center() - base.transform.position).normalized;
		bot.syncData.movementInput = normalized;
		bot.syncData.lookDireciton = normalized;
	}
}
