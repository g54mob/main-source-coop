using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Foundation.Tasks
{
	public class AsyncTask : CustomYieldInstruction, IDisposable
	{
		public static bool DisableMultiThread;

		public static bool LogErrors;

		public TaskStrategy Strategy;

		protected TaskStatus _status;

		protected Action _action;

		protected IEnumerator _routine;

		private List<Delegate> _completeList;

		private static AsyncTask _successTask;

		public Exception Exception { get; set; }

		public TaskStatus Status { get; set; }

		public override bool keepWaiting => !IsCompleted;

		public bool IsRunning => Status == TaskStatus.Pending;

		public bool IsCompleted
		{
			get
			{
				if (Status == TaskStatus.Success || Status == TaskStatus.Faulted)
				{
					return !HasContinuations;
				}
				return false;
			}
		}

		public bool IsFaulted => Status == TaskStatus.Faulted;

		public bool IsSuccess => Status == TaskStatus.Success;

		public bool HasContinuations { get; protected set; }

		static AsyncTask()
		{
			DisableMultiThread = false;
			LogErrors = false;
			_successTask = new AsyncTask(TaskStrategy.Custom)
			{
				Status = TaskStatus.Success
			};
			TaskManager.ConfirmInit();
		}

		public AsyncTask()
		{
		}

		public AsyncTask(TaskStrategy mode)
		{
			Strategy = mode;
		}

		public AsyncTask(Exception ex)
		{
			Exception = ex;
			Strategy = TaskStrategy.Custom;
			Status = TaskStatus.Faulted;
		}

		public AsyncTask(Action action)
		{
			_action = action;
			Strategy = TaskStrategy.BackgroundThread;
		}

		public AsyncTask(Action action, TaskStrategy mode)
			: this()
		{
			if (mode == TaskStrategy.Coroutine)
			{
				throw new ArgumentException("Action tasks may not be coroutines");
			}
			_action = action;
			Strategy = mode;
		}

		public AsyncTask(IEnumerator action)
			: this()
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			_routine = action;
			Strategy = TaskStrategy.Coroutine;
		}

		protected virtual void Execute()
		{
			try
			{
				if (_action != null)
				{
					_action();
				}
				Status = TaskStatus.Success;
				OnTaskComplete();
			}
			catch (Exception ex)
			{
				Exception exception = (Exception = ex);
				Status = TaskStatus.Faulted;
				if (LogErrors)
				{
					Debug.LogException(exception);
				}
			}
		}

		protected void RunOnBackgroundThread()
		{
			Status = TaskStatus.Pending;
			ThreadPool.QueueUserWorkItem(delegate
			{
				Execute();
			});
		}

		protected void RunOnCurrentThread()
		{
			Status = TaskStatus.Pending;
			Execute();
		}

		protected void RunOnMainThread()
		{
			Status = TaskStatus.Pending;
			TaskManager.RunOnMainThread(Execute);
		}

		protected void RunAsCoroutine()
		{
			Status = TaskStatus.Pending;
			TaskManager.StartRoutine(new TaskManager.CoroutineCommand
			{
				Coroutine = _routine,
				OnComplete = OnRoutineComplete
			});
		}

		protected virtual void OnTaskComplete()
		{
			if (_completeList != null)
			{
				foreach (Delegate complete in _completeList)
				{
					complete?.DynamicInvoke(this);
				}
				_completeList = null;
			}
			HasContinuations = false;
		}

		protected void OnRoutineComplete()
		{
			if (Status == TaskStatus.Pending)
			{
				Status = TaskStatus.Success;
				OnTaskComplete();
			}
		}

		public virtual void Complete(Exception ex = null)
		{
			if (ex == null)
			{
				Exception = null;
				Status = TaskStatus.Success;
				OnTaskComplete();
			}
			else
			{
				Exception = ex;
				Status = TaskStatus.Faulted;
				OnTaskComplete();
			}
		}

		public virtual void Start()
		{
			Status = TaskStatus.Pending;
			switch (Strategy)
			{
			case TaskStrategy.Coroutine:
				RunAsCoroutine();
				break;
			case TaskStrategy.BackgroundThread:
				if (DisableMultiThread)
				{
					RunOnCurrentThread();
				}
				else
				{
					RunOnBackgroundThread();
				}
				break;
			case TaskStrategy.CurrentThread:
				RunOnCurrentThread();
				break;
			case TaskStrategy.MainThread:
				RunOnMainThread();
				break;
			case TaskStrategy.Custom:
				break;
			}
		}

		public virtual void Dispose()
		{
			Status = TaskStatus.Pending;
			Exception = null;
			_action = null;
			_routine = null;
			_completeList = null;
			HasContinuations = false;
		}

		public void AddContinue(Delegate action)
		{
			HasContinuations = true;
			if (_completeList == null)
			{
				_completeList = new List<Delegate>();
			}
			_completeList.Add(action);
		}

		public static AsyncTask Run(Action action)
		{
			AsyncTask asyncTask = new AsyncTask(action);
			asyncTask.Start();
			return asyncTask;
		}

		public static AsyncTask RunOnMain(Action action)
		{
			AsyncTask asyncTask = new AsyncTask(action, TaskStrategy.MainThread);
			asyncTask.Start();
			return asyncTask;
		}

		public static AsyncTask RunOnCurrent(Action action)
		{
			AsyncTask asyncTask = new AsyncTask(action, TaskStrategy.CurrentThread);
			asyncTask.Start();
			return asyncTask;
		}

		public static AsyncTask RunCoroutine(IEnumerator function)
		{
			AsyncTask asyncTask = new AsyncTask(function);
			asyncTask.Start();
			return asyncTask;
		}

		public static AsyncTask RunCoroutine(Func<IEnumerator> function)
		{
			AsyncTask asyncTask = new AsyncTask(function());
			asyncTask.Start();
			return asyncTask;
		}

		public static AsyncTask RunCoroutine(Func<AsyncTask, IEnumerator> function)
		{
			AsyncTask asyncTask = new AsyncTask();
			asyncTask.Strategy = TaskStrategy.Coroutine;
			asyncTask._routine = function(asyncTask);
			asyncTask.Start();
			return asyncTask;
		}

		public static AsyncTask<TResult> Run<TResult>(Func<TResult> function)
		{
			AsyncTask<TResult> asyncTask = new AsyncTask<TResult>(function);
			asyncTask.Start();
			return asyncTask;
		}

		public static AsyncTask<TResult> RunOnMain<TResult>(Func<TResult> function)
		{
			AsyncTask<TResult> asyncTask = new AsyncTask<TResult>(function, TaskStrategy.MainThread);
			asyncTask.Start();
			return asyncTask;
		}

		public static AsyncTask<TResult> RunOnCurrent<TResult>(Func<TResult> function)
		{
			AsyncTask<TResult> asyncTask = new AsyncTask<TResult>(function, TaskStrategy.CurrentThread);
			asyncTask.Start();
			return asyncTask;
		}

		public static AsyncTask<TResult> RunCoroutine<TResult>(IEnumerator function)
		{
			AsyncTask<TResult> asyncTask = new AsyncTask<TResult>(function);
			asyncTask.Start();
			return asyncTask;
		}

		public static AsyncTask<TResult> RunCoroutine<TResult>(Func<AsyncTask<TResult>, IEnumerator> function)
		{
			AsyncTask<TResult> asyncTask = new AsyncTask<TResult>();
			asyncTask.Strategy = TaskStrategy.Coroutine;
			asyncTask._routine = function(asyncTask);
			asyncTask.Start();
			return asyncTask;
		}

		public static AsyncTask<T> SuccessTask<T>(T result)
		{
			return new AsyncTask<T>(TaskStrategy.Custom)
			{
				Status = TaskStatus.Success,
				Result = result
			};
		}

		public static AsyncTask SuccessTask()
		{
			return _successTask;
		}

		public static AsyncTask FailedTask(string exception)
		{
			return FailedTask(new Exception(exception));
		}

		public static AsyncTask FailedTask(Exception ex)
		{
			return new AsyncTask(TaskStrategy.Custom)
			{
				Status = TaskStatus.Faulted,
				Exception = ex
			};
		}

		public static AsyncTask<T> FailedTask<T>(string exception)
		{
			return FailedTask<T>(new Exception(exception));
		}

		public static AsyncTask<T> FailedTask<T>(Exception ex)
		{
			return new AsyncTask<T>(TaskStrategy.Custom)
			{
				Status = TaskStatus.Faulted,
				Exception = ex
			};
		}
	}
	public class AsyncTask<TResult> : AsyncTask
	{
		public TResult Result;

		private Func<TResult> _function;

		public AsyncTask()
		{
		}

		public AsyncTask(TResult result)
			: this()
		{
			base.Status = TaskStatus.Success;
			Strategy = TaskStrategy.Custom;
			Result = result;
		}

		public AsyncTask(Func<TResult> function)
			: this()
		{
			if (function == null)
			{
				throw new ArgumentNullException("function");
			}
			_function = function;
		}

		public AsyncTask(Func<TResult> function, TaskStrategy mode)
			: this()
		{
			if (function == null)
			{
				throw new ArgumentNullException("function");
			}
			if (mode == TaskStrategy.Coroutine)
			{
				throw new ArgumentException("Mode can not be coroutine");
			}
			_function = function;
			Strategy = mode;
		}

		public AsyncTask(IEnumerator routine)
		{
			if (routine == null)
			{
				throw new ArgumentNullException("routine");
			}
			_routine = routine;
			Strategy = TaskStrategy.Coroutine;
		}

		public AsyncTask(Exception ex)
		{
			base.Exception = ex;
			Strategy = TaskStrategy.Custom;
			base.Status = TaskStatus.Faulted;
		}

		public AsyncTask(TaskStrategy mode)
			: this()
		{
			Strategy = mode;
		}

		public override void Complete(Exception ex = null)
		{
			Result = default(TResult);
			base.Complete(ex);
		}

		public void Complete(TResult result)
		{
			Result = result;
			base.Complete();
		}

		public override void Start()
		{
			Result = default(TResult);
			base.Start();
		}

		protected override void Execute()
		{
			try
			{
				if (_function != null)
				{
					Result = _function();
				}
				base.Status = TaskStatus.Success;
				OnTaskComplete();
			}
			catch (Exception ex)
			{
				Exception exception = (base.Exception = ex);
				base.Status = TaskStatus.Faulted;
				if (AsyncTask.LogErrors)
				{
					Debug.LogException(exception);
				}
			}
		}
	}
}
