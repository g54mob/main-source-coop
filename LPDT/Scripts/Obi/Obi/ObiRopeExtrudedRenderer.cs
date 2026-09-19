using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Rope Extruded Renderer", 883)]
	[ExecuteInEditMode]
	[RequireComponent(typeof(ObiPathSmoother))]
	public class ObiRopeExtrudedRenderer : MonoBehaviour, ObiActorRenderer<ObiRopeExtrudedRenderer>, IActorRenderer, ObiRenderer<ObiRopeExtrudedRenderer>
	{
		public Material material;

		public RenderBatchParams renderParameters = new RenderBatchParams(receiveShadow: true);

		[Range(0f, 1f)]
		public float uvAnchor;

		public Vector2 uvScale = Vector2.one;

		public bool normalizeV = true;

		public ObiRopeSection section;

		public float thicknessScale = 0.8f;

		public ObiPathSmoother smoother { get; private set; }

		public ObiActor actor { get; private set; }

		public void Awake()
		{
			actor = GetComponent<ObiActor>();
		}

		public void OnEnable()
		{
			smoother = GetComponent<ObiPathSmoother>();
			((ObiActorRenderer<ObiRopeExtrudedRenderer>)this).EnableRenderer();
		}

		public void OnDisable()
		{
			((ObiActorRenderer<ObiRopeExtrudedRenderer>)this).DisableRenderer();
		}

		public void OnValidate()
		{
			((ObiActorRenderer<ObiRopeExtrudedRenderer>)this).SetRendererDirty(Oni.RenderingSystemType.AllSmoothedRopes);
		}

		RenderSystem<ObiRopeExtrudedRenderer> ObiRenderer<ObiRopeExtrudedRenderer>.CreateRenderSystem(ObiSolver solver)
		{
			ObiSolver.BackendType backendType = solver.backendType;
			if (backendType != ObiSolver.BackendType.Compute && backendType == ObiSolver.BackendType.Burst)
			{
				return new BurstExtrudedRopeRenderSystem(solver);
			}
			if (SystemInfo.supportsComputeShaders)
			{
				return new ComputeExtrudedRopeRenderSystem(solver);
			}
			return null;
		}
	}
}
