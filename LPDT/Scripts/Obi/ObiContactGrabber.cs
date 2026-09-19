using System.Collections.Generic;
using Obi;
using UnityEngine;

[RequireComponent(typeof(ObiCollider))]
public class ObiContactGrabber : MonoBehaviour
{
	private class GrabbedParticle : IEqualityComparer<GrabbedParticle>
	{
		public int index;

		public float invMass;

		public Vector3 localPosition;

		public ObiSolver solver;

		public GrabbedParticle(ObiSolver solver, int index, float invMass)
		{
			this.solver = solver;
			this.index = index;
			this.invMass = invMass;
		}

		public bool Equals(GrabbedParticle x, GrabbedParticle y)
		{
			return x.index == y.index;
		}

		public int GetHashCode(GrabbedParticle obj)
		{
			return index;
		}
	}

	public ObiSolver[] solvers = new ObiSolver[0];

	private Dictionary<ObiSolver, ObiNativeContactList> collisionEvents = new Dictionary<ObiSolver, ObiNativeContactList>();

	private ObiCollider localCollider;

	private HashSet<GrabbedParticle> grabbedParticles = new HashSet<GrabbedParticle>();

	private HashSet<ObiActor> grabbedActors = new HashSet<ObiActor>();

	public bool grabbed => grabbedActors.Count > 0;

	private void Awake()
	{
		localCollider = GetComponent<ObiCollider>();
	}

	private void OnEnable()
	{
		if (solvers != null)
		{
			ObiSolver[] array = solvers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnCollision += Solver_OnCollision;
			}
		}
	}

	private void OnDisable()
	{
		if (solvers != null)
		{
			ObiSolver[] array = solvers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnCollision -= Solver_OnCollision;
			}
		}
	}

	private void Solver_OnCollision(object sender, ObiNativeContactList e)
	{
		collisionEvents[(ObiSolver)sender] = e;
	}

	private void UpdateParticleProperties()
	{
		foreach (ObiActor grabbedActor in grabbedActors)
		{
			grabbedActor.UpdateParticleProperties();
		}
	}

	private bool GrabParticle(ObiSolver solver, int index)
	{
		GrabbedParticle grabbedParticle = new GrabbedParticle(solver, index, solver.invMasses[index]);
		if (!grabbedParticles.Contains(grabbedParticle))
		{
			grabbedParticle.localPosition = (base.transform.worldToLocalMatrix * solver.transform.localToWorldMatrix).MultiplyPoint3x4(solver.positions[index]);
			grabbedParticles.Add(grabbedParticle);
			solver.invMasses[index] = 0f;
			solver.velocities[index] = Vector4.zero;
			return true;
		}
		return false;
	}

	public void Grab()
	{
		Release();
		ObiColliderWorld instance = ObiColliderWorld.GetInstance();
		if (solvers != null && collisionEvents != null)
		{
			ObiSolver[] array = solvers;
			foreach (ObiSolver obiSolver in array)
			{
				if (!collisionEvents.TryGetValue(obiSolver, out var value))
				{
					continue;
				}
				foreach (Oni.Contact item in value)
				{
					if (item.distance < 0.01f)
					{
						ObiColliderBase owner = instance.colliderHandles[item.bodyB].owner;
						int num = obiSolver.simplices[item.bodyA];
						if (owner == localCollider && GrabParticle(obiSolver, num))
						{
							grabbedActors.Add(obiSolver.particleToActor[num].actor);
						}
					}
				}
			}
		}
		UpdateParticleProperties();
	}

	public void Release()
	{
		foreach (GrabbedParticle grabbedParticle in grabbedParticles)
		{
			grabbedParticle.solver.invMasses[grabbedParticle.index] = grabbedParticle.invMass;
		}
		UpdateParticleProperties();
		grabbedActors.Clear();
		grabbedParticles.Clear();
	}

	private void FixedUpdate()
	{
		foreach (GrabbedParticle grabbedParticle in grabbedParticles)
		{
			Matrix4x4 matrix4x = grabbedParticle.solver.transform.worldToLocalMatrix * base.transform.localToWorldMatrix;
			grabbedParticle.solver.positions[grabbedParticle.index] = matrix4x.MultiplyPoint3x4(grabbedParticle.localPosition);
		}
	}
}
