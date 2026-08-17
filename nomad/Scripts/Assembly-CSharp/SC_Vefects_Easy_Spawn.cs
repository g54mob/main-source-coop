using UnityEngine;

public class SC_Vefects_Easy_Spawn : MonoBehaviour
{
	public GameObject toSpawnVFX;

	public GameObject WhereToSpawn;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Object.Instantiate(toSpawnVFX, WhereToSpawn.transform);
		}
	}
}
