using UnityEngine;

namespace Obi
{
	public class ObiTerrainShapeTracker : ObiShapeTracker
	{
		private ObiHeightFieldHandle handle;

		public ObiTerrainShapeTracker(ObiCollider source, TerrainCollider collider)
		{
			base.source = source;
			base.collider = collider;
		}

		public void UpdateHeightData()
		{
			ObiColliderWorld.GetInstance().DestroyHeightField(handle);
		}

		public override void UpdateIfNeeded()
		{
			TerrainCollider terrainCollider = collider as TerrainCollider;
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			int index = source.Handle.index;
			int heightmapResolution = terrainCollider.terrainData.heightmapResolution;
			if (handle == null || !handle.isValid)
			{
				handle = instance.GetOrCreateHeightField(terrainCollider.terrainData);
				handle.Reference();
			}
			ColliderShape value = instance.colliderShapes[index];
			value.type = ColliderShape.ShapeType.Heightmap;
			value.filter = source.Filter;
			value.SetSign(source.Inverted);
			value.isTrigger = terrainCollider.isTrigger;
			value.rigidbodyIndex = ((source.Rigidbody != null) ? source.Rigidbody.Handle.index : (-1));
			value.materialIndex = ((source.CollisionMaterial != null) ? source.CollisionMaterial.handle.index : (-1));
			value.forceZoneIndex = ((source.ForceZone != null) ? source.ForceZone.Handle.index : (-1));
			value.contactOffset = source.Thickness;
			value.dataIndex = handle.index;
			value.size = terrainCollider.terrainData.size;
			value.center = new Vector4(heightmapResolution, heightmapResolution, heightmapResolution, heightmapResolution);
			instance.colliderShapes[index] = value;
			Aabb value2 = instance.colliderAabbs[index];
			value2.FromBounds(terrainCollider.bounds, value.contactOffset);
			instance.colliderAabbs[index] = value2;
			AffineTransform value3 = instance.colliderTransforms[index];
			value3.FromTransform3D(terrainCollider.transform, source.Rigidbody as ObiRigidbody);
			instance.colliderTransforms[index] = value3;
		}

		public override void Destroy()
		{
			base.Destroy();
			if (handle != null && handle.Dereference())
			{
				ObiColliderWorld.GetInstance().DestroyHeightField(handle);
			}
		}
	}
}
