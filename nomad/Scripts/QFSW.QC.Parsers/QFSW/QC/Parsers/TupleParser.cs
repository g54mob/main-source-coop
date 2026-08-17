using System;
using System.Collections.Generic;

namespace QFSW.QC.Parsers
{
	public class TupleParser : MassGenericQcParser
	{
		private const int MaxFlatTupleSize = 8;

		protected override HashSet<Type> GenericTypes { get; } = new HashSet<Type>
		{
			typeof(ValueTuple<>),
			typeof(ValueTuple<, >),
			typeof(ValueTuple<, , >),
			typeof(ValueTuple<, , , >),
			typeof(ValueTuple<, , , , >),
			typeof(ValueTuple<, , , , , >),
			typeof(ValueTuple<, , , , , , >),
			typeof(ValueTuple<, , , , , , , >),
			typeof(Tuple<>),
			typeof(Tuple<, >),
			typeof(Tuple<, , >),
			typeof(Tuple<, , , >),
			typeof(Tuple<, , , , >),
			typeof(Tuple<, , , , , >),
			typeof(Tuple<, , , , , , >),
			typeof(Tuple<, , , , , , , >)
		};

		public override object Parse(string value, Type type)
		{
			TextProcessing.ScopedSplitOptions options = TextProcessing.ScopedSplitOptions.Default;
			options.MaxCount = 8;
			string[] array = value.ReduceScope('(', ')').SplitScoped(',', options);
			Type[] genericArguments = type.GetGenericArguments();
			if (genericArguments.Length != array.Length)
			{
				throw new ParserInputException($"Desired tuple type {type} has {genericArguments.Length} elements but input contained {array.Length}.");
			}
			object[] array2 = new object[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = ParseRecursive(array[i], genericArguments[i]);
			}
			return Activator.CreateInstance(type, array2);
		}
	}
}
