using System.Collections.Immutable;

namespace MessagePack.ImmutableCollection
{
	public class ImmutableQueueBuilder<T>
	{
		public ImmutableQueue<T> Q { get; set; } = ImmutableQueue<T>.Empty;

		public void Add(T value)
		{
			Q = Q.Enqueue(value);
		}
	}
}
