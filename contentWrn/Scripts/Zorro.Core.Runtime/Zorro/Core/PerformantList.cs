namespace Zorro.Core
{
	public class PerformantList<T>
	{
		private T[] array;

		private int count;

		private int capacity;

		public int Count => count;

		public T this[int index] => array[index];

		public PerformantList(int capacity)
		{
			array = new T[capacity];
			this.capacity = capacity;
		}

		public void Add(T entry)
		{
			if (count == capacity)
			{
				capacity += capacity;
				T[] array = new T[capacity];
				this.array.CopyTo(array, 0);
				this.array = array;
			}
			this.array[count] = entry;
			count++;
		}

		public void RemoveAtSwapBack(int index)
		{
			int num = count - 1;
			if (index != num)
			{
				array[index] = array[num];
			}
			array[num] = default(T);
			count--;
		}
	}
}
