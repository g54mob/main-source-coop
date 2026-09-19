using UnityEngine;

namespace Obi
{
	public class ObiDistanceFieldShapeTracker : ObiShapeTracker
	{
		public ObiDistanceField distanceField;

		private ObiDistanceFieldHandle handle;

		public ObiDistanceFieldShapeTracker(ObiCollider source, Component collider, ObiDistanceField distanceField)
		{
			base.source = source;
			base.collider = collider;
			this.distanceField = distanceField;
		}

		public void UpdateDistanceFieldData()
		{
			ObiColliderWorld.GetInstance().DestroyDistanceField(handle);
		}

		public override void UpdateIfNeeded()
		{
			bool isTrigger = false;
			if (collider is Collider)
			{
				isTrigger = ((Collider)collider).isTrigger;
			}
			else if (collider is Collider2D)
			{
				isTrigger = ((Collider2D)collider).isTrigger;
			}
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			int index = source.Handle.index;
			if (handle != null && handle.owner != distanceField && handle.Dereference())
			{
				instance.DestroyDistanceField(handle);
			}
			if (handle == null || !handle.isValid)
			{
				handle = instance.GetOrCreateDistanceField(distanceField);
				handle.Reference();
			}
			ColliderShape value = instance.colliderShapes[index];
			value.type = ColliderShape.ShapeType.SignedDistanceField;
			value.filter = source.Filter;
			value.SetSign(source.Inverted);
			value.isTrigger = isTrigger;
			value.rigidbodyIndex = ((source.Rigidbody != null) ? source.Rigidbody.Handle.index : (-1));
			value.materialIndex = ((source.CollisionMaterial != null) ? source.CollisionMaterial.handle.index : (-1));
			value.forceZoneIndex = ((source.ForceZone != null) ? source.ForceZone.Handle.index : (-1));
			value.contactOffset = source.Thickness;
			value.dataIndex = handle.index;
			instance.colliderShapes[index] = value;
			Aabb value2 = instance.colliderAabbs[index];
			value2.FromBounds(distanceField.FieldBounds.Transform(source.transform.localToWorldMatrix), value.contactOffset);
			instance.colliderAabbs[index] = value2;
			AffineTransform value3 = instance.colliderTransforms[index];
			value3.FromTransform3D(source.transform, source.Rigidbody as ObiRigidbody);
			instance.colliderTransforms[index] = value3;
		}

		public override void Destroy()
		{
			base.Destroy();
			if (handle != null && handle.Dereference())
			{
				ObiColliderWorld.GetInstance().DestroyDistanceField(handle);
			}
		}
	}
}
