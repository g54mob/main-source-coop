using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class FindReplaceRequest : IDirectResponseSchema
	{
		[JsonProperty("allSheets")]
		public virtual bool? AllSheets { get; set; }

		[JsonProperty("find")]
		public virtual string Find { get; set; }

		[JsonProperty("includeFormulas")]
		public virtual bool? IncludeFormulas { get; set; }

		[JsonProperty("matchCase")]
		public virtual bool? MatchCase { get; set; }

		[JsonProperty("matchEntireCell")]
		public virtual bool? MatchEntireCell { get; set; }

		[JsonProperty("range")]
		public virtual GridRange Range { get; set; }

		[JsonProperty("replacement")]
		public virtual string Replacement { get; set; }

		[JsonProperty("searchByRegex")]
		public virtual bool? SearchByRegex { get; set; }

		[JsonProperty("sheetId")]
		public virtual int? SheetId { get; set; }

		public virtual string ETag { get; set; }
	}
}
