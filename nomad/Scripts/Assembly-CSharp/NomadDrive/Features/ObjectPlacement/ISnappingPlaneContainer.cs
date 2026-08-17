namespace NomadDrive.Features.ObjectPlacement
{
	public interface ISnappingPlaneContainer
	{
		SnappingPlane[] SnappingPlanes { get; }

		bool IsAnySnappingPlaneOneShotSlotFull();

		bool IsSnappingPlaneOccupied(int snappingPlaneID);

		bool IsSnappingPlaneOccupied(SnappingPlane snappingPlane);

		SnappingPlane GetSnappingPlaneByIndex(ushort index);

		ushort GetSnappingPlaneIDBySnappingPlaneReference(SnappingPlane snappingPlane);

		void EnableAllSnappingPlanes();

		void DisableAllSnappingPlanes();

		void EnableSnappingPlane(int snappingPlaneID);

		void DisableSnappingPlane(int snappingPlaneID);
	}
}
