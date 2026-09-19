using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class FilterCriteria : IDirectResponseSchema
	{
		[JsonProperty("condition")]
		public virtual BooleanCondition Condition { get; set; }

		[JsonProperty("hiddenValues")]
		public virtual IList<string> HiddenValues { get; set; }

		[JsonProperty("visibleBackgroundColor")]
		public virtual Color VisibleBackgroundColor { get; set; }

		[JsonProperty("visibleBackgroundColorStyle")]
		public virtual ColorStyle VisibleBackgroundColorStyle { get; set; }

		[JsonProperty("visibleForegroundColor")]
		public virtual Color VisibleForegroundColor { get; set; }

		[JsonProperty("visibleForegroundColorStyle")]
		public virtual ColorStyle VisibleForegroundColorStyle { get; set; }

		public virtual string ETag { get; set; }
	}
}
