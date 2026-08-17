using UnityEngine;

namespace NomadDrive.Features.ColliderLOD
{
	public interface IColliderLodTarget
	{
		Transform transform { get; }

		bool ParticipatesInColliderLod { get; }

		bool LodPositionMayChange { get; }

		Collider[] GetRigidColliders();

		void SetLodColliderActive(bool active);
	}
}
