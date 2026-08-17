using System;

namespace QFSW.QC
{
	public interface IQcParser
	{
		int Priority { get; }

		bool CanParse(Type type);

		object Parse(string value, Type type, Func<string, Type, object> recursiveParser);
	}
}
