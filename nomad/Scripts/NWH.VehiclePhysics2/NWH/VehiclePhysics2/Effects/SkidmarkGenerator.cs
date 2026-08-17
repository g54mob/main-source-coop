using System.Collections.Generic;
using NWH.Common.Vehicles;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;
using UnityEngine.Rendering;

namespace NWH.VehiclePhysics2.Effects
{
	public class SkidmarkGenerator
	{
		private readonly Bounds _bounds = new Bounds(Vector3.zero, Vector3.one * 1000f);

		private readonly Vector2 _vector00 = new Vector2(0f, 0f);

		private readonly Vector2 _vector01 = new Vector2(0f, 1f);

		private readonly Vector2 _vector10 = new Vector2(1f, 0f);

		private readonly Vector2 _vector11 = new Vector2(1f, 1f);

		private SkidmarkRect _currentRect;

		private SkidmarkRect _previousRect;

		private bool _isGrounded;

		private bool _wasGrounded;

		private float _markWidth = -1f;

		private MeshFilter _meshFilter;

		private MeshRenderer _meshRenderer;

		private float _minSqrDistance;

		private SkidmarkDestroy _skidmarkDestroy;

		private Mesh _skidmarkMesh;

		private WheelComponent _targetWheelComponent;

		private bool _surfaceChangedFlag;

		private bool _groundedFlag;

		private bool _intensityFlag;

		private Color[] _colors;

		private Vector3[] _vertices;

		private Vector3[] _normals;

		private int[] _triangles;

		private Vector4[] _tangents;

		private Vector2[] _uvs;

		private int _triCount;

		private Queue<GameObject> _skidObjectQueue = new Queue<GameObject>();

		private GameObject _currentSkidObject;

		private SkidmarkManager _skidmarkManager;

		public void Initialize(SkidmarkManager skidmarkManager, WheelComponent wheelComponent)
		{
			_skidmarkManager = skidmarkManager;
			_targetWheelComponent = wheelComponent;
			_minSqrDistance = skidmarkManager.minDistance * skidmarkManager.minDistance;
			_triCount = 0;
			_skidObjectQueue = new Queue<GameObject>();
			_markWidth = wheelComponent.wheelUAPI.Width;
			_currentRect.intensity = 0f;
			_currentRect.position = Vector3.zero;
			_currentRect.position = Vector3.up;
			_currentRect.color = Color.clear;
			_currentRect.surfaceMapIndex = -999;
			_previousRect = _currentRect;
			GenerateNewSkidmark();
		}

		public void Update(int surfaceMapIndex, float targetIntensity, float dt)
		{
			_previousRect = _currentRect;
			_currentRect.intensity = targetIntensity;
			_currentRect.color.a = _currentRect.intensity;
			_currentRect.surfaceMapIndex = surfaceMapIndex;
			_wasGrounded = _isGrounded;
			_isGrounded = _targetWheelComponent.wheelUAPI.IsGrounded;
			if (surfaceMapIndex >= 0 && _currentRect.surfaceMapIndex != _previousRect.surfaceMapIndex)
			{
				_surfaceChangedFlag = true;
			}
			if (_isGrounded && !_wasGrounded)
			{
				_groundedFlag = true;
			}
			if (_currentRect.intensity > _skidmarkManager.lowerIntensityThreshold && _previousRect.intensity <= _skidmarkManager.lowerIntensityThreshold)
			{
				_intensityFlag = true;
			}
			Vector3 currentPosition = GetCurrentPosition(dt);
			if ((currentPosition - _previousRect.position).sqrMagnitude < _minSqrDistance || targetIntensity <= _skidmarkManager.lowerIntensityThreshold)
			{
				return;
			}
			if (_intensityFlag || _groundedFlag)
			{
				_previousRect.position -= _currentRect.forwardDirection * 0.001f;
			}
			if (_currentRect.surfaceMapIndex >= 0 && _isGrounded)
			{
				if (_surfaceChangedFlag)
				{
					GenerateNewSkidmark();
				}
				Vector3 hitNormal = _targetWheelComponent.wheelUAPI.HitNormal;
				_currentRect.position = currentPosition;
				_currentRect.normal = hitNormal;
				if (_intensityFlag || _groundedFlag)
				{
					_previousRect.position = _currentRect.position - _targetWheelComponent.wheelUAPI.transform.forward * 0.0001f;
					_previousRect.color = Color.clear;
					_previousRect.intensity = 0f;
				}
				_currentRect.forwardDirection = _currentRect.position - _previousRect.position;
				_currentRect.rightDirection = Vector3.Cross(_currentRect.forwardDirection, _currentRect.normal).normalized;
				_currentRect.color.a = _currentRect.intensity;
				AppendGeometry();
				if (_triCount + 2 >= _skidmarkManager.maxTrisPerSection)
				{
					GenerateNewSkidmark();
				}
				_surfaceChangedFlag = false;
				_intensityFlag = false;
				_groundedFlag = false;
			}
		}

