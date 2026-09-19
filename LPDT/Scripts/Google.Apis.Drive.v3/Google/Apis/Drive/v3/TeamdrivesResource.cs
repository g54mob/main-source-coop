using Google.Apis.Discovery;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util;

namespace Google.Apis.Drive.v3
{
	public class TeamdrivesResource
	{
		public class CreateRequest : DriveBaseServiceRequest<TeamDrive>
		{
			[RequestParameter("requestId", RequestParameterType.Query)]
			public virtual string RequestId { get; private set; }

			private TeamDrive Body { get; set; }

			public override string MethodName => "create";

			public override string HttpMethod => "POST";

			public override string RestPath => "teamdrives";

			public CreateRequest(IClientService service, TeamDrive body, string requestId)
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
			[RequestParameter("teamDriveId", RequestParameterType.Path)]
			public virtual string TeamDriveId { get; private set; }

			public override string MethodName => "delete";

			public override string HttpMethod => "DELETE";

			public override string RestPath => "teamdrives/{teamDriveId}";

			public DeleteRequest(IClientService service, string teamDriveId)
				: base(service)
			{
				TeamDriveId = teamDriveId;
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("teamDriveId", new Parameter
				{
					Name = "teamDriveId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		public class GetRequest : DriveBaseServiceRequest<TeamDrive>
		{
			[RequestParameter("teamDriveId", RequestParameterType.Path)]
			public virtual string TeamDriveId { get; private set; }

			[RequestParameter("useDomainAdminAccess", RequestParameterType.Query)]
			public virtual bool? UseDomainAdminAccess { get; set; }

			public override string MethodName => "get";

			public override string HttpMethod => "GET";

			public override string RestPath => "teamdrives/{teamDriveId}";

			public GetRequest(IClientService service, string teamDriveId)
				: base(service)
			{
				TeamDriveId = teamDriveId;
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("teamDriveId", new Parameter
				{
					Name = "teamDriveId",
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

		public class ListRequest : DriveBaseServiceRequest<TeamDriveList>
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

			public override string RestPath => "teamdrives";

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

		public class UpdateRequest : DriveBaseServiceRequest<TeamDrive>
		{
			[RequestParameter("teamDriveId", RequestParameterType.Path)]
			public virtual string TeamDriveId { get; private set; }

			[RequestParameter("useDomainAdminAccess", RequestParameterType.Query)]
			public virtual bool? UseDomainAdminAccess { get; set; }

			private TeamDrive Body { get; set; }

			public override string MethodName => "update";

			public override string HttpMethod => "PATCH";

			public override string RestPath => "teamdrives/{teamDriveId}";

			public UpdateRequest(IClientService service, TeamDrive body, string teamDriveId)
				: base(service)
			{
				TeamDriveId = teamDriveId;
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
				base.RequestParameters.Add("teamDriveId", new Parameter
				{
					Name = "teamDriveId",
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

		private const string Resource = "teamdrives";

		private readonly IClientService service;

		public TeamdrivesResource(IClientService service)
		{
			this.service = service;
		}

		public virtual CreateRequest Create(TeamDrive body, string requestId)
		{
			return new CreateRequest(service, body, requestId);
		}

		public virtual DeleteRequest Delete(string teamDriveId)
		{
			return new DeleteRequest(service, teamDriveId);
		}

		public virtual GetRequest Get(string teamDriveId)
		{
			return new GetRequest(service, teamDriveId);
		}

		public virtual ListRequest List()
		{
			return new ListRequest(service);
		}

		public virtual UpdateRequest Update(TeamDrive body, string teamDriveId)
		{
			return new UpdateRequest(service, body, teamDriveId);
		}
	}
}
