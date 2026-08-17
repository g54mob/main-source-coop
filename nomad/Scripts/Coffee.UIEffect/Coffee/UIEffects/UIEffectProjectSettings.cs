using System;
using System.Collections.Generic;
using Coffee.UIEffectInternal;
using UnityEngine;

namespace Coffee.UIEffects
{
	public class UIEffectProjectSettings : PreloadedProjectSettings<UIEffectProjectSettings>
	{
		[Header("Setting")]
		[SerializeField]
		internal List<UIEffect> m_RuntimePresets = new List<UIEffect>();

		[SerializeField]
		internal List<UIEffectPreset> m_RuntimePresetsV2 = new List<UIEffectPreset>();

		[Header("Editor")]
		[Tooltip("Use HDR color pickers on color fields.")]
		[SerializeField]
		private bool m_UseHDRColorPicker = true;

		[HideInInspector]
		[SerializeField]
		internal ShaderVariantCollection m_ShaderVariantCollection;

		[HideInInspector]
		[SerializeField]
		private ShaderVariantRegistry m_ShaderVariantRegistry = new ShaderVariantRegistry();

		public static ShaderVariantRegistry shaderRegistry => PreloadedProjectSettings<UIEffectProjectSettings>.instance.m_ShaderVariantRegistry;

		public static ShaderVariantCollection shaderVariantCollection => shaderRegistry.shaderVariantCollection;

		public static bool useHdrColorPicker
		{
			get
			{
				return PreloadedProjectSettings<UIEffectProjectSettings>.instance.m_UseHDRColorPicker;
			}
			set
			{
				PreloadedProjectSettings<UIEffectProjectSettings>.instance.m_UseHDRColorPicker = value;
			}
		}

		public static void RegisterRuntimePreset(UIEffectPreset preset)
		{
			if ((bool)preset && !PreloadedProjectSettings<UIEffectProjectSettings>.instance.m_RuntimePresetsV2.Contains(preset))
			{
				PreloadedProjectSettings<UIEffectProjectSettings>.instance.m_RuntimePresetsV2.Add(preset);
			}
		}

		[Obsolete("LoadRuntimePreset is obsolete. Use LoadPreset instead.", false)]
		public static UIEffect LoadRuntimePreset(string presetName)
		{
			for (int i = 0; i < PreloadedProjectSettings<UIEffectProjectSettings>.instance.m_RuntimePresets.Count; i++)
			{
				UIEffect uIEffect = PreloadedProjectSettings<UIEffectProjectSettings>.instance.m_RuntimePresets[i];
				if ((bool)uIEffect && uIEffect.name == presetName)
				{
					return uIEffect;
				}
			}
			return null;
		}

		public static UnityEngine.Object LoadPreset(string presetName)
		{
			for (int i = 0; i < PreloadedProjectSettings<UIEffectProjectSettings>.instance.m_RuntimePresetsV2.Count; i++)
			{
				UIEffectPreset uIEffectPreset = PreloadedProjectSettings<UIEffectProjectSettings>.instance.m_RuntimePresetsV2[i];
				if ((bool)uIEffectPreset && uIEffectPreset.name == presetName)
				{
					return uIEffectPreset;
				}
			}
			return LoadRuntimePreset(presetName);
		}
	}
}
