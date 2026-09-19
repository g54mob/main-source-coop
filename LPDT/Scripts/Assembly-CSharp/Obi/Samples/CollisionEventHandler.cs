using UnityEngine;

namespace Obi.Samples
{
	[RequireComponent(typeof(ObiSolver))]
	public class CollisionEventHandler : MonoBehaviour
	{
		private ObiSolver solver;

		public int contactCount;

		private ObiNativeContactList frame;

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
			frame = e;
		}

		private void OnDrawGizmos()
		{
			if (!(solver == null) && frame != null)
			{
				Gizmos.matrix = solver.transform.localToWorldMatrix;
				contactCount = frame.count;
				for (int i = 0; i < frame.count; i++)
				{
					Oni.Contact contact = frame[i];
					Gizmos.color = ((contact.distance <= 0f) ? Color.red : Color.green);
					Vector3 vector = frame[i].pointB;
					Gizmos.DrawSphere(vector, 0.01f);
					Gizmos.DrawRay(vector, contact.normal * contact.distance);
					Gizmos.color = Color.cyan;
				}
			}
		}
	}
}
