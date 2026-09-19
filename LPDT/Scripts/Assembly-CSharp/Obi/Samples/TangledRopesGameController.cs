using UnityEngine;
using UnityEngine.Events;

namespace Obi.Samples
{
	[RequireComponent(typeof(ObiSolver))]
	public class TangledRopesGameController : MonoBehaviour
	{
		public TangledPegSlot[] pegSlots;

		public float pegHoverHeight = 1f;

		public float maxPegDistanceFromSlot = 1.5f;

		public int framesWithoutContactsToWin = 30;

		public UnityEvent onFinish = new UnityEvent();

		private TangledPeg selectedPeg;

		private Plane floor = new Plane(Vector3.up, 0f);

		private int framesSinceLastContact;

		private void OnEnable()
		{
			GetComponent<ObiSolver>().OnParticleCollision += Solver_OnParticleCollision;
		}

		private void OnDisable()
		{
			GetComponent<ObiSolver>().OnParticleCollision -= Solver_OnParticleCollision;
		}

		private TangledPegSlot FindCandidateSlot(TangledPeg peg)
		{
			TangledPegSlot result = null;
			float num = float.MaxValue;
			TangledPegSlot[] array = pegSlots;
			foreach (TangledPegSlot tangledPegSlot in array)
			{
				tangledPegSlot.ResetColor();
				if (!(tangledPegSlot.currentPeg != null))
				{
					Vector3 a = floor.ClosestPointOnPlane(tangledPegSlot.transform.position);
					Vector3 b = floor.ClosestPointOnPlane(peg.transform.position);
					float num2 = Vector3.Distance(a, b);
					if (num2 < num && num2 < maxPegDistanceFromSlot)
					{
						result = tangledPegSlot;
						num = num2;
					}
				}
			}
			return result;
		}

		private void Update()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out var hitInfo) && hitInfo.transform.TryGetComponent<TangledPeg>(out var component) && component.currentSlot != null)
			{
				selectedPeg = component;
				selectedPeg.UndockFromCurrentSlot();
			}
			if (selectedPeg != null)
			{
				if (floor.Raycast(ray, out var enter))
				{
					selectedPeg.MoveTowards(ray.GetPoint(enter) + Vector3.up * pegHoverHeight);
				}
				TangledPegSlot tangledPegSlot = FindCandidateSlot(selectedPeg);
				if (tangledPegSlot != null)
				{
					tangledPegSlot.Tint();
				}
				if (Input.GetMouseButtonUp(0))
				{
					if (tangledPegSlot != null)
					{
						selectedPeg.currentSlot = null;
						selectedPeg.DockInSlot(tangledPegSlot);
						tangledPegSlot.ResetColor();
					}
					else
					{
						selectedPeg.DockInSlot(selectedPeg.currentSlot);
					}
					selectedPeg = null;
				}
			}
			if (framesSinceLastContact >= framesWithoutContactsToWin && onFinish != null)
			{
				onFinish.Invoke();
			}
		}

		private void Solver_OnParticleCollision(ObiSolver s, ObiNativeContactList e)
		{
			int num = 0;
			for (int i = 0; i < e.count; i++)
			{
				ObiActor actor = s.particleToActor[s.simplices[e[i].bodyA]].actor;
				ObiActor actor2 = s.particleToActor[s.simplices[e[i].bodyB]].actor;
				if (actor != actor2)
				{
					num++;
				}
			}
			if (num == 0)
			{
				framesSinceLastContact++;
			}
			else
			{
				framesSinceLastContact = 0;
			}
		}
	}
}
