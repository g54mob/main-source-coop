using System.Xml;

namespace Rewired.Utils.Interfaces
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal interface IExportToXml
	{
		bool writesOwnElementTag { get; }

		void WriteXml(XmlWriter writer);
	}
}
