using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class Editors : IDirectResponseSchema
	{
		[JsonProperty("domainUsersCanEdit")]
		public virtual bool? DomainUsersCanEdit { get; set; }

		[JsonProperty("groups")]
		public virtual IList<string> Groups { get; set; }

		[JsonProperty("users")]
		public virtual IList<string> Users { get; set; }

		public virtual string ETag { get; set; }
	}
}
