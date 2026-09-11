using UnityEngine;

namespace pworld.Scripts
{
	public class PCam : MonoBehaviour
	{
		public static PCam me;

		public Camera mainCam;

		public static Camera Main => me.mainCam;

		private void Awake()
		{
			me = this;
			mainCam = Camera.main;
		}

		private void Start()
		{
		}

		private void Update()
		{
		}
	}
}
