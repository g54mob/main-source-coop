using System;
using System.Collections.Generic;
using Boxophobic.StyledGUI;
using Boxophobic.Utility;
using UnityEngine;

namespace TheVisualEngine
{
	public class TVETerrain : StyledMonoBehaviour
	{
		[Tooltip("Sync the terrain data with the material in editor if the terrain is modified by external tools.")]
		public TVERefreshMode terrainRefresh;

		[Tooltip("Sets the terrain bounds multiplier used to avoid patches culling when using vertex offset elements.")]
		public float terrainBounds = 1f;

		public Material terrainMaterial;

		[Tooltip("Override the terrain layer maps and settings without modifying the actual terrain layer.")]
		public TVETerrainSettings terrainSettings = new TVETerrainSettings();

		[Tooltip("Override the terrain layer maps and settings without modifying the actual terrain layer.")]
		public TVETerrainRenderer terrainRenderer = new TVETerrainRenderer();

		[NonSerialized]
		public Terrain terrain;

		[NonSerialized]
		public Renderer meshRenderer;

		[NonSerialized]
		public MeshFilter meshFilter;

		[NonSerialized]
		public Vector3 terrainPosition;

		[NonSerialized]
		public Vector3 terrainSize;

		[NonSerialized]
		public int terrainLayers;

		[NonSerialized]
		public MaterialPropertyBlock terrainPropertyBlock;

		[NonSerialized]
		public bool isActive;

		[NonSerialized]
		public int terrainID;

		private bool isValidTerrain;

		private bool isValidRenderer;

		private Mesh terrainProxyMesh;

		private void OnEnable()
		{
			BoxoUtils.DisableServerExecution();
			InitializeTerrain();
			UpdateTerrainSettings();
			if (terrainRenderer.bakeMode == TVETerrainBaking.RuntimeRenderTexture)
			{
				DestroyProxyTextures();
				CreateProxyTextures(saveTextures: false);
			}
			UpdateProxySettings();
			AddTerrainToManager();
		}

		private void OnDisable()
		{
			if (terrainRenderer.bakeMode == TVETerrainBaking.RuntimeRenderTexture)
			{
				DestroyProxyTextures();
			}
		}

		private void OnDestroy()
		{
			if (terrainRenderer.bakeMode == TVETerrainBaking.RuntimeRenderTexture)
			{
				DestroyProxyTextures();
			}
		}

		private void Update()
		{
			BoxoUtils.DisableServerExecution();
		}

		private void InitializeTerrain()
		{
			terrain = GetComponent<Terrain>();
			meshRenderer = GetComponent<Renderer>();
			meshFilter = GetComponent<MeshFilter>();
			isValidTerrain = terrain != null && terrain.terrainData != null && terrain.materialTemplate != null;
			isValidRenderer = meshRenderer != null && meshRenderer.sharedMaterial != null && meshFilter != null && meshFilter.sharedMesh != null;
			if (isValidTerrain && isValidRenderer)
			{
				isValidRenderer = false;
			}
			if (terrainMaterial != null)
			{
				terrain.materialTemplate = terrainMaterial;
			}
			if (isValidTerrain)
			{
				terrainMaterial = terrain.materialTemplate;
				terrainPosition = terrain.transform.position;
				terrainSize = terrain.terrainData.size;
			}
			if (isValidRenderer)
			{
				terrainMaterial = meshRenderer.sharedMaterial;
				terrainPosition = meshRenderer.bounds.center - meshRenderer.bounds.extents;
				terrainSize = meshRenderer.bounds.size;
			}
			if (terrainProxyMesh == null)
			{
				terrainProxyMesh = TVEUtils.CreateQuadFromTerrain(terrainPosition, terrainSize);
			}
			terrainID = GetHashCode();
		}

