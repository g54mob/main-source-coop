using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Features.PhysicsVolumeModule.Scripts
{
	[RequireComponent(typeof(SplineContainer))]
	public class CartFlowZone : MonoBehaviour
	{
		[SerializeField]
		private SplineContainer _splineContainer;

		[Header("Influence")]
		[Tooltip("Full-strength channel core around the curve, meters.")]
		[SerializeField]
		private float _coreRadius = 1f;

		[Tooltip("Strength falls smoothly to zero between core and this outer radius, meters.")]
		[SerializeField]
		private float _influenceRadius = 3f;

		[Tooltip("Target flow speed along the curve tangent at full strength, m/s.")]
		[SerializeField]
		private float _flowSpeed = 2f;

		[Tooltip("The cart's forward must be within this yaw angle of the flow tangent for the channel's DRIVE to take hold - a cart parked perpendicular to a stair channel must not get dragged in. Steering and momentum redirection keep full authority inside the cone; only the forward push ramps in from the cone edge to full at half of it.")]
		[SerializeField]
		private float _acceptanceAngle = 60f;

		[Header("Sampling")]
		[Tooltip("Spacing of the baked polyline, meters. The channel is metres wide, so this is what decides sampling cost, not accuracy.")]
		[SerializeField]
		private float _bakeSpacing = 0.25f;

		private const int MAX_BAKED_SEGMENTS = 512;

		[SerializeField]
		[HideInInspector]
		private Vector3[] _bakedPoints;

		[SerializeField]
		[HideInInspector]
		private float[] _bakedArcLengths;

		[SerializeField]
		[HideInInspector]
		private int _bakedSignature;

		private Bounds _bakedBounds;

		private bool _hasBakedBounds;

		public float FlowSpeed => _flowSpeed;

		public float AcceptanceAngle => _acceptanceAngle;

		private void Reset()
		{
			_splineContainer = GetComponent<SplineContainer>();
			Spline spline = _splineContainer.Spline;
			if (spline != null && spline.Count <= 0)
			{
				spline.Add(new BezierKnot(new float3(0f, 0f, 0f)));
				spline.Add(new BezierKnot(new float3(0f, 0f, 5f)));
				spline.Add(new BezierKnot(new float3(0f, 0f, 10f)));
				spline.SetTangentMode(TangentMode.AutoSmooth);
			}
		}

		private void Awake()
		{
			if (_splineContainer == null)
			{
				_splineContainer = GetComponent<SplineContainer>();
			}
			EnsureBaked();
		}

		private void OnEnable()
		{
			EnsureBaked();
			CartFlowZoneProvider.Register(this);
		}

		private void OnDisable()
		{
			CartFlowZoneProvider.Unregister(this);
		}

		public bool TrySample(Vector3 worldPoint, float lookAheadDistance, out Vector3 closestPoint, out Vector3 guidePoint, out Vector3 guideTangent, out float strength)
		{
			closestPoint = worldPoint;
			guidePoint = worldPoint;
			guideTangent = Vector3.forward;
			strength = 0f;
			Spline spline = ((_splineContainer != null) ? _splineContainer.Spline : null);
			if (spline == null || spline.Count < 2)
			{
				return false;
			}
			if (!HasUsableBake())
			{
				EnsureBaked();
			}
			if (!HasUsableBake())
			{
				return false;
			}
			if (!_hasBakedBounds)
			{
				RebuildBounds();
			}
			Vector3 vector = base.transform.InverseTransformPoint(worldPoint);
			Vector3 lossyScale = base.transform.lossyScale;
			float num = Mathf.Max(0.0001f, Mathf.Min(Mathf.Abs(lossyScale.x), Mathf.Min(Mathf.Abs(lossyScale.y), Mathf.Abs(lossyScale.z))));
			float num2 = _influenceRadius / num;
			if (_bakedBounds.SqrDistance(vector) >= num2 * num2)
			{
				return false;
			}
			int num3 = 0;
			float t = 0f;
			float num4 = float.MaxValue;
			Vector3 position = _bakedPoints[0];
			for (int i = 0; i < _bakedPoints.Length - 1; i++)
			{
				Vector3 vector2 = _bakedPoints[i];
				Vector3 vector3 = _bakedPoints[i + 1] - vector2;
				float sqrMagnitude = vector3.sqrMagnitude;
				float num5 = ((sqrMagnitude > 1E-08f) ? Mathf.Clamp01(Vector3.Dot(vector - vector2, vector3) / sqrMagnitude) : 0f);
				Vector3 vector4 = vector2 + vector3 * num5;
				float sqrMagnitude2 = (vector - vector4).sqrMagnitude;
				if (!(sqrMagnitude2 >= num4))
				{
					num4 = sqrMagnitude2;
					num3 = i;
					t = num5;
					position = vector4;
				}
			}
			closestPoint = base.transform.TransformPoint(position);
			float num6 = Vector3.Distance(worldPoint, closestPoint);
			if (num6 >= _influenceRadius)
			{
				return false;
			}
			float num7 = Mathf.Lerp(_bakedArcLengths[num3], _bakedArcLengths[num3 + 1], t);
			float num8 = Mathf.Min(b: _bakedArcLengths[_bakedArcLengths.Length - 1], a: num7 + Mathf.Max(0f, lookAheadDistance));
			int j;
			for (j = num3; j < _bakedPoints.Length - 2 && _bakedArcLengths[j + 1] < num8; j++)
			{
			}
			float num9 = _bakedArcLengths[j + 1] - _bakedArcLengths[j];
			float num10 = ((num9 > 1E-05f) ? Mathf.Clamp01((num8 - _bakedArcLengths[j]) / num9) : 0f);
			Vector3 vector5 = _bakedPoints[j + 1] - _bakedPoints[j];
			guidePoint = base.transform.TransformPoint(_bakedPoints[j] + vector5 * num10);
			if (vector5.sqrMagnitude < 1E-12f)
			{
				return false;
			}
			Vector3 vector6 = base.transform.TransformDirection(vector5.normalized);
			if (vector6.sqrMagnitude < 1E-08f)
			{
				return false;
			}
			guideTangent = vector6.normalized;
			strength = ((num6 <= _coreRadius) ? 1f : (1f - Mathf.SmoothStep(0f, 1f, (num6 - _coreRadius) / (_influenceRadius - _coreRadius))));
			return strength > 0f;
		}

		private bool HasUsableBake()
		{
			if (_bakedPoints != null && _bakedPoints.Length >= 2 && _bakedArcLengths != null)
			{
				return _bakedArcLengths.Length == _bakedPoints.Length;
			}
			return false;
		}

		private void EnsureBaked()
		{
			Spline spline = ((_splineContainer != null) ? _splineContainer.Spline : null);
			if (spline != null && spline.Count >= 2)
			{
				int num = ComputeSignature(spline);
				if (!HasUsableBake() || _bakedSignature != num)
				{
					Bake(spline, num);
				}
			}
		}

		private int ComputeSignature(Spline spline)
		{
			int num = 17;
			num = num * 31 + spline.Count;
			num = num * 31 + spline.Closed.GetHashCode();
			num = num * 31 + _bakeSpacing.GetHashCode();
			num = num * 31 + base.transform.lossyScale.GetHashCode();
			for (int i = 0; i < spline.Count; i++)
			{
				BezierKnot bezierKnot = spline[i];
				num = num * 31 + bezierKnot.Position.GetHashCode();
				num = num * 31 + bezierKnot.TangentIn.GetHashCode();
				num = num * 31 + bezierKnot.TangentOut.GetHashCode();
				num = num * 31 + bezierKnot.Rotation.value.GetHashCode();
			}
			return num;
		}

		private void RebuildBounds()
		{
			_bakedBounds = new Bounds(_bakedPoints[0], Vector3.zero);
			for (int i = 1; i < _bakedPoints.Length; i++)
			{
				_bakedBounds.Encapsulate(_bakedPoints[i]);
			}
			_hasBakedBounds = true;
		}

		private void Bake(Spline spline, int signature)
		{
			Vector3 lossyScale = base.transform.lossyScale;
			float num = Mathf.Max(0.0001f, Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Max(Mathf.Abs(lossyScale.y), Mathf.Abs(lossyScale.z))));
			int num2 = Mathf.Clamp(Mathf.CeilToInt(spline.GetLength() * num / Mathf.Max(0.05f, _bakeSpacing)), 1, 512);
			int num3 = num2 + 1;
			_bakedPoints = new Vector3[num3];
			_bakedArcLengths = new float[num3];
			for (int i = 0; i < num3; i++)
			{
				_bakedPoints[i] = spline.EvaluatePosition((float)i / (float)num2);
			}
			_bakedArcLengths[0] = 0f;
			for (int j = 1; j < num3; j++)
			{
				_bakedArcLengths[j] = _bakedArcLengths[j - 1] + Vector3.Distance(_bakedPoints[j - 1], _bakedPoints[j]);
			}
			RebuildBounds();
			_bakedSignature = signature;
		}
	}
}
