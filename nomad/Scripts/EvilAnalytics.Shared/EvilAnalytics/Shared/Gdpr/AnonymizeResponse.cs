using System;

namespace EvilAnalytics.Shared.Gdpr
{
	public class AnonymizeResponse
	{
		public bool Success { get; set; }

		public DateTimeOffset AnonymizedAt { get; set; }

		public AnonymizedRecordCounts Records { get; set; } = new AnonymizedRecordCounts();
	}
}
