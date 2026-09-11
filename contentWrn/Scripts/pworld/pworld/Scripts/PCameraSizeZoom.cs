using UnityEngine;
using pworld.Scripts.Extensions;
using pworld.Scripts.PPhys;

namespace pworld.Scripts
{
	public class PCameraSizeZoom : MonoBehaviour
	{
		public float velocity;

		public float target;

		public float current;

		public float spring;

		public float damp;

		public float maxStepSize = 0.008f;

		public float stepSize = 0.05f;

		public Vector2 minMaxSize = new Vector2(10f, 40f);

		public Vector2 minMaxFOV = new Vector2(10f, 40f);

		public float mouseDiffMullInPerspective;

		private Camera cam;

		private PChartCameraMover mover;

		[SerializeReference]
		public ITimeSource timeSouce = new DefaultTime();

		public Vector2 MinMaxZoom
		{
			get
			{
				if (cam.orthographic)
				{
					return minMaxSize;
				}
				return minMaxFOV;
			}
		}

		public float Zoom
		{
			get
			{
				if (cam.orthographic)
				{
					return cam.orthographicSize;
				}
				return cam.fieldOfView;
			}
			set
			{
				if (cam.orthographic)
				{
					cam.orthographicSize = value;
				}
				else
				{
					cam.fieldOfView = value;
				}
			}
		}

		public Vector3 MousePosInFrustum
		{
			get
			{
				Vector3 mousePosition = Input.mousePosition;
				mousePosition.z = cam.nearClipPlane;
				return cam.ScreenToWorldPoint(mousePosition);
			}
		}

		public void Awake()
		{
			mover = GetComponent<PChartCameraMover>();
			cam = GetComponent<Camera>();
			current = ExtMath.InverseEerp(minMaxSize.x, minMaxSize.y, Zoom);
			target = current;
		}

		public void Update()
		{
			float deltaTime = timeSouce.DeltaTime;
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				target -= stepSize;
			}
			else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				target += stepSize;
			}
			target = Mathf.Clamp01(target);
			PPhysSpringBase.LagControll(PhysStep, deltaTime, maxStepSize);
			Vector3 mousePosInFrustum = MousePosInFrustum;
			Zoom = ExtMath.Eerp(MinMaxZoom.x, MinMaxZoom.y, current);
			Vector3 mousePosInFrustum2 = MousePosInFrustum;
			Vector3 me = mousePosInFrustum - mousePosInFrustum2;
			me *= (cam.orthographic ? 1f : mouseDiffMullInPerspective);
			mover.current += me.PToVec2XZ0();
			mover.Align();
		}

		private void PhysStep(float dt)
		{
			velocity = FRILerp.PLerp(velocity, (target - current) * spring, damp, dt);
			current += velocity * dt;
		}
	}
}
