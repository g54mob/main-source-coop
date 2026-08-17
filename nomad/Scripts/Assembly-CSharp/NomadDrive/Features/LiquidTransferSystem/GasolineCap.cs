using NomadDrive.Features.Vehicle;
using NomadDrive.Features.Vehicle.Interactables;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public class GasolineCap : VehicleLiquidCap
	{
		protected override void Awake()
		{
			base.Awake();
			ILiquidContainer component = GetComponent<ILiquidContainer>();
			AssignTargetLiquidContainer(component);
		}

		protected override void Start()
		{
			base.Start();
			if (base.NetworkSync != null && base.NetworkSync.isServer)
			{
				RoutedLiquidContainer.CmdSetLiquidType(LiquidType.Gasoline);
				if (!(RoutedLiquidContainer is LiquidContainerComponent { RandomizeInitialFill: not false }))
				{
					RoutedLiquidContainer.CmdSetAmount(0f);
				}
			}
		}

		public void SetVehicleManager(VehicleManager vm)
		{
			vm.FuelModuleWrapper.module.amount = RoutedLiquidContainer.CurrentAmount;
			vm.FuelModuleWrapper.module.capacity = RoutedLiquidContainer.Capacity;
		}
	}
}
