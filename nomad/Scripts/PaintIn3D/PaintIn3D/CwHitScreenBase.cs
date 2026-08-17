using System;
using CW.Common;
using PaintCore;
using UnityEngine;
using UnityEngine.Serialization;

namespace PaintIn3D
{
	public abstract class CwHitScreenBase : CwHitPointers
	{
		public enum RotationType
		{
			Normal = 0,
			World = 1,
			ThisRotation = 2,
			ThisLocalRotation = 3,
			CustomRotation = 4,
			CustomLocalRotation = 5
		}

		public enum RelativeType
		{
			WorldUp = 0,
			CameraUp = 1,
			DrawAngle = 2
		}

		public enum DirectionType
		{
			HitNormal = 0,
			RayDirection = 1,
			CameraDirection = 2
		}

		public enum EmitType
		{
			PointsIn3D = 0,
			PointsOnUV = 20,
			TrianglesIn3D = 30
		}

		[Flags]
		public enum ButtonTypes
		{
			LeftMouse = 1,
			RightMouse = 2,
			MiddleMouse = 4,
			Touch = 0x20
		}

		[SerializeField]
		private Camera _camera;

		[SerializeField]
		private LayerMask layers = -5;

		[FormerlySerializedAs("draw")]
		[SerializeField]
		private EmitType emit;

		[SerializeField]
		private RotationType rotateTo;

		[FormerlySerializedAs("normal")]
		[SerializeField]
		private DirectionType normalDirection = DirectionType.CameraDirection;

		[FormerlySerializedAs("orientation")]
		[SerializeField]
		private RelativeType normalRelativeTo = RelativeType.CameraUp;

		[SerializeField]
		private Transform customTransform;

		[SerializeField]
		protected bool storeStates = true;

		[SerializeField]
		private int priority;

		[SerializeField]
		private float normalOffset;

		[SerializeField]
		private ButtonTypes requiredButtons;

		[SerializeField]
		private KeyCode requiredKey;

		[SerializeField]
		private float mouseOffset;

		[SerializeField]
		private float touchOffset;

		[SerializeField]
		private bool showPreview;

		public Camera Camera
		{
			get
			{
				return _camera;
			}
			set
			{
				_camera = value;
			}
		}

		public LayerMask Layers
		{
			get
			{
				return layers;
			}
			set
			{
				layers = value;
			}
		}

		public EmitType Emit
		{
			get
			{
				return emit;
			}
			set
			{
				emit = value;
			}
		}

		public RotationType RotateTo
		{
			get
			{
				return rotateTo;
			}
			set
			{
				rotateTo = value;
			}
		}

		public DirectionType NormalDirection
		{
			get
			{
				return normalDirection;
			}
			set
			{
				normalDirection = value;
			}
		}

		public RelativeType NormalRelativeTo
		{
			get
			{
				return normalRelativeTo;
			}
			set
			{
				normalRelativeTo = value;
			}
		}

		public Transform CustomTransform
		{
			get
			{
				return customTransform;
			}
			set
			{
				customTransform = value;
			}
		}

		public bool StoreStates
		{
			get
			{
				return storeStates;
			}
			set
			{
				storeStates = value;
			}
		}

		public int Priority
		{
			get
			{
				return priority;
			}
			set
			{
				priority = value;
			}
		}

		public float NormalOffset
		{
			get
			{
				return normalOffset;
			}
			set
			{
				normalOffset = value;
			}
		}

		public bool NeedsDrawAngle
		{
			get
			{
				if (rotateTo == RotationType.Normal)
				{
					return normalRelativeTo == RelativeType.DrawAngle;
				}
				return false;
			}
		}

		public bool ShouldUpgradePointers()
		{
			if ((requiredButtons & ButtonTypes.LeftMouse) == 0 && (requiredButtons & ButtonTypes.RightMouse) == 0 && (requiredButtons & ButtonTypes.MiddleMouse) == 0)
			{
				return (requiredButtons & ButtonTypes.Touch) != 0;
			}
			return true;
		}

		public void TryUpgradePointers()
		{
			bool flag = (requiredButtons & ButtonTypes.LeftMouse) != 0;
			bool flag2 = (requiredButtons & ButtonTypes.RightMouse) != 0;
			bool flag3 = (requiredButtons & ButtonTypes.MiddleMouse) != 0;
			if (flag || flag2 || flag3)
			{
				requiredButtons &= ~ButtonTypes.LeftMouse;
				requiredButtons &= ~ButtonTypes.RightMouse;
				requiredButtons &= ~ButtonTypes.MiddleMouse;
				CwPointerMouse cwPointerMouse = base.gameObject.AddComponent<CwPointerMouse>();
				cwPointerMouse.Preview = showPreview;
				if (flag)
				{
					cwPointerMouse.TryAddKey(KeyCode.Mouse0);
				}
				if (flag2)
				{
					cwPointerMouse.TryAddKey(KeyCode.Mouse1);
				}
				if (flag3)
				{
					cwPointerMouse.TryAddKey(KeyCode.Mouse2);
				}
			}
			if ((requiredButtons & ButtonTypes.Touch) != 0)
			{
				requiredButtons &= ~ButtonTypes.Touch;
				base.gameObject.AddComponent<CwPointerTouch>().Offset = touchOffset;
			}
		}

