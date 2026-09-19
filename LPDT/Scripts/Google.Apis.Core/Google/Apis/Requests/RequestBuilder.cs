using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Google.Apis.Logging;
using Google.Apis.Util;

namespace Google.Apis.Requests
{
	public class RequestBuilder
	{
		private static readonly ILogger Logger;

		private static Regex PathParametersPattern;

		private static IEnumerable<string> SupportedMethods;

		private string method;

		private const string OPERATORS = "+#./;?&|!@=";

		private IDictionary<string, IList<string>> PathParameters { get; set; }

		private List<KeyValuePair<string, string>> QueryParameters { get; set; }

		public Uri BaseUri { get; set; }

		public string Path { get; set; }

		public string Method
		{
			get
			{
				return method;
			}
			set
			{
				if (!SupportedMethods.Contains(value))
				{
					throw new ArgumentOutOfRangeException("Method");
				}
				method = value;
			}
		}

		static RequestBuilder()
		{
			Logger = ApplicationContext.Logger.ForType<RequestBuilder>();
			PathParametersPattern = new Regex("{[^{}]*}*");
			SupportedMethods = new List<string> { "GET", "POST", "PUT", "DELETE", "PATCH" };
			UriPatcher.PatchUriQuirks();
		}

		public RequestBuilder()
		{
			PathParameters = new Dictionary<string, IList<string>>();
			QueryParameters = new List<KeyValuePair<string, string>>();
			Method = "GET";
		}

		public Uri BuildUri()
		{
			StringBuilder stringBuilder = BuildRestPath();
			if (QueryParameters.Count > 0)
			{
				stringBuilder.Append(stringBuilder.ToString().Contains("?") ? "&" : "?");
				stringBuilder.Append(string.Join("&", QueryParameters.Select((KeyValuePair<string, string> x) => (!string.IsNullOrEmpty(x.Value)) ? $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value)}" : Uri.EscapeDataString(x.Key)).ToArray()));
			}
			return UriJoin(BaseUri, stringBuilder.ToString());
		}

		private Uri UriJoin(Uri baseUri, string path)
		{
			if (path == "")
			{
				return baseUri;
			}
			string text = baseUri.AbsoluteUri;
			if (path.StartsWith("/"))
			{
				text = Regex.Replace(text, "^([^:]+://[^/]+)/.*$", "$1");
			}
			else if (!path.StartsWith("?") && !path.StartsWith("#"))
			{
				while (!text.EndsWith("/"))
				{
					text = text.Substring(0, text.Length - 1);
				}
			}
			return new Uri(text + path);
		}

		private StringBuilder BuildRestPath()
		{
			if (string.IsNullOrEmpty(Path))
			{
				return new StringBuilder(string.Empty);
			}
			StringBuilder stringBuilder = new StringBuilder(Path);
			foreach (object item in PathParametersPattern.Matches(stringBuilder.ToString()))
			{
				string text = item.ToString();
				string text2 = text.Substring(1, text.Length - 2);
				string text3 = string.Empty;
				if ("+#./;?&|!@=".Contains(text2[0].ToString()))
				{
					text3 = text2[0].ToString();
					text2 = text2.Substring(1);
				}
				StringBuilder stringBuilder2 = new StringBuilder();
				string[] array = text2.Split(new char[1] { ',' });
				for (int i = 0; i < array.Length; i++)
				{
					string text4 = array[i];
					bool flag = false;
					int result = 0;
					if (text4[text4.Length - 1] == '*')
					{
						flag = true;
						text4 = text4.Substring(0, text4.Length - 1);
					}
					if (text4.Contains(":"))
					{
						if (!int.TryParse(text4.Substring(text4.IndexOf(":") + 1), out result))
						{
							throw new ArgumentException($"Can't parse number after ':' in Path \"{Path}\". Parameter is \"{text4}\"", Path);
						}
						text4 = text4.Substring(0, text4.IndexOf(":"));
					}
					string separator = text3;
					string text5 = text3;
					switch (text3)
					{
					case "+":
						text5 = ((i == 0) ? "" : ",");
						separator = ",";
						break;
					case ".":
						if (!flag)
						{
							separator = ",";
						}
						break;
					case "/":
						if (!flag)
						{
							separator = ",";
						}
						break;
					case "#":
						text5 = ((i == 0) ? "#" : ",");
						separator = ",";
						break;
					case "?":
						text5 = ((i == 0) ? "?" : "&") + text4 + "=";
						separator = ",";
						if (flag)
						{
							separator = "&" + text4 + "=";
						}
						break;
					case "&":
					case ";":
						text5 = text3 + text4 + "=";
						separator = ",";
						if (flag)
						{
							separator = text3 + text4 + "=";
						}
						break;
					default:
						if (i > 0)
						{
							text5 = ",";
						}
						separator = ",";
						break;
					}
					if (PathParameters.ContainsKey(text4))
					{
						string text6 = string.Join(separator, PathParameters[text4]);
						if (result != 0 && result < text6.Length)
						{
							text6 = text6.Substring(0, result);
						}
						if (text3 != "+" && text3 != "#" && PathParameters[text4].Count == 1)
						{
							text6 = Uri.EscapeDataString(text6);
						}
						text6 = text5 + text6;
						stringBuilder2.Append(text6);
						continue;
					}
					throw new ArgumentException($"Path \"{Path}\" misses a \"{text4}\" parameter", Path);
				}
				if (text3 == ";")
				{
					if (stringBuilder2[stringBuilder2.Length - 1] == '=')
					{
						stringBuilder2 = stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
					}
					stringBuilder2 = stringBuilder2.Replace("=;", ";");
				}
				stringBuilder = stringBuilder.Replace(text, stringBuilder2.ToString());
			}
			return stringBuilder;
		}

		public void AddParameter(RequestParameterType type, string name, string value)
		{
			name.ThrowIfNull("name");
			if (value == null)
			{
				Logger.Warning("Add parameter should not get null values. type={0}, name={1}", type, name);
				return;
			}
			switch (type)
			{
			case RequestParameterType.Path:
				if (!PathParameters.ContainsKey(name))
				{
					PathParameters[name] = new List<string> { value };
				}
				else
				{
					PathParameters[name].Add(value);
				}
				break;
			case RequestParameterType.Query:
				QueryParameters.Add(new KeyValuePair<string, string>(name, value));
				break;
			default:
				throw new ArgumentOutOfRangeException("type");
			}
		}

		public HttpRequestMessage CreateRequest()
		{
			return new HttpRequestMessage(new HttpMethod(Method), BuildUri());
		}
	}
}
