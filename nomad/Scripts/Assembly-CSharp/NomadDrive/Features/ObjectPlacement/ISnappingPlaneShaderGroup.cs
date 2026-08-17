using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	public interface ISnappingPlaneShaderGroup
	{
		Vector3 SharedOrigin { get; }

		void OnMemberShaderActivated(SnappingPlane source, Vector3 centerPos, float radius);

		void OnMemberShaderUpdated(SnappingPlane source, Vector3 centerPos);

		void OnMemberShaderDeactivated(SnappingPlane source);
	}
}
