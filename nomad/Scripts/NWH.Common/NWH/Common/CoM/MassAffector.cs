using UnityEngine;

namespace NWH.Common.CoM
{
	public class MassAffector : MonoBehaviour, IMassAffector
	{
		public float mass = 100f;

		public float GetMass()
		{
			return mass;
		}

		public Transform GetTransform()
		{
			return base.transform;
		}

		public Vector3 GetWorldCenterOfMass()
		{
			return base.transform.position;
		}
	}
}
