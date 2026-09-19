using System;
using Cysharp.Threading.Tasks;

namespace Features.SessionManagementModule.Models
{
	public abstract class SessionStateBase : ISessionState
	{
		private ISessionStateContext _context;

		public abstract SessionState State { get; }

		public abstract SessionAuthorityLane ActivateLane { get; }

		public abstract SessionAuthorityLane DeactivateLane { get; }

		protected bool IsTargetStateActive
		{
			get
			{
				return _context.IsTargetStateActive;
			}
			set
			{
				_context.IsTargetStateActive = value;
			}
		}

		protected SessionState PreviousState => _context.PreviousState;

		public void Bind(ISessionStateContext context)
		{
			_context = context;
		}

		public abstract UniTask EnterAsync();

		public virtual void UpdateActive()
		{
		}

		public virtual void UpdateActiveLocal()
		{
		}

		public abstract UniTask ExitAsync(SessionState next);

		protected void Transition(SessionState target)
		{
			_context.Transition(target);
		}

		protected UniTask AuthorityGate(SessionAuthorityLane lane, Action work)
		{
			return _context.AuthorityGate(lane, work);
		}

		protected UniTask AuthorityGate(SessionAuthorityLane lane, Func<UniTask> work)
		{
			return _context.AuthorityGate(lane, work);
		}

		protected UniTask AuthorityGate(SessionAuthorityLane lane, Action<int> work)
		{
			return _context.AuthorityGate(lane, work);
		}

		protected UniTask AuthorityGate(SessionAuthorityLane lane, Func<int, UniTask> work)
		{
			return _context.AuthorityGate(lane, work);
		}

		protected UniTask SynchronizationGate(SynchronizationGateKey key)
		{
			return _context.SynchronizationGate(key);
		}
	}
}
