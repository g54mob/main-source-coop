using System.Collections.Generic;

namespace EvilAnalytics.Shared.Analytics
{
	public class GeographyOverview
	{
		public int TotalCountries { get; set; }

		public int TotalContinents { get; set; }

		public string? TopCountry { get; set; }

		public int TotalOnlinePlayers { get; set; }

		public List<CountryPlayerStats> Countries { get; set; } = new List<CountryPlayerStats>();

		public List<ContinentStats> Continents { get; set; } = new List<ContinentStats>();

		public List<OnlinePlayerGeo> OnlineByCountry { get; set; } = new List<OnlinePlayerGeo>();
	}
}
