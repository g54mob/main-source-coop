using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;

namespace Google.Apis.Drive.v3
{
	public class AboutResource
	{
		public class GetRequest : DriveBaseServiceRequest<About>
		{
			public override string MethodName => "get";

			public override string HttpMethod => "GET";

			public override string RestPath => "about";

			public GetRequest(IClientService service)
				: base(service)
			{
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
			}
		}

		private const string Resource = "about";

		private readonly IClientService service;

		public AboutResource(IClientService service)
		{
			this.service = service;
		}

		public virtual GetRequest Get()
		{
			return new GetRequest(service);
		}
	}
}
