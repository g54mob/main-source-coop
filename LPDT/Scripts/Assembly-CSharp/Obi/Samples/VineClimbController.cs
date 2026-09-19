using UnityEngine;

namespace Obi.Samples
{
	public class VineClimbController : MonoBehaviour
	{
		public ObiSolver solver;

		public float climbSpeed = 1.5f;

		private ObiPinhole pinhole;

		private bool pressedSpace;

		private void Start()
		{
			solver.OnCollision += Solver_OnCollision;
		}

		public void Update()
		{
			if (pinhole != null)
			{
				float motorSpeed = 0f;
				if (Input.GetKey(KeyCode.W))
				{
					motorSpeed = climbSpeed;
				}
				if (Input.GetKey(KeyCode.S))
				{
					motorSpeed = 0f - climbSpeed;
				}
				pinhole.motorSpeed = motorSpeed;
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				pressedSpace = true;
				DetachFromVine();
			}
		}

		private void Solver_OnCollision(ObiSolver solver, ObiNativeContactList contacts)
		{
			if (!pressedSpace)
			{
				return;
			}
			int num = -1;
			float num2 = float.MaxValue;
			Vector3 offset = Vector3.zero;
			foreach (Oni.Contact contact in contacts)
			{
				if (!(contact.distance < 0.001f))
				{
					continue;
				}
				ObiCollider obiCollider = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner as ObiCollider;
				if (obiCollider.sourceCollider.isTrigger)
				{
					int num3 = solver.simplices[contact.bodyA];
					Vector3 vector = solver.transform.TransformPoint(solver.positions[num3]) - obiCollider.transform.position;
					float num4 = Vector3.Magnitude(vector);
					if (num4 < num2)
					{
						num2 = num4;
						offset = vector;
						num = num3;
					}
				}
			}
			if (num >= 0)
			{
				ObiActor actor = solver.particleToActor[num].actor;
				AttachToVine(actor as ObiRope, num, offset);
			}
			pressedSpace = false;
		}

		private float GetParticleMu(ObiRope rope, int solverParticleIndex)
		{
			for (int i = 0; i < rope.elements.Count; i++)
			{
				if (rope.elements[i].particle1 == solverParticleIndex)
				{
					return (float)i / (float)rope.elements.Count;
				}
				if (rope.elements[i].particle2 == solverParticleIndex)
				{
					return (float)(i + 1) / (float)rope.elements.Count;
				}
			}
			return 1f;
		}

		private void AttachToVine(ObiRope rope, int particle, Vector3 offset)
		{
			if (pinhole == null && rope != null)
			{
				base.transform.position += offset;
				pinhole = rope.gameObject.AddComponent<ObiPinhole>();
				pinhole.position = GetParticleMu(rope, particle);
				pinhole.motorForce = float.PositiveInfinity;
				pinhole.friction = 1f;
				pinhole.target = base.transform;
			}
		}

		private void DetachFromVine()
		{
			if (pinhole != null)
			{
				Object.Destroy(pinhole);
				pinhole = null;
				pressedSpace = false;
			}
		}
	}
}
