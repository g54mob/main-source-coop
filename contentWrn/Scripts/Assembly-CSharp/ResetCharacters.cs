using UnityEngine;

public class ResetCharacters : MonoBehaviour
{
	private void ResetChars()
	{
		for (int i = 0; i < PlayerHandler.instance.players.Count; i++)
		{
			PlayerHandler.instance.players[i].CallRevive();
		}
	}
}
