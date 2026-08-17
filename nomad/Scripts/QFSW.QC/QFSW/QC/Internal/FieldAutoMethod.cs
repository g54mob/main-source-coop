using System;
using System.Reflection;

namespace QFSW.QC.Internal
{
	internal class FieldAutoMethod : FieldMethod
	{
		public enum AccessType
		{
			Read = 0,
			Write = 1
		}

		private readonly AccessType _accessType;

		public FieldAutoMethod(FieldInfo fieldInfo, AccessType accessType)
			: base(fieldInfo)
		{
			_accessType = accessType;
			if (_accessType == AccessType.Read)
			{
				Func<FieldInfo, object> internalDelegate;
				if (!_fieldInfo.IsStatic)
				{
					Func<FieldInfo, object> func = new Func<object, object>(_fieldInfo.GetValue);
					internalDelegate = func;
				}
				else
				{
					internalDelegate = _StaticReader;
				}
				_internalDelegate = internalDelegate;
				_parameters = Array.Empty<ParameterInfo>();
				return;
			}
			Action<FieldInfo, object> internalDelegate2;
			if (!_fieldInfo.IsStatic)
			{
				Action<FieldInfo, object> action = new Action<object, object>(_fieldInfo.SetValue);
				internalDelegate2 = action;
			}
			else
			{
				internalDelegate2 = _StaticWriter;
			}
			_internalDelegate = internalDelegate2;
			_parameters = new ParameterInfo[1]
			{
				new CustomParameter(_internalDelegate.Method.GetParameters()[1], _fieldInfo.FieldType, "value")
			};
		}

		private static object _StaticReader(FieldInfo field)
		{
			return field.GetValue(null);
		}

		private static void _StaticWriter(FieldInfo field, object value)
		{
			field.SetValue(null, value);
		}
	}
}
