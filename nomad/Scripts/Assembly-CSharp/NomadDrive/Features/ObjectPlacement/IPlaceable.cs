using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	public interface IPlaceable
	{
		IObjectPlacementManager PlacementManager { get; set; }

		PlacementNetworkData PlacementNetworkData { get; }

		float SnappingAreaDistance { get; set; }

		Vector3 DefaultPlacingRotation { get; set; }

		RotationAxis PlacementRotationAxis { get; set; }

		Vector3 PositionOffsetForPlacement { get; set; }

		Vector3 BoundsCenterLocalOffset { get; }

		float PlacementShaderScale { get; set; }

		GhostPreviewMode GhostPreviewMode { get; set; }

		Vector3 GhostPreviewScale { get; set; }

		float DefaultPlacementDistance { get; set; }

		float MinPlacementDistance { get; set; }

		float MaxPlacementDistance { get; set; }

		bool CanPlaceable { get; set; }

		void SetPlacementNetworkData(uint parentNetworkID, byte snappingPlaneIndex);

		void OnPlacementModeEnter();

		void OnPlacementModeExit();

		void StartPlacing();

		void OnPlacedActions();

		void OnRemovedActions();

		void OnFallenActions();

		void Release();

		bool IsCollidingAny();

		void ClearColliders();
	}
}