		protected virtual void DoQuery(Vector2 screenPosition, ref Camera camera, ref Ray ray, ref CwHit hit3D, ref RaycastHit2D hit2D)
		{
			RaycastHit hitInfo = default(RaycastHit);
			camera = CwHelper.GetCamera(_camera);
			ray = camera.ScreenPointToRay(screenPosition);
			hit2D = Physics2D.GetRayIntersection(ray, 1f / 0f, layers);
			Physics.Raycast(ray, out hitInfo, 1f / 0f, layers);
			hit3D = new CwHit(hitInfo);
		}

		protected void PaintAt(CwPointConnector connector, CwHitCache hitCache, Vector2 screenPosition, Vector2 screenPositionOld, bool preview, float pressure, object owner)
		{
			Camera camera = null;
			Ray ray = default(Ray);
			RaycastHit2D hit2D = default(RaycastHit2D);
			CwHit hit3D = default(CwHit);
			Vector3 finalPosition = default(Vector3);
			Quaternion finalRotation = default(Quaternion);
			DoQuery(screenPosition, ref camera, ref ray, ref hit3D, ref hit2D);
			bool flag = hit2D.distance > 0f;
			if (hit3D.Distance > 0f && (!flag || hit3D.Distance < hit2D.distance))
			{
				CalcHitData(hit3D.Position, hit3D.Normal, ray, camera, screenPositionOld, ref finalPosition, ref finalRotation);
				if (emit == EmitType.PointsIn3D)
				{
					if (connector != null)
					{
						connector.SubmitPoint(base.gameObject, preview, priority, pressure, finalPosition, finalRotation, owner);
					}
					else
					{
						hitCache.InvokePoint(base.gameObject, preview, priority, pressure, finalPosition, finalRotation);
					}
					return;
				}
				if (emit == EmitType.PointsOnUV)
				{
					hitCache.InvokeCoord(base.gameObject, preview, priority, pressure, hit3D, finalRotation);
					return;
				}
				if (emit == EmitType.TrianglesIn3D)
				{
					hitCache.InvokeTriangle(base.gameObject, preview, priority, pressure, hit3D, finalRotation);
					return;
				}
			}
			else if (flag)
			{
				CalcHitData(hit2D.point, new Vector3(0f, 0f, -1f), ray, camera, screenPositionOld, ref finalPosition, ref finalRotation);
				if (emit == EmitType.PointsIn3D)
				{
					if (connector != null)
					{
						connector.SubmitPoint(base.gameObject, preview, priority, pressure, finalPosition, finalRotation, owner);
					}
					else
					{
						hitCache.InvokePoint(base.gameObject, preview, priority, pressure, finalPosition, finalRotation);
					}
					return;
				}
			}
			connector?.BreakHits(owner);
		}

		private void CalcHitData(Vector3 hitPoint, Vector3 hitNormal, Ray ray, Camera camera, Vector2 screenPositionOld, ref Vector3 finalPosition, ref Quaternion finalRotation)
		{
			finalPosition = hitPoint + hitNormal * normalOffset;
			finalRotation = Quaternion.identity;
			switch (rotateTo)
			{
			case RotationType.Normal:
			{
				Vector3 vector = default(Vector3);
				switch (normalDirection)
				{
				case DirectionType.HitNormal:
					vector = hitNormal;
					break;
				case DirectionType.RayDirection:
					vector = -ray.direction;
					break;
				case DirectionType.CameraDirection:
					vector = -camera.transform.forward;
					break;
				}
				Vector3 upwards = Vector3.up;
				switch (normalRelativeTo)
				{
				case RelativeType.CameraUp:
					upwards = camera.transform.up;
					break;
				case RelativeType.DrawAngle:
				{
					Ray ray2 = camera.ScreenPointToRay(screenPositionOld);
					upwards = ((!camera.orthographic) ? Vector3.Cross(ray2.direction, ray.direction) : Vector3.Cross(ray2.GetPoint(1f) - ray.origin, ray.GetPoint(1f) - ray.origin));
					break;
				}
				}
				finalRotation = Quaternion.LookRotation(-vector, upwards);
				break;
			}
			case RotationType.World:
				finalRotation = Quaternion.identity;
				break;
			case RotationType.ThisRotation:
				finalRotation = base.transform.rotation;
				break;
			case RotationType.ThisLocalRotation:
				finalRotation = base.transform.localRotation;
				break;
			case RotationType.CustomRotation:
				if (customTransform != null)
				{
					finalRotation = customTransform.rotation;
				}
				break;
			case RotationType.CustomLocalRotation:
				if (customTransform != null)
				{
					finalRotation = customTransform.localRotation;
				}
				break;
			}
		}
	}
}
