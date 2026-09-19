using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	public class ObiColliderWorld
	{
		[NonSerialized]
		public List<IColliderWorldImpl> implementations;

		[NonSerialized]
		public List<ObiColliderHandle> colliderHandles;

		[NonSerialized]
		public ObiNativeColliderShapeList colliderShapes;

		[NonSerialized]
		public ObiNativeAabbList colliderAabbs;

		[NonSerialized]
		public ObiNativeAffineTransformList colliderTransforms;

		[NonSerialized]
		public List<ObiForceZoneHandle> forceZoneHandles;

		[NonSerialized]
		public ObiNativeForceZoneList forceZones;

		[NonSerialized]
		public List<ObiCollisionMaterialHandle> materialHandles;

		[NonSerialized]
		public ObiNativeCollisionMaterialList collisionMaterials;

		[NonSerialized]
		public List<ObiRigidbodyHandle> rigidbodyHandles;

		[NonSerialized]
		public ObiNativeRigidbodyList rigidbodies;

		[NonSerialized]
		public ObiTriangleMeshContainer triangleMeshContainer;

		[NonSerialized]
		public ObiEdgeMeshContainer edgeMeshContainer;

		[NonSerialized]
		public ObiDistanceFieldContainer distanceFieldContainer;

		[NonSerialized]
		public ObiHeightFieldContainer heightFieldContainer;

		private List<ObiColliderHandle> collidersToCreate;

		private List<ObiColliderHandle> collidersToDestroy;

		private List<ObiForceZoneHandle> forceZonesToCreate;

		private List<ObiForceZoneHandle> forceZonesToDestroy;

		private List<ObiRigidbodyHandle> rigidbodiesToCreate;

		private List<ObiRigidbodyHandle> rigidbodiesToDestroy;

		private bool dirty;

		private static ObiColliderWorld instance;

		public int collidersToUpdateCount { get; private set; }

		public static ObiColliderWorld GetInstance()
		{
			if (instance == null)
			{
				instance = new ObiColliderWorld();
				instance.Initialize();
			}
			return instance;
		}

		private void Initialize()
		{
			if (implementations == null)
			{
				implementations = new List<IColliderWorldImpl>();
			}
			if (colliderHandles == null)
			{
				colliderHandles = new List<ObiColliderHandle>();
			}
			if (colliderShapes == null)
			{
				colliderShapes = new ObiNativeColliderShapeList();
			}
			if (colliderAabbs == null)
			{
				colliderAabbs = new ObiNativeAabbList();
			}
			if (colliderTransforms == null)
			{
				colliderTransforms = new ObiNativeAffineTransformList();
			}
			if (forceZoneHandles == null)
			{
				forceZoneHandles = new List<ObiForceZoneHandle>();
			}
			if (forceZones == null)
			{
				forceZones = new ObiNativeForceZoneList();
			}
			if (materialHandles == null)
			{
				materialHandles = new List<ObiCollisionMaterialHandle>();
			}
			if (collisionMaterials == null)
			{
				collisionMaterials = new ObiNativeCollisionMaterialList();
			}
			if (rigidbodyHandles == null)
			{
				rigidbodyHandles = new List<ObiRigidbodyHandle>();
			}
			if (rigidbodies == null)
			{
				rigidbodies = new ObiNativeRigidbodyList();
			}
			if (triangleMeshContainer == null)
			{
				triangleMeshContainer = new ObiTriangleMeshContainer();
			}
			if (edgeMeshContainer == null)
			{
				edgeMeshContainer = new ObiEdgeMeshContainer();
			}
			if (distanceFieldContainer == null)
			{
				distanceFieldContainer = new ObiDistanceFieldContainer();
			}
			if (heightFieldContainer == null)
			{
				heightFieldContainer = new ObiHeightFieldContainer();
			}
			if (collidersToCreate == null)
			{
				collidersToCreate = new List<ObiColliderHandle>();
			}
			if (collidersToDestroy == null)
			{
				collidersToDestroy = new List<ObiColliderHandle>();
			}
			if (forceZonesToCreate == null)
			{
				forceZonesToCreate = new List<ObiForceZoneHandle>();
			}
			if (forceZonesToDestroy == null)
			{
				forceZonesToDestroy = new List<ObiForceZoneHandle>();
			}
			if (rigidbodiesToCreate == null)
			{
				rigidbodiesToCreate = new List<ObiRigidbodyHandle>();
			}
			if (rigidbodiesToDestroy == null)
			{
				rigidbodiesToDestroy = new List<ObiRigidbodyHandle>();
			}
		}

		private void Destroy()
		{
			dirty = false;
			for (int i = 0; i < implementations.Count; i++)
			{
				implementations[i].SetColliders(colliderShapes, colliderAabbs, colliderTransforms);
				implementations[i].UpdateWorld(0f);
			}
			if (colliderHandles != null)
			{
				foreach (ObiColliderHandle colliderHandle in colliderHandles)
				{
					colliderHandle.Invalidate();
				}
			}
			if (rigidbodyHandles != null)
			{
				foreach (ObiRigidbodyHandle rigidbodyHandle in rigidbodyHandles)
				{
					rigidbodyHandle.Invalidate();
				}
			}
			if (materialHandles != null)
			{
				foreach (ObiCollisionMaterialHandle materialHandle in materialHandles)
				{
					materialHandle.Invalidate();
				}
			}
			if (forceZoneHandles != null)
			{
				foreach (ObiForceZoneHandle forceZoneHandle in forceZoneHandles)
				{
					forceZoneHandle.Invalidate();
				}
			}
			implementations = null;
			colliderHandles = null;
			rigidbodyHandles = null;
			materialHandles = null;
			forceZoneHandles = null;
			collidersToCreate = null;
			collidersToDestroy = null;
			forceZonesToCreate = null;
			forceZonesToDestroy = null;
			rigidbodiesToCreate = null;
			rigidbodiesToDestroy = null;
			colliderShapes?.Dispose();
			colliderAabbs?.Dispose();
			colliderTransforms?.Dispose();
			forceZones?.Dispose();
			collisionMaterials?.Dispose();
			rigidbodies?.Dispose();
			triangleMeshContainer?.Dispose();
			edgeMeshContainer?.Dispose();
			distanceFieldContainer?.Dispose();
			heightFieldContainer?.Dispose();
			instance = null;
		}

		private void DestroyIfUnused()
		{
			if (colliderHandles.Count == 0 && rigidbodyHandles.Count == 0 && forceZoneHandles.Count == 0 && implementations.Count == 0)
			{
				Destroy();
			}
		}

		public void RegisterImplementation(IColliderWorldImpl impl)
		{
			if (!implementations.Contains(impl))
			{
				implementations.Add(impl);
			}
		}

		public void UnregisterImplementation(IColliderWorldImpl impl)
		{
			implementations.Remove(impl);
			DestroyIfUnused();
		}

		public ObiColliderHandle CreateCollider()
		{
			ObiColliderHandle obiColliderHandle = new ObiColliderHandle();
			if (!Application.isPlaying)
			{
				CreateColliderData(obiColliderHandle);
			}
			else
			{
				collidersToCreate.Add(obiColliderHandle);
			}
			return obiColliderHandle;
		}

		public ObiForceZoneHandle CreateForceZone()
		{
			ObiForceZoneHandle obiForceZoneHandle = new ObiForceZoneHandle();
			if (!Application.isPlaying)
			{
				CreateForceZoneData(obiForceZoneHandle);
			}
			else
			{
				forceZonesToCreate.Add(obiForceZoneHandle);
			}
			return obiForceZoneHandle;
		}

		public ObiRigidbodyHandle CreateRigidbody()
		{
			ObiRigidbodyHandle obiRigidbodyHandle = new ObiRigidbodyHandle();
			if (!Application.isPlaying)
			{
				CreateRigidbodyData(obiRigidbodyHandle);
			}
			else
			{
				rigidbodiesToCreate.Add(obiRigidbodyHandle);
			}
			return obiRigidbodyHandle;
		}

		public ObiCollisionMaterialHandle CreateCollisionMaterial()
		{
			ObiCollisionMaterialHandle obiCollisionMaterialHandle = new ObiCollisionMaterialHandle(materialHandles.Count);
			materialHandles.Add(obiCollisionMaterialHandle);
			collisionMaterials.Add(default(CollisionMaterial));
			return obiCollisionMaterialHandle;
		}

		public ObiTriangleMeshHandle GetOrCreateTriangleMesh(Mesh mesh)
		{
			return triangleMeshContainer.GetOrCreateTriangleMesh(mesh);
		}

		public void DestroyTriangleMesh(ObiTriangleMeshHandle meshHandle)
		{
			triangleMeshContainer.DestroyTriangleMesh(meshHandle);
		}

		public ObiEdgeMeshHandle GetOrCreateEdgeMesh(EdgeCollider2D collider)
		{
			return edgeMeshContainer.GetOrCreateEdgeMesh(collider);
		}

		public void DestroyEdgeMesh(ObiEdgeMeshHandle meshHandle)
		{
			edgeMeshContainer.DestroyEdgeMesh(meshHandle);
		}

		public ObiDistanceFieldHandle GetOrCreateDistanceField(ObiDistanceField df)
		{
			return distanceFieldContainer.GetOrCreateDistanceField(df);
		}

		public void DestroyDistanceField(ObiDistanceFieldHandle dfHandle)
		{
			distanceFieldContainer.DestroyDistanceField(dfHandle);
		}

		public ObiHeightFieldHandle GetOrCreateHeightField(TerrainData hf)
		{
			return heightFieldContainer.GetOrCreateHeightField(hf);
		}

		public void DestroyHeightField(ObiHeightFieldHandle hfHandle)
		{
			heightFieldContainer.DestroyHeightField(hfHandle);
		}

		public void DestroyCollider(ObiColliderHandle handle)
		{
			if (!Application.isPlaying || implementations.Count == 0)
			{
				DestroyColliderData(handle);
			}
			else if (!collidersToCreate.Remove(handle))
			{
				collidersToDestroy.Add(handle);
			}
		}

		public void DestroyForceZone(ObiForceZoneHandle handle)
		{
			if (!Application.isPlaying || implementations.Count == 0)
			{
				DestroyForceZoneData(handle);
			}
			else if (!forceZonesToCreate.Remove(handle))
			{
				forceZonesToDestroy.Add(handle);
			}
		}

		public void DestroyRigidbody(ObiRigidbodyHandle handle)
		{
			if (!Application.isPlaying || implementations.Count == 0)
			{
				DestroyRigidbodyData(handle);
			}
			else if (!rigidbodiesToCreate.Remove(handle))
			{
				rigidbodiesToDestroy.Add(handle);
			}
		}

		public void DestroyCollisionMaterial(ObiCollisionMaterialHandle handle)
		{
			if (collisionMaterials != null && handle != null && handle.isValid && handle.index < materialHandles.Count)
			{
				int index = handle.index;
				int num = materialHandles.Count - 1;
				materialHandles.Swap(index, num);
				collisionMaterials.Swap(index, num);
				materialHandles[index].index = index;
				handle.Invalidate();
				materialHandles.RemoveAt(num);
				collisionMaterials.count--;
				DestroyIfUnused();
			}
		}

		private void DestroyColliderData(ObiColliderHandle handle)
		{
			if (colliderShapes != null && handle != null && handle.isValid && handle.index < colliderHandles.Count)
			{
				int index = handle.index;
				int num = colliderHandles.Count - 1;
				colliderHandles.Swap(index, num);
				colliderShapes.Swap(index, num);
				colliderAabbs.Swap(index, num);
				colliderTransforms.Swap(index, num);
				colliderHandles[index].index = index;
				handle.Invalidate();
				colliderHandles.RemoveAt(num);
				colliderShapes.count--;
				colliderAabbs.count--;
				colliderTransforms.count--;
				collidersToUpdateCount = colliderHandles.Count;
				DestroyIfUnused();
			}
		}

		private void DestroyForceZoneData(ObiForceZoneHandle handle)
		{
			if (forceZones != null && handle != null && handle.isValid && handle.index < forceZoneHandles.Count)
			{
				int index = handle.index;
				int num = forceZoneHandles.Count - 1;
				forceZoneHandles.Swap(index, num);
				forceZones.Swap(index, num);
				forceZoneHandles[index].index = index;
				handle.Invalidate();
				forceZoneHandles.RemoveAt(num);
				forceZones.count--;
				DestroyIfUnused();
			}
		}

		private void DestroyRigidbodyData(ObiRigidbodyHandle handle)
		{
			if (rigidbodies != null && handle != null && handle.isValid && handle.index < rigidbodyHandles.Count)
			{
				int index = handle.index;
				int num = rigidbodyHandles.Count - 1;
				rigidbodyHandles.Swap(index, num);
				rigidbodies.Swap(index, num);
				rigidbodyHandles[index].index = index;
				handle.Invalidate();
				rigidbodyHandles.RemoveAt(num);
				rigidbodies.count--;
				DestroyIfUnused();
			}
		}

		private void CreateColliderData(ObiColliderHandle handle)
		{
			handle.index = colliderHandles.Count;
			colliderHandles.Add(handle);
			colliderShapes.Add(new ColliderShape
			{
				materialIndex = -1,
				rigidbodyIndex = -1,
				dataIndex = -1
			});
			colliderAabbs.Add(default(Aabb));
			colliderTransforms.Add(default(AffineTransform));
			MarkColliderAsNeedingUpdate(handle);
		}

		private void CreateForceZoneData(ObiForceZoneHandle handle)
		{
			handle.index = forceZoneHandles.Count;
			forceZoneHandles.Add(handle);
			forceZones.Add(default(ForceZone));
		}

		private void CreateRigidbodyData(ObiRigidbodyHandle handle)
		{
			handle.index = rigidbodyHandles.Count;
			rigidbodyHandles.Add(handle);
			rigidbodies.Add(default(ColliderRigidbody));
		}

		public bool DoesColliderRequireToBeUpdated(ObiColliderHandle handle)
		{
			if (handle != null && handle.isValid && handle.index < colliderHandles.Count && handle.index < collidersToUpdateCount)
			{
				return true;
			}
			return false;
		}

		public void MarkColliderAsNeedingUpdate(ObiColliderHandle handle)
		{
			if (colliderShapes != null && handle != null && handle.isValid && handle.index < colliderHandles.Count && handle.index >= collidersToUpdateCount && collidersToUpdateCount < colliderHandles.Count)
			{
				int index = handle.index;
				int num = collidersToUpdateCount;
				colliderHandles.Swap(index, num);
				colliderShapes.Swap(index, num);
				colliderAabbs.Swap(index, num);
				colliderTransforms.Swap(index, num);
				colliderHandles[num].index = num;
				colliderHandles[index].index = index;
				collidersToUpdateCount++;
			}
		}

		public void MarkColliderAsNotNeedingUpdate(ObiColliderHandle handle)
		{
			if (colliderShapes != null && handle != null && handle.isValid && handle.index < colliderHandles.Count && handle.index < collidersToUpdateCount)
			{
				int index = handle.index;
				int num = collidersToUpdateCount - 1;
				colliderHandles.Swap(index, num);
				colliderShapes.Swap(index, num);
				colliderAabbs.Swap(index, num);
				colliderTransforms.Swap(index, num);
				colliderHandles[num].index = num;
				colliderHandles[index].index = index;
				collidersToUpdateCount--;
			}
		}

		public void FlushHandleBuffers()
		{
			if (collidersToDestroy != null)
			{
				foreach (ObiColliderHandle item in collidersToDestroy)
				{
					DestroyColliderData(item);
				}
				collidersToDestroy?.Clear();
			}
			if (forceZonesToDestroy != null)
			{
				foreach (ObiForceZoneHandle item2 in forceZonesToDestroy)
				{
					DestroyForceZoneData(item2);
				}
				forceZonesToDestroy?.Clear();
			}
			if (rigidbodiesToDestroy != null)
			{
				foreach (ObiRigidbodyHandle item3 in rigidbodiesToDestroy)
				{
					DestroyRigidbodyData(item3);
				}
				rigidbodiesToDestroy?.Clear();
			}
			if (collidersToCreate != null)
			{
				foreach (ObiColliderHandle item4 in collidersToCreate)
				{
					CreateColliderData(item4);
				}
				collidersToCreate?.Clear();
			}
			if (forceZonesToCreate != null)
			{
				foreach (ObiForceZoneHandle item5 in forceZonesToCreate)
				{
					CreateForceZoneData(item5);
				}
				forceZonesToCreate?.Clear();
			}
			if (rigidbodiesToCreate == null)
			{
				return;
			}
			foreach (ObiRigidbodyHandle item6 in rigidbodiesToCreate)
			{
				CreateRigidbodyData(item6);
			}
			rigidbodiesToCreate?.Clear();
		}

		public void UpdateWorld(float deltaTime, bool updateDynamics = true)
		{
			if (!dirty)
			{
				return;
			}
			dirty = false;
			FlushHandleBuffers();
			if (colliderHandles != null)
			{
				for (int i = 0; i < collidersToUpdateCount; i++)
				{
					colliderHandles[i].owner.UpdateIfNeeded();
				}
			}
			if (forceZoneHandles != null)
			{
				for (int j = 0; j < forceZoneHandles.Count; j++)
				{
					forceZoneHandles[j].owner.UpdateIfNeeded();
				}
			}
			if (rigidbodyHandles != null && updateDynamics)
			{
				for (int k = 0; k < rigidbodyHandles.Count; k++)
				{
					rigidbodyHandles[k].owner.UpdateIfNeeded(deltaTime);
				}
			}
			if (implementations == null)
			{
				return;
			}
			for (int l = 0; l < implementations.Count; l++)
			{
				if (implementations[l].referenceCount > 0)
				{
					implementations[l].SetColliders(colliderShapes, colliderAabbs, colliderTransforms);
					implementations[l].SetForceZones(forceZones);
					implementations[l].SetRigidbodies(rigidbodies);
					implementations[l].SetCollisionMaterials(collisionMaterials);
					implementations[l].SetTriangleMeshData(triangleMeshContainer.headers, triangleMeshContainer.bihNodes, triangleMeshContainer.triangles, triangleMeshContainer.vertices);
					implementations[l].SetEdgeMeshData(edgeMeshContainer.headers, edgeMeshContainer.bihNodes, edgeMeshContainer.edges, edgeMeshContainer.vertices);
					implementations[l].SetDistanceFieldData(distanceFieldContainer.headers, distanceFieldContainer.dfNodes);
					implementations[l].SetHeightFieldData(heightFieldContainer.headers, heightFieldContainer.samples);
					if (updateDynamics)
					{
						implementations[l].UpdateWorld(deltaTime);
					}
				}
			}
		}

		public void SetDirty()
		{
			dirty = true;
		}

		public void UpdateCollisionMaterials()
		{
			if (implementations == null)
			{
				return;
			}
			for (int i = 0; i < implementations.Count; i++)
			{
				if (implementations[i].referenceCount > 0)
				{
					implementations[i].SetCollisionMaterials(collisionMaterials);
				}
			}
		}

		public void UpdateRigidbodyVelocities(ObiSolver solver)
		{
			if (solver != null && solver.initialized)
			{
				int num = Mathf.Min(rigidbodyHandles.Count, solver.rigidbodyLinearDeltas.count);
				for (int i = 0; i < num; i++)
				{
					rigidbodyHandles[i].owner.UpdateVelocities(solver.rigidbodyLinearDeltas[i], solver.rigidbodyAngularDeltas[i]);
				}
			}
			solver.rigidbodyLinearDeltas.WipeToZero();
			solver.rigidbodyAngularDeltas.WipeToZero();
			solver.rigidbodyLinearDeltas.Upload();
			solver.rigidbodyAngularDeltas.Upload();
		}
	}
}
