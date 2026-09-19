using UnityEngine;

namespace Features.BlackjackBeachInteractableModule.Scripts
{
	public class BlackjackDrawOccupancyZone : MonoBehaviour
	{
		[SerializeField]
		private Color _gizmoColor = new Color(1f, 0.55f, 0.15f, 0.25f);

		public bool ContainsWorldPoint(Vector3 point)
		{
			Vector3 vector = base.transform.InverseTransformPoint(point);
			if (Mathf.Abs(vector.x) <= 0.5f)
			{
				return Mathf.Abs(vector.z) <= 0.5f;
			}
			return false;
		}

		private void OnDrawGizmos()
		{
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = _gizmoColor;
			Vector3 size = new Vector3(1f, 0.05f, 1f);
			Gizmos.DrawCube(Vector3.zero, size);
			Gizmos.color = new Color(_gizmoColor.r, _gizmoColor.g, _gizmoColor.b, 1f);
			Gizmos.DrawWireCube(Vector3.zero, size);
			Gizmos.matrix = matrix;
		}
	}
}