		public void UpdateTerrainSettings()
		{
			if (terrainPropertyBlock == null)
			{
				terrainPropertyBlock = new MaterialPropertyBlock();
			}
			if (isValidTerrain)
			{
				if (terrain.terrainData.holesTexture != null)
				{
					terrainPropertyBlock.SetTexture("_TerrainHolesTex", terrain.terrainData.holesTexture);
				}
				else
				{
					terrainPropertyBlock.SetTexture("_TerrainHolesTex", Texture2D.whiteTexture);
				}
				for (int i = 0; i < terrain.terrainData.alphamapTextures.Length; i++)
				{
					Texture2D texture2D = terrain.terrainData.alphamapTextures[i];
					int num = i + 1;
					if (texture2D != null)
					{
						terrainPropertyBlock.SetTexture("_TerrainControlTex" + num, texture2D);
					}
					else
					{
						terrainPropertyBlock.SetTexture("_TerrainControlTex" + num, Texture2D.blackTexture);
					}
				}
				for (int j = 0; j < terrain.terrainData.terrainLayers.Length; j++)
				{
					TerrainLayer terrainLayer = terrain.terrainData.terrainLayers[j];
					int index = j + 1;
					if (!(terrainLayer == null))
					{
						CopyLayerSettings(terrainPropertyBlock, terrainLayer, index);
					}
				}
				terrainLayers = terrain.terrainData.terrainLayers.Length;
			}
			if (isValidTerrain || isValidRenderer)
			{
				if (terrainSettings.useCustomTextures)
				{
					if (terrainSettings.terrainHolesMask != null)
					{
						terrainPropertyBlock.SetTexture("_TerrainHolesTex", terrainSettings.terrainHolesMask);
					}
					if (terrainSettings.terrainControl01 != null)
					{
						terrainPropertyBlock.SetTexture("_TerrainControlTex1", terrainSettings.terrainControl01);
					}
					if (terrainSettings.terrainControl02 != null)
					{
						terrainPropertyBlock.SetTexture("_TerrainControlTex2", terrainSettings.terrainControl02);
					}
					if (terrainSettings.terrainControl03 != null)
					{
						terrainPropertyBlock.SetTexture("_TerrainControlTex3", terrainSettings.terrainControl03);
					}
					if (terrainSettings.terrainControl04 != null)
					{
						terrainPropertyBlock.SetTexture("_TerrainControlTex4", terrainSettings.terrainControl04);
					}
				}
				for (int k = 0; k < terrainSettings.terrainLayers.Count; k++)
				{
					TVETerrainLayerSettings tVETerrainLayerSettings = terrainSettings.terrainLayers[k];
					if (!tVETerrainLayerSettings.isInitialized)
					{
						terrainSettings.terrainLayers[k] = new TVETerrainLayerSettings();
						terrainSettings.terrainLayers[k].isInitialized = true;
					}
					int index2 = ((!terrainSettings.useLayersOrderAsID) ? tVETerrainLayerSettings.layerID : (k + 1));
					terrainPropertyBlock.SetVector("_TerrainColor" + index2, tVETerrainLayerSettings.layerColor);
					if (tVETerrainLayerSettings.useCustomLayer && tVETerrainLayerSettings.terrainLayer != null)
					{
						CopyLayerSettings(terrainPropertyBlock, tVETerrainLayerSettings.terrainLayer, index2);
					}
					if (tVETerrainLayerSettings.useCustomTextures)
					{
						if (tVETerrainLayerSettings.layerAlbedo != null)
						{
							terrainPropertyBlock.SetTexture("_TerrainAlbedoTex" + index2, tVETerrainLayerSettings.layerAlbedo);
						}
						if (tVETerrainLayerSettings.layerNormal != null)
						{
							terrainPropertyBlock.SetTexture("_TerrainNormalTex" + index2, tVETerrainLayerSettings.layerNormal);
						}
						if (tVETerrainLayerSettings.layerShader != null)
						{
							terrainPropertyBlock.SetTexture("_TerrainShaderTex" + index2, tVETerrainLayerSettings.layerShader);
						}
					}
					if (tVETerrainLayerSettings.useCustomSettings)
					{
						float x = 1f / (tVETerrainLayerSettings.layerRemapMax.x - tVETerrainLayerSettings.layerRemapMin.x);
						float y = 1f / (tVETerrainLayerSettings.layerRemapMax.y - tVETerrainLayerSettings.layerRemapMin.y);
						float z = 1f / (tVETerrainLayerSettings.layerRemapMax.z - tVETerrainLayerSettings.layerRemapMin.z);
						float w = 1f / (tVETerrainLayerSettings.layerRemapMax.w - tVETerrainLayerSettings.layerRemapMin.w);
						terrainPropertyBlock.SetVector("_TerrainSpecular" + index2, tVETerrainLayerSettings.layerSpecular);
						terrainPropertyBlock.SetVector("_TerrainShaderMin" + index2, tVETerrainLayerSettings.layerRemapMin);
						terrainPropertyBlock.SetVector("_TerrainShaderRcp" + index2, new Vector4(x, y, z, w));
						terrainPropertyBlock.SetVector("_TerrainParams" + index2, new Vector4(0f, 0f, tVETerrainLayerSettings.layerNormalScale, tVETerrainLayerSettings.layerSmoothness));
					}
					if (tVETerrainLayerSettings.useCustomCoords)
					{
						if (tVETerrainLayerSettings.layerUVMode == TVEUVMode.Tilling)
						{
							terrainPropertyBlock.SetVector("_TerrainCoord" + index2, new Vector4(tVETerrainLayerSettings.layerUVValue.x, tVETerrainLayerSettings.layerUVValue.y, tVETerrainLayerSettings.layerUVValue.z, tVETerrainLayerSettings.layerUVValue.w));
						}
						else
						{
							terrainPropertyBlock.SetVector("_TerrainCoord" + index2, new Vector4(1f / tVETerrainLayerSettings.layerUVValue.x, 1f / tVETerrainLayerSettings.layerUVValue.y, tVETerrainLayerSettings.layerUVValue.z, tVETerrainLayerSettings.layerUVValue.w));
						}
					}
				}
			}
			terrainPropertyBlock.SetVector("_TerrainPosition", terrainPosition);
			terrainPropertyBlock.SetVector("_TerrainSize", terrainSize);
			if (isValidTerrain)
			{
				terrainPropertyBlock.SetFloat("_TerrainModelMode", 0f);
				terrain.SetSplatMaterialPropertyBlock(terrainPropertyBlock);
				terrainMaterial = terrain.materialTemplate;
				if (terrain.patchBoundsMultiplier.x != terrainBounds)
				{
					terrain.patchBoundsMultiplier = new Vector3(terrainBounds, terrainBounds, terrainBounds);
				}
			}
			if (isValidRenderer)
			{
				terrainPropertyBlock.SetFloat("_TerrainModelMode", 1f);
				meshRenderer.SetPropertyBlock(terrainPropertyBlock);
				terrainMaterial = meshRenderer.sharedMaterial;
			}
		}

