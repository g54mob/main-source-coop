namespace Google.Apis.Discovery
{
	public interface IParameter
	{
		string Name { get; }

		string Pattern { get; }

		bool IsRequired { get; }

		string DefaultValue { get; }

		string ParameterType { get; }
	}
}
