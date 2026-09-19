using System;

namespace GameAnalyticsSDK.Net
{
	internal class TimedBlock : IComparable<TimedBlock>
	{
		public readonly DateTime deadline;

		public readonly Action block;

		public readonly string blockName;

		public TimedBlock(DateTime deadline, Action block, string blockName)
		{
			this.deadline = deadline;
			this.block = block;
			this.blockName = blockName;
		}

		public int CompareTo(TimedBlock other)
		{
			DateTime dateTime = deadline;
			return dateTime.CompareTo(other.deadline);
		}

		public override string ToString()
		{
			string[] obj = new string[5] { "{TimedBlock: deadLine=", null, null, null, null };
			DateTime dateTime = deadline;
			obj[1] = dateTime.Ticks.ToString();
			obj[2] = ", block=";
			obj[3] = blockName;
			obj[4] = "}";
			return string.Concat(obj);
		}
	}
}
