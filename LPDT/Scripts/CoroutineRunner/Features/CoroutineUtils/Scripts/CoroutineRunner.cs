using System.Collections;
using UnityEngine;

namespace Features.CoroutineUtils.Scripts
{
	public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
	{
		public bool IsActive { get; private set; } = true;

		public void OnDestroy()
		{
			IsActive = false;
			StopAllCoroutines();
		}

		Coroutine ICoroutineRunner.StartCoroutine(IEnumerator coroutine)
		{
			if (this == null || coroutine == null)
			{
				return null;
			}
			return StartCoroutine(coroutine);
		}

		void ICoroutineRunner.StopCoroutine(Coroutine coroutine)
		{
			if (!(this == null) && coroutine != null)
			{
				StopCoroutine(coroutine);
			}
		}
	}
}
