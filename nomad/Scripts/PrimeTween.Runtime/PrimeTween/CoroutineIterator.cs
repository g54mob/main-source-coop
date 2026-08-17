using System;
using System.Collections;
using UnityEngine;

namespace PrimeTween
{
	internal class CoroutineIterator : IEnumerator
	{
		internal Tween _tween;

		object IEnumerator.Current => null;

		bool IEnumerator.MoveNext()
		{
			if (!_tween.IsCreated)
			{
				Debug.LogError("IEnumerator.MoveNext() was called after animation coroutine has finished. This is not allowed.");
				return false;
			}
			if (_tween.isAlive)
			{
				return true;
			}
			_tween = default(Tween);
			PrimeTweenManager.Instance._coroutineIterators.Add(this);
			return false;
		}

		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}
}
