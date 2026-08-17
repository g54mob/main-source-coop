using System;
using System.Collections.Generic;

namespace EvilCore.EvilPack.EvilLogger
{
	[Serializable]
	public class LogStateWrapper
	{
		public List<LogStatePair> logStates = new List<LogStatePair>();

		public LogStateWrapper()
		{
		}

		public LogStateWrapper(Dictionary<string, bool> states)
		{
			logStates = new List<LogStatePair>();
			foreach (KeyValuePair<string, bool> state in states)
			{
				logStates.Add(new LogStatePair(state.Key, state.Value));
			}
		}

		public Dictionary<string, bool> ToDictionary()
		{
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			foreach (LogStatePair logState in logStates)
			{
				dictionary[logState.key] = logState.value;
			}
			return dictionary;
		}
	}
}
