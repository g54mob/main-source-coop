using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using NomadDrive.Features.Interaction;
using UnityEngine;
using UnityEngine.Rendering;

namespace NomadDrive.Features.ObjectPlacement
{
	public class DropIndicatorController : MonoBehaviour
	{
		private DropIndicatorSettings _settings;

		private LineRenderer _lineRenderer;

		private GameObject _ghostPreview;

		private HighlightModel _highlightModel;

		private List<Renderer> _ghostRenderers = new List<Renderer>();

		private MaterialPropertyBlock _linePropertyBlock;

		private MaterialPropertyBlock _ghostPropertyBlock;

		private Transform _sourceTransform;

		private Transform _modelTransform;

		private Vector3 _boundsCenterLocalOffset;

		private GhostPreviewMode _ghostPreviewMode;

		private Vector3 _ghostPreviewScale;

		private bool _isVisible;

		private bool _isColliding;

		private bool _isInitialized;

		private bool _hasAppliedColors;

		private bool _lastColliding;

		private bool _lastSnapping;

		private static readonly int ColorId = Shader.PropertyToID("_Color");

		private static readonly int DashLengthId = Shader.PropertyToID("_DashLength");

		private static readonly int GapLengthId = Shader.PropertyToID("_GapLength");

		private static readonly int ScrollSpeedId = Shader.PropertyToID("_ScrollSpeed");

		private static readonly int FresnelPowerId = Shader.PropertyToID("_FresnelPower");

		private static readonly int FresnelIntensityId = Shader.PropertyToID("_FresnelIntensity");

		private static readonly int TileCountId = Shader.PropertyToID("_TileCount");

		public void Initialize(DropIndicatorSettings settings, Interactable interactable, Vector3 boundsCenterLocalOffset, GhostPreviewMode ghostPreviewMode, Vector3 ghostPreviewScale)
		{
			if (settings == null)
			{
				EvilLogger.LogError("[DropIndicatorController] Settings cannot be null!", "Initialize", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\ObjectPlacement\\DropIndicatorController.cs", 59);
				return;
			}
			_settings = settings;
			_sourceTransform = interactable.transform;
			_boundsCenterLocalOffset = boundsCenterLocalOffset;
			_ghostPreviewMode = ghostPreviewMode;
			_ghostPreviewScale = ghostPreviewScale;
			_modelTransform = ((interactable.ModelTransform != null) ? interactable.ModelTransform : interactable.transform);
			_linePropertyBlock = new MaterialPropertyBlock();
			_ghostPropertyBlock = new MaterialPropertyBlock();
			CreateLineRenderer();
			CreateGhostPreview(interactable);
			_isInitialized = true;
			Hide();
		}

		private void CreateLineRenderer()
		{
			if (!(_settings.dashedLineMaterial == null))
			{
				GameObject gameObject = new GameObject("DropIndicatorLine");
				gameObject.transform.SetParent(base.transform);
				_lineRenderer = gameObject.AddComponent<LineRenderer>();
				_lineRenderer.material = new Material(_settings.dashedLineMaterial);
				_lineRenderer.startWidth = _settings.lineWidth;
				_lineRenderer.endWidth = _settings.lineWidth;
				_lineRenderer.positionCount = 2;
				_lineRenderer.useWorldSpace = true;
				_lineRenderer.textureMode = LineTextureMode.Tile;
				_lineRenderer.generateLightingData = false;
				_lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
				_lineRenderer.receiveShadows = false;
				_lineRenderer.allowOcclusionWhenDynamic = false;
				ApplyLineShaderProperties();
			}
		}

		private void ApplyLineShaderProperties()
		{
			if (!(_lineRenderer == null) && !(_lineRenderer.material == null))
			{
				_lineRenderer.material.SetFloat(DashLengthId, _settings.dashLength);
				_lineRenderer.material.SetFloat(GapLengthId, _settings.gapLength);
				_lineRenderer.material.SetFloat(ScrollSpeedId, _settings.scrollSpeed);
				_lineRenderer.material.SetColor(ColorId, _settings.lineColor);
			}
		}

		private void CreateGhostPreview(Interactable interactable)
		{
			if (!(_settings.ghostMaterial == null))
			{
				if (_ghostPreviewMode == GhostPreviewMode.FootprintPlane)
				{
					_ghostPreview = CreateFootprintGhost();
				}
				else
				{
					Transform source = ((interactable.ModelTransform != null) ? interactable.ModelTransform : interactable.transform);
					_highlightModel = HighlightModelFactory.CreateFrom(source, base.transform);
					_ghostPreview = _highlightModel.Root;
				}
				_ghostPreview.name = "GhostPreview";
				_ghostPreview.transform.SetParent(base.transform);
				CollectAndApplyGhostMaterial();
			}
		}

		private GameObject CreateFootprintGhost()
		{
			GameObject gameObject = new GameObject("GhostRoot");
			GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			obj.name = "FootprintCube";
			obj.transform.SetParent(gameObject.transform);
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.localScale = _ghostPreviewScale;
			Collider component = obj.GetComponent<Collider>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			MeshRenderer component2 = obj.GetComponent<MeshRenderer>();
			component2.shadowCastingMode = ShadowCastingMode.Off;
			component2.receiveShadows = false;
			return gameObject;
		}

		private void CollectAndApplyGhostMaterial()
		{
			_ghostRenderers.Clear();
			Renderer[] componentsInChildren = _ghostPreview.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				if (!(renderer is LineRenderer) && !(renderer is TrailRenderer) && !(renderer is ParticleSystemRenderer))
				{
					_ghostRenderers.Add(renderer);
					Material[] array = new Material[renderer.sharedMaterials.Length];
					for (int j = 0; j < array.Length; j++)
					{
						array[j] = new Material(_settings.ghostMaterial);
					}
					renderer.materials = array;
				}
			}
			ApplyGhostShaderProperties(_settings.ghostColor);
		}

