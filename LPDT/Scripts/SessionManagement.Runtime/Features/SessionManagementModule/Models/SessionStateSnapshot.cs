using System;

namespace Features.SessionManagementModule.Models
{
	public sealed class SessionStateSnapshot
	{
		private double _globalEnteredAt;

		private double _localEnteredAt;

		private double _localSubstateEnteredAt;

		public bool IsActive { get; private set; }

		public SessionState GlobalState { get; private set; }

		public bool TargetActive { get; private set; }

		public bool HasAuthority { get; private set; }

		public SessionState LocalState { get; private set; }

		public SessionSubstate LocalSubstate { get; private set; }

		public bool WasRosterRejected { get; private set; }

		public string GlobalStateName => GlobalState.ToString();

		public string LocalStateName => LocalState.ToString();

		public string LocalSubstateName => LocalSubstate.ToString();

		public event Action RosterRejected;

		public double SecondsInGlobalState(double now)
		{
			if (!IsActive)
			{
				return 0.0;
			}
			return now - _globalEnteredAt;
		}

		public double SecondsInLocalState(double now)
		{
			if (!IsActive)
			{
				return 0.0;
			}
			return now - _localEnteredAt;
		}

		public double SecondsInLocalSubstate(double now)
		{
			if (!IsActive)
			{
				return 0.0;
			}
			return now - _localSubstateEnteredAt;
		}

		public void Activate(double now)
		{
			IsActive = true;
			GlobalState = SessionState.None;
			TargetActive = false;
			HasAuthority = false;
			LocalState = SessionState.None;
			LocalSubstate = SessionSubstate.None;
			WasRosterRejected = false;
			_globalEnteredAt = now;
			_localEnteredAt = now;
			_localSubstateEnteredAt = now;
		}

		public void MarkRosterRejected()
		{
			WasRosterRejected = true;
			this.RosterRejected?.Invoke();
		}

		public void Observe(SessionState global, bool targetActive, bool hasAuthority, SessionState local, SessionSubstate substate, double now)
		{
			if (!IsActive)
			{
				Activate(now);
			}
			TargetActive = targetActive;
			HasAuthority = hasAuthority;
			if (global != GlobalState)
			{
				GlobalState = global;
				_globalEnteredAt = now;
			}
			if (local != LocalState)
			{
				LocalState = local;
				_localEnteredAt = now;
			}
			if (substate != LocalSubstate)
			{
				LocalSubstate = substate;
				_localSubstateEnteredAt = now;
			}
		}

		public void Deactivate()
		{
			IsActive = false;
			GlobalState = SessionState.None;
			TargetActive = false;
			HasAuthority = false;
			LocalState = SessionState.None;
			LocalSubstate = SessionSubstate.None;
		}
	}
}
