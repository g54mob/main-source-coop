using Features.GrabModule.Scripts;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

[NetworkBehaviourWeaved(0)]
public class RigidBodySmallGrabable : NetworkBehaviour, IGrabableBase
{
	[SerializeField]
	private NetworkRigidbody _networkRigidbody3D;

	[SerializeField]
	private Rigidbody _rigidbody;

	[SerializeField]
	private Collider _collider;

	[SerializeField]
	private VisualController _visualController;

	private SpringJoint _joint;

	public Rigidbody ConnectedRigidbody;

	public bool IsEnableToGrab => ConnectedRigidbody == null;

	public GrabableType GrabableType => GrabableType.SmallRigidBody;

	public GameObject GameObject => base.gameObject;

	public void Join(Rigidbody handRigidbody)
	{
		base.Object.RequestStateAuthority();
		if (!(ConnectedRigidbody != null))
		{
			ConnectedRigidbody = handRigidbody;
			_networkRigidbody3D.InterpolationTarget = base.transform;
			_rigidbody.freezeRotation = true;
			_collider.isTrigger = true;
			_collider.enabled = false;
			_visualController.SetTargetPositionForInterpolation(handRigidbody.GetComponent<SmallObjectPositioner>().GetFreePosition());
		}
	}

	public void Unjoin(Rigidbody handRigidbody)
	{
		if (!(ConnectedRigidbody != handRigidbody))
		{
			_networkRigidbody3D.Teleport(_visualController.VisualsRoot.position);
			handRigidbody.GetComponent<SmallObjectPositioner>().ReturnPosition(_visualController.TargetInterpolationPosition);
			_visualController.SetTargetPositionForInterpolation(null);
			ConnectedRigidbody = null;
			_rigidbody.freezeRotation = false;
			_networkRigidbody3D.InterpolationTarget = _visualController.VisualsRoot;
			_rigidbody.isKinematic = false;
			_collider.isTrigger = false;
			_collider.enabled = true;
			handRigidbody.GetComponent<GrabController>().NotifyUnjoin(this);
		}
	}

	public void UnjoinAll()
	{
		if (!(ConnectedRigidbody == null))
		{
			_rigidbody.isKinematic = true;
			_networkRigidbody3D.Teleport(_visualController.VisualsRoot.position);
			ConnectedRigidbody.GetComponent<SmallObjectPositioner>().ReturnPosition(_visualController.TargetInterpolationPosition);
			_visualController.SetTargetPositionForInterpolation(null);
			ConnectedRigidbody.GetComponent<GrabController>().NotifyUnjoin(this);
			ConnectedRigidbody = null;
			_rigidbody.freezeRotation = false;
			_networkRigidbody3D.InterpolationTarget = _visualController.VisualsRoot;
			_rigidbody.isKinematic = false;
			_collider.isTrigger = false;
			_collider.enabled = true;
		}
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
