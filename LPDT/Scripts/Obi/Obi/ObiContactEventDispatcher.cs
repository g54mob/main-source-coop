using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Obi
{
	[RequireComponent(typeof(ObiSolver))]
	public class ObiContactEventDispatcher : MonoBehaviour
	{
		private class ContactComparer : IComparer<Oni.Contact>
		{
			private ObiSolver solver;

			public ContactComparer(ObiSolver solver)
			{
				this.solver = solver;
			}

			public int Compare(Oni.Contact x, Oni.Contact y)
			{
				return CompareByRef(x, y, solver);
			}
		}

		[Serializable]
		public class ContactCallback : UnityEvent<ObiSolver, Oni.Contact>
		{
		}

		private ObiSolver solver;

		private Oni.Contact[] prevData;

		private int prevCount;

		private ContactComparer comparer;

		public float distanceThreshold = 0.01f;

		public ContactCallback onContactEnter = new ContactCallback();

		public ContactCallback onContactStay = new ContactCallback();

		public ContactCallback onContactExit = new ContactCallback();

		private static int CompareByRef(Oni.Contact a, Oni.Contact b, ObiSolver solver)
		{
			if (a.bodyB == b.bodyB)
			{
				int instanceID = solver.particleToActor[a.bodyA].actor.GetInstanceID();
				int instanceID2 = solver.particleToActor[b.bodyA].actor.GetInstanceID();
				return instanceID.CompareTo(instanceID2);
			}
			return a.bodyB.CompareTo(b.bodyB);
		}

		private void Awake()
		{
			solver = GetComponent<ObiSolver>();
			comparer = new ContactComparer(solver);
			prevData = new Oni.Contact[0];
		}

		private void OnEnable()
		{
			solver.OnCollision += Solver_OnCollision;
		}

		private void OnDisable()
		{
			solver.OnCollision -= Solver_OnCollision;
		}

		private int FilterOutDistantContacts(ObiNativeContactList data, int count)
		{
			int num = count;
			for (int num2 = count - 1; num2 >= 0; num2--)
			{
				if (data[num2].distance > distanceThreshold)
				{
					data.Swap(num2, --num);
				}
			}
			return num;
		}

		private int RemoveDuplicates(ObiNativeContactList data, int count)
		{
			if (count == 0)
			{
				return 0;
			}
			int num = 0;
			int num2 = 0;
			while (++num != count)
			{
				if (CompareByRef(data[num], data[num2], solver) != 0 && ++num2 != num)
				{
					data[num2] = data[num];
				}
			}
			return ++num2;
		}

		private void InvokeCallbacks(ObiNativeContactList data, int count)
		{
			int num = 0;
			int num2 = 0;
			int num3 = prevCount;
			while (num < count && num2 < num3)
			{
				int num4 = CompareByRef(data[num], prevData[num2], solver);
				if (num4 < 0)
				{
					onContactEnter.Invoke(solver, data[num++]);
					continue;
				}
				if (num4 > 0)
				{
					onContactExit.Invoke(solver, prevData[num2++]);
					continue;
				}
				onContactStay.Invoke(solver, data[num++]);
				num2++;
			}
			while (num < count)
			{
				onContactEnter.Invoke(solver, data[num++]);
			}
			while (num2 < num3)
			{
				onContactExit.Invoke(solver, prevData[num2++]);
			}
		}

		private void Solver_OnCollision(object sender, ObiNativeContactList contacts)
		{
			int num = FilterOutDistantContacts(contacts, contacts.count);
			contacts.AsNativeArray().Slice(0, num).Sort(comparer);
			num = RemoveDuplicates(contacts, num);
			InvokeCallbacks(contacts, num);
			if (num > prevData.Length)
			{
				Array.Resize(ref prevData, num);
			}
			contacts.CopyTo(prevData, 0, num);
			prevCount = num;
		}
	}
}
