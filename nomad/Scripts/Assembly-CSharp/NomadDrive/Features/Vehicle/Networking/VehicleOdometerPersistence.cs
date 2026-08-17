using EvilCore.EvilSave;
using Mirror;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Networking
{
	public class VehicleOdometerPersistence : MonoBehaviour
	{
		private const string OdometerKey = "vehicle.main.odometer_km";

		private VehicleManager _vehicleManager;

		private IEvilSaveManager _saveManager;

		private void Awake()
		{
			_vehicleManager = GetComponentInParent<VehicleManager>();
		}

		private void OnEnable()
		{
			_saveManager = Object.FindAnyObjectByType<EvilSaveManager>();
			if (_saveManager != null)
			{
				_saveManager.OnBeforeSave += HandleBeforeSave;
				_saveManager.OnAfterLoad += HandleAfterLoad;
			}
		}

		private void OnDisable()
		{
			if (_saveManager != null)
			{
				_saveManager.OnBeforeSave -= HandleBeforeSave;
				_saveManager.OnAfterLoad -= HandleAfterLoad;
			}
		}

		private void Start()
		{
			SeedFromSave();
		}

		private void HandleAfterLoad()
		{
			SeedFromSave();
		}

		private void SeedFromSave()
		{
			if (NetworkServer.active)
			{
				VehicleNetworkSync vehicleNetworkSync = ((_vehicleManager != null) ? _vehicleManager.NetworkSync : null);
				if (!(vehicleNetworkSync == null))
				{
					int km = EvilSave.Load("vehicle.main.odometer_km", 0);
					vehicleNetworkSync.ServerSetOdometerKm(km);
				}
			}
		}

		private void HandleBeforeSave()
		{
			if (NetworkServer.active)
			{
				VehicleNetworkSync vehicleNetworkSync = ((_vehicleManager != null) ? _vehicleManager.NetworkSync : null);
				if (!(vehicleNetworkSync == null))
				{
					EvilSave.Save("vehicle.main.odometer_km", vehicleNetworkSync.OdometerKm);
				}
			}
		}
	}
}
