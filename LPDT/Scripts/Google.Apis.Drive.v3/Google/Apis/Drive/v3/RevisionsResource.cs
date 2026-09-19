using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Discovery;
using Google.Apis.Download;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util;

namespace Google.Apis.Drive.v3
{
	public class RevisionsResource
	{
		public class DeleteRequest : DriveBaseServiceRequest<string>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("revisionId", RequestParameterType.Path)]
			public virtual string RevisionId { get; private set; }

			public override string MethodName => "delete";

			public override string HttpMethod => "DELETE";

			public override string RestPath => "files/{fileId}/revisions/{revisionId}";

			public DeleteRequest(IClientService service, string fileId, string revisionId)
				: base(service)
			{
				FileId = fileId;
				RevisionId = revisionId;
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
				base.RequestParameters.Add("revisionId", new Parameter
				{
					Name = "revisionId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		public class GetRequest : DriveBaseServiceRequest<Revision>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("revisionId", RequestParameterType.Path)]
			public virtual string RevisionId { get; private set; }

			[RequestParameter("acknowledgeAbuse", RequestParameterType.Query)]
			public virtual bool? AcknowledgeAbuse { get; set; }

			public override string MethodName => "get";

			public override string HttpMethod => "GET";

			public override string RestPath => "files/{fileId}/revisions/{revisionId}";

			public IMediaDownloader MediaDownloader { get; private set; }

			public GetRequest(IClientService service, string fileId, string revisionId)
				: base(service)
			{
				FileId = fileId;
				RevisionId = revisionId;
				MediaDownloader = new MediaDownloader(service);
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
				base.RequestParameters.Add("revisionId", new Parameter
				{
					Name = "revisionId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("acknowledgeAbuse", new Parameter
				{
					Name = "acknowledgeAbuse",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
			}

			public virtual void Download(Stream stream)
			{
				MediaDownloader obj = (MediaDownloader)MediaDownloader;
				obj.Range = null;
				obj.Download(GenerateRequestUri(), stream);
			}

			public virtual IDownloadProgress DownloadWithStatus(Stream stream)
			{
				MediaDownloader obj = (MediaDownloader)MediaDownloader;
				obj.Range = null;
				return obj.Download(GenerateRequestUri(), stream);
			}

			public virtual Task<IDownloadProgress> DownloadAsync(Stream stream)
			{
				MediaDownloader obj = (MediaDownloader)MediaDownloader;
				obj.Range = null;
				return obj.DownloadAsync(GenerateRequestUri(), stream);
			}

			public virtual Task<IDownloadProgress> DownloadAsync(Stream stream, CancellationToken cancellationToken)
			{
				MediaDownloader obj = (MediaDownloader)MediaDownloader;
				obj.Range = null;
				return obj.DownloadAsync(GenerateRequestUri(), stream, cancellationToken);
			}

			public virtual IDownloadProgress DownloadRange(Stream stream, RangeHeaderValue range)
			{
				MediaDownloader obj = (MediaDownloader)MediaDownloader;
				obj.Range = range;
				return obj.Download(GenerateRequestUri(), stream);
			}

			public virtual Task<IDownloadProgress> DownloadRangeAsync(Stream stream, RangeHeaderValue range, CancellationToken cancellationToken = default(CancellationToken))
			{
				MediaDownloader obj = (MediaDownloader)MediaDownloader;
				obj.Range = range;
				return obj.DownloadAsync(GenerateRequestUri(), stream, cancellationToken);
			}
		}

		public class ListRequest : DriveBaseServiceRequest<RevisionList>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("pageSize", RequestParameterType.Query)]
			public virtual int? PageSize { get; set; }

			[RequestParameter("pageToken", RequestParameterType.Query)]
			public virtual string PageToken { get; set; }

			public override string MethodName => "list";

			public override string HttpMethod => "GET";

			public override string RestPath => "files/{fileId}/revisions";

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
				base.RequestParameters.Add("pageSize", new Parameter
				{
					Name = "pageSize",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "200",
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

		public class UpdateRequest : DriveBaseServiceRequest<Revision>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("revisionId", RequestParameterType.Path)]
			public virtual string RevisionId { get; private set; }

			private Revision Body { get; set; }

			public override string MethodName => "update";

			public override string HttpMethod => "PATCH";

			public override string RestPath => "files/{fileId}/revisions/{revisionId}";

			public UpdateRequest(IClientService service, Revision body, string fileId, string revisionId)
				: base(service)
			{
				FileId = fileId;
				RevisionId = revisionId;
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
				base.RequestParameters.Add("revisionId", new Parameter
				{
					Name = "revisionId",
					IsRequired = true,
					ParameterType = "path",
					DefaultValue = null,
					Pattern = null
				});
			}
		}

		private const string Resource = "revisions";

		private readonly IClientService service;

		public RevisionsResource(IClientService service)
		{
			this.service = service;
		}

		public virtual DeleteRequest Delete(string fileId, string revisionId)
		{
			return new DeleteRequest(service, fileId, revisionId);
		}

		public virtual GetRequest Get(string fileId, string revisionId)
		{
			return new GetRequest(service, fileId, revisionId);
		}

		public virtual ListRequest List(string fileId)
		{
			return new ListRequest(service, fileId);
		}

		public virtual UpdateRequest Update(Revision body, string fileId, string revisionId)
		{
			return new UpdateRequest(service, body, fileId, revisionId);
		}
	}
}
