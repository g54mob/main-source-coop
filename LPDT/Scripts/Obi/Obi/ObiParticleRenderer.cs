using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Particle Renderer", 1000)]
	[ExecuteInEditMode]
	[RequireComponent(typeof(ObiActor))]
	public class ObiParticleRenderer : MonoBehaviour, IParticleRenderer, ObiActorRenderer<ObiParticleRenderer>, IActorRenderer, ObiRenderer<ObiParticleRenderer>
	{
		public Material material;

		public RenderBatchParams renderParameters = new RenderBatchParams(receiveShadow: true);

		[field: SerializeField]
		public Color particleColor { get; set; } = Color.white;

		[field: SerializeField]
		public float radiusScale { get; set; } = 1f;

		public ObiActor actor { get; private set; }

		public void Awake()
		{
			actor = GetComponent<ObiActor>();
		}

		public void OnEnable()
		{
			((ObiActorRenderer<ObiParticleRenderer>)this).EnableRenderer();
		}

		public void OnDisable()
		{
			((ObiActorRenderer<ObiParticleRenderer>)this).DisableRenderer();
		}

		public void OnValidate()
		{
			((ObiActorRenderer<ObiParticleRenderer>)this).SetRendererDirty(Oni.RenderingSystemType.Particles);
		}

		RenderSystem<ObiParticleRenderer> ObiRenderer<ObiParticleRenderer>.CreateRenderSystem(ObiSolver solver)
		{
			ObiSolver.BackendType backendType = solver.backendType;
			if (backendType != ObiSolver.BackendType.Compute && backendType == ObiSolver.BackendType.Burst)
			{
				return new BurstParticleRenderSystem(solver);
			}
			if (SystemInfo.supportsComputeShaders)
			{
				return new ComputeParticleRenderSystem(solver);
			}
			return null;
		}
	}
}
