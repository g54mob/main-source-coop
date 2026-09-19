using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[ExecuteInEditMode]
	public class ObiStitcher : MonoBehaviour
	{
		[Serializable]
		public class Stitch
		{
			public int particleIndex1;

			public int particleIndex2;

			public Stitch(int particleIndex1, int particleIndex2)
			{
				this.particleIndex1 = particleIndex1;
				this.particleIndex2 = particleIndex2;
			}
		}

		[SerializeField]
		[HideInInspector]
		private List<Stitch> stitches = new List<Stitch>();

		[SerializeField]
		[HideInInspector]
		private ObiActor actor1;

		[SerializeField]
		[HideInInspector]
		private ObiActor actor2;

		[HideInInspector]
		public ObiNativeIntList particleIndices;

		[HideInInspector]
		public ObiNativeFloatList stiffnesses;

		[HideInInspector]
		public ObiNativeFloatList lambdas;

		private IStitchConstraintsBatchImpl m_BatchImpl;

		private bool inSolver;

		public ObiActor Actor1
		{
			get
			{
				return actor1;
			}
			set
			{
				if (actor1 != value)
				{
					UnregisterActor(actor1);
					actor1 = value;
					RegisterActor(actor1);
				}
			}
		}

		public ObiActor Actor2
		{
			get
			{
				return actor2;
			}
			set
			{
				if (actor2 != value)
				{
					UnregisterActor(actor2);
					actor2 = value;
					RegisterActor(actor2);
				}
			}
		}

		public int StitchCount => stitches.Count;

		public IEnumerable<Stitch> Stitches => stitches.AsReadOnly();

		private void RegisterActor(ObiActor actor)
		{
			if (actor != null)
			{
				actor.OnBlueprintLoaded += Actor_OnBlueprintLoaded;
				actor.OnBlueprintUnloaded += Actor_OnBlueprintUnloaded;
				if (actor.solver != null)
				{
					Actor_OnBlueprintLoaded(actor, actor.sourceBlueprint);
				}
			}
		}

		private void UnregisterActor(ObiActor actor)
		{
			if (actor != null)
			{
				actor.OnBlueprintLoaded -= Actor_OnBlueprintLoaded;
				actor.OnBlueprintUnloaded -= Actor_OnBlueprintUnloaded;
				if (actor.solver != null)
				{
					Actor_OnBlueprintUnloaded(actor, actor.sourceBlueprint);
				}
			}
		}

		public void OnEnable()
		{
			RegisterActor(actor1);
			RegisterActor(actor2);
		}

		public void OnDisable()
		{
			UnregisterActor(actor1);
			UnregisterActor(actor2);
		}

		public int AddStitch(int particle1, int particle2)
		{
			stitches.Add(new Stitch(particle1, particle2));
			return stitches.Count - 1;
		}

		public void RemoveStitch(int index)
		{
			if (index >= 0 && index < stitches.Count)
			{
				stitches.RemoveAt(index);
			}
		}

		public void Clear()
		{
			stitches.Clear();
			PushDataToSolver();
		}

		private void Actor_OnBlueprintUnloaded(ObiActor actor, ObiActorBlueprint blueprint)
		{
			RemoveFromSolver(actor.solver);
		}

		private void Actor_OnBlueprintLoaded(ObiActor actor, ObiActorBlueprint blueprint)
		{
			if (actor1 != null && actor2 != null && actor1.isLoaded && actor2.isLoaded)
			{
				if (actor1.solver != actor2.solver)
				{
					Debug.LogError("ObiStitcher cannot handle actors in different solvers.");
				}
				else
				{
					AddToSolver(actor1.solver);
				}
			}
		}

		private void AddToSolver(ObiSolver solver)
		{
			if (!inSolver)
			{
				inSolver = true;
				particleIndices = new ObiNativeIntList();
				stiffnesses = new ObiNativeFloatList();
				lambdas = new ObiNativeFloatList();
				m_BatchImpl = solver.implementation.CreateConstraintsBatch(Oni.ConstraintType.Stitch) as IStitchConstraintsBatchImpl;
				PushDataToSolver();
				m_BatchImpl.enabled = base.isActiveAndEnabled;
			}
		}

		private void RemoveFromSolver(ObiSolver solver)
		{
			if (inSolver && m_BatchImpl != null)
			{
				lambdas.Dispose();
				particleIndices.Dispose();
				stiffnesses.Dispose();
				solver.implementation.DestroyConstraintsBatch(m_BatchImpl);
				m_BatchImpl.Destroy();
				m_BatchImpl = null;
				inSolver = false;
			}
		}

		public void PushDataToSolver()
		{
			if (inSolver)
			{
				lambdas.Clear();
				particleIndices.ResizeUninitialized(stitches.Count * 2);
				stiffnesses.ResizeUninitialized(stitches.Count);
				lambdas.ResizeUninitialized(stitches.Count);
				for (int i = 0; i < stitches.Count; i++)
				{
					particleIndices[i * 2] = actor1.solverIndices[stitches[i].particleIndex1];
					particleIndices[i * 2 + 1] = actor2.solverIndices[stitches[i].particleIndex2];
					stiffnesses[i] = 0f;
					lambdas[i] = 0f;
				}
				m_BatchImpl.SetStitchConstraints(particleIndices, stiffnesses, lambdas, stitches.Count);
			}
		}
	}
}
