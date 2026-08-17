using System.Collections.Generic;
using UnityEngine;

namespace NWH.WheelController3D
{
	[RequireComponent(typeof(WheelController))]
	public class StandardGroundDetection : GroundDetectionBase
	{
		public bool forceMulticast;

		private RaycastHit _castResult;

		private WheelController _wheelController;

		private Transform _transform;

		private const int MaxHits = 32;

		private RaycastHit[] _raycastHits = new RaycastHit[32];

		private RaycastHit[] _spherecastHits = new RaycastHit[32];

		private List<RaycastHit> _allHits = new List<RaycastHit>(32);

		private void Awake()
		{
			_wheelController = GetComponent<WheelController>();
			_transform = base.transform;
		}

		public override bool WheelCast(in Vector3 origin, in Vector3 direction, in float distance, in float radius, in float width, ref WheelHit wheelHit, LayerMask layerMask)
		{
			bool flag = WheelCastSingleSphere(origin, direction, distance, radius, width, ref _castResult, layerMask);
			if (forceMulticast || !flag)
			{
				flag = WheelCastMultiSphere(origin, direction, distance, radius, width, ref _castResult, layerMask);
			}
			if (flag)
			{
				wheelHit.point = _castResult.point;
				wheelHit.normal = _castResult.normal;
				wheelHit.collider = _castResult.collider;
			}
			return flag;
		}

		private bool WheelCastSingleSphere(Vector3 origin, Vector3 direction, float distance, float radius, float width, ref RaycastHit hit, LayerMask layerMask)
		{
			bool flag = width <= 0.01f;
			int num = 0;
			direction = Vector3.Normalize(direction);
			num = ((!flag) ? Physics.SphereCastNonAlloc(origin, radius, direction, _spherecastHits, distance, layerMask, QueryTriggerInteraction.Ignore) : Physics.RaycastNonAlloc(origin, direction, _raycastHits, distance, layerMask, QueryTriggerInteraction.Ignore));
			if (num > 0)
			{
				float num2 = 3.4028235E+38f;
				RaycastHit raycastHit = default(RaycastHit);
				RaycastHit[] array = (flag ? _raycastHits : _spherecastHits);
				for (int i = 0; i < num; i++)
				{
					RaycastHit raycastHit2 = array[i];
					if (!_wheelController.vehicleColliders.Contains(raycastHit2.collider))
					{
						Vector3 vector = raycastHit2.point - origin;
						Vector3 vector2 = _transform.InverseTransformVector(vector);
						float num3 = width * 0.5f;
						if (!(vector2.x < 0f - num3) && !(vector2.x > num3) && !(vector2.z > radius) && !(vector2.z < 0f - radius) && raycastHit2.distance < num2)
						{
							num2 = raycastHit2.distance;
							raycastHit = raycastHit2;
						}
					}
				}
				if (num2 != 3.4028235E+38f)
				{
					hit = raycastHit;
					return true;
				}
			}
			return false;
		}

		private bool WheelCastMultiSphere(Vector3 origin, Vector3 direction, float distance, float radius, float width, ref RaycastHit hit, LayerMask layerMask)
		{
			float num = width * 0.5f;
			direction = Vector3.Normalize(direction);
			bool flag = num <= 0.01f;
			int a = (flag ? 1 : Mathf.RoundToInt(radius / num * 2f));
			a = Mathf.Max(a, 3);
			a = ((a % 2 == 0) ? (a + 1) : a);
			float angle = 180f / (float)(a - 1);
			Vector3 up = _transform.up;
			Vector3 vector = _transform.forward * radius;
			Quaternion quaternion = Quaternion.AngleAxis(_wheelController.SteerAngle, up);
			Quaternion quaternion2 = Quaternion.AngleAxis(angle, _transform.right);
			Quaternion identity = Quaternion.identity;
			_allHits.Clear();
			for (int i = 0; i < a; i++)
			{
				Vector3 origin2 = origin + quaternion * identity * vector;
				int num2 = 0;
				RaycastHit[] array = (flag ? _raycastHits : _spherecastHits);
				num2 = ((!flag) ? Physics.SphereCastNonAlloc(origin2, num, direction, array, distance, layerMask, QueryTriggerInteraction.Ignore) : Physics.RaycastNonAlloc(origin2, direction, array, distance, layerMask, QueryTriggerInteraction.Ignore));
				for (int j = 0; j < num2; j++)
				{
					RaycastHit item = array[j];
					_allHits.Add(item);
				}
				identity *= quaternion2;
			}
			if (_allHits.Count > 0)
			{
				float num3 = 3.4028235E+38f;
				RaycastHit raycastHit = default(RaycastHit);
				for (int k = 0; k < _allHits.Count; k++)
				{
					RaycastHit raycastHit2 = _allHits[k];
					if (!_wheelController.vehicleColliders.Contains(raycastHit2.collider))
					{
						Vector3 vector2 = raycastHit2.point - origin;
						Vector3 vector3 = _transform.InverseTransformVector(vector2);
						float num4 = width * 0.5f;
						if (!(vector3.x < 0f - num4) && !(vector3.x > num4) && !(vector3.z > radius) && !(vector3.z < 0f - radius) && raycastHit2.distance < num3)
						{
							num3 = raycastHit2.distance;
							raycastHit = raycastHit2;
						}
					}
				}
				if (num3 != 3.4028235E+38f)
				{
					hit = raycastHit;
					return true;
				}
			}
			return false;
		}
	}
}
