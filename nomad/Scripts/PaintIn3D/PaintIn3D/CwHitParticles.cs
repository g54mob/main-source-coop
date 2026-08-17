using System;
using System.Collections.Generic;
using CW.Common;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	[RequireComponent(typeof(ParticleSystem))]
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwHitParticles")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Hit Particles")]
	public class CwHitParticles : MonoBehaviour
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

		public enum NormalType
		{
			ParticleVelocity = 0,
			CollisionNormal = 1
		}

		public enum PressureType
		{
			Constant = 0,
			Distance = 1,
			Speed = 2
		}

		[SerializeField]
		private EmitType emit;

		[SerializeField]
		private float raycastDistance = 0.0001f;

		[SerializeField]
		private LayerMask layers = -1;

		[SerializeField]
		private OrientationType orientation;

		[SerializeField]
		private Camera _camera;

		[SerializeField]
		private NormalType normal;

		[SerializeField]
		private float offset;

		[SerializeField]
		private int skip;

		[SerializeField]
		private bool preview;

		[SerializeField]
		private int priority;

		[SerializeField]
		private PressureType pressureMode;

		[SerializeField]
		private float pressureMin;

		[SerializeField]
		private float pressureMax;

		[SerializeField]
		[Range(0f, 1f)]
		private float pressureConstant = 1f;

		[SerializeField]
		private float pressureMultiplier = 1f;

		[SerializeField]
		private GameObject root;

		[NonSerialized]
		private ParticleSystem cachedParticleSystem;

		[NonSerialized]
		private bool cachedParticleSystemSet;

		[NonSerialized]
		private static List<ParticleCollisionEvent> particleCollisionEvents = new List<ParticleCollisionEvent>();

		[NonSerialized]
		private CwHitCache hitCache = new CwHitCache();

		[NonSerialized]
		private int skipCounter;

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

		public NormalType Normal
		{
			get
			{
				return normal;
			}
			set
			{
				normal = value;
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

		public int Skip
		{
			get
			{
				return skip;
			}
			set
			{
				skip = value;
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

		private bool TryGetRaycastHit(ParticleCollisionEvent collision, ref RaycastHit hit)
		{
			if (raycastDistance > 0f)
			{
				Collider collider = collision.colliderComponent as Collider;
				if (collider != null)
				{
					Ray ray = new Ray(collision.intersection + collision.normal * raycastDistance, -collision.normal);
					if (collider.Raycast(ray, out hit, raycastDistance * 2f))
					{
						return true;
					}
				}
			}
			return false;
		}

		protected virtual void OnParticleCollision(GameObject hitGameObject)
		{
			if (!cachedParticleSystemSet)
			{
				cachedParticleSystem = GetComponent<ParticleSystem>();
				cachedParticleSystemSet = true;
			}
			int safeCollisionEventSize = cachedParticleSystem.GetSafeCollisionEventSize();
			for (int i = particleCollisionEvents.Count; i < safeCollisionEventSize; i++)
			{
				particleCollisionEvents.Add(default(ParticleCollisionEvent));
			}
			safeCollisionEventSize = cachedParticleSystem.GetCollisionEvents(hitGameObject, particleCollisionEvents);
			Vector3 upwards = ((orientation == OrientationType.CameraUp) ? PaintCore.CwCommon.GetCameraUp(_camera) : Vector3.up);
			GameObject gameObject = ((root != null) ? root : base.gameObject);
			for (int j = 0; j < safeCollisionEventSize; j++)
			{
				ParticleCollisionEvent collision = particleCollisionEvents[j];
				if (!CwHelper.IndexInMask(collision.colliderComponent.gameObject.layer, layers))
				{
					continue;
				}
				if (skip > 0)
				{
					if (skipCounter++ <= skip)
					{
						continue;
					}
					skipCounter = 0;
				}
				Vector3 position = collision.intersection + collision.normal * offset;
				Vector3 vector = ((normal == NormalType.CollisionNormal) ? collision.normal : (-collision.velocity));
				Quaternion rotation = ((vector != Vector3.zero) ? Quaternion.LookRotation(-vector, upwards) : Quaternion.identity);
				float num = pressureMultiplier;
				if (cachedParticleSystem.collision.mode == ParticleSystemCollisionMode.Collision2D)
				{
					rotation = Quaternion.LookRotation(Vector3.forward, -vector);
				}
				switch (pressureMode)
				{
				case PressureType.Constant:
					num *= pressureConstant;
					break;
				case PressureType.Distance:
				{
					float value = Vector3.Distance(base.transform.position, collision.intersection);
					num *= Mathf.InverseLerp(pressureMin, pressureMax, value);
					break;
				}
				case PressureType.Speed:
				{
					float num2 = Vector3.SqrMagnitude(collision.velocity);
					if (num2 > 0f)
					{
						num2 = Mathf.Sqrt(num2);
					}
					num *= Mathf.InverseLerp(pressureMin, pressureMax, num2);
					break;
				}
				}
				switch (emit)
				{
				case EmitType.PointsIn3D:
					hitCache.InvokePoint(gameObject, preview, priority, num, position, rotation);
					break;
				case EmitType.PointsOnUV:
				{
					RaycastHit hit2 = default(RaycastHit);
					if (TryGetRaycastHit(collision, ref hit2))
					{
						hitCache.InvokeCoord(gameObject, preview, priority, num, new CwHit(hit2), rotation);
					}
					break;
				}
				case EmitType.TrianglesIn3D:
				{
					RaycastHit hit = default(RaycastHit);
					if (TryGetRaycastHit(collision, ref hit))
					{
						hitCache.InvokeTriangle(base.gameObject, preview, priority, num, new CwHit(hit), rotation);
					}
					break;
				}
				}
			}
		}
	}
}
