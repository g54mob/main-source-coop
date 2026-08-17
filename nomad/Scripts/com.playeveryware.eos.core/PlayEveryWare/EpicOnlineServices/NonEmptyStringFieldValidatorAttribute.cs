using System;

namespace PlayEveryWare.EpicOnlineServices
{
	[AttributeUsage(AttributeTargets.Field)]
	public class NonEmptyStringFieldValidatorAttribute : FieldValidatorAttribute
	{
		public const string FieldIsEmptyMessage = "The field value is an empty string.";

		public override bool FieldValueIsValid(object toValidate, out string configurationProblemMessage)
		{
			if (!(toValidate is string value))
			{
				configurationProblemMessage = "The field value is not of type string.";
				return false;
			}
			if (string.IsNullOrEmpty(value))
			{
				configurationProblemMessage = "The field value is an empty string.";
				return false;
			}
			configurationProblemMessage = string.Empty;
			return true;
		}
	}
}