		private void CopyLayerSettings(MaterialPropertyBlock materialPropertyBlock, TerrainLayer layer, int index)
		{
			if (layer.diffuseTexture != null)
			{
				materialPropertyBlock.SetTexture("_TerrainAlbedoTex" + index, layer.diffuseTexture);
			}
			else
			{
				materialPropertyBlock.SetTexture("_TerrainAlbedoTex" + index, Texture2D.whiteTexture);
			}
			if (layer.normalMapTexture != null)
			{
				materialPropertyBlock.SetTexture("_TerrainNormalTex" + index, layer.normalMapTexture);
			}
			else
			{
				materialPropertyBlock.SetTexture("_TerrainNormalTex" + index, Texture2D.linearGrayTexture);
			}
			if (layer.maskMapTexture != null)
			{
				materialPropertyBlock.SetTexture("_TerrainShaderTex" + index, layer.maskMapTexture);
			}
			else
			{
				materialPropertyBlock.SetTexture("_TerrainShaderTex" + index, Texture2D.whiteTexture);
			}
			float x = 1f / (layer.maskMapRemapMax.x - layer.maskMapRemapMin.x);
			float y = 1f / (layer.maskMapRemapMax.y - layer.maskMapRemapMin.y);
			float z = 1f / (layer.maskMapRemapMax.z - layer.maskMapRemapMin.z);
			float w = 1f / (layer.maskMapRemapMax.w - layer.maskMapRemapMin.w);
			materialPropertyBlock.SetVector("_TerrainShaderMin" + index, layer.maskMapRemapMin);
			materialPropertyBlock.SetVector("_TerrainShaderRcp" + index, new Vector4(x, y, z, w));
			materialPropertyBlock.SetVector("_TerrainParams" + index, new Vector4(layer.metallic, 0f, layer.normalScale, layer.smoothness));
			materialPropertyBlock.SetVector("_TerrainSpecular" + index, layer.specular);
			materialPropertyBlock.SetVector("_TerrainCoord" + index, new Vector4(1f / layer.tileSize.x, 1f / layer.tileSize.y, layer.tileOffset.x, layer.tileOffset.y));
		}

