using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;

namespace Google.Apis.Drive.v3
{
	public class ChannelsResource
	{
		public class StopRequest : DriveBaseServiceRequest<string>
		{
			private Channel Body { get; set; }

			public override string MethodName => "stop";

			public override string HttpMethod => "POST";

			public override string RestPath => "channels/stop";

			public StopRequest(IClientService service, Channel body)
				: base(service)
			{
				Body = body;
				InitParameters();
			}

			protected override object GetBody()
			{
				return Body;
			}

			protected override void InitParameters()
			{
				base.InitParameters();
			}
		}

		private const string Resource = "channels";

		private readonly IClientService service;

		public ChannelsResource(IClientService service)
		{
			this.service = service;
		}

		public virtual StopRequest Stop(Channel body)
		{
			return new StopRequest(service, body);
		}
	}
}
