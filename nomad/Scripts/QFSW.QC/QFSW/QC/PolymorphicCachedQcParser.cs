using System;
using System.Collections.Generic;

namespace QFSW.QC
{
	public abstract class PolymorphicCachedQcParser<T> : PolymorphicQcParser<T> where T : class
	{
		private readonly Dictionary<(string, Type), T> _cacheLookup = new Dictionary<(string, Type), T>();

		public override object Parse(string value, Type type, Func<string, Type, object> recursiveParser)
		{
			(string, Type) key = (value, type);
			if (_cacheLookup.ContainsKey(key))
			{
				return _cacheLookup[key];
			}
			T val = (T)base.Parse(value, type, recursiveParser);
			_cacheLookup[key] = val;
			return val;
		}
	}
}
