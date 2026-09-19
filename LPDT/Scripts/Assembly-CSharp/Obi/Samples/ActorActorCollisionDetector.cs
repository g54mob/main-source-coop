using UnityEngine;
using UnityEngine.Events;

namespace Obi.Samples
{
	[RequireComponent(typeof(ObiSolver))]
	public class ActorActorCollisionDetector : MonoBehaviour
	{
		public struct ActorPair
		{
			public readonly ObiActor actorA;

			public readonly ObiActor actorB;

			public int particleA;

			public int particleB;

			public ActorPair(ObiActor actorA, ObiActor actorB, int particleA, int particleB)
			{
				this.actorA = actorA;
				this.actorB = actorB;
				this.particleA = particleA;
				this.particleB = particleB;
			}
		}

		public UnityEvent<ActorPair> callback;

		private ObiSolver solver;

		private void OnEnable()
		{
			solver = GetComponent<ObiSolver>();
			solver.OnParticleCollision += Solver_OnCollision;
		}

		private void OnDisable()
		{
			solver.OnParticleCollision -= Solver_OnCollision;
		}

		private void Solver_OnCollision(object sender, ObiNativeContactList e)
		{
			if (!solver.initialized || callback == null)
			{
				return;
			}
			foreach (Oni.Contact item in e)
			{
				if ((double)item.distance < 0.01)
				{
					int size;
					int simplexStartAndSize = solver.simplexCounts.GetSimplexStartAndSize(item.bodyA, out size);
					int simplexStartAndSize2 = solver.simplexCounts.GetSimplexStartAndSize(item.bodyB, out size);
					int num = solver.simplices[simplexStartAndSize];
					int num2 = solver.simplices[simplexStartAndSize2];
					ObiSolver.ParticleInActor particleInActor = solver.particleToActor[num];
					ObiSolver.ParticleInActor particleInActor2 = solver.particleToActor[num2];
					if (particleInActor != null && particleInActor2 != null && particleInActor.actor != particleInActor2.actor)
					{
						callback.Invoke(new ActorPair(particleInActor.actor, particleInActor2.actor, num, num2));
					}
				}
			}
		}
	}
}
