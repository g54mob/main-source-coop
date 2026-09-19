using UnityEngine;

namespace Obi
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(ObiRopeBase))]
	public class ObiPathSmoother : MonoBehaviour, ObiActorRenderer<ObiPathSmoother>, IActorRenderer, ObiRenderer<ObiPathSmoother>
	{
		[Range(0f, 1f)]
		[Tooltip("Curvature threshold below which the path will be decimated. A value of 0 won't apply any decimation. As you increase the value, decimation will become more aggresive.")]
		public float decimation;

		[Range(0f, 3f)]
		[Tooltip("Smoothing iterations applied to the path. A smoothing value of 0 won't perform any smoothing at all. Note that smoothing is applied after decimation.")]
		public uint smoothing;

		[Tooltip("Twist in degrees applied to each sucessive path section.")]
		public float twist;

		[HideInInspector]
		public int indexInSystem;

		public ObiActor actor { get; private set; }

		public float SmoothLength
		{
			get
			{
				if (actor.isLoaded && actor.solver.GetRenderSystem<ObiPathSmoother>() is ObiPathSmootherRenderSystem obiPathSmootherRenderSystem)
				{
					return obiPathSmootherRenderSystem.GetSmoothLength(indexInSystem);
				}
				return 0f;
			}
		}

		public float SmoothSections
		{
			get
			{
				if (actor.isLoaded && actor.solver.GetRenderSystem<ObiPathSmoother>() is ObiPathSmootherRenderSystem obiPathSmootherRenderSystem)
				{
					return obiPathSmootherRenderSystem.GetSmoothFrameCount(indexInSystem);
				}
				return 0f;
			}
		}

		public void OnEnable()
		{
			actor = GetComponent<ObiActor>();
			((ObiActorRenderer<ObiPathSmoother>)this).EnableRenderer();
		}

		private void OnDisable()
		{
			((ObiActorRenderer<ObiPathSmoother>)this).DisableRenderer();
		}

		private void OnValidate()
		{
			((ObiActorRenderer<ObiPathSmoother>)this).SetRendererDirty(Oni.RenderingSystemType.AllSmoothedRopes);
		}

		public ObiPathFrame GetSectionAt(float mu)
		{
			if (actor.isLoaded && actor.solver.GetRenderSystem<ObiPathSmoother>() is ObiPathSmootherRenderSystem obiPathSmootherRenderSystem)
			{
				return obiPathSmootherRenderSystem.GetFrameAt(indexInSystem, mu);
			}
			return ObiPathFrame.Identity;
		}

		RenderSystem<ObiPathSmoother> ObiRenderer<ObiPathSmoother>.CreateRenderSystem(ObiSolver solver)
		{
			ObiSolver.BackendType backendType = solver.backendType;
			if (backendType != ObiSolver.BackendType.Compute && backendType == ObiSolver.BackendType.Burst)
			{
				return new BurstPathSmootherRenderSystem(solver);
			}
			if (SystemInfo.supportsComputeShaders)
			{
				return new ComputePathSmootherRenderSystem(solver);
			}
			return null;
		}
	}
}
