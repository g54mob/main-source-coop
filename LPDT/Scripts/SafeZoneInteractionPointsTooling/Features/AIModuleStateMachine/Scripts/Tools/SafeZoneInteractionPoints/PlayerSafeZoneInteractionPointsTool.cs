using Features.AIModuleStateMachine.Scripts.Core.SafeZones;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Tools.SafeZoneInteractionPoints
{
	public class PlayerSafeZoneInteractionPointsTool : MonoBehaviour
	{
		private const float CANDIDATE_POINT_SPACING = 0f;

		private const float CANDIDATE_POINT_RADIUS = 0.02f;

		private const int CANDIDATE_MAX_GROUPED_POINTS_COUNT = 32;

		[Header("Preview")]
		[SerializeField]
		private bool _realtimePreview = true;

		[SerializeField]
		private bool _candidateGroupPointsPreview;

		[Header("Shape")]
		[SerializeField]
		private PlayerSafeZoneInteractionPointPreviewShape _candidatePreviewShape;

		[SerializeField]
		private float _candidateHeightOffset;

		[Min(0.1f)]
		[SerializeField]
		private float _candidateSphereRadius = 0.5f;

		[Header("Box Shape")]
		[SerializeField]
		private float _candidateBoxRotationY;

		[SerializeField]
		private Transform _candidateBoxRotationYTransform;

		[SerializeField]
		private Vector3 _candidateBoxSize = new Vector3(1f, 1f, 1f);

		[SerializeField]
		private bool _candidateSeparateSurfaceGroups;

		[Range(0f, 1f)]
		[SerializeField]
		private float _candidateVerticalCoverage = 0.9f;

		[Header("Validation")]
		[SerializeField]
		private bool _ignoreValidation;

		[SerializeField]
		private LayerMask _candidateObstacleMask = -1;

		[SerializeField]
		private float _candidateFloorRayMaxDistance = 4f;

		[SerializeField]
		private float _candidateObstacleProbeRadius = 0.25f;

		[SerializeField]
		private bool _candidateIgnoreTriggers = true;

		[SerializeField]
		private bool _candidateIgnoreOwnSafeZoneColliders = true;

		public bool IgnoreValidation => _ignoreValidation;

		public int CandidateMaxGroupedPointsCount => 32;

		public PlayerSafeZoneInteractionPointPreview.Settings PreviewSettings => CreatePreviewSettings();

		public PlayerSafeZoneInteractionPointValidationSettings ValidationSettings => CreateValidationSettings();

		private void OnDrawGizmosSelected()
		{
			if (_realtimePreview)
			{
				PlayerSafeZone playerSafeZone = PlayerSafeZoneInteractionPointsToolResolver.FindSafeZoneForTool(this);
				if (!(playerSafeZone == null))
				{
					PlayerSafeZoneInteractionPointPreview.DrawSelected(playerSafeZone, PreviewSettings);
				}
			}
		}

		private PlayerSafeZoneInteractionPointPreview.Settings CreatePreviewSettings()
		{
			return new PlayerSafeZoneInteractionPointPreview.Settings(_candidatePreviewShape, _candidateHeightOffset, _candidateSphereRadius, GetCandidateBoxRotationY(), GetCandidateBoxSize(), 0f, 0.02f, _candidateGroupPointsPreview, _candidateSeparateSurfaceGroups, 32, _candidateVerticalCoverage, _candidateObstacleMask, _candidateFloorRayMaxDistance, _candidateObstacleProbeRadius, _candidateIgnoreTriggers, _candidateIgnoreOwnSafeZoneColliders);
		}

		private float GetCandidateBoxRotationY()
		{
			if (!(_candidateBoxRotationYTransform != null))
			{
				return _candidateBoxRotationY;
			}
			return _candidateBoxRotationYTransform.eulerAngles.y + _candidateBoxRotationY;
		}

		private Vector3 GetCandidateBoxSize()
		{
			if (_candidateBoxRotationYTransform == null)
			{
				return _candidateBoxSize;
			}
			Vector3 lossyScale = _candidateBoxRotationYTransform.lossyScale;
			lossyScale.x = Mathf.Abs(lossyScale.x);
			lossyScale.y = Mathf.Abs(lossyScale.y);
			lossyScale.z = Mathf.Abs(lossyScale.z);
			return Vector3.Scale(lossyScale, _candidateBoxSize);
		}

		private PlayerSafeZoneInteractionPointValidationSettings CreateValidationSettings()
		{
			return new PlayerSafeZoneInteractionPointValidationSettings(_candidateObstacleMask, _candidateFloorRayMaxDistance, _candidateObstacleProbeRadius, _candidateIgnoreTriggers, _candidateIgnoreOwnSafeZoneColliders);
		}
	}
}
