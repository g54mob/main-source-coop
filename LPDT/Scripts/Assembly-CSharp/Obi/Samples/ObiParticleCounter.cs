using System.Collections.Generic;
using UnityEngine;

namespace Obi.Samples
{
	[RequireComponent(typeof(ObiSolver))]
	public class ObiParticleCounter : MonoBehaviour
	{
		private ObiSolver solver;

		public int counter;

		public Collider2D targetCollider;

		private ObiNativeContactList frame;

		private HashSet<int> particles = new HashSet<int>();

		private void Awake()
		{
			solver = GetComponent<ObiSolver>();
		}

		private void OnEnable()
		{
			solver.OnCollision += Solver_OnCollision;
		}

		private void OnDisable()
		{
			solver.OnCollision -= Solver_OnCollision;
		}

		private void Solver_OnCollision(object sender, ObiNativeContactList e)
		{
			HashSet<int> other = new HashSet<int>();
			for (int i = 0; i < e.count; i++)
			{
				_ = e[i].distance;
				_ = 0.001f;
			}
			particles.ExceptWith(other);
			counter += particles.Count;
			particles = other;
			Debug.Log(counter);
		}
	}
}
