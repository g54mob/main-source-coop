namespace QFSW.QC
{
	public interface IQcPreprocessor
	{
		int Priority { get; }

		string Process(string text);
	}
}