		private Vector3 GetCurrentPosition(float dt)
		{
			Transform transform = _targetWheelComponent.wheelUAPI.transform;
			Vector3 hitPoint = _targetWheelComponent.wheelUAPI.HitPoint;
			Vector3 position = transform.InverseTransformPoint(hitPoint);
			position.x = _targetWheelComponent.wheelUAPI.LateralSpeed * dt * 0.5f;
			position.z = _targetWheelComponent.wheelUAPI.LongitudinalSpeed * dt * 0.5f;
			hitPoint = transform.TransformPoint(position);
			return hitPoint + _targetWheelComponent.wheelUAPI.HitNormal * _skidmarkManager.groundOffset;
		}

		public void GenerateNewSkidmark()
		{
			if (_skidmarkDestroy != null)
			{
				_skidmarkDestroy.skidmarkIsBeingUsed = false;
			}
			WheelUAPI wheelUAPI = _targetWheelComponent.wheelUAPI;
			_currentSkidObject = new GameObject("SkidmarkContainer");
			_currentSkidObject.transform.parent = _skidmarkManager.skidmarkContainer.transform;
			_currentSkidObject.transform.position = wheelUAPI.HitPoint;
			_currentSkidObject.isStatic = true;
			_skidmarkDestroy = _currentSkidObject.AddComponent<SkidmarkDestroy>();
			_skidmarkDestroy.targetTransform = _targetWheelComponent.wheelUAPI.transform;
			_skidmarkDestroy.distanceThreshold = _skidmarkManager.skidmarkDestroyDistance;
			_skidmarkDestroy.timeThreshold = _skidmarkManager.skidmarkDestroyTime;
			_skidmarkDestroy.skidmarkIsBeingUsed = true;
			_meshRenderer = _currentSkidObject.GetComponent<MeshRenderer>();
			if (_meshRenderer == null)
			{
				_meshRenderer = _currentSkidObject.AddComponent<MeshRenderer>();
			}
			if (_targetWheelComponent.surfacePreset != null)
			{
				_meshRenderer.material = _targetWheelComponent.surfacePreset.skidmarkMaterial;
			}
			else
			{
				_meshRenderer.material = _skidmarkManager.fallbackMaterial;
			}
			_meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			_meshRenderer.lightProbeUsage = LightProbeUsage.Off;
			_meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
			_meshFilter = _currentSkidObject.AddComponent<MeshFilter>();
			_vertices = new Vector3[_skidmarkManager.maxTrisPerSection * 3];
			_normals = new Vector3[_skidmarkManager.maxTrisPerSection * 3];
			_tangents = new Vector4[_skidmarkManager.maxTrisPerSection * 3];
			_colors = new Color[_skidmarkManager.maxTrisPerSection * 3];
			_uvs = new Vector2[_skidmarkManager.maxTrisPerSection * 3];
			_triangles = new int[_skidmarkManager.maxTrisPerSection * 3];
			_skidmarkMesh = new Mesh();
			float num = _skidmarkManager.minDistance * (float)_skidmarkManager.maxTrisPerSection * 1.1f;
			Bounds bounds = new Bounds(size: new Vector3(num, num, num), center: _currentSkidObject.transform.position);
			_skidmarkMesh.bounds = bounds;
			_skidmarkMesh.MarkDynamic();
			_skidmarkMesh.name = "SkidmarkMesh";
			_meshFilter.mesh = _skidmarkMesh;
			_triCount = 0;
			_skidObjectQueue.Enqueue(_currentSkidObject);
			int count = _skidObjectQueue.Count;
			if (count <= 1 || count * _skidmarkManager.maxTrisPerSection <= _skidmarkManager.maxTotalTris)
			{
				return;
			}
			GameObject gameObject = _skidObjectQueue.Dequeue();
			if (gameObject != null)
			{
				SkidmarkDestroy component = gameObject.GetComponent<SkidmarkDestroy>();
				if (component != null)
				{
					component.destroyFlag = true;
				}
			}
		}

