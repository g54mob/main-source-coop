using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class OrgChartSpec : IDirectResponseSchema
	{
		[JsonProperty("labels")]
		public virtual ChartData Labels { get; set; }

		[JsonProperty("nodeColor")]
		public virtual Color NodeColor { get; set; }

		[JsonProperty("nodeColorStyle")]
		public virtual ColorStyle NodeColorStyle { get; set; }

		[JsonProperty("nodeSize")]
		public virtual string NodeSize { get; set; }

		[JsonProperty("parentLabels")]
		public virtual ChartData ParentLabels { get; set; }

		[JsonProperty("selectedNodeColor")]
		public virtual Color SelectedNodeColor { get; set; }

		[JsonProperty("selectedNodeColorStyle")]
		public virtual ColorStyle SelectedNodeColorStyle { get; set; }

		[JsonProperty("tooltips")]
		public virtual ChartData Tooltips { get; set; }

		public virtual string ETag { get; set; }
	}
}
