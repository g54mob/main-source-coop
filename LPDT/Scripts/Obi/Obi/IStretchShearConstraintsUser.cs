using UnityEngine;

namespace Obi
{
	public interface IStretchShearConstraintsUser
	{
		bool stretchShearConstraintsEnabled { get; set; }

		Vector3 GetStretchShearCompliance(ObiStretchShearConstraintsBatch batch, int constraintIndex);
	}
}
