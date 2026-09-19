namespace Features.FogModule.Scripts
{
	public readonly struct FogBlend
	{
		public readonly FogLocationType StartLocation;

		public readonly FogLocationType EndLocation;

		public readonly float Ratio;

		public bool IsValid
		{
			get
			{
				if (StartLocation != FogLocationType.None)
				{
					return EndLocation != FogLocationType.None;
				}
				return false;
			}
		}

		public FogBlend(FogLocationType startLocation, FogLocationType endLocation, float ratio)
		{
			StartLocation = startLocation;
			EndLocation = endLocation;
			Ratio = ratio;
		}
	}
}
