using UnityEngine;

namespace CW.Common
{
	[HelpURL("https://carloswilkes.com/Documentation/Common#CwRotate")]
	[AddComponentMenu("Common/CW Rotate")]
	public class CwRotate : MonoBehaviour
	{
		[SerializeField]
		private Vector3 angularVelocity = Vector3.up;

		[SerializeField]
		private Space relativeTo;

		public Vector3 AngularVelocity
		{
			get
			{
				return angularVelocity;
			}
			set
			{
				angularVelocity = value;
			}
		}

		public Space RelativeTo
		{
			get
			{
				return relativeTo;
			}
			set
			{
				relativeTo = value;
			}
		}

		protected virtual void Update()
		{
			base.transform.Rotate(angularVelocity * Time.deltaTime, relativeTo);
		}
	}
}
