using System;
using System.Collections.Generic;

namespace QFSW.QC.Actions
{
	public class ReadValue<T> : Composite
	{
		private static readonly QuantumParser Parser = new QuantumParser();

		public ReadValue(Action<T> getValue, ResponseConfig config)
			: base(Generate(getValue, config))
		{
		}

		public ReadValue(Action<T> getValue)
			: this(getValue, ResponseConfig.Default)
		{
		}

		private static IEnumerator<ICommandAction> Generate(Action<T> getValue, ResponseConfig config)
		{
			string line = null;
			yield return new ReadLine(delegate(string t)
			{
				line = t;
			}, config);
			T obj = Parser.Parse<T>(line);
			getValue(obj);
		}
	}
}
