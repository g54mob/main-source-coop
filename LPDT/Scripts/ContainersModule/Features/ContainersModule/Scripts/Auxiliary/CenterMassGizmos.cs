using UnityEngine;

namespace Features.ContainersModule.Scripts.Auxiliary
{
	public class CenterMassGizmos : MonoBehaviour
	{
		[SerializeField]
		private float _radius = 0.1f;

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(GetComponent<Rigidbody>().worldCenterOfMass, _radius);
		}
	}
}
