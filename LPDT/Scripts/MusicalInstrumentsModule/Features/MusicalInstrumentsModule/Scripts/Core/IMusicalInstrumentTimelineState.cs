namespace Features.MusicalInstrumentsModule.Scripts.Core
{
	public interface IMusicalInstrumentTimelineState
	{
		bool IsPlaying { get; }

		int PerformanceStartTick { get; }
	}
}
