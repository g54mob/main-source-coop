using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.PhysicsVolumeModule.Scripts
{
	[ExecuteAlways]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class RopeTube : MonoBehaviour
	{
		private const float MIN_DIRECTION_SQR = 1E-08f;

		private const float MIN_SPAN = 0.0001f;

		private const int MIN_LENGTH_SAMPLES = 2;

		private const int MIN_RADIAL_SEGMENTS = 3;

		private const int UINT16_VERTEX_LIMIT = 65000;

		private const string MESH_NAME = "RopeTube";

		[SerializeField]
		private MeshFilter _meshFilter;

		[Tooltip("One end of the rope. Ignored while an owner drives the endpoints from code.")]
		[SerializeField]
		private Transform _from;

		[Tooltip("The other end. Ignored while an owner drives the endpoints from code.")]
		[SerializeField]
		private Transform _to;

		[Tooltip("Rope diameter in world units.")]
		[SerializeField]
		private float _thickness = 0.08f;

		[Tooltip("Sag depth at midspan as a fraction of the span — 0.06 is a taut steel cable, 0.2 a slack rope. 0 draws it dead straight.")]
		[SerializeField]
		[Range(0f, 0.5f)]
		private float _sagPercent = 0.06f;

		[Tooltip("Extra sag in metres, on top of the proportional sag. Unlike it, this is allowed to belly slightly below the low end, so a near-level span can still be given visible slack.")]
		[SerializeField]
		[Min(0f)]
		private float _slack;

		[Tooltip("Direction the rope sags in. Down for anything hanging under gravity.")]
		[SerializeField]
		private Vector3 _sagDirection = Vector3.down;

		[Tooltip("Samples along the rope. Drives the tube's fixed topology, not a per-frame cost.")]
		[SerializeField]
		[Min(2f)]
		private int _lengthSamples = 16;

		[Tooltip("Radial segments around the cross-section.")]
		[SerializeField]
		[Min(3f)]
		private int _radialSegments = 8;

		[SerializeField]
		private bool _capEnds = true;

		[Header("Material Mapping")]
		[Tooltip("Tiles along the rope per world unit — the texture keeps a constant real-world scale.")]
		[SerializeField]
		private float _uvTilesPerUnit = 4f;

		[Tooltip("Tiles around the circumference.")]
		[SerializeField]
		private float _uvTilesAround = 1f;

		private Vector3 _startPoint;

		private Vector3 _endPoint;

		private bool _isDriven;

		private int _sampleCount;

		private int _sideCount;

		private Vector3[] _spine = Array.Empty<Vector3>();

		private Vector3[] _vertices = Array.Empty<Vector3>();

		private Vector3[] _normals = Array.Empty<Vector3>();

		private Vector4[] _tangents = Array.Empty<Vector4>();

		private Vector2[] _uvs = Array.Empty<Vector2>();

		private int[] _triangles = Array.Empty<int>();

		private Vector2[] _unitCircle = Array.Empty<Vector2>();

		private int _topologySamples;

		private int _topologySides;

		private bool _topologyCapEnds;

		private bool _topologyFlipWinding;

		private int _indexCount;

		private Mesh _mesh;

		public void SetSlack(float slack)
		{
			_slack = Mathf.Max(0f, slack);
		}

		public void SetEndpoints(Vector3 start, Vector3 end)
		{
			_isDriven = true;
			_startPoint = start;
			_endPoint = end;
			Rebuild();
		}

		private void Reset()
		{
			_meshFilter = GetComponent<MeshFilter>();
		}

		private void Awake()
		{
			EnsureComponents();
			EnsureMesh();
		}

		private void LateUpdate()
		{
			if (!_isDriven && !(_from == null) && !(_to == null))
			{
				_startPoint = _from.position;
				_endPoint = _to.position;
				Rebuild();
			}
		}

		private void OnDestroy()
		{
			if (!(_mesh == null))
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(_mesh);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(_mesh);
				}
				_mesh = null;
			}
		}

		private void OnValidate()
		{
			_thickness = Mathf.Max(0f, _thickness);
			_lengthSamples = Mathf.Max(2, _lengthSamples);
			_radialSegments = Mathf.Max(3, _radialSegments);
			if (_sagDirection.sqrMagnitude < 1E-08f)
			{
				_sagDirection = Vector3.down;
			}
			InvalidateTopology();
		}

		private void EnsureComponents()
		{
			if (_meshFilter == null)
			{
				_meshFilter = GetComponent<MeshFilter>();
			}
		}

		private void EnsureMesh()
		{
			if (_meshFilter == null)
			{
				return;
			}
			if (_mesh != null)
			{
				if (_meshFilter.sharedMesh != _mesh)
				{
					_meshFilter.sharedMesh = _mesh;
				}
				return;
			}
			if (_meshFilter.sharedMesh != null && _meshFilter.sharedMesh.name == "RopeTube")
			{
				_mesh = _meshFilter.sharedMesh;
				InvalidateTopology();
				return;
			}
			_mesh = new Mesh
			{
				name = "RopeTube",
				hideFlags = HideFlags.HideAndDontSave
			};
			_mesh.MarkDynamic();
			_meshFilter.sharedMesh = _mesh;
			InvalidateTopology();
		}

		private void InvalidateTopology()
		{
			_topologySamples = 0;
			_topologySides = 0;
			_indexCount = 0;
		}

		private void ClearMesh()
		{
			if (!(_mesh == null))
			{
				_mesh.Clear();
				InvalidateTopology();
			}
		}

		private void Rebuild()
		{
			EnsureComponents();
			EnsureMesh();
			if (!(_mesh == null))
			{
				float magnitude = (_endPoint - _startPoint).magnitude;
				if (magnitude < 0.0001f || _thickness <= 0f)
				{
					ClearMesh();
					return;
				}
				_sampleCount = Mathf.Max(2, _lengthSamples);
				_sideCount = Mathf.Max(3, _radialSegments);
				BuildSpine(magnitude);
				SweepTube();
			}
		}

		private void BuildSpine(float span)
		{
			int sampleCount = _sampleCount;
			EnsureArray(ref _spine, sampleCount);
			Vector3 vector = ((_sagDirection.sqrMagnitude < 1E-08f) ? Vector3.down : _sagDirection.normalized);
			float a = span * _sagPercent + _slack;
			float num = Mathf.Abs(Vector3.Dot(_endPoint - _startPoint, vector));
			a = Mathf.Min(a, num * 0.25f + _slack);
			for (int i = 0; i < sampleCount; i++)
			{
				float num2 = (float)i / (float)(sampleCount - 1);
				_spine[i] = Vector3.Lerp(_startPoint, _endPoint, num2) + vector * (a * 4f * num2 * (1f - num2));
			}
		}

		private void SweepTube()
		{
			int sampleCount = _sampleCount;
			int sideCount = _sideCount;
			int num = sideCount + 1;
			float num2 = _thickness * 0.5f;
			bool capEnds = _capEnds;
			int num3 = sampleCount * num;
			int num4 = num3;
			int num5 = num4 + num;
			int num6 = (capEnds ? (num5 + 1) : num3);
			int num7 = num6 + num;
			int num8 = num3 + (capEnds ? ((num + 1) * 2) : 0);
			EnsureUnitCircle(sideCount);
			EnsureArray(ref _vertices, num8);
			EnsureArray(ref _normals, num8);
			EnsureArray(ref _tangents, num8);
			EnsureArray(ref _uvs, num8);
			Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
			Matrix4x4 transpose = base.transform.localToWorldMatrix.transpose;
			Vector3 boundsMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 boundsMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			Vector3 vector3 = Vector3.forward;
			Vector3 normalWorld = Vector3.up;
			Vector3 binormalWorld = Vector3.right;
			Vector3 capNormalWorld = Vector3.forward;
			Vector3 normalWorld2 = Vector3.up;
			Vector3 binormalWorld2 = Vector3.right;
			float num9 = 0f;
			for (int i = 0; i < sampleCount; i++)
			{
				Vector3 vector4 = ComputeForward(i, sampleCount, vector2);
				vector = ((i != 0) ? TransportNormal(vector2, vector4, vector) : InitialNormal(vector4));
				vector2 = vector4;
				vector -= vector4 * Vector3.Dot(vector4, vector);
				if (vector.sqrMagnitude < 1E-08f)
				{
					vector = InitialNormal(vector4);
				}
				else
				{
					vector.Normalize();
				}
				Vector3 vector5 = Vector3.Cross(vector4, vector);
				Vector3 vector6 = _spine[i];
				if (i > 0)
				{
					num9 += Vector3.Distance(_spine[i - 1], vector6);
				}
				int num10 = i * num;
				float y = num9 * _uvTilesPerUnit;
				for (int j = 0; j <= sideCount; j++)
				{
					Vector2 vector7 = _unitCircle[j];
					Vector3 vector8 = vector * vector7.x + vector5 * vector7.y;
					Vector3 vector9 = vector * (0f - vector7.y) + vector5 * vector7.x;
					Vector3 point = vector6 + vector8 * num2;
					Vector3 vector10 = worldToLocalMatrix.MultiplyPoint3x4(point);
					Vector3 normalized = worldToLocalMatrix.MultiplyVector(vector9).normalized;
					int num11 = num10 + j;
					_vertices[num11] = vector10;
					_normals[num11] = transpose.MultiplyVector(vector8).normalized;
					_tangents[num11] = new Vector4(normalized.x, normalized.y, normalized.z, -1f);
					_uvs[num11] = new Vector2((float)j / (float)sideCount * _uvTilesAround, y);
					boundsMin = Vector3.Min(boundsMin, vector10);
					boundsMax = Vector3.Max(boundsMax, vector10);
				}
				if (i == 0)
				{
					vector3 = vector4;
					normalWorld = vector;
					binormalWorld = vector5;
				}
				if (i == sampleCount - 1)
				{
					capNormalWorld = vector4;
					normalWorld2 = vector;
					binormalWorld2 = vector5;
				}
			}
			if (capEnds)
			{
				WriteCap(num4, num5, 0, sideCount, num, -vector3, normalWorld, binormalWorld, _spine[0], worldToLocalMatrix, transpose, ref boundsMin, ref boundsMax);
				WriteCap(num6, num7, sampleCount - 1, sideCount, num, capNormalWorld, normalWorld2, binormalWorld2, _spine[sampleCount - 1], worldToLocalMatrix, transpose, ref boundsMin, ref boundsMax);
			}
			bool flag = base.transform.localToWorldMatrix.determinant < 0f;
			int num12;
			if (_topologySamples == sampleCount && _topologySides == sideCount && _topologyCapEnds == capEnds)
			{
				num12 = ((_topologyFlipWinding != flag) ? 1 : 0);
				if (num12 == 0)
				{
					goto IL_043f;
				}
			}
			else
			{
				num12 = 1;
			}
			BuildTriangles(sampleCount, sideCount, num, capEnds, num4, num5, num6, num7, flag);
			_topologySamples = sampleCount;
			_topologySides = sideCount;
			_topologyCapEnds = capEnds;
			_topologyFlipWinding = flag;
			_mesh.Clear();
			_mesh.indexFormat = ((num8 > 65000) ? IndexFormat.UInt32 : IndexFormat.UInt16);
			goto IL_043f;
			IL_043f:
			_mesh.SetVertices(_vertices, 0, num8, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
			_mesh.SetNormals(_normals, 0, num8, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
			_mesh.SetTangents(_tangents, 0, num8, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
			_mesh.SetUVs(0, _uvs, 0, num8, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
			if (num12 != 0)
			{
				_mesh.SetTriangles(_triangles, 0, _indexCount, 0, calculateBounds: false);
			}
			_mesh.bounds = new Bounds((boundsMin + boundsMax) * 0.5f, boundsMax - boundsMin);
		}

		private void WriteCap(int ringStart, int centerIndex, int sourceRing, int sides, int ringStride, Vector3 capNormalWorld, Vector3 normalWorld, Vector3 binormalWorld, Vector3 centerWorld, Matrix4x4 worldToLocal, Matrix4x4 normalToLocal, ref Vector3 boundsMin, ref Vector3 boundsMax)
		{
			Vector3 normalized = normalToLocal.MultiplyVector(capNormalWorld).normalized;
			Vector3 normalized2 = worldToLocal.MultiplyVector(normalWorld).normalized;
			Vector4 vector = new Vector4(normalized2.x, normalized2.y, normalized2.z, -1f);
			int num = sourceRing * ringStride;
			for (int i = 0; i <= sides; i++)
			{
				Vector2 vector2 = _unitCircle[i];
				int num2 = ringStart + i;
				_vertices[num2] = _vertices[num + i];
				_normals[num2] = normalized;
				_tangents[num2] = vector;
				_uvs[num2] = new Vector2(0.5f + vector2.x * 0.5f, 0.5f + vector2.y * 0.5f);
			}
			Vector3 vector3 = worldToLocal.MultiplyPoint3x4(centerWorld);
			_vertices[centerIndex] = vector3;
			_normals[centerIndex] = normalized;
			_tangents[centerIndex] = vector;
			_uvs[centerIndex] = new Vector2(0.5f, 0.5f);
			boundsMin = Vector3.Min(boundsMin, vector3);
			boundsMax = Vector3.Max(boundsMax, vector3);
		}

		private void BuildTriangles(int sampleCount, int sides, int ringStride, bool capEnds, int startCapRing, int startCapCenter, int endCapRing, int endCapCenter, bool flipWinding)
		{
			int num = (sampleCount - 1) * sides * 6 + (capEnds ? (sides * 6) : 0);
			if (_triangles.Length < num)
			{
				_triangles = new int[num];
			}
			int num2 = 0;
			for (int i = 0; i < sampleCount - 1; i++)
			{
				int num3 = i * ringStride;
				int num4 = num3 + ringStride;
				for (int j = 0; j < sides; j++)
				{
					int num5 = num3 + j;
					int num6 = num5 + 1;
					int num7 = num4 + j;
					int num8 = num7 + 1;
					_triangles[num2++] = num5;
					_triangles[num2++] = num6;
					_triangles[num2++] = num7;
					_triangles[num2++] = num6;
					_triangles[num2++] = num8;
					_triangles[num2++] = num7;
				}
			}
			if (capEnds)
			{
				for (int k = 0; k < sides; k++)
				{
					_triangles[num2++] = startCapCenter;
					_triangles[num2++] = startCapRing + k + 1;
					_triangles[num2++] = startCapRing + k;
				}
				for (int l = 0; l < sides; l++)
				{
					_triangles[num2++] = endCapCenter;
					_triangles[num2++] = endCapRing + l;
					_triangles[num2++] = endCapRing + l + 1;
				}
			}
			if (flipWinding)
			{
				for (int m = 0; m < num; m += 3)
				{
					int num9 = _triangles[m + 1];
					_triangles[m + 1] = _triangles[m + 2];
					_triangles[m + 2] = num9;
				}
			}
			_indexCount = num;
		}

		private Vector3 ComputeForward(int index, int sampleCount, Vector3 fallback)
		{
			Vector3 vector = ((index == 0) ? (_spine[1] - _spine[0]) : ((index != sampleCount - 1) ? (_spine[index + 1] - _spine[index - 1]) : (_spine[sampleCount - 1] - _spine[sampleCount - 2])));
			if (vector.sqrMagnitude >= 1E-08f)
			{
				return vector.normalized;
			}
			if (fallback.sqrMagnitude >= 1E-08f)
			{
				return fallback;
			}
			return Vector3.forward;
		}

		private static Vector3 TransportNormal(Vector3 previousForward, Vector3 forward, Vector3 previousNormal)
		{
			float num = Vector3.Dot(previousForward, forward);
			Vector3 rhs;
			if (num > 0.9999f || num < -0.9999f)
			{
				rhs = previousNormal - forward * Vector3.Dot(forward, previousNormal);
			}
			else
			{
				rhs = Quaternion.FromToRotation(previousForward, forward) * previousNormal;
				rhs -= forward * Vector3.Dot(forward, rhs);
			}
			if (rhs.sqrMagnitude < 1E-08f)
			{
				return InitialNormal(forward);
			}
			return rhs.normalized;
		}

		private static Vector3 InitialNormal(Vector3 forward)
		{
			Vector3 vector = Vector3.Cross(forward, Vector3.up);
			if (vector.sqrMagnitude < 1E-08f)
			{
				vector = Vector3.Cross(forward, Vector3.right);
			}
			return vector.normalized;
		}

		private void EnsureUnitCircle(int sides)
		{
			if (_unitCircle.Length != sides + 1)
			{
				_unitCircle = new Vector2[sides + 1];
				for (int i = 0; i <= sides; i++)
				{
					float f = (float)i / (float)sides * MathF.PI * 2f;
					_unitCircle[i] = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
				}
			}
		}

		private static void EnsureArray<T>(ref T[] array, int size)
		{
			if (array == null || array.Length < size)
			{
				array = new T[size];
			}
		}
	}
}
