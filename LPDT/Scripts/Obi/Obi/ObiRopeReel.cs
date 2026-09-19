using UnityEngine;

namespace Obi
{
	[RequireComponent(typeof(ObiRopeCursor))]
	public class ObiRopeReel : MonoBehaviour
	{
		private ObiRopeCursor cursor;

		private ObiRope rope;

		[Header("Roll out/in thresholds")]
		public float outThreshold = 0.8f;

		public float inThreshold = 0.4f;

		[Header("Roll out/in speeds")]
		public float outSpeed = 4f;

		public float inSpeed = 2f;

		public float maxLength = 10f;

		private float restLength;

		public void Awake()
		{
			cursor = GetComponent<ObiRopeCursor>();
			rope = GetComponent<ObiRope>();
			restLength = rope.restLength;
		}

		public void OnValidate()
		{
			outThreshold = Mathf.Max(inThreshold, outThreshold);
		}

		private void Update()
		{
			float num = rope.CalculateLength();
			float num2 = Mathf.Max(0f, num - restLength);
			float num3 = 0f;
			if (num2 > outThreshold)
			{
				num3 = outSpeed * Time.deltaTime;
			}
			if (num2 < inThreshold)
			{
				num3 = (0f - inSpeed) * Time.deltaTime;
			}
			num3 -= Mathf.Max(0f, restLength + num3 - maxLength);
			restLength = cursor.ChangeLength(num3);
		}
	}
}
