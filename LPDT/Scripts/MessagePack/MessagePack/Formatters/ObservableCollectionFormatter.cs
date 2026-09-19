using System.Collections.ObjectModel;
using MessagePack.Internal;

namespace MessagePack.Formatters
{
	[Preserve]
	public sealed class ObservableCollectionFormatter<T> : CollectionFormatterBase<T, ObservableCollection<T>>
	{
		protected override void Add(ObservableCollection<T> collection, int index, T value, MessagePackSerializerOptions options)
		{
			collection.Add(value);
		}

		protected override ObservableCollection<T> Create(int count, MessagePackSerializerOptions options)
		{
			return new ObservableCollection<T>();
		}
	}
}
