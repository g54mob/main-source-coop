using System;
using System.Collections.Generic;
using Boxophobic.StyledGUI;
using Boxophobic.Utility;
using UnityEngine;
using UnityEngine.Rendering;

namespace TheVisualEngine
{
	[HelpURL("https://docs.google.com/document/d/145JOVlJ1tE-WODW45YoJ6Ixg23mFc56EnB_8Tbwloz8/edit#heading=h.hbq3w8ae720x")]
	[ExecuteInEditMode]
	[AddComponentMenu("BOXOPHOBIC/The Visual Engine/TVE Manager")]
	public class TVEManager : StyledMonoBehaviour
	{
		public static TVEManager Instance;

		[StyledCategory("Quick Settings", 5f, 10f)]
		public bool quickCat;

		[Tooltip("Controls the global wind power.")]
		[StyledRangeOptions("Motion Control", 0f, 1f, new string[] { "None", "Windy", "Strong" })]
		public float motionControl = 0.5f;

		[Space(10f)]
		[Tooltip("Use the Seasons slider to control the element properties when the element is set to Seasons mode.")]
		[StyledRangeOptions("Season Control", 0f, 4f, new string[] { "Winter", "Spring", "Summer", "Autumn", "Winter" })]
		public float seasonControl = 2f;

		[StyledCategory("Main Settings")]
		public bool mainCat;

		[StyledMessage("Error", "Main Camera not found! Make sure you have a main camera with Main Camera tag in your scene! Particle elements updating will be skipped without it. Enter play mode to update the status!", 0f, 10f)]
		public bool styledCameraMessaage;

		[Tooltip("Sets the main camera used for scene rendering.")]
		public Camera mainCamera;

		[Tooltip("Sets the main light used as the sun in the scene.")]
		public Light mainLight;

		[Tooltip("Sets the main direction from a gameobject.")]
		public GameObject mainWind;

		[StyledCategory("Player Settings")]
		public bool playerCat;

		[Tooltip("Sets the main player gameobject.")]
		public GameObject playerObject;

		[Tooltip("Sets the main player radius.")]
		public float playerRadius = 1f;

		[StyledCategory("Global Settings")]
		public bool globalCat;

		public TVEGlobalCoatData globalCoatData = new TVEGlobalCoatData();

		public TVEGlobalPaintData globalPaintData = new TVEGlobalPaintData();

		public TVEGlobalAtmoData globalAtmoData = new TVEGlobalAtmoData();

		public TVEGlobalGlowData globalGlowData = new TVEGlobalGlowData();

		public TVEGlobalFormData globalFormData = new TVEGlobalFormData();

		[StyledCategory("Element Settings")]
		public bool elementCat;

		[Tooltip("Controls the elements visibility in scene and game view.")]
		public TVEElementsVisibility elementVisibility = TVEElementsVisibility.HiddenAtRuntime;

		[HideInInspector]
		public TVEElementsVisibility elementVisibilityOld = TVEElementsVisibility.HiddenAtRuntime;

		[Tooltip("Controls the elements sorting by element position. Always enabled in edit mode.")]
		public TVEElementsOrdering elementOrdering;

		[Tooltip("Controls the elements rendering.")]
		public TVEElementRendererData elementRenderer = new TVEElementRendererData();

		[StyledCategory("Other Settings")]
		public bool otherCat;

		public bool useShaderMetaSettings = true;

		[NonSerialized]
		[Space(10f)]
		public List<TVEElementBufferData> renderDataSet = new List<TVEElementBufferData>();

		[NonSerialized]
		public List<TVEElement> renderElements = new List<TVEElement>();

		[NonSerialized]
		public List<TVEInstanced> renderInstances = new List<TVEInstanced>();

		[NonSerialized]
		public List<TVETerrain> sceneTerrains = new List<TVETerrain>();

		private Vector3 focusPosition;

		private Vector3 focusRotation;

		private MaterialPropertyBlock propertyBlock;

		private Matrix4x4 projectionMatrix;

		private Matrix4x4 modelViewMatrix = new Matrix4x4(new Vector4(1f, 0f, 0f, 0f), new Vector4(0f, 0f, -1f, 0f), new Vector4(0f, -1f, 0f, 0f), new Vector4(0f, 0f, 0f, 1f));

