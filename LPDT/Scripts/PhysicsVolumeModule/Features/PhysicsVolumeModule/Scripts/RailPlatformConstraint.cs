using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	[RequireComponent(typeof(Rigidbody))]
	public class RailPlatformConstraint : MonoBehaviour
	{
		[SerializeField]
		private Transform _railEndA;

		[SerializeField]
		private Transform _railEndB;

		[SerializeField]
		private float _fallbackTravelLength = 8f;

		private void Start()
		{
			Rigidbody component = GetComponent<Rigidbody>();
			Vector3 vector;
			Vector3 vector2;
			if (_railEndA != null && _railEndB != null)
			{
				vector = _railEndA.position;
				vector2 = _railEndB.position;
			}
			else
			{
				Vector3 vector3 = base.transform.forward * (_fallbackTravelLength * 0.5f);
				vector = base.transform.position - vector3;
				vector2 = base.transform.position + vector3;
			}
			Vector3 vector4 = vector2 - vector;
			if (vector4.sqrMagnitude < 0.01f)
			{
				Debug.LogError("[RailPlatformConstraint] rail ends coincide on '" + base.name + "' — no path to confine to", this);
				return;
			}
			ConfigurableJoint configurableJoint = base.gameObject.AddComponent<ConfigurableJoint>();
			configurableJoint.autoConfigureConnectedAnchor = false;
			configurableJoint.connectedBody = null;
			configurableJoint.anchor = Vector3.zero;
			configurableJoint.connectedAnchor = (vector + vector2) * 0.5f;
			configurableJoint.axis = base.transform.InverseTransformDirection(vector4.normalized);
			configurableJoint.secondaryAxis = base.transform.InverseTransformDirection(Vector3.up);
			configurableJoint.xMotion = ConfigurableJointMotion.Limited;
			configurableJoint.yMotion = ConfigurableJointMotion.Locked;
			configurableJoint.zMotion = ConfigurableJointMotion.Locked;
			configurableJoint.angularXMotion = ConfigurableJointMotion.Locked;
			configurableJoint.angularYMotion = ConfigurableJointMotion.Locked;
			configurableJoint.angularZMotion = ConfigurableJointMotion.Locked;
			configurableJoint.linearLimit = new SoftJointLimit
			{
				limit = vector4.magnitude * 0.5f
			};
			component.constraints = RigidbodyConstraints.FreezeRotation;
		}
	}
}
