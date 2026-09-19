using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.LightBaking
{
	public class PrefabLightmapData : MonoBehaviour
	{
		[Serializable]
		private struct RendererInfo
		{
			public Renderer Renderer;

			public int LightmapIndex;

			public Vector4 LightmapOffsetScale;
		}

		[Serializable]
		private struct LightInfo
		{
			public Light Light;

			public int LightmapBakeType;

			public int MixedLightingMode;
		}

		[Tooltip("Reassigns shaders when applying the baked lightmaps. Might conflict with transparent HDRP shaders.")]
		[SerializeField]
		private bool _releaseShaders;

		[SerializeField]
		private RendererInfo[] _rendererInfos;

		[SerializeField]
		private Texture2D[] _lightmaps;

		[SerializeField]
		private Texture2D[] _lightmapsDir;

		[SerializeField]
		private Texture2D[] _shadowMasks;

		[SerializeField]
		private LightInfo[] _lightInfos;

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			if (_rendererInfos == null || _rendererInfos.Length == 0)
			{
				return;
			}
			LightmapData[] lightmaps = LightmapSettings.lightmaps;
			int[] array = new int[_lightmaps.Length];
			int num = lightmaps.Length;
			List<LightmapData> list = new List<LightmapData>();
			for (int i = 0; i < _lightmaps.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < lightmaps.Length; j++)
				{
					if (!(_lightmaps[i] != lightmaps[j].lightmapColor))
					{
						flag = true;
						array[i] = j;
						break;
					}
				}
				if (!flag)
				{
					array[i] = num;
					list.Add(new LightmapData
					{
						lightmapColor = _lightmaps[i],
						lightmapDir = ((_lightmapsDir.Length == _lightmaps.Length) ? _lightmapsDir[i] : null),
						shadowMask = ((_shadowMasks.Length == _lightmaps.Length) ? _shadowMasks[i] : null)
					});
					num++;
				}
			}
			LightmapData[] array2 = new LightmapData[num];
			lightmaps.CopyTo(array2, 0);
			list.ToArray().CopyTo(array2, lightmaps.Length);
			bool flag2 = true;
			Texture2D[] lightmapsDir = _lightmapsDir;
			for (int k = 0; k < lightmapsDir.Length; k++)
			{
				if (!(lightmapsDir[k] != null))
				{
					flag2 = false;
					break;
				}
			}
			LightmapSettings.lightmapsMode = ((_lightmapsDir.Length == _lightmaps.Length && flag2) ? LightmapsMode.CombinedDirectional : LightmapsMode.NonDirectional);
			LightmapSettings.lightmaps = array2;
			ApplyRendererInfos(_rendererInfos, array, _lightInfos);
		}

		private void ApplyRendererInfos(RendererInfo[] infos, int[] lightmapOffsetIndexes, LightInfo[] lightInfos)
		{
			for (int i = 0; i < infos.Length; i++)
			{
				RendererInfo rendererInfo = infos[i];
				if (rendererInfo.Renderer == null)
				{
					Debug.LogWarning("Renderer reference is missing . Skipping lightmap assignment for this renderer.");
					continue;
				}
				rendererInfo.Renderer.lightmapIndex = lightmapOffsetIndexes[rendererInfo.LightmapIndex];
				rendererInfo.Renderer.lightmapScaleOffset = rendererInfo.LightmapOffsetScale;
				if (!_releaseShaders)
				{
					continue;
				}
				Material[] sharedMaterials = rendererInfo.Renderer.sharedMaterials;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					if (sharedMaterials[j] != null && Shader.Find(sharedMaterials[j].shader.name) != null)
					{
						sharedMaterials[j].shader = Shader.Find(sharedMaterials[j].shader.name);
					}
				}
			}
			for (int i = 0; i < lightInfos.Length; i++)
			{
				LightInfo lightInfo = lightInfos[i];
				if (!(lightInfo.Light == null))
				{
					lightInfo.Light.bakingOutput = new LightBakingOutput
					{
						isBaked = true,
						lightmapBakeType = (LightmapBakeType)lightInfo.LightmapBakeType,
						mixedLightingMode = (MixedLightingMode)lightInfo.MixedLightingMode
					};
				}
			}
		}
	}
}
