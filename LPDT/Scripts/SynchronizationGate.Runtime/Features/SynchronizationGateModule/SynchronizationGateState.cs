namespace Features.SynchronizationGateModule
{
	public readonly struct SynchronizationGateState
	{
		internal GateStateType State { get; }

		public int ArrivedCount { get; }

		public int ParticipantCount { get; }

		public int TicksRemaining { get; }

		public bool IsResolved
		{
			get
			{
				if (State != GateStateType.Passed)
				{
					return State == GateStateType.TimedOut;
				}
				return true;
			}
		}

		internal SynchronizationGateState(GateStateType state, int arrivedCount, int participantCount, int ticksRemaining)
		{
			State = state;
			ArrivedCount = arrivedCount;
			ParticipantCount = participantCount;
			TicksRemaining = ticksRemaining;
		}
	}
}
