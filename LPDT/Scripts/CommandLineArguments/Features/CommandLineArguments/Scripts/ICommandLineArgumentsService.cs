namespace Features.CommandLineArguments.Scripts
{
	public interface ICommandLineArgumentsService
	{
		bool HasArgument(string argumentName);

		string GetString(string argumentName, string defaultValue = null);

		int GetInt(string argumentName, int defaultValue = 0);

		float GetFloat(string argumentName, float defaultValue = float.NaN);
	}
}
