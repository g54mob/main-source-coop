using System;
using System.Collections.Generic;
using Boxophobic.StyledGUI;
using Boxophobic.Utility;
using UnityEngine;

namespace TheVisualEngine
{
	[HelpURL("https://docs.google.com/document/d/145JOVlJ1tE-WODW45YoJ6Ixg23mFc56EnB_8Tbwloz8/edit#heading=h.fd5y8rbb7aia")]
	[ExecuteInEditMode]
	[AddComponentMenu("BOXOPHOBIC/The Visual Engine/TVE Element")]
	public class TVEElement : StyledMonoBehaviour
	{
		[Tooltip("Sets the element visibility.")]
		public TVEElementVisibility customVisibility = TVEElementVisibility.UseGlobalSettings;

		[Tooltip("Sets a custom material for element rendering.")]
		public Material customMaterial;

		[Space(10f)]
		[Tooltip("Sets the terrain height or splat map as element textures.")]
		public Terrain terrainData;

		public TVETerrainTexture terrainMask = TVETerrainTexture.Auto;

		[HideInInspector]
		public TVEElementMaterialData materialData;

		[NonSerialized]
		public Renderer elementRenderer;

		[NonSerialized]
		public Material elementMaterial;

		[NonSerialized]
		public Mesh elementMesh;

		[NonSerialized]
		public Vector4 elementParams = Vector4.one;

		[NonSerialized]
		public int elementID;

		[NonSerialized]
		public int instancedID;

		[NonSerialized]
		public string renderName = "";

		[NonSerialized]
		public int renderDataID;

		[NonSerialized]
		public List<int> renderLayers;

		[NonSerialized]
		public int renderPass;

		[NonSerialized]
		public bool isActive;

		private ParticleSystem particleSystem;

		private int useVertexColorDirection;

		private int useRaycastFading;

		private Vector3 lastPosition;

		private LayerMask raycastMask;

		private float raycastStart;

		private float raycastLimit;

		private float raycastDistance;

		private float speedTreshold = 10f;

		private bool isSelected;

		private void OnEnable()
		{
			BoxoUtils.DisableServerExecution();
			particleSystem = base.gameObject.GetComponent<ParticleSystem>();
			elementRenderer = base.gameObject.GetComponent<Renderer>();
			if (customMaterial != null)
			{
				elementMaterial = customMaterial;
			}
			else
			{
				elementMaterial = elementRenderer.sharedMaterial;
			}
			if (elementMaterial == null || elementMaterial.name == "Element")
			{
				if (materialData == null)
				{
					materialData = new TVEElementMaterialData();
				}
				if (!(materialData.shader == null))
				{
					elementMaterial = new Material(materialData.shader);
					LoadMaterialData(elementMaterial);
				}
				elementMaterial.name = "Element";
				base.gameObject.GetComponent<Renderer>().sharedMaterial = elementMaterial;
			}
		}

		private void OnDestroy()
		{
			BoxoUtils.DisableServerExecution();
			isActive = false;
		}

		private void OnDisable()
		{
			BoxoUtils.DisableServerExecution();
			isActive = false;
		}

		private void Update()
		{
			BoxoUtils.DisableServerExecution();
			if (TVEManager.Instance == null)
			{
				isActive = false;
				return;
			}
			if (!isActive)
			{
				UpdateElement();
				isActive = true;
			}
			UpdateFading();
		}

		private void UpdateElement()
		{
			elementID = GetHashCode();
			if (customMaterial != null)
			{
				elementMaterial = customMaterial;
			}
			else
			{
				elementMaterial = elementRenderer.sharedMaterial;
			}
			if (elementMaterial != null)
			{
				TVEUtils.SetElementSettings(elementMaterial);
				TVEUtils.CopyTerrainDataToElement(terrainData, terrainMask, elementMaterial);
				renderName = elementMaterial.GetTag("ElementType", searchFallbacks: false);
				renderDataID = renderName.GetHashCode();
				GetMaterialParameters();
			}
			MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
			if (component != null)
			{
				elementMesh = component.sharedMesh;
			}
			if (elementMesh != null && elementMaterial.enableInstancing)
			{
				instancedID = elementMesh.GetHashCode() + elementMaterial.GetHashCode();
			}
			AddElementToVolume();
			SetElementVisibility();
		}

