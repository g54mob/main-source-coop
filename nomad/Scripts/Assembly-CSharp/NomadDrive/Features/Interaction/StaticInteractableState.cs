namespace NomadDrive.Features.Interaction
{
	public struct StaticInteractableState
	{
		public int id;

		public byte stateData;

		public float timestamp;

		public StaticInteractableState(int id, byte stateData, float timestamp)
		{
			this.id = id;
			this.stateData = stateData;
			this.timestamp = timestamp;
		}
	}
}
