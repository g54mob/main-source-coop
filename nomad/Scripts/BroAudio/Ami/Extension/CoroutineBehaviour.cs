using System.Collections;
using UnityEngine;
using UnityEngine.Internal;

namespace Ami.Extension
{
	public abstract class CoroutineBehaviour
	{
		private MonoBehaviour _mono;

		public CoroutineBehaviour(MonoBehaviour mono)
		{
			_mono = mono;
		}

		public Coroutine StartCoroutine(string methodName)
		{
			return _mono.CheckNull()?.StartCoroutine(methodName);
		}

		public Coroutine StartCoroutine(IEnumerator routine)
		{
			return _mono.CheckNull()?.StartCoroutine(routine);
		}

		public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
		{
			return _mono.CheckNull()?.StartCoroutine(methodName, value);
		}

		public void StopAllCoroutines()
		{
			_mono.CheckNull()?.StopAllCoroutines();
		}

		public void StopCoroutine(IEnumerator routine)
		{
			_mono.CheckNull()?.StopCoroutine(routine);
		}

		public void StopCoroutine(Coroutine routine)
		{
			_mono.CheckNull()?.StopCoroutine(routine);
		}

		public void StopCoroutine(string methodName)
		{
			_mono.CheckNull()?.StopCoroutine(methodName);
		}

		public void StartCoroutineAndReassign(IEnumerator enumerator, ref Coroutine coroutine)
		{
			_mono.CheckNull()?.StartCoroutineAndReassign(enumerator, ref coroutine);
		}

		public void SafeStopCoroutine(Coroutine coroutine)
		{
			_mono.CheckNull()?.SafeStopCoroutine(coroutine);
		}
	}
}