		private void UpdateFading()
		{
			if (particleSystem != null)
			{
				ParticleSystem.MainModule main = particleSystem.main;
				Color color = main.startColor.color;
				if (useVertexColorDirection > 0)
				{
					Vector3 direction = base.transform.position - lastPosition;
					Vector3 vector = base.transform.InverseTransformDirection(direction);
					Vector3 vector2 = base.transform.TransformVector(vector);
					lastPosition = base.transform.position;
					float r = Mathf.Clamp(vector2.x * speedTreshold, -1f, 1f) * 0.5f + 0.5f;
					float g = Mathf.Clamp(vector2.z * speedTreshold, -1f, 1f) * 0.5f + 0.5f;
					color = new Color(r, g, 0f, 1f);
				}
				if (useRaycastFading > 0)
				{
					float racastFading = GetRacastFading();
					color = new Color(color.r, color.g, color.b, racastFading);
				}
				main.startColor = color;
			}
			else if (useRaycastFading > 0)
			{
				float racastFading2 = GetRacastFading();
				elementParams.w = racastFading2;
			}
			else
			{
				elementParams.w = 1f;
			}
		}

		private void AddElementToVolume()
		{
			List<TVEElementBufferData> renderDataSet = TVEManager.Instance.renderDataSet;
			List<TVEElement> renderElements = TVEManager.Instance.renderElements;
			List<TVEInstanced> renderInstances = TVEManager.Instance.renderInstances;
			bool flag = false;
			for (int i = 0; i < renderDataSet.Count; i++)
			{
				if (renderDataSet[i].renderDataID == renderDataID)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				TVEManager.Instance.CreateRenderData(renderName);
			}
			for (int j = 0; j < renderDataSet.Count; j++)
			{
				TVEElementBufferData tVEElementBufferData = renderDataSet[j];
				if (tVEElementBufferData == null || tVEElementBufferData.renderDataID != renderDataID)
				{
					continue;
				}
				int num = 0;
				renderLayers = new List<int>(9);
				if (tVEElementBufferData.textureArray == TVEBool.On && elementMaterial.HasProperty("_ElementLayerMask"))
				{
					int num2 = elementMaterial.GetInt("_ElementLayerMask");
					for (int k = 0; k < 9; k++)
					{
						if (((1 << k) & num2) != 0)
						{
							renderLayers.Add(1);
							num = k;
						}
						else
						{
							renderLayers.Add(0);
						}
					}
				}
				else
				{
					renderLayers.Add(1);
					for (int l = 1; l < 9; l++)
					{
						renderLayers.Add(0);
					}
				}
				if (num > tVEElementBufferData.bufferSize)
				{
					tVEElementBufferData.bufferSize = num;
					TVEManager.Instance.CreateRenderBuffer(tVEElementBufferData);
				}
				if (Application.isPlaying && SystemInfo.supportsInstancing)
				{
					if (instancedID == 0)
					{
						bool flag2 = false;
						for (int m = 0; m < renderElements.Count; m++)
						{
							if (renderElements[m].elementID == elementID)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							renderElements.Add(this);
						}
						continue;
					}
					bool flag3 = false;
					int index = 0;
					for (int n = 0; n < renderInstances.Count; n++)
					{
						if (renderInstances[n].renderers.Count <= 1022 && renderInstances[n].instancedDataID == instancedID)
						{
							flag3 = true;
							index = n;
							break;
						}
					}
					if (!flag3)
					{
						TVEInstanced tVEInstanced = new TVEInstanced();
						tVEInstanced.instancedDataID = instancedID;
						tVEInstanced.renderDataID = renderDataID;
						tVEInstanced.renderLayers = renderLayers;
						tVEInstanced.renderPass = renderPass;
						tVEInstanced.material = elementMaterial;
						tVEInstanced.mesh = elementMesh;
						tVEInstanced.elements.Add(this);
						tVEInstanced.renderers.Add(elementRenderer);
						renderInstances.Add(tVEInstanced);
						continue;
					}
					bool flag4 = false;
					for (int num3 = 0; num3 < renderInstances[index].renderers.Count; num3++)
					{
						if (renderInstances[index].renderers[num3] == elementRenderer)
						{
							flag4 = true;
							break;
						}
					}
					if (!flag4)
					{
						renderInstances[index].elements.Add(this);
						renderInstances[index].renderers.Add(elementRenderer);
					}
					continue;
				}
				bool flag5 = false;
				for (int num4 = 0; num4 < renderElements.Count; num4++)
				{
					if (renderElements[num4].elementID == elementID)
					{
						flag5 = true;
						break;
					}
				}
				if (!flag5)
				{
					renderElements.Add(this);
				}
			}
		}

