using UnityEngine;

namespace Obi
{
	public class ObiSphereShapeTracker : ObiShapeTracker
	{
		public ObiSphereShapeTracker(ObiCollider source, SphereCollider collider)
		{
			base.source = source;
			base.collider = collider;
		}

		public override void UpdateIfNeeded()
		{
			SphereCollider sphereCollider = collider as SphereCollider;
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			int index = source.Handle.index;
			ColliderShape value = instance.colliderShapes[index];
			value.type = ColliderShape.ShapeType.Sphere;
			value.filter = source.Filter;
			value.SetSign(source.Inverted);
			value.isTrigger = sphereCollider.isTrigger;
			value.rigidbodyIndex = ((source.Rigidbody != null) ? source.Rigidbody.Handle.index : (-1));
			value.materialIndex = ((source.CollisionMaterial != null) ? source.CollisionMaterial.handle.index : (-1));
			value.forceZoneIndex = ((source.ForceZone != null) ? source.ForceZone.Handle.index : (-1));
			value.contactOffset = source.Thickness;
			value.center = sphereCollider.center;
			value.size = Vector3.one * sphereCollider.radius;
			instance.colliderShapes[index] = value;
			Aabb value2 = instance.colliderAabbs[index];
			value2.FromBounds(sphereCollider.bounds, value.contactOffset);
			instance.colliderAabbs[index] = value2;
			AffineTransform value3 = instance.colliderTransforms[index];
			value3.FromTransform3D(sphereCollider.transform, source.Rigidbody as ObiRigidbody);
			instance.colliderTransforms[index] = value3;
		}
	}
}
