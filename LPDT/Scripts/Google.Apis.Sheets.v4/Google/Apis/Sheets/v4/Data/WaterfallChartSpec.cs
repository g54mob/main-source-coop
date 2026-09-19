using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class WaterfallChartSpec : IDirectResponseSchema
	{
		[JsonProperty("connectorLineStyle")]
		public virtual LineStyle ConnectorLineStyle { get; set; }

		[JsonProperty("domain")]
		public virtual WaterfallChartDomain Domain { get; set; }

		[JsonProperty("firstValueIsTotal")]
		public virtual bool? FirstValueIsTotal { get; set; }

		[JsonProperty("hideConnectorLines")]
		public virtual bool? HideConnectorLines { get; set; }

		[JsonProperty("series")]
		public virtual IList<WaterfallChartSeries> Series { get; set; }

		[JsonProperty("stackedType")]
		public virtual string StackedType { get; set; }

		[JsonProperty("totalDataLabel")]
		public virtual DataLabel TotalDataLabel { get; set; }

		public virtual string ETag { get; set; }
	}
}
