using UnityEngine;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwRotate")]
	[AddComponentMenu("CW/Paint in 3D/CW Rotate")]
	public class CwRotate : MonoBehaviour
	{
		[SerializeField]
		private Space space = Space.Self;

		[SerializeField]
		private Vector3 perSecond;

		public Space Space
		{
			get
			{
				return space;
			}
			set
			{
				space = value;
			}
		}

		public Vector3 PerSecond
		{
			get
			{
				return perSecond;
			}
			set
			{
				perSecond = value;
			}
		}

		protected virtual void Update()
		{
			base.transform.Rotate(perSecond * Time.deltaTime, space);
		}
	}
}
