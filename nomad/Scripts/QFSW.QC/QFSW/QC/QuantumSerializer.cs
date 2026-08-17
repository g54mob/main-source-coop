using System;
using System.Collections.Generic;
using System.Linq;
using QFSW.QC.Utilities;

namespace QFSW.QC
{
	public class QuantumSerializer
	{
		private readonly IQcSerializer[] _serializers;

		private readonly Dictionary<Type, IQcSerializer> _serializerLookup = new Dictionary<Type, IQcSerializer>();

		private readonly HashSet<Type> _unserializableLookup = new HashSet<Type>();

		private readonly Func<object, QuantumTheme, string> _recursiveSerializer;

		public QuantumSerializer(IEnumerable<IQcSerializer> serializers)
		{
			_recursiveSerializer = SerializeFormatted;
			_serializers = serializers.OrderByDescending((IQcSerializer x) => x.Priority).ToArray();
		}

		public QuantumSerializer()
			: this(new InjectionLoader<IQcSerializer>().GetInjectedInstances())
		{
		}

		public string SerializeFormatted(object value, QuantumTheme theme = null)
		{
			if (value == null)
			{
				return string.Empty;
			}
			Type type = value.GetType();
			string text = string.Empty;
			if (_serializerLookup.ContainsKey(type))
			{
				text = SerializeInternal(_serializerLookup[type]);
			}
			else if (_unserializableLookup.Contains(type))
			{
				text = value.ToString();
			}
			else
			{
				bool flag = false;
				IQcSerializer[] serializers = _serializers;
				foreach (IQcSerializer qcSerializer in serializers)
				{
					if (qcSerializer.CanSerialize(type))
					{
						text = SerializeInternal(qcSerializer);
						_serializerLookup[type] = qcSerializer;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					text = value.ToString();
					_unserializableLookup.Add(type);
				}
			}
			if ((bool)theme && !string.IsNullOrWhiteSpace(text))
			{
				text = theme.ColorizeReturn(text, type);
			}
			return text;
			string SerializeInternal(IQcSerializer serializer)
			{
				try
				{
					return serializer.SerializeFormatted(value, theme, _recursiveSerializer);
				}
				catch (Exception ex)
				{
					throw new Exception($"Serialization of {type.GetDisplayName()} via {serializer} failed:\n{ex.Message}", ex);
				}
			}
		}
	}
}
