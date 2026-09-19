using System;
using System.Collections.Generic;
using Google.Apis.Requests;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class Comment : IDirectResponseSchema
	{
		public class QuotedFileContentData
		{
			[JsonProperty("mimeType")]
			public virtual string MimeType { get; set; }

			[JsonProperty("value")]
			public virtual string Value { get; set; }
		}

		[JsonProperty("anchor")]
		public virtual string Anchor { get; set; }

		[JsonProperty("author")]
		public virtual User Author { get; set; }

		[JsonProperty("content")]
		public virtual string Content { get; set; }

		[JsonProperty("createdTime")]
		public virtual string CreatedTimeRaw { get; set; }

		[JsonIgnore]
		public virtual DateTime? CreatedTime
		{
			get
			{
				return Utilities.GetDateTimeFromString(CreatedTimeRaw);
			}
			set
			{
				CreatedTimeRaw = Utilities.GetStringFromDateTime(value);
			}
		}

		[JsonProperty("deleted")]
		public virtual bool? Deleted { get; set; }

		[JsonProperty("htmlContent")]
		public virtual string HtmlContent { get; set; }

		[JsonProperty("id")]
		public virtual string Id { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("modifiedTime")]
		public virtual string ModifiedTimeRaw { get; set; }

		[JsonIgnore]
		public virtual DateTime? ModifiedTime
		{
			get
			{
				return Utilities.GetDateTimeFromString(ModifiedTimeRaw);
			}
			set
			{
				ModifiedTimeRaw = Utilities.GetStringFromDateTime(value);
			}
		}

		[JsonProperty("quotedFileContent")]
		public virtual QuotedFileContentData QuotedFileContent { get; set; }

		[JsonProperty("replies")]
		public virtual IList<Reply> Replies { get; set; }

		[JsonProperty("resolved")]
		public virtual bool? Resolved { get; set; }

		public virtual string ETag { get; set; }
	}
}
