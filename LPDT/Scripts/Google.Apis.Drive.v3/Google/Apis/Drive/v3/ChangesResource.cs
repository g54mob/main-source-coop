using Google.Apis.Discovery;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util;

namespace Google.Apis.Drive.v3
{
	public class ChangesResource
	{
		public class GetStartPageTokenRequest : DriveBaseServiceRequest<StartPageToken>
		{
			[RequestParameter("driveId", RequestParameterType.Query)]
			public virtual string DriveId { get; set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			[RequestParameter("teamDriveId", RequestParameterType.Query)]
			public virtual string TeamDriveId { get; set; }

			public override string MethodName => "getStartPageToken";

			public override string HttpMethod => "GET";

			public override string RestPath => "changes/startPageToken";

			public GetStartPageTokenRequest(IClientService service)
				: base(service)
			{
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("driveId", new Parameter
				{
					Name = "driveId",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("supportsAllDrives", new Parameter
				{
					Name = "supportsAllDrives",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("supportsTeamDrives", new Parameter
				{
					Name = "supportsTeamDrives",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("teamDriveId", new Parameter
				{
					Name = "teamDriveId",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		public class ListRequest : DriveBaseServiceRequest<ChangeList>
		{
			[RequestParameter("pageToken", RequestParameterType.Query)]
			public virtual string PageToken { get; private set; }

			[RequestParameter("driveId", RequestParameterType.Query)]
			public virtual string DriveId { get; set; }

			[RequestParameter("includeCorpusRemovals", RequestParameterType.Query)]
			public virtual bool? IncludeCorpusRemovals { get; set; }

			[RequestParameter("includeItemsFromAllDrives", RequestParameterType.Query)]
			public virtual bool? IncludeItemsFromAllDrives { get; set; }

			[RequestParameter("includeLabels", RequestParameterType.Query)]
			public virtual string IncludeLabels { get; set; }

			[RequestParameter("includePermissionsForView", RequestParameterType.Query)]
			public virtual string IncludePermissionsForView { get; set; }

			[RequestParameter("includeRemoved", RequestParameterType.Query)]
			public virtual bool? IncludeRemoved { get; set; }

			[RequestParameter("includeTeamDriveItems", RequestParameterType.Query)]
			public virtual bool? IncludeTeamDriveItems { get; set; }

			[RequestParameter("pageSize", RequestParameterType.Query)]
			public virtual int? PageSize { get; set; }

			[RequestParameter("restrictToMyDrive", RequestParameterType.Query)]
			public virtual bool? RestrictToMyDrive { get; set; }

			[RequestParameter("spaces", RequestParameterType.Query)]
			public virtual string Spaces { get; set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			[RequestParameter("teamDriveId", RequestParameterType.Query)]
			public virtual string TeamDriveId { get; set; }

			public override string MethodName => "list";

			public override string HttpMethod => "GET";

			public override string RestPath => "changes";

			public ListRequest(IClientService service, string pageToken)
				: base(service)
			{
				PageToken = pageToken;
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("pageToken", new Parameter
				{
					Name = "pageToken",
					IsRequired = true,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("driveId", new Parameter
				{
					Name = "driveId",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("includeCorpusRemovals", new Parameter
				{
					Name = "includeCorpusRemovals",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("includeItemsFromAllDrives", new Parameter
				{
					Name = "includeItemsFromAllDrives",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("includeLabels", new Parameter
				{
					Name = "includeLabels",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("includePermissionsForView", new Parameter
				{
					Name = "includePermissionsForView",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("includeRemoved", new Parameter
				{
					Name = "includeRemoved",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "true",
					Pattern = null
				});
				base.RequestParameters.Add("includeTeamDriveItems", new Parameter
				{
					Name = "includeTeamDriveItems",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("pageSize", new Parameter
				{
					Name = "pageSize",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "100",
					Pattern = null
				});
				base.RequestParameters.Add("restrictToMyDrive", new Parameter
				{
					Name = "restrictToMyDrive",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("spaces", new Parameter
				{
					Name = "spaces",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "drive",
					Pattern = null
				});
				base.RequestParameters.Add("supportsAllDrives", new Parameter
				{
					Name = "supportsAllDrives",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("supportsTeamDrives", new Parameter
				{
					Name = "supportsTeamDrives",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("teamDriveId", new Parameter
				{
					Name = "teamDriveId",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		public class WatchRequest : DriveBaseServiceRequest<Channel>
		{
			[RequestParameter("pageToken", RequestParameterType.Query)]
			public virtual string PageToken { get; private set; }

			[RequestParameter("driveId", RequestParameterType.Query)]
			public virtual string DriveId { get; set; }

			[RequestParameter("includeCorpusRemovals", RequestParameterType.Query)]
			public virtual bool? IncludeCorpusRemovals { get; set; }

			[RequestParameter("includeItemsFromAllDrives", RequestParameterType.Query)]
			public virtual bool? IncludeItemsFromAllDrives { get; set; }

			[RequestParameter("includeLabels", RequestParameterType.Query)]
			public virtual string IncludeLabels { get; set; }

			[RequestParameter("includePermissionsForView", RequestParameterType.Query)]
			public virtual string IncludePermissionsForView { get; set; }

			[RequestParameter("includeRemoved", RequestParameterType.Query)]
			public virtual bool? IncludeRemoved { get; set; }

			[RequestParameter("includeTeamDriveItems", RequestParameterType.Query)]
			public virtual bool? IncludeTeamDriveItems { get; set; }

			[RequestParameter("pageSize", RequestParameterType.Query)]
			public virtual int? PageSize { get; set; }

			[RequestParameter("restrictToMyDrive", RequestParameterType.Query)]
			public virtual bool? RestrictToMyDrive { get; set; }

			[RequestParameter("spaces", RequestParameterType.Query)]
			public virtual string Spaces { get; set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			[RequestParameter("teamDriveId", RequestParameterType.Query)]
			public virtual string TeamDriveId { get; set; }

			private Channel Body { get; set; }

			public override string MethodName => "watch";

			public override string HttpMethod => "POST";

			public override string RestPath => "changes/watch";

			public WatchRequest(IClientService service, Channel body, string pageToken)
				: base(service)
			{
				PageToken = pageToken;
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
				base.RequestParameters.Add("pageToken", new Parameter
				{
					Name = "pageToken",
					IsRequired = true,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("driveId", new Parameter
				{
					Name = "driveId",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("includeCorpusRemovals", new Parameter
				{
					Name = "includeCorpusRemovals",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("includeItemsFromAllDrives", new Parameter
				{
					Name = "includeItemsFromAllDrives",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("includeLabels", new Parameter
				{
					Name = "includeLabels",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("includePermissionsForView", new Parameter
				{
					Name = "includePermissionsForView",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("includeRemoved", new Parameter
				{
					Name = "includeRemoved",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "true",
					Pattern = null
				});
				base.RequestParameters.Add("includeTeamDriveItems", new Parameter
				{
					Name = "includeTeamDriveItems",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("pageSize", new Parameter
				{
					Name = "pageSize",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "100",
					Pattern = null
				});
				base.RequestParameters.Add("restrictToMyDrive", new Parameter
				{
					Name = "restrictToMyDrive",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("spaces", new Parameter
				{
					Name = "spaces",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "drive",
					Pattern = null
				});
				base.RequestParameters.Add("supportsAllDrives", new Parameter
				{
					Name = "supportsAllDrives",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("supportsTeamDrives", new Parameter
				{
					Name = "supportsTeamDrives",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("teamDriveId", new Parameter
				{
					Name = "teamDriveId",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		private const string Resource = "changes";

		private readonly IClientService service;

		public ChangesResource(IClientService service)
		{
			this.service = service;
		}

		public virtual GetStartPageTokenRequest GetStartPageToken()
		{
			return new GetStartPageTokenRequest(service);
		}

		public virtual ListRequest List(string pageToken)
		{
			return new ListRequest(service, pageToken);
		}

		public virtual WatchRequest Watch(Channel body, string pageToken)
		{
			return new WatchRequest(service, body, pageToken);
		}
	}
}
