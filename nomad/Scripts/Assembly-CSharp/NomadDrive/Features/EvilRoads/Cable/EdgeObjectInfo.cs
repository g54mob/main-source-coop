using UnityEngine;

namespace NomadDrive.Features.EvilRoads.Cable
{
	public struct EdgeObjectInfo
	{
		public GameObject Object;

		public CableConnectionPoints Points;

		public CableConnectionConfig Config;

		public EvilRoad OwnerRoad;

		public bool IsExit;

		public bool IsLeftSide;
	}
}
