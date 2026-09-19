using System.Collections;
using UnityEngine;

namespace Obi.Samples
{
	[RequireComponent(typeof(Rigidbody))]
	public class TangledPeg : MonoBehaviour
	{
		public TangledPegSlot currentSlot;

		public Collider floorCollider;

		public ObiRope attachedRope;

		[Header("Movement")]
		public float stiffness = 200f;

		public float damping = 20f;

		public float maxAccel = 50f;

		public float minDistance = 0.05f;

		public Rigidbody rb { get; private set; }

		public ObiRigidbody orb { get; private set; }

		private void Awake()
		{
			rb = GetComponent<Rigidbody>();
			orb = GetComponent<ObiRigidbody>();
			Physics.IgnoreCollision(GetComponent<Collider>(), floorCollider);
			if (currentSlot != null)
			{
				currentSlot.currentPeg = this;
				base.transform.position = currentSlot.transform.position;
			}
		}

		public float MoveTowards(Vector3 position)
		{
			Vector3 vector = position - base.transform.position;
			float result = Vector3.Magnitude(vector);
			Vector3 vector2 = stiffness * vector - damping * rb.linearVelocity;
			vector2 = Vector3.ClampMagnitude(vector2, maxAccel);
			rb.AddForce(vector2, ForceMode.Acceleration);
			return result;
		}

		public void DockInSlot(TangledPegSlot slot)
		{
			StopAllCoroutines();
			StartCoroutine(MoveTowardsSlot(slot));
		}

		public void UndockFromCurrentSlot()
		{
			if (currentSlot != null)
			{
				currentSlot.currentPeg = null;
				rb.isKinematic = false;
			}
		}

		private IEnumerator MoveTowardsSlot(TangledPegSlot slot)
		{
			float distance = float.MaxValue;
			orb.kinematicForParticles = true;
			while (distance > minDistance)
			{
				distance = MoveTowards(slot.transform.position);
				yield return 0;
			}
			currentSlot = slot;
			currentSlot.currentPeg = this;
			base.transform.position = currentSlot.transform.position;
			rb.isKinematic = true;
			orb.kinematicForParticles = false;
		}
	}
}
