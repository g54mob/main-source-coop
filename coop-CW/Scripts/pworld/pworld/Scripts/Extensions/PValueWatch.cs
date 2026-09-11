using System;

namespace pworld.Scripts.Extensions
{
	public class PValueWatch
	{
		public Action<IComparable> alternativeCompare;

		public Action<int> onValueChanged;

		public IComparable valueWatched;

		public IComparable ValueWatched
		{
			get
			{
				return valueWatched;
			}
			set
			{
				if (alternativeCompare != null)
				{
					alternativeCompare?.Invoke(value);
					return;
				}
				int num = valueWatched.CompareTo(value);
				if (num != 0)
				{
					valueWatched = value;
					onValueChanged?.Invoke(num);
				}
			}
		}

		public PValueWatch(IComparable val, Action<int> cb, Action<IComparable> alternativeCompare = null)
		{
			valueWatched = val;
			onValueChanged = cb;
			this.alternativeCompare = alternativeCompare;
		}
	}
}
