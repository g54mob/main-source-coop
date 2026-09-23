namespace Mimicraft.Dev
{
	public sealed class DevCommand
	{
		public readonly string Name;

		public readonly string Usage;

		public readonly string Help;

		public readonly DevCommandHandler Run;

		public readonly bool Cheat;

		public DevCommand(string name, string usage, string help, DevCommandHandler run, bool cheat = false)
		{
			Name = name;
			Usage = usage;
			Help = help;
			Run = run;
			Cheat = cheat;
		}
	}
}
