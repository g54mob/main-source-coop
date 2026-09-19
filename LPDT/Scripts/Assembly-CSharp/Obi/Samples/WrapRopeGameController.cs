using UnityEngine;
using UnityEngine.Events;

namespace Obi.Samples
{
	[RequireComponent(typeof(ObiSolver))]
	public class WrapRopeGameController : MonoBehaviour
	{
		private ObiSolver solver;

		public Wrappable[] wrappables;

		public UnityEvent onFinish = new UnityEvent();

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

		private void Update()
		{
			bool flag = true;
			Wrappable[] array = wrappables;
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].IsWrapped())
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				onFinish.Invoke();
			}
		}

		private void Solver_OnCollision(ObiSolver s, ObiNativeContactList e)
		{
			Wrappable[] array = wrappables;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Reset();
			}
			ObiColliderWorld instance = ObiColliderWorld.GetInstance();
			foreach (Oni.Contact item in e)
			{
				if (!(item.distance < 0.025f))
				{
					continue;
				}
				ObiColliderBase owner = instance.colliderHandles[item.bodyB].owner;
				if (owner != null)
				{
					Wrappable component = owner.GetComponent<Wrappable>();
					if (component != null)
					{
						component.SetWrapped();
					}
				}
			}
		}
	}
}
