using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class MetricsModel
	{
		[DataMember(Name = "labels", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "labels")]
		public List<string> Labels { get; set; }

		[DataMember(Name = "datasets", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "datasets")]
		public List<decimal?> Datasets { get; set; }

		[DataMember(Name = "timestamps", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "timestamps")]
		public List<DateTime?> Timestamps { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class MetricsModel {\n");
			stringBuilder.Append("  Labels: ").Append(Labels).Append("\n");
			stringBuilder.Append("  Datasets: ").Append(Datasets).Append("\n");
			stringBuilder.Append("  Timestamps: ").Append(Timestamps).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
