using System.Collections.Generic;
using Features.DeadPartsModule.Data;
using Features.PlayerSkinModule.Scripts.Features.PlayerSkinModule.Scripts.VisibilityHandling;
using Features.SkinConfiguration.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.DeadPartsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class BaseDeadPartCustomizer : NetworkBehaviour
	{
		private static readonly int _baseMapProperty = Shader.PropertyToID("_BaseMap");

		private static readonly int _normalMapProperty = Shader.PropertyToID("_NormalMap");

		private static readonly int _maskProperty = Shader.PropertyToID("_Mask");

		private static readonly int _metallicProperty = Shader.PropertyToID("_Metallic");

		private static readonly int _colorProperty = Shader.PropertyToID("_Color");

		[SerializeField]
		private List<SkinnedMeshRenderer> _meshRenderers;

		[SerializeField]
		private List<Renderer> _maskedRenderers;

		[SerializeField]
		private Transform _skinParent;

		[SerializeField]
		private Transform _characterRootBone;

		[SerializeField]
		private SkinnedMeshRenderer _characterSkinnedMesh;

		private SkinsConfiguration _skinsConfiguration;

		private ButtTexturePresetConfiguration _buttTexturePresetConfiguration;

		private ButtMeshPresetConfiguration _buttMeshPresetConfiguration;

		[Inject]
		public void InjectDependencies(SkinsConfiguration skinConfiguration, ButtTexturePresetConfiguration buttTexturePresetConfiguration, ButtMeshPresetConfiguration buttMeshPresetConfiguration)
		{
			_skinsConfiguration = skinConfiguration;
			_buttTexturePresetConfiguration = buttTexturePresetConfiguration;
			_buttMeshPresetConfiguration = buttMeshPresetConfiguration;
		}

		protected void CustomizeDeadPart(DeadPartCustomizationData customizationData)
		{
			ApplyButtPreset(customizationData.ButtMeshPreset);
			ApplyButtTexturePreset(customizationData.ButtTexturePreset);
			AssignShaderColor(_maskedRenderers, customizationData.ButtColor, _colorProperty);
			ApplySkin(customizationData.ButtSkinId);
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

		private void AssignShaderColor(List<Renderer> deadPartRenderers, Color color, int shaderColorProperty)
		{
			color.a = 1f;
			foreach (Renderer deadPartRenderer in deadPartRenderers)
			{
				Material material = new Material(deadPartRenderer.material);
				material.SetColor(shaderColorProperty, color);
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

		private void ApplySkin(SkinType skinId)
		{
			PlayerSkin playerSkin = UnityEngine.Object.Instantiate(_skinsConfiguration.PlayerSkins[skinId], _skinParent, worldPositionStays: false);
			SkinnedMeshRenderer[] componentsInChildren = playerSkin.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach (SkinnedMeshRenderer obj in componentsInChildren)
			{
				obj.rootBone = _characterRootBone;
				obj.bones = _characterSkinnedMesh.bones;
			}
			MultipleVisibilityHandler component = playerSkin.GetComponent<MultipleVisibilityHandler>();
			component.EnableBottomRenderObjectOnly();
			component.DisableHatRenderObject();
			component.DisableTorsoRenderObject();
		}

		private void ApplyButtPreset(ButtMeshPreset currentPreset)
		{
			if (currentPreset != ButtMeshPreset.None)
			{
				ButtMeshPresetData buttMeshPresetData = _buttMeshPresetConfiguration.MeshPresets[currentPreset];
				AssignMesh(_meshRenderers, buttMeshPresetData.Mesh);
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
