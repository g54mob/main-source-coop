using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public static class ExtRigidbody
	{
		public static void AddForceAtPosition(this Rigidbody me, Vector3 force, Vector3 position, ForceMode fMode, float lineLenMul)
		{
			me.AddForceAtPosition(force, position, fMode);
			if (lineLenMul > 0f)
			{
				Debug.DrawLine(position, position + force * lineLenMul, Color.red);
			}
		}

		public static void AddForce(this Rigidbody me, Vector3 force, ForceMode fMode, float lineLenMul)
		{
			me.AddForce(force, fMode);
			if (lineLenMul > 0f)
			{
				Debug.DrawLine(me.position, me.position + force * lineLenMul, Color.red);
			}
		}
	}
}
