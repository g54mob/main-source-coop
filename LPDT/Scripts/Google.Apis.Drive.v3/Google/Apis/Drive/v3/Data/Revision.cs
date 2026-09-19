using System;
using System.Collections.Generic;
using Google.Apis.Requests;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class Revision : IDirectResponseSchema
	{
		[JsonProperty("exportLinks")]
		public virtual IDictionary<string, string> ExportLinks { get; set; }

		[JsonProperty("id")]
		public virtual string Id { get; set; }

		[JsonProperty("keepForever")]
		public virtual bool? KeepForever { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("lastModifyingUser")]
		public virtual User LastModifyingUser { get; set; }

		[JsonProperty("md5Checksum")]
		public virtual string Md5Checksum { get; set; }

		[JsonProperty("mimeType")]
		public virtual string MimeType { get; set; }

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

		[JsonProperty("originalFilename")]
		public virtual string OriginalFilename { get; set; }

		[JsonProperty("publishAuto")]
		public virtual bool? PublishAuto { get; set; }

		[JsonProperty("published")]
		public virtual bool? Published { get; set; }

		[JsonProperty("publishedLink")]
		public virtual string PublishedLink { get; set; }

		[JsonProperty("publishedOutsideDomain")]
		public virtual bool? PublishedOutsideDomain { get; set; }

		[JsonProperty("size")]
		public virtual long? Size { get; set; }

		public virtual string ETag { get; set; }
	}
}
