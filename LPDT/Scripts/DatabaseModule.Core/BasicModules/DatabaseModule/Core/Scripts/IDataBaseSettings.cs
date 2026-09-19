namespace BasicModules.DatabaseModule.Core.Scripts
{
	public interface IDataBaseSettings
	{
		string GetConnectionString();

		string GetJsonFullPath();
	}
}
