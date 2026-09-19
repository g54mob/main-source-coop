using System.Collections.Generic;
using System.Text;
using Google.Apis.Util;

namespace Google.Apis.Requests
{
	public class RequestError
	{
		public enum ErrorCodes
		{
			ETagConditionFailed = 412
		}

		public IList<SingleError> Errors { get; set; }

		public int Code { get; set; }

		public string Message { get; set; }

		public string ErrorResponseContent { get; set; }

		internal bool IsOnlyRawContent
		{
			get
			{
				if (Message == null && Code == 0)
				{
					return Errors.IsNullOrEmpty();
				}
				return false;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(GetType().FullName).Append(Message).AppendFormat(" [{0}]", Code)
				.AppendLine();
			if (Errors.IsNullOrEmpty())
			{
				stringBuilder.AppendLine("No individual errors");
			}
			else
			{
				stringBuilder.AppendLine("Errors [");
				foreach (SingleError error in Errors)
				{
					stringBuilder.Append('\t').AppendLine(error.ToString());
				}
				stringBuilder.AppendLine("]");
			}
			return stringBuilder.ToString();
		}
	}
}
