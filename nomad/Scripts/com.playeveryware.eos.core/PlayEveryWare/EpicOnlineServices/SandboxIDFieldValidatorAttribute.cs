using System;
using System.Text.RegularExpressions;

namespace PlayEveryWare.EpicOnlineServices
{
	[AttributeUsage(AttributeTargets.Field)]
	public class SandboxIDFieldValidatorAttribute : FieldValidatorAttribute
	{
		private const string PreProductionEnvironmentRegex = "^p\\-[a-zA-Z\\d]{30}$";

		public const string FieldDidNotMatchMessage = "The field value is not a GUID, and did not match the regex used for Pre Production Environments: '^p\\-[a-zA-Z\\d]{30}$'.";

		public override bool FieldValueIsValid(object toValidate, out string configurationProblemMessage)
		{
			if (!(toValidate is string input))
			{
				configurationProblemMessage = "The field value is not of type string.";
				return false;
			}
			if (Regex.IsMatch(input, "^p\\-[a-zA-Z\\d]{30}$"))
			{
				configurationProblemMessage = string.Empty;
				return true;
			}
			if (Guid.TryParse(input, out var _))
			{
				configurationProblemMessage = string.Empty;
				return true;
			}
			configurationProblemMessage = "The field value is not a GUID, and did not match the regex used for Pre Production Environments: '^p\\-[a-zA-Z\\d]{30}$'.";
			return false;
		}
	}
}
