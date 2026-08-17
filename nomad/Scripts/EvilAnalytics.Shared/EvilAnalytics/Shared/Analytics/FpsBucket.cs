namespace EvilAnalytics.Shared.Analytics
{
	public class FpsBucket
	{
		public string Range { get; set; } = string.Empty;

		public int MinFps { get; set; }

		public int MaxFps { get; set; }

		public int PlayerCount { get; set; }

		public int SessionCount { get; set; }

		public double Percentage { get; set; }
	}
}
