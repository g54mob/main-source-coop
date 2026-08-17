using System;
using QFSW.QC.Utilities;
using UnityEngine;

namespace QFSW.QC.Parsers
{
	public class ComponentParser : PolymorphicQcParser<Component>
	{
		public override Component Parse(string value, Type type)
		{
			Component component = ParseRecursive<GameObject>(value).GetComponent(type);
			if (!component)
			{
				throw new ParserInputException("No component on the object '" + value + "' of type " + type.GetDisplayName() + " existed.");
			}
			return component;
		}
	}
}
