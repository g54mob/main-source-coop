using Google.Apis.Discovery;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util;

namespace Google.Apis.Drive.v3
{
	public class CommentsResource
	{
		public class CreateRequest : DriveBaseServiceRequest<Comment>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			private Comment Body { get; set; }

			public override string MethodName => "create";

			public override string HttpMethod => "POST";

			public override string RestPath => "files/{fileId}/comments";

			public CreateRequest(IClientService service, Comment body, string fileId)
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
			}
		}

		public class DeleteRequest : DriveBaseServiceRequest<string>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("commentId", RequestParameterType.Path)]
			public virtual string CommentId { get; private set; }

			public override string MethodName => "delete";

			public override string HttpMethod => "DELETE";

			public override string RestPath => "files/{fileId}/comments/{commentId}";

			public DeleteRequest(IClientService service, string fileId, string commentId)
				: base(service)
			{
				FileId = fileId;
				CommentId = commentId;
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
				base.RequestParameters.Add("commentId", new Parameter
				{
					Name = "commentId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		public class GetRequest : DriveBaseServiceRequest<Comment>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("commentId", RequestParameterType.Path)]
			public virtual string CommentId { get; private set; }

			[RequestParameter("includeDeleted", RequestParameterType.Query)]
			public virtual bool? IncludeDeleted { get; set; }

			public override string MethodName => "get";

			public override string HttpMethod => "GET";

			public override string RestPath => "files/{fileId}/comments/{commentId}";

			public GetRequest(IClientService service, string fileId, string commentId)
				: base(service)
			{
				FileId = fileId;
				CommentId = commentId;
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
				base.RequestParameters.Add("commentId", new Parameter
				{
					Name = "commentId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("includeDeleted", new Parameter
				{
					Name = "includeDeleted",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
			}
		}

		public class ListRequest : DriveBaseServiceRequest<CommentList>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("includeDeleted", RequestParameterType.Query)]
			public virtual bool? IncludeDeleted { get; set; }

			[RequestParameter("pageSize", RequestParameterType.Query)]
			public virtual int? PageSize { get; set; }

			[RequestParameter("pageToken", RequestParameterType.Query)]
			public virtual string PageToken { get; set; }

			[RequestParameter("startModifiedTime", RequestParameterType.Query)]
			public virtual string StartModifiedTime { get; set; }

			public override string MethodName => "list";

			public override string HttpMethod => "GET";

			public override string RestPath => "files/{fileId}/comments";

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
				base.RequestParameters.Add("includeDeleted", new Parameter
				{
					Name = "includeDeleted",
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
					DefaultValue = "20",
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
				base.RequestParameters.Add("startModifiedTime", new Parameter
				{
					Name = "startModifiedTime",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		public class UpdateRequest : DriveBaseServiceRequest<Comment>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("commentId", RequestParameterType.Path)]
			public virtual string CommentId { get; private set; }

			private Comment Body { get; set; }

			public override string MethodName => "update";

			public override string HttpMethod => "PATCH";

			public override string RestPath => "files/{fileId}/comments/{commentId}";

			public UpdateRequest(IClientService service, Comment body, string fileId, string commentId)
				: base(service)
			{
				FileId = fileId;
				CommentId = commentId;
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
				base.RequestParameters.Add("commentId", new Parameter
				{
					Name = "commentId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		private const string Resource = "comments";

		private readonly IClientService service;

		public CommentsResource(IClientService service)
		{
			this.service = service;
		}

		public virtual CreateRequest Create(Comment body, string fileId)
		{
			return new CreateRequest(service, body, fileId);
		}

		public virtual DeleteRequest Delete(string fileId, string commentId)
		{
			return new DeleteRequest(service, fileId, commentId);
		}

		public virtual GetRequest Get(string fileId, string commentId)
		{
			return new GetRequest(service, fileId, commentId);
		}

		public virtual ListRequest List(string fileId)
		{
			return new ListRequest(service, fileId);
		}

		public virtual UpdateRequest Update(Comment body, string fileId, string commentId)
		{
			return new UpdateRequest(service, body, fileId, commentId);
		}
	}
}
