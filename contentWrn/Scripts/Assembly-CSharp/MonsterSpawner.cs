using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Zorro.Core.CLI;

public class MonsterSpawner : MonoBehaviour
{
	[Range(0f, 1f)]
	public float spawnChance = 1f;

	public string monster;

	public GameObject monsterPrefab;

	[ConsoleCommand]
	public static void SpawnMonster(string monster)
	{
		RaycastHit raycastHit = HelperFunctions.LineCheck(MainCamera.instance.transform.position, MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 30f, HelperFunctions.LayerType.TerrainProp);
		Vector3 vector = MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 30f;
		if (raycastHit.collider != null)
		{
			vector = raycastHit.point;
		}
		vector = HelperFunctions.GetGroundPos(vector + Vector3.up, HelperFunctions.LayerType.TerrainProp);
		PhotonNetwork.Instantiate(monster, vector, quaternion.identity, 0);
	}

	public static GameObject SpawnMonster(string monster, Vector3 position)
	{
		Vector3 groundPos = HelperFunctions.GetGroundPos(position + Vector3.up, HelperFunctions.LayerType.TerrainProp);
		return PhotonNetwork.Instantiate(monster, groundPos, quaternion.identity, 0);
	}

	internal void SpawnWithConditionalObject(GameObject conditionalObject)
	{
		if (!(UnityEngine.Random.value > spawnChance))
		{
			Bot componentInChildren = PhotonNetwork.Instantiate(monster, HelperFunctions.GetGroundPos(base.transform.position + Vector3.up, HelperFunctions.LayerType.TerrainProp), quaternion.identity, 0).GetComponentInChildren<Bot>();
			componentInChildren.hasConditionalObject = true;
			componentInChildren.conditionalObject = conditionalObject;
		}
	}

	public void TestSpawnChance()
	{
		Debug.Log(UnityEngine.Random.value > spawnChance);
	}
}
