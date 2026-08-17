using System;
using System.Globalization;
using System.Reflection;
using QFSW.QC.Utilities;

namespace QFSW.QC.Internal
{
	internal class FieldDelegateMethod : FieldMethod
	{
		public FieldDelegateMethod(FieldInfo fieldInfo)
			: base(fieldInfo)
		{
			if (!_fieldInfo.IsStrongDelegate())
			{
				throw new ArgumentException("Invalid delegate type.", "fieldInfo");
			}
			if (_fieldInfo.IsStatic)
			{
				_internalDelegate = new Func<FieldInfo, object[], object>(StaticInvoker);
			}
			else
			{
				_internalDelegate = new Func<object, FieldInfo, object[], object>(NonStaticInvoker);
			}
			_parameters = _fieldInfo.FieldType.GetMethod("Invoke").GetParameters();
			for (int i = 0; i < _parameters.Length; i++)
			{
				_parameters[i] = new CustomParameter(_parameters[i], $"arg{i}");
			}
		}

		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			object[] array = new object[_internalDelegate.Method.GetParameters().Length];
			if (array.Length < 2)
			{
				throw new Exception("FieldDelegateMethod's internal delegate must contain at least two paramaters.");
			}
			if (!base.IsStatic)
			{
				array[0] = obj;
			}
			array[array.Length - 2] = _fieldInfo;
			array[array.Length - 1] = parameters;
			return _internalDelegate.DynamicInvoke(array);
		}

		private static object StaticInvoker(FieldInfo field, params object[] args)
		{
			Delegate obj = (Delegate)field.GetValue(null);
			if ((object)obj != null)
			{
				return obj.DynamicInvoke(args);
			}
			throw new Exception("Delegate was invalid and could not be invoked.");
		}

		private object NonStaticInvoker(object obj, FieldInfo field, params object[] args)
		{
			Delegate obj2 = (Delegate)field.GetValue(obj);
			if ((object)obj2 != null)
			{
				return obj2.DynamicInvoke(args);
			}
			throw new Exception("Delegate was invalid and could not be invoked.");
		}
	}
}
