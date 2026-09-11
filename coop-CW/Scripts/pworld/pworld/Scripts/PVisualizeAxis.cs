using UnityEngine;
using pworld.Scripts.Extensions;

namespace pworld.Scripts
{
	public class PVisualizeAxis : MonoBehaviour
	{
		public float length = 1f;

		private void OnDrawGizmos()
		{
			DrawArrow.ForGizmo(base.transform.position, length * base.transform.forward, Color.blue, length * 0.25f);
			DrawArrow.ForGizmo(base.transform.position, length * base.transform.right, Color.red, length * 0.25f);
			DrawArrow.ForGizmo(base.transform.position, length * base.transform.up, Color.green, length * 0.25f);
		}
	}
}
