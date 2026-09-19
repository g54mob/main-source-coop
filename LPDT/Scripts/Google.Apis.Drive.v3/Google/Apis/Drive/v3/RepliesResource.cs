using Google.Apis.Discovery;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util;

namespace Google.Apis.Drive.v3
{
	public class RepliesResource
	{
		public class CreateRequest : DriveBaseServiceRequest<Reply>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("commentId", RequestParameterType.Path)]
			public virtual string CommentId { get; private set; }

			private Reply Body { get; set; }

			public override string MethodName => "create";

			public override string HttpMethod => "POST";

			public override string RestPath => "files/{fileId}/comments/{commentId}/replies";

			public CreateRequest(IClientService service, Reply body, string fileId, string commentId)
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

		public class DeleteRequest : DriveBaseServiceRequest<string>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("commentId", RequestParameterType.Path)]
			public virtual string CommentId { get; private set; }

			[RequestParameter("replyId", RequestParameterType.Path)]
			public virtual string ReplyId { get; private set; }

			public override string MethodName => "delete";

			public override string HttpMethod => "DELETE";

			public override string RestPath => "files/{fileId}/comments/{commentId}/replies/{replyId}";

			public DeleteRequest(IClientService service, string fileId, string commentId, string replyId)
				: base(service)
			{
				FileId = fileId;
				CommentId = commentId;
				ReplyId = replyId;
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
				base.RequestParameters.Add("replyId", new Parameter
				{
					Name = "replyId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		public class GetRequest : DriveBaseServiceRequest<Reply>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("commentId", RequestParameterType.Path)]
			public virtual string CommentId { get; private set; }

			[RequestParameter("replyId", RequestParameterType.Path)]
			public virtual string ReplyId { get; private set; }

			[RequestParameter("includeDeleted", RequestParameterType.Query)]
			public virtual bool? IncludeDeleted { get; set; }

			public override string MethodName => "get";

			public override string HttpMethod => "GET";

			public override string RestPath => "files/{fileId}/comments/{commentId}/replies/{replyId}";

			public GetRequest(IClientService service, string fileId, string commentId, string replyId)
				: base(service)
			{
				FileId = fileId;
				CommentId = commentId;
				ReplyId = replyId;
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
				base.RequestParameters.Add("replyId", new Parameter
				{
					Name = "replyId",
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

		public class ListRequest : DriveBaseServiceRequest<ReplyList>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("commentId", RequestParameterType.Path)]
			public virtual string CommentId { get; private set; }

			[RequestParameter("includeDeleted", RequestParameterType.Query)]
			public virtual bool? IncludeDeleted { get; set; }

			[RequestParameter("pageSize", RequestParameterType.Query)]
			public virtual int? PageSize { get; set; }

			[RequestParameter("pageToken", RequestParameterType.Query)]
			public virtual string PageToken { get; set; }

			public override string MethodName => "list";

			public override string HttpMethod => "GET";

			public override string RestPath => "files/{fileId}/comments/{commentId}/replies";

			public ListRequest(IClientService service, string fileId, string commentId)
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
			}
		}

		public class UpdateRequest : DriveBaseServiceRequest<Reply>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("commentId", RequestParameterType.Path)]
			public virtual string CommentId { get; private set; }

			[RequestParameter("replyId", RequestParameterType.Path)]
			public virtual string ReplyId { get; private set; }

			private Reply Body { get; set; }

			public override string MethodName => "update";

			public override string HttpMethod => "PATCH";

			public override string RestPath => "files/{fileId}/comments/{commentId}/replies/{replyId}";

			public UpdateRequest(IClientService service, Reply body, string fileId, string commentId, string replyId)
				: base(service)
			{
				FileId = fileId;
				CommentId = commentId;
				ReplyId = replyId;
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
				base.RequestParameters.Add("replyId", new Parameter
				{
					Name = "replyId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		private const string Resource = "replies";

		private readonly IClientService service;

		public RepliesResource(IClientService service)
		{
			this.service = service;
		}

		public virtual CreateRequest Create(Reply body, string fileId, string commentId)
		{
			return new CreateRequest(service, body, fileId, commentId);
		}

		public virtual DeleteRequest Delete(string fileId, string commentId, string replyId)
		{
			return new DeleteRequest(service, fileId, commentId, replyId);
		}

		public virtual GetRequest Get(string fileId, string commentId, string replyId)
		{
			return new GetRequest(service, fileId, commentId, replyId);
		}

		public virtual ListRequest List(string fileId, string commentId)
		{
			return new ListRequest(service, fileId, commentId);
		}

		public virtual UpdateRequest Update(Reply body, string fileId, string commentId, string replyId)
		{
			return new UpdateRequest(service, body, fileId, commentId, replyId);
		}
	}
}
