using Fusion;
using UnityEngine;

namespace Features.PhysicsUtilsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PhysicsResolutionController : NetworkBehaviour
	{
		[SerializeField]
		private Rigidbody _rb;

		[SerializeField]
		private PhysicsResolutionConfiguration _physicsResolutionConfiguration;

		[SerializeField]
		private PhysicsResolutionTier _initialPhysicsResolutionTier = PhysicsResolutionTier.Default;

		[SerializeField]
		private PhysicsResolutionTier _resolutionTierInContainer = PhysicsResolutionTier.Low;

		private void Start()
		{
			SetPhysicsResolution(_initialPhysicsResolutionTier);
		}

		public void SetPhysicsResolution(PhysicsResolutionTier tier)
		{
			if (_physicsResolutionConfiguration.PhysicsResolutionSettings.TryGetValue(tier, out var value))
			{
				_rb.solverIterations = value.SolverIterations;
				_rb.solverVelocityIterations = value.SolverVelocityIterations;
				_rb.collisionDetectionMode = value.CollisionDetectionMode;
				_rb.maxDepenetrationVelocity = value.MaxDepenetrationVelocity;
			}
		}

		public void SetPhysicsResolutionInContainer()
		{
			SetPhysicsResolution(_resolutionTierInContainer);
		}

		public void RestoreDefaultResolution()
		{
			SetPhysicsResolution(_initialPhysicsResolutionTier);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
