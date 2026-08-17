using EvilCore.Networking;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.ObjectPlacement
{
	public interface IObjectPlacementManager : IUniqueNetworkComponent
	{
		UnityEvent<GameObject> OnPlacementEnter { get; set; }

		UnityEvent OnPlacementExit { get; set; }

		float DefaultObjectDistanceToCamera { get; set; }

		float SnappingAreaDetectionRayLength { get; set; }

		float PlacementShaderScale { get; set; }

		SnappingPlane CurrentSnappingPlane { get; set; }

		void Init();

		void Execute(GameObject gameObject);

		void Release();

		void Reset();
	}
}
