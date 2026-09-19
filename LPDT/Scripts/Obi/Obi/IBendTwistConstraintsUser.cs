using UnityEngine;

namespace Obi
{
	public interface IBendTwistConstraintsUser
	{
		bool bendTwistConstraintsEnabled { get; set; }

		Vector3 GetBendTwistCompliance(ObiBendTwistConstraintsBatch batch, int constraintIndex);

		Vector2 GetBendTwistPlasticity(ObiBendTwistConstraintsBatch batch, int constraintIndex);
	}
}
