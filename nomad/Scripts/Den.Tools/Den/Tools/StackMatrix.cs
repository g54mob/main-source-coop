using System;
using System.Collections.Generic;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public class StackMatrix<T>
	{
		[SerializeField]
		internal Dictionary<Coord, (uint, T)> stack = new Dictionary<Coord, (uint, T)>();

		[SerializeField]
		private int count;

		[SerializeField]
		private int capacity = 10;

		[SerializeField]
		private uint lastId;

		public T this[Coord c]
		{
			get
			{
				stack.TryGetValue(c, out var value);
				return value.Item2;
			}
			set
			{
				lastId++;
				if (stack.ContainsKey(c))
				{
					stack[c] = (lastId, value);
				}
				else if (count < capacity)
				{
					stack.Add(c, (lastId, value));
					count++;
				}
				else
				{
					Coord coordWithLowerestId = GetCoordWithLowerestId();
					stack.Remove(coordWithLowerestId);
					stack.Add(c, (lastId, value));
				}
				if (lastId >= 4294967294u)
				{
					SetMaxId(2147483647u);
					lastId = 2147483648u;
				}
			}
		}

		public T this[int x, int z]
		{
			get
			{
				return this[new Coord(x, z)];
			}
			set
			{
				this[new Coord(x, z)] = value;
			}
		}

		public int Count => count;

		public int Capacity
		{
			get
			{
				return capacity;
			}
			set
			{
				if (value < count)
				{
					Limit(value);
				}
				capacity = value;
			}
		}

		public StackMatrix()
		{
		}

		public StackMatrix(int capacity)
		{
			this.capacity = capacity;
		}

		public bool Contains(Coord c)
		{
			return stack.ContainsKey(c);
		}

		public bool Contains(int x, int z)
		{
			return stack.ContainsKey(new Coord(x, z));
		}

		private void Limit(int num)
		{
			Coord[] array = SortByIds();
			Dictionary<Coord, (uint, T)> dictionary = new Dictionary<Coord, (uint, T)>(stack);
			stack.Clear();
			for (int num2 = array.Length - 1; num2 >= array.Length - num; num2--)
			{
				Coord key = array[num2];
				(uint, T) value = dictionary[key];
				stack.Add(key, value);
			}
			count = num;
		}

		private Coord[] SortByIds()
		{
			Coord[] array = new Coord[stack.Count];
			uint[] array2 = new uint[stack.Count];
			int num = 0;
			foreach (KeyValuePair<Coord, (uint, T)> item in stack)
			{
				array[num] = item.Key;
				array2[num] = item.Value.Item1;
				num++;
			}
			Array.Sort(array2, array);
			return array;
		}

		private Coord GetCoordWithLowerestId()
		{
			uint num = 4294967295u;
			Coord result = default(Coord);
			foreach (KeyValuePair<Coord, (uint, T)> item in stack)
			{
				if (item.Value.Item1 <= num)
				{
					num = item.Value.Item1;
					result = item.Key;
				}
			}
			return result;
		}

		internal void SetMaxId(uint maxId)
		{
			Dictionary<Coord, (uint, T)> dictionary = new Dictionary<Coord, (uint, T)>(stack);
			stack.Clear();
			foreach (KeyValuePair<Coord, (uint, T)> item2 in dictionary)
			{
				uint item = item2.Value.Item1;
				item = ((item > maxId) ? (item - maxId) : 0u);
				stack.Add(item2.Key, (item, item2.Value.Item2));
			}
		}
	}
}
