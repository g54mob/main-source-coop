using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	public class GrabPointOffsetFollower : GrabPointOffsetFollowerBase
	{
		[SerializeField]
		private EnemyItemHolder _holder;

		protected override bool IsGrabbing => _holder.IsGrabbing;

		protected override Transform GrabbedTransform => _holder.GrabbedTransform;

		protected override Transform HolderTransform => _holder.transform;
	}
}
