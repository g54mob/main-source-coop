using System.Collections.Generic;

namespace QFSW.QC
{
	public static class ActionExecuter
	{
		public static ActionState Execute(this IEnumerator<ICommandAction> action, ActionContext context)
		{
			ActionState state = ActionState.Running;
			bool idle = false;
			while (!idle)
			{
				if (action.Current == null)
				{
					MoveNext();
				}
				else if (action.Current.IsFinished)
				{
					action.Current.Finalize(context);
					MoveNext();
				}
				else
				{
					idle = true;
				}
			}
			return state;
			void MoveNext()
			{
				if (action.MoveNext())
				{
					action.Current?.Start(context);
					idle = action.Current?.StartsIdle ?? false;
				}
				else
				{
					idle = true;
					state = ActionState.Complete;
					action.Dispose();
				}
			}
		}
	}
}
