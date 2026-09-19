using UnityEngine;

namespace Obi
{
	public class ObiCircleShapeTracker2D : ObiShapeTracker
	{
		public ObiCircleShapeTracker2D(ObiCollider2D source, CircleCollider2D collider)
		{
			base.source = source;
			base.collider = collider;
		}

		public override void UpdateIfNeeded()
		{
			CircleCollider2D circleCollider2D = collider as CircleCollider2D;
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			int index = source.Handle.index;
			ColliderShape value = instance.colliderShapes[index];
			value.is2D = true;
			value.type = ColliderShape.ShapeType.Sphere;
			value.filter = source.Filter;
			value.SetSign(source.Inverted);
			value.isTrigger = circleCollider2D.isTrigger;
			value.rigidbodyIndex = ((source.Rigidbody != null) ? source.Rigidbody.Handle.index : (-1));
			value.materialIndex = ((source.CollisionMaterial != null) ? source.CollisionMaterial.handle.index : (-1));
			value.forceZoneIndex = ((source.ForceZone != null) ? source.ForceZone.Handle.index : (-1));
			value.contactOffset = source.Thickness;
			value.center = circleCollider2D.offset;
			value.size = Vector3.one * circleCollider2D.radius;
			instance.colliderShapes[index] = value;
			Aabb value2 = instance.colliderAabbs[index];
			value2.FromBounds(circleCollider2D.bounds, value.contactOffset, is2D: true);
			instance.colliderAabbs[index] = value2;
			AffineTransform value3 = instance.colliderTransforms[index];
			value3.FromTransform2D(circleCollider2D.transform, source.Rigidbody as ObiRigidbody2D);
			instance.colliderTransforms[index] = value3;
		}
	}
}
