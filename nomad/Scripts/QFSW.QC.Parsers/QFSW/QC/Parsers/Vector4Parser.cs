using UnityEngine;

namespace QFSW.QC.Parsers
{
	public class Vector4Parser : BasicCachedQcParser<Vector4>
	{
		public override Vector4 Parse(string value)
		{
			string[] array = value.SplitScoped(',');
			Vector4 result = default(Vector4);
			if (array.Length < 2 || array.Length > 4)
			{
				throw new ParserInputException("Cannot parse '" + value + "' as a vector, the format must be either x,y x,y,z or x,y,z,w.");
			}
			for (int i = 0; i < array.Length; i++)
			{
				result[i] = ParseRecursive<float>(array[i]);
			}
			return result;
		}
	}
}
