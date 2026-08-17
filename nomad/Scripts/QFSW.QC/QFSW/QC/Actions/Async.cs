using System;
using System.Threading.Tasks;

namespace QFSW.QC.Actions
{
	public class Async : ICommandAction
	{
		private readonly Task _task;

		public bool IsFinished
		{
			get
			{
				if (!_task.IsCompleted && !_task.IsCanceled)
				{
					return _task.IsFaulted;
				}
				return true;
			}
		}

		public bool StartsIdle => false;

		public Async(Task task)
		{
			_task = task;
		}

		public void Start(ActionContext context)
		{
		}

		public void Finalize(ActionContext context)
		{
			if (_task.IsFaulted)
			{
				throw _task.Exception.InnerException;
			}
			if (_task.IsCanceled)
			{
				throw new TaskCanceledException();
			}
		}
	}
	public class Async<T> : ICommandAction
	{
		private readonly Task<T> _task;

		private readonly Action<T> _onResult;

		public bool IsFinished
		{
			get
			{
				if (!_task.IsCompleted && !_task.IsCanceled)
				{
					return _task.IsFaulted;
				}
				return true;
			}
		}

		public bool StartsIdle => false;

		public Async(Task<T> task, Action<T> onResult)
		{
			_task = task;
			_onResult = onResult;
		}

		public void Start(ActionContext context)
		{
		}

		public void Finalize(ActionContext context)
		{
			if (_task.IsFaulted)
			{
				throw _task.Exception.InnerException;
			}
			if (_task.IsCanceled)
			{
				throw new TaskCanceledException();
			}
			_onResult(_task.Result);
		}
	}
}
