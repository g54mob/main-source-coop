using System;

namespace PlayEveryWare.EpicOnlineServices
{
	[AttributeUsage(AttributeTargets.Field)]
	public class GUIDFieldValidatorAttribute : FieldValidatorAttribute
	{
		public const string NotAGuidMessage = "The field value could not be parsed into a Guid.";

		public override bool FieldValueIsValid(object toValidate, out string configurationProblemMessage)
		{
			if (!(toValidate is string text))
			{
				configurationProblemMessage = "The field value is not of type string.";
				return false;
			}
			if (string.IsNullOrEmpty(text))
			{
				configurationProblemMessage = "The field value is an empty string.";
				return false;
			}
			if (!Guid.TryParse(text, out var _))
			{
				configurationProblemMessage = "The field value could not be parsed into a Guid.";
				return false;
			}
			configurationProblemMessage = string.Empty;
			return true;
		}
	}
}
