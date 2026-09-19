using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Foam Generator", 1000)]
	[ExecuteInEditMode]
	[RequireComponent(typeof(ObiActor))]
	[DisallowMultipleComponent]
	public class ObiFoamGenerator : MonoBehaviour, ObiActorRenderer<ObiFoamGenerator>, IActorRenderer, ObiRenderer<ObiFoamGenerator>
	{
		[Header("Foam spawning")]
		public float foamGenerationRate = 100f;

		public float foamPotential = 50f;

		[Range(0f, 1f)]
		public float foamPotentialDiffusion = 0.95f;

		public Vector2 velocityRange = new Vector2(2f, 4f);

		public Vector2 vorticityRange = new Vector2(4f, 8f);

		[Header("Foam properties")]
		public Light volumeLight;

		public Color color = new Color(1f, 1f, 1f, 0.25f);

		public float size = 0.02f;

		[Range(0f, 1f)]
		public float sizeRandom = 0.2f;

		public float lifetime = 5f;

		[Range(0f, 1f)]
		public float lifetimeRandom = 0.2f;

		public float buoyancy = 0.5f;

		[Range(0f, 1f)]
		public float drag = 0.5f;

		[Range(0f, 1f)]
		public float atmosphericDrag = 0.5f;

		[Range(1f, 50f)]
		public float airAging = 2f;

		[Range(0f, 1f)]
		public float isosurface = 0.02f;

		[Header("Density Control (Compute only)")]
		[Range(0f, 1f)]
		public float pressure = 1f;

		[Range(0f, 1f)]
		public float density = 0.3f;

		[Range(1f, 4f)]
		public float smoothingRadius = 2.5f;

		[Range(0f, 1f)]
		public float viscosity = 0.5f;

		[Min(0f)]
		public float surfaceTension = 2f;

		public ObiActor actor { get; private set; }

		public void Awake()
		{
			actor = GetComponent<ObiActor>();
		}

		public void OnEnable()
		{
			((ObiActorRenderer<ObiFoamGenerator>)this).EnableRenderer();
		}

		public void OnDisable()
		{
			((ObiActorRenderer<ObiFoamGenerator>)this).DisableRenderer();
		}

		public void OnValidate()
		{
			((ObiActorRenderer<ObiFoamGenerator>)this).SetRendererDirty(Oni.RenderingSystemType.FoamParticles);
		}

		RenderSystem<ObiFoamGenerator> ObiRenderer<ObiFoamGenerator>.CreateRenderSystem(ObiSolver solver)
		{
			ObiSolver.BackendType backendType = solver.backendType;
			if (backendType != ObiSolver.BackendType.Compute && backendType == ObiSolver.BackendType.Burst)
			{
				return new BurstFoamRenderSystem(solver);
			}
			if (SystemInfo.supportsComputeShaders)
			{
				return new ComputeFoamRenderSystem(solver);
			}
			return null;
		}
	}
}
