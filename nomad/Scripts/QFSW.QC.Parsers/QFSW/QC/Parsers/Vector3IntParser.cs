using UnityEngine;

namespace QFSW.QC.Parsers
{
	public class Vector3IntParser : BasicCachedQcParser<Vector3Int>
	{
		public override Vector3Int Parse(string value)
		{
			string[] array = value.Split(',');
			Vector3Int result = default(Vector3Int);
			if (array.Length < 2 || array.Length > 3)
			{
				throw new ParserInputException("Cannot parse '" + value + "' as an int vector, the format must be either x,y or x,y,z");
			}
			int i = 0;
			try
			{
				for (; i < array.Length; i++)
				{
					result[i] = int.Parse(array[i]);
				}
				return result;
			}
			catch
			{
				throw new ParserInputException("Cannot parse '" + array[i] + "' as it must be integral.");
			}
		}
	}
}
