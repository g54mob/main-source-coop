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
	public class TextureChangerByPlayerRef : NetworkBehaviour
	{
		private static readonly int _baseMapProperty = Shader.PropertyToID("_BaseMap");

		private static readonly int _normalMapProperty = Shader.PropertyToID("_NormalMap");

		private static readonly int _maskProperty = Shader.PropertyToID("_Mask");

		private static readonly int _colorProperty = Shader.PropertyToID("_Color");

		private static readonly int _metallicProperty = Shader.PropertyToID("_Metallic");

		[SerializeField]
		private List<Renderer> _maskedRenderers;

		private PlayerCustomizationModel _playerCustomizationModel;

		private ButtTexturePresetConfiguration _buttTexturePresetConfiguration;

		[Inject]
		public void InjectDependencies(PlayerCustomizationModel playerCustomizationModel, ButtTexturePresetConfiguration buttTexturePresetConfiguration)
		{
			_playerCustomizationModel = playerCustomizationModel;
			_buttTexturePresetConfiguration = buttTexturePresetConfiguration;
		}

		public override void Spawned()
		{
			base.Spawned();
			AdjustColorByCustomization();
			_playerCustomizationModel.OnSlotsChanged += AdjustColorByCustomization;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_playerCustomizationModel.OnSlotsChanged -= AdjustColorByCustomization;
		}

		private void AdjustColorByCustomization()
		{
			PlayerCustomizationSlotData playerCustomizationSlotData = _playerCustomizationModel.Slots.FirstOrDefault((PlayerCustomizationSlotData x) => x.PlayerId == base.Object.InputAuthority.PlayerId);
			if (playerCustomizationSlotData != null)
			{
				ApplyButtTexturePreset(playerCustomizationSlotData.ButtTexturePreset);
				AssignShaderColor(_maskedRenderers, playerCustomizationSlotData.VariableColor, _colorProperty);
			}
		}

		private void ApplyButtTexturePreset(ButtTexturePreset customizationDataButtTexturePreset)
		{
			if (customizationDataButtTexturePreset != ButtTexturePreset.None)
			{
				ButtTexturePresetData buttTexturePresetData = _buttTexturePresetConfiguration.TexturePresets[customizationDataButtTexturePreset];
				AssignShaderTexture(_maskedRenderers, buttTexturePresetData.BaseTexture, _baseMapProperty);
				AssignShaderTexture(_maskedRenderers, buttTexturePresetData.NormalMap, _normalMapProperty);
				AssignShaderTexture(_maskedRenderers, buttTexturePresetData.Mask, _maskProperty);
				AssignShaderFloat(_maskedRenderers, buttTexturePresetData.Metallic, _metallicProperty);
			}
		}

		private void AssignShaderFloat(List<Renderer> deadPartRenderers, float value, int shaderFloatProperty)
		{
			foreach (Renderer deadPartRenderer in deadPartRenderers)
			{
				Material material = new Material(deadPartRenderer.material);
				material.SetFloat(shaderFloatProperty, value);
				deadPartRenderer.material = material;
			}
		}

		private void AssignShaderTexture(List<Renderer> deadPartRenderers, Texture texture, int shaderTextureProperty)
		{
			foreach (Renderer deadPartRenderer in deadPartRenderers)
			{
				Material material = new Material(deadPartRenderer.material);
				material.SetTexture(shaderTextureProperty, texture);
				deadPartRenderer.material = material;
			}
		}

		private void AssignShaderColor(List<Renderer> playerRenderers, Color color, int shaderColorProperty)
		{
			foreach (Renderer playerRenderer in playerRenderers)
			{
				Material material = new Material(playerRenderer.material);
				material.SetColor(shaderColorProperty, color);
				playerRenderer.material = material;
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
