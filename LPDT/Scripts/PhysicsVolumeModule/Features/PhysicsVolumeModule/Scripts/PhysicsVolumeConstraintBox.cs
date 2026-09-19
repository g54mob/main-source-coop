using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public class PhysicsVolumeConstraintBox : PhysicsVolumeConstraintShape
	{
		private const float GIZMO_FACE_THICKNESS = 0.02f;

		private const float GIZMO_NORMAL_LENGTH = 0.25f;

		[SerializeField]
		private Vector3 _center = Vector3.zero;

		[SerializeField]
		private Vector3 _size = new Vector3(2f, 1.5f, 2f);

		[Tooltip("Faces cargo may NOT cross, in this box's own local space. Unticked faces are open — cargo can leave through them freely. Rotating or flipping the box rotates its walls with it.")]
		[SerializeField]
		private ConstraintBoxFace _hardFaces = ConstraintBoxFace.PositiveX | ConstraintBoxFace.NegativeX | ConstraintBoxFace.PositiveZ | ConstraintBoxFace.NegativeZ;

		private const float SURFACE_INSET = 0.001f;

		public ConstraintBoxFace HardFaces => _hardFaces;

		public override bool HasHardSurface => _hardFaces != ConstraintBoxFace.None;

		public override bool TryConstrain(Vector3 previousLocal, Vector3 currentLocal, out Vector3 constrainedLocal, out Vector3 outwardWorld)
		{
			outwardWorld = Vector3.zero;
			if (!TryConstrain(previousLocal, currentLocal, out constrainedLocal, out var crossedFace))
			{
				return false;
			}
			outwardWorld = FaceNormalWorld(crossedFace);
			return true;
		}

		public Vector3 FaceNormalWorld(ConstraintBoxFace face)
		{
			return face switch
			{
				ConstraintBoxFace.PositiveX => base.transform.right, 
				ConstraintBoxFace.NegativeX => -base.transform.right, 
				ConstraintBoxFace.PositiveY => base.transform.up, 
				ConstraintBoxFace.NegativeY => -base.transform.up, 
				ConstraintBoxFace.PositiveZ => base.transform.forward, 
				ConstraintBoxFace.NegativeZ => -base.transform.forward, 
				_ => Vector3.zero, 
			};
		}

		public override bool ContainsColumnLocal(Vector3 localPoint, float columnHeight)
		{
			Vector3 vector = localPoint - _center;
			Vector3 vector2 = _size * 0.5f;
			if (Mathf.Abs(vector.x) <= vector2.x && Mathf.Abs(vector.z) <= vector2.z && vector.y >= 0f - vector2.y)
			{
				return vector.y <= 0f - vector2.y + columnHeight;
			}
			return false;
		}

		public override bool TryGetFloorCentreLocal(out Vector3 floorCentreLocal)
		{
			floorCentreLocal = _center + new Vector3(0f, (0f - _size.y) * 0.5f, 0f);
			return true;
		}

		public override bool ContainsLocal(Vector3 localPoint)
		{
			Vector3 vector = localPoint - _center;
			Vector3 vector2 = _size * 0.5f;
			if (Mathf.Abs(vector.x) <= vector2.x && Mathf.Abs(vector.y) <= vector2.y)
			{
				return Mathf.Abs(vector.z) <= vector2.z;
			}
			return false;
		}

		public bool TryConstrain(Vector3 previousLocal, Vector3 currentLocal, out Vector3 constrainedLocal, out ConstraintBoxFace crossedFace)
		{
			constrainedLocal = currentLocal;
			crossedFace = ConstraintBoxFace.None;
			if (!TryEarliestCrossing(previousLocal, currentLocal, out var face, out var axis, out var sign))
			{
				return false;
			}
			Vector3 vector = currentLocal - _center;
			Vector3 vector2 = _size * 0.5f;
			if ((_hardFaces & face) == 0)
			{
				return false;
			}
			Vector3 vector3 = vector;
			vector3[axis] = (vector2[axis] - 0.001f) * sign;
			constrainedLocal = vector3 + _center;
			crossedFace = face;
			return true;
		}

		public override string DescribeExit(Vector3 previousLocal, Vector3 currentLocal)
		{
			if (ContainsLocal(currentLocal) || !ContainsLocal(previousLocal))
			{
				return null;
			}
			if (!TryEarliestCrossing(previousLocal, currentLocal, out var face, out var _, out var _))
			{
				return null;
			}
			return string.Format("{0}:{1}", face, ((_hardFaces & face) != ConstraintBoxFace.None) ? "HARD" : "open");
		}

		private bool TryEarliestCrossing(Vector3 previousLocal, Vector3 currentLocal, out ConstraintBoxFace face, out int axis, out float sign)
		{
			face = ConstraintBoxFace.None;
			axis = -1;
			sign = 0f;
			Vector3 vector = previousLocal - _center;
			Vector3 vector2 = currentLocal - _center;
			Vector3 vector3 = _size * 0.5f;
			float num = float.MaxValue;
			for (int i = 0; i < 3; i++)
			{
				float num2 = vector3[i];
				for (int j = -1; j <= 1; j += 2)
				{
					float num3 = num2 * (float)j;
					float num4 = vector[i] * (float)j;
					float num5 = vector2[i] * (float)j;
					if (num4 > num2 || num5 <= num2)
					{
						continue;
					}
					float num6 = vector2[i] - vector[i];
					if (!(Mathf.Abs(num6) < Mathf.Epsilon))
					{
						float num7 = (num3 - vector[i]) / num6;
						if (!(num7 < 0f) && !(num7 > num))
						{
							num = num7;
							axis = i;
							sign = j;
						}
					}
				}
			}
			if (axis < 0)
			{
				return false;
			}
			face = FaceOf(axis, sign);
			return true;
		}

		private static ConstraintBoxFace FaceOf(int axis, float sign)
		{
			switch (axis)
			{
			case 0:
				if (!(sign > 0f))
				{
					return ConstraintBoxFace.NegativeX;
				}
				return ConstraintBoxFace.PositiveX;
			case 1:
				if (!(sign > 0f))
				{
					return ConstraintBoxFace.NegativeY;
				}
				return ConstraintBoxFace.PositiveY;
			default:
				if (!(sign > 0f))
				{
					return ConstraintBoxFace.NegativeZ;
				}
				return ConstraintBoxFace.PositiveZ;
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = new Color(1f, 0.55f, 0.1f, 0.9f);
			Gizmos.DrawWireCube(_center, _size);
			Vector3 vector = _size * 0.5f;
			DrawFace(ConstraintBoxFace.PositiveX, Vector3.right, vector.x, new Vector3(0f, _size.y, _size.z));
			DrawFace(ConstraintBoxFace.NegativeX, Vector3.left, vector.x, new Vector3(0f, _size.y, _size.z));
			DrawFace(ConstraintBoxFace.PositiveY, Vector3.up, vector.y, new Vector3(_size.x, 0f, _size.z));
			DrawFace(ConstraintBoxFace.NegativeY, Vector3.down, vector.y, new Vector3(_size.x, 0f, _size.z));
			DrawFace(ConstraintBoxFace.PositiveZ, Vector3.forward, vector.z, new Vector3(_size.x, _size.y, 0f));
			DrawFace(ConstraintBoxFace.NegativeZ, Vector3.back, vector.z, new Vector3(_size.x, _size.y, 0f));
		}

		private void DrawFace(ConstraintBoxFace face, Vector3 outward, float extent, Vector3 size)
		{
			if ((_hardFaces & face) != ConstraintBoxFace.None)
			{
				Vector3 vector = _center + outward * extent;
				Vector3 size2 = new Vector3(Mathf.Max(size.x, 0.02f), Mathf.Max(size.y, 0.02f), Mathf.Max(size.z, 0.02f));
				Gizmos.color = new Color(1f, 0.35f, 0.05f, 0.22f);
				Gizmos.DrawCube(vector, size2);
				Gizmos.color = new Color(1f, 0.55f, 0.1f, 0.9f);
				Gizmos.DrawWireCube(vector, size2);
				Gizmos.DrawLine(vector, vector + outward * 0.25f);
			}
		}
	}
}
