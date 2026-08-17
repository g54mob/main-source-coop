using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace StylizedGrass
{
	[AddComponentMenu("Stylized Grass/Colormap Renderer")]
	[ExecuteInEditMode]
	[HelpURL("http://staggart.xyz/unity/stylized-grass-shader/sgs-docs/?section=blending-with-terrain-colors")]
	public class GrassColorMapRenderer : MonoBehaviour
	{
		[Serializable]
		public class LayerScaleSettings
		{
			public int layerID;

			[Range(0f, 1f)]
			public float strength = 1f;
		}

		public static GrassColorMapRenderer Instance;

		public UniversalRendererData renderData;

		public GrassColorMap colorMap;

		[Tooltip("These objects can be Unity Terrains or custom Mesh Terrains. Their size can be used to automatically fit the render area")]
		public List<GameObject> terrainObjects = new List<GameObject>();

		public int resIdx = 4;

		public int resolution = 1024;

		[Tooltip("Objects set to this layer will be included in the render")]
		public LayerMask renderLayer = -1;

		[Tooltip("Render objects on specific layers into the color map. When disabled, the terrain(s) are temporarily moved up 1000 units")]
		public bool useLayers;

		[Tooltip("Enable this option if you're using a custom terrain shader which greatly alters the terrain color (eg. global noise).\n\nWhen disabled, the terrains are temporarily rendered using an Unlit shader (based on the default Unity terrain shader)\n\nThis only applies to Unity terrain, not meshes")]
		public bool thirdPartyShader;

		public Camera renderCam;

		[NonSerialized]
		public bool showBounds = true;

		public List<LayerScaleSettings> layerScaleSettings = new List<LayerScaleSettings>();

		private void OnEnable()
		{
			Instance = this;
			AssignColorMap();
		}

		private void OnDisable()
		{
			Instance = null;
			GrassColorMap.DisableGlobally();
		}

		private void OnDrawGizmosSelected()
		{
			if ((bool)colorMap && showBounds)
			{
				Gizmos.color = (Color32)new Color(0f, 0.66f, 1f, 0.25f);
				Gizmos.DrawCube(colorMap.bounds.center, colorMap.bounds.size);
				Gizmos.color = (Color32)new Color(0f, 0.66f, 1f, 1f);
				Gizmos.DrawWireCube(colorMap.bounds.center, colorMap.bounds.size);
			}
		}

		public void AssignActiveTerrains()
		{
			Terrain[] activeTerrains = Terrain.activeTerrains;
			for (int i = 0; i < activeTerrains.Length; i++)
			{
				if (!terrainObjects.Contains(activeTerrains[i].gameObject))
				{
					terrainObjects.Add(activeTerrains[i].gameObject);
				}
			}
		}

		public void AssignVegetationStudioMeshTerrains()
		{
		}

		public void AssignChildMeshes()
		{
			MeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!terrainObjects.Contains(componentsInChildren[i].gameObject))
				{
					terrainObjects.Add(componentsInChildren[i].gameObject);
				}
			}
		}

		public void AssignColorMap()
		{
			if ((bool)colorMap)
			{
				colorMap.SetActive();
			}
		}
	}
}
