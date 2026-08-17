using System;

namespace PlayEveryWare.EpicOnlineServices
{
	[AttributeUsage(AttributeTargets.Field)]
	public abstract class FieldValidatorAttribute : Attribute
	{
		public abstract bool FieldValueIsValid(object toValidate, out string configurationProblemMessage);
	}
}
