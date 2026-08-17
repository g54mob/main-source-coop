using System;

namespace QFSW.QC.Actions
{
	public class GetContext : ICommandAction
	{
		private readonly Action<ActionContext> _onContext;

		public bool IsFinished => true;

		public bool StartsIdle => false;

		public GetContext(Action<ActionContext> onContext)
		{
			_onContext = onContext;
		}

		public void Start(ActionContext context)
		{
		}

		public void Finalize(ActionContext context)
		{
			_onContext(context);
		}
	}
}
