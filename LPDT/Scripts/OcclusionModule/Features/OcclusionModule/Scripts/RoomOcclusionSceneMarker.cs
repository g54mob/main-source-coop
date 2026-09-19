using UnityEngine;

namespace Features.OcclusionModule.Scripts
{
	public class RoomOcclusionSceneMarker : MonoBehaviour
	{
		[Header("Mode")]
		[Tooltip("Disabled = kill switch, everything rendered (use to confirm occlusion causes a disappearing-object bug). Portal = only rooms seen through open doorways. Graph = door graph + distance gate, view-independent.")]
		[SerializeField]
		private OcclusionCullingMode _cullingMode = OcclusionCullingMode.Portal;

		[Header("Common (both modes)")]
		[Tooltip("Max player→room distance still rendered. 0 = no cap.")]
		[SerializeField]
		private float _maxRenderDistance = 100f;

		[Tooltip("Peek range: picks the current room at a doorway, and in Graph mode lets a shut door count as an edge.")]
		[SerializeField]
		private float _peekDistance = 3f;

		[Tooltip("Delay before hiding a room that left view (anti-flicker). Show instant, hide delayed.")]
		[SerializeField]
		private float _visibilityHideDelay = 0.4f;

		[Tooltip("How many rooms deep the visible set expands from the current room.")]
		[SerializeField]
		private int _maxDepth = 3;

		[Tooltip("Swing needed to count a door open: 0 = closed, 1 = fully open (0.3 ≈ 17°).")]
		[SerializeField]
		private float _doorOpenThreshold = 0.15f;

		[Header("Portal mode only")]
		[Tooltip("Max distance a closed door still renders the room behind it (see through the gap). A chain may cross only one closed door: behind a second one nothing loads. Should be <= Max Render Distance. 0 = closed doors never render the room behind.")]
		[SerializeField]
		private float _closedDoorRenderDistance = 100f;

		[Tooltip("Layers blocking line-of-sight through doorways. Default Wall+Ground.")]
		[SerializeField]
		private LayerMask _occlusionBlockerMask;

		private void Reset()
		{
			_occlusionBlockerMask = LayerMask.GetMask("Wall", "Ground");
		}

		private void OnEnable()
		{
			OcclusionRegistry.SetMarker(this);
		}

		private void OnDisable()
		{
			OcclusionRegistry.ClearMarker(this);
		}

		public void ApplyTo(OcclusionSettings occlusionSettings)
		{
			occlusionSettings.CullingMode = _cullingMode;
			occlusionSettings.PeekDistance = _peekDistance;
			occlusionSettings.ClosedDoorRenderDistance = _closedDoorRenderDistance;
			occlusionSettings.VisibilityHideDelay = _visibilityHideDelay;
			occlusionSettings.MaxDepth = _maxDepth;
			occlusionSettings.DoorOpenThreshold = _doorOpenThreshold;
			occlusionSettings.OcclusionBlockerMask = _occlusionBlockerMask;
			occlusionSettings.MaxRenderDistance = _maxRenderDistance;
		}
	}
}
