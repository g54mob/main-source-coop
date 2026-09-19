using UnityEngine;

namespace Obi
{
	public class ObiCapsuleShapeTracker : ObiShapeTracker
	{
		public ObiCapsuleShapeTracker(ObiCollider source, CapsuleCollider collider)
		{
			base.collider = collider;
			base.source = source;
		}

		public override void UpdateIfNeeded()
		{
			CapsuleCollider capsuleCollider = collider as CapsuleCollider;
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			int index = source.Handle.index;
			ColliderShape value = instance.colliderShapes[index];
			value.type = ColliderShape.ShapeType.Capsule;
			value.filter = source.Filter;
			value.SetSign(source.Inverted);
			value.isTrigger = capsuleCollider.isTrigger;
			value.rigidbodyIndex = ((source.Rigidbody != null) ? source.Rigidbody.Handle.index : (-1));
			value.materialIndex = ((source.CollisionMaterial != null) ? source.CollisionMaterial.handle.index : (-1));
			value.forceZoneIndex = ((source.ForceZone != null) ? source.ForceZone.Handle.index : (-1));
			value.contactOffset = source.Thickness;
			value.center = capsuleCollider.center;
			value.size = new Vector4(capsuleCollider.radius, capsuleCollider.height, capsuleCollider.direction, 0f);
			instance.colliderShapes[index] = value;
			Aabb value2 = instance.colliderAabbs[index];
			value2.FromBounds(capsuleCollider.bounds, value.contactOffset);
			instance.colliderAabbs[index] = value2;
			AffineTransform value3 = instance.colliderTransforms[index];
			value3.FromTransform3D(capsuleCollider.transform, source.Rigidbody as ObiRigidbody);
			instance.colliderTransforms[index] = value3;
		}
	}
}
