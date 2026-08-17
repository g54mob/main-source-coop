namespace Den.Tools
{
	public struct StructArrayExtended<T>
	{
		public T i0;

		public T i1;

		public T i2;

		public T i3;

		public int count;

		private T[] others;

		public T this[int n]
		{
			get
			{
				return n switch
				{
					0 => i0, 
					1 => i1, 
					2 => i2, 
					3 => i3, 
					_ => others[n - 4], 
				};
			}
			set
			{
				switch (n)
				{
				case 0:
					i0 = value;
					break;
				case 1:
					i1 = value;
					break;
				case 2:
					i2 = value;
					break;
				case 3:
					i3 = value;
					break;
				default:
					others[n - 4] = value;
					break;
				}
			}
		}

		public int Capacity
		{
			get
			{
				return 4 + ((others != null) ? others.Length : 0);
			}
			set
			{
				if (value <= 4)
				{
					if (others != null)
					{
						others = null;
					}
				}
				else if (others == null)
				{
					others = new T[value - 4];
				}
				else
				{
					ArrayTools.Resize(ref others, value - 4);
				}
			}
		}

		public StructArrayExtended(int capacity)
		{
			i0 = default(T);
			i1 = default(T);
			i2 = default(T);
			i3 = default(T);
			others = null;
			count = 0;
			Capacity = capacity;
		}

		public void Add(T item)
		{
			switch (count)
			{
			case 0:
				i0 = item;
				break;
			case 1:
				i1 = item;
				break;
			case 2:
				i2 = item;
				break;
			case 3:
				i3 = item;
				break;
			default:
				if (others == null)
				{
					others = new T[4];
				}
				if (others.Length <= count - 4)
				{
					ArrayTools.Resize(ref others, others.Length * 2);
				}
				others[count - 4] = item;
				break;
			}
			count++;
		}

		public void AddRange(StructArrayExtended<T> other)
		{
			for (int i = 0; i < other.count; i++)
			{
				Add(other[i]);
			}
		}

		public int FindIndex(T item)
		{
			if (i0.Equals(item))
			{
				return 0;
			}
			if (i1.Equals(item))
			{
				return 1;
			}
			if (i2.Equals(item))
			{
				return 2;
			}
			if (i3.Equals(item))
			{
				return 3;
			}
			if (others != null)
			{
				for (int i = 0; i < others.Length; i++)
				{
					if (others[i].Equals(item))
					{
						return i + 4;
					}
				}
			}
			return -1;
		}
	}
}
