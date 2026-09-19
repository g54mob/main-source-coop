using UnityEngine;

namespace Obi
{
	public class ObiEdgeShapeTracker2D : ObiShapeTracker
	{
		private ObiEdgeMeshHandle handle;

		public ObiEdgeShapeTracker2D(ObiCollider2D source, EdgeCollider2D collider)
		{
			base.source = source;
			base.collider = collider;
		}

		public void UpdateEdgeData()
		{
			ObiColliderWorld.GetInstance().DestroyEdgeMesh(handle);
		}

		public override void UpdateIfNeeded()
		{
			EdgeCollider2D edgeCollider2D = collider as EdgeCollider2D;
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			int index = source.Handle.index;
			if (handle == null || !handle.isValid)
			{
				handle = instance.GetOrCreateEdgeMesh(edgeCollider2D);
				handle.Reference();
			}
			ColliderShape value = instance.colliderShapes[index];
			value.is2D = true;
			value.type = ColliderShape.ShapeType.EdgeMesh;
			value.filter = source.Filter;
			value.SetSign(source.Inverted);
			value.isTrigger = edgeCollider2D.isTrigger;
			value.rigidbodyIndex = ((source.Rigidbody != null) ? source.Rigidbody.Handle.index : (-1));
			value.materialIndex = ((source.CollisionMaterial != null) ? source.CollisionMaterial.handle.index : (-1));
			value.forceZoneIndex = ((source.ForceZone != null) ? source.ForceZone.Handle.index : (-1));
			value.center = edgeCollider2D.offset;
			value.contactOffset = source.Thickness + edgeCollider2D.edgeRadius;
			value.dataIndex = handle.index;
			instance.colliderShapes[index] = value;
			Aabb value2 = instance.colliderAabbs[index];
			value2.FromBounds(edgeCollider2D.bounds, value.contactOffset, is2D: true);
			instance.colliderAabbs[index] = value2;
			AffineTransform value3 = instance.colliderTransforms[index];
			value3.FromTransform2D(edgeCollider2D.transform, source.Rigidbody as ObiRigidbody2D);
			instance.colliderTransforms[index] = value3;
		}

		public override void Destroy()
		{
			base.Destroy();
			if (handle != null && handle.Dereference())
			{
				ObiColliderWorld.GetInstance().DestroyEdgeMesh(handle);
			}
		}
	}
}
