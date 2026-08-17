using System.Collections.Generic;
using System.Reflection;

namespace PlayEveryWare.EpicOnlineServices
{
	public class FieldValidator
	{
		public static List<FieldValidatorFailure> GetFailingValidatorAttributeOnObject(FieldInfo fieldInfo, object singularValue)
		{
			List<FieldValidatorFailure> list = new List<FieldValidatorFailure>();
			if (fieldInfo.GetCustomAttribute(typeof(ExpandFieldAttribute)) != null)
			{
				list.AddRange(GetFailingValidatorAttributeOnClass(singularValue));
			}
			else
			{
				foreach (FieldValidatorAttribute customAttribute in fieldInfo.GetCustomAttributes(typeof(FieldValidatorAttribute)))
				{
					if (!customAttribute.FieldValueIsValid(singularValue, out var configurationProblemMessage))
					{
						list.Add(new FieldValidatorFailure(fieldInfo, customAttribute, configurationProblemMessage));
					}
				}
			}
			return list;
		}

		public static List<FieldValidatorFailure> GetFailingValidatorAttributeOnField(FieldInfo fieldInfo, object fieldValue)
		{
			List<FieldValidatorFailure> list = new List<FieldValidatorFailure>();
			if (fieldInfo.FieldType.IsGenericType && fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>))
			{
				foreach (object item in new List<object>((IEnumerable<object>)fieldValue))
				{
					list.AddRange(GetFailingValidatorAttributeOnObject(fieldInfo, item));
				}
			}
			else
			{
				list.AddRange(GetFailingValidatorAttributeOnObject(fieldInfo, fieldValue));
			}
			return list;
		}

		public static List<FieldValidatorFailure> GetFailingValidatorAttributeOnClass(object target)
		{
			List<FieldValidatorFailure> list = new List<FieldValidatorFailure>();
			FieldInfo[] fields = target.GetType().GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				list.AddRange(GetFailingValidatorAttributeOnField(fieldInfo, fieldInfo.GetValue(target)));
			}
			return list;
		}

		public static bool TryGetFailingValidatorAttributes(object target, out List<FieldValidatorFailure> failingValidatorAttributes)
		{
			failingValidatorAttributes = new List<FieldValidatorFailure>();
			failingValidatorAttributes.AddRange(GetFailingValidatorAttributeOnClass(target));
			return failingValidatorAttributes.Count > 0;
		}
	}
}
