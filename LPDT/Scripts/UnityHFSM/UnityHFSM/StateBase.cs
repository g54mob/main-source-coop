using UnityHFSM.Inspection;

namespace UnityHFSM
{
	public class StateBase<TStateId> : IVisitableState
	{
		public bool needsExitTime;

		public bool isGhostState;

		public TStateId name;

		public IStateTimingManager fsm;

		public StateBase(bool needsExitTime, bool isGhostState = false)
		{
			this.needsExitTime = needsExitTime;
			this.isGhostState = isGhostState;
		}

		public virtual void Init()
		{
		}

		public virtual void OnEnter()
		{
		}

		public virtual void OnLogic()
		{
		}

		public virtual void OnExit()
		{
		}

		public virtual void OnExitRequest()
		{
		}

		public virtual string GetActiveHierarchyPath()
		{
			return name.ToString();
		}

		public virtual void AcceptVisitor(IStateVisitor visitor)
		{
			visitor.VisitRegularState(this);
		}
	}
	public class StateBase : StateBase<string>
	{
		public StateBase(bool needsExitTime, bool isGhostState = false)
			: base(needsExitTime, isGhostState)
		{
		}
	}
}
