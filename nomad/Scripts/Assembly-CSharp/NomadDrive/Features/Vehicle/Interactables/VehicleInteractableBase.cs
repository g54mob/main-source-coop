using NomadDrive.Features.Interaction;
using NomadDrive.Features.Vehicle.Networking;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Interactables
{
	public abstract class VehicleInteractableBase : StaticInteractable
	{
		public VehicleManager VehicleManager { get; private set; }

		protected VehicleNetworkSync NetworkSync { get; private set; }

		protected virtual bool HasNetworkState => false;

		public override bool ParticipatesInColliderLod => false;

		protected override void Awake()
		{
			VehicleManager = GetComponentInParent<VehicleManager>();
			NetworkSync = GetComponentInParent<VehicleNetworkSync>();
			base.Awake();
			SetInteractionAvailability(newValue: true);
			SetupChildColliderRouting();
		}

		protected override void OnDestroy()
		{
		}

		private void SetupChildColliderRouting()
		{
			int layer = LayerMask.NameToLayer("Interactable");
			Collider[] rigidColliders = GetRigidColliders();
			foreach (Collider collider in rigidColliders)
			{
				if (!(collider.gameObject == base.gameObject))
				{
					collider.gameObject.layer = layer;
					CleanupStaleRouters(collider.gameObject);
					AddRouterIfMissing(collider.gameObject);
				}
			}
			rigidColliders = GetTriggerColliders();
			foreach (Collider collider2 in rigidColliders)
			{
				if (!(collider2.gameObject == base.gameObject))
				{
					collider2.gameObject.layer = layer;
					CleanupStaleRouters(collider2.gameObject);
					AddRouterIfMissing(collider2.gameObject);
				}
			}
			CleanupStaleRouters(base.gameObject);
		}

		private void CleanupStaleRouters(GameObject target)
		{
			if (target.TryGetComponent<InteractableRouter>(out var component))
			{
				Object.Destroy(component);
			}
		}

		private void AddRouterIfMissing(GameObject target)
		{
			if (!target.TryGetComponent<StaticInteractableRouter>(out var _))
			{
				target.AddComponent<StaticInteractableRouter>().SetRouteTarget(this);
			}
		}

		public new void SetInteractionAvailability(bool newValue)
		{
			base.SetInteractionAvailability(newValue);
			int layer = LayerMask.NameToLayer(newValue ? "Interactable" : "Default");
			Collider[] rigidColliders = GetRigidColliders();
			Collider[] array;
			if (rigidColliders != null)
			{
				array = rigidColliders;
				foreach (Collider collider in array)
				{
					if (!(collider == null) && !(collider.gameObject == base.gameObject))
					{
						collider.gameObject.layer = layer;
					}
				}
			}
			Collider[] triggerColliders = GetTriggerColliders();
			if (triggerColliders == null)
			{
				return;
			}
			array = triggerColliders;
			foreach (Collider collider2 in array)
			{
				if (!(collider2 == null) && !(collider2.gameObject == base.gameObject))
				{
					collider2.gameObject.layer = layer;
				}
			}
		}

		public virtual void ApplyStateFromNetwork(byte stateData, bool skipAnimation)
		{
		}

		public override void ApplyStateFromManager(byte stateData, bool skipAnimation)
		{
			ApplyStateFromNetwork(stateData, skipAnimation);
		}

		public void SetTriggerCollidersEnabled(bool enabled)
		{
			Collider[] triggerColliders = GetTriggerColliders();
			if (triggerColliders == null)
			{
				return;
			}
			Collider[] array = triggerColliders;
			foreach (Collider collider in array)
			{
				if (!(collider == null))
				{
					collider.enabled = enabled;
				}
			}
		}

		public void SetRigidCollidersEnabled(bool enabled)
		{
			SetLogicalRigidCollidersEnabled(enabled);
		}

		public void SetRigidCollidersTriggered(bool isTrigger)
		{
			Collider[] rigidColliders = GetRigidColliders();
			if (rigidColliders == null)
			{
				return;
			}
			Collider[] array = rigidColliders;
			foreach (Collider collider in array)
			{
				if (!(collider == null))
				{
					collider.isTrigger = isTrigger;
				}
			}
		}

		public virtual void RefreshInteractionState()
		{
			SetInteractionAvailability(base.AvailableForInteraction);
			if (!ParticipatesInColliderLod)
			{
				SetLodColliderActive(active: true);
			}
			if (base.AvailableForInteraction)
			{
				SetTriggerCollidersEnabled(enabled: true);
			}
			UpdateState();
		}
	}
}
