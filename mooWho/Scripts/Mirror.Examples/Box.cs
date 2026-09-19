using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkTransformReliable))]
[DisallowMultipleComponent]
public class Box : NetworkBehaviour
{
	[Header("Components")]
	public Rigidbody rigidBody;

	protected override void OnValidate()
	{
		if (!Application.isPlaying)
		{
			base.OnValidate();
			Reset();
		}
	}

	private void Reset()
	{
		rigidBody = GetComponent<Rigidbody>();
		rigidBody.isKinematic = true;
	}

	public override void OnStartServer()
	{
		rigidBody.isKinematic = false;
	}

	public override void OnStopServer()
	{
		rigidBody.isKinematic = true;
	}

	public override void OnStartClient()
	{
	}

	public override void OnStopClient()
	{
	}

	public override void OnStartLocalPlayer()
	{
	}

	public override void OnStopLocalPlayer()
	{
	}

	public override void OnStartAuthority()
	{
	}

	public override void OnStopAuthority()
	{
	}

	public override bool Weaved()
	{
		return true;
	}
}
