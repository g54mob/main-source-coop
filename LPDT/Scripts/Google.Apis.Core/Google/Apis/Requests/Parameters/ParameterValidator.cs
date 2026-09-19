using System;
using System.Text.RegularExpressions;
using Google.Apis.Discovery;
using Google.Apis.Testing;

namespace Google.Apis.Requests.Parameters
{
	public static class ParameterValidator
	{
		[VisibleForTestOnly]
		[Obsolete("Use ValidateParameter instead")]
		public static bool ValidateRegex(IParameter param, string paramValue)
		{
			string error;
			return ValidateRegex(param, paramValue, out error);
		}

		[VisibleForTestOnly]
		internal static bool ValidateRegex(IParameter param, string paramValue, out string error)
		{
			if (string.IsNullOrEmpty(param.Pattern) || new Regex(param.Pattern).IsMatch(paramValue))
			{
				error = null;
				return true;
			}
			error = "The value did not match the regular expression " + param.Pattern;
			return false;
		}

		[Obsolete("Use the overload with error output instead")]
		public static bool ValidateParameter(IParameter parameter, string value)
		{
			string error;
			return ValidateParameter(parameter, value, out error);
		}

		public static bool ValidateParameter(IParameter parameter, string value, out string error)
		{
			if (string.IsNullOrEmpty(value))
			{
				if (parameter.IsRequired)
				{
					error = "The parameter value must be a non-empty string";
					return false;
				}
				error = null;
				return true;
			}
			return ValidateRegex(parameter, value, out error);
		}
	}
}
