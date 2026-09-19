using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

[NetworkBehaviourWeaved(0)]
public class RigidBodyBigGrabable : NetworkBehaviour, IGrabableBase
{
	[SerializeField]
	private NetworkRigidbody _networkRigidbody3D;

	[SerializeField]
	private Rigidbody _rigidbody;

	private List<FixedJoint> _springJoints = new List<FixedJoint>();

	private Dictionary<Rigidbody, FixedJoint> _handToJoint = new Dictionary<Rigidbody, FixedJoint>();

	public GameObject GameObject => base.gameObject;

	public bool IsEnableToGrab => !_springJoints.Any();

	public GrabableType GrabableType => GrabableType.BigRigidBody;

	public void Join(Rigidbody handRigidbody)
	{
		base.Object.RequestStateAuthority();
		if (!_handToJoint.Any() && !_handToJoint.ContainsKey(handRigidbody))
		{
			FixedJoint fixedJoint = base.gameObject.AddComponent<FixedJoint>();
			fixedJoint.connectedBody = handRigidbody;
			fixedJoint.autoConfigureConnectedAnchor = false;
			fixedJoint.connectedAnchor = handRigidbody.transform.InverseTransformPoint(_rigidbody.position);
			fixedJoint.enableCollision = false;
			fixedJoint.enablePreprocessing = true;
			_springJoints.Add(fixedJoint);
			_handToJoint[handRigidbody] = fixedJoint;
		}
	}

	public void Unjoin(Rigidbody handRigidbody)
	{
		Debug.LogError("Ungrabbed");
		if (_handToJoint.TryGetValue(handRigidbody, out var value))
		{
			_springJoints.Remove(value);
			_handToJoint.Remove(handRigidbody);
			UnityEngine.Object.Destroy(value);
			handRigidbody.GetComponent<GrabController>().NotifyUnjoin(this);
		}
	}

	public void UnjoinAll()
	{
		GrabController grabController = null;
		foreach (KeyValuePair<Rigidbody, FixedJoint> item in _handToJoint)
		{
			UnityEngine.Object.Destroy(item.Key);
			grabController = item.Value.GetComponent<GrabController>();
		}
		_springJoints.Clear();
		_handToJoint.Clear();
		grabController?.NotifyUnjoin(this);
	}

	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool P_0)
	{
	}

	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
	}
}
