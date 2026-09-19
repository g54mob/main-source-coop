using System.Collections.Immutable;
using MessagePack.Formatters;

namespace MessagePack.ImmutableCollection
{
	public class ImmutableArrayFormatter<T> : IMessagePackFormatter<ImmutableArray<T>>, IMessagePackFormatter
	{
		public void Serialize(ref MessagePackWriter writer, ImmutableArray<T> value, MessagePackSerializerOptions options)
		{
			if (value.IsDefault)
			{
				writer.WriteNil();
				return;
			}
			if (value.IsEmpty)
			{
				writer.WriteArrayHeader(0);
				return;
			}
			IMessagePackFormatter<T> formatterWithVerify = options.Resolver.GetFormatterWithVerify<T>();
			writer.WriteArrayHeader(value.Length);
			ImmutableArray<T>.Enumerator enumerator = value.GetEnumerator();
			while (enumerator.MoveNext())
			{
				T current = enumerator.Current;
				formatterWithVerify.Serialize(ref writer, current, options);
			}
		}

		public ImmutableArray<T> Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.TryReadNil())
			{
				return default(ImmutableArray<T>);
			}
			int num = reader.ReadArrayHeader();
			if (num == 0)
			{
				return ImmutableArray<T>.Empty;
			}
			IMessagePackFormatter<T> formatterWithVerify = options.Resolver.GetFormatterWithVerify<T>();
			T[] array = new T[num];
			options.Security.DepthStep(ref reader);
			checked
			{
				try
				{
					for (int i = 0; i < num; i++)
					{
						array[i] = formatterWithVerify.Deserialize(ref reader, options);
					}
				}
				finally
				{
					reader.Depth--;
				}
				return ImmutableArray.Create(array);
			}
		}
	}
}
