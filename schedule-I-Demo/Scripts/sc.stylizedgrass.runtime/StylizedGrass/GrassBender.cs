using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace StylizedGrass
{
	[ExecuteInEditMode]
	[AddComponentMenu("Stylized Grass/Grass Bender")]
	[HelpURL("http://staggart.xyz/unity/stylized-grass-shader/sgs-docs/?section=using-grass-bending")]
	public class GrassBender : MonoBehaviour
	{
		[Tooltip("Higher layers are always drawn over lower layers. Use this to override other benders on a lower layer.")]
		[Range(0f, 4f)]
		public int sortingLayer;

		public Renderer renderer;

		[Range(-1f, 1f)]
		public float heightOffset;

		[FormerlySerializedAs("strength")]
		[Range(0f, 3f)]
		public float flattenStrength = 1f;

		[Range(0f, 3f)]
		public float pushStrength = 1f;

		[Range(0f, 3f)]
		public float scaleMultiplier = 1f;

		[Tooltip("When enabled, overlapping benders of the same type will blend together")]
		public bool alphaBlending;

		public MeshFilter meshFilter;

		public TrailRenderer trailRenderer;

		[Tooltip("Jitter the position of the trail, to force the mesh to constantly update")]
		public bool forceUpdating;

		public LineRenderer lineRenderer;

		private MaterialPropertyBlock _props;

		private Material material;

		[SerializeField]
		private Shader bendingShader;

		private readonly int paramsID = Shader.PropertyToID("_Params");

		private readonly int _SrcFactor = Shader.PropertyToID("_SrcFactor");

		private readonly int _DstFactor = Shader.PropertyToID("_DstFactor");

		private Vector3 targetPosition;

		private static Vector3 TrailRotation = new Vector3(-90f, 0f, 0f);

		private const string TRAIL_KEYWORD = "_TRAIL";

		public const string MESH_SHADER_OPAQUE_NAME = "Hidden/Nature/Grass Bend Mesh";

		private static Material _TrailMaterial;

		private static Material _MeshMaterial;

		public MaterialPropertyBlock props
		{
			get
			{
				if (_props == null)
				{
					_props = new MaterialPropertyBlock();
					renderer.GetPropertyBlock(_props);
				}
				return _props;
			}
		}

		private static Material TrailMaterial
		{
			get
			{
				if (!_TrailMaterial)
				{
					_TrailMaterial = new Material(Shader.Find("Hidden/Nature/Grass Bend Mesh"));
					_TrailMaterial.name = "TrailOrLineBender";
					_TrailMaterial.EnableKeyword("_TRAIL");
					_TrailMaterial.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
				}
				return _TrailMaterial;
			}
		}

		private static Material MeshMaterial
		{
			get
			{
				if (!_MeshMaterial)
				{
					_MeshMaterial = new Material(Shader.Find("Hidden/Nature/Grass Bend Mesh"));
					_MeshMaterial.name = "MeshOrParticleBender";
					_MeshMaterial.DisableKeyword("_TRAIL");
					_MeshMaterial.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
				}
				return _MeshMaterial;
			}
		}

		private void Reset()
		{
			bendingShader = Shader.Find("Hidden/Nature/Grass Bend Mesh");
		}

		public void OnEnable()
		{
			GetRenderer();
			if ((bool)trailRenderer)
			{
				trailRenderer.forceRenderingOff = false;
				trailRenderer.emitting = true;
				trailRenderer.generateLightingData = true;
			}
			if ((bool)lineRenderer)
			{
				lineRenderer.forceRenderingOff = false;
				lineRenderer.generateLightingData = true;
			}
			bendingShader = Shader.Find("Hidden/Nature/Grass Bend Mesh");
			SetupMaterial();
			UpdateProperties();
		}

		private void SetupMaterial()
		{
			if ((bool)renderer)
			{
				Material source = GetMaterial(renderer.GetType());
				if (GrassBendingFeature.SRPBatcherEnabled())
				{
					material = new Material(source);
					material.hideFlags = HideFlags.DontSave;
					material.enableInstancing = false;
					material.name += "(Instance)";
					renderer.material = material;
				}
				else
				{
					material = source;
					material.enableInstancing = true;
					renderer.sharedMaterial = material;
				}
			}
		}

		public void UpdateProperties()
		{
			if (!renderer)
			{
				GetRenderer();
			}
			if (!material)
			{
				SetupMaterial();
			}
			if (GrassBendingFeature.SRPBatcherEnabled())
			{
				material.SetVector(paramsID, new Vector4(flattenStrength, heightOffset, pushStrength, scaleMultiplier));
				material.renderQueue = (alphaBlending ? 3000 : 2000) + sortingLayer;
				material.SetFloat(_SrcFactor, (!alphaBlending) ? 1 : 5);
				material.SetFloat(_DstFactor, alphaBlending ? 10 : 0);
				renderer.SetPropertyBlock(null);
			}
			else
			{
				props.SetVector(paramsID, new Vector4(flattenStrength, heightOffset, pushStrength, scaleMultiplier));
				renderer.SetPropertyBlock(props);
			}
		}

		public void GetRenderer()
		{
			renderer = GetComponent<Renderer>();
			Type type = renderer.GetType();
			if (type == typeof(MeshRenderer))
			{
				meshFilter = renderer.gameObject.GetComponent<MeshFilter>();
			}
			else
			{
				meshFilter = null;
			}
			if (type == typeof(TrailRenderer))
			{
				trailRenderer = renderer.gameObject.GetComponent<TrailRenderer>();
			}
			else
			{
				trailRenderer = null;
			}
			if (type == typeof(LineRenderer))
			{
				lineRenderer = renderer.gameObject.GetComponent<LineRenderer>();
			}
			else
			{
				lineRenderer = null;
			}
		}

		private void OnDisable()
		{
			if ((bool)trailRenderer)
			{
				trailRenderer.emitting = false;
			}
		}

		private void Update()
		{
			if ((bool)trailRenderer)
			{
				if (forceUpdating)
				{
					targetPosition = trailRenderer.transform.position;
					trailRenderer.transform.position = Vector3.Lerp(targetPosition, targetPosition + UnityEngine.Random.insideUnitSphere * 0.0001f, Time.deltaTime * 1000f);
				}
				trailRenderer.alignment = LineAlignment.Local;
				base.transform.rotation = Quaternion.Euler(TrailRotation);
			}
		}

		public static Material GetMaterial(Type type)
		{
			if (!(type == typeof(MeshRenderer)) && !(type == typeof(ParticleSystemRenderer)))
			{
				return TrailMaterial;
			}
			return MeshMaterial;
		}

		private void OnDrawGizmosSelected()
		{
			if ((bool)meshFilter && renderer.GetType() == typeof(MeshRenderer) && (bool)meshFilter.sharedMesh)
			{
				Gizmos.color = new Color(0f, 0f, 0f, 0.2f);
				Gizmos.DrawWireMesh(meshFilter.sharedMesh, 0, meshFilter.transform.position + new Vector3(0f, heightOffset, 0f), meshFilter.transform.rotation, meshFilter.transform.lossyScale * scaleMultiplier);
			}
		}
	}
}
