using System;
using System.Collections.Generic;
using UnityHFSM.Exceptions;

namespace UnityHFSM
{
	public class ActionStorage<TEvent>
	{
		private readonly Dictionary<TEvent, Delegate> actionsByEvent = new Dictionary<TEvent, Delegate>();

		private TTarget TryGetAndCastAction<TTarget>(TEvent trigger) where TTarget : Delegate
		{
			Delegate value = null;
			actionsByEvent.TryGetValue(trigger, out value);
			if ((object)value == null)
			{
				return null;
			}
			return (value as TTarget) ?? throw new InvalidOperationException(ExceptionFormatter.Format($"Trying to call the action '{trigger}'.", $"The expected argument type ({typeof(TTarget)}) does not match the " + $"type of the added action ({value}).", "Check that the type of action that was added matches the type of action that is called. \nE.g. AddAction<int>(...) => OnAction<int>(...) \nE.g. AddAction(...) => OnAction(...) \nE.g. NOT: AddAction<int>(...) => OnAction<bool>(...)"));
		}

		public void AddAction(TEvent trigger, Action action)
		{
			actionsByEvent[trigger] = action;
		}

		public void AddAction<TData>(TEvent trigger, Action<TData> action)
		{
			actionsByEvent[trigger] = action;
		}

		public void RunAction(TEvent trigger)
		{
			TryGetAndCastAction<Action>(trigger)?.Invoke();
		}

		public void RunAction<TData>(TEvent trigger, TData data)
		{
			TryGetAndCastAction<Action<TData>>(trigger)?.Invoke(data);
		}

		public bool HasAction(TEvent trigger)
		{
			return actionsByEvent.ContainsKey(trigger);
		}
	}
}
