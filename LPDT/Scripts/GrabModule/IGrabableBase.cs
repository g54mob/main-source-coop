using UnityEngine;

public interface IGrabableBase
{
	GameObject GameObject { get; }

	bool IsEnableToGrab { get; }

	GrabableType GrabableType { get; }

	void Join(Rigidbody handRigidbody);

	void Unjoin(Rigidbody handRigidbody);

	void UnjoinAll();
}
