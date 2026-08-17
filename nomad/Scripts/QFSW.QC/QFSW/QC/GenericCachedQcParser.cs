using System;
using System.Collections.Generic;

namespace QFSW.QC
{
	public abstract class GenericCachedQcParser : GenericQcParser
	{
		private readonly Dictionary<(string, Type), object> _cacheLookup = new Dictionary<(string, Type), object>();

		public override object Parse(string value, Type type, Func<string, Type, object> recursiveParser)
		{
			(string, Type) key = (value, type);
			if (_cacheLookup.ContainsKey(key))
			{
				return _cacheLookup[key];
			}
			object obj = base.Parse(value, type, recursiveParser);
			_cacheLookup[key] = obj;
			return obj;
		}
	}
}
