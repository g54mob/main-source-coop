using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Rod", 881)]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class ObiRod : ObiRopeBase, IStretchShearConstraintsUser, IBendTwistConstraintsUser, IChainConstraintsUser
	{
		[SerializeField]
		protected ObiRodBlueprint m_RodBlueprint;

		[SerializeField]
		protected bool _stretchShearConstraintsEnabled = true;

		[SerializeField]
		protected float _stretchCompliance;

		[SerializeField]
		protected float _shear1Compliance;

		[SerializeField]
		protected float _shear2Compliance;

		[SerializeField]
		protected bool _bendTwistConstraintsEnabled = true;

		[SerializeField]
		protected float _torsionCompliance;

		[SerializeField]
		protected float _bend1Compliance;

		[SerializeField]
		protected float _bend2Compliance;

		[SerializeField]
		[Range(0f, 0.1f)]
		protected float _plasticYield;

		[SerializeField]
		protected float _plasticCreep;

		[SerializeField]
		protected bool _chainConstraintsEnabled = true;

		[SerializeField]
		[Range(0f, 1f)]
		protected float _tightness = 1f;

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
					SetSelfCollisions(m_SelfCollisions);
				}
			}
		}

		public bool stretchShearConstraintsEnabled
		{
			get
			{
				return _stretchShearConstraintsEnabled;
			}
			set
			{
				if (value != _stretchShearConstraintsEnabled)
				{
					_stretchShearConstraintsEnabled = value;
					SetConstraintsDirty(Oni.ConstraintType.StretchShear);
				}
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
				SetConstraintsDirty(Oni.ConstraintType.StretchShear);
			}
		}

		public float shear1Compliance
		{
			get
			{
				return _shear1Compliance;
			}
			set
			{
				_shear1Compliance = value;
				SetConstraintsDirty(Oni.ConstraintType.StretchShear);
			}
		}

		public float shear2Compliance
		{
			get
			{
				return _shear2Compliance;
			}
			set
			{
				_shear2Compliance = value;
				SetConstraintsDirty(Oni.ConstraintType.StretchShear);
			}
		}

		public bool bendTwistConstraintsEnabled
		{
			get
			{
				return _bendTwistConstraintsEnabled;
			}
			set
			{
				if (value != _bendTwistConstraintsEnabled)
				{
					_bendTwistConstraintsEnabled = value;
					SetConstraintsDirty(Oni.ConstraintType.BendTwist);
				}
			}
		}

		public float torsionCompliance
		{
			get
			{
				return _torsionCompliance;
			}
			set
			{
				_torsionCompliance = value;
				SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			}
		}

		public float bend1Compliance
		{
			get
			{
				return _bend1Compliance;
			}
			set
			{
				_bend1Compliance = value;
				SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			}
		}

		public float bend2Compliance
		{
			get
			{
				return _bend2Compliance;
			}
			set
			{
				_bend2Compliance = value;
				SetConstraintsDirty(Oni.ConstraintType.BendTwist);
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
				SetConstraintsDirty(Oni.ConstraintType.BendTwist);
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
				SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			}
		}

		public bool chainConstraintsEnabled
		{
			get
			{
				return _chainConstraintsEnabled;
			}
			set
			{
				if (value != _chainConstraintsEnabled)
				{
					_chainConstraintsEnabled = value;
					SetConstraintsDirty(Oni.ConstraintType.BendTwist);
				}
			}
		}

		public float tightness
		{
			get
			{
				return _tightness;
			}
			set
			{
				_tightness = value;
				SetConstraintsDirty(Oni.ConstraintType.Chain);
			}
		}

		public float interParticleDistance => m_RodBlueprint.interParticleDistance;

		public override ObiActorBlueprint sourceBlueprint => m_RodBlueprint;

		public ObiRodBlueprint rodBlueprint
		{
			get
			{
				return m_RodBlueprint;
			}
			set
			{
				if (m_RodBlueprint != value)
				{
					RemoveFromSolver();
					ClearState();
					m_RodBlueprint = value;
					AddToSolver();
				}
			}
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			SetupRuntimeConstraints();
		}

		internal override void LoadBlueprint()
		{
			base.LoadBlueprint();
			RebuildElementsFromConstraints();
			SetupRuntimeConstraints();
		}

		public override void RequestReadback()
		{
			base.RequestReadback();
			base.solver.orientations.Readback();
		}

		public override void SimulationEnd(float simulatedTime, float substepTime)
		{
			base.SimulationEnd(simulatedTime, substepTime);
			base.solver.orientations.WaitForReadback();
		}

		private void SetupRuntimeConstraints()
		{
			SetConstraintsDirty(Oni.ConstraintType.StretchShear);
			SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			SetConstraintsDirty(Oni.ConstraintType.Chain);
			SetConstraintsDirty(Oni.ConstraintType.Aerodynamics);
			SetSelfCollisions(selfCollisions);
			SetMassScale(m_MassScale);
			RecalculateRestLength();
			SetSimplicesDirty();
		}

		public Vector3 GetBendTwistCompliance(ObiBendTwistConstraintsBatch batch, int constraintIndex)
		{
			return new Vector3(bend1Compliance, bend2Compliance, torsionCompliance);
		}

		public Vector2 GetBendTwistPlasticity(ObiBendTwistConstraintsBatch batch, int constraintIndex)
		{
			return new Vector2(plasticYield, plasticCreep);
		}

		public Vector3 GetStretchShearCompliance(ObiStretchShearConstraintsBatch batch, int constraintIndex)
		{
			return new Vector3(shear1Compliance, shear2Compliance, stretchCompliance);
		}

		protected override void RebuildElementsFromConstraintsInternal()
		{
			if (GetConstraintsByType(Oni.ConstraintType.StretchShear) is ObiConstraints<ObiStretchShearConstraintsBatch> { batchCount: >=2 } obiConstraints)
			{
				int num = obiConstraints.batches[0].activeConstraintCount + obiConstraints.batches[1].activeConstraintCount;
				elements = new List<ObiStructuralElement>(num);
				for (int i = 0; i < num; i++)
				{
					ObiStretchShearConstraintsBatch obiStretchShearConstraintsBatch = obiConstraints.batches[i % 2];
					int num2 = i / 2;
					ObiStructuralElement obiStructuralElement = new ObiStructuralElement();
					obiStructuralElement.particle1 = solverIndices[obiStretchShearConstraintsBatch.particleIndices[num2 * 2]];
					obiStructuralElement.particle2 = solverIndices[obiStretchShearConstraintsBatch.particleIndices[num2 * 2 + 1]];
					obiStructuralElement.restLength = obiStretchShearConstraintsBatch.restLengths[num2];
					elements.Add(obiStructuralElement);
				}
				if (obiConstraints.batches.Count > 2)
				{
					ObiStretchShearConstraintsBatch obiStretchShearConstraintsBatch2 = obiConstraints.batches[2];
					ObiStructuralElement obiStructuralElement2 = new ObiStructuralElement();
					obiStructuralElement2.particle1 = solverIndices[obiStretchShearConstraintsBatch2.particleIndices[0]];
					obiStructuralElement2.particle2 = solverIndices[obiStretchShearConstraintsBatch2.particleIndices[1]];
					obiStructuralElement2.restLength = obiStretchShearConstraintsBatch2.restLengths[0];
					elements.Add(obiStructuralElement2);
				}
			}
		}
	}
}
