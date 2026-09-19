using System;
using System.Collections.Generic;

namespace Fusion
{
	internal struct OffsetSetBuilder
	{
		private List<int> _values;

		public void Add(int value)
		{
			if (_values != null)
			{
				if (_values.Count > 0)
				{
					List<int> values = _values;
					if (values[values.Count - 1] >= value)
					{
						throw new ArgumentException("value");
					}
				}
			}
			else
			{
				_values = new List<int>();
			}
			_values.Add(value);
		}

		public OffsetSet MakeOffsetSet()
		{
			if (_values == null)
			{
				return null;
			}
			return new OffsetSet(_values.ToArray());
		}
	}
}
