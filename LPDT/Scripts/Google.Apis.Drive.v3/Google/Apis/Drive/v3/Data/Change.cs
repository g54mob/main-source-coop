using System;
using Google.Apis.Requests;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class Change : IDirectResponseSchema
	{
		[JsonProperty("changeType")]
		public virtual string ChangeType { get; set; }

		[JsonProperty("drive")]
		public virtual Drive Drive { get; set; }

		[JsonProperty("driveId")]
		public virtual string DriveId { get; set; }

		[JsonProperty("file")]
		public virtual File File { get; set; }

		[JsonProperty("fileId")]
		public virtual string FileId { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("removed")]
		public virtual bool? Removed { get; set; }

		[JsonProperty("teamDrive")]
		public virtual TeamDrive TeamDrive { get; set; }

		[JsonProperty("teamDriveId")]
		public virtual string TeamDriveId { get; set; }

		[JsonProperty("time")]
		public virtual string TimeRaw { get; set; }

		[JsonIgnore]
		public virtual DateTime? Time
		{
			get
			{
				return Utilities.GetDateTimeFromString(TimeRaw);
			}
			set
			{
				TimeRaw = Utilities.GetStringFromDateTime(value);
			}
		}

		[JsonProperty("type")]
		public virtual string Type { get; set; }

		public virtual string ETag { get; set; }
	}
}
