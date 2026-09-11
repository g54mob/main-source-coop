using UnityEngine;

namespace pworld.Scripts
{
	public class PStepChild : MonoBehaviour
	{
		public Transform parent;

		private void Awake()
		{
		}

		private void Start()
		{
		}

		private void Update()
		{
			if ((bool)parent)
			{
				base.transform.position = parent.position;
				base.transform.rotation = parent.rotation;
			}
		}

		private void OnDestroy()
		{
		}
	}
}
