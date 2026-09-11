using UnityEngine;

namespace pworld.Scripts
{
	public class P : MonoBehaviour
	{
		public static P me;

		public Camera cam;

		public float DONTUSEME;

		public static Camera Cam => me.cam;

		private void Awake()
		{
			me = this;
			cam = Camera.main;
		}

		private void Start()
		{
		}

		private void Update()
		{
		}
	}
}
