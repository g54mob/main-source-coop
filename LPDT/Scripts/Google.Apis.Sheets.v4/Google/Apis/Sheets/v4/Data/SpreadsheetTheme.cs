using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class SpreadsheetTheme : IDirectResponseSchema
	{
		[JsonProperty("primaryFontFamily")]
		public virtual string PrimaryFontFamily { get; set; }

		[JsonProperty("themeColors")]
		public virtual IList<ThemeColorPair> ThemeColors { get; set; }

		public virtual string ETag { get; set; }
	}
}
