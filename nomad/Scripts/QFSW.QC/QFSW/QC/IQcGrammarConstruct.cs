using System;

namespace QFSW.QC
{
	public interface IQcGrammarConstruct
	{
		int Precedence { get; }

		bool Match(string value, Type type);

		object Parse(string value, Type type, Func<string, Type, object> recursiveParser);
	}
}
