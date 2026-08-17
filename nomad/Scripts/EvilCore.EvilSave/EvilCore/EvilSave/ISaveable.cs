namespace EvilCore.EvilSave
{
	public interface ISaveable
	{
		string SaveId { get; }

		void OnSave(EvilWriter writer);

		void OnLoad(EvilReader reader);
	}
}
