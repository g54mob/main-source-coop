using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Rope", 880)]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class ObiRope : ObiRopeBase, IDistanceConstraintsUser, IBendConstraintsUser
	{
		public delegate void RopeTornCallback(ObiRope rope, ObiRopeTornEventArgs tearInfo);

		public class ObiRopeTornEventArgs
		{
			public ObiStructuralElement element;

			public int particleIndex;

			public ObiRopeTornEventArgs(ObiStructuralElement element, int particle)
			{
				this.element = element;
				particleIndex = particle;
			}
		}

		[SerializeField]
		protected ObiRopeBlueprint m_RopeBlueprint;

		private ObiRopeBlueprint m_RopeBlueprintInstance;

		public bool tearingEnabled;

		public float tearResistanceMultiplier = 1000f;

		public int tearRate = 1;

		[SerializeField]
		protected bool _distanceConstraintsEnabled = true;

		[SerializeField]
		protected float _stretchingScale = 1f;

		[SerializeField]
		protected float _stretchCompliance;

		[SerializeField]
		[Range(0f, 1f)]
		protected float _maxCompression;

		[SerializeField]
		protected bool _bendConstraintsEnabled = true;

		[SerializeField]
		protected float _bendCompliance;

		[SerializeField]
		[Range(0f, 0.5f)]
		protected float _maxBending = 0.025f;

		[SerializeField]
		[Range(0f, 0.1f)]
		protected float _plasticYield;

		[SerializeField]
		protected float _plasticCreep;

		private List<ObiStructuralElement> tornElements = new List<ObiStructuralElement>();

		public bool selfCollisions
		{
			get
			{
				return m_SelfCollisions;
			}
			set
			{
				if (value != m_SelfCollisions)
				{
					m_SelfCollisions = value;
					SetSelfCollisions(selfCollisions);
				}
			}
		}

		public bool distanceConstraintsEnabled
		{
			get
			{
				return _distanceConstraintsEnabled;
			}
			set
			{
				if (value != _distanceConstraintsEnabled)
				{
					_distanceConstraintsEnabled = value;
					SetConstraintsDirty(Oni.ConstraintType.Distance);
				}
			}
		}

		public float stretchingScale
		{
			get
			{
				return _stretchingScale;
			}
			set
			{
				_stretchingScale = value;
				SetConstraintsDirty(Oni.ConstraintType.Distance);
			}
		}

		public float stretchCompliance
		{
			get
			{
				return _stretchCompliance;
			}
			set
			{
				_stretchCompliance = value;
				SetConstraintsDirty(Oni.ConstraintType.Distance);
			}
		}

		public float maxCompression
		{
			get
			{
				return _maxCompression;
			}
			set
			{
				_maxCompression = value;
				SetConstraintsDirty(Oni.ConstraintType.Distance);
			}
		}

		public bool bendConstraintsEnabled
		{
			get
			{
				return _bendConstraintsEnabled;
			}
			set
			{
				if (value != _bendConstraintsEnabled)
				{
					_bendConstraintsEnabled = value;
					SetConstraintsDirty(Oni.ConstraintType.Bending);
				}
			}
		}

		public float bendCompliance
		{
			get
			{
				return _bendCompliance;
			}
			set
			{
				_bendCompliance = value;
				SetConstraintsDirty(Oni.ConstraintType.Bending);
			}
		}

		public float maxBending
		{
			get
			{
				return _maxBending;
			}
			set
			{
				_maxBending = value;
				SetConstraintsDirty(Oni.ConstraintType.Bending);
			}
		}

		public float plasticYield
		{
			get
			{
				return _plasticYield;
			}
			set
			{
				_plasticYield = value;
				SetConstraintsDirty(Oni.ConstraintType.Bending);
			}
		}

		public float plasticCreep
		{
			get
			{
				return _plasticCreep;
			}
			set
			{
				_plasticCreep = value;
				SetConstraintsDirty(Oni.ConstraintType.Bending);
			}
		}

		public float interParticleDistance => m_RopeBlueprint.interParticleDistance;

		public override ObiActorBlueprint sourceBlueprint => m_RopeBlueprint;

		public ObiRopeBlueprint ropeBlueprint
		{
			get
			{
				return m_RopeBlueprint;
			}
			set
			{
				if (m_RopeBlueprint != value)
				{
					RemoveFromSolver();
					ClearState();
					m_RopeBlueprint = value;
					AddToSolver();
				}
			}
		}

		public event RopeTornCallback OnRopeTorn;

		protected override void OnValidate()
		{
			base.OnValidate();
			SetupRuntimeConstraints();
		}

		internal override void LoadBlueprint()
		{
			if (Application.isPlaying)
			{
				m_RopeBlueprintInstance = base.blueprint as ObiRopeBlueprint;
			}
			base.LoadBlueprint();
			RebuildElementsFromConstraints();
			SetupRuntimeConstraints();
		}

		internal override void UnloadBlueprint()
		{
			base.UnloadBlueprint();
			if (m_RopeBlueprintInstance != null)
			{
				Object.DestroyImmediate(m_RopeBlueprintInstance);
			}
		}

		private void SetupRuntimeConstraints()
		{
			SetConstraintsDirty(Oni.ConstraintType.Distance);
			SetConstraintsDirty(Oni.ConstraintType.Bending);
			SetConstraintsDirty(Oni.ConstraintType.Aerodynamics);
			SetSelfCollisions(selfCollisions);
			SetMassScale(m_MassScale);
			RecalculateRestLength();
			SetSimplicesDirty();
		}

		public override void SimulationStart(float timeToSimulate, float substepTime)
		{
			base.SimulationStart(timeToSimulate, substepTime);
			if (base.isActiveAndEnabled && tearingEnabled)
			{
				ApplyTearing(substepTime);
			}
		}

		protected void ApplyTearing(float substepTime)
		{
			float num = substepTime * substepTime;
			tornElements.Clear();
			ObiConstraints<ObiDistanceConstraintsBatch> obiConstraints = GetConstraintsByType(Oni.ConstraintType.Distance) as ObiConstraints<ObiDistanceConstraintsBatch>;
			ObiConstraints<ObiDistanceConstraintsBatch> obiConstraints2 = base.solver.GetConstraintsByType(Oni.ConstraintType.Distance) as ObiConstraints<ObiDistanceConstraintsBatch>;
			if (obiConstraints != null && obiConstraints2 != null)
			{
				for (int i = 0; i < solverBatchOffsets[5].Count; i++)
				{
					ObiDistanceConstraintsBatch obiDistanceConstraintsBatch = obiConstraints.GetBatch(i) as ObiDistanceConstraintsBatch;
					ObiDistanceConstraintsBatch obiDistanceConstraintsBatch2 = obiConstraints2.batches[i];
					for (int j = 0; j < obiDistanceConstraintsBatch.activeConstraintCount; j++)
					{
						int index = i + 2 * j;
						int num2 = solverBatchOffsets[5][i];
						float num3 = obiDistanceConstraintsBatch2.lambdas[num2 + j] / num;
						elements[index].constraintForce = num3;
						if (0f - num3 > tearResistanceMultiplier)
						{
							tornElements.Add(elements[index]);
						}
					}
				}
			}
			if (tornElements.Count <= 0)
			{
				return;
			}
			tornElements.Sort((ObiStructuralElement x, ObiStructuralElement y) => x.constraintForce.CompareTo(y.constraintForce));
			int num4 = 0;
			for (int num5 = 0; num5 < tornElements.Count; num5++)
			{
				if (Tear(tornElements[num5]))
				{
					num4++;
				}
				if (num4 >= tearRate)
				{
					break;
				}
			}
			if (num4 > 0)
			{
				RebuildConstraintsFromElements();
			}
		}

		private int SplitParticle(int splitIndex)
		{
			m_Solver.invMasses[splitIndex] *= 2f;
			CopyParticle(base.solver.particleToActor[splitIndex].indexInActor, base.activeParticleCount);
			ActivateParticle();
			SetRenderingDirty(Oni.RenderingSystemType.AllRopes);
			return solverIndices[base.activeParticleCount - 1];
		}

		public bool Tear(ObiStructuralElement element)
		{
			if (base.activeParticleCount >= m_RopeBlueprint.particleCount)
			{
				return false;
			}
			if (m_Solver.invMasses[element.particle1] == 0f)
			{
				return false;
			}
			int num = elements.IndexOf(element);
			if (num > 0 && elements[num - 1].particle2 != element.particle1)
			{
				return false;
			}
			element.particle1 = SplitParticle(element.particle1);
			this.OnRopeTorn?.Invoke(this, new ObiRopeTornEventArgs(element, element.particle1));
			return true;
		}

		protected override void RebuildElementsFromConstraintsInternal()
		{
			if (GetConstraintsByType(Oni.ConstraintType.Distance) is ObiConstraints<ObiDistanceConstraintsBatch> { batchCount: >=2 } obiConstraints)
			{
				int num = obiConstraints.batches[0].activeConstraintCount + obiConstraints.batches[1].activeConstraintCount;
				elements = new List<ObiStructuralElement>(num);
				for (int i = 0; i < num; i++)
				{
					ObiDistanceConstraintsBatch obiDistanceConstraintsBatch = obiConstraints.batches[i % 2];
					int num2 = i / 2;
					ObiStructuralElement obiStructuralElement = new ObiStructuralElement();
					obiStructuralElement.particle1 = solverIndices[obiDistanceConstraintsBatch.particleIndices[num2 * 2]];
					obiStructuralElement.particle2 = solverIndices[obiDistanceConstraintsBatch.particleIndices[num2 * 2 + 1]];
					obiStructuralElement.restLength = obiDistanceConstraintsBatch.restLengths[num2];
					obiStructuralElement.tearResistance = 1f;
					elements.Add(obiStructuralElement);
				}
				if (obiConstraints.batches.Count > 2)
				{
					ObiDistanceConstraintsBatch obiDistanceConstraintsBatch2 = obiConstraints.batches[2];
					ObiStructuralElement obiStructuralElement2 = new ObiStructuralElement();
					obiStructuralElement2.particle1 = solverIndices[obiDistanceConstraintsBatch2.particleIndices[0]];
					obiStructuralElement2.particle2 = solverIndices[obiDistanceConstraintsBatch2.particleIndices[1]];
					obiStructuralElement2.restLength = obiDistanceConstraintsBatch2.restLengths[0];
					obiStructuralElement2.tearResistance = 1f;
					elements.Add(obiStructuralElement2);
				}
			}
		}

		public override void RebuildConstraintsFromElements()
		{
			ObiConstraints<ObiDistanceConstraintsBatch> obiConstraints = GetConstraintsByType(Oni.ConstraintType.Distance) as ObiConstraints<ObiDistanceConstraintsBatch>;
			ObiConstraints<ObiBendConstraintsBatch> obiConstraints2 = GetConstraintsByType(Oni.ConstraintType.Bending) as ObiConstraints<ObiBendConstraintsBatch>;
			ObiConstraints<ObiAerodynamicConstraintsBatch> obiConstraints3 = GetConstraintsByType(Oni.ConstraintType.Aerodynamics) as ObiConstraints<ObiAerodynamicConstraintsBatch>;
			obiConstraints.DeactivateAllConstraints();
			obiConstraints2.DeactivateAllConstraints();
			obiConstraints3.DeactivateAllConstraints();
			for (int i = 0; i < base.activeParticleCount; i++)
			{
				ObiAerodynamicConstraintsBatch obiAerodynamicConstraintsBatch = obiConstraints3.batches[0];
				int activeConstraintCount = obiAerodynamicConstraintsBatch.activeConstraintCount;
				obiAerodynamicConstraintsBatch.particleIndices[activeConstraintCount] = i;
				obiAerodynamicConstraintsBatch.aerodynamicCoeffs[activeConstraintCount * 3] = 2f * base.solver.principalRadii[solverIndices[i]].x;
				obiAerodynamicConstraintsBatch.ActivateConstraint(activeConstraintCount);
			}
			int num = elements.Count - (ropeBlueprint.path.Closed ? 1 : 0);
			for (int j = 0; j < num; j++)
			{
				ObiDistanceConstraintsBatch obiDistanceConstraintsBatch = obiConstraints.batches[j % 2];
				int activeConstraintCount2 = obiDistanceConstraintsBatch.activeConstraintCount;
				obiDistanceConstraintsBatch.particleIndices[activeConstraintCount2 * 2] = base.solver.particleToActor[elements[j].particle1].indexInActor;
				obiDistanceConstraintsBatch.particleIndices[activeConstraintCount2 * 2 + 1] = base.solver.particleToActor[elements[j].particle2].indexInActor;
				obiDistanceConstraintsBatch.restLengths[activeConstraintCount2] = elements[j].restLength;
				obiDistanceConstraintsBatch.stiffnesses[activeConstraintCount2] = new Vector2(_stretchCompliance, _maxCompression * obiDistanceConstraintsBatch.restLengths[activeConstraintCount2]);
				obiDistanceConstraintsBatch.ActivateConstraint(activeConstraintCount2);
				if (j < num - 1)
				{
					ObiBendConstraintsBatch obiBendConstraintsBatch = obiConstraints2.batches[j % 3];
					if (elements[j].particle2 == elements[j + 1].particle1)
					{
						activeConstraintCount2 = obiBendConstraintsBatch.activeConstraintCount;
						int particle = elements[j].particle1;
						int particle2 = elements[j + 1].particle2;
						int particle3 = elements[j].particle2;
						float value = 0f;
						obiBendConstraintsBatch.particleIndices[activeConstraintCount2 * 3] = base.solver.particleToActor[particle].indexInActor;
						obiBendConstraintsBatch.particleIndices[activeConstraintCount2 * 3 + 1] = base.solver.particleToActor[particle2].indexInActor;
						obiBendConstraintsBatch.particleIndices[activeConstraintCount2 * 3 + 2] = base.solver.particleToActor[particle3].indexInActor;
						obiBendConstraintsBatch.restBends[activeConstraintCount2] = value;
						obiBendConstraintsBatch.bendingStiffnesses[activeConstraintCount2] = new Vector2(_maxBending, _bendCompliance);
						obiBendConstraintsBatch.ActivateConstraint(activeConstraintCount2);
					}
				}
			}
			if (obiConstraints.batches.Count > 2)
			{
				ObiDistanceConstraintsBatch obiDistanceConstraintsBatch2 = obiConstraints.batches[2];
				ObiStructuralElement obiStructuralElement = elements[elements.Count - 1];
				obiDistanceConstraintsBatch2.particleIndices[0] = base.solver.particleToActor[obiStructuralElement.particle1].indexInActor;
				obiDistanceConstraintsBatch2.particleIndices[1] = base.solver.particleToActor[obiStructuralElement.particle2].indexInActor;
				obiDistanceConstraintsBatch2.restLengths[0] = obiStructuralElement.restLength;
				obiDistanceConstraintsBatch2.stiffnesses[0] = new Vector2(_stretchCompliance, _maxCompression * obiDistanceConstraintsBatch2.restLengths[0]);
				obiDistanceConstraintsBatch2.ActivateConstraint(0);
			}
			if (obiConstraints2.batches.Count > 4 && elements.Count > 2)
			{
				ObiBendConstraintsBatch obiBendConstraintsBatch2 = obiConstraints2.batches[3];
				ObiStructuralElement obiStructuralElement2 = elements[elements.Count - 2];
				obiBendConstraintsBatch2.particleIndices[0] = base.solver.particleToActor[obiStructuralElement2.particle1].indexInActor;
				obiBendConstraintsBatch2.particleIndices[1] = base.solver.particleToActor[elements[0].particle1].indexInActor;
				obiBendConstraintsBatch2.particleIndices[2] = base.solver.particleToActor[obiStructuralElement2.particle2].indexInActor;
				obiBendConstraintsBatch2.restBends[0] = 0f;
				obiBendConstraintsBatch2.bendingStiffnesses[0] = new Vector2(_maxBending, _bendCompliance);
				obiBendConstraintsBatch2.ActivateConstraint(0);
				ObiBendConstraintsBatch obiBendConstraintsBatch3 = obiConstraints2.batches[4];
				obiBendConstraintsBatch3.particleIndices[0] = base.solver.particleToActor[obiStructuralElement2.particle2].indexInActor;
				obiBendConstraintsBatch3.particleIndices[1] = base.solver.particleToActor[elements[0].particle2].indexInActor;
				obiBendConstraintsBatch3.particleIndices[2] = base.solver.particleToActor[elements[0].particle1].indexInActor;
				obiBendConstraintsBatch3.restBends[0] = 0f;
				obiBendConstraintsBatch3.bendingStiffnesses[0] = new Vector2(_maxBending, _bendCompliance);
				obiBendConstraintsBatch3.ActivateConstraint(0);
			}
			ObiRopeBlueprint obiRopeBlueprint = base.sharedBlueprint as ObiRopeBlueprint;
			obiRopeBlueprint.edges = new int[elements.Count * 2];
			obiRopeBlueprint.deformableEdges = new int[elements.Count * 2];
			for (int k = 0; k < elements.Count; k++)
			{
				obiRopeBlueprint.deformableEdges[k * 2] = (obiRopeBlueprint.edges[k * 2] = base.solver.particleToActor[elements[k].particle1].indexInActor);
				obiRopeBlueprint.deformableEdges[k * 2 + 1] = (obiRopeBlueprint.edges[k * 2 + 1] = base.solver.particleToActor[elements[k].particle2].indexInActor);
			}
			SetConstraintsDirty(Oni.ConstraintType.Distance);
			SetConstraintsDirty(Oni.ConstraintType.Bending);
			SetConstraintsDirty(Oni.ConstraintType.Aerodynamics);
			base.solver.dirtyDeformableEdges = true;
			SetSimplicesDirty();
		}
	}
}
