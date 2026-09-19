using UnityEngine;

namespace Features.AerohockeyBeachInteractableModule.Scripts
{
	public class AerohockeyPaddleZone : MonoBehaviour
	{
		[SerializeField]
		private Color _gizmoColor = new Color(0.25f, 0.85f, 1f, 0.3f);

		public void ClampPlayfieldLocal(Transform playfield, float localX, float localZ, out float clampedX, out float clampedZ)
		{
			GetPlayfieldLocalXzBounds(playfield, out var minX, out var maxX, out var minZ, out var maxZ);
			clampedX = Mathf.Clamp(localX, minX, maxX);
			clampedZ = Mathf.Clamp(localZ, minZ, maxZ);
		}

		private void GetPlayfieldLocalXzBounds(Transform playfield, out float minX, out float maxX, out float minZ, out float maxZ)
		{
			minX = float.PositiveInfinity;
			maxX = float.NegativeInfinity;
			minZ = float.PositiveInfinity;
			maxZ = float.NegativeInfinity;
			for (int i = 0; i < 8; i++)
			{
				Vector3 position = new Vector3(((i & 1) == 0) ? (-0.5f) : 0.5f, ((i & 2) == 0) ? (-0.5f) : 0.5f, ((i & 4) == 0) ? (-0.5f) : 0.5f);
				Vector3 vector = playfield.InverseTransformPoint(base.transform.TransformPoint(position));
				minX = Mathf.Min(minX, vector.x);
				maxX = Mathf.Max(maxX, vector.x);
				minZ = Mathf.Min(minZ, vector.z);
				maxZ = Mathf.Max(maxZ, vector.z);
			}
		}

		private void OnDrawGizmos()
		{
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = _gizmoColor;
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
			Gizmos.color = new Color(_gizmoColor.r, _gizmoColor.g, _gizmoColor.b, 1f);
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
			Gizmos.matrix = matrix;
		}
	}
}
