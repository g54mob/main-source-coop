using UnityEngine;

namespace NWH.Common.CoM
{
	public interface IMassAffector
	{
		float GetMass();

		Vector3 GetWorldCenterOfMass();

		Transform GetTransform();
	}
}
