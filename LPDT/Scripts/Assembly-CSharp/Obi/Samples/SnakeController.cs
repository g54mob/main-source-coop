using UnityEngine;

namespace Obi.Samples
{
	public class SnakeController : MonoBehaviour
	{
		public Transform headReferenceFrame;

		public float headSpeed = 20f;

		public float upSpeed = 40f;

		public float slitherSpeed = 10f;

		private ObiRope rope;

		private ObiSolver solver;

		private float[] traction;

		private Vector3[] surfaceNormal;

		private void Start()
		{
			rope = GetComponent<ObiRope>();
			solver = rope.solver;
			rope.OnSimulationStart += ResetSurfaceInfo;
			solver.OnCollision += AnalyzeContacts;
			solver.OnParticleCollision += AnalyzeContacts;
		}

		private void OnDestroy()
		{
			rope.OnSimulationStart -= ResetSurfaceInfo;
			solver.OnCollision -= AnalyzeContacts;
			solver.OnParticleCollision -= AnalyzeContacts;
		}

		private void ResetSurfaceInfo(ObiActor a, float simulatedTime, float substepTime)
		{
			if (traction == null)
			{
				traction = new float[rope.activeParticleCount];
				surfaceNormal = new Vector3[rope.activeParticleCount];
			}
			if (Input.GetKey(KeyCode.J))
			{
				for (int i = 1; i < rope.activeParticleCount; i++)
				{
					int index = rope.solverIndices[i];
					int index2 = rope.solverIndices[i - 1];
					Vector4 vector = Vector3.ProjectOnPlane(solver.positions[index2] - solver.positions[index], surfaceNormal[i]).normalized;
					solver.velocities[index] += vector * traction[i] / solver.invMasses[index] * slitherSpeed * simulatedTime;
				}
			}
			int index3 = rope.solverIndices[0];
			if (headReferenceFrame != null)
			{
				Vector3 zero = Vector3.zero;
				if (Input.GetKey(KeyCode.W))
				{
					zero += headReferenceFrame.forward * headSpeed;
				}
				if (Input.GetKey(KeyCode.A))
				{
					zero += -headReferenceFrame.right * headSpeed;
				}
				if (Input.GetKey(KeyCode.S))
				{
					zero += -headReferenceFrame.forward * headSpeed;
				}
				if (Input.GetKey(KeyCode.D))
				{
					zero += headReferenceFrame.right * headSpeed;
				}
				zero.y = 0f;
				solver.velocities[index3] += (Vector4)zero * simulatedTime;
			}
			if (Input.GetKey(KeyCode.Space))
			{
				solver.velocities[index3] += (Vector4)Vector3.up * simulatedTime * upSpeed;
			}
			for (int j = 0; j < traction.Length; j++)
			{
				traction[j] = 0f;
				surfaceNormal[j] = Vector3.zero;
			}
		}

		private void AnalyzeContacts(object sender, ObiNativeContactList e)
		{
			for (int i = 0; i < e.count; i++)
			{
				Oni.Contact contact = e[i];
				if (contact.distance < 0.005f)
				{
					int num = solver.simplices[contact.bodyA];
					ObiSolver.ParticleInActor particleInActor = solver.particleToActor[num];
					if (particleInActor != null && particleInActor.actor == rope && traction != null)
					{
						traction[particleInActor.indexInActor] = 1f;
						surfaceNormal[particleInActor.indexInActor] += (Vector3)contact.normal;
					}
				}
			}
		}
	}
}
