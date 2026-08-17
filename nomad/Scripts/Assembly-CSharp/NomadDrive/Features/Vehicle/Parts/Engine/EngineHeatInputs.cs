namespace NomadDrive.Features.Vehicle.Parts.Engine
{
	public readonly struct EngineHeatInputs
	{
		public readonly bool EngineRunning;

		public readonly float LoadFactor;

		public readonly bool HasRadiator;

		public readonly float CoolantRatio;

		public static EngineHeatInputs Off => new EngineHeatInputs(engineRunning: false, 0f, hasRadiator: false, 0f);

		public EngineHeatInputs(bool engineRunning, float loadFactor, bool hasRadiator, float coolantRatio)
		{
			EngineRunning = engineRunning;
			LoadFactor = loadFactor;
			HasRadiator = hasRadiator;
			CoolantRatio = coolantRatio;
		}
	}
}
