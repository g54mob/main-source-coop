using UnityEngine;

namespace Obi
{
	public class ObiCapsuleShapeTracker2D : ObiShapeTracker
	{
		public ObiCapsuleShapeTracker2D(ObiCollider2D source, CapsuleCollider2D collider)
		{
			base.source = source;
			base.collider = collider;
		}

		public override void UpdateIfNeeded()
		{
			CapsuleCollider2D capsuleCollider2D = collider as CapsuleCollider2D;
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			int index = source.Handle.index;
			ColliderShape value = instance.colliderShapes[index];
			value.is2D = true;
			value.type = ColliderShape.ShapeType.Capsule;
			value.filter = source.Filter;
			value.SetSign(source.Inverted);
			value.isTrigger = capsuleCollider2D.isTrigger;
			value.rigidbodyIndex = ((source.Rigidbody != null) ? source.Rigidbody.Handle.index : (-1));
			value.materialIndex = ((source.CollisionMaterial != null) ? source.CollisionMaterial.handle.index : (-1));
			value.forceZoneIndex = ((source.ForceZone != null) ? source.ForceZone.Handle.index : (-1));
			value.contactOffset = source.Thickness;
			value.center = capsuleCollider2D.offset;
			Vector2 size = capsuleCollider2D.size;
			value.size = new Vector4(((capsuleCollider2D.direction == CapsuleDirection2D.Horizontal) ? size.y : size.x) * 0.5f, Mathf.Max(size.x, size.y), (capsuleCollider2D.direction != CapsuleDirection2D.Horizontal) ? 1 : 0, 0f);
			instance.colliderShapes[index] = value;
			Aabb value2 = instance.colliderAabbs[index];
			value2.FromBounds(capsuleCollider2D.bounds, value.contactOffset, is2D: true);
			instance.colliderAabbs[index] = value2;
			AffineTransform value3 = instance.colliderTransforms[index];
			value3.FromTransform2D(capsuleCollider2D.transform, source.Rigidbody as ObiRigidbody2D);
			instance.colliderTransforms[index] = value3;
		}
	}
}
