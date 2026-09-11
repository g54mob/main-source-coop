using UnityEngine;
using pworld.Scripts.Extensions;

namespace pworld.Scripts.PPhys.Bursted
{
	public class SpawnWall : MonoBehaviour
	{
		public Vector2 size;

		public GameObject pref;

		public Vector2 count;

		public void Run()
		{
			base.transform.KillAllChildren(destroyImmediate: true);
			for (int i = 0; (float)i < count.y; i++)
			{
				for (int j = 0; (float)j < count.x; j++)
				{
					Object.Instantiate(position: new Vector2(size.x * (float)j, size.y * (float)i), original: pref, rotation: Quaternion.identity).transform.parent = base.transform;
				}
			}
		}

		private void Awake()
		{
		}

		private void Start()
		{
		}

		private void Update()
		{
		}
	}
}