		private void AddTerrainToManager()
		{
			if (!(TVEManager.Instance == null))
			{
				List<TVETerrain> sceneTerrains = TVEManager.Instance.sceneTerrains;
				if (!sceneTerrains.Contains(this))
				{
					sceneTerrains.Add(this);
				}
			}
		}

		public void DestroyProxyTextures()
		{
		}

		public void TryGetProxyTextures()
		{
		}

		public void CreateProxyTextures(bool saveTextures)
		{
			string proxyName = GetProxyName();
			TVEProxyData tVEProxyData = new TVEProxyData();
			Shader dependency = terrainMaterial.shader.GetDependency("BakerShader");
			tVEProxyData.blitTVETerrain = this;
			tVEProxyData.blitMesh = terrainProxyMesh;
			tVEProxyData.blitShader = dependency;
			tVEProxyData.saveSize = (int)terrainRenderer.bakeTexture;
			if (terrainRenderer.bakeMaterial != null)
			{
				tVEProxyData.blitMaterial = terrainRenderer.bakeMaterial;
			}
			else
			{
				tVEProxyData.blitMaterial = terrainMaterial;
			}
			tVEProxyData.bakeData = 0;
			tVEProxyData.saveAsSRGB = true;
			tVEProxyData.saveAsDefault = true;
			if (saveTextures)
			{
				tVEProxyData.bakeAlbedoAsSRGB = true;
			}
			else
			{
				tVEProxyData.bakeAlbedoAsSRGB = false;
			}
			terrainSettings.terrainAlbedo = TVEUtils.CreateProxyTextureFromTerrain(tVEProxyData);
			terrainSettings.terrainAlbedo.name = proxyName + "_Albedo";
			tVEProxyData.bakeData = 1;
			tVEProxyData.saveAsSRGB = false;
			tVEProxyData.saveAsDefault = true;
			terrainSettings.terrainNormal = TVEUtils.CreateProxyTextureFromTerrain(tVEProxyData);
			terrainSettings.terrainNormal.name = proxyName + "_Normal";
			tVEProxyData.bakeData = 2;
			tVEProxyData.saveAsSRGB = false;
			tVEProxyData.saveAsDefault = true;
			terrainSettings.terrainShader = TVEUtils.CreateProxyTextureFromTerrain(tVEProxyData);
			terrainSettings.terrainShader.name = proxyName + "_Shader";
			tVEProxyData.bakeData = 3;
			tVEProxyData.saveAsSRGB = false;
			tVEProxyData.saveAsDefault = true;
			terrainSettings.terrainFeature = TVEUtils.CreateProxyTextureFromTerrain(tVEProxyData);
			terrainSettings.terrainFeature.name = proxyName + "_Feature";
		}

		public void UpdateProxySettings()
		{
			if (terrainSettings.terrainAlbedo != null)
			{
				terrainPropertyBlock.SetTexture("_TerrainAlbedoTex", terrainSettings.terrainAlbedo);
			}
			else
			{
				Texture texture = terrainPropertyBlock.GetTexture("_terrainAlbedoTex1");
				if (texture != null)
				{
					terrainPropertyBlock.SetTexture("_TerrainAlbedoTex", texture);
				}
			}
			if (terrainSettings.terrainNormal != null)
			{
				terrainPropertyBlock.SetTexture("_TerrainNormalTex", terrainSettings.terrainNormal);
			}
			if (terrainSettings.terrainShader != null)
			{
				terrainPropertyBlock.SetTexture("_TerrainShaderTex", terrainSettings.terrainShader);
			}
			if (terrainSettings.terrainFeature != null)
			{
				terrainPropertyBlock.SetTexture("_TerrainFeatureTex", terrainSettings.terrainFeature);
			}
			if (isValidTerrain)
			{
				terrain.SetSplatMaterialPropertyBlock(terrainPropertyBlock);
			}
			if (isValidRenderer)
			{
				meshRenderer.SetPropertyBlock(terrainPropertyBlock);
			}
		}

		private string GetProxyName()
		{
			string result = "Terrain";
			if (isValidTerrain)
			{
				result = terrain.terrainData.name;
			}
			if (isValidRenderer)
			{
				result = meshFilter.sharedMesh.name;
			}
			return result;
		}
	}
}
