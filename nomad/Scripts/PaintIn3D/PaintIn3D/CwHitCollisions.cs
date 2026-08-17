using System;
using CW.Common;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwHitCollisions")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Hit Collisions")]
	public class CwHitCollisions : MonoBehaviour
	{
		public enum EmitType
		{
			PointsIn3D = 0,
			PointsOnUV = 20,
			TrianglesIn3D = 30
		}

		public enum OrientationType
		{
			WorldUp = 0,
			CameraUp = 1
		}

		public enum PressureType
		{
			Constant = 0,
			ImpactSpeed = 1
		}

		[SerializeField]
		private EmitType emit;

		[SerializeField]
		private float raycastDistance = 0.0001f;

		[SerializeField]
		private LayerMask layers = -1;

		[SerializeField]
		private bool onlyUseFirstContact = true;

		[SerializeField]
		private float delay;

		[SerializeField]
		private OrientationType orientation;

		[SerializeField]
		private Camera _camera;

		[SerializeField]
		private bool preview;

		[SerializeField]
		private float threshold = 50f;

		[SerializeField]
		private PressureType pressureMode = PressureType.ImpactSpeed;

		[SerializeField]
		private float pressureMin = 50f;

		[SerializeField]
		private float pressureMax = 100f;

		[SerializeField]
		[Range(0f, 1f)]
		private float pressureConstant = 1f;

		[SerializeField]
		private float pressureMultiplier = 1f;

		[SerializeField]
		private float offset;

		[SerializeField]
		private int priority;

		[SerializeField]
		private GameObject root;

		[SerializeField]
		private float cooldown;

		[NonSerialized]
		private CwHitCache hitCache = new CwHitCache();

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

		public float RaycastDistance
		{
			get
			{
				return raycastDistance;
			}
			set
			{
				raycastDistance = value;
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

		public bool OnlyUseFirstContact
		{
			get
			{
				return onlyUseFirstContact;
			}
			set
			{
				onlyUseFirstContact = value;
			}
		}

		public float Delay
		{
			get
			{
				return delay;
			}
			set
			{
				delay = value;
			}
		}

		public OrientationType Orientation
		{
			get
			{
				return orientation;
			}
			set
			{
				orientation = value;
			}
		}

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

		public bool Preview
		{
			get
			{
				return preview;
			}
			set
			{
				preview = value;
			}
		}

		public float Threshold
		{
			get
			{
				return threshold;
			}
			set
			{
				threshold = value;
			}
		}

		public PressureType PressureMode
		{
			get
			{
				return pressureMode;
			}
			set
			{
				pressureMode = value;
			}
		}

		public float PressureMin
		{
			get
			{
				return pressureMin;
			}
			set
			{
				pressureMin = value;
			}
		}

		public float PressureMax
		{
			get
			{
				return pressureMax;
			}
			set
			{
				pressureMax = value;
			}
		}

		public float PressureConstant
		{
			get
			{
				return pressureConstant;
			}
			set
			{
				pressureConstant = value;
			}
		}

		public float PressureMultiplier
		{
			get
			{
				return pressureMultiplier;
			}
			set
			{
				pressureMultiplier = value;
			}
		}

		public float Offset
		{
			get
			{
				return offset;
			}
			set
			{
				offset = value;
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

		public GameObject Root
		{
			get
			{
				return root;
			}
			set
			{
				ClearHitCache();
				root = value;
			}
		}

		public CwHitCache HitCache => hitCache;

		[ContextMenu("Clear Hit Cache")]
		public void ClearHitCache()
		{
			hitCache.Clear();
		}

		protected virtual void OnCollisionEnter(Collision collision)
		{
			CheckCollision(collision);
		}

		protected virtual void OnCollisionStay(Collision collision)
		{
			CheckCollision(collision);
		}

		protected virtual void Update()
		{
			cooldown -= Time.deltaTime;
		}

		private bool TryGetRaycastHit(ContactPoint contact, ref RaycastHit hit)
		{
			if (raycastDistance > 0f)
			{
				Ray ray = new Ray(contact.point + contact.normal * raycastDistance, -contact.normal);
				if (contact.otherCollider.Raycast(ray, out hit, raycastDistance * 2f))
				{
					return true;
				}
			}
			return false;
		}

		private void CheckCollision(Collision collision)
		{
			if (cooldown > 0f)
			{
				return;
			}
			float num = collision.impulse.magnitude / Time.fixedDeltaTime;
			if (!(num >= pressureMin))
			{
				return;
			}
			cooldown = delay;
			Vector3 upwards = ((orientation == OrientationType.CameraUp) ? PaintCore.CwCommon.GetCameraUp(_camera) : Vector3.up);
			ContactPoint[] contacts = collision.contacts;
			float num2 = pressureMultiplier;
			GameObject gameObject = ((root != null) ? root : base.gameObject);
			switch (pressureMode)
			{
			case PressureType.Constant:
				num2 *= pressureConstant;
				break;
			case PressureType.ImpactSpeed:
				num2 *= Mathf.InverseLerp(pressureMin, pressureMax, num);
				break;
			}
			for (int num3 = contacts.Length - 1; num3 >= 0; num3--)
			{
				ContactPoint contact = contacts[num3];
				if (CwHelper.IndexInMask(contact.otherCollider.gameObject.layer, layers))
				{
					Vector3 position = contact.point + contact.normal * offset;
					Quaternion rotation = Quaternion.LookRotation(-contact.normal, upwards);
					switch (emit)
					{
					case EmitType.PointsIn3D:
						hitCache.InvokePoint(gameObject, preview, priority, num2, position, rotation);
						break;
					case EmitType.PointsOnUV:
					{
						RaycastHit hit2 = default(RaycastHit);
						if (TryGetRaycastHit(contact, ref hit2))
						{
							hitCache.InvokeCoord(gameObject, preview, priority, num2, new CwHit(hit2), rotation);
						}
						break;
					}
					case EmitType.TrianglesIn3D:
					{
						RaycastHit hit = default(RaycastHit);
						if (TryGetRaycastHit(contact, ref hit))
						{
							hitCache.InvokeTriangle(base.gameObject, preview, priority, num2, new CwHit(hit), rotation);
						}
						break;
					}
					}
					if (onlyUseFirstContact)
					{
						break;
					}
				}
			}
		}
	}
}
