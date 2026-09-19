using System;
using System.Net;
using Google.Apis.Requests;
using Google.Apis.Util;

namespace Google
{
	public class GoogleApiException : Exception
	{
		private readonly bool _hasMessage;

		public string ServiceName { get; }

		public RequestError Error { get; set; }

		private string ErrorMessage => Error?.Message ?? "No error message was specified.";

		private string ContentMessage
		{
			get
			{
				if (Error != null)
				{
					if (!Error.IsOnlyRawContent)
					{
						return $"{Error}";
					}
					return "No JSON error details were specified." + Environment.NewLine + (string.IsNullOrWhiteSpace(Error.ErrorResponseContent) ? "Raw error details are empty or white spaces only." : ("Raw error details are: " + Error.ErrorResponseContent));
				}
				return "No error details were specified.";
			}
		}

		public HttpStatusCode HttpStatusCode { get; set; }

		private string HttpStatusCodeMessage
		{
			get
			{
				if (HttpStatusCode <= (HttpStatusCode)0)
				{
					return "No HttpStatusCode was specified.";
				}
				return $"HttpStatusCode is {HttpStatusCode}.";
			}
		}

		private string ServiceNameMessage => "The service " + ServiceName + " has thrown an exception.";

		private string CombinedMessage => ServiceNameMessage + " " + HttpStatusCodeMessage + " " + ErrorMessage;

		public override string Message
		{
			get
			{
				if (!_hasMessage)
				{
					return CombinedMessage;
				}
				return base.Message;
			}
		}

		public GoogleApiException(string serviceName, string message, Exception inner)
			: base(message, inner)
		{
			ServiceName = serviceName.ThrowIfNull("serviceName");
			_hasMessage = message != null;
		}

		public GoogleApiException(string serviceName, string message)
			: this(serviceName, message, null)
		{
		}

		public GoogleApiException(string serviceName)
			: this(serviceName, null, null)
		{
		}

		public override string ToString()
		{
			return ServiceNameMessage + Environment.NewLine + HttpStatusCodeMessage + Environment.NewLine + ContentMessage + Environment.NewLine + base.ToString();
		}
	}
}
