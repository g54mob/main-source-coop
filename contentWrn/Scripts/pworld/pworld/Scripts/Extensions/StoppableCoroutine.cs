using System.Collections;
using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public class StoppableCoroutine
	{
		private readonly MonoBehaviour mb;

		private readonly Coroutine nested;

		private readonly IEnumerator payload;

		private bool terminated;

		public bool wasStopped;

		public StoppableCoroutine(MonoBehaviour mb, IEnumerator aCoroutine)
		{
			payload = aCoroutine;
			nested = mb.StartCoroutine(Wrapper());
			this.mb = mb;
		}

		public Coroutine WaitFor()
		{
			return mb.StartCoroutine(Wait());
		}

		public void Stop()
		{
			terminated = true;
			wasStopped = true;
			mb.StopCoroutine(nested);
		}

		private IEnumerator Wrapper()
		{
			while (payload.MoveNext())
			{
				yield return payload.Current;
			}
			terminated = true;
		}

		private IEnumerator Wait()
		{
			while (!terminated)
			{
				yield return null;
			}
		}
	}
}
