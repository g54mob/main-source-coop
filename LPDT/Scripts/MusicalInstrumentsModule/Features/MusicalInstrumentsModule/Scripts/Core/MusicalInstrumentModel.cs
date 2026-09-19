using System.Collections.Generic;

namespace Features.MusicalInstrumentsModule.Scripts.Core
{
	public class MusicalInstrumentModel
	{
		private readonly List<IMusicalInstrumentTimelineState> _instruments = new List<IMusicalInstrumentTimelineState>();

		public void Register(IMusicalInstrumentTimelineState instrument)
		{
			if (!_instruments.Contains(instrument))
			{
				_instruments.Add(instrument);
			}
		}

		public void Unregister(IMusicalInstrumentTimelineState instrument)
		{
			_instruments.Remove(instrument);
		}

		public bool TryGetEarliestActiveStartTick(IMusicalInstrumentTimelineState excludedInstrument, out int startTick)
		{
			startTick = 0;
			bool flag = false;
			foreach (IMusicalInstrumentTimelineState instrument in _instruments)
			{
				if (instrument != excludedInstrument && instrument.IsPlaying && (!flag || instrument.PerformanceStartTick < startTick))
				{
					startTick = instrument.PerformanceStartTick;
					flag = true;
				}
			}
			return flag;
		}
	}
}
