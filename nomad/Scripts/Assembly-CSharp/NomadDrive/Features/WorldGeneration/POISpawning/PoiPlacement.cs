namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	public readonly struct PoiPlacement
	{
		public readonly int Ordinal;

		public readonly int ChunkZ;

		public readonly POICategory Category;

		public readonly PoiWeightEntry Entry;

		public bool HasEntry
		{
			get
			{
				if (Entry != null)
				{
					return Entry.HasValidReference;
				}
				return false;
			}
		}

		public PoiPlacement(int ordinal, int chunkZ, POICategory category, PoiWeightEntry entry)
		{
			Ordinal = ordinal;
			ChunkZ = chunkZ;
			Category = category;
			Entry = entry;
		}
	}
}
