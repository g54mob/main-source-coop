using Google.Apis.Discovery;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util;

namespace Google.Apis.Drive.v3
{
	public class DrivesResource
	{
		public class CreateRequest : DriveBaseServiceRequest<Google.Apis.Drive.v3.Data.Drive>
		{
			[RequestParameter("requestId", RequestParameterType.Query)]
			public virtual string RequestId { get; private set; }

			private Google.Apis.Drive.v3.Data.Drive Body { get; set; }

			public override string MethodName => "create";

			public override string HttpMethod => "POST";

			public override string RestPath => "drives";

			public CreateRequest(IClientService service, Google.Apis.Drive.v3.Data.Drive body, string requestId)
				: base(service)
			{
				RequestId = requestId;
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
				base.RequestParameters.Add("requestId", new Parameter
				{
					Name = "requestId",
					IsRequired = true,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		public class DeleteRequest : DriveBaseServiceRequest<string>
		{
			[RequestParameter("driveId", RequestParameterType.Path)]
			public virtual string DriveId { get; private set; }

			[RequestParameter("allowItemDeletion", RequestParameterType.Query)]
			public virtual bool? AllowItemDeletion { get; set; }

			[RequestParameter("useDomainAdminAccess", RequestParameterType.Query)]
			public virtual bool? UseDomainAdminAccess { get; set; }

			public override string MethodName => "delete";

			public override string HttpMethod => "DELETE";

			public override string RestPath => "drives/{driveId}";

			public DeleteRequest(IClientService service, string driveId)
				: base(service)
			{
				DriveId = driveId;
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("driveId", new Parameter
				{
					Name = "driveId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("allowItemDeletion", new Parameter
				{
					Name = "allowItemDeletion",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("useDomainAdminAccess", new Parameter
				{
					Name = "useDomainAdminAccess",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
			}
		}

		public class GetRequest : DriveBaseServiceRequest<Google.Apis.Drive.v3.Data.Drive>
		{
			[RequestParameter("driveId", RequestParameterType.Path)]
			public virtual string DriveId { get; private set; }

			[RequestParameter("useDomainAdminAccess", RequestParameterType.Query)]
			public virtual bool? UseDomainAdminAccess { get; set; }

			public override string MethodName => "get";

			public override string HttpMethod => "GET";

			public override string RestPath => "drives/{driveId}";

			public GetRequest(IClientService service, string driveId)
				: base(service)
			{
				DriveId = driveId;
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("driveId", new Parameter
				{
					Name = "driveId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("useDomainAdminAccess", new Parameter
				{
					Name = "useDomainAdminAccess",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
			}
		}

		public class HideRequest : DriveBaseServiceRequest<Google.Apis.Drive.v3.Data.Drive>
		{
			[RequestParameter("driveId", RequestParameterType.Path)]
			public virtual string DriveId { get; private set; }

			public override string MethodName => "hide";

			public override string HttpMethod => "POST";

			public override string RestPath => "drives/{driveId}/hide";

			public HideRequest(IClientService service, string driveId)
				: base(service)
			{
				DriveId = driveId;
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("driveId", new Parameter
				{
					Name = "driveId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		public class ListRequest : DriveBaseServiceRequest<DriveList>
		{
			[RequestParameter("pageSize", RequestParameterType.Query)]
			public virtual int? PageSize { get; set; }

			[RequestParameter("pageToken", RequestParameterType.Query)]
			public virtual string PageToken { get; set; }

			[RequestParameter("q", RequestParameterType.Query)]
			public virtual string Q { get; set; }

			[RequestParameter("useDomainAdminAccess", RequestParameterType.Query)]
			public virtual bool? UseDomainAdminAccess { get; set; }

			public override string MethodName => "list";

			public override string HttpMethod => "GET";

			public override string RestPath => "drives";

			public ListRequest(IClientService service)
				: base(service)
			{
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("pageSize", new Parameter
				{
					Name = "pageSize",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "10",
					Pattern = null
				});
				base.RequestParameters.Add("pageToken", new Parameter
				{
					Name = "pageToken",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("q", new Parameter
				{
					Name = "q",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("useDomainAdminAccess", new Parameter
				{
					Name = "useDomainAdminAccess",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
			}
		}

		public class UnhideRequest : DriveBaseServiceRequest<Google.Apis.Drive.v3.Data.Drive>
		{
			[RequestParameter("driveId", RequestParameterType.Path)]
			public virtual string DriveId { get; private set; }

			public override string MethodName => "unhide";

			public override string HttpMethod => "POST";

			public override string RestPath => "drives/{driveId}/unhide";

			public UnhideRequest(IClientService service, string driveId)
				: base(service)
			{
				DriveId = driveId;
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("driveId", new Parameter
				{
					Name = "driveId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		public class UpdateRequest : DriveBaseServiceRequest<Google.Apis.Drive.v3.Data.Drive>
		{
			[RequestParameter("driveId", RequestParameterType.Path)]
			public virtual string DriveId { get; private set; }

			[RequestParameter("useDomainAdminAccess", RequestParameterType.Query)]
			public virtual bool? UseDomainAdminAccess { get; set; }

			private Google.Apis.Drive.v3.Data.Drive Body { get; set; }

			public override string MethodName => "update";

			public override string HttpMethod => "PATCH";

			public override string RestPath => "drives/{driveId}";

			public UpdateRequest(IClientService service, Google.Apis.Drive.v3.Data.Drive body, string driveId)
				: base(service)
			{
				DriveId = driveId;
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
				base.RequestParameters.Add("driveId", new Parameter
				{
					Name = "driveId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("useDomainAdminAccess", new Parameter
				{
					Name = "useDomainAdminAccess",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
			}
		}

		private const string Resource = "drives";

		private readonly IClientService service;

		public DrivesResource(IClientService service)
		{
			this.service = service;
		}

		public virtual CreateRequest Create(Google.Apis.Drive.v3.Data.Drive body, string requestId)
		{
			return new CreateRequest(service, body, requestId);
		}

		public virtual DeleteRequest Delete(string driveId)
		{
			return new DeleteRequest(service, driveId);
		}

		public virtual GetRequest Get(string driveId)
		{
			return new GetRequest(service, driveId);
		}

		public virtual HideRequest Hide(string driveId)
		{
			return new HideRequest(service, driveId);
		}

		public virtual ListRequest List()
		{
			return new ListRequest(service);
		}

		public virtual UnhideRequest Unhide(string driveId)
		{
			return new UnhideRequest(service, driveId);
		}

		public virtual UpdateRequest Update(Google.Apis.Drive.v3.Data.Drive body, string driveId)
		{
			return new UpdateRequest(service, body, driveId);
		}
	}
}
