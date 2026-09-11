using UnityEngine;

namespace pworld.Scripts
{
	public class PDestroyAfterTime : MonoBehaviour
	{
		public float time = 1f;

		private void Awake()
		{
		}

		private void Start()
		{
		}

		private void Update()
		{
			time -= Time.deltaTime;
			if (time <= 0f)
			{
				Object.Destroy(base.gameObject);
			}
		}

		private void OnDestroy()
		{
		}
	}
}
