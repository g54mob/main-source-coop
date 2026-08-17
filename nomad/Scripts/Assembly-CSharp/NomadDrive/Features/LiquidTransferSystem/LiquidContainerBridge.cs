using NomadDrive.Features.Interaction;
using NomadDrive.Features.LiquidTransferSystem.UI;
using VContainer;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public class LiquidContainerBridge : Interactable
	{
		public ILiquidContainer RoutedLiquidContainer;

		[Inject]
		private LiquidContainerInfoPanel _liquidContainerInfoPanel;

		public void AssignTargetLiquidContainer(ILiquidContainer targetLiquidContainer)
		{
			RoutedLiquidContainer = targetLiquidContainer;
		}

		public void RemoveTargetLiquidContainer()
		{
			RoutedLiquidContainer = null;
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			if (RoutedLiquidContainer != null)
			{
				_liquidContainerInfoPanel.OnLiquidContainerHovered(RoutedLiquidContainer);
			}
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			_liquidContainerInfoPanel.OnLiquidContainerHovered(null);
		}

		protected void HideTransferUI()
		{
			_liquidContainerInfoPanel?.OnLiquidContainerHovered(null);
		}

		protected void ShowTransferUI()
		{
			if (RoutedLiquidContainer != null)
			{
				_liquidContainerInfoPanel?.OnLiquidContainerHovered(RoutedLiquidContainer);
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
