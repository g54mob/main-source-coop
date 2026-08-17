using System;
using System.Collections.Generic;
using System.Reflection;

namespace QFSW.QC.Serializers
{
	public class KeyValuePairSerializer : GenericQcSerializer
	{
		private readonly Dictionary<Type, PropertyInfo> _keyPropertyLookup = new Dictionary<Type, PropertyInfo>();

		private readonly Dictionary<Type, PropertyInfo> _valuePropertyLookup = new Dictionary<Type, PropertyInfo>();

		protected override Type GenericType { get; } = typeof(KeyValuePair<, >);

		public override string SerializeFormatted(object value, QuantumTheme theme)
		{
			Type type = value.GetType();
			PropertyInfo propertyInfo;
			if (_keyPropertyLookup.ContainsKey(type))
			{
				propertyInfo = _keyPropertyLookup[type];
			}
			else
			{
				propertyInfo = type.GetProperty("Key");
				_keyPropertyLookup[type] = propertyInfo;
			}
			PropertyInfo propertyInfo2;
			if (_valuePropertyLookup.ContainsKey(type))
			{
				propertyInfo2 = _valuePropertyLookup[type];
			}
			else
			{
				propertyInfo2 = type.GetProperty("Value");
				_valuePropertyLookup[type] = propertyInfo2;
			}
			string text = SerializeRecursive(propertyInfo.GetValue(value, null), theme);
			string text2 = SerializeRecursive(propertyInfo2.GetValue(value, null), theme);
			return text + ": " + text2;
		}
	}
}
