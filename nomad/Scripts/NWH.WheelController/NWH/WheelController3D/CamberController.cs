using UnityEngine;

namespace NWH.WheelController3D
{
	[RequireComponent(typeof(WheelController))]
	public class CamberController : MonoBehaviour
	{
		[Tooltip("Curve representing spring compression on X-axis and \r\ncamber angle on Y-axis.")]
		public AnimationCurve camberCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f));

		private WheelController _wc;

		private void Awake()
		{
			_wc = GetComponent<WheelController>();
		}

		private void FixedUpdate()
		{
			_wc.Camber = camberCurve.Evaluate(_wc.SpringCompression);
		}
	}
}
