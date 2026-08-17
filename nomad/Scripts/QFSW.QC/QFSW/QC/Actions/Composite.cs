using System.Collections.Generic;

namespace QFSW.QC.Actions
{
	public class Composite : ICommandAction
	{
		private ActionContext _context;

		private readonly IEnumerator<ICommandAction> _actions;

		public bool IsFinished => _actions.Execute(_context) == ActionState.Complete;

		public bool StartsIdle => false;

		public Composite(IEnumerator<ICommandAction> actions)
		{
			_actions = actions;
		}

		public Composite(IEnumerable<ICommandAction> actions)
			: this(actions.GetEnumerator())
		{
		}

		public void Start(ActionContext context)
		{
			_context = context;
		}

		public void Finalize(ActionContext context)
		{
		}
	}
}
