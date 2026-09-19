using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.FogModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class FogRegionRegistrar : NetworkBehaviour
	{
		[Tooltip("Fixed declares 'this space IS one fog location' (a beach or a location). Blend declares 'this space transitions between two' (an aisle), along each box collider's local Z axis.")]
		[SerializeField]
		private FogRegionType _fogRegionType = FogRegionType.Fixed;

		[Tooltip("What this region IS. Fixed pins this fog location. Blend starts from it at ratio 0.")]
		[SerializeField]
		private FogLocationType _fogLocation;

		[Tooltip("Blend only (ignored by Fixed): the fog location this region blends TO, reached at ratio 1.")]
		[SerializeField]
		private FogLocationType _blendTargetFogLocation;

		[Tooltip("Where regions overlap, the highest priority wins. Give the aisle a higher priority than the locations it connects.")]
		[SerializeField]
		private int _priority;

		[Tooltip("Box colliders making up this region. Tile as many as the shape needs. For Blend regions, each collider's local +Z must point towards the end location, and the ratio ranges should chain (0-0.5, 0.5-1, ...) along the aisle.")]
		[SerializeField]
		private List<FogRegionBox> _fogRegionBoxes = new List<FogRegionBox>();

		private FogRegionsModel _fogRegionsModel;

		private FogRegion _fogRegion;

		[Inject]
		public void InjectDependencies(FogRegionsModel fogRegionsModel)
		{
			_fogRegionsModel = fogRegionsModel;
		}

		public override void Spawned()
		{
			List<FogRegionBox> list = new List<FogRegionBox>(_fogRegionBoxes.Count);
			for (int i = 0; i < _fogRegionBoxes.Count; i++)
			{
				FogRegionBox fogRegionBox = _fogRegionBoxes[i];
				if (fogRegionBox.BoxCollider == null)
				{
					Debug.LogError(string.Format("{0} on {1}: box #{2} has no box collider assigned and is ignored.", "FogRegionRegistrar", base.name, i + 1), this);
					continue;
				}
				fogRegionBox.DisablePhysics();
				list.Add(fogRegionBox);
			}
			_fogRegion = new FogRegion(_fogRegionType, _fogLocation, _blendTargetFogLocation, _priority, list);
			_fogRegionsModel.AddFogRegion(_fogRegion);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_fogRegionsModel.RemoveFogRegion(_fogRegion);
		}

		private void OnDrawGizmosSelected()
		{
			for (int i = 0; i < _fogRegionBoxes.Count; i++)
			{
				DrawFogRegionBoxGizmo(_fogRegionBoxes[i]);
			}
		}

		private void DrawFogRegionBoxGizmo(FogRegionBox fogRegionBox)
		{
			BoxCollider boxCollider = fogRegionBox.BoxCollider;
			if (!(boxCollider == null))
			{
				Gizmos.matrix = boxCollider.transform.localToWorldMatrix;
				Gizmos.color = ((_fogRegionType == FogRegionType.Blend) ? Color.cyan : Color.yellow);
				Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
				if (_fogRegionType == FogRegionType.Blend)
				{
					float num = boxCollider.size.z * 0.5f;
					Gizmos.DrawLine(boxCollider.center + new Vector3(0f, 0f, 0f - num), boxCollider.center + new Vector3(0f, 0f, num));
					Gizmos.DrawWireSphere(boxCollider.center + new Vector3(0f, 0f, num), boxCollider.size.x * 0.05f);
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
