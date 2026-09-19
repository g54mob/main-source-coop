namespace Features.EnumHelpersModule.Scripts
{
	public interface IEnumParser
	{
		T Parse<T>(string value) where T : struct;

		bool TryParse<T>(string value, out T parsedEnum) where T : struct;
	}
}
