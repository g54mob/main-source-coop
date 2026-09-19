using Features.GrabModule.Scripts.PhysGrab;
using Fusion;
using UnityEngine;

namespace Features.LineArmModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class LineArmController : LineArmControllerBase
	{
		[SerializeField]
		private PhysGrabber _physGrabber;

		public override PhysGrabber PhysGrabber => _physGrabber;

		public override LineArmType ArmType => LineArmType.RightArmDefault;

		protected override Ray GetRayByRaycastType()
		{
			if (_cameraModel.CameraObject == null)
			{
				return default(Ray);
			}
			return new Ray(_cameraModel.AimTransform.position, _cameraModel.AimTransform.forward);
		}

		public override void Spawned()
		{
			base.Spawned();
			ProcessStatsInitialization();
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