		private bool sortDirty;

		private void OnEnable()
		{
			BoxoUtils.DisableServerExecution();
			EnableManager();
			InitElementsRendering();
			SortElementObjects();
			SetElementsVisibility();
		}

		private void OnDisable()
		{
			BoxoUtils.DisableServerExecution();
			DisableManager();
		}

		private void OnDestroy()
		{
			BoxoUtils.DisableServerExecution();
			DisableManager();
		}

		private void Start()
		{
			BoxoUtils.DisableServerExecution();
			if (Application.isPlaying)
			{
				base.gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
			else
			{
				base.gameObject.GetComponent<MeshRenderer>().enabled = true;
			}
			if (mainLight == null)
			{
				SetGlobalLightingMainLight();
			}
		}

		private void Update()
		{
			BoxoUtils.DisableServerExecution();
			if (mainWind == null)
			{
				mainWind = base.gameObject;
			}
			base.gameObject.transform.eulerAngles = new Vector3(0f, mainWind.transform.eulerAngles.y, 0f);
			SetGlobalShaderProperties();
		}

		private void LateUpdate()
		{
			BoxoUtils.DisableServerExecution();
			if (mainCamera == null)
			{
				mainCamera = Camera.main;
			}
			if (mainCamera != null)
			{
				focusPosition = mainCamera.transform.position;
				focusRotation = mainCamera.transform.eulerAngles;
			}
			Shader.SetGlobalFloat("TVE_MainCameraSpeedValue", Vector3.Magnitude(focusPosition) * 0.1f + (focusRotation.x + focusRotation.y) * 0.001f);
			if (propertyBlock == null)
			{
				propertyBlock = new MaterialPropertyBlock();
			}
			if (elementOrdering == TVEElementsOrdering.SortAtRuntime || sortDirty)
			{
				SortElementObjects();
			}
			if (elementVisibilityOld != elementVisibility)
			{
				SetElementsVisibility();
				elementVisibilityOld = elementVisibility;
			}
			SubmitRenderBuffers();
			ExecuteRenderBuffers(isBase: true);
			ExecuteRenderBuffers(isBase: false);
		}

		private void EnableManager()
		{
			if (Instance != null && Instance != this)
			{
				Instance.gameObject.SetActive(value: false);
			}
			Instance = this;
			Instance.name = "The Visual Engine";
			Shader.SetGlobalFloat("TVE_ManagerActive", 1f);
			Shader.SetGlobalFloat("TVE_IsEnabled", 1f);
		}

		private void DisableManager()
		{
			Instance = null;
			Shader.SetGlobalFloat("TVE_ManagerActive", 0f);
			Shader.SetGlobalFloat("TVE_IsEnabled", 0f);
			DestroyRenderBuffers();
			DisableElementsRendering();
		}

		private void SetGlobalShaderProperties()
		{
			Shader.SetGlobalFloat("TVE_RenderBaseFadeValue", 0.24975f);
			Shader.SetGlobalFloat("TVE_RenderNearFadeValue", (1f - elementRenderer.baseToNearBlend) * 0.999f);
			if (playerObject != null)
			{
				Vector3 position = playerObject.transform.position;
				Shader.SetGlobalVector("TVE_PlayerPositionRadius", new Vector4(position.x, position.y, position.z, playerRadius));
			}
			else
			{
				Shader.SetGlobalVector("TVE_PlayerPositionRadius", Vector4.zero);
			}
			if (mainLight != null)
			{
				Color linear = mainLight.color.linear;
				float intensity = mainLight.intensity;
				Color linear2 = new Color(intensity, intensity, intensity).linear;
				Color color = new Color(linear.r, linear.g, linear.b, linear2.r);
				Shader.SetGlobalVector("TVE_MainLightParams", color);
				Shader.SetGlobalVector("TVE_MainLightDirection", Vector4.Normalize(-mainLight.transform.forward));
			}
			else
			{
				Vector4 value = new Vector4(1f, 1f, 1f, 1f);
				Shader.SetGlobalVector("TVE_MainLightParams", value);
				Shader.SetGlobalVector("TVE_MainLightDirection", new Vector4(0f, 1f, 0f, 0f));
			}
			float num = 0f;
			if (seasonControl >= 0f && seasonControl < 1f)
			{
				num = seasonControl;
				Shader.SetGlobalVector("TVE_SeasonOption", new Vector4(1f, 0f, 0f, 0f));
				Shader.SetGlobalVector("TVE_SeasonParams", new Vector4(1f - num, num, 0f, 0f));
			}
			else if (seasonControl >= 1f && seasonControl < 2f)
			{
				num = seasonControl - 1f;
				Shader.SetGlobalVector("TVE_SeasonOption", new Vector4(0f, 1f, 0f, 0f));
				Shader.SetGlobalVector("TVE_SeasonParams", new Vector4(0f, 1f - num, num, 0f));
			}
			else if (seasonControl >= 2f && seasonControl < 3f)
			{
				num = seasonControl - 2f;
				Shader.SetGlobalVector("TVE_SeasonOption", new Vector4(0f, 0f, 1f, 0f));
				Shader.SetGlobalVector("TVE_SeasonParams", new Vector4(0f, 0f, 1f - num, num));
			}
			else if (seasonControl >= 3f && seasonControl <= 4f)
			{
				num = seasonControl - 3f;
				Shader.SetGlobalVector("TVE_SeasonOption", new Vector4(0f, 0f, 0f, 1f));
				Shader.SetGlobalVector("TVE_SeasonParams", new Vector4(num, 0f, 0f, 1f - num));
			}
			Shader.SetGlobalFloat("TVE_SeasonLerp", num);
			if (QualitySettings.activeColorSpace == ColorSpace.Linear)
			{
				Color color2 = Color.Lerp(Color.gray.linear, globalPaintData.tintingColor.linear, globalPaintData.tintingIntensity);
				float cutoutIntensity = globalPaintData.cutoutIntensity;
				Shader.SetGlobalVector("TVE_PaintParams", new Vector4(color2.r, color2.g, color2.b, cutoutIntensity));
			}
			else
			{
				Color color3 = Color.Lerp(Color.gray, globalPaintData.tintingColor, globalPaintData.tintingIntensity);
				float cutoutIntensity2 = globalPaintData.cutoutIntensity;
				Shader.SetGlobalVector("TVE_PaintParams", new Vector4(color3.r, color3.g, color3.b, cutoutIntensity2));
			}
			if (QualitySettings.activeColorSpace == ColorSpace.Linear)
			{
				Color color4 = globalGlowData.emissiveColor.linear * globalGlowData.emissiveIntensity;
				float subsurfaceIntensity = globalGlowData.subsurfaceIntensity;
				Shader.SetGlobalVector("TVE_GlowParams", new Vector4(color4.r, color4.g, color4.b, subsurfaceIntensity));
			}
			else
			{
				Color color5 = globalGlowData.emissiveColor * globalGlowData.emissiveIntensity;
				float subsurfaceIntensity2 = globalGlowData.subsurfaceIntensity;
				Shader.SetGlobalVector("TVE_GlowParams", new Vector4(color5.r, color5.g, color5.b, subsurfaceIntensity2));
			}
			Color color6 = new Color(0f, globalCoatData.detailIntensity, globalCoatData.layerIntensity, 0f);
			Shader.SetGlobalVector("TVE_CoatParams", color6);
			Color color7 = new Color(globalAtmoData.drynessIntensity, globalAtmoData.overlayIntensity, globalAtmoData.wetnessIntensity, globalAtmoData.raindropsIntensity);
			Shader.SetGlobalVector("TVE_AtmoParams", color7);
			Vector4 value2 = new Vector4(0.5f, 0.5f, globalFormData.confromHeight, globalFormData.sizeFadeValue);
			Shader.SetGlobalVector("TVE_FormParams", value2);
			Vector3 forward = mainWind.transform.forward;
			float num2 = Mathf.Clamp01(motionControl);
			Shader.SetGlobalVector("TVE_WindParams", new Vector4(forward.x, forward.z, num2, 1f));
			Shader.SetGlobalVector("TVE_FlowParams", new Vector4(0f, 0f, 0f, num2));
			if (useShaderMetaSettings)
			{
				Shader.SetGlobalFloat("TVE_MetaIsEnabled", 1f);
			}
			else
			{
				Shader.SetGlobalFloat("TVE_MetaIsEnabled", 0f);
			}
		}

		public void InitElementsRendering()
		{
			renderDataSet = new List<TVEElementBufferData>();
			renderElements = new List<TVEElement>();
			renderInstances = new List<TVEInstanced>();
		}

		public void DisableElementsRendering()
		{
			for (int i = 0; i < renderElements.Count; i++)
			{
				TVEElement tVEElement = renderElements[i];
				if (tVEElement != null)
				{
					tVEElement.isActive = false;
				}
			}
			for (int j = 0; j < renderInstances.Count; j++)
			{
				TVEInstanced tVEInstanced = renderInstances[j];
				for (int k = 0; k < tVEInstanced.elements.Count; k++)
				{
					if (tVEInstanced.elements[k] != null)
					{
						tVEInstanced.elements[k].isActive = false;
					}
				}
			}
			renderDataSet = new List<TVEElementBufferData>();
			renderElements = new List<TVEElement>();
			renderInstances = new List<TVEInstanced>();
		}

		public void CreateRenderData(string renderName)
		{
			TVEElementBufferData tVEElementBufferData = new TVEElementBufferData();
			if (!tVEElementBufferData.isInitialized)
			{
				tVEElementBufferData.renderMode = TVEBool.On;
				tVEElementBufferData.renderName = "Custom";
				tVEElementBufferData.textureType = TVETextureRange.HDRHalf;
				tVEElementBufferData.textureArray = TVEBool.On;
				tVEElementBufferData.isRendering = true;
				tVEElementBufferData.isInitialized = true;
			}
			string text = "TVE_" + renderName;
			tVEElementBufferData.renderName = renderName;
			tVEElementBufferData.texBaseName = text + "BaseTex";
			tVEElementBufferData.texNearName = text + "NearTex";
			tVEElementBufferData.texParams = text + "Params";
			tVEElementBufferData.texLayers = text + "Layers";
			tVEElementBufferData.renderDataID = renderName.GetHashCode();
			tVEElementBufferData.bufferSize = -1;
			renderDataSet.Add(tVEElementBufferData);
			Shader.SetGlobalFloat(text + "Active", 1f);
		}

		public void CreateRenderBuffer(TVEElementBufferData renderData)
		{
			if (renderData.renderTexBase != null)
			{
				renderData.renderTexBase.Release();
			}
			if (renderData.renderTexNear != null)
			{
				renderData.renderTexNear.Release();
			}
			if (renderData.commandBuffers != null)
			{
				for (int i = 0; i < renderData.commandBuffers.Length; i++)
				{
					renderData.commandBuffers[i].Clear();
				}
			}
			renderData.bufferUsage = new float[9];
			for (int j = 0; j < renderData.bufferUsage.Length; j++)
			{
				renderData.bufferUsage[j] = 0f;
			}
			Shader.SetGlobalFloatArray(renderData.texLayers, renderData.bufferUsage);
			if (renderData.renderMode != TVEBool.Off && renderData.bufferSize > -1)
			{
				int baseTexture = (int)elementRenderer.baseTexture;
				int nearTexture = (int)elementRenderer.nearTexture;
				RenderTextureFormat format = RenderTextureFormat.Default;
				if (renderData.textureType == TVETextureRange.HDRHalf)
				{
					format = RenderTextureFormat.ARGBHalf;
				}
				renderData.renderTexBase = new RenderTexture(baseTexture, baseTexture, 0, format, 0);
				renderData.renderTexNear = new RenderTexture(nearTexture, nearTexture, 0, format, 0);
				TextureDimension dimension = TextureDimension.Tex2D;
				if (renderData.textureArray == TVEBool.On)
				{
					dimension = TextureDimension.Tex2DArray;
				}
				renderData.renderTexBase.dimension = dimension;
				renderData.renderTexBase.volumeDepth = renderData.bufferSize + 1;
				renderData.renderTexBase.name = renderData.texBaseName;
				renderData.renderTexBase.wrapMode = TextureWrapMode.Clamp;
				renderData.renderTexBase.useMipMap = false;
				renderData.renderTexNear.dimension = dimension;
				renderData.renderTexNear.volumeDepth = renderData.bufferSize + 1;
				renderData.renderTexNear.name = renderData.texNearName;
				renderData.renderTexNear.wrapMode = TextureWrapMode.Clamp;
				renderData.renderTexNear.useMipMap = false;
				renderData.commandBuffers = new CommandBuffer[renderData.bufferSize + 1];
				for (int k = 0; k < renderData.commandBuffers.Length; k++)
				{
					renderData.commandBuffers[k] = new CommandBuffer();
					renderData.commandBuffers[k].name = "The Visual Engine/" + renderData.renderName;
				}
				Shader.SetGlobalTexture(renderData.texBaseName, renderData.renderTexBase);
				Shader.SetGlobalTexture(renderData.texNearName, renderData.renderTexNear);
			}
			else if (renderData.textureArray == TVEBool.On)
			{
				Shader.SetGlobalTexture(renderData.texBaseName, Resources.Load<Texture2DArray>("Internal ArrayTex"));
				Shader.SetGlobalTexture(renderData.texNearName, Resources.Load<Texture2DArray>("Internal ArrayTex"));
			}
			else
			{
				Shader.SetGlobalTexture(renderData.texBaseName, Texture2D.whiteTexture);
			}
		}

		private void SubmitRenderBuffers()
		{
			for (int i = 0; i < renderDataSet.Count; i++)
			{
				TVEElementBufferData tVEElementBufferData = renderDataSet[i];
				if (tVEElementBufferData == null || tVEElementBufferData.commandBuffers == null || tVEElementBufferData.renderMode == TVEBool.Off || !tVEElementBufferData.isRendering)
				{
					continue;
				}
				Vector4 globalVector = Shader.GetGlobalVector(tVEElementBufferData.texParams);
				for (int j = 0; j < tVEElementBufferData.commandBuffers.Length; j++)
				{
					tVEElementBufferData.commandBuffers[j].Clear();
					tVEElementBufferData.commandBuffers[j].ClearRenderTarget(clearDepth: true, clearColor: true, globalVector);
					tVEElementBufferData.bufferUsage[j] = 0f;
					for (int k = 0; k < renderElements.Count; k++)
					{
						TVEElement tVEElement = renderElements[k];
						if (!tVEElement.isActive || tVEElement == null)
						{
							renderElements.RemoveAt(k);
						}
						else if (tVEElementBufferData.renderDataID == tVEElement.renderDataID && tVEElement.renderLayers[j] == 1)
						{
							if (tVEElement.elementMesh == null)
							{
								Camera.SetupCurrent(mainCamera);
							}
							propertyBlock.SetVector("_ElementParams", tVEElement.elementParams);
							tVEElement.elementRenderer.SetPropertyBlock(propertyBlock);
							tVEElementBufferData.commandBuffers[j].DrawRenderer(tVEElement.elementRenderer, tVEElement.elementMaterial, 0, tVEElement.renderPass);
							tVEElementBufferData.bufferUsage[j] = 1f;
						}
					}
					for (int l = 0; l < renderInstances.Count; l++)
					{
						TVEInstanced tVEInstanced = renderInstances[l];
						if (tVEInstanced.material == null || !tVEInstanced.material.enableInstancing || tVEInstanced.elements.Count == 0 || tVEElementBufferData.renderDataID != tVEInstanced.renderDataID || tVEInstanced.renderLayers[j] != 1)
						{
							continue;
						}
						for (int m = 0; m < tVEInstanced.elements.Count; m++)
						{
							if (!tVEInstanced.elements[m].isActive || tVEInstanced.elements[m] == null || tVEInstanced.renderers[m] == null)
							{
								tVEInstanced.elements.RemoveAt(m);
								tVEInstanced.renderers.RemoveAt(m);
							}
						}
						int count = tVEInstanced.elements.Count;
						if (count == 0)
						{
							continue;
						}
						if (tVEInstanced.matrices == null || tVEInstanced.matrices.Length != count)
						{
							tVEInstanced.matrices = new Matrix4x4[count];
							tVEInstanced.parameters = new Vector4[count];
						}
						for (int n = 0; n < count; n++)
						{
							if (tVEInstanced.renderers[n] != null)
							{
								tVEInstanced.matrices[n] = tVEInstanced.renderers[n].localToWorldMatrix;
								tVEInstanced.parameters[n] = tVEInstanced.elements[n].elementParams;
							}
						}
						if (tVEInstanced.propertyBlockCount != count)
						{
							if (tVEInstanced.propertyBlock == null)
							{
								tVEInstanced.propertyBlock = new MaterialPropertyBlock();
							}
							else
							{
								tVEInstanced.propertyBlock.Clear();
							}
							tVEInstanced.propertyBlockCount = count;
						}
						tVEInstanced.propertyBlock.SetVectorArray("_ElementParams", tVEInstanced.parameters);
						tVEElementBufferData.commandBuffers[j].DrawMeshInstanced(tVEInstanced.mesh, 0, tVEInstanced.material, tVEInstanced.renderPass, tVEInstanced.matrices, count, tVEInstanced.propertyBlock);
						tVEElementBufferData.bufferUsage[j] = 1f;
					}
				}
				Shader.SetGlobalFloatArray(tVEElementBufferData.texLayers, tVEElementBufferData.bufferUsage);
			}
		}

		private void ExecuteRenderBuffers(bool isBase)
		{
			GL.PushMatrix();
			RenderTexture active = RenderTexture.active;
			Vector3 vector = Vector3.zero;
			if (isBase)
			{
				if (elementRenderer.baseCenter == null)
				{
					if (mainCamera != null)
					{
						vector = mainCamera.transform.position;
					}
				}
				else
				{
					vector = elementRenderer.baseCenter.position;
				}
			}
			else if (elementRenderer.nearCenter == null)
			{
				if (mainCamera != null)
				{
					vector = mainCamera.transform.position;
				}
			}
			else
			{
				vector = elementRenderer.nearCenter.position;
			}
			Vector3 one = Vector3.one;
			one = ((!isBase) ? new Vector3(elementRenderer.nearRadius * 2f, 100000f, elementRenderer.nearRadius * 2f) : new Vector3(elementRenderer.baseRadius * 2f, 100000f, elementRenderer.baseRadius * 2f));
			int num = 32;
			num = (int)((!isBase) ? elementRenderer.nearTexture : elementRenderer.baseTexture);
			float num2 = one.x / (float)num;
			float num3 = one.z / (float)num;
			float x = Mathf.Round(vector.x / num2) * num2;
			float z = Mathf.Round(vector.z / num3) * num3;
			vector = new Vector3(x, vector.y, z);
			float num4 = 1f / one.x;
			float num5 = 1f / one.z;
			float num6 = num4 * vector.x - 0.5f;
			float num7 = num5 * vector.z - 0.5f;
			Vector4 value = new Vector4(num4, num5, 0f - num6, 0f - num7);
			if (isBase)
			{
				Shader.SetGlobalVector("TVE_RenderBaseCoords", value);
				Shader.SetGlobalVector("TVE_RenderBasePositionR", new Vector4(vector.x, vector.y, vector.z, elementRenderer.baseRadius));
			}
			else
			{
				Shader.SetGlobalVector("TVE_RenderNearCoords", value);
				Shader.SetGlobalVector("TVE_RenderNearPositionR", new Vector4(vector.x, vector.y, vector.z, elementRenderer.nearRadius));
			}
			GL.modelview = modelViewMatrix;
			projectionMatrix = Matrix4x4.Ortho((0f - one.x) / 2f + vector.x, one.x / 2f + vector.x, one.z / 2f - vector.z, (0f - one.z) / 2f - vector.z, (0f - one.y) / 2f + vector.y, one.y / 2f + vector.y);
			GL.LoadProjectionMatrix(projectionMatrix);
			for (int i = 0; i < renderDataSet.Count; i++)
			{
				TVEElementBufferData tVEElementBufferData = renderDataSet[i];
				if (tVEElementBufferData == null || tVEElementBufferData.commandBuffers == null || tVEElementBufferData.renderMode == TVEBool.Off || !tVEElementBufferData.isRendering)
				{
					continue;
				}
				for (int j = 0; j < tVEElementBufferData.commandBuffers.Length; j++)
				{
					if (isBase)
					{
						Graphics.SetRenderTarget(tVEElementBufferData.renderTexBase, 0, CubemapFace.Unknown, j);
					}
					else
					{
						Graphics.SetRenderTarget(tVEElementBufferData.renderTexNear, 0, CubemapFace.Unknown, j);
					}
					Graphics.ExecuteCommandBuffer(tVEElementBufferData.commandBuffers[j]);
				}
			}
			RenderTexture.active = active;
			GL.PopMatrix();
		}

		private void DestroyRenderBuffers()
		{
			for (int i = 0; i < renderDataSet.Count; i++)
			{
				TVEElementBufferData tVEElementBufferData = renderDataSet[i];
				if (tVEElementBufferData == null)
				{
					continue;
				}
				if (tVEElementBufferData.renderTexBase != null)
				{
					tVEElementBufferData.renderTexBase.Release();
				}
				if (tVEElementBufferData.renderTexNear != null)
				{
					tVEElementBufferData.renderTexNear.Release();
				}
				if (tVEElementBufferData.commandBuffers != null)
				{
					for (int j = 0; j < tVEElementBufferData.commandBuffers.Length; j++)
					{
						tVEElementBufferData.commandBuffers[j].Clear();
						tVEElementBufferData.bufferUsage[j] = 0f;
					}
				}
				Shader.SetGlobalFloatArray(tVEElementBufferData.texLayers, tVEElementBufferData.bufferUsage);
			}
		}

		public void MarkSortDirty()
		{
			sortDirty = true;
		}

		public void SortElementObjects()
		{
			renderElements.Sort(delegate(TVEElement e1, TVEElement e2)
			{
				if (e1 == null && e2 == null)
				{
					return 0;
				}
				if (e1 == null)
				{
					return -1;
				}
				return (e2 == null) ? 1 : e1.transform.position.y.CompareTo(e2.transform.position.y);
			});
			sortDirty = false;
		}

		public void SetElementsRendering(string renderName, bool isRendering)
		{
			for (int i = 0; i < renderDataSet.Count; i++)
			{
				if (renderDataSet[i].renderName == renderName)
				{
					renderDataSet[i].isRendering = isRendering;
				}
			}
		}

		private void SetElementsVisibility()
		{
			if (elementVisibility == TVEElementsVisibility.AlwaysHidden)
			{
				DisableElementsVisibility();
			}
			else if (elementVisibility == TVEElementsVisibility.AlwaysVisible)
			{
				EnableElementsVisibility();
			}
			else if (elementVisibility == TVEElementsVisibility.HiddenAtRuntime)
			{
				if (Application.isPlaying)
				{
					DisableElementsVisibility();
				}
				else
				{
					EnableElementsVisibility();
				}
			}
		}

		private void EnableElementsVisibility()
		{
			for (int i = 0; i < renderElements.Count; i++)
			{
				TVEElement tVEElement = renderElements[i];
				if (tVEElement != null && tVEElement.customVisibility == TVEElementVisibility.UseGlobalSettings)
				{
					tVEElement.elementRenderer.enabled = true;
				}
			}
		}

		private void DisableElementsVisibility()
		{
			for (int i = 0; i < renderElements.Count; i++)
			{
				TVEElement tVEElement = renderElements[i];
				if (tVEElement != null && tVEElement.customVisibility == TVEElementVisibility.UseGlobalSettings)
				{
					tVEElement.elementRenderer.enabled = false;
				}
			}
		}

		private void SetGlobalLightingMainLight()
		{
			Light[] array = UnityEngine.Object.FindObjectsByType<Light>(FindObjectsSortMode.None);
			float num = 0f;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].type == LightType.Directional && array[i].intensity > num)
				{
					mainLight = array[i];
				}
			}
		}
	}
}
