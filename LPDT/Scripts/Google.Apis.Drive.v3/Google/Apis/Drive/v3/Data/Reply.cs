using System;
using Google.Apis.Requests;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class Reply : IDirectResponseSchema
	{
		[JsonProperty("action")]
		public virtual string Action { get; set; }

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

		public virtual string ETag { get; set; }
	}
}
