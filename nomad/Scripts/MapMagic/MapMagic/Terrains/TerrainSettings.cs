using System;
using Den.Tools;
using Den.Tools.GUI;
using MapMagic.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace MapMagic.Terrains
{
	[Serializable]
	public class TerrainSettings
	{
		[Val(name = "Auto Connect", cat = "AutoConnect")]
		public bool allowAutoConnect = true;

		[Val(name = "Grouping ID", cat = "AutoConnect")]
		public int groupingID;

		[Val(name = "Base Map Dist.", cat = "BaseMap")]
		public int baseMapDist = 1000;

		[Val(name = "Show Base Map", cat = "BaseMap")]
		public bool showBaseMap = true;

		[Val(name = "Base Map Resolution", cat = "BaseMap")]
		public int baseMapResolution = 1024;

		[Val(name = "Draw Instanced", cat = "Terrain")]
		public bool drawInstanced = true;

		[Val(name = "Pixel Error", cat = "Terrain")]
		public int pixelError = 1;

		[Val(name = "Cast Shadows", cat = "Terrain")]
		public ShadowCastingMode shadowCastingMode;

		[Val(name = "Reflection Probes", cat = "Terrain")]
		public ReflectionProbeUsage reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;

		[Val(name = "Editor Render Flags", cat = "Misc")]
		public TerrainRenderFlags editorRenderFlags = TerrainRenderFlags.all;

		[Val(name = "Maximim LOD", cat = "Misc")]
		public int heightmapMaximumLOD;

		[Val(name = "Material Template", cat = "Materials", type = typeof(Material))]
		public Material material;

		[Val(name = "Draw Detail", cat = "Grass")]
		public bool detailDraw = true;

		[Val(name = "Detail Distance", cat = "Grass")]
		public float detailDistance = 80f;

		[Val(name = "Detail Density", cat = "Grass")]
		public float detailDensity = 1f;

		[Val(name = "Tree Distance", cat = "Trees")]
		public float treeDistance = 1000f;

		[Val(name = "Billboard Start", cat = "Trees")]
		public float treeBillboardStart = 200f;

		[Val(name = "Fade Length", cat = "Trees")]
		public float treeFadeLength = 5f;

		[Val(name = "Max Full LOD Trees", cat = "Trees")]
		public int treeFullLod = 150;

		[Val(name = "Tree LOD Bias Multiplier", cat = "Trees")]
		public float treeLODBiasMultiplier = 1f;

		[Val(name = "Bake Light Probes For Trees", cat = "Trees")]
		public bool bakeLightProbesForTrees;

		[Val(name = "Remove Light Probe Ringing", cat = "Trees")]
		public bool deringLightProbesForTrees = true;

		[Val(name = "Preserve Tree Prototype Layers", cat = "Trees")]
		public bool preserveTreePrototypeLayers;

		[Val(name = "Wind Speed", cat = "WindTint")]
		public float windSpeed = 0.5f;

		[Val(name = "Wind Bending", cat = "WindTint")]
		public float windSize = 0.5f;

		[Val(name = "Wind Size", cat = "WindTint")]
		public float windBending = 0.5f;

		[Val(name = "Grass Tint", cat = "WindTint")]
		public Color grassTint = Color.gray;

		[Val(name = "Copy Layers", cat = "Copy")]
		public bool guiCopy;

		[Val(name = "Copy Tags", cat = "Copy")]
		public bool copyLayersTags = true;

		[Val(name = "Copy Components", cat = "Copy")]
		public bool copyComponents;

		public void ApplyAll(Terrain terrain)
		{
			ApplySettings(terrain);
			ApplyMaterial(terrain);
			CopyLayersTagsComponents(terrain);
		}

		public void ApplySettings(Terrain terrain)
		{
			terrain.allowAutoConnect = allowAutoConnect;
			terrain.groupingID = groupingID;
			terrain.editorRenderFlags = editorRenderFlags;
			terrain.drawInstanced = drawInstanced;
			terrain.heightmapPixelError = pixelError;
			terrain.basemapDistance = (showBaseMap ? baseMapDist : 2147483647);
			if (terrain.terrainData.baseMapResolution != baseMapResolution)
			{
				terrain.terrainData.baseMapResolution = baseMapResolution;
			}
			terrain.shadowCastingMode = shadowCastingMode;
			terrain.reflectionProbeUsage = reflectionProbeUsage;
			terrain.heightmapMaximumLOD = heightmapMaximumLOD;
			terrain.drawTreesAndFoliage = detailDraw;
			terrain.detailObjectDistance = detailDistance;
			terrain.detailObjectDensity = detailDensity;
			terrain.treeDistance = treeDistance;
			terrain.treeBillboardDistance = treeBillboardStart;
			terrain.treeCrossFadeLength = treeFadeLength;
			terrain.treeLODBiasMultiplier = treeLODBiasMultiplier;
			terrain.treeMaximumFullLODCount = treeFullLod;
			terrain.preserveTreePrototypeLayers = preserveTreePrototypeLayers;
			terrain.terrainData.SetDetailScatterMode(DetailScatterMode.InstanceCountMode);
			terrain.terrainData.wavingGrassSpeed = windSpeed;
			terrain.terrainData.wavingGrassAmount = windSize;
			terrain.terrainData.wavingGrassStrength = windBending;
			terrain.terrainData.wavingGrassTint = grassTint;
		}

		public void ApplyMaterial(Terrain terrain)
		{
			terrain.materialTemplate = material;
		}

		public void CopyLayersTagsComponents(Terrain terrain)
		{
			GameObject gameObject = terrain.gameObject;
			GameObject gameObject2 = gameObject.transform.parent.parent.gameObject;
			if (copyLayersTags)
			{
				gameObject.layer = gameObject2.layer;
				gameObject.isStatic = gameObject2.isStatic;
				try
				{
					gameObject.tag = gameObject2.tag;
				}
				catch
				{
					Debug.LogError("MapMagic: could not copy object tag");
				}
			}
			if (!copyComponents)
			{
				return;
			}
			MonoBehaviour[] components = gameObject2.GetComponents<MonoBehaviour>();
			for (int i = 0; i < components.Length; i++)
			{
				if (!(components[i] is MapMagicObject) && !(components[i] == null) && terrain.gameObject.GetComponent(components[i].GetType()) == null)
				{
					ReflectionExtensions.CopyComponent(components[i], gameObject);
				}
			}
		}
	}
}