		private void ApplyGhostShaderProperties(Color color)
		{
			foreach (Renderer ghostRenderer in _ghostRenderers)
			{
				if (!(ghostRenderer == null))
				{
					ghostRenderer.GetPropertyBlock(_ghostPropertyBlock);
					_ghostPropertyBlock.SetColor(ColorId, color);
					_ghostPropertyBlock.SetFloat(FresnelPowerId, _settings.fresnelPower);
					_ghostPropertyBlock.SetFloat(FresnelIntensityId, _settings.fresnelIntensity);
					ghostRenderer.SetPropertyBlock(_ghostPropertyBlock);
				}
			}
		}

		public void Show()
		{
			if (_isInitialized)
			{
				_isVisible = true;
				if (_lineRenderer != null)
				{
					_lineRenderer.enabled = true;
				}
				if (_ghostPreview != null)
				{
					_ghostPreview.SetActive(value: true);
				}
			}
		}

		public void SetColliding(bool isColliding)
		{
			_isColliding = isColliding;
		}

		public void Hide()
		{
			_isVisible = false;
			if (_lineRenderer != null)
			{
				_lineRenderer.enabled = false;
			}
			if (_ghostPreview != null)
			{
				_ghostPreview.SetActive(value: false);
			}
		}

		public void UpdateIndicator(DropResult dropResult)
		{
			if (!_isInitialized)
			{
				return;
			}
			if (!dropResult.Success)
			{
				Hide();
				return;
			}
			if (!_isVisible)
			{
				Show();
			}
			UpdateLinePositions(dropResult);
			UpdateGhostTransform(dropResult);
			UpdateColors(dropResult.SnappingPlane != null);
		}

		private void UpdateLinePositions(DropResult dropResult)
		{
			if (!(_lineRenderer == null) && !(_sourceTransform == null))
			{
				Vector3 vector = _sourceTransform.TransformPoint(_boundsCenterLocalOffset);
				Vector3 vector2 = ((_ghostPreviewMode != GhostPreviewMode.FootprintPlane) ? (dropResult.TargetPosition + dropResult.TargetRotation * Vector3.Scale(_boundsCenterLocalOffset, _sourceTransform.lossyScale)) : (dropResult.HitInfo.point + dropResult.HitInfo.normal * 0.005f));
				_lineRenderer.SetPosition(0, vector);
				_lineRenderer.SetPosition(1, vector2);
				float num = Vector3.Distance(vector, vector2);
				float num2 = _settings.dashLength + _settings.gapLength;
				float value = num / num2;
				_lineRenderer.material.SetFloat(TileCountId, value);
			}
		}

		private void UpdateGhostTransform(DropResult dropResult)
		{
			if (!(_ghostPreview == null) && !(_sourceTransform == null) && !(_modelTransform == null))
			{
				if (_ghostPreviewMode == GhostPreviewMode.FootprintPlane)
				{
					Vector3 normal = dropResult.HitInfo.normal;
					float y = _sourceTransform.eulerAngles.y;
					Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal) * Quaternion.Euler(0f, y, 0f);
					Vector3 position = dropResult.HitInfo.point + normal * 0.005f;
					_ghostPreview.transform.SetPositionAndRotation(position, rotation);
				}
				else
				{
					Quaternion quaternion = Quaternion.Inverse(_sourceTransform.rotation) * _modelTransform.rotation;
					Quaternion rotation2 = dropResult.TargetRotation * quaternion;
					Vector3 vector = _sourceTransform.InverseTransformPoint(_modelTransform.position);
					Vector3 position2 = dropResult.TargetPosition + dropResult.TargetRotation * vector;
					_ghostPreview.transform.SetPositionAndRotation(position2, rotation2);
				}
			}
		}

		private void UpdateColors(bool isSnapping)
		{
			if (!_hasAppliedColors || _lastColliding != _isColliding || _lastSnapping != isSnapping)
			{
				_hasAppliedColors = true;
				_lastColliding = _isColliding;
				_lastSnapping = isSnapping;
				Color value = (_isColliding ? _settings.lineColorCollision : (isSnapping ? _settings.lineColorSnapping : _settings.lineColor));
				Color color = (_isColliding ? _settings.ghostColorCollision : (isSnapping ? _settings.ghostColorSnapping : _settings.ghostColor));
				if (_lineRenderer != null && _lineRenderer.material != null)
				{
					_lineRenderer.material.SetColor(ColorId, value);
				}
				ApplyGhostShaderProperties(color);
			}
		}

		private void OnDestroy()
		{
			if (_lineRenderer != null && _lineRenderer.material != null)
			{
				Object.Destroy(_lineRenderer.material);
			}
			foreach (Renderer ghostRenderer in _ghostRenderers)
			{
				if (ghostRenderer == null)
				{
					continue;
				}
				Material[] materials = ghostRenderer.materials;
				foreach (Material material in materials)
				{
					if (material != null)
					{
						Object.Destroy(material);
					}
				}
			}
			if (_ghostPreview != null)
			{
				Object.Destroy(_ghostPreview);
			}
		}
	}
}
