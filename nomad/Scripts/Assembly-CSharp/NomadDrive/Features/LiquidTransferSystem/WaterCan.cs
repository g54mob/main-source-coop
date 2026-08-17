using NomadDrive.Features.LiquidDrinking;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public class WaterCan : DrinkableHeldItem
	{
		protected override void OnEnable()
		{
			base.OnEnable();
			base.LiquidContainer.OnLiquidAmountChangedEvent.AddListener(OnLiquidAmountChanged);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.LiquidContainer.OnLiquidAmountChangedEvent.RemoveListener(OnLiquidAmountChanged);
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			base.LiquidContainer.CmdSetLiquidType(LiquidType.Water);
			base.LiquidContainer.ServerApplyRandomizedFill();
		}

		private void OnLiquidAmountChanged(float oldAmount, float newAmount)
		{
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
