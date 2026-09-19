using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	public abstract class ObiRopeBase : ObiActor, IAerodynamicConstraintsUser
	{
		[SerializeField]
		protected bool m_SelfCollisions;

		[HideInInspector]
		[SerializeField]
		protected float restLength_;

		[HideInInspector]
		public List<ObiStructuralElement> elements = new List<ObiStructuralElement>();

		[SerializeField]
		protected bool _aerodynamicsEnabled = true;

		[SerializeField]
		protected float _drag = 0.05f;

		[SerializeField]
		protected float _lift = 0.02f;

		public bool aerodynamicsEnabled
		{
			get
			{
				return _aerodynamicsEnabled;
			}
			set
			{
				if (value != _aerodynamicsEnabled)
				{
					_aerodynamicsEnabled = value;
					SetConstraintsDirty(Oni.ConstraintType.Aerodynamics);
				}
			}
		}

		public float drag
		{
			get
			{
				return _drag;
			}
			set
			{
				_drag = value;
				SetConstraintsDirty(Oni.ConstraintType.Aerodynamics);
			}
		}

		public float lift
		{
			get
			{
				return _lift;
			}
			set
			{
				_lift = value;
				SetConstraintsDirty(Oni.ConstraintType.Aerodynamics);
			}
		}

		public float restLength => restLength_;

		public ObiPath path
		{
			get
			{
				ObiRopeBlueprintBase obiRopeBlueprintBase = sourceBlueprint as ObiRopeBlueprintBase;
				if (!(obiRopeBlueprintBase != null))
				{
					return null;
				}
				return obiRopeBlueprintBase.path;
			}
		}

		public event ActorCallback OnElementsGenerated;

		public float GetDrag(ObiAerodynamicConstraintsBatch batch, int constraintIndex)
		{
			return drag;
		}

		public float GetLift(ObiAerodynamicConstraintsBatch batch, int constraintIndex)
		{
			return lift;
		}

		public override void ProvideDeformableEdges(ObiNativeIntList deformableEdges)
		{
			base.deformableEdgesOffset = deformableEdges.count / 2;
			ObiRopeBlueprintBase obiRopeBlueprintBase = base.sharedBlueprint as ObiRopeBlueprintBase;
			if (obiRopeBlueprintBase != null && obiRopeBlueprintBase.deformableEdges != null)
			{
				for (int i = 0; i < obiRopeBlueprintBase.deformableEdges.Length; i++)
				{
					deformableEdges.Add(solverIndices[obiRopeBlueprintBase.deformableEdges[i]]);
				}
			}
		}

		public override int GetDeformableEdgeCount()
		{
			ObiRopeBlueprintBase obiRopeBlueprintBase = base.sharedBlueprint as ObiRopeBlueprintBase;
			if (obiRopeBlueprintBase != null && obiRopeBlueprintBase.deformableEdges != null)
			{
				return obiRopeBlueprintBase.deformableEdges.Length / 2;
			}
			return 0;
		}

		public float CalculateLength()
		{
			float num = 0f;
			if (base.isLoaded)
			{
				int count = elements.Count;
				for (int i = 0; i < count; i++)
				{
					num += Vector4.Distance(base.solver.positions[elements[i].particle1], base.solver.positions[elements[i].particle2]);
				}
			}
			return num;
		}

		public void RecalculateRestLength()
		{
			restLength_ = 0f;
			int count = elements.Count;
			for (int i = 0; i < count; i++)
			{
				restLength_ += elements[i].restLength;
			}
		}

		public void RecalculateRestPositions()
		{
			float num = 0f;
			int count = elements.Count;
			for (int i = 0; i < count; i++)
			{
				base.solver.restPositions[elements[i].particle1] = new Vector4(num, 0f, 0f, 1f);
				num += elements[i].restLength;
				base.solver.restPositions[elements[i].particle2] = new Vector4(num, 0f, 0f, 1f);
			}
		}

		public void RebuildElementsFromConstraints()
		{
			RebuildElementsFromConstraintsInternal();
			if (this.OnElementsGenerated != null)
			{
				this.OnElementsGenerated(this);
			}
		}

		protected abstract void RebuildElementsFromConstraintsInternal();

		public virtual void RebuildConstraintsFromElements()
		{
		}

		public ObiStructuralElement GetElementAt(float mu, out float elementMu)
		{
			float num = (float)elements.Count * Mathf.Clamp(mu, 0f, 0.99999f);
			int num2 = (int)num;
			elementMu = num - (float)num2;
			if (elements != null && num2 < elements.Count)
			{
				return elements[num2];
			}
			return null;
		}

		public int GetEdgeAt(float mu, out float elementMu)
		{
			elementMu = -1f;
			ObiRopeBlueprintBase obiRopeBlueprintBase = base.sharedBlueprint as ObiRopeBlueprintBase;
			if (obiRopeBlueprintBase != null && obiRopeBlueprintBase.deformableEdges != null)
			{
				float num = (float)(obiRopeBlueprintBase.deformableEdges.Length / 2) * Mathf.Clamp(mu, 0f, 0.99999f);
				int num2 = (int)num;
				elementMu = num - (float)num2;
				if (num2 < obiRopeBlueprintBase.deformableEdges.Length / 2)
				{
					return num2;
				}
			}
			return -1;
		}
	}
}
