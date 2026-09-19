using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Rope Line Renderer", 884)]
	[ExecuteInEditMode]
	[RequireComponent(typeof(ObiPathSmoother))]
	public class ObiRopeLineRenderer : MonoBehaviour, ObiActorRenderer<ObiRopeLineRenderer>, IActorRenderer, ObiRenderer<ObiRopeLineRenderer>
	{
		public Material material;

		public RenderBatchParams renderParams = new RenderBatchParams(receiveShadow: true);

		[Range(0f, 1f)]
		public float uvAnchor;

		public Vector2 uvScale = Vector2.one;

		public bool normalizeV = true;

		public float thicknessScale = 0.8f;

		public ObiActor actor { get; private set; }

		public void Awake()
		{
			actor = GetComponent<ObiActor>();
		}

		private void OnEnable()
		{
			((ObiActorRenderer<ObiRopeLineRenderer>)this).EnableRenderer();
		}

		private void OnDisable()
		{
			((ObiActorRenderer<ObiRopeLineRenderer>)this).DisableRenderer();
		}

		public void OnValidate()
		{
			((ObiActorRenderer<ObiRopeLineRenderer>)this).SetRendererDirty(Oni.RenderingSystemType.LineRope);
		}

		RenderSystem<ObiRopeLineRenderer> ObiRenderer<ObiRopeLineRenderer>.CreateRenderSystem(ObiSolver solver)
		{
			ObiSolver.BackendType backendType = solver.backendType;
			if (backendType != ObiSolver.BackendType.Compute && backendType == ObiSolver.BackendType.Burst)
			{
				return new BurstLineRopeRenderSystem(solver);
			}
			if (SystemInfo.supportsComputeShaders)
			{
				return new ComputeLineRopeRenderSystem(solver);
			}
			return null;
		}
	}
}
