using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Rope Mesh Renderer", 886)]
	[ExecuteInEditMode]
	[RequireComponent(typeof(ObiPathSmoother))]
	public class ObiRopeMeshRenderer : MonoBehaviour, ObiActorRenderer<ObiRopeMeshRenderer>, IActorRenderer, ObiRenderer<ObiRopeMeshRenderer>, IMeshDataProvider
	{
		public RenderBatchParams renderParameters = new RenderBatchParams(receiveShadow: true);

		public ObiPathFrame.Axis axis;

		public float volumeScaling;

		public bool stretchWithRope = true;

		public bool spanEntireLength = true;

		public uint instances = 1u;

		public float instanceSpacing;

		public float offset;

		public Vector3 scale = Vector3.one;

		public Renderer sourceRenderer { get; protected set; }

		public ObiActor actor { get; private set; }

		public uint meshInstances => instances;

		[field: SerializeField]
		public Mesh sourceMesh { get; set; }

		[field: SerializeField]
		public Material[] materials { get; set; }

		public virtual int vertexCount
		{
			get
			{
				if (!sourceMesh)
				{
					return 0;
				}
				return sourceMesh.vertexCount;
			}
		}

		public virtual int triangleCount
		{
			get
			{
				if (!sourceMesh)
				{
					return 0;
				}
				return sourceMesh.triangles.Length / 3;
			}
		}

		public void Awake()
		{
			actor = GetComponent<ObiActor>();
			sourceRenderer = GetComponent<MeshRenderer>();
		}

		public void OnEnable()
		{
			((ObiActorRenderer<ObiRopeMeshRenderer>)this).EnableRenderer();
		}

		public void OnDisable()
		{
			((ObiActorRenderer<ObiRopeMeshRenderer>)this).DisableRenderer();
		}

		public void OnValidate()
		{
			((ObiActorRenderer<ObiRopeMeshRenderer>)this).SetRendererDirty(Oni.RenderingSystemType.MeshRope);
		}

		RenderSystem<ObiRopeMeshRenderer> ObiRenderer<ObiRopeMeshRenderer>.CreateRenderSystem(ObiSolver solver)
		{
			ObiSolver.BackendType backendType = solver.backendType;
			if (backendType != ObiSolver.BackendType.Compute && backendType == ObiSolver.BackendType.Burst)
			{
				return new BurstMeshRopeRenderSystem(solver);
			}
			if (SystemInfo.supportsComputeShaders)
			{
				return new ComputeMeshRopeRenderSystem(solver);
			}
			return null;
		}

		public virtual void GetVertices(List<Vector3> vertices)
		{
			sourceMesh.GetVertices(vertices);
		}

		public virtual void GetNormals(List<Vector3> normals)
		{
			sourceMesh.GetNormals(normals);
		}

		public virtual void GetTangents(List<Vector4> tangents)
		{
			sourceMesh.GetTangents(tangents);
		}

		public virtual void GetColors(List<Color> colors)
		{
			sourceMesh.GetColors(colors);
		}

		public virtual void GetUVs(int channel, List<Vector2> uvs)
		{
			sourceMesh.GetUVs(channel, uvs);
		}

		public virtual void GetTriangles(List<int> triangles)
		{
			triangles.Clear();
			triangles.AddRange(sourceMesh.triangles);
		}
	}
}
