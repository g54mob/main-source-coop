using System;
using Google.Apis.Requests;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class ContentRestriction : IDirectResponseSchema
	{
		[JsonProperty("readOnly")]
		public virtual bool? ReadOnly__ { get; set; }

		[JsonProperty("reason")]
		public virtual string Reason { get; set; }

		[JsonProperty("restrictingUser")]
		public virtual User RestrictingUser { get; set; }

		[JsonProperty("restrictionTime")]
		public virtual string RestrictionTimeRaw { get; set; }

		[JsonIgnore]
		public virtual DateTime? RestrictionTime
		{
			get
			{
				return Utilities.GetDateTimeFromString(RestrictionTimeRaw);
			}
			set
			{
				RestrictionTimeRaw = Utilities.GetStringFromDateTime(value);
			}
		}

		[JsonProperty("type")]
		public virtual string Type { get; set; }

		public virtual string ETag { get; set; }
	}
}
