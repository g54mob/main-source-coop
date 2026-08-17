using UnityEngine;

namespace NomadDrive.Features.FloatingOrigin
{
	public interface IFloatingOriginShiftable
	{
		void OnOriginShift(Vector3 delta);
	}
}
