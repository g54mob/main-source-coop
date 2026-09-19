using UnityEngine;

namespace Features.ContainersModule.Scripts.Auxiliary
{
	public class HingeJointAnchorGizmos : MonoBehaviour
	{
		[SerializeField]
		private float _radius = 0.1f;

		private void OnDrawGizmos()
		{
			HingeJoint component = GetComponent<HingeJoint>();
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(base.transform.position + component.anchor, _radius);
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(base.transform.position + component.connectedAnchor, _radius);
		}
	}
}
