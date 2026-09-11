using System.Linq;
using EasyTextEffects.Editor.MyBoxCopy.Extensions;

namespace EasyTextEffects.Editor.MyBoxCopy.Tools.Internal
{
	public class ConditionalData
	{
		private readonly string _fieldToCheck;

		private readonly bool _inverse;

		private readonly string[] _compareValues;

		private readonly string[] _fieldsToCheckMultiple;

		private readonly bool[] _inverseMultiple;

		private readonly string[] _compareValuesMultiple;

		private readonly string _predicateMethod;

		public bool IsSet
		{
			get
			{
				if (!_fieldToCheck.NotNullOrEmpty() && !_fieldsToCheckMultiple.NotNullOrEmpty())
				{
					return _predicateMethod.NotNullOrEmpty();
				}
				return true;
			}
		}

		public ConditionalData(string fieldToCheck, bool inverse = false, params object[] compareValues)
		{
			bool inverse2 = inverse;
			string[] compareValues2 = compareValues.Select((object c) => c.ToString().ToUpper()).ToArray();
			_fieldToCheck = fieldToCheck;
			_inverse = inverse2;
			_compareValues = compareValues2;
		}

		public ConditionalData(string[] fieldToCheck, bool[] inverse = null, params object[] compare)
		{
			string[] compareValuesMultiple = compare.Select((object c) => c.ToString().ToUpper()).ToArray();
			_fieldsToCheckMultiple = fieldToCheck;
			_inverseMultiple = inverse;
			_compareValuesMultiple = compareValuesMultiple;
		}

		public ConditionalData(params string[] fieldToCheck)
		{
			_fieldsToCheckMultiple = fieldToCheck;
		}

		public ConditionalData(bool useMethod, string methodName, bool inverse = false)
		{
			bool inverse2 = inverse;
			_predicateMethod = methodName;
			_inverse = inverse2;
		}
	}
}
