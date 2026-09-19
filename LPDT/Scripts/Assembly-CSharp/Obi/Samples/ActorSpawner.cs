using UnityEngine;

namespace Obi.Samples
{
	public class ActorSpawner : MonoBehaviour
	{
		public ObiActor template;

		public int maxInstances = 32;

		public float spawnDelay = 0.3f;

		private int instances;

		private float timeFromLastSpawn;

		private void Update()
		{
			timeFromLastSpawn += Time.deltaTime;
			if (Input.GetMouseButtonDown(0) && instances < maxInstances && timeFromLastSpawn > spawnDelay)
			{
				Object.Instantiate(template.gameObject, base.transform.position, Quaternion.identity).transform.SetParent(base.transform.parent);
				instances++;
				timeFromLastSpawn = 0f;
			}
		}
	}
}
