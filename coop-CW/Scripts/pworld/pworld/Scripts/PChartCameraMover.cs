using UnityEngine;
using pworld.Scripts.Extensions;
using pworld.Scripts.PPhys;

namespace pworld.Scripts
{
	public class PChartCameraMover : MonoBehaviour
	{
		[SerializeReference]
		public ITimeSource timeSource = new DefaultTime();

		public float spring;

		public float realsedDamp = 1f;

		public float damp;

		public Vector2 Velocity;

		public Vector2 target;

		public bool isDragging;

		public Vector2 current;

		public Vector2 touchDown;

		public float dragSense = 1f;

		public bool moveCam;

		public float maxStepSize = 0.016f;

		[SerializeField]
		private float perspectiveDragSense;

		private readonly KeyCode dragButton = KeyCode.Mouse2;

		private bool applicationHadFocusLF;

		private Vector2 lastMousePos;

		public Vector2 Target
		{
			get
			{
				return target;
			}
			set
			{
				target = value;
			}
		}

		public Vector2 Current
		{
			get
			{
				return current;
			}
			set
			{
				current = value;
			}
		}

		private Camera Camera { get; set; }

		private Vector2 MousePos
		{
			get
			{
				Vector3 mousePosition = Input.mousePosition;
				mousePosition.z = Camera.nearClipPlane;
				return Camera.ScreenToWorldPoint(mousePosition).PToVec2XZ0();
			}
		}

		private Vector2 MouseMoveDelta => MousePos - lastMousePos;

		public float DragSense
		{
			get
			{
				if (Camera.orthographic)
				{
					return dragSense;
				}
				return perspectiveDragSense;
			}
		}

		private void Awake()
		{
			Camera = GetComponent<Camera>();
		}

		private void Start()
		{
			if (moveCam)
			{
				Current = base.transform.position.PToVec2XZ0();
			}
		}

		private void Update()
		{
			float deltaTime = timeSource.DeltaTime;
			if (Input.GetKeyDown(dragButton))
			{
				Target = Current;
				isDragging = true;
			}
			if (Input.GetKey(dragButton) && applicationHadFocusLF)
			{
				Target -= MouseMoveDelta * DragSense;
			}
			if (Input.GetKeyUp(dragButton))
			{
				isDragging = false;
			}
			for (float num = deltaTime / Mathf.Max(maxStepSize, 0.005f); num > 0f; num -= 1f)
			{
				if (num > 1f)
				{
					PhysStep(maxStepSize);
				}
				else
				{
					PhysStep(deltaTime);
				}
			}
			if (moveCam)
			{
				Align();
			}
			applicationHadFocusLF = Application.isFocused;
			lastMousePos = MousePos;
		}

		public void Align()
		{
			base.transform.position = new Vector3(current.x, base.transform.position.y, current.y);
		}

		public void GoTo(Vector2 positionxz)
		{
			Target = positionxz;
			Current = positionxz;
			Velocity = 0.ToVec2();
		}

		private void PhysStep(float _dt)
		{
			if (isDragging)
			{
				Velocity = FRILerp.PLerp(Velocity, (Target - Current) * spring, damp, _dt);
				Current += Velocity * _dt;
			}
			if (!isDragging)
			{
				Velocity = FRILerp.PLerp(Velocity, Vector2.zero, realsedDamp, _dt);
				Current += Velocity * _dt;
			}
		}
	}
}
