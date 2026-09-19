using Google.Apis.Discovery;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util;

namespace Google.Apis.Drive.v3
{
	public class PermissionsResource
	{
		public class CreateRequest : DriveBaseServiceRequest<Permission>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("emailMessage", RequestParameterType.Query)]
			public virtual string EmailMessage { get; set; }

			[RequestParameter("enforceSingleParent", RequestParameterType.Query)]
			public virtual bool? EnforceSingleParent { get; set; }

			[RequestParameter("moveToNewOwnersRoot", RequestParameterType.Query)]
			public virtual bool? MoveToNewOwnersRoot { get; set; }

			[RequestParameter("sendNotificationEmail", RequestParameterType.Query)]
			public virtual bool? SendNotificationEmail { get; set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			[RequestParameter("transferOwnership", RequestParameterType.Query)]
			public virtual bool? TransferOwnership { get; set; }

			[RequestParameter("useDomainAdminAccess", RequestParameterType.Query)]
			public virtual bool? UseDomainAdminAccess { get; set; }

			private Permission Body { get; set; }

			public override string MethodName => "create";

			public override string HttpMethod => "POST";

			public override string RestPath => "files/{fileId}/permissions";

			public CreateRequest(IClientService service, Permission body, string fileId)
				: base(service)
			{
				FileId = fileId;
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
				base.RequestParameters.Add("fileId", new Parameter
				{
					Name = "fileId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("emailMessage", new Parameter
				{
					Name = "emailMessage",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("enforceSingleParent", new Parameter
				{
					Name = "enforceSingleParent",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("moveToNewOwnersRoot", new Parameter
				{
					Name = "moveToNewOwnersRoot",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("sendNotificationEmail", new Parameter
				{
					Name = "sendNotificationEmail",
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
				base.RequestParameters.Add("transferOwnership", new Parameter
				{
					Name = "transferOwnership",
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

		public class DeleteRequest : DriveBaseServiceRequest<string>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("permissionId", RequestParameterType.Path)]
			public virtual string PermissionId { get; private set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			[RequestParameter("useDomainAdminAccess", RequestParameterType.Query)]
			public virtual bool? UseDomainAdminAccess { get; set; }

			public override string MethodName => "delete";

			public override string HttpMethod => "DELETE";

			public override string RestPath => "files/{fileId}/permissions/{permissionId}";

			public DeleteRequest(IClientService service, string fileId, string permissionId)
				: base(service)
			{
				FileId = fileId;
				PermissionId = permissionId;
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("fileId", new Parameter
				{
					Name = "fileId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("permissionId", new Parameter
				{
					Name = "permissionId",
					IsRequired = true,
					ParameterType = "path",
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

		public class GetRequest : DriveBaseServiceRequest<Permission>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("permissionId", RequestParameterType.Path)]
			public virtual string PermissionId { get; private set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			[RequestParameter("useDomainAdminAccess", RequestParameterType.Query)]
			public virtual bool? UseDomainAdminAccess { get; set; }

			public override string MethodName => "get";

			public override string HttpMethod => "GET";

			public override string RestPath => "files/{fileId}/permissions/{permissionId}";

			public GetRequest(IClientService service, string fileId, string permissionId)
				: base(service)
			{
				FileId = fileId;
				PermissionId = permissionId;
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("fileId", new Parameter
				{
					Name = "fileId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("permissionId", new Parameter
				{
					Name = "permissionId",
					IsRequired = true,
					ParameterType = "path",
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

		public class ListRequest : DriveBaseServiceRequest<PermissionList>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("includePermissionsForView", RequestParameterType.Query)]
			public virtual string IncludePermissionsForView { get; set; }

			[RequestParameter("pageSize", RequestParameterType.Query)]
			public virtual int? PageSize { get; set; }

			[RequestParameter("pageToken", RequestParameterType.Query)]
			public virtual string PageToken { get; set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			[RequestParameter("useDomainAdminAccess", RequestParameterType.Query)]
			public virtual bool? UseDomainAdminAccess { get; set; }

			public override string MethodName => "list";

			public override string HttpMethod => "GET";

			public override string RestPath => "files/{fileId}/permissions";

			public ListRequest(IClientService service, string fileId)
				: base(service)
			{
				FileId = fileId;
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("fileId", new Parameter
				{
					Name = "fileId",
					IsRequired = true,
					ParameterType = "path",
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
				base.RequestParameters.Add("pageSize", new Parameter
				{
					Name = "pageSize",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
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

		public class UpdateRequest : DriveBaseServiceRequest<Permission>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("permissionId", RequestParameterType.Path)]
			public virtual string PermissionId { get; private set; }

			[RequestParameter("removeExpiration", RequestParameterType.Query)]
			public virtual bool? RemoveExpiration { get; set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			[RequestParameter("transferOwnership", RequestParameterType.Query)]
			public virtual bool? TransferOwnership { get; set; }

			[RequestParameter("useDomainAdminAccess", RequestParameterType.Query)]
			public virtual bool? UseDomainAdminAccess { get; set; }

			private Permission Body { get; set; }

			public override string MethodName => "update";

			public override string HttpMethod => "PATCH";

			public override string RestPath => "files/{fileId}/permissions/{permissionId}";

			public UpdateRequest(IClientService service, Permission body, string fileId, string permissionId)
				: base(service)
			{
				FileId = fileId;
				PermissionId = permissionId;
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
				base.RequestParameters.Add("fileId", new Parameter
				{
					Name = "fileId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("permissionId", new Parameter
				{
					Name = "permissionId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("removeExpiration", new Parameter
				{
					Name = "removeExpiration",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
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
				base.RequestParameters.Add("transferOwnership", new Parameter
				{
					Name = "transferOwnership",
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

		private const string Resource = "permissions";

		private readonly IClientService service;

		public PermissionsResource(IClientService service)
		{
			this.service = service;
		}

		public virtual CreateRequest Create(Permission body, string fileId)
		{
			return new CreateRequest(service, body, fileId);
		}

		public virtual DeleteRequest Delete(string fileId, string permissionId)
		{
			return new DeleteRequest(service, fileId, permissionId);
		}

		public virtual GetRequest Get(string fileId, string permissionId)
		{
			return new GetRequest(service, fileId, permissionId);
		}

		public virtual ListRequest List(string fileId)
		{
			return new ListRequest(service, fileId);
		}

		public virtual UpdateRequest Update(Permission body, string fileId, string permissionId)
		{
			return new UpdateRequest(service, body, fileId, permissionId);
		}
	}
}
