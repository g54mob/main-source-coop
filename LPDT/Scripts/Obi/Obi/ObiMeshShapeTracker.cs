using UnityEngine;

namespace Obi
{
	public class ObiMeshShapeTracker : ObiShapeTracker
	{
		private ObiTriangleMeshHandle handle;

		public Mesh targetMesh => (collider as MeshCollider)?.sharedMesh;

		public ObiMeshShapeTracker(ObiCollider source, MeshCollider collider)
		{
			base.source = source;
			base.collider = collider;
		}

		public void UpdateMeshData()
		{
			ObiColliderWorld.GetInstance().DestroyTriangleMesh(handle);
		}

		public override void UpdateIfNeeded()
		{
			MeshCollider meshCollider = collider as MeshCollider;
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			int index = source.Handle.index;
			if (handle != null && handle.owner != meshCollider.sharedMesh && handle.Dereference())
			{
				instance.DestroyTriangleMesh(handle);
			}
			if (handle == null || !handle.isValid)
			{
				handle = instance.GetOrCreateTriangleMesh(meshCollider.sharedMesh);
				handle.Reference();
			}
			ColliderShape value = instance.colliderShapes[index];
			value.type = ColliderShape.ShapeType.TriangleMesh;
			value.filter = source.Filter;
			value.SetSign(source.Inverted);
			value.isTrigger = meshCollider.isTrigger;
			value.rigidbodyIndex = ((source.Rigidbody != null) ? source.Rigidbody.Handle.index : (-1));
			value.materialIndex = ((source.CollisionMaterial != null) ? source.CollisionMaterial.handle.index : (-1));
			value.forceZoneIndex = ((source.ForceZone != null) ? source.ForceZone.Handle.index : (-1));
			value.contactOffset = source.Thickness;
			value.dataIndex = handle.index;
			instance.colliderShapes[index] = value;
			Aabb value2 = instance.colliderAabbs[index];
			value2.FromBounds(meshCollider.bounds, value.contactOffset);
			instance.colliderAabbs[index] = value2;
			AffineTransform value3 = instance.colliderTransforms[index];
			value3.FromTransform3D(meshCollider.transform, source.Rigidbody as ObiRigidbody);
			instance.colliderTransforms[index] = value3;
		}

		public override void Destroy()
		{
			base.Destroy();
			if (handle != null && handle.Dereference())
			{
				ObiColliderWorld.GetInstance().DestroyTriangleMesh(handle);
			}
		}
	}
}
