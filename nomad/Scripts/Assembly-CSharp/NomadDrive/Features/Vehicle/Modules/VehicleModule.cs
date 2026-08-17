using EvilCore.Extensions;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public abstract class VehicleModule : MonoBehaviour
	{
		protected VehicleManager VehicleManager { get; private set; }

		protected VehicleEventBus EventBus => VehicleManager.EventBus;

		protected virtual void Awake()
		{
			base.gameObject.InjectGameObject();
			VehicleManager = GetComponentInParent<VehicleManager>();
		}

		protected virtual void OnEnable()
		{
			VehicleManager.RegisterModule(this);
			SubscribeEvents();
		}

		protected virtual void OnDisable()
		{
			UnsubscribeEvents();
			VehicleManager.UnregisterModule(this);
		}

		protected abstract void SubscribeEvents();

		protected abstract void UnsubscribeEvents();

		public virtual void OnFrontSeatsTaken()
		{
		}

		public virtual void OnFrontSeatsVacated()
		{
		}
	}
}
