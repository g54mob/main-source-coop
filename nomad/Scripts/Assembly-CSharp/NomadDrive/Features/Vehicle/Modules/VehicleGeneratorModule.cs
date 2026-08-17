using EvilCore.EvilPack.EvilLogger;
using NomadDrive.Features.Vehicle.Parts.Generator;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleGeneratorModule : VehicleModule
	{
		private const string GENERATOR = "Generator";

		[SerializeField]
		private GeneratorSlot _generatorSlotRef;

		[SerializeField]
		public bool IsGeneratorInstalled;

		[SerializeField]
		private Generator _installedGenerator;

		[SerializeField]
		private float _generatorMaxPower;

		[SerializeField]
		private float _generatorFuelConsumptionRate;

		[SerializeField]
		private float _generatorFuelCapacity;

		public Generator InstalledGenerator => _installedGenerator;

		protected override void SubscribeEvents()
		{
			_generatorSlotRef.OnGeneratorInstalled.AddListener(OnGeneratorInstalled);
			_generatorSlotRef.OnGeneratorRemoved.AddListener(OnGeneratorRemoved);
		}

		protected override void UnsubscribeEvents()
		{
			_generatorSlotRef.OnGeneratorInstalled.RemoveListener(OnGeneratorInstalled);
			_generatorSlotRef.OnGeneratorRemoved.RemoveListener(OnGeneratorRemoved);
		}

		private void SetGeneratorProperties(Generator generator)
		{
			if (generator == null)
			{
				_installedGenerator = null;
				IsGeneratorInstalled = false;
				_generatorMaxPower = 0f;
				_generatorFuelConsumptionRate = 0f;
				_generatorFuelCapacity = 0f;
			}
			else if (generator.generatorConfig == null)
			{
				EvilLogger.LogError("Generator config is null", "SetGeneratorProperties", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Modules\\VehicleGeneratorModule.cs", 47);
			}
			else
			{
				_installedGenerator = generator;
				IsGeneratorInstalled = true;
				_generatorMaxPower = generator.generatorConfig.maxPower;
				_generatorFuelConsumptionRate = generator.generatorConfig.fuelConsumption;
				_generatorFuelCapacity = generator.generatorConfig.fuelCapacity;
			}
		}

		private void OnGeneratorInstalled(Generator generator)
		{
			SetGeneratorProperties(generator);
			base.VehicleManager.VehiclePhysiscsManager.SetMassAffector(50f, _generatorSlotRef.gameObject);
			base.VehicleManager.VehiclePhysiscsManager.ImpactForceAtPosition(Vector3.down, generator.transform.position, 150f);
		}

		private void OnGeneratorRemoved()
		{
			base.VehicleManager.VehiclePhysiscsManager.SetMassAffector(0f, _generatorSlotRef.gameObject);
			base.VehicleManager.VehiclePhysiscsManager.ImpactForceAtPosition(Vector3.down, _installedGenerator.transform.position, 150f);
			SetGeneratorProperties(null);
		}
	}
}
