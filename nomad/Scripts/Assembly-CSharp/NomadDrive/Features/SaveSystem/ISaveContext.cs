using System;

namespace NomadDrive.Features.SaveSystem
{
	public interface ISaveContext
	{
		DateTime WorldSaveTimeUtc { get; }

		string ToGuid(uint netId);

		bool TryToNetId(string guid, out uint netId);
	}
}
