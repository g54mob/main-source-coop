using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Instanced Particle Renderer", 1001)]
	[ExecuteInEditMode]
	[RequireComponent(typeof(ObiActor))]
	public class ObiInstancedParticleRenderer : MonoBehaviour, ObiActorRenderer<ObiInstancedParticleRenderer>, IActorRenderer, ObiRenderer<ObiInstancedParticleRenderer>
	{
		public Mesh mesh;

		public Material material;

		public RenderBatchParams renderParameters = new RenderBatchParams(receiveShadow: true);

		public Color instanceColor = Color.white;

		public float instanceScale = 1f;

		public ObiActor actor { get; private set; }

		private void Awake()
		{
			actor = GetComponent<ObiActor>();
		}

		public void OnEnable()
		{
			((ObiActorRenderer<ObiInstancedParticleRenderer>)this).EnableRenderer();
		}

		public void OnDisable()
		{
			((ObiActorRenderer<ObiInstancedParticleRenderer>)this).DisableRenderer();
		}

		public void OnValidate()
		{
			((ObiActorRenderer<ObiInstancedParticleRenderer>)this).SetRendererDirty(Oni.RenderingSystemType.InstancedParticles);
		}

		RenderSystem<ObiInstancedParticleRenderer> ObiRenderer<ObiInstancedParticleRenderer>.CreateRenderSystem(ObiSolver solver)
		{
			ObiSolver.BackendType backendType = solver.backendType;
			if (backendType != ObiSolver.BackendType.Compute && backendType == ObiSolver.BackendType.Burst)
			{
				return new BurstInstancedParticleRenderSystem(solver);
			}
			if (SystemInfo.supportsComputeShaders)
			{
				return new ComputeInstancedParticleRenderSystem(solver);
			}
			return null;
		}
	}
}
