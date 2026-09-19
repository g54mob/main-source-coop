using UnityEngine;

namespace Obi.Samples
{
	public class CraneController : MonoBehaviour
	{
		private ObiRopeCursor cursor;

		private ObiRope rope;

		public float speed = 1f;

		private void Start()
		{
			cursor = GetComponentInChildren<ObiRopeCursor>();
			rope = cursor.GetComponent<ObiRope>();
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.W) && rope.restLength > 6.5f)
			{
				cursor.ChangeLength((0f - speed) * Time.deltaTime);
			}
			if (Input.GetKey(KeyCode.S))
			{
				cursor.ChangeLength(speed * Time.deltaTime);
			}
			if (Input.GetKey(KeyCode.A))
			{
				base.transform.Rotate(0f, Time.deltaTime * 15f, 0f);
			}
			if (Input.GetKey(KeyCode.D))
			{
				base.transform.Rotate(0f, (0f - Time.deltaTime) * 15f, 0f);
			}
		}
	}
}
