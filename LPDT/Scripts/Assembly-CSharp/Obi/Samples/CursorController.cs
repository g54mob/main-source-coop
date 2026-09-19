using UnityEngine;

namespace Obi.Samples
{
	[RequireComponent(typeof(ObiRope))]
	public class CursorController : MonoBehaviour
	{
		public float minLength = 0.1f;

		public float speed = 1f;

		private ObiRopeCursor cursor;

		private ObiRope rope;

		private void OnEnable()
		{
			rope = GetComponent<ObiRope>();
			cursor = GetComponent<ObiRopeCursor>();
		}

		private void Update()
		{
			float num = 0f;
			if (Input.GetKey(KeyCode.W) && cursor != null)
			{
				num -= speed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.S) && cursor != null)
			{
				num += speed * Time.deltaTime;
			}
			if (rope.restLength + num < minLength)
			{
				num = minLength - rope.restLength;
			}
			cursor.ChangeLength(num);
			if (Input.GetKey(KeyCode.A))
			{
				rope.transform.Translate(Vector3.left * Time.deltaTime, Space.World);
			}
			if (Input.GetKey(KeyCode.D))
			{
				rope.transform.Translate(Vector3.right * Time.deltaTime, Space.World);
			}
		}
	}
}
