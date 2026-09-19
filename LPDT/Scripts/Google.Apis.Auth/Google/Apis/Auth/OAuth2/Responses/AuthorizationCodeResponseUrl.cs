using System;
using System.Collections.Generic;

namespace Google.Apis.Auth.OAuth2.Responses
{
	public class AuthorizationCodeResponseUrl
	{
		public string Code { get; set; }

		public string State { get; set; }

		public string Error { get; set; }

		public string ErrorDescription { get; set; }

		public string ErrorUri { get; set; }

		public IDictionary<string, string> AdditionalParameters { get; } = new Dictionary<string, string>();

		public AuthorizationCodeResponseUrl(IDictionary<string, string> queryString)
		{
			InitFromDictionary(queryString);
		}

		public AuthorizationCodeResponseUrl(string query)
		{
			string[] array = query.Split(new char[1] { '&' });
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string[] array3 = array2[i].Split(new char[1] { '=' });
				dictionary[array3[0]] = array3[1];
			}
			InitFromDictionary(dictionary);
		}

		private void InitFromDictionary(IDictionary<string, string> queryString)
		{
			IDictionary<string, Action<string>> dictionary = new Dictionary<string, Action<string>>();
			dictionary["code"] = delegate(string v)
			{
				Code = v;
			};
			dictionary["state"] = delegate(string v)
			{
				State = v;
			};
			dictionary["error"] = delegate(string v)
			{
				Error = v;
			};
			dictionary["error_description"] = delegate(string v)
			{
				ErrorDescription = v;
			};
			dictionary["error_uri"] = delegate(string v)
			{
				ErrorUri = v;
			};
			foreach (KeyValuePair<string, string> item in queryString)
			{
				if (dictionary.TryGetValue(item.Key, out var value))
				{
					value(item.Value);
				}
				else
				{
					AdditionalParameters[item.Key] = item.Value;
				}
			}
		}

		public AuthorizationCodeResponseUrl()
		{
		}
	}
}
