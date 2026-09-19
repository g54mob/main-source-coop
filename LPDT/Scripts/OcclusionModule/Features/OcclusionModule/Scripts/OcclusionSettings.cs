namespace Features.OcclusionModule.Scripts
{
	public class OcclusionSettings
	{
		public OcclusionCullingMode CullingMode = OcclusionCullingMode.Portal;

		public float MaxRenderDistance = 100f;

		public int MaxDepth = 3;

		public float PeekDistance = 3f;

		public float ClosedDoorRenderDistance = 100f;

		public float DoorOpenThreshold = 0.15f;

		public int OcclusionBlockerMask;

		public float VisibilityHideDelay = 0.4f;
	}
}
