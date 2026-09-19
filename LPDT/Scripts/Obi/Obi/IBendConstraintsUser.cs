namespace Obi
{
	public interface IBendConstraintsUser
	{
		bool bendConstraintsEnabled { get; set; }

		float bendCompliance { get; set; }

		float maxBending { get; set; }

		float plasticYield { get; set; }

		float plasticCreep { get; set; }
	}
}
