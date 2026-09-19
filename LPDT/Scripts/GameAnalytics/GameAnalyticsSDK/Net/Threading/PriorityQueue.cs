using System;
using System.Collections.Generic;
using System.Linq;

namespace GameAnalyticsSDK.Net.Threading
{
	internal class PriorityQueue<TPriority, TItem>
	{
		private readonly SortedDictionary<TPriority, Queue<TItem>> _subqueues;

		public bool HasItems => _subqueues.Any();

		public int Count => _subqueues.Sum((KeyValuePair<TPriority, Queue<TItem>> q) => q.Value.Count);

		public PriorityQueue(IComparer<TPriority> priorityComparer)
		{
			_subqueues = new SortedDictionary<TPriority, Queue<TItem>>(priorityComparer);
		}

		public PriorityQueue()
			: this((IComparer<TPriority>)Comparer<TPriority>.Default)
		{
		}

		public void Enqueue(TPriority priority, TItem item)
		{
			if (!_subqueues.ContainsKey(priority))
			{
				AddQueueOfPriority(priority);
			}
			_subqueues[priority].Enqueue(item);
		}

		private void AddQueueOfPriority(TPriority priority)
		{
			_subqueues.Add(priority, new Queue<TItem>());
		}

		public TItem Peek()
		{
			if (HasItems)
			{
				return _subqueues.First().Value.Peek();
			}
			throw new InvalidOperationException("The queue is empty");
		}

		public TItem Dequeue()
		{
			if (_subqueues.Any())
			{
				return DequeueFromHighPriorityQueue();
			}
			throw new InvalidOperationException("The queue is empty");
		}

		private TItem DequeueFromHighPriorityQueue()
		{
			KeyValuePair<TPriority, Queue<TItem>> keyValuePair = _subqueues.First();
			TItem result = keyValuePair.Value.Dequeue();
			if (!keyValuePair.Value.Any())
			{
				_subqueues.Remove(keyValuePair.Key);
			}
			return result;
		}
	}
}
