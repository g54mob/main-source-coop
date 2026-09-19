using UnityEngine;

namespace Obi
{
	public interface ISkinConstraintsUser
	{
		bool skinConstraintsEnabled { get; set; }

		Vector3 GetSkinRadiiBackstop(ObiSkinConstraintsBatch batch, int constraintIndex);

		float GetSkinCompliance(ObiSkinConstraintsBatch batch, int constraintIndex);
	}
}
