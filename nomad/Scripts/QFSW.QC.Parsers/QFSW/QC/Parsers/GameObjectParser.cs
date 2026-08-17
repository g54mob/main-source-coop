using QFSW.QC.Utilities;
using UnityEngine;

namespace QFSW.QC.Parsers
{
	public class GameObjectParser : BasicQcParser<GameObject>
	{
		public override GameObject Parse(string value)
		{
			GameObject gameObject = GameObjectExtensions.Find(ParseRecursive<string>(value), includeInactive: true);
			if (!gameObject)
			{
				throw new ParserInputException("Could not find GameObject of name " + value + ".");
			}
			return gameObject;
		}
	}
}
