namespace Obi
{
	public interface IAerodynamicConstraintsUser
	{
		bool aerodynamicsEnabled { get; set; }

		float GetDrag(ObiAerodynamicConstraintsBatch batch, int constraintIndex);

		float GetLift(ObiAerodynamicConstraintsBatch batch, int constraintIndex);
	}
}
