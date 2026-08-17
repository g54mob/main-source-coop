using UnityEngine;

namespace CW.Common
{
	[ExecuteInEditMode]
	[HelpURL("https://carloswilkes.com/Documentation/Common#CwFollow")]
	[AddComponentMenu("Common/CW Follow")]
	public class CwFollow : MonoBehaviour
	{
		public enum FollowType
		{
			TargetTransform = 0,
			MainCamera = 1
		}

		public enum UpdateType
		{
			Update = 0,
			LateUpdate = 1
		}

		[SerializeField]
		private FollowType follow;

		[SerializeField]
		private Transform target;

		[SerializeField]
		private float damping = -1f;

		[SerializeField]
		private bool rotate = true;

		[SerializeField]
		private bool ignoreZ;

		[SerializeField]
		private UpdateType followIn = UpdateType.LateUpdate;

		[SerializeField]
		private Vector3 localPosition;

		[SerializeField]
		private Vector3 localRotation;

		public FollowType Follow
		{
			get
			{
				return follow;
			}
			set
			{
				follow = value;
			}
		}

		public Transform Target
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

		public float Damping
		{
			get
			{
				return damping;
			}
			set
			{
				damping = value;
			}
		}

		public bool Rotate
		{
			get
			{
				return rotate;
			}
			set
			{
				rotate = value;
			}
		}

		public bool IgnoreZ
		{
			get
			{
				return ignoreZ;
			}
			set
			{
				ignoreZ = value;
			}
		}

		public UpdateType FollowIn
		{
			get
			{
				return followIn;
			}
			set
			{
				followIn = value;
			}
		}

		public Vector3 LocalPosition
		{
			get
			{
				return localPosition;
			}
			set
			{
				localPosition = value;
			}
		}

		public Vector3 LocalRotation
		{
			get
			{
				return localRotation;
			}
			set
			{
				localRotation = value;
			}
		}

		[ContextMenu("UpdatePosition")]
		public void UpdatePosition()
		{
			Transform transform = target;
			if (follow == FollowType.MainCamera)
			{
				Camera main = Camera.main;
				if (main != null)
				{
					transform = main.transform;
				}
			}
			if (transform != null)
			{
				Vector3 position = base.transform.position;
				Vector3 b = transform.TransformPoint(localPosition);
				float t = CwHelper.DampenFactor(damping, Time.deltaTime);
				if (ignoreZ)
				{
					b.z = position.z;
				}
				base.transform.position = Vector3.Lerp(position, b, t);
				if (rotate)
				{
					Quaternion b2 = transform.rotation * Quaternion.Euler(localRotation);
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b2, t);
				}
			}
		}

		protected virtual void Update()
		{
			if (followIn == UpdateType.Update)
			{
				UpdatePosition();
			}
		}

		protected virtual void LateUpdate()
		{
			if (followIn == UpdateType.LateUpdate)
			{
				UpdatePosition();
			}
		}
	}
}
