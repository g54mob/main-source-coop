using Google.Apis.Discovery;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Util;

namespace Google.Apis.Sheets.v4
{
	public abstract class SheetsBaseServiceRequest<TResponse> : ClientServiceRequest<TResponse>
	{
		public enum XgafvEnum
		{
			[StringValue("1")]
			Value1 = 0,
			[StringValue("2")]
			Value2 = 1
		}

		public enum AltEnum
		{
			[StringValue("json")]
			Json = 0,
			[StringValue("media")]
			Media = 1,
			[StringValue("proto")]
			Proto = 2
		}

		[RequestParameter("$.xgafv", RequestParameterType.Query)]
		public virtual XgafvEnum? Xgafv { get; set; }

		[RequestParameter("access_token", RequestParameterType.Query)]
		public virtual string AccessToken { get; set; }

		[RequestParameter("alt", RequestParameterType.Query)]
		public virtual AltEnum? Alt { get; set; }

		[RequestParameter("callback", RequestParameterType.Query)]
		public virtual string Callback { get; set; }

		[RequestParameter("fields", RequestParameterType.Query)]
		public virtual string Fields { get; set; }

		[RequestParameter("key", RequestParameterType.Query)]
		public virtual string Key { get; set; }

		[RequestParameter("oauth_token", RequestParameterType.Query)]
		public virtual string OauthToken { get; set; }

		[RequestParameter("prettyPrint", RequestParameterType.Query)]
		public virtual bool? PrettyPrint { get; set; }

		[RequestParameter("quotaUser", RequestParameterType.Query)]
		public virtual string QuotaUser { get; set; }

		[RequestParameter("uploadType", RequestParameterType.Query)]
		public virtual string UploadType { get; set; }

		[RequestParameter("upload_protocol", RequestParameterType.Query)]
		public virtual string UploadProtocol { get; set; }

		protected SheetsBaseServiceRequest(IClientService service)
			: base(service)
		{
		}

		protected override void InitParameters()
		{
			base.InitParameters();
			base.RequestParameters.Add("$.xgafv", new Parameter
			{
				Name = "$.xgafv",
				IsRequired = false,
				ParameterType = "query",
				DefaultValue = null,
				Pattern = null
			});
			base.RequestParameters.Add("access_token", new Parameter
			{
				Name = "access_token",
				IsRequired = false,
				ParameterType = "query",
				DefaultValue = null,
				Pattern = null
			});
			base.RequestParameters.Add("alt", new Parameter
			{
				Name = "alt",
				IsRequired = false,
				ParameterType = "query",
				DefaultValue = "json",
				Pattern = null
			});
			base.RequestParameters.Add("callback", new Parameter
			{
				Name = "callback",
				IsRequired = false,
				ParameterType = "query",
				DefaultValue = null,
				Pattern = null
			});
			base.RequestParameters.Add("fields", new Parameter
			{
				Name = "fields",
				IsRequired = false,
				ParameterType = "query",
				DefaultValue = null,
				Pattern = null
			});
			base.RequestParameters.Add("key", new Parameter
			{
				Name = "key",
				IsRequired = false,
				ParameterType = "query",
				DefaultValue = null,
				Pattern = null
			});
			base.RequestParameters.Add("oauth_token", new Parameter
			{
				Name = "oauth_token",
				IsRequired = false,
				ParameterType = "query",
				DefaultValue = null,
				Pattern = null
			});
			base.RequestParameters.Add("prettyPrint", new Parameter
			{
				Name = "prettyPrint",
				IsRequired = false,
				ParameterType = "query",
				DefaultValue = "true",
				Pattern = null
			});
			base.RequestParameters.Add("quotaUser", new Parameter
			{
				Name = "quotaUser",
				IsRequired = false,
				ParameterType = "query",
				DefaultValue = null,
				Pattern = null
			});
			base.RequestParameters.Add("uploadType", new Parameter
			{
				Name = "uploadType",
				IsRequired = false,
				ParameterType = "query",
				DefaultValue = null,
				Pattern = null
			});
			base.RequestParameters.Add("upload_protocol", new Parameter
			{
				Name = "upload_protocol",
				IsRequired = false,
				ParameterType = "query",
				DefaultValue = null,
				Pattern = null
			});
		}
	}
}
