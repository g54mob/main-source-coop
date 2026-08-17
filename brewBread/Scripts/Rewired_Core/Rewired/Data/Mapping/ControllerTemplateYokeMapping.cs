using System;
using Rewired.Utils.Attributes;

namespace Rewired.Data.Mapping
{
	[Serializable]
	[Preserve]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class ControllerTemplateYokeMapping : ControllerTemplateSpecialElementMapping
	{
		public int eid_axisX = -1;

		public int eid_axisZ = -1;
	}
}
