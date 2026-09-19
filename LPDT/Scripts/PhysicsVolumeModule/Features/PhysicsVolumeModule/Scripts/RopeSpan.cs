using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	[ExecuteAlways]
	public class RopeSpan : MonoBehaviour
	{
		[Tooltip("One end of the cable — e.g. the hanging weight's tie point.")]
		[SerializeField]
		private Transform _from;

		[Tooltip("The other end — e.g. the pulley the cable runs over.")]
		[SerializeField]
		private Transform _to;

		[Tooltip("Cable diameter in world units.")]
		[SerializeField]
		private float _thickness = 0.1f;

		private void LateUpdate()
		{
			if (!(_from == null) && !(_to == null))
			{
				Vector3 position = _from.position;
				Vector3 position2 = _to.position;
				Vector3 vector = position2 - position;
				float magnitude = vector.magnitude;
				base.transform.position = (position + position2) * 0.5f;
				if (magnitude > Mathf.Epsilon)
				{
					base.transform.rotation = Quaternion.FromToRotation(Vector3.up, vector / magnitude);
				}
				base.transform.localScale = new Vector3(_thickness, magnitude * 0.5f, _thickness);
			}
		}
	}
}
