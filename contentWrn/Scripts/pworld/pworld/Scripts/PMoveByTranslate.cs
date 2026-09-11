using UnityEngine;

namespace pworld.Scripts
{
	public class PMoveByTranslate : MonoBehaviour
	{
		public Vector3 localSpeed;

		private void Awake()
		{
		}

		private void Start()
		{
		}

		private void Update()
		{
			base.transform.Translate(localSpeed * Time.deltaTime, Space.Self);
		}
	}
}
