using Photon.Pun;
using UnityEngine;

public class Puppetmonster : MonoBehaviour
{
	public float initialWait = 30f;

	public float interventionRate = 30f;

	private float interventionCounter;

	private void Update()
	{
		if (BotHandler.instance.bots.Count != 0 && PhotonNetwork.IsMasterClient)
		{
			initialWait -= Time.deltaTime;
			interventionCounter += Time.deltaTime;
			if (!(initialWait > 0f) && interventionCounter > interventionRate)
			{
				interventionCounter = 0f;
				Intervene();
			}
		}
	}

	private void Intervene()
	{
		Bot furthestFromAnyPlayer = BotHandler.instance.GetFurthestFromAnyPlayer();
		if (!(furthestFromAnyPlayer == null))
		{
			Debug.Log("Teleporting " + furthestFromAnyPlayer.transform.root.gameObject.name);
			furthestFromAnyPlayer.Teleport(Level.currentLevel.GetClosestHiddenPoint(PlayerHandler.instance.GetRandomPlayerAlive().Center()).transform.position);
		}
	}
}
