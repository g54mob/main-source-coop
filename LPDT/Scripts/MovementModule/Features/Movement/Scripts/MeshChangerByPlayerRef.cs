using System.Collections.Generic;
using System.Linq;
using Features.DeadPartsModule.Data;
using Fusion;
using PlayerCustomization;
using UnityEngine;
using Zenject;

namespace Features.Movement.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class MeshChangerByPlayerRef : NetworkBehaviour
	{
		[SerializeField]
		private List<SkinnedMeshRenderer> _renderers;

		private PlayerCustomizationModel _playerCustomizationModel;

		private ButtMeshPresetConfiguration _buttMeshPresetConfiguration;

		[Inject]
		public void InjectDependencies(PlayerCustomizationModel playerCustomizationModel, ButtMeshPresetConfiguration buttMeshPresetConfiguration)
		{
			_playerCustomizationModel = playerCustomizationModel;
			_buttMeshPresetConfiguration = buttMeshPresetConfiguration;
		}

		public override void Spawned()
		{
			base.Spawned();
			AdjustMeshByCustomization();
			_playerCustomizationModel.OnSlotsChanged += AdjustMeshByCustomization;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_playerCustomizationModel.OnSlotsChanged -= AdjustMeshByCustomization;
		}

		private void AdjustMeshByCustomization()
		{
			PlayerCustomizationSlotData playerCustomizationSlotData = _playerCustomizationModel.Slots.FirstOrDefault((PlayerCustomizationSlotData slotData) => slotData.PlayerId == base.Object.InputAuthority.PlayerId);
			if (playerCustomizationSlotData != null)
			{
				ApplyButtPreset(playerCustomizationSlotData.ButtMeshPreset);
			}
		}

		private void ApplyButtPreset(ButtMeshPreset currentPreset)
		{
			if (currentPreset != ButtMeshPreset.None)
			{
				ButtMeshPresetData buttMeshPresetData = _buttMeshPresetConfiguration.MeshPresets[currentPreset];
				AssignMesh(_renderers, buttMeshPresetData.Mesh);
			}
		}

		private void AssignMesh(List<SkinnedMeshRenderer> deadPartRenderers, Mesh mesh)
		{
			foreach (SkinnedMeshRenderer deadPartRenderer in deadPartRenderers)
			{
				deadPartRenderer.sharedMesh = UnityEngine.Object.Instantiate(mesh);
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
