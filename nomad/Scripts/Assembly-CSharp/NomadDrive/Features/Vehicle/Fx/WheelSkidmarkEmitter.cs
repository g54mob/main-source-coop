using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace NomadDrive.Features.Vehicle.Fx
{
	public sealed class WheelSkidmarkEmitter
	{
		public float MinDistance = 0.12f;

		public float GroundOffset = 0.025f;

		public float LowerIntensityThreshold = 0.05f;

		public int MaxTrisPerSection = 300;

		public int MaxTotalTris = 1440;

		public float MaxAlpha = 0.6f;

		private static readonly Bounds BigBounds = new Bounds(Vector3.zero, Vector3.one * 1000f);

		private readonly Transform _container;

		private float _markWidth;

		private Vector3 _prevPos;

		private Vector3 _prevRight;

		private Vector3 _prevNormal;

		private float _prevIntensity;

		private bool _hasPrev;

		private Material _currentMaterial;

		private GameObject _section;

		private Mesh _mesh;

		private MeshFilter _meshFilter;

		private Vector3[] _vertices;

		private Vector3[] _normals;

		private Vector4[] _tangents;

		private Color[] _colors;

		private Vector2[] _uvs;

		private int[] _triangles;

		private int _triCount;

		private readonly Queue<GameObject> _sections = new Queue<GameObject>();

		public WheelSkidmarkEmitter(Transform container, float markWidth)
		{
			_container = container;
			_markWidth = ((markWidth > 0f) ? markWidth : 0.25f);
		}

		public void SetMarkWidth(float width)
		{
			if (width > 0f)
			{
				_markWidth = width;
			}
		}

		public void AddPoint(Vector3 contactPoint, Vector3 contactNormal, float intensity, Material material, float dt)
		{
			if (material == null)
			{
				return;
			}
			intensity = Mathf.Clamp(intensity, 0f, MaxAlpha);
			Vector3 vector = contactPoint + contactNormal * GroundOffset;
			if (intensity <= LowerIntensityThreshold)
			{
				_hasPrev = false;
			}
			else if (!_hasPrev || _section == null)
			{
				StartSection(material, vector);
				_currentMaterial = material;
				_prevPos = vector;
				_prevNormal = contactNormal;
				_prevRight = Vector3.zero;
				_prevIntensity = 0f;
				_hasPrev = true;
			}
			else if (!((vector - _prevPos).sqrMagnitude < MinDistance * MinDistance))
			{
				if (material != _currentMaterial)
				{
					_currentMaterial = material;
					StartSection(material, vector);
					_prevPos = vector;
					_prevRight = Vector3.zero;
					_prevIntensity = 0f;
				}
				Vector3 normalized = Vector3.Cross(vector - _prevPos, contactNormal).normalized;
				if (_prevRight == Vector3.zero)
				{
					_prevRight = normalized;
				}
				AppendQuad(_prevPos, _prevRight, _prevNormal, _prevIntensity, vector, normalized, contactNormal, intensity);
				_prevPos = vector;
				_prevRight = normalized;
				_prevNormal = contactNormal;
				_prevIntensity = intensity;
				if (_triCount + 2 >= MaxTrisPerSection)
				{
					StartSection(material, vector);
				}
			}
		}

		public void Break()
		{
			_hasPrev = false;
		}

		public void Cleanup()
		{
			while (_sections.Count > 0)
			{
				GameObject gameObject = _sections.Dequeue();
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
			}
			_section = null;
			_mesh = null;
			_meshFilter = null;
			_triCount = 0;
			_hasPrev = false;
		}

		private void StartSection(Material material, Vector3 worldPos)
		{
			_section = new GameObject("SkidmarkSection");
			_section.transform.SetParent(_container, worldPositionStays: true);
			_section.transform.SetPositionAndRotation(worldPos, Quaternion.identity);
			MeshRenderer meshRenderer = _section.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = material;
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.lightProbeUsage = LightProbeUsage.Off;
			meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
			meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
			_meshFilter = _section.AddComponent<MeshFilter>();
			int num = MaxTrisPerSection * 3;
			_vertices = new Vector3[num];
			_normals = new Vector3[num];
			_tangents = new Vector4[num];
			_colors = new Color[num];
			_uvs = new Vector2[num];
			_triangles = new int[num];
			_triCount = 0;
			_mesh = new Mesh
			{
				name = "SkidmarkMesh"
			};
			_mesh.MarkDynamic();
			_mesh.bounds = BigBounds;
			_meshFilter.mesh = _mesh;
			_sections.Enqueue(_section);
			int num2 = Mathf.Max(2, MaxTotalTris / Mathf.Max(1, MaxTrisPerSection));
			while (_sections.Count > num2)
			{
				GameObject gameObject = _sections.Dequeue();
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
			}
		}

		private void AppendQuad(Vector3 prevPos, Vector3 prevRight, Vector3 prevNormal, float prevIntensity, Vector3 curPos, Vector3 curRight, Vector3 curNormal, float curIntensity)
		{
			if (!(_section == null))
			{
				Transform transform = _section.transform;
				int num = _triCount * 2;
				int num2 = _triCount * 3;
				int num3 = num;
				int num4 = num + 1;
				int num5 = num + 2;
				int num6 = num + 3;
				if (num6 < _vertices.Length && num2 + 5 < _triangles.Length)
				{
					float num7 = _markWidth * 0.5f;
					_vertices[num3] = transform.InverseTransformPoint(prevPos + prevRight * num7);
					_vertices[num4] = transform.InverseTransformPoint(prevPos - prevRight * num7);
					_vertices[num5] = transform.InverseTransformPoint(curPos + curRight * num7);
					_vertices[num6] = transform.InverseTransformPoint(curPos - curRight * num7);
					Vector3 vector = transform.InverseTransformDirection(prevNormal);
					Vector3 vector2 = transform.InverseTransformDirection(curNormal);
					_normals[num3] = vector;
					_normals[num4] = vector;
					_normals[num5] = vector2;
					_normals[num6] = vector2;
					Vector4 vector3 = transform.InverseTransformDirection(prevRight);
					vector3.w = 1f;
					Vector4 vector4 = transform.InverseTransformDirection(curRight);
					vector4.w = 1f;
					_tangents[num3] = vector3;
					_tangents[num4] = vector3;
					_tangents[num5] = vector4;
					_tangents[num6] = vector4;
					Color color = new Color(0f, 0f, 0f, prevIntensity);
					Color color2 = new Color(0f, 0f, 0f, curIntensity);
					_colors[num3] = color;
					_colors[num4] = color;
					_colors[num5] = color2;
					_colors[num6] = color2;
					_uvs[num3] = new Vector2(0f, 0f);
					_uvs[num4] = new Vector2(1f, 0f);
					_uvs[num5] = new Vector2(0f, 1f);
					_uvs[num6] = new Vector2(1f, 1f);
					_triangles[num2] = num3;
					_triangles[num2 + 2] = num4;
					_triangles[num2 + 1] = num5;
					_triangles[num2 + 3] = num5;
					_triangles[num2 + 5] = num4;
					_triangles[num2 + 4] = num6;
					_mesh.vertices = _vertices;
					_mesh.normals = _normals;
					_mesh.tangents = _tangents;
					_mesh.triangles = _triangles;
					_mesh.colors = _colors;
					_mesh.uv = _uvs;
					_mesh.bounds = BigBounds;
					_triCount += 2;
				}
			}
		}
	}
}
