using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	[ExecuteAlways]
	public class LiftRope : MonoBehaviour
	{
		[SerializeField]
		private Lift _lift;

		[Tooltip("The moving end of the cable. Its other end is pinned to the shaft top.")]
		[SerializeField]
		private Transform _anchor;

		[Tooltip("Offset of the pinned (top) end from the shaft top, relative to the lift — moves the top attach point (e.g. a counterweight beam) beside the lift. The point stays world-fixed as the deck travels.")]
		[SerializeField]
		private Vector3 _beamOffset = Vector3.zero;

		[Tooltip("Cable diameter in world units.")]
		[SerializeField]
		private float _thickness = 0.1f;

		private void LateUpdate()
		{
			if (!(_lift == null) && !(_anchor == null))
			{
				Vector3 position = _anchor.position;
				Vector3 vector = _lift.TopPoint + _beamOffset;
				Vector3 vector2 = vector - position;
				float magnitude = vector2.magnitude;
				base.transform.position = (position + vector) * 0.5f;
				if (magnitude > Mathf.Epsilon)
				{
					base.transform.rotation = Quaternion.FromToRotation(Vector3.up, vector2 / magnitude);
				}
				base.transform.localScale = new Vector3(_thickness, magnitude * 0.5f, _thickness);
			}
		}
	}
}
