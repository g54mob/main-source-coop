using UnityEngine;

namespace Obi
{
	[ExecuteInEditMode]
	public abstract class ObiRigidbodyBase : MonoBehaviour
	{
		public bool kinematicForParticles;

		protected ObiRigidbodyHandle rigidbodyHandle;

		public ObiRigidbodyHandle Handle
		{
			get
			{
				if (rigidbodyHandle == null)
				{
					ObiColliderWorld instance = ObiColliderWorld.GetInstance();
					rigidbodyHandle = instance.CreateRigidbody();
					rigidbodyHandle.owner = this;
				}
				return rigidbodyHandle;
			}
		}

		protected virtual void OnEnable()
		{
			rigidbodyHandle = ObiColliderWorld.GetInstance().CreateRigidbody();
			rigidbodyHandle.owner = this;
		}

		public void OnDisable()
		{
			ObiColliderWorld.GetInstance().DestroyRigidbody(rigidbodyHandle);
		}

		public abstract void UpdateIfNeeded(float stepTime);

		public abstract void UpdateVelocities(Vector3 linearDelta, Vector3 angularDelta);
	}
}
