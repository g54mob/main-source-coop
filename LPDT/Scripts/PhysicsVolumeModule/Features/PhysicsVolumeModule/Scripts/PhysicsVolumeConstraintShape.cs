using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	public abstract class PhysicsVolumeConstraintShape : MonoBehaviour
	{
		public abstract bool HasHardSurface { get; }

		public Vector3 ToLocal(Vector3 worldPoint)
		{
			return base.transform.InverseTransformPoint(worldPoint);
		}

		public Vector3 ToWorld(Vector3 localPoint)
		{
			return base.transform.TransformPoint(localPoint);
		}

		public abstract bool ContainsLocal(Vector3 localPoint);

		public virtual bool ContainsWithinHardSurfaces(Vector3 localPoint)
		{
			return ContainsLocal(localPoint);
		}

		public virtual bool ContainsColumnLocal(Vector3 localPoint, float columnHeight)
		{
			return false;
		}

		public virtual bool TryGetFloorCentreLocal(out Vector3 floorCentreLocal)
		{
			floorCentreLocal = Vector3.zero;
			return false;
		}

		public abstract bool TryConstrain(Vector3 previousLocal, Vector3 currentLocal, out Vector3 constrainedLocal, out Vector3 outwardWorld);

		public abstract string DescribeExit(Vector3 previousLocal, Vector3 currentLocal);
	}
}
