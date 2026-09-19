using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	public class PlayerGrabPointOffsetFollower : GrabPointOffsetFollowerBase
	{
		[SerializeField]
		private PlayerGrabHolder _holder;

		protected override bool IsGrabbing => _holder.IsGrabbing;

		protected override Transform GrabbedTransform => _holder.GrabbedTransform;

		protected override Transform HolderTransform => _holder.transform;
	}
}