		private void AppendGeometry()
		{
			Transform transform = _currentSkidObject.transform;
			int num = _triCount * 2;
			int num2 = _triCount * 3;
			int num3 = num;
			int num4 = num + 1;
			int num5 = num + 2;
			int num6 = num + 3;
			Vector3 position = _previousRect.position + _previousRect.rightDirection * (_markWidth * 0.5f);
			Vector3 position2 = _previousRect.position - _previousRect.rightDirection * (_markWidth * 0.5f);
			Vector3 position3 = _currentRect.position + _currentRect.rightDirection * (_markWidth * 0.5f);
			Vector3 position4 = _currentRect.position - _currentRect.rightDirection * (_markWidth * 0.5f);
			Vector3 vector = transform.InverseTransformPoint(position);
			Vector3 vector2 = transform.InverseTransformPoint(position2);
			Vector3 vector3 = transform.InverseTransformPoint(position3);
			Vector3 vector4 = transform.InverseTransformPoint(position4);
			_vertices[num3] = vector;
			_vertices[num4] = vector2;
			_vertices[num5] = vector3;
			_vertices[num6] = vector4;
			Vector3 vector5 = _currentSkidObject.transform.InverseTransformDirection(_previousRect.normal);
			Vector3 vector6 = _currentSkidObject.transform.InverseTransformDirection(_currentRect.normal);
			_normals[num3] = vector5;
			_normals[num4] = vector5;
			_normals[num5] = vector6;
			_normals[num6] = vector6;
			Vector3 direction = new Vector4(_currentRect.rightDirection.x, _currentRect.rightDirection.y, _currentRect.rightDirection.z, 1f);
			Vector3 direction2 = new Vector4(_previousRect.rightDirection.x, _previousRect.rightDirection.y, _previousRect.rightDirection.z, 1f);
			Vector3 vector7 = transform.InverseTransformDirection(direction);
			Vector3 vector8 = transform.InverseTransformDirection(direction2);
			_tangents[num3] = vector8;
			_tangents[num4] = vector8;
			_tangents[num5] = vector7;
			_tangents[num6] = vector7;
			_colors[num3] = _previousRect.color;
			_colors[num4] = _previousRect.color;
			_colors[num5] = _currentRect.color;
			_colors[num6] = _currentRect.color;
			_uvs[num3] = _vector00;
			_uvs[num4] = _vector10;
			_uvs[num5] = _vector01;
			_uvs[num6] = _vector11;
			_triangles[num2] = num3;
			_triangles[num2 + 2] = num4;
			_triangles[num2 + 1] = num5;
			_triangles[num2 + 3] = num5;
			_triangles[num2 + 5] = num4;
			_triangles[num2 + 4] = num6;
			_skidmarkMesh.vertices = _vertices;
			_skidmarkMesh.normals = _normals;
			_skidmarkMesh.tangents = _tangents;
			_skidmarkMesh.triangles = _triangles;
			_skidmarkMesh.colors = _colors;
			_skidmarkMesh.uv = _uvs;
			_skidmarkMesh.bounds = _bounds;
			_meshFilter.mesh = _skidmarkMesh;
			_triCount += 2;
		}
	}
}
