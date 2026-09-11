using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Zorro.Core.CLI;

public class Monster
{
	[ConsoleCommand]
	public static void KillAll()
	{
		BotHandler.instance.DestroyAll();
	}

	[ConsoleCommand]
	public static void SpawnMonster(string monster)
	{
		Vector3 vector = MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 30f;
		vector = HelperFunctions.GetGroundPos(vector + Vector3.up, HelperFunctions.LayerType.TerrainProp);
		PhotonNetwork.Instantiate(monster, vector, quaternion.identity, 0);
	}
}
