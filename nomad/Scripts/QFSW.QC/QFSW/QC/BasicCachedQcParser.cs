using System;
using System.Collections.Generic;

namespace QFSW.QC
{
	public abstract class BasicCachedQcParser<T> : BasicQcParser<T>
	{
		private readonly Dictionary<string, T> _cacheLookup = new Dictionary<string, T>();

		public override object Parse(string value, Type type, Func<string, Type, object> recursiveParser)
		{
			if (_cacheLookup.ContainsKey(value))
			{
				return _cacheLookup[value];
			}
			T val = (T)base.Parse(value, type, recursiveParser);
			_cacheLookup[value] = val;
			return val;
		}
	}
}
