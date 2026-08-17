namespace QFSW.QC
{
	public struct ResponseConfig
	{
		public string InputPrompt;

		public bool LogInput;

		public static readonly ResponseConfig Default = new ResponseConfig
		{
			InputPrompt = "Enter input...",
			LogInput = true
		};
	}
}
