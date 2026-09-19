using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Rope Chain Renderer", 885)]
	[ExecuteInEditMode]
	public class ObiRopeChainRenderer : MonoBehaviour, ObiActorRenderer<ObiRopeChainRenderer>, IActorRenderer, ObiRenderer<ObiRopeChainRenderer>
	{
		[Serializable]
		public struct LinkModifier
		{
			public Vector3 translation;

			public Vector3 scale;

			public Vector3 rotation;

			public void Clear()
			{
				translation = Vector3.zero;
				scale = Vector3.one;
				rotation = Vector3.zero;
			}
		}

		public Mesh linkMesh;

		public Material linkMaterial;

		public Vector3 linkScale = Vector3.one;

		[Range(0f, 1f)]
		public float twistAnchor;

		public float linkTwist;

		public List<LinkModifier> linkModifiers = new List<LinkModifier>();

		public RenderBatchParams renderParameters = new RenderBatchParams(receiveShadow: true);

		public ObiActor actor { get; private set; }

		private void Awake()
		{
			actor = GetComponent<ObiActor>();
		}

		public void OnEnable()
		{
			((ObiActorRenderer<ObiRopeChainRenderer>)this).EnableRenderer();
		}

		public void OnDisable()
		{
			((ObiActorRenderer<ObiRopeChainRenderer>)this).DisableRenderer();
		}

		public void OnValidate()
		{
			((ObiActorRenderer<ObiRopeChainRenderer>)this).SetRendererDirty(Oni.RenderingSystemType.ChainRope);
		}

		RenderSystem<ObiRopeChainRenderer> ObiRenderer<ObiRopeChainRenderer>.CreateRenderSystem(ObiSolver solver)
		{
			ObiSolver.BackendType backendType = solver.backendType;
			if (backendType != ObiSolver.BackendType.Compute && backendType == ObiSolver.BackendType.Burst)
			{
				return new BurstChainRopeRenderSystem(solver);
			}
			if (SystemInfo.supportsComputeShaders)
			{
				return new ComputeChainRopeRenderSystem(solver);
			}
			return null;
		}
	}
}
