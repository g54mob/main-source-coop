using System;
using UnityEngine;

namespace PaintIn3D
{
	[ExecuteInEditMode]
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwSpiral")]
	[AddComponentMenu("CW/Paint in 3D/CW Spiral")]
	public class CwSpiral : MonoBehaviour
	{
		[SerializeField]
		private Vector3 position;

		[SerializeField]
		private Vector3 rotation;

		[SerializeField]
		private float radius = 10f;

		[SerializeField]
		private float radiusAngle;

		[SerializeField]
		private float radiusSpeed = 5f;

		[SerializeField]
		private float offset = 1f;

		[SerializeField]
		private float offsetAngle;

		[SerializeField]
		private float offsetSpeed = 1f;

		public Vector3 Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		public Vector3 Rotation
		{
			get
			{
				return rotation;
			}
			set
			{
				rotation = value;
			}
		}

		public float Radius
		{
			get
			{
				return radius;
			}
			set
			{
				radius = value;
			}
		}

		public float RadiusAngle
		{
			get
			{
				return radiusAngle;
			}
			set
			{
				radiusAngle = value;
			}
		}

		public float RadiusSpeed
		{
			get
			{
				return radiusSpeed;
			}
			set
			{
				radiusSpeed = value;
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

		public float OffsetAngle
		{
			get
			{
				return offsetAngle;
			}
			set
			{
				offsetAngle = value;
			}
		}

		public float OffsetSpeed
		{
			get
			{
				return offsetSpeed;
			}
			set
			{
				offsetSpeed = value;
			}
		}

		protected virtual void Update()
		{
			if (Application.isPlaying)
			{
				radiusAngle += radiusSpeed * Time.deltaTime;
				offsetAngle += offsetSpeed * Time.deltaTime;
			}
			float num = Mathf.Sin(offsetAngle * ((float)Math.PI / 180f)) * offset;
			float x = Mathf.Sin(radiusAngle * ((float)Math.PI / 180f)) * (radius + num);
			float z = Mathf.Cos(radiusAngle * ((float)Math.PI / 180f)) * (radius + num);
			Matrix4x4 matrix4x = Matrix4x4.TRS(position, Quaternion.Euler(rotation), Vector3.one);
			base.transform.localPosition = matrix4x.MultiplyPoint(new Vector3(x, 0f, z));
		}
	}
}