		private void SetElementVisibility()
		{
			if (customVisibility == TVEElementVisibility.UseGlobalSettings)
			{
				TVEElementsVisibility elementVisibility = TVEManager.Instance.elementVisibility;
				if (elementVisibility == TVEElementsVisibility.AlwaysHidden)
				{
					elementRenderer.enabled = false;
				}
				if (elementVisibility == TVEElementsVisibility.AlwaysVisible)
				{
					elementRenderer.enabled = true;
				}
				if (elementVisibility == TVEElementsVisibility.HiddenAtRuntime)
				{
					if (Application.isPlaying)
					{
						elementRenderer.enabled = false;
					}
					else
					{
						elementRenderer.enabled = true;
					}
				}
				return;
			}
			if (customVisibility == TVEElementVisibility.AlwaysHidden)
			{
				elementRenderer.enabled = false;
			}
			if (customVisibility == TVEElementVisibility.AlwaysVisible)
			{
				elementRenderer.enabled = true;
			}
			if (customVisibility == TVEElementVisibility.HiddenAtRuntime)
			{
				if (Application.isPlaying)
				{
					elementRenderer.enabled = false;
				}
				else
				{
					elementRenderer.enabled = true;
				}
			}
		}

		private void LoadMaterialData(Material material)
		{
			material.shader = materialData.shader;
			for (int i = 0; i < materialData.props.Count; i++)
			{
				if (materialData.props[i].type == TVEPropertyType.Texture)
				{
					material.SetTexture(materialData.props[i].prop, materialData.props[i].texture);
				}
				if (materialData.props[i].type == TVEPropertyType.Vector)
				{
					material.SetVector(materialData.props[i].prop, materialData.props[i].vector);
				}
				if (materialData.props[i].type == TVEPropertyType.Value)
				{
					material.SetFloat(materialData.props[i].prop, materialData.props[i].value);
				}
			}
		}

		private void GetMaterialParameters()
		{
			if (elementMaterial.HasProperty("_ElementPassValue"))
			{
				renderPass = elementMaterial.GetInt("_ElementPassValue");
			}
			if (elementMaterial.HasProperty("_MotionDirectionMode"))
			{
				if (elementMaterial.GetInt("_MotionDirectionMode") == 2)
				{
					useVertexColorDirection = 1;
				}
				else
				{
					useVertexColorDirection = 0;
				}
			}
			if (elementMaterial.HasProperty("_SpeedTresholdValue"))
			{
				speedTreshold = elementMaterial.GetFloat("_SpeedTresholdValue");
			}
			if (elementMaterial.HasProperty("_ElementRaycastMode"))
			{
				useRaycastFading = elementMaterial.GetInt("_ElementRaycastMode");
				raycastMask = elementMaterial.GetInt("_RaycastLayerMask");
				raycastStart = elementMaterial.GetFloat("_RaycastDistanceMinValue");
				raycastLimit = elementMaterial.GetFloat("_RaycastDistanceMaxValue");
				raycastDistance = elementMaterial.GetFloat("_RaycastDistanceCheckValue");
			}
		}

		private float GetRacastFading()
		{
			float maxDistance = raycastLimit;
			if (raycastDistance > 0f)
			{
				maxDistance = raycastDistance;
			}
			if (Physics.Raycast(base.transform.position, -Vector3.up, out var hitInfo, maxDistance, raycastMask))
			{
				return 1f - Mathf.Clamp01(BoxoUtils.MathRemap(hitInfo.distance, raycastStart, raycastLimit));
			}
			return 1f;
		}

		private void OnDrawGizmosSelected()
		{
			DrawGizmos(selected: true);
		}

		private void OnDrawGizmos()
		{
			DrawGizmos(selected: false);
		}

		private void DrawGizmos(bool selected)
		{
			if (!(TVEManager.Instance == null) && isActive)
			{
				Color color = new Color(0f, 0f, 0f, 0.1f);
				if (selected)
				{
					color = new Color(0f, 0f, 0f, 1f);
				}
				Gizmos.color = color;
				if (isSelected && useRaycastFading > 0)
				{
					Gizmos.DrawLine(base.transform.position, new Vector3(base.transform.position.x, base.transform.position.y - raycastLimit, base.transform.position.z));
				}
				Bounds bounds;
				if (elementMesh != null)
				{
					bounds = elementMesh.bounds;
					Gizmos.matrix = base.transform.localToWorldMatrix;
				}
				else
				{
					bounds = elementRenderer.bounds;
				}
				Gizmos.DrawWireCube(bounds.center, bounds.size);
			}
		}
	}
}
