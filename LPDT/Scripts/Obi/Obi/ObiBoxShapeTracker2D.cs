using UnityEngine;

namespace Obi
{
	public class ObiBoxShapeTracker2D : ObiShapeTracker
	{
		public ObiBoxShapeTracker2D(ObiCollider2D source, BoxCollider2D collider)
		{
			base.source = source;
			base.collider = collider;
		}

		public override void UpdateIfNeeded()
		{
			BoxCollider2D boxCollider2D = collider as BoxCollider2D;
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			int index = source.Handle.index;
			ColliderShape value = instance.colliderShapes[index];
			value.is2D = true;
			value.type = ColliderShape.ShapeType.Box;
			value.filter = source.Filter;
			value.SetSign(source.Inverted);
			value.isTrigger = boxCollider2D.isTrigger;
			value.rigidbodyIndex = ((source.Rigidbody != null) ? source.Rigidbody.Handle.index : (-1));
			value.materialIndex = ((source.CollisionMaterial != null) ? source.CollisionMaterial.handle.index : (-1));
			value.forceZoneIndex = ((source.ForceZone != null) ? source.ForceZone.Handle.index : (-1));
			value.contactOffset = source.Thickness + boxCollider2D.edgeRadius;
			value.center = boxCollider2D.offset;
			value.size = boxCollider2D.size;
			instance.colliderShapes[index] = value;
			Aabb value2 = instance.colliderAabbs[index];
			value2.FromBounds(boxCollider2D.bounds, value.contactOffset, is2D: true);
			instance.colliderAabbs[index] = value2;
			AffineTransform value3 = instance.colliderTransforms[index];
			value3.FromTransform2D(boxCollider2D.transform, source.Rigidbody as ObiRigidbody2D);
			instance.colliderTransforms[index] = value3;
		}
	}
}
