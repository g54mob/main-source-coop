using System.Collections;
using UnityEngine;

namespace Features.CoroutineUtils.Scripts
{
	public interface ICoroutineRunner
	{
		bool IsActive { get; }

		Coroutine StartCoroutine(IEnumerator coroutine);

		void StopCoroutine(Coroutine coroutine);
	}
}
