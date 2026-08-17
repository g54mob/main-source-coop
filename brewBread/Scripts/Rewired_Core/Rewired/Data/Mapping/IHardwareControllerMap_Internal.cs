using System.Collections.Generic;
using Rewired.Interfaces;

namespace Rewired.Data.Mapping
{
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = false)]
	[CustomObfuscation(rename = false)]
	internal interface IHardwareControllerMap_Internal
	{
		IEnumerable<IControllerElementIdentifierCommon_Internal> ElementIdentifiers { get; }

		IControllerElementIdentifierCommon_Internal GetElementIdentifier(int id);
	}
}
