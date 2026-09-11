using System;
using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class PPhysRot : MonoBehaviour
	{
		public enum SPACE
		{
			World = 0,
			Parent = 1,
			Start = 2
		}

		public float rotSpring = 15f;

		public float rotDamp = 25f;

		public float fakeMass = 1f;

		public bool lockX;

		public bool lockY;

		public bool lockZ;

		public Vector3 angularVel;

		public Transform TarTrans;

		public SPACE space;

		private Vector3 localStartDir;

		private Vector3 localStartUp;

		public Vector3 TarPos
		{
			get
			{
				if ((bool)TarTrans)
				{
					return TarTrans.position;
				}
				return base.transform.parent.TransformPoint(localStartDir);
			}
		}

		public Vector3 TarUp => space switch
		{
			SPACE.World => Vector3.up, 
			SPACE.Parent => base.transform.parent.up, 
			SPACE.Start => base.transform.parent.TransformDirection(localStartUp), 
			_ => throw new Exception("fix it"), 
		};

		public void Awake()
		{
			localStartDir = base.transform.InverseTransformPoint(base.transform.position + base.transform.forward);
			localStartUp = base.transform.parent.InverseTransformDirection(base.transform.up);
		}

		private void Start()
		{
		}

		private void Update()
		{
			float num = Mathf.Clamp(Time.unscaledDeltaTime, 0f, 0.02f);
			Vector3 direction = TarPos - base.transform.position;
			Vector3 tarUp = TarUp;
			direction = ConstrainVector(direction, base.transform.parent);
			Vector3 vector = Vector3.Cross(base.transform.up, tarUp).normalized * Vector3.Angle(base.transform.up, tarUp);
			Vector3 vector2 = Vector3.Cross(base.transform.forward, direction).normalized * Vector3.Angle(base.transform.forward, direction);
			angularVel = FRILerp.Lerp(angularVel, (vector2 + vector) * rotSpring, rotDamp, useTimeScale: false);
			angularVel /= fakeMass;
			base.transform.Rotate(angularVel * num, Space.World);
		}

		public void AddForce(Vector3 force)
		{
			angularVel += force;
		}

		private Vector3 ConstrainVector(Vector3 direction, Transform baseTransform)
		{
			if (lockY)
			{
				direction = Vector3.ProjectOnPlane(direction, baseTransform.up);
			}
			if (lockX)
			{
				direction = Vector3.ProjectOnPlane(direction, baseTransform.right);
			}
			if (lockZ)
			{
				direction = Vector3.ProjectOnPlane(direction, baseTransform.forward);
			}
			return direction;
		}
	}
}
