using System.Collections.Generic;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	internal interface IControllerTemplateElement_Internal
	{
		IControllerTemplate parent { get; }

		int elementCount { get; }

		IControllerTemplateElement GetElement(int index);

		int GetElementTargets(ControllerElementTarget find, ref IList<ControllerTemplateElementTarget> list);
	}
}
