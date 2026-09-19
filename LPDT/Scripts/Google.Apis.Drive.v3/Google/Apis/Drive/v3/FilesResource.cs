using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Discovery;
using Google.Apis.Download;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util;

namespace Google.Apis.Drive.v3
{
	public class FilesResource
	{
		public class CopyRequest : DriveBaseServiceRequest<Google.Apis.Drive.v3.Data.File>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("enforceSingleParent", RequestParameterType.Query)]
			public virtual bool? EnforceSingleParent { get; set; }

			[RequestParameter("ignoreDefaultVisibility", RequestParameterType.Query)]
			public virtual bool? IgnoreDefaultVisibility { get; set; }

			[RequestParameter("includeLabels", RequestParameterType.Query)]
			public virtual string IncludeLabels { get; set; }

			[RequestParameter("includePermissionsForView", RequestParameterType.Query)]
			public virtual string IncludePermissionsForView { get; set; }

			[RequestParameter("keepRevisionForever", RequestParameterType.Query)]
			public virtual bool? KeepRevisionForever { get; set; }

			[RequestParameter("ocrLanguage", RequestParameterType.Query)]
			public virtual string OcrLanguage { get; set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			private Google.Apis.Drive.v3.Data.File Body { get; set; }

			public override string MethodName => "copy";

			public override string HttpMethod => "POST";

			public override string RestPath => "files/{fileId}/copy";

			public CopyRequest(IClientService service, Google.Apis.Drive.v3.Data.File body, string fileId)
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
				base.RequestParameters.Add("enforceSingleParent", new Parameter
				{
					Name = "enforceSingleParent",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("ignoreDefaultVisibility", new Parameter
				{
					Name = "ignoreDefaultVisibility",
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
				base.RequestParameters.Add("keepRevisionForever", new Parameter
				{
					Name = "keepRevisionForever",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("ocrLanguage", new Parameter
				{
					Name = "ocrLanguage",
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
			}
		}

		public class CreateRequest : DriveBaseServiceRequest<Google.Apis.Drive.v3.Data.File>
		{
			[RequestParameter("enforceSingleParent", RequestParameterType.Query)]
			public virtual bool? EnforceSingleParent { get; set; }

			[RequestParameter("ignoreDefaultVisibility", RequestParameterType.Query)]
			public virtual bool? IgnoreDefaultVisibility { get; set; }

			[RequestParameter("includeLabels", RequestParameterType.Query)]
			public virtual string IncludeLabels { get; set; }

			[RequestParameter("includePermissionsForView", RequestParameterType.Query)]
			public virtual string IncludePermissionsForView { get; set; }

			[RequestParameter("keepRevisionForever", RequestParameterType.Query)]
			public virtual bool? KeepRevisionForever { get; set; }

			[RequestParameter("ocrLanguage", RequestParameterType.Query)]
			public virtual string OcrLanguage { get; set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			[RequestParameter("useContentAsIndexableText", RequestParameterType.Query)]
			public virtual bool? UseContentAsIndexableText { get; set; }

			private Google.Apis.Drive.v3.Data.File Body { get; set; }

			public override string MethodName => "create";

			public override string HttpMethod => "POST";

			public override string RestPath => "files";

			public CreateRequest(IClientService service, Google.Apis.Drive.v3.Data.File body)
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
				base.RequestParameters.Add("enforceSingleParent", new Parameter
				{
					Name = "enforceSingleParent",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("ignoreDefaultVisibility", new Parameter
				{
					Name = "ignoreDefaultVisibility",
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
				base.RequestParameters.Add("keepRevisionForever", new Parameter
				{
					Name = "keepRevisionForever",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("ocrLanguage", new Parameter
				{
					Name = "ocrLanguage",
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
				base.RequestParameters.Add("useContentAsIndexableText", new Parameter
				{
					Name = "useContentAsIndexableText",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
			}
		}

		public class CreateMediaUpload : ResumableUpload<Google.Apis.Drive.v3.Data.File, Google.Apis.Drive.v3.Data.File>
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

			[RequestParameter("enforceSingleParent", RequestParameterType.Query)]
			public virtual bool? EnforceSingleParent { get; set; }

			[RequestParameter("ignoreDefaultVisibility", RequestParameterType.Query)]
			public virtual bool? IgnoreDefaultVisibility { get; set; }

			[RequestParameter("includeLabels", RequestParameterType.Query)]
			public virtual string IncludeLabels { get; set; }

			[RequestParameter("includePermissionsForView", RequestParameterType.Query)]
			public virtual string IncludePermissionsForView { get; set; }

			[RequestParameter("keepRevisionForever", RequestParameterType.Query)]
			public virtual bool? KeepRevisionForever { get; set; }

			[RequestParameter("ocrLanguage", RequestParameterType.Query)]
			public virtual string OcrLanguage { get; set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			[RequestParameter("useContentAsIndexableText", RequestParameterType.Query)]
			public virtual bool? UseContentAsIndexableText { get; set; }

			public CreateMediaUpload(IClientService service, Google.Apis.Drive.v3.Data.File body, Stream stream, string contentType)
				: base(service, string.Format("/{0}/{1}{2}", "upload", service.BasePath, "files"), "POST", stream, contentType)
			{
				base.Body = body;
			}
		}

		public class DeleteRequest : DriveBaseServiceRequest<string>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("enforceSingleParent", RequestParameterType.Query)]
			public virtual bool? EnforceSingleParent { get; set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			public override string MethodName => "delete";

			public override string HttpMethod => "DELETE";

			public override string RestPath => "files/{fileId}";

			public DeleteRequest(IClientService service, string fileId)
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
				base.RequestParameters.Add("enforceSingleParent", new Parameter
				{
					Name = "enforceSingleParent",
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
			}
		}

		public class EmptyTrashRequest : DriveBaseServiceRequest<string>
		{
			[RequestParameter("enforceSingleParent", RequestParameterType.Query)]
			public virtual bool? EnforceSingleParent { get; set; }

			public override string MethodName => "emptyTrash";

			public override string HttpMethod => "DELETE";

			public override string RestPath => "files/trash";

			public EmptyTrashRequest(IClientService service)
				: base(service)
			{
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("enforceSingleParent", new Parameter
				{
					Name = "enforceSingleParent",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
			}
		}

		public class ExportRequest : DriveBaseServiceRequest<string>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("mimeType", RequestParameterType.Query)]
			public virtual string MimeType { get; private set; }

			public override string MethodName => "export";

			public override string HttpMethod => "GET";

			public override string RestPath => "files/{fileId}/export";

			public IMediaDownloader MediaDownloader { get; private set; }

			public ExportRequest(IClientService service, string fileId, string mimeType)
				: base(service)
			{
				FileId = fileId;
				MimeType = mimeType;
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
				base.RequestParameters.Add("mimeType", new Parameter
				{
					Name = "mimeType",
					IsRequired = true,
					ParameterType = "query",
					DefaultValue = null,
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

		public class GenerateIdsRequest : DriveBaseServiceRequest<GeneratedIds>
		{
			[RequestParameter("count", RequestParameterType.Query)]
			public virtual int? Count { get; set; }

			[RequestParameter("space", RequestParameterType.Query)]
			public virtual string Space { get; set; }

			[RequestParameter("type", RequestParameterType.Query)]
			public virtual string Type { get; set; }

			public override string MethodName => "generateIds";

			public override string HttpMethod => "GET";

			public override string RestPath => "files/generateIds";

			public GenerateIdsRequest(IClientService service)
				: base(service)
			{
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("count", new Parameter
				{
					Name = "count",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "10",
					Pattern = null
				});
				base.RequestParameters.Add("space", new Parameter
				{
					Name = "space",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "drive",
					Pattern = null
				});
				base.RequestParameters.Add("type", new Parameter
				{
					Name = "type",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "files",
					Pattern = null
				});
			}
		}

		public class GetRequest : DriveBaseServiceRequest<Google.Apis.Drive.v3.Data.File>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("acknowledgeAbuse", RequestParameterType.Query)]
			public virtual bool? AcknowledgeAbuse { get; set; }

			[RequestParameter("includeLabels", RequestParameterType.Query)]
			public virtual string IncludeLabels { get; set; }

			[RequestParameter("includePermissionsForView", RequestParameterType.Query)]
			public virtual string IncludePermissionsForView { get; set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			public override string MethodName => "get";

			public override string HttpMethod => "GET";

			public override string RestPath => "files/{fileId}";

			public IMediaDownloader MediaDownloader { get; private set; }

			public GetRequest(IClientService service, string fileId)
				: base(service)
			{
				FileId = fileId;
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
				base.RequestParameters.Add("acknowledgeAbuse", new Parameter
				{
					Name = "acknowledgeAbuse",
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

		public class ListRequest : DriveBaseServiceRequest<FileList>
		{
			public enum CorpusEnum
			{
				[StringValue("domain")]
				Domain = 0,
				[StringValue("user")]
				User = 1
			}

			[RequestParameter("corpora", RequestParameterType.Query)]
			public virtual string Corpora { get; set; }

			[RequestParameter("corpus", RequestParameterType.Query)]
			public virtual CorpusEnum? Corpus { get; set; }

			[RequestParameter("driveId", RequestParameterType.Query)]
			public virtual string DriveId { get; set; }

			[RequestParameter("includeItemsFromAllDrives", RequestParameterType.Query)]
			public virtual bool? IncludeItemsFromAllDrives { get; set; }

			[RequestParameter("includeLabels", RequestParameterType.Query)]
			public virtual string IncludeLabels { get; set; }

			[RequestParameter("includePermissionsForView", RequestParameterType.Query)]
			public virtual string IncludePermissionsForView { get; set; }

			[RequestParameter("includeTeamDriveItems", RequestParameterType.Query)]
			public virtual bool? IncludeTeamDriveItems { get; set; }

			[RequestParameter("orderBy", RequestParameterType.Query)]
			public virtual string OrderBy { get; set; }

			[RequestParameter("pageSize", RequestParameterType.Query)]
			public virtual int? PageSize { get; set; }

			[RequestParameter("pageToken", RequestParameterType.Query)]
			public virtual string PageToken { get; set; }

			[RequestParameter("q", RequestParameterType.Query)]
			public virtual string Q { get; set; }

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

			public override string RestPath => "files";

			public ListRequest(IClientService service)
				: base(service)
			{
				InitParameters();
			}

			protected override void InitParameters()
			{
				base.InitParameters();
				base.RequestParameters.Add("corpora", new Parameter
				{
					Name = "corpora",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("corpus", new Parameter
				{
					Name = "corpus",
					IsRequired = false,
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
				base.RequestParameters.Add("includeTeamDriveItems", new Parameter
				{
					Name = "includeTeamDriveItems",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("orderBy", new Parameter
				{
					Name = "orderBy",
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
					DefaultValue = "100",
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

		public class ListLabelsRequest : DriveBaseServiceRequest<LabelList>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("maxResults", RequestParameterType.Query)]
			public virtual int? MaxResults { get; set; }

			[RequestParameter("pageToken", RequestParameterType.Query)]
			public virtual string PageToken { get; set; }

			public override string MethodName => "listLabels";

			public override string HttpMethod => "GET";

			public override string RestPath => "files/{fileId}/listLabels";

			public ListLabelsRequest(IClientService service, string fileId)
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
				base.RequestParameters.Add("maxResults", new Parameter
				{
					Name = "maxResults",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "100",
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

		public class ModifyLabelsRequest : DriveBaseServiceRequest<ModifyLabelsResponse>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			private Google.Apis.Drive.v3.Data.ModifyLabelsRequest Body { get; set; }

			public override string MethodName => "modifyLabels";

			public override string HttpMethod => "POST";

			public override string RestPath => "files/{fileId}/modifyLabels";

			public ModifyLabelsRequest(IClientService service, Google.Apis.Drive.v3.Data.ModifyLabelsRequest body, string fileId)
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

		public class UpdateRequest : DriveBaseServiceRequest<Google.Apis.Drive.v3.Data.File>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("addParents", RequestParameterType.Query)]
			public virtual string AddParents { get; set; }

			[RequestParameter("enforceSingleParent", RequestParameterType.Query)]
			public virtual bool? EnforceSingleParent { get; set; }

			[RequestParameter("includeLabels", RequestParameterType.Query)]
			public virtual string IncludeLabels { get; set; }

			[RequestParameter("includePermissionsForView", RequestParameterType.Query)]
			public virtual string IncludePermissionsForView { get; set; }

			[RequestParameter("keepRevisionForever", RequestParameterType.Query)]
			public virtual bool? KeepRevisionForever { get; set; }

			[RequestParameter("ocrLanguage", RequestParameterType.Query)]
			public virtual string OcrLanguage { get; set; }

			[RequestParameter("removeParents", RequestParameterType.Query)]
			public virtual string RemoveParents { get; set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			[RequestParameter("useContentAsIndexableText", RequestParameterType.Query)]
			public virtual bool? UseContentAsIndexableText { get; set; }

			private Google.Apis.Drive.v3.Data.File Body { get; set; }

			public override string MethodName => "update";

			public override string HttpMethod => "PATCH";

			public override string RestPath => "files/{fileId}";

			public UpdateRequest(IClientService service, Google.Apis.Drive.v3.Data.File body, string fileId)
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
				base.RequestParameters.Add("addParents", new Parameter
				{
					Name = "addParents",
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
				base.RequestParameters.Add("keepRevisionForever", new Parameter
				{
					Name = "keepRevisionForever",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
				base.RequestParameters.Add("ocrLanguage", new Parameter
				{
					Name = "ocrLanguage",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = null,
					Pattern = null
				});
				base.RequestParameters.Add("removeParents", new Parameter
				{
					Name = "removeParents",
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
				base.RequestParameters.Add("useContentAsIndexableText", new Parameter
				{
					Name = "useContentAsIndexableText",
					IsRequired = false,
					ParameterType = "query",
					DefaultValue = "false",
					Pattern = null
				});
			}
		}

		public class UpdateMediaUpload : ResumableUpload<Google.Apis.Drive.v3.Data.File, Google.Apis.Drive.v3.Data.File>
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

			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("addParents", RequestParameterType.Query)]
			public virtual string AddParents { get; set; }

			[RequestParameter("enforceSingleParent", RequestParameterType.Query)]
			public virtual bool? EnforceSingleParent { get; set; }

			[RequestParameter("includeLabels", RequestParameterType.Query)]
			public virtual string IncludeLabels { get; set; }

			[RequestParameter("includePermissionsForView", RequestParameterType.Query)]
			public virtual string IncludePermissionsForView { get; set; }

			[RequestParameter("keepRevisionForever", RequestParameterType.Query)]
			public virtual bool? KeepRevisionForever { get; set; }

			[RequestParameter("ocrLanguage", RequestParameterType.Query)]
			public virtual string OcrLanguage { get; set; }

			[RequestParameter("removeParents", RequestParameterType.Query)]
			public virtual string RemoveParents { get; set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			[RequestParameter("useContentAsIndexableText", RequestParameterType.Query)]
			public virtual bool? UseContentAsIndexableText { get; set; }

			public UpdateMediaUpload(IClientService service, Google.Apis.Drive.v3.Data.File body, string fileId, Stream stream, string contentType)
				: base(service, string.Format("/{0}/{1}{2}", "upload", service.BasePath, "files/{fileId}"), "PATCH", stream, contentType)
			{
				FileId = fileId;
				base.Body = body;
			}
		}

		public class WatchRequest : DriveBaseServiceRequest<Channel>
		{
			[RequestParameter("fileId", RequestParameterType.Path)]
			public virtual string FileId { get; private set; }

			[RequestParameter("acknowledgeAbuse", RequestParameterType.Query)]
			public virtual bool? AcknowledgeAbuse { get; set; }

			[RequestParameter("includeLabels", RequestParameterType.Query)]
			public virtual string IncludeLabels { get; set; }

			[RequestParameter("includePermissionsForView", RequestParameterType.Query)]
			public virtual string IncludePermissionsForView { get; set; }

			[RequestParameter("supportsAllDrives", RequestParameterType.Query)]
			public virtual bool? SupportsAllDrives { get; set; }

			[RequestParameter("supportsTeamDrives", RequestParameterType.Query)]
			public virtual bool? SupportsTeamDrives { get; set; }

			private Channel Body { get; set; }

			public override string MethodName => "watch";

			public override string HttpMethod => "POST";

			public override string RestPath => "files/{fileId}/watch";

			public IMediaDownloader MediaDownloader { get; private set; }

			public WatchRequest(IClientService service, Channel body, string fileId)
				: base(service)
			{
				FileId = fileId;
				Body = body;
				MediaDownloader = new MediaDownloader(service);
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
				base.RequestParameters.Add("acknowledgeAbuse", new Parameter
				{
					Name = "acknowledgeAbuse",
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

		private const string Resource = "files";

		private readonly IClientService service;

		public FilesResource(IClientService service)
		{
			this.service = service;
		}

		public virtual CopyRequest Copy(Google.Apis.Drive.v3.Data.File body, string fileId)
		{
			return new CopyRequest(service, body, fileId);
		}

		public virtual CreateRequest Create(Google.Apis.Drive.v3.Data.File body)
		{
			return new CreateRequest(service, body);
		}

		public virtual CreateMediaUpload Create(Google.Apis.Drive.v3.Data.File body, Stream stream, string contentType)
		{
			return new CreateMediaUpload(service, body, stream, contentType);
		}

		public virtual DeleteRequest Delete(string fileId)
		{
			return new DeleteRequest(service, fileId);
		}

		public virtual EmptyTrashRequest EmptyTrash()
		{
			return new EmptyTrashRequest(service);
		}

		public virtual ExportRequest Export(string fileId, string mimeType)
		{
			return new ExportRequest(service, fileId, mimeType);
		}

		public virtual GenerateIdsRequest GenerateIds()
		{
			return new GenerateIdsRequest(service);
		}

		public virtual GetRequest Get(string fileId)
		{
			return new GetRequest(service, fileId);
		}

		public virtual ListRequest List()
		{
			return new ListRequest(service);
		}

		public virtual ListLabelsRequest ListLabels(string fileId)
		{
			return new ListLabelsRequest(service, fileId);
		}

		public virtual ModifyLabelsRequest ModifyLabels(Google.Apis.Drive.v3.Data.ModifyLabelsRequest body, string fileId)
		{
			return new ModifyLabelsRequest(service, body, fileId);
		}

		public virtual UpdateRequest Update(Google.Apis.Drive.v3.Data.File body, string fileId)
		{
			return new UpdateRequest(service, body, fileId);
		}

		public virtual UpdateMediaUpload Update(Google.Apis.Drive.v3.Data.File body, string fileId, Stream stream, string contentType)
		{
			return new UpdateMediaUpload(service, body, fileId, stream, contentType);
		}

		public virtual WatchRequest Watch(Channel body, string fileId)
		{
			return new WatchRequest(service, body, fileId);
		}
	}
}
