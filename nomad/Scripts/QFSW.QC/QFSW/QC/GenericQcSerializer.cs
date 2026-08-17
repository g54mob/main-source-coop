using System;
using QFSW.QC.Utilities;

namespace QFSW.QC
{
	public abstract class GenericQcSerializer : IQcSerializer
	{
		private Func<object, QuantumTheme, string> _recursiveSerializer;

		protected abstract Type GenericType { get; }

		public virtual int Priority => -500;

		protected GenericQcSerializer()
		{
			if (!GenericType.IsGenericType)
			{
				throw new ArgumentException("Generic Serializers must use a generic type as their base");
			}
			if (GenericType.IsConstructedGenericType)
			{
				throw new ArgumentException("Generic Serializers must use an incomplete generic type as their base");
			}
		}

		public bool CanSerialize(Type type)
		{
			return type.IsGenericTypeOf(GenericType);
		}

		string IQcSerializer.SerializeFormatted(object value, QuantumTheme theme, Func<object, QuantumTheme, string> recursiveSerializer)
		{
			_recursiveSerializer = recursiveSerializer;
			return SerializeFormatted(value, theme);
		}

		protected string SerializeRecursive(object value, QuantumTheme theme)
		{
			return _recursiveSerializer(value, theme);
		}

		public abstract string SerializeFormatted(object value, QuantumTheme theme);
	}
}
