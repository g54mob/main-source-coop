using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading;

public class DeferredSynchronizeInvoke : ISynchronizeInvoke
{
	private class UnityAsyncResult : IAsyncResult
	{
		public Delegate method;

		public object[] args;

		public bool IsCompleted { get; set; }

		public WaitHandle AsyncWaitHandle { get; internal set; }

		public object AsyncState { get; set; }

		public bool CompletedSynchronously => IsCompleted;
	}

	private Queue<UnityAsyncResult> fifoToExecute = new Queue<UnityAsyncResult>();

	private Thread mainThread;

	public bool InvokeRequired => mainThread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId;

	public DeferredSynchronizeInvoke()
	{
		mainThread = Thread.CurrentThread;
	}

	public IAsyncResult BeginInvoke(Delegate method, object[] args)
	{
		UnityAsyncResult unityAsyncResult = new UnityAsyncResult
		{
			method = method,
			args = args,
			IsCompleted = false,
			AsyncWaitHandle = new ManualResetEvent(initialState: false)
		};
		lock (fifoToExecute)
		{
			fifoToExecute.Enqueue(unityAsyncResult);
			return unityAsyncResult;
		}
	}

	public object EndInvoke(IAsyncResult result)
	{
		if (!result.IsCompleted)
		{
			result.AsyncWaitHandle.WaitOne();
		}
		return result.AsyncState;
	}

	public object Invoke(Delegate method, object[] args)
	{
		if (InvokeRequired)
		{
			IAsyncResult result = BeginInvoke(method, args);
			return EndInvoke(result);
		}
		return method.DynamicInvoke(args);
	}

	public void ProcessQueue()
	{
		if (Thread.CurrentThread != mainThread)
		{
			throw new TargetException(GetType()?.ToString() + "." + MethodBase.GetCurrentMethod().Name + "() must be called from the same thread it was created on (created on thread id: " + mainThread.ManagedThreadId + ", called from thread id: " + Thread.CurrentThread.ManagedThreadId);
		}
		bool flag = true;
		UnityAsyncResult unityAsyncResult = null;
		while (flag)
		{
			lock (fifoToExecute)
			{
				flag = fifoToExecute.Count > 0;
				if (!flag)
				{
					break;
				}
				unityAsyncResult = fifoToExecute.Dequeue();
			}
			unityAsyncResult.AsyncState = Invoke(unityAsyncResult.method, unityAsyncResult.args);
			unityAsyncResult.IsCompleted = true;
		}
	}
}
