using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace Fusion
{
	public sealed class FusionCoroutine : ICoroutine, IAsyncOperation, IEnumerator, IDisposable
	{
		private readonly IEnumerator _inner;

		private Action<IAsyncOperation> _completed;

		public bool IsDone { get; private set; }

		public float Progress { get; private set; }

		public ExceptionDispatchInfo Error { get; private set; }

		object IEnumerator.Current => _inner.Current;

		public event Action<IAsyncOperation> Completed
		{
			add
			{
				_completed = (Action<IAsyncOperation>)Delegate.Combine(_completed, value);
				if (IsDone)
				{
					value(this);
				}
			}
			remove
			{
				_completed = (Action<IAsyncOperation>)Delegate.Remove(_completed, value);
			}
		}

		public FusionCoroutine(IEnumerator inner)
		{
			_inner = inner ?? throw new ArgumentNullException("inner");
		}

		bool IEnumerator.MoveNext()
		{
			try
			{
				if (_inner.MoveNext())
				{
					return true;
				}
				Progress = 1f;
				IsDone = true;
			}
			catch (Exception source)
			{
				IsDone = true;
				Error = ExceptionDispatchInfo.Capture(source);
			}
			Action<IAsyncOperation> completed = _completed;
			if (completed != null)
			{
				List<Exception> list = null;
				Delegate[] invocationList = completed.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					Action<IAsyncOperation> action = (Action<IAsyncOperation>)invocationList[i];
					try
					{
						action(this);
					}
					catch (Exception item)
					{
						if (list == null)
						{
							list = new List<Exception>();
						}
						list.Add(item);
					}
				}
				if (list != null)
				{
					throw new AggregateException("Error during Completed", list.ToArray());
				}
			}
			return false;
		}

		void IEnumerator.Reset()
		{
			_inner.Reset();
			IsDone = false;
			Progress = 0f;
			Error = null;
			_completed = null;
		}

		public void Dispose()
		{
			if (_inner is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
	}
}
