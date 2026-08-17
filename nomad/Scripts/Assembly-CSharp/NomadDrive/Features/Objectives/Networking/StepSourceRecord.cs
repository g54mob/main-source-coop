using System;

namespace NomadDrive.Features.Objectives.Networking
{
	[Serializable]
	public struct StepSourceRecord
	{
		public string ObjectiveId;

		public string StepId;

		public string SourceId;
	}
}
