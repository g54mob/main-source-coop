using System;
using System.Text.RegularExpressions;
using QFSW.QC.Utilities;

namespace QFSW.QC.Grammar
{
	public class ExpressionBodyGrammar : IQcGrammarConstruct
	{
		private readonly Regex _expressionBodyRegex = new Regex("^{.+}\\??$");

		public int Precedence => 0;

		public bool Match(string value, Type type)
		{
			return _expressionBodyRegex.IsMatch(value);
		}

		public object Parse(string value, Type type, Func<string, Type, object> recursiveParser)
		{
			bool flag = false;
			if (value.EndsWith("?"))
			{
				flag = true;
				value = value.Substring(0, value.Length - 1);
			}
			value = value.ReduceScope('{', '}');
			object obj = QuantumConsoleProcessor.InvokeCommand(value);
			if (obj == null)
			{
				if (flag)
				{
					if (type.IsClass)
					{
						return obj;
					}
					throw new ParserInputException("Expression body {" + value + "} evaluated to null which is incompatible with the expected type '" + type.GetDisplayName() + "'.");
				}
				throw new ParserInputException("Expression body {" + value + "} evaluated to null. If this is intended, please use nullable expression bodies, {expr}?");
			}
			if (obj.GetType().IsCastableTo(type, implicitly: true))
			{
				return type.Cast(obj);
			}
			throw new ParserInputException("Expression body {" + value + "} evaluated to an object of type '" + obj.GetType().GetDisplayName() + "', which is incompatible with the expected type '" + type.GetDisplayName() + "'.");
		}
	}
}
