using System;
using System.Collections;

namespace QFSW.QC.Parsers
{
	public class ArrayParser : IQcParser
	{
		public int Priority => -100;

		public bool CanParse(Type type)
		{
			return type.IsArray;
		}

		public object Parse(string value, Type type, Func<string, Type, object> recursiveParser)
		{
			Type elementType = type.GetElementType();
			string[] array = value.ReduceScope('[', ']').SplitScoped(',');
			IList list = Array.CreateInstance(elementType, array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				list[i] = recursiveParser(array[i], elementType);
			}
			return list;
		}
	}
}
