using EvilCore.EvilSave;

namespace NomadDrive.Features.SaveSystem
{
	public interface INetworkSaveable
	{
		string ContributorKey { get; }

		void CaptureState(EvilWriter writer, ISaveContext ctx);

		void RestoreSelfState(EvilReader reader, ISaveContext ctx);

		void RestoreLinks(EvilReader reader, ISaveContext ctx);
	}
}
