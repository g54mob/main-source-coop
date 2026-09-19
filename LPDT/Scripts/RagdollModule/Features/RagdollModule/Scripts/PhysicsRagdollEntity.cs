using Fusion;
using UnityEngine;

namespace Features.RagdollModule.Scripts
{
	[NetworkBehaviourWeaved(2)]
	public class PhysicsRagdollEntity : RagdollEntity
	{
		[SerializeField]
		protected Collider _entityCollider;

		[SerializeField]
		private Rigidbody _entityRigidbody;

		protected override void SnapEntityToRagdoll()
		{
			base.SnapEntityToRagdoll();
			Physics.SyncTransforms();
		}

		protected override void EnableRagdoll()
		{
			base.EnableRagdoll();
			_entityCollider.enabled = false;
			TransferVelocityToRagdoll();
			_entityRigidbody.isKinematic = true;
		}

		protected override void OnRagdollBlendOutFinished()
		{
			base.OnRagdollBlendOutFinished();
			_entityCollider.enabled = true;
			_entityRigidbody.isKinematic = false;
		}

		protected override void DisableRagdoll()
		{
			base.DisableRagdoll();
			if (!base.IsSimulated)
			{
				_entityCollider.enabled = true;
				_entityRigidbody.isKinematic = false;
			}
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			DisableRagdollPlayerCollisions();
		}

		private void TransferVelocityToRagdoll()
		{
			Rigidbody rigidBody = base.RootPhysData.RigidBody;
			rigidBody.inertiaTensor = rigidBody.inertiaTensor;
			rigidBody.inertiaTensorRotation = rigidBody.inertiaTensorRotation;
			rigidBody.linearVelocity = _entityRigidbody.linearVelocity;
			rigidBody.angularVelocity = _entityRigidbody.angularVelocity;
		}

		private void DisableRagdollPlayerCollisions()
		{
			Collider[] ragdollColliders = base.RagdollColliders;
			foreach (Collider collider in ragdollColliders)
			{
				Physics.IgnoreCollision(_entityCollider, collider, ignore: true);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
