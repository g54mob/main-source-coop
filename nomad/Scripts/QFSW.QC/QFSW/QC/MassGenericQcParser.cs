using System;
using System.Collections.Generic;

namespace QFSW.QC
{
	public abstract class MassGenericQcParser : IQcParser
	{
		private Func<string, Type, object> _recursiveParser;

		protected abstract HashSet<Type> GenericTypes { get; }

		public virtual int Priority => -2000;

		protected MassGenericQcParser()
		{
			foreach (Type genericType in GenericTypes)
			{
				if (!genericType.IsGenericType)
				{
					throw new ArgumentException("Generic Parsers must use a generic type as their base");
				}
				if (genericType.IsConstructedGenericType)
				{
					throw new ArgumentException("Generic Parsers must use an incomplete generic type as their base");
				}
			}
		}

		public bool CanParse(Type type)
		{
			if (type.IsGenericType)
			{
				return GenericTypes.Contains(type.GetGenericTypeDefinition());
			}
			return false;
		}

		public virtual object Parse(string value, Type type, Func<string, Type, object> recursiveParser)
		{
			_recursiveParser = recursiveParser;
			return Parse(value, type);
		}

		protected object ParseRecursive(string value, Type type)
		{
			return _recursiveParser(value, type);
		}

		protected TElement ParseRecursive<TElement>(string value)
		{
			return (TElement)_recursiveParser(value, typeof(TElement));
		}

		public abstract object Parse(string value, Type type);
	}
}
