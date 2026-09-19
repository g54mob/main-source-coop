using System.Collections.Generic;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	[NetworkBehaviourWeaved(0)]
	public class PhysGrabber : NetworkBehaviour
	{
		private INetworkTRSPTeleport _cachedNetworkTransform;

		public bool UseOwnPositionAsGrabPoint;

		public float forceMax;

		public Transform physGrabPointPullerPosition;

		public Dictionary<GrabObjectBase, Transform> physGrabPoints = new Dictionary<GrabObjectBase, Transform>();

		public float springConstant;

		public float dampingConstant;

		public float grabStrength;

		public Vector3 currentGrabForce;

		public float forceConstant;

		public PlayerLookDetection PlayerLookDetection;

		[Header("Force axis mask (world space)")]
		[Tooltip("Zero the grab force on that world axis. E.g. IgnoreWorldY = pull only along the surface.")]
		public bool IgnoreWorldX;

		public bool IgnoreWorldY;

		public bool IgnoreWorldZ;

		public bool IsProcessPhysGrabbing { get; set; }

		public Transform GrabberTransform => base.transform;

		public int PlayerId => base.Object.StateAuthority.PlayerId;

		public INetworkTRSPTeleport AttachedNetworkTransform => _cachedNetworkTransform;

		public override void Spawned()
		{
			base.Spawned();
			_cachedNetworkTransform = GetComponent<INetworkTRSPTeleport>();
		}

		public Vector3 ApplyForceAxisMask(Vector3 force)
		{
			if (IgnoreWorldX)
			{
				force.x = 0f;
			}
			if (IgnoreWorldY)
			{
				force.y = 0f;
			}
			if (IgnoreWorldZ)
			{
				force.z = 0f;
			}
			return force;
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
