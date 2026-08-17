using System;
using System.Reflection;

namespace QFSW.QC.Grammar
{
	internal class BinaryOperatorData : IBinaryOperator
	{
		private readonly MethodInfo _method;

		public Type LArg { get; }

		public Type RArg { get; }

		public Type Ret { get; }

		public BinaryOperatorData(MethodInfo OperatorMethod)
		{
			_method = OperatorMethod;
			Ret = OperatorMethod.ReturnType;
			ParameterInfo[] parameters = _method.GetParameters();
			if (parameters.Length != 2)
			{
				throw new ArgumentException($"Cannot create a binary operator from a method with {parameters.Length} parameters");
			}
			LArg = parameters[0].ParameterType;
			RArg = parameters[1].ParameterType;
		}

		public object Invoke(object left, object right)
		{
			return _method.Invoke(null, new object[2] { left, right });
		}
	}
}
