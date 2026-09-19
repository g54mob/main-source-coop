using System;
using System.Collections.Generic;

namespace Obi
{
	public class PriorityQueue<T> where T : IComparable<T>
	{
		public List<T> data;

		public PriorityQueue()
		{
			data = new List<T>();
		}

		public void Enqueue(T item)
		{
			data.Add(item);
			int num = data.Count - 1;
			while (num > 0)
			{
				int num2 = (num - 1) / 2;
				if (data[num].CompareTo(data[num2]) < 0)
				{
					T value = data[num];
					data[num] = data[num2];
					data[num2] = value;
					num = num2;
					continue;
				}
				break;
			}
		}

		public T Dequeue()
		{
			int num = data.Count - 1;
			T result = data[0];
			data[0] = data[num];
			data.RemoveAt(num);
			num--;
			int num2 = 0;
			while (true)
			{
				int num3 = num2 * 2 + 1;
				if (num3 > num)
				{
					break;
				}
				int num4 = num3 + 1;
				if (num4 <= num && data[num4].CompareTo(data[num3]) < 0)
				{
					num3 = num4;
				}
				if (data[num2].CompareTo(data[num3]) <= 0)
				{
					break;
				}
				T value = data[num2];
				data[num2] = data[num3];
				data[num3] = value;
				num2 = num3;
			}
			return result;
		}

		public T Peek()
		{
			return data[0];
		}

		public IEnumerable<T> GetEnumerator()
		{
			int i = 0;
			while (i < data.Count)
			{
				yield return data[i];
				int num = i + 1;
				i = num;
			}
		}

		public void Clear()
		{
			data.Clear();
		}

		public int Count()
		{
			return data.Count;
		}

		public override string ToString()
		{
			string text = "";
			for (int i = 0; i < data.Count; i++)
			{
				text = text + data[i].ToString() + " ";
			}
			return text + "count = " + data.Count;
		}

		public bool IsConsistent()
		{
			if (data.Count == 0)
			{
				return true;
			}
			int num = data.Count - 1;
			for (int i = 0; i < data.Count; i++)
			{
				int num2 = 2 * i + 1;
				int num3 = 2 * i + 2;
				if (num2 <= num && data[i].CompareTo(data[num2]) > 0)
				{
					return false;
				}
				if (num3 <= num && data[i].CompareTo(data[num3]) > 0)
				{
					return false;
				}
			}
			return true;
		}
	}
}
