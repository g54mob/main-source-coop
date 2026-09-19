using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.SnakeModule.Scripts
{
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class SnakeBodyLineRenderer : MonoBehaviour
	{
		private struct TubeVertex
		{
			public Vector3 Position;

			public Vector3 Normal;

			public Vector4 Tangent;

			public Color Color;

			public Vector2 Uv;
		}

		private struct NeckSample
		{
			public float Angle;

			public int VertexIndex;
		}

		private const float MIN_TAPER_FACTOR = 0.0001f;

		private const float MIN_DIRECTION_SQR = 1E-08f;

		private const float MIN_BISECTOR_LENGTH_SQR = 0.01f;

		private const float MIN_POINT_SPACING_SQR = 1E-06f;

		private const float MIN_COLUMN_ARC = 1E-05f;

		private const float DEFAULT_MAX_ANGLE_PER_SAMPLE = 8f;

		private const int DEFAULT_MAX_SMOOTHING = 24;

		private const int DEFAULT_HEAD_WELD_RINGS = 6;

		private const float WELD_QUANTIZATION = 10000f;

		private const float DEFAULT_HEAD_TEXTURE_BLEND_DISTANCE = 0.5f;

		private const float DEFAULT_SHARP_TURN_DEGREES = 90f;

		private const int DEFAULT_SHARP_TURN_EXTRA_SAMPLES = 24;

		private const float UNIT_SCALE_EPSILON = 0.0001f;

		private static readonly Color _opaqueBodyColor = new Color(1f, 1f, 1f, 0f);

		private static readonly ProfilerMarker _tickMarker = new ProfilerMarker("Snake.LineRenderer.TickLate");

		private static readonly ProfilerMarker _splineMarker = new ProfilerMarker("Snake.LineRenderer.Spline");

		private static readonly ProfilerMarker _rebuildMarker = new ProfilerMarker("Snake.LineRenderer.Rebuild");

		[SerializeField]
		private MeshFilter _meshFilter;

		[SerializeField]
		private MeshRenderer _meshRenderer;

		[SerializeField]
		private List<Transform> _points = new List<Transform>();

		[SerializeField]
		private bool _includeSelfAsFirstPoint = true;

		[Tooltip("Tube diameter in world units — not affected by transform scale.")]
		[SerializeField]
		private float _thickness = 0.15f;

		[Tooltip("Normalized length (0–1) where the tube starts tapering to 0 radius at the tip.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _taperStartPercent = 0.7f;

		[Tooltip("Minimum extra points between each pair of body parts.")]
		[SerializeField]
		[Min(0f)]
		private int _smoothing = 4;

		[Tooltip("Max direction change in degrees between two consecutive samples. Lower inserts more points on sharp turns so the bend follows the curve instead of cutting the corner.")]
		[SerializeField]
		[Range(1f, 45f)]
		private float _maxAnglePerSample = 8f;

		[Tooltip("Upper bound on extra points inserted between two body parts.")]
		[SerializeField]
		[Min(1f)]
		private int _maxSmoothing = 24;

		[Tooltip("Radial segments around the tube cross-section. Ignored while a head is welded — the head's neck vertex count drives it instead, so ring 0 lands on the neck vertices exactly.")]
		[SerializeField]
		[Min(3f)]
		private int _radialSegments = 8;

		[Tooltip("Turn across one spline segment (degrees) past which the segment is allowed to exceed Max Smoothing. Below it the normal budget applies. 0 = not set (uses the default); negative = never exceed Max Smoothing.")]
		[SerializeField]
		private float _sharpTurnDegrees;

		[Tooltip("Extra samples granted to a fully reversed (180°) segment, scaled down toward 0 at the Sharp Turn threshold. 0 = not set (uses the default).")]
		[SerializeField]
		private int _sharpTurnExtraSamples;

		[SerializeField]
		private bool _capEnds = true;

		[Tooltip("Keep rebuilding the tube while it is off-screen. Off (the default) skips the rebuild entirely, at the cost of the mesh lagging one frame on the frame it comes back into view. Phrased as an opt-OUT because Unity writes false into new serialized fields on existing prefabs, so the zero value has to be the one we want.")]
		[SerializeField]
		private bool _rebuildWhileOffscreen;

		[Header("Head Weld")]
		[Tooltip("Head whose neck cross-section becomes the tube's first ring. Empty = plain circular tube.")]
		[SerializeField]
		private SkinnedMeshRenderer _headSkinnedMesh;

		[Tooltip("Rings over which the cross-section blends from the neck profile to a circle. Counts RINGS, not distance — at high _smoothing the rings are dense and this becomes a very short blend.")]
		[SerializeField]
		[Min(1f)]
		private int _headWeldRings = 6;

		[Tooltip("Which contour is the neck. -1 = pick automatically. A head has more contours than the neck — mouth, eyes, nostrils — so auto-selection can pick the wrong one; use the gizmos to see which contour was taken.")]
		[SerializeField]
		private int _headNeckLoopIndex = -1;

		[Tooltip("World distance over which the shader cross-fades the body texture into a neck tone at the seam. Written to vertex colour alpha: 1 at the seam, 0 past this distance. 0 = not set (falls back to the default); use a negative value to switch the cross-fade off.")]
		[SerializeField]
		private float _headTextureBlendDistance = 0.5f;

		[Tooltip("Carry the head's boundary normals into the weld zone for continuous lighting. Only helps when the neck rim has smooth radial normals; on a flat rim it darkens the zone into a band.")]
		[SerializeField]
		private bool _weldBlendsHeadNormals;

		[Tooltip("Draws the skinned neck loop and the weld-zone ring centres as gizmos.")]
		[SerializeField]
		private bool _drawWeldGizmos;

		[Header("Material Mapping")]
		[SerializeField]
		private SnakeTubeUvMode _uvMode = SnakeTubeUvMode.WorldLength;

		[Tooltip("Tiles along the body per world unit. Used by WorldLength mode — the texture keeps a constant real-world scale but slides as the body stretches.")]
		[SerializeField]
		private float _uvTilesPerUnit = 1f;

		[Tooltip("Tiles across the whole body. Used by NormalizedLength and NormalizedPerColumn modes — the texture stays locked to the body and never slides, but squashes as the body stretches.")]
		[SerializeField]
		private float _uvTilesAlongBody = 4f;

		[Tooltip("Tiles around the circumference. Unused while a head is welded — the head's neck unwrap defines the around-mapping.")]
		[SerializeField]
		private float _uvTilesPerSegment = 1f;

		[SerializeField]
		private float _uvTilesAround = 1f;

		[SerializeField]
		private Vector2 _uvOffset = Vector2.zero;

		[Tooltip("Which axis the body length runs along. Swap it if the head's unwrap has the body going horizontally. Negative Uv Tiles Per Unit flips the direction.")]
		[SerializeField]
		private bool _uvSwapAxes;

		private readonly List<Vector3> _sourcePoints = new List<Vector3>();

		private readonly List<Vector3> _controlPoints = new List<Vector3>();

		private readonly List<float> _sourceRest = new List<float>();

		private float[] _renderRest = Array.Empty<float>();

		private Vector3[] _renderPoints = Array.Empty<Vector3>();

		private int _renderPointCount;

		private Vector3[] _framePoints = Array.Empty<Vector3>();

		private Vector3[] _solvedControls = Array.Empty<Vector3>();

		private Vector3[] _solveRhs = Array.Empty<Vector3>();

		private float[] _solveUpper = Array.Empty<float>();

		private float[] _cumulativeLengths = Array.Empty<float>();

		private static readonly VertexAttributeDescriptor[] _vertexLayout = new VertexAttributeDescriptor[5]
		{
			new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, 0),
			new VertexAttributeDescriptor(VertexAttribute.Normal),
			new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 4),
			new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2)
		};

		private TubeVertex[] _tubeVertices = Array.Empty<TubeVertex>();

		private int[] _triangles = Array.Empty<int>();

		private readonly List<NeckSample> _loopSamples = new List<NeckSample>();

		private readonly Stack<int> _loopFloodStack = new Stack<int>();

		private readonly List<List<int>> _profileContours = new List<List<int>>();

		private int[] _headProfileVertices;

		private Vector3[] _neckWorldPositions;

		private Vector3[] _neckWorldNormals;

		private Vector3[] _neckFrameOffsets;

		private Vector3[] _neckFrameNormals;

		private Vector3[] _headBindVertices;

		private Vector3[] _headBindNormals;

		private BoneWeight[] _headBoneWeights;

		private Matrix4x4[] _headBindPoses;

		private Transform[] _headBones;

		private Matrix4x4[] _headSkinMatrices;

		private Transform _headTransform;

		private bool _hasHeadProfile;

		private int _headProfileSides;

		private float[] _columnArcs = Array.Empty<float>();

		private Vector3[] _previousRingWorld = Array.Empty<Vector3>();

		private Vector2[] _unitCircle = Array.Empty<Vector2>();

		private float[] _uCoords = Array.Empty<float>();

		private float[] _uScaled = Array.Empty<float>();

		private float _uScaledTiles = float.NaN;

		private int _unitCircleSides;

		private bool _hasBuiltMesh;

		private int _topologyPathCount;

		private int _topologySides;

		private bool _topologyHasHeadCap;

		private bool _topologyHasTipCap;

		private bool _topologyFlipWinding;

		private bool _topologyWelding;

		private int _indexCount;

		private Mesh _mesh;

		private void Awake()
		{
			EnsureMesh();
			BuildHeadProfile();
		}

		private void OnDrawGizmos()
		{
			if (!_drawWeldGizmos)
			{
				return;
			}
			if (_hasHeadProfile && _neckWorldPositions != null)
			{
				Gizmos.color = Color.cyan;
				for (int i = 0; i < _headProfileSides; i++)
				{
					Gizmos.DrawLine(_neckWorldPositions[i], _neckWorldPositions[i + 1]);
				}
				Gizmos.color = Color.yellow;
				for (int j = 0; j <= _headProfileSides; j++)
				{
					Gizmos.DrawSphere(_neckWorldPositions[j], 0.01f);
				}
			}
			if (_renderPointCount != 0)
			{
				int num = Mathf.Min((_headWeldRings > 0) ? _headWeldRings : 6, _renderPointCount - 1);
				for (int k = 0; k <= num; k++)
				{
					Gizmos.color = Color.Lerp(Color.red, Color.green, (float)k / (float)Mathf.Max(1, num));
					Gizmos.DrawSphere(_renderPoints[k], 0.02f);
				}
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

		public void TickLate()
		{
			if (!_rebuildWhileOffscreen && _hasBuiltMesh && _meshRenderer != null && !_meshRenderer.isVisible)
			{
				return;
			}
			using (_tickMarker.Auto())
			{
				BuildSourcePoints();
				if (_sourcePoints.Count < 2)
				{
					ClearMesh();
					return;
				}
				using (_splineMarker.Auto())
				{
					BuildRenderPoints();
				}
				using (_rebuildMarker.Auto())
				{
					RebuildTubeMesh();
				}
			}
		}

		private void OnValidate()
		{
			_thickness = Mathf.Max(0f, _thickness);
			_taperStartPercent = Mathf.Clamp01(_taperStartPercent);
			_smoothing = Mathf.Max(0, _smoothing);
			_radialSegments = Mathf.Max(3, _radialSegments);
			if (_maxAnglePerSample > 0f)
			{
				_maxAnglePerSample = Mathf.Clamp(_maxAnglePerSample, 1f, 45f);
			}
			if (_maxSmoothing > 0)
			{
				_maxSmoothing = Mathf.Max(_maxSmoothing, _smoothing);
			}
			_uvTilesPerUnit = Mathf.Max(0f, _uvTilesPerUnit);
			_uvTilesAlongBody = Mathf.Max(0f, _uvTilesAlongBody);
			_uvTilesPerSegment = Mathf.Max(0f, _uvTilesPerSegment);
			if (_sharpTurnDegrees > 0f)
			{
				_sharpTurnDegrees = Mathf.Clamp(_sharpTurnDegrees, 10f, 179f);
			}
			_sharpTurnExtraSamples = Mathf.Max(0, _sharpTurnExtraSamples);
		}

		public void RebuildPreview()
		{
			EnsureMesh();
			BuildHeadProfile();
			TickLate();
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
			_mesh = new Mesh
			{
				name = "SnakeBodyTube",
				hideFlags = HideFlags.HideAndDontSave
			};
			_mesh.MarkDynamic();
			_meshFilter.sharedMesh = _mesh;
			InvalidateTopology();
		}

		private void ClearMesh()
		{
			if (!(_mesh == null))
			{
				_mesh.Clear();
				InvalidateTopology();
				_hasBuiltMesh = false;
			}
		}

		private void InvalidateTopology()
		{
			_topologyPathCount = 0;
			_topologySides = 0;
			_indexCount = 0;
		}

		private void BuildHeadProfile()
		{
			_hasHeadProfile = false;
			_headProfileSides = 0;
			if (_headSkinnedMesh == null)
			{
				return;
			}
			_headTransform = _headSkinnedMesh.transform;
			Mesh sharedMesh = _headSkinnedMesh.sharedMesh;
			if (sharedMesh == null)
			{
				return;
			}
			_headBindVertices = sharedMesh.vertices;
			_headBindNormals = sharedMesh.normals;
			_headBoneWeights = sharedMesh.boneWeights;
			_headBindPoses = sharedMesh.bindposes;
			_headBones = _headSkinnedMesh.bones;
			int[] triangles = sharedMesh.triangles;
			if (_headBindVertices.Length == 0 || triangles.Length == 0 || _headBones == null || _headBones.Length == 0 || _headBindPoses.Length != _headBones.Length || _headBoneWeights.Length != _headBindVertices.Length)
			{
				return;
			}
			if (_headSkinMatrices == null || _headSkinMatrices.Length < _headBones.Length)
			{
				_headSkinMatrices = new Matrix4x4[_headBones.Length];
			}
			int[] canonical = WeldByPosition(_headBindVertices);
			_profileContours.Clear();
			BuildBoundaryContours(canonical, triangles);
			if (_profileContours.Count != 0)
			{
				Vector3 reference = _headTransform.InverseTransformPoint(base.transform.position);
				List<int> list = SelectContour(reference);
				if (list != null)
				{
					OrderContour(list);
				}
			}
		}

		private void BuildBoundaryContours(int[] canonical, int[] triangles)
		{
			Dictionary<long, int> dictionary = new Dictionary<long, int>(triangles.Length);
			for (int i = 0; i < triangles.Length; i += 3)
			{
				AccumulateEdge(dictionary, canonical[triangles[i]], canonical[triangles[i + 1]]);
				AccumulateEdge(dictionary, canonical[triangles[i + 1]], canonical[triangles[i + 2]]);
				AccumulateEdge(dictionary, canonical[triangles[i + 2]], canonical[triangles[i]]);
			}
			Dictionary<int, List<int>> adjacency = new Dictionary<int, List<int>>();
			foreach (KeyValuePair<long, int> item in dictionary)
			{
				if (item.Value == 1)
				{
					int num = (int)(item.Key >> 32);
					int num2 = (int)(item.Key & 0xFFFFFFFFu);
					AddAdjacency(adjacency, num, num2);
					AddAdjacency(adjacency, num2, num);
				}
			}
			BuildContours(adjacency);
		}

		private void BuildContours(Dictionary<int, List<int>> adjacency)
		{
			HashSet<int> hashSet = new HashSet<int>();
			foreach (KeyValuePair<int, List<int>> item in adjacency)
			{
				if (hashSet.Contains(item.Key))
				{
					continue;
				}
				List<int> list = new List<int>();
				_loopFloodStack.Clear();
				_loopFloodStack.Push(item.Key);
				while (_loopFloodStack.Count > 0)
				{
					int num = _loopFloodStack.Pop();
					if (!hashSet.Add(num))
					{
						continue;
					}
					list.Add(num);
					List<int> list2 = adjacency[num];
					for (int i = 0; i < list2.Count; i++)
					{
						if (!hashSet.Contains(list2[i]))
						{
							_loopFloodStack.Push(list2[i]);
						}
					}
				}
				if (list.Count >= 3)
				{
					_profileContours.Add(list);
				}
			}
		}

		private List<int> SelectContour(Vector3 reference)
		{
			if (_headNeckLoopIndex >= 0 && _headNeckLoopIndex < _profileContours.Count)
			{
				return _profileContours[_headNeckLoopIndex];
			}
			List<int> result = null;
			float num = float.MaxValue;
			for (int i = 0; i < _profileContours.Count; i++)
			{
				float sqrMagnitude = (ContourCentroid(_profileContours[i]) - reference).sqrMagnitude;
				if (!(sqrMagnitude >= num))
				{
					num = sqrMagnitude;
					result = _profileContours[i];
				}
			}
			return result;
		}

		private Vector3 ContourCentroid(List<int> contour)
		{
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < contour.Count; i++)
			{
				zero += _headBindVertices[contour[i]];
			}
			return zero / contour.Count;
		}

		private void OrderContour(List<int> contour)
		{
			Vector3 vector = ContourCentroid(contour);
			Vector3 lhs = ResolveNeckAxis(vector);
			Vector3 rhs = Vector3.Cross(lhs, Vector3.up);
			if (rhs.sqrMagnitude < 1E-08f)
			{
				rhs = Vector3.Cross(lhs, Vector3.right);
			}
			rhs.Normalize();
			Vector3 rhs2 = Vector3.Cross(lhs, rhs);
			_loopSamples.Clear();
			for (int i = 0; i < contour.Count; i++)
			{
				int num = contour[i];
				Vector3 lhs2 = _headBindVertices[num] - vector;
				float num2 = Mathf.Atan2(Vector3.Dot(lhs2, rhs2), Vector3.Dot(lhs2, rhs));
				if (num2 < 0f)
				{
					num2 += MathF.PI * 2f;
				}
				_loopSamples.Add(new NeckSample
				{
					Angle = num2,
					VertexIndex = num
				});
			}
			_loopSamples.Sort((NeckSample a, NeckSample b) => a.Angle.CompareTo(b.Angle));
			int count = _loopSamples.Count;
			int size = count + 1;
			EnsureArray(ref _headProfileVertices, size);
			EnsureArray(ref _neckWorldPositions, size);
			EnsureArray(ref _neckWorldNormals, size);
			EnsureArray(ref _neckFrameOffsets, size);
			EnsureArray(ref _neckFrameNormals, size);
			for (int num3 = 0; num3 < count; num3++)
			{
				_headProfileVertices[num3] = _loopSamples[num3].VertexIndex;
			}
			_headProfileVertices[count] = _headProfileVertices[0];
			_headProfileSides = count;
			_hasHeadProfile = true;
		}

		private static long EdgeKey(int a, int b)
		{
			int num = Mathf.Min(a, b);
			int num2 = Mathf.Max(a, b);
			return ((long)num << 32) | (uint)num2;
		}

		[ContextMenu("Rebuild Head Profile")]
		private void RebuildHeadProfileFromMenu()
		{
			BuildHeadProfile();
		}

		private static int[] WeldByPosition(Vector3[] vertices)
		{
			Dictionary<Vector3Int, int> dictionary = new Dictionary<Vector3Int, int>(vertices.Length);
			int[] array = new int[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector3 vector = vertices[i];
				Vector3Int key = new Vector3Int(Mathf.RoundToInt(vector.x * 10000f), Mathf.RoundToInt(vector.y * 10000f), Mathf.RoundToInt(vector.z * 10000f));
				if (!dictionary.TryGetValue(key, out var value))
				{
					dictionary.Add(key, i);
					value = i;
				}
				array[i] = value;
			}
			return array;
		}

		private static void AccumulateEdge(Dictionary<long, int> edgeUse, int a, int b)
		{
			if (a != b)
			{
				long key = EdgeKey(a, b);
				edgeUse.TryGetValue(key, out var value);
				edgeUse[key] = value + 1;
			}
		}

		private static void AddAdjacency(Dictionary<int, List<int>> adjacency, int from, int to)
		{
			if (!adjacency.TryGetValue(from, out var value))
			{
				value = new List<int>(2);
				adjacency.Add(from, value);
			}
			if (!value.Contains(to))
			{
				value.Add(to);
			}
		}

		private Vector3 ResolveNeckAxis(Vector3 loopCentroid)
		{
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < _headBindVertices.Length; i++)
			{
				zero += _headBindVertices[i];
			}
			zero /= (float)_headBindVertices.Length;
			Vector3 vector = loopCentroid - zero;
			if (vector.sqrMagnitude < 1E-08f)
			{
				return Vector3.forward;
			}
			return vector.normalized;
		}

		private void SkinNeckLoop(int sides)
		{
			for (int i = 0; i < _headBones.Length; i++)
			{
				Transform transform = _headBones[i];
				_headSkinMatrices[i] = ((transform != null) ? (transform.localToWorldMatrix * _headBindPoses[i]) : Matrix4x4.identity);
			}
			bool hasNormals = _headBindNormals.Length == _headBindVertices.Length;
			for (int j = 0; j <= sides; j++)
			{
				SkinVertex(_headProfileVertices[j], hasNormals, out var position, out var normal);
				_neckWorldPositions[j] = position;
				_neckWorldNormals[j] = normal;
			}
		}

		private void SkinVertex(int index, bool hasNormals, out Vector3 position, out Vector3 normal)
		{
			Vector3 bindPosition = _headBindVertices[index];
			Vector3 bindNormal = (hasNormals ? _headBindNormals[index] : Vector3.up);
			BoneWeight boneWeight = _headBoneWeights[index];
			position = Vector3.zero;
			normal = Vector3.zero;
			AccumulateSkin(boneWeight.boneIndex0, boneWeight.weight0, bindPosition, bindNormal, ref position, ref normal);
			AccumulateSkin(boneWeight.boneIndex1, boneWeight.weight1, bindPosition, bindNormal, ref position, ref normal);
			AccumulateSkin(boneWeight.boneIndex2, boneWeight.weight2, bindPosition, bindNormal, ref position, ref normal);
			AccumulateSkin(boneWeight.boneIndex3, boneWeight.weight3, bindPosition, bindNormal, ref position, ref normal);
			if (normal.sqrMagnitude < 1E-08f)
			{
				normal = Vector3.up;
			}
			else
			{
				normal.Normalize();
			}
		}

		private void AccumulateSkin(int boneIndex, float weight, Vector3 bindPosition, Vector3 bindNormal, ref Vector3 position, ref Vector3 normal)
		{
			if (!(weight <= 0f) && boneIndex >= 0 && boneIndex < _headSkinMatrices.Length)
			{
				Matrix4x4 matrix4x = _headSkinMatrices[boneIndex];
				position += matrix4x.MultiplyPoint3x4(bindPosition) * weight;
				normal += matrix4x.MultiplyVector(bindNormal) * weight;
			}
		}

		private void BuildSourcePoints()
		{
			_sourcePoints.Clear();
			_sourceRest.Clear();
			if (_includeSelfAsFirstPoint)
			{
				_sourcePoints.Add(base.transform.position);
				_sourceRest.Add(0f);
			}
			for (int i = 0; i < _points.Count; i++)
			{
				Transform transform = _points[i];
				if (!(transform == null))
				{
					Vector3 position = transform.position;
					if (_sourcePoints.Count <= 0 || !((position - _sourcePoints[_sourcePoints.Count - 1]).sqrMagnitude < 1E-06f))
					{
						_sourcePoints.Add(position);
						_sourceRest.Add(_includeSelfAsFirstPoint ? (i + 1) : i);
					}
				}
			}
		}

		private void BuildRenderPoints()
		{
			_renderPointCount = 0;
			SolveInterpolatingControlPoints();
			int num = _controlPoints.Count - 3;
			if (num < 1)
			{
				EnsureArray(ref _renderPoints, _sourcePoints.Count);
				EnsureArray(ref _renderRest, _sourcePoints.Count);
				for (int i = 0; i < _sourcePoints.Count; i++)
				{
					_renderRest[_renderPointCount] = _sourceRest[i];
					_renderPoints[_renderPointCount++] = _sourcePoints[i];
				}
				return;
			}
			int size = 1 + num * (ResolveWorstCaseSampleCeiling() + 1);
			EnsureArray(ref _renderPoints, size);
			EnsureArray(ref _renderRest, size);
			_renderRest[_renderPointCount] = _sourceRest[0];
			_renderPoints[_renderPointCount++] = _sourcePoints[0];
			for (int j = 0; j < num; j++)
			{
				Vector3 vector = _controlPoints[j];
				Vector3 vector2 = _controlPoints[j + 1];
				Vector3 vector3 = _controlPoints[j + 2];
				Vector3 vector4 = _controlPoints[j + 3];
				float a = _sourceRest[j];
				float b = _sourceRest[j + 1];
				int num2 = ResolveSampleCount(vector, vector2, vector3, vector4) + 1;
				for (int k = 1; k <= num2; k++)
				{
					float num3 = (float)k / (float)num2;
					_renderRest[_renderPointCount] = Mathf.Lerp(a, b, num3);
					_renderPoints[_renderPointCount++] = EvaluateBSpline(vector, vector2, vector3, vector4, num3);
				}
			}
		}

		private void SolveInterpolatingControlPoints()
		{
			int num = _sourcePoints.Count - 1;
			int num2 = num - 1;
			EnsureSolveCapacity(num + 1);
			_solvedControls[0] = _sourcePoints[0];
			_solvedControls[num] = _sourcePoints[num];
			if (num2 >= 1)
			{
				for (int i = 1; i <= num2; i++)
				{
					_solveRhs[i] = 6f * _sourcePoints[i];
				}
				_solveRhs[1] -= _solvedControls[0];
				_solveRhs[num2] -= _solvedControls[num];
				_solveUpper[1] = 0.25f;
				_solveRhs[1] /= 4f;
				for (int j = 2; j <= num2; j++)
				{
					float num3 = 4f - _solveUpper[j - 1];
					_solveUpper[j] = 1f / num3;
					_solveRhs[j] = (_solveRhs[j] - _solveRhs[j - 1]) / num3;
				}
				_solvedControls[num2] = _solveRhs[num2];
				for (int num4 = num2 - 1; num4 >= 1; num4--)
				{
					_solvedControls[num4] = _solveRhs[num4] - _solveUpper[num4] * _solvedControls[num4 + 1];
				}
			}
			_controlPoints.Clear();
			_controlPoints.Add(2f * _solvedControls[0] - _solvedControls[1]);
			for (int k = 0; k <= num; k++)
			{
				_controlPoints.Add(_solvedControls[k]);
			}
			_controlPoints.Add(2f * _solvedControls[num] - _solvedControls[num - 1]);
		}

		private void EnsureSolveCapacity(int count)
		{
			EnsureArray(ref _solvedControls, count);
			EnsureArray(ref _solveRhs, count);
			EnsureArray(ref _solveUpper, count);
		}

		private static Vector3 EvaluateBSpline(Vector3 c0, Vector3 c1, Vector3 c2, Vector3 c3, float u)
		{
			float num = u * u;
			float num2 = num * u;
			float num3 = 1f - u;
			float num4 = num3 * num3 * num3;
			float num5 = 3f * num2 - 6f * num + 4f;
			float num6 = -3f * num2 + 3f * num + 3f * u + 1f;
			float num7 = num2;
			return (c0 * num4 + c1 * num5 + c2 * num6 + c3 * num7) / 6f;
		}

		private int ResolveSampleCount(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
		{
			float num = ((_maxAnglePerSample > 0f) ? _maxAnglePerSample : 8f);
			int num2 = ((_maxSmoothing > 0) ? _maxSmoothing : 24);
			Vector3 vector = p1 - p0;
			Vector3 vector2 = p2 - p1;
			Vector3 to = p3 - p2;
			if (vector2.sqrMagnitude < 1E-08f)
			{
				return Mathf.Clamp(_smoothing, 0, num2);
			}
			float num3 = 0f;
			if (vector.sqrMagnitude >= 1E-08f)
			{
				num3 += Vector3.Angle(vector, vector2);
			}
			if (to.sqrMagnitude >= 1E-08f)
			{
				num3 += Vector3.Angle(vector2, to);
			}
			return Mathf.Clamp(Mathf.Max(Mathf.CeilToInt(num3 / num) - 1, _smoothing), 0, ResolveSampleCeiling(num3, num2));
		}

		private int ResolveWorstCaseSampleCeiling()
		{
			return ResolveSampleCeiling(360f, (_maxSmoothing > 0) ? _maxSmoothing : 24);
		}

		private int ResolveSampleCeiling(float turnDegrees, int maxSamples)
		{
			if (_sharpTurnDegrees < 0f)
			{
				return maxSamples;
			}
			float num = ((_sharpTurnDegrees == 0f) ? 90f : _sharpTurnDegrees);
			if (turnDegrees <= num)
			{
				return maxSamples;
			}
			int num2 = ((_sharpTurnExtraSamples > 0) ? _sharpTurnExtraSamples : 24);
			float num3 = Mathf.Clamp01((turnDegrees - num) / (360f - num));
			return maxSamples + Mathf.CeilToInt(num3 * (float)num2);
		}

		private void RebuildTubeMesh()
		{
			int renderPointCount = _renderPointCount;
			if (renderPointCount < 2)
			{
				ClearMesh();
				return;
			}
			BuildCumulativeLengths(renderPointCount);
			float num = _cumulativeLengths[renderPointCount - 1];
			float num2 = _thickness * 0.5f;
			bool hasHeadProfile = _hasHeadProfile;
			int num3 = (hasHeadProfile ? _headProfileSides : _radialSegments);
			int num4 = num3 + 1;
			bool flag = _capEnds && !hasHeadProfile;
			float num5 = EvaluateTaper(1f);
			bool flag2 = _capEnds && num5 > 0.0001f;
			int num6 = renderPointCount * num4;
			int num7 = num6;
			int num8 = num7 + num4;
			int num9 = (flag ? (num8 + 1) : num6);
			int num10 = num9 + num4;
			int num11 = num6 + (flag ? (num4 + 1) : 0) + (flag2 ? (num4 + 1) : 0);
			EnsureUnitCircle(num3);
			EnsureVertexCapacity(num11);
			EnsureScaledUCoords(num3, SanitizeTiling(_uvTilesAround));
			EnsureArray(ref _columnArcs, num4);
			EnsureArray(ref _previousRingWorld, num4);
			bool flag3 = _uvMode == SnakeTubeUvMode.NormalizedLength;
			bool flag4 = _uvMode == SnakeTubeUvMode.NormalizedPerColumn;
			bool flag5 = _uvMode == SnakeTubeUvMode.RestLength;
			float num12 = SanitizeTiling(_uvTilesAlongBody);
			float num13 = SanitizeTiling(_uvTilesPerSegment);
			float num14 = SanitizeTiling(_uvTilesPerUnit);
			bool uvSwapAxes = _uvSwapAxes;
			Vector2 uvOffset = _uvOffset;
			Vector2 vector = ((!flag4) ? uvOffset : (uvSwapAxes ? new Vector2(0f, uvOffset.y) : new Vector2(uvOffset.x, 0f)));
			Vector3 lossyScale = base.transform.lossyScale;
			bool flag6 = Mathf.Abs(lossyScale.x - 1f) > 0.0001f || Mathf.Abs(lossyScale.y - 1f) > 0.0001f || Mathf.Abs(lossyScale.z - 1f) > 0.0001f;
			float num15 = 0f;
			if (hasHeadProfile)
			{
				num15 = ((_headTextureBlendDistance == 0f) ? 0.5f : Mathf.Max(0f, _headTextureBlendDistance));
			}
			int num16 = ((_headWeldRings > 0) ? _headWeldRings : 6);
			if (hasHeadProfile)
			{
				SkinNeckLoop(num3);
			}
			Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
			Matrix4x4 transpose = base.transform.localToWorldMatrix.transpose;
			float boundsMinX = float.MaxValue;
			float boundsMinY = float.MaxValue;
			float boundsMinZ = float.MaxValue;
			float boundsMaxX = float.MinValue;
			float boundsMaxY = float.MinValue;
			float boundsMaxZ = float.MinValue;
			Vector3 zero = Vector3.zero;
			Vector3 normalWorld = Vector3.zero;
			Vector3 vector2 = Vector3.forward;
			Vector3 normalWorld2 = Vector3.zero;
			Vector3 capNormalWorld = Vector3.forward;
			BuildFramePoints(renderPointCount);
			Vector3 vector3 = Vector3.zero;
			Vector3 vector4 = Vector3.zero;
			for (int i = 0; i < renderPointCount; i++)
			{
				Vector3 vector5 = ComputeForward(_framePoints, i, renderPointCount, vector3);
				Vector3 vector6 = ComputeForward(_renderPoints, i, renderPointCount, vector5);
				vector4 = ((i != 0) ? TransportNormalInFramePlane(vector3, vector5, vector4) : Vector3.Cross(Vector3.up, vector5).normalized);
				vector3 = vector5;
				zero = vector4 - vector6 * Vector3.Dot(vector6, vector4);
				if (zero.sqrMagnitude < 1E-08f)
				{
					zero = InitialNormal(vector6);
				}
				else
				{
					zero.Normalize();
				}
				Vector3 vector7 = Vector3.Cross(vector6, zero);
				float num17 = ((num > 0f) ? (_cumulativeLengths[i] / num) : 0f);
				float num18 = num2 * EvaluateTaper(num17);
				Vector3 vector8 = _renderPoints[i];
				float a = ((num15 > 0f) ? (1f - Mathf.Clamp01(_cumulativeLengths[i] / num15)) : 0f);
				int num19 = i * num4;
				float num20 = (hasHeadProfile ? (1f - Mathf.Clamp01((float)i / (float)num16)) : 0f);
				if (hasHeadProfile && i == 0)
				{
					CaptureNeckFrame(num3, zero, vector7, vector6);
				}
				Vector3 vector9 = worldToLocalMatrix.MultiplyPoint3x4(vector8);
				Vector3 vector10 = worldToLocalMatrix.MultiplyVector(zero);
				Vector3 vector11 = worldToLocalMatrix.MultiplyVector(vector7);
				Vector3 vector12 = transpose.MultiplyVector(zero);
				Vector3 vector13 = transpose.MultiplyVector(vector7);
				Vector3 vector14 = vector10 * num18;
				Vector3 vector15 = vector11 * num18;
				Color color = new Color(1f, 1f, 1f, a);
				bool flag7 = num20 > 0f;
				bool flag8 = flag7 && _weldBlendsHeadNormals;
				float num21 = (flag5 ? (_renderRest[i] * num13) : (num17 * num12));
				for (int j = 0; j <= num3; j++)
				{
					Vector2 vector16 = _unitCircle[j];
					float x = vector16.x;
					float y = vector16.y;
					Vector3 vector17 = zero * x + vector7 * y;
					Vector3 offsetWorld = vector17 * num18;
					Vector3 normalWorldOut = vector17;
					Vector3 position;
					if (flag7)
					{
						BlendNeckProfile(j, num20, i, zero, vector7, vector6, ref offsetWorld, ref normalWorldOut);
						position = worldToLocalMatrix.MultiplyPoint3x4(vector8 + offsetWorld);
					}
					else
					{
						position = vector9 + vector14 * x + vector15 * y;
					}
					Vector3 vector18 = vector8 + offsetWorld;
					if (i == 0)
					{
						_columnArcs[j] = 0f;
					}
					else
					{
						Vector3 vector19 = _previousRingWorld[j];
						float num22 = vector18.x - vector19.x;
						float num23 = vector18.y - vector19.y;
						float num24 = vector18.z - vector19.z;
						_columnArcs[j] += Mathf.Sqrt(num22 * num22 + num23 * num23 + num24 * num24);
					}
					_previousRingWorld[j] = vector18;
					Vector3 normal = (flag8 ? transpose.MultiplyVector(normalWorldOut) : (vector12 * x + vector13 * y));
					Vector3 vector20 = vector11 * x - vector10 * y;
					if (flag6 || flag8)
					{
						normal.Normalize();
						vector20.Normalize();
					}
					float num25 = ((flag3 || flag5) ? num21 : ((!flag4) ? (_columnArcs[j] * num14) : _columnArcs[j]));
					float num26 = _uScaled[j];
					ref TubeVertex reference = ref _tubeVertices[num19 + j];
					reference.Position = position;
					reference.Normal = normal;
					reference.Tangent = new Vector4(vector20.x, vector20.y, vector20.z, -1f);
					reference.Color = color;
					reference.Uv = (uvSwapAxes ? new Vector2(num25 + vector.x, num26 + vector.y) : new Vector2(num26 + vector.x, num25 + vector.y));
					if (position.x < boundsMinX)
					{
						boundsMinX = position.x;
					}
					if (position.x > boundsMaxX)
					{
						boundsMaxX = position.x;
					}
					if (position.y < boundsMinY)
					{
						boundsMinY = position.y;
					}
					if (position.y > boundsMaxY)
					{
						boundsMaxY = position.y;
					}
					if (position.z < boundsMinZ)
					{
						boundsMinZ = position.z;
					}
					if (position.z > boundsMaxZ)
					{
						boundsMaxZ = position.z;
					}
				}
				if (i == 0)
				{
					normalWorld = zero;
					vector2 = vector6;
				}
				if (i == renderPointCount - 1)
				{
					normalWorld2 = zero;
					capNormalWorld = vector6;
				}
			}
			if (flag4)
			{
				NormalizeColumnUv(renderPointCount, num3, num4, num12, uvSwapAxes, uvOffset);
			}
			if (flag)
			{
				WriteCap(num7, num8, 0, num3, num4, -vector2, normalWorld, _renderPoints[0], worldToLocalMatrix, transpose, ref boundsMinX, ref boundsMinY, ref boundsMinZ, ref boundsMaxX, ref boundsMaxY, ref boundsMaxZ);
			}
			if (flag2)
			{
				WriteCap(num9, num10, renderPointCount - 1, num3, num4, capNormalWorld, normalWorld2, _renderPoints[renderPointCount - 1], worldToLocalMatrix, transpose, ref boundsMinX, ref boundsMinY, ref boundsMinZ, ref boundsMaxX, ref boundsMaxY, ref boundsMaxZ);
			}
			bool flag9 = base.transform.localToWorldMatrix.determinant < 0f;
			int num27;
			if (_topologyPathCount == renderPointCount && _topologySides == num3 && _topologyHasHeadCap == flag && _topologyHasTipCap == flag2 && _topologyFlipWinding == flag9)
			{
				num27 = ((_topologyWelding != hasHeadProfile) ? 1 : 0);
				if (num27 == 0)
				{
					goto IL_08ae;
				}
			}
			else
			{
				num27 = 1;
			}
			BuildTriangles(renderPointCount, num3, num4, flag, flag2, num7, num8, num9, num10, flag9);
			_topologyPathCount = renderPointCount;
			_topologySides = num3;
			_topologyHasHeadCap = flag;
			_topologyHasTipCap = flag2;
			_topologyFlipWinding = flag9;
			_topologyWelding = hasHeadProfile;
			goto IL_08ae;
			IL_08ae:
			if (num27 != 0)
			{
				_mesh.Clear();
				_mesh.SetVertexBufferParams(num11, _vertexLayout);
				_mesh.SetIndexBufferParams(_indexCount, IndexFormat.UInt32);
			}
			_mesh.SetVertexBufferData(_tubeVertices, 0, 0, num11, 0, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
			if (num27 != 0)
			{
				_mesh.SetIndexBufferData(_triangles, 0, 0, _indexCount, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
				_mesh.subMeshCount = 1;
				_mesh.SetSubMesh(0, new SubMeshDescriptor(0, _indexCount), MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
			}
			Vector3 vector21 = new Vector3(boundsMinX, boundsMinY, boundsMinZ);
			Vector3 vector22 = new Vector3(boundsMaxX, boundsMaxY, boundsMaxZ);
			_mesh.bounds = new Bounds((vector21 + vector22) * 0.5f, vector22 - vector21);
			_hasBuiltMesh = true;
		}

		private void NormalizeColumnUv(int pathCount, int sides, int ringStride, float tilesAlongBody, bool swapUvAxes, Vector2 uvOffset)
		{
			for (int i = 0; i <= sides; i++)
			{
				float num = _columnArcs[i];
				float num2 = ((num > 1E-05f) ? (tilesAlongBody / num) : 0f);
				for (int j = 0; j < pathCount; j++)
				{
					ref TubeVertex reference = ref _tubeVertices[j * ringStride + i];
					if (swapUvAxes)
					{
						reference.Uv = new Vector2(reference.Uv.x * num2 + uvOffset.x, reference.Uv.y);
					}
					else
					{
						reference.Uv = new Vector2(reference.Uv.x, reference.Uv.y * num2 + uvOffset.y);
					}
				}
			}
		}

		private void WriteCap(int ringStart, int centerIndex, int sourceRing, int sides, int ringStride, Vector3 capNormalWorld, Vector3 normalWorld, Vector3 centerWorld, Matrix4x4 worldToLocal, Matrix4x4 normalToLocal, ref float boundsMinX, ref float boundsMinY, ref float boundsMinZ, ref float boundsMaxX, ref float boundsMaxY, ref float boundsMaxZ)
		{
			Vector3 normalized = normalToLocal.MultiplyVector(capNormalWorld).normalized;
			Vector3 normalized2 = worldToLocal.MultiplyVector(normalWorld).normalized;
			Vector4 tangent = new Vector4(normalized2.x, normalized2.y, normalized2.z, -1f);
			int num = sourceRing * ringStride;
			for (int i = 0; i <= sides; i++)
			{
				Vector2 vector = _unitCircle[i];
				ref TubeVertex reference = ref _tubeVertices[ringStart + i];
				reference.Position = _tubeVertices[num + i].Position;
				reference.Normal = normalized;
				reference.Tangent = tangent;
				reference.Color = _opaqueBodyColor;
				reference.Uv = new Vector2(0.5f + vector.x * 0.5f, 0.5f + vector.y * 0.5f) + _uvOffset;
			}
			Vector3 position = worldToLocal.MultiplyPoint3x4(centerWorld);
			ref TubeVertex reference2 = ref _tubeVertices[centerIndex];
			reference2.Position = position;
			reference2.Normal = normalized;
			reference2.Tangent = tangent;
			reference2.Color = _opaqueBodyColor;
			reference2.Uv = new Vector2(0.5f, 0.5f) + _uvOffset;
			if (position.x < boundsMinX)
			{
				boundsMinX = position.x;
			}
			if (position.x > boundsMaxX)
			{
				boundsMaxX = position.x;
			}
			if (position.y < boundsMinY)
			{
				boundsMinY = position.y;
			}
			if (position.y > boundsMaxY)
			{
				boundsMaxY = position.y;
			}
			if (position.z < boundsMinZ)
			{
				boundsMinZ = position.z;
			}
			if (position.z > boundsMaxZ)
			{
				boundsMaxZ = position.z;
			}
		}

		private void BuildTriangles(int pathCount, int sides, int ringStride, bool hasHeadCap, bool hasTipCap, int headCapStart, int headCapCenter, int tipCapStart, int tipCapCenter, bool flipWinding)
		{
			int num = (pathCount - 1) * sides * 6 + (hasHeadCap ? (sides * 3) : 0) + (hasTipCap ? (sides * 3) : 0);
			if (_triangles.Length < num)
			{
				_triangles = new int[num];
			}
			int num2 = 0;
			for (int i = 0; i < pathCount - 1; i++)
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
					_triangles[num2++] = num7;
					_triangles[num2++] = num6;
					_triangles[num2++] = num6;
					_triangles[num2++] = num7;
					_triangles[num2++] = num8;
				}
			}
			if (hasHeadCap)
			{
				for (int k = 0; k < sides; k++)
				{
					_triangles[num2++] = headCapCenter;
					_triangles[num2++] = headCapStart + k + 1;
					_triangles[num2++] = headCapStart + k;
				}
			}
			if (hasTipCap)
			{
				for (int l = 0; l < sides; l++)
				{
					_triangles[num2++] = tipCapCenter;
					_triangles[num2++] = tipCapStart + l;
					_triangles[num2++] = tipCapStart + l + 1;
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

		private void BuildFramePoints(int pathCount)
		{
			if (pathCount != 0)
			{
				EnsureArray(ref _framePoints, pathCount);
				float y = _renderPoints[0].y;
				for (int i = 0; i < pathCount; i++)
				{
					Vector3 vector = _renderPoints[i];
					_framePoints[i] = new Vector3(vector.x, y, vector.z);
				}
			}
		}

		private static Vector3 ComputeForward(Vector3[] points, int index, int pathCount, Vector3 fallback)
		{
			Vector3 vector;
			if (index == 0)
			{
				vector = points[1] - points[0];
			}
			else if (index == pathCount - 1)
			{
				vector = points[pathCount - 1] - points[pathCount - 2];
			}
			else
			{
				Vector3 vector2 = points[index] - points[index - 1];
				Vector3 vector3 = points[index + 1] - points[index];
				bool num = vector2.sqrMagnitude >= 1E-08f;
				bool flag = vector3.sqrMagnitude >= 1E-08f;
				if (!(num && flag))
				{
					vector = ((!flag) ? vector2 : vector3);
				}
				else
				{
					vector = vector2.normalized + vector3.normalized;
					if (vector.sqrMagnitude < 0.01f && fallback.sqrMagnitude >= 1E-08f)
					{
						return fallback;
					}
				}
			}
			if (vector.sqrMagnitude >= 1E-08f)
			{
				return vector.normalized;
			}
			if (fallback.sqrMagnitude >= 1E-08f)
			{
				return fallback;
			}
			for (int i = index + 1; i < pathCount; i++)
			{
				Vector3 vector4 = points[i] - points[index];
				if (vector4.sqrMagnitude >= 1E-08f)
				{
					return vector4.normalized;
				}
			}
			return Vector3.forward;
		}

		private static float SanitizeTiling(float tiling)
		{
			if (tiling != 0f)
			{
				return tiling;
			}
			return 1f;
		}

		private void CaptureNeckFrame(int sides, Vector3 normalWorld, Vector3 binormalWorld, Vector3 forward)
		{
			Vector3 vector = _renderPoints[0];
			for (int i = 0; i <= sides; i++)
			{
				Vector3 lhs = _neckWorldPositions[i] - vector;
				_neckFrameOffsets[i] = new Vector3(0f - Vector3.Dot(lhs, binormalWorld), Vector3.Dot(lhs, normalWorld), Vector3.Dot(lhs, forward));
				Vector3 lhs2 = _neckWorldNormals[i];
				_neckFrameNormals[i] = new Vector3(0f - Vector3.Dot(lhs2, binormalWorld), Vector3.Dot(lhs2, normalWorld), Vector3.Dot(lhs2, forward));
			}
		}

		private void BlendNeckProfile(int side, float weld, int ringIndex, Vector3 normalWorld, Vector3 binormalWorld, Vector3 forward, ref Vector3 offsetWorld, ref Vector3 normalWorldOut)
		{
			Vector3 b;
			Vector3 b2;
			if (ringIndex == 0)
			{
				b = _neckWorldPositions[side] - _renderPoints[0];
				b2 = _neckWorldNormals[side];
			}
			else
			{
				Vector3 vector = _neckFrameOffsets[side];
				Vector3 vector2 = _neckFrameNormals[side];
				b = normalWorld * vector.y - binormalWorld * vector.x + forward * vector.z;
				b2 = normalWorld * vector2.y - binormalWorld * vector2.x + forward * vector2.z;
			}
			offsetWorld = Vector3.Lerp(offsetWorld, b, weld);
			if (_weldBlendsHeadNormals)
			{
				normalWorldOut = Vector3.Slerp(normalWorldOut, b2, weld);
			}
		}

		private Vector3 TransportNormalInFramePlane(Vector3 previousForward, Vector3 forward, Vector3 previousNormal)
		{
			Vector3 up = Vector3.up;
			float x = Vector3.Dot(previousForward, forward);
			Vector3 rhs = Quaternion.AngleAxis(Mathf.Atan2(Vector3.Dot(Vector3.Cross(previousForward, forward), up), x) * 57.29578f, up) * previousNormal;
			rhs -= forward * Vector3.Dot(forward, rhs);
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
			if (_unitCircleSides != sides || _unitCircle.Length != sides + 1)
			{
				_unitCircle = new Vector2[sides + 1];
				_uCoords = new float[sides + 1];
				for (int i = 0; i <= sides; i++)
				{
					float num = (float)i / (float)sides;
					float f = num * MathF.PI * 2f;
					_unitCircle[i] = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
					_uCoords[i] = num;
				}
				_unitCircleSides = sides;
				_uScaledTiles = float.NaN;
			}
		}

		private void EnsureScaledUCoords(int sides, float tilesAround)
		{
			if (_uScaledTiles != tilesAround || _uScaled.Length != sides + 1)
			{
				EnsureArray(ref _uScaled, sides + 1);
				for (int i = 0; i <= sides; i++)
				{
					_uScaled[i] = _uCoords[i] * tilesAround;
				}
				_uScaledTiles = tilesAround;
			}
		}

		public void RebuildHeadProfile()
		{
			BuildHeadProfile();
		}

		private void EnsureVertexCapacity(int count)
		{
			EnsureArray(ref _tubeVertices, count);
		}

		private static void EnsureArray<T>(ref T[] array, int size)
		{
			if (array == null || array.Length < size)
			{
				array = new T[size];
			}
		}

		private void BuildCumulativeLengths(int pathCount)
		{
			EnsureArray(ref _cumulativeLengths, pathCount);
			_cumulativeLengths[0] = 0f;
			for (int i = 1; i < pathCount; i++)
			{
				float num = Vector3.Distance(_renderPoints[i - 1], _renderPoints[i]);
				_cumulativeLengths[i] = _cumulativeLengths[i - 1] + num;
			}
		}

		private float EvaluateTaper(float normalizedLength)
		{
			float taperStartPercent = _taperStartPercent;
			if (taperStartPercent >= 1f)
			{
				return 1f;
			}
			if (normalizedLength <= taperStartPercent)
			{
				return 1f;
			}
			if (taperStartPercent <= 0f)
			{
				return 1f - normalizedLength;
			}
			return 1f - (normalizedLength - taperStartPercent) / (1f - taperStartPercent);
		}
	}
}
