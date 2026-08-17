using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using QFSW.QC.Utilities;

namespace QFSW.QC.Grammar
{
	public abstract class BinaryOperatorGrammar : IQcGrammarConstruct
	{
		private Regex _operatorRegex;

		private readonly HashSet<Type> _missingOperatorTable = new HashSet<Type>();

		private readonly Dictionary<Type, IBinaryOperator> _foundOperatorTable = new Dictionary<Type, IBinaryOperator>();

		public abstract int Precedence { get; }

		protected abstract char OperatorToken { get; }

		protected abstract string OperatorMethodName { get; }

		protected abstract Func<Expression, Expression, BinaryExpression> PrimitiveExpressionGenerator { get; }

		public bool Match(string value, Type type)
		{
			if (_missingOperatorTable.Contains(type))
			{
				return false;
			}
			if (!IsSyntaxMatch(value))
			{
				return false;
			}
			if (_foundOperatorTable.ContainsKey(type))
			{
				return true;
			}
			IBinaryOperator operatorData = GetOperatorData(type);
			if (operatorData != null)
			{
				_foundOperatorTable.Add(type, operatorData);
				return true;
			}
			_missingOperatorTable.Add(type);
			return false;
		}

		private bool IsSyntaxMatch(string value)
		{
			if (_operatorRegex == null)
			{
				_operatorRegex = new Regex($"^.+\\{OperatorToken}.+$");
			}
			if (!_operatorRegex.IsMatch(value))
			{
				return false;
			}
			int operatorPosition = GetOperatorPosition(value);
			if (operatorPosition > 0)
			{
				return operatorPosition < value.Length;
			}
			return false;
		}

		private IBinaryOperator GetOperatorData(Type type)
		{
			if (type.IsPrimitive)
			{
				return GeneratePrimitiveOperator(type);
			}
			BinaryOperatorData[] source = (from x in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
				where x.Name == OperatorMethodName
				where x.ReturnType == type
				where x.GetParameters().Length == 2
				select new BinaryOperatorData(x)).ToArray();
			return source.FirstOrDefault((BinaryOperatorData x) => x.LArg == type && x.RArg == type) ?? source.FirstOrDefault((BinaryOperatorData x) => x.LArg == type) ?? source.FirstOrDefault((BinaryOperatorData x) => x.RArg == type) ?? source.FirstOrDefault();
		}

		private IBinaryOperator GeneratePrimitiveOperator(Type type)
		{
			ParameterExpression parameterExpression = Expression.Parameter(type, "left");
			ParameterExpression parameterExpression2 = Expression.Parameter(type, "right");
			BinaryExpression body;
			try
			{
				body = PrimitiveExpressionGenerator(parameterExpression, parameterExpression2);
			}
			catch (InvalidOperationException)
			{
				return null;
			}
			return new DynamicBinaryOperator(Expression.Lambda(body, true, parameterExpression, parameterExpression2).Compile(), type, type, type);
		}

		protected virtual int GetOperatorPosition(string value)
		{
			return TextProcessing.GetScopedSplitPoints(value, OperatorToken, TextProcessing.DefaultLeftScopers, TextProcessing.DefaultRightScopers).LastOr(-1);
		}

		public object Parse(string value, Type type, Func<string, Type, object> recursiveParser)
		{
			IBinaryOperator binaryOperator = _foundOperatorTable[type];
			int operatorPosition = GetOperatorPosition(value);
			string arg = value.Substring(0, operatorPosition);
			string arg2 = value.Substring(operatorPosition + 1);
			object left = recursiveParser(arg, binaryOperator.LArg);
			object right = recursiveParser(arg2, binaryOperator.RArg);
			try
			{
				return binaryOperator.Invoke(left, right);
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException ?? ex;
			}
		}
	}
}
