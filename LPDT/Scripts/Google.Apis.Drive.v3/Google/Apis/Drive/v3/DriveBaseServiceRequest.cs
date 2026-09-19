using Google.Apis.Discovery;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Util;

namespace Google.Apis.Drive.v3
{
	public abstract class DriveBaseServiceRequest<TResponse> : ClientServiceRequest<TResponse>
	{
		public enum AltEnum
		{
			[StringValue("json")]
			Json = 0
		}

		[RequestParameter("alt", RequestParameterType.Query)]
		public virtual AltEnum? Alt { get; set; }

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

		[RequestParameter("userIp", RequestParameterType.Query)]
		public virtual string UserIp { get; set; }

		protected DriveBaseServiceRequest(IClientService service)
			: base(service)
		{
		}

		protected override void InitParameters()
		{
			base.InitParameters();
			base.RequestParameters.Add("alt", new Parameter
			{
				Name = "alt",
				IsRequired = false,
				ParameterType = "query",
				DefaultValue = "json",
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
			base.RequestParameters.Add("userIp", new Parameter
			{
				Name = "userIp",
				IsRequired = false,
				ParameterType = "query",
				DefaultValue = null,
				Pattern = null
			});
		}
	}
}
