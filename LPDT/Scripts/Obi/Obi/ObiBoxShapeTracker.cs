using UnityEngine;

namespace Obi
{
	public class ObiBoxShapeTracker : ObiShapeTracker
	{
		public ObiBoxShapeTracker(ObiCollider source, BoxCollider collider)
		{
			base.source = source;
			base.collider = collider;
		}

		public override void UpdateIfNeeded()
		{
			BoxCollider boxCollider = collider as BoxCollider;
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			int index = source.Handle.index;
			ColliderShape value = instance.colliderShapes[index];
			value.type = ColliderShape.ShapeType.Box;
			value.filter = source.Filter;
			value.SetSign(source.Inverted);
			value.isTrigger = boxCollider.isTrigger;
			value.rigidbodyIndex = ((source.Rigidbody != null) ? source.Rigidbody.Handle.index : (-1));
			value.materialIndex = ((source.CollisionMaterial != null) ? source.CollisionMaterial.handle.index : (-1));
			value.forceZoneIndex = ((source.ForceZone != null) ? source.ForceZone.Handle.index : (-1));
			value.contactOffset = source.Thickness;
			value.center = boxCollider.center;
			value.size = boxCollider.size;
			instance.colliderShapes[index] = value;
			Aabb value2 = instance.colliderAabbs[index];
			value2.FromBounds(boxCollider.bounds, value.contactOffset);
			instance.colliderAabbs[index] = value2;
			AffineTransform value3 = instance.colliderTransforms[index];
			value3.FromTransform3D(boxCollider.transform, source.Rigidbody as ObiRigidbody);
			instance.colliderTransforms[index] = value3;
		}
	}
}
