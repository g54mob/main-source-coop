using UnityEngine;

public class SpawnObject : MonoBehaviour
{
	public GameObject objectToSpawn;

	public void DoSpawn()
	{
		Object.Instantiate(objectToSpawn, base.transform.position, base.transform.rotation);
	}
}
