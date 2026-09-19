namespace Features.BeachInteractableCommonModule.Scripts
{
	public class RecordedBeachInteractableData
	{
		public BeachInteractableType RecordedBeachInteractableType { get; set; }

		public int SpawnPointMarker { get; set; }

		public RecordedBeachInteractableData(BeachInteractableType recordedBeachInteractableType, int spawnPointMarker)
		{
			RecordedBeachInteractableType = recordedBeachInteractableType;
			SpawnPointMarker = spawnPointMarker;
		}
	}
}
